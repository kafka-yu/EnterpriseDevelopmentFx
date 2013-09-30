//--------------------------文档信息----------------------------
//       
//                 文件名: BllClassBuilder                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.Tools.ModelBuilder
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/8/6 0:18:03
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NkjSoft.Extensions;
using NkjSoft.ORM;
using System.ComponentModel;
namespace NkjSoft.Tools.ModelBuilder
{
    /// <summary>
    /// NkjSoft 的 BLL 类生成器。
    /// </summary>
    public sealed class BllClassBuilder
    {
        #region --- 属性 ---
        /// <summary>
        /// 获取或设置需要生成的类名集合。
        /// </summary>
        [Description("获取或设置需要生成的类名集合。该集合可以从对应数据库的表列表中选择。")]
        [ReadOnly(true)]
        [Browsable(false)]
        [Category("实体")]
        [DisplayName("源")]
        public List<Table> Classes { get; set; }

        /// <summary>
        /// 获取或设置BLL层项目文件夹。不存在则会自动创建。
        /// </summary>
        [Description("获取或设置 BLL 层项目文件夹，该文件夹用于保存生成的各个BLL子类的文件。\r\n如果目录不存在，则会自动创建。如果不指定，则会在 Model 类文件保存路径下创建相应BLL目录。")]
        [Category("生成")]
        [DisplayName("输出路径")]
        [DefaultValue("<default>")]
        [NkjSoft.Validation.Validators.RequireValidator(KeyName = "BLL子类输出路径")]
        [Editor(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string BllLibPath
        {
            get { return _bllLibPath; }
            set
            {
                _bllLibPath = Templates.OnPropertyChanged(value);

                OnPropertyChanged(new GeneratorPropertyChangedEventArgs(Templates.BllClassSessionName, "BllLibPath", _bllLibPath));
            }
        }

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="NkjSoft.Tools.ModelBuilder.GeneratorPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnPropertyChanged(GeneratorPropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        private string _bllLibPath;

        /// <summary>
        /// 获取或设置Model层命名空间。
        /// </summary>
        [Description("获取或设置Model层命名空间。该命名空间用于在BLL内部引用对应的实体类。")]
        [Category("实体")]
        [DefaultValue("<default>")]
        [DisplayName("Model层命名空间")]
        [NkjSoft.Validation.Validators.RequireValidator(KeyName = "Bll 配置项  --> Model层命名空间")]
        public string ModelLibNameSpace
        {
            get { return _modelLibNameSpace; }
            set
            {
                _modelLibNameSpace = Templates.OnPropertyChanged(value);

                OnPropertyChanged(new GeneratorPropertyChangedEventArgs(Templates.BllClassSessionName, "ModelLibNameSpace", _modelLibNameSpace));
            }
        }
        private string _modelLibNameSpace;


        /// <summary>
        /// 获取或设置 <see cref="QueryContext"/> 对象的完全限定名。
        /// </summary>
        [Description("获取或设置 QueryContext 对象的完全限定名。该完全限定名用于引用具体的 QueryContext 类。")]
        [Category("实体")]
        [DisplayName("查询上下文")]
        [DefaultValue("<default>")]
        [NkjSoft.Validation.Validators.RequireValidator(KeyName = "Bll 配置项  --> 查询上下文名称")]
        public string QueryContextFullName
        {
            get { return _queryContextFullName; }
            set
            {
                _queryContextFullName = Templates.OnPropertyChanged(value);
                OnPropertyChanged(new GeneratorPropertyChangedEventArgs(Templates.BllClassSessionName, "QueryContextFullName", _queryContextFullName));

            }
        }
        private string _queryContextFullName;

        /// <summary>
        /// BLL 层命名空间..
        /// </summary>
        [Description("获取或设置BLL 层命名空间。")]
        [Category("BLL")]
        [DefaultValue("<default>")]
        [DisplayName("命名空间")]
        [NkjSoft.Validation.Validators.RequireValidator(KeyName = "Bll 配置项  --> BLL 层命名空间")]
        public string BllNameSpace
        {
            get { return _BllNameSpace; }
            set
            {
                _BllNameSpace = Templates.OnPropertyChanged(value);

                OnPropertyChanged(new GeneratorPropertyChangedEventArgs(Templates.BllClassSessionName, "BllNameSpace", _BllNameSpace));


            }
        }
        private string _BllNameSpace;

        /// <summary>
        /// BLL 基类名字。
        /// </summary>
        [Description("获取或设置 Bll 基类名字。默认引用数据库名字。")]
        [DefaultValue("<default>")]
        [Category("BLL")]
        [DisplayName("Bll 基类名")]
        [NkjSoft.Validation.Validators.RequireValidator(KeyName = "Bll 配置项  --> Bll 基类名")]
        public string BllBaseClassName
        {
            get { return _BllBaseClassName; }
            set
            {
                _BllBaseClassName = Templates.OnPropertyChanged(value);
            }
        }
        private string _BllBaseClassName;

        /// <summary>
        /// 各个类的类名格式化器,默认{0}.
        /// </summary>
        [Description("获取或设置各个类BLL子类的统一命名格式。默认{0}Manager，即以实体的名字+Manager后缀。")]
        [DefaultValue("{0}_Manager")]
        [Category("生成")]
        [DisplayName("子类命名格式")]
        [NkjSoft.Validation.Validators.RequireValidator(KeyName = "Bll 配置项  --> 子类命名格式")]
        public string ClassNameFormatter
        {
            get
            {
                return _classNameFormatter;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    _classNameFormatter = "{0}_Manager";
                else
                    _classNameFormatter = value;

                OnPropertyChanged(new GeneratorPropertyChangedEventArgs(Templates.BllClassSessionName, "ClassNameFormatter", _classNameFormatter));

            }
        }

        private string _classNameFormatter;


        /// <summary>
        /// 获取或设置bll 连接的数据库服务器。
        /// </summary>
        [Browsable(false)]
        public DataBaseServer Server { get; set; }


        /// <summary>
        /// 获取或设置 BllClass模版
        /// </summary>
        [Browsable(false)]
        [NkjSoft.Validation.Validators.RequireValidator(KeyName = "Bll 配置 ---> Bll类 模版")]
        public string BllClassTemplate { get; set; }
        /// <summary>
        /// 获取或设置BllBaseClassTemplate。
        /// </summary>
        [Browsable(false)]
        [NkjSoft.Validation.Validators.RequireValidator(KeyName = "Bll 配置 ---> Bll Base类 模版")]
        public string BllBaseClassTemplate { get; set; }
        #endregion

        #region --- 事件 ---

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event GeneratorPropertyChangedEventHandler PropertyChanged;
        #endregion

        #region --- Ctor ---
        /// <summary>
        /// Initializes a new instance of the <see cref="BllClassBuilder"/> class.
        /// </summary>
        public BllClassBuilder()
        {
            //ClassNameFormatter = Templates.DefaultValueTag;
            //this.BllBaseClassName = Templates.DefaultValueTag;
            //this.QueryContextFullName = Templates.DefaultValueTag;
            //this.ModelLibNameSpace = Templates.DefaultValueTag;
            // this.BllLibPath = Templates.DefaultValueTag;
            this.ClassNameFormatter = "{0}_Manager";
            // this.BllNameSpace = Templates.DefaultValueTag;

        }
        #endregion


        /// <summary>
        /// 在生成一个bll类之后发生。
        /// </summary>
        [Description("当生成一个bll类内容,并在类内容被成功写入到指定的文件后触发。")]
        [DisplayName("OnGenerated")]
        [Category("生成")]
        public event EventHandler<GeneratorEventArgs> Generated;

        private string bllBaseName = string.Empty;
        private readonly string bllNameSpaceTag = "$BllNameSpace$";
        private readonly string modelLibNameSpaceTag = "$ModelNameSpace$";
        private readonly string queryContextFullNameTag = "$QueryContextFullName$";
        private readonly string bllBaseNameTag = "$BllBaseName$";
        private readonly string classNameTag = "$ClassName$";
        private readonly string modelClassNameTag = "$ModelName$";


        private string MakeBllBase()
        {
            temp = new StringBuilder(BllBaseClassTemplate);
            /*   ..
            
             * 
             * 
             */
            temp.Replace(bllNameSpaceTag, BllNameSpace)
                   .Replace(modelLibNameSpaceTag, ModelLibNameSpace)
                   .Replace(queryContextFullNameTag, QueryContextFullName)
                   .Replace(bllBaseNameTag, BllBaseClassName);
            return temp.ToString();
        }

        StringBuilder temp = null;
        private string MakeSimpleBllClass(Table table)
        {
            temp = new StringBuilder(BllClassTemplate);

            temp.Replace(bllNameSpaceTag, BllNameSpace)
                .Replace(classNameTag, ClassNameFormatter.FormatWith(table.Name))
                .Replace(bllBaseNameTag, this.BllBaseClassName)
                .Replace(modelClassNameTag, table.Name)
                .Replace(modelLibNameSpaceTag, ModelLibNameSpace);

            return temp.ToString();
        }


        /// <summary>
        /// 开始生成。
        /// </summary>
        public void Generate()
        {
            //条件检查
            if (!Prepared())
            { return; }
            if (!Directory.Exists(BllLibPath))
                Directory.CreateDirectory(BllLibPath);
            // Build BllBase .cs 
            var bllBaseContent = MakeBllBase();
            //Bll基类
            WriteFile(1, "BLL基类", Path.Combine(BllLibPath, "{0}.cs".FormatWith(BllBaseClassName)), BllLibPath, bllBaseContent, true);

            //Build Each Bll Class
            Classes.ForEach((p, index) =>
            {
                WriteFile(index, "[BLL] " + p.Name, Path.Combine(BllLibPath, "{0}.cs".FormatWith(ClassNameFormatter.FormatWith(p))), BllLibPath, MakeSimpleBllClass(p), true);

            });
        }


        /// <summary>
        /// Prepareds this instance.
        /// </summary>
        /// <returns></returns>
        private bool Prepared()
        {
            if (BllLibPath.IsNullOrEmpty())
                throw new NullReferenceException("BllLibPath");
            if (QueryContextFullName.IsNullOrEmpty())
                throw new NullReferenceException("QueryContextFullName");
            if (Classes == null || Classes.Count == 0)
                throw new NullReferenceException("Classes");
            return true;
        }


        /// <summary>
        /// Writes the file.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="name">The name.</param>
        /// <param name="filepath">The filepath.</param>
        /// <param name="saveFolder">The save folder.</param>
        /// <param name="context">The context.</param>
        /// <param name="isok">if set to <c>true</c> [isok].</param>
        private void WriteFile(int status, string name, string filepath, string saveFolder, string context, bool isok)
        {
            File.WriteAllText(filepath, context);
            OnGenerated(new GeneratorEventArgs(status, name, saveFolder, isok, context, filepath));
        }

        /// <summary>
        /// Called when [generated].
        /// </summary>
        /// <param name="e">The e.</param>
        private void OnGenerated(GeneratorEventArgs e)
        {
            if (Generated != null)
            {
                Generated(this, e);
            }
        }

    }
}
