//--------------------------文档信息----------------------------
//       
//                 文件名: WorkQueue                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.Utility
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/8/19 17:44:39
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


namespace NkjSoft.Utility
{

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.ComponentModel;

    /// <summary>
    /// 
    /// </summary>
    public delegate void AnonymousHandler();

    /// <summary>
    /// 
    /// </summary>
    public class WorkQueue : WorkQueue<AnonymousHandler>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkQueue"/> class.
        /// </summary>
        public WorkQueue() : this(16, -1) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkQueue"/> class.
        /// </summary>
        /// <param name="thread">The thread.</param>
        public WorkQueue(int thread)
            : this(thread, -1)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkQueue"/> class.
        /// </summary>
        /// <param name="thread">The thread.</param>
        /// <param name="capacity">The capacity.</param>
        public WorkQueue(int thread, int capacity)
        {
            base.Thread = thread;
            base.Capacity = capacity;
            base.Process += delegate(AnonymousHandler ah)
            {
                ah();
            };
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WorkQueue<T> : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public delegate void WorkQueueProcessHandler(T item);
        /// <summary>
        /// Occurs when [process].
        /// </summary>
        public event WorkQueueProcessHandler Process;

        private int _thread = 16;
        private int _capacity = -1;
        private int _work_index = 0;
        private Dictionary<int, WorkInfo> _works = new Dictionary<int, WorkInfo>();
        private object _works_lock = new object();
        private Queue<T> _queue = new Queue<T>();
        private object _queue_lock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkQueue&lt;T&gt;"/> class.
        /// </summary>
        public WorkQueue() : this(16, -1) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkQueue&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="thread">The thread.</param>
        public WorkQueue(int thread)
            : this(thread, -1)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkQueue&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="thread">The thread.</param>
        /// <param name="capacity">The capacity.</param>
        public WorkQueue(int thread, int capacity)
        {
            _thread = thread;
            _capacity = capacity;
        }

        /// <summary>
        /// Enqueues the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Enqueue(T item)
        {
            lock (_queue_lock)
            {
                if (_capacity > 0 && _queue.Count >= _capacity) return;
                _queue.Enqueue(item);
            }
            lock (_works_lock)
            {
                foreach (WorkInfo w in _works.Values)
                {
                    if (w.IsWaiting)
                    {
                        w.Set();
                        return;
                    }
                }
            }
            if (_works.Count < _thread)
            {
                if (_queue.Count > 0)
                {
                    int index = 0;
                    lock (_works_lock)
                    {
                        index = _work_index++;
                        _works.Add(index, new WorkInfo());
                    }
                    new Thread(delegate()
                    {
                        WorkInfo work = _works[index];
                        while (true)
                        {
                            List<T> de = new List<T>();
                            if (_queue.Count > 0)
                            {
                                lock (_queue_lock)
                                {
                                    if (_queue.Count > 0)
                                    {
                                        de.Add(_queue.Dequeue());
                                    }
                                }
                            }

                            if (de.Count > 0)
                            {
                                try
                                {
                                    this.OnProcess(de[0]);
                                }
                                catch
                                {
                                }
                            }

                            if (_queue.Count == 0)
                            {
                                work.WaitOne(TimeSpan.FromSeconds(20));

                                if (_queue.Count == 0)
                                {
                                    break;
                                }
                            }
                        }
                        lock (_works_lock)
                        {
                            _works.Remove(index);
                        }
                        work.Dispose();
                    }).Start();
                }
            }
        }

        /// <summary>
        /// Called when [process].
        /// </summary>
        /// <param name="item">The item.</param>
        protected virtual void OnProcess(T item)
        {
            if (Process != null)
            {
                Process(item);
            }
        }

        #region IDisposable 成员

        public void Dispose()
        {
            lock (_queue_lock)
            {
                _queue.Clear();
            }
            lock (_works_lock)
            {
                foreach (WorkInfo w in _works.Values)
                {
                    w.Dispose();
                }
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets the thread.
        /// </summary>
        /// <value>The thread.</value>
        public int Thread
        {
            get { return _thread; }
            set
            {
                if (_thread != value)
                {
                    _thread = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the capacity.
        /// </summary>
        /// <value>The capacity.</value>
        public int Capacity
        {
            get { return _capacity; }
            set
            {
                if (_capacity != value)
                {
                    _capacity = value;
                }
            }
        }

        /// <summary>
        /// Gets the used thread.
        /// </summary>
        /// <value>The used thread.</value>
        public int UsedThread
        {
            get { return _works.Count; }
        }
        /// <summary>
        /// Gets the queue.
        /// </summary>
        /// <value>The queue.</value>
        public int Queue
        {
            get { return _queue.Count; }
        }

        /// <summary>
        /// Gets the statistics.
        /// </summary>
        /// <value>The statistics.</value>
        public string Statistics
        {
            get
            {
                string value = string.Format(@"线程：{0}/{1}
队列：{2}

", _works.Count, _thread, _queue.Count);
                int[] keys = new int[_works.Count];
                try
                {
                    _works.Keys.CopyTo(keys, 0);
                }
                catch
                {
                    lock (_works_lock)
                    {
                        keys = new int[_works.Count];
                        _works.Keys.CopyTo(keys, 0);
                    }
                }
                foreach (int k in keys)
                {
                    WorkInfo w = null;
                    if (_works.TryGetValue(k, out w))
                    {
                        value += string.Format(@"线程{0}：{1}
", k, w.IsWaiting);
                    }
                }
                return value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        class WorkInfo : IDisposable
        {
            private ManualResetEvent _reset = new ManualResetEvent(false);
            private bool _isWaiting = false;

            public void WaitOne(TimeSpan timeout)
            {
                try
                {
                    _reset.Reset();
                    _isWaiting = true;
                    _reset.WaitOne(timeout, false);
                }
                catch { }
            }
            public void Set()
            {
                try
                {
                    _isWaiting = false;
                    _reset.Set();
                }
                catch { }
            }

            public bool IsWaiting
            {
                get { return _isWaiting; }
            }

            #region IDisposable 成员

            public void Dispose()
            {
                this.Set();
                _reset.Close();
            }

            #endregion
        }
    }

}