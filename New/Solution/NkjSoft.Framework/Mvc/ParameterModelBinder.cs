using NkjSoft.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Helpers;
namespace NkjSoft.Framework.Mvc
{
    public class ParameterModelBinder : DefaultModelBinder
    {
        protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            if (EqualsOrNullEmpty(propertyDescriptor.Name, "Value", StringComparison.CurrentCultureIgnoreCase))
            {
                var value = controllerContext.RequestContext.HttpContext.Request.Unvalidated().Form[bindingContext.ModelName];
                var dataTypeModelName = bindingContext.ModelName.Replace("Value", "DataType");
                var dataType = (DataType)Enum.Parse(typeof(DataType), bindingContext.ValueProvider.GetValue(dataTypeModelName).AttemptedValue);
                var parameterValue = DataTypeHelper.ParseValue(dataType, value, false);
                return parameterValue;
            }
            else
                return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
        }

        /// <summary>
        /// Equalses the or null empty.
        /// </summary>
        /// <param name="str1">The STR1.</param>
        /// <param name="str2">The STR2.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns></returns>
        public static bool EqualsOrNullEmpty(string str1, string str2, StringComparison comparisonType)
        {
            return String.Compare(str1 ?? "", str2 ?? "", comparisonType) == 0;
        }
    }
}
