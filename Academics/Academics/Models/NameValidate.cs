using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Academics.Models
{
    public class NameValidate : Attribute, IModelValidator
    {
        public string ErrorMessage { get; set; } = "Name can only contain letters.";

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            var results = new List<ModelValidationResult>();

            if (context.Model == null || string.IsNullOrWhiteSpace(context.Model.ToString()))
            {
                return results;
            }

            string name = context.Model.ToString();

            if (!Regex.IsMatch(name, @"^[a-zA-Z]+$"))
            {
                results.Add(new ModelValidationResult("", ErrorMessage));
            }

            return results;
        }
    }
}