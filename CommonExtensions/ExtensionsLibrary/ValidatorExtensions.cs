using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionsLibrary
{
    public static class ValidatorExtensions
    {
        /// <summary>
        /// 验证对象是否有效
        /// </summary>
        /// <param name="this">要验证的对象</param>
        /// <returns></returns>
        public static bool IsValid(this object @this)
        {
            var isValid = true;
            if (@this is IEnumerable list)
            {
                foreach (var item in list)
                {
                    if (!item.IsValid())
                    {
                        isValid = false;
                        break;
                    }
                }
            }
            else
            {
                isValid = Validator.TryValidateObject(@this, new ValidationContext(@this, null, null), new Collection<ValidationResult>(), true);
            }
            return isValid;
        }

        /// <summary>
        /// 验证对象是否有效
        /// </summary>
        /// <param name="this">要验证的对象</param>
        /// <returns></returns>
        public static (bool isValid, Collection<ValidationResult> validationResults) IsValidWithResult(this object @this)
        {
            var isValid = true;
            var validationResults = new Collection<ValidationResult>();
            if (@this is IEnumerable list)
            {
                foreach (var item in list)
                {
                    var (r, results) = item.IsValidWithResult();
                    if (!r && !results.IsNullOrEmpty())
                    {
                        isValid = false;
                        results.ToList().ForEach(o => validationResults.Add(o));
                        break;
                    }
                }
            }
            else
            {
                isValid = Validator.TryValidateObject(@this, new ValidationContext(@this, null, null), validationResults, true);
            }
            return (isValid, validationResults);
        }
    }
}