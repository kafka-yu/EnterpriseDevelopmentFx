using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NkjSoft.Framework.Utilities
{
    public class EFConfigurationGenerator
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelDllPath"></param>
        /// <param name="ns"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetModels(string modelDllPath, string ns)
        {
            var assm = Assembly.LoadFile(modelDllPath);

            if (assm == null)
                return null;

            try
            {
                
            var result = assm.GetTypes()
                   .Where(p => p != null && p.Namespace != null && p.Namespace.Equals(ns));

            return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static IEnumerable<ModelTypeInfo> GetModelsWithKey(string modelDllPath, string ns)
        {
            var types = GetModels(modelDllPath, ns);

            if (types == null)
                yield break;

            foreach (var item in types)
            {
                var prop = item.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.IsDefined(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), true))
                    .FirstOrDefault();

                yield return new ModelTypeInfo(item, prop);
            }
        }
    }


    public class ModelTypeInfo
    {
        public ModelTypeInfo(Type type, PropertyInfo typeOfKeyType)
        {
            // TODO: Complete member initialization
            this.EntityType = type;
            this.TypeOfKeyType = typeOfKeyType;
        }
        public Type EntityType { get; set; }
        public PropertyInfo TypeOfKeyType { get; set; }

        public string TypeNameOfKeyType
        {
            get
            {
                if (this.TypeOfKeyType == null)
                    return "Object";

                return TypeOfKeyType.PropertyType.Name;
            }
        }
    }
}
