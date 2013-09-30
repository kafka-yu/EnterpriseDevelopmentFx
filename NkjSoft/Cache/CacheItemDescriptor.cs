
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;



namespace NkjSoft.Cache
{
    /// <summary>
    /// Descriptor class to describe/display the
    /// contents of an item in the cache.
    /// </summary>
    public class CacheItemDescriptor
    {
        private string _key;
        private string _type;
        private string _serializedData = string.Empty;


        /// <summary>
        /// Initialize the read-only properties.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        public CacheItemDescriptor(string key, string type)
            : this(key, type, string.Empty)
        {
        }


        /// <summary>
        /// Initialize the read-only properties.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        public CacheItemDescriptor(string key, string type, string serializedData)
        {
            _key = key;
            _type = type;
            _serializedData = serializedData;
        }


        /// <summary>
        /// Key
        /// </summary>
        public string Key
        {
            get { return _key; }
        }


        /// <summary>
        /// CacheItemType
        /// </summary>
        public string ItemType
        {
            get { return _type; }
        }


        /// <summary>
        /// Get the serialzied data.
        /// </summary>
        public string Data
        {
            get { return _serializedData; }
        }
    }

}
