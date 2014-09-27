using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using TopGame.Core;

namespace TopGame.Web.Framework.Validator
{
    public sealed class CustomValidationCpfAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// Verifica se cpf informado é válido
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return false;

            return Regex.IsMatch(value.ToString(), @"^\d{3}\.?\d{3}\.?\d{3}\-?\d{2}$") && CommonHelper.IsValidCpf(value.ToString());
        }

        /// <summary>
        /// Regras de validação no cliente
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(null),
                ValidationType = "customvalidationcpf"
            };
        }
    }
}
