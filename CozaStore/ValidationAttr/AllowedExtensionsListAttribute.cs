﻿using System.ComponentModel.DataAnnotations;

namespace CozaStore.ValidationAttr
{
    public class AllowedExtensionsListAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsListAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var files = value as List<IFormFile>;
            if (files != null)
            {
                foreach (var file in files)
                {
                    var extension = Path.GetExtension(file.FileName);
                    if (!_extensions.Contains(extension.ToLower()))
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }
            }
            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            string errorMessage = "Please select files with the following formats: ";
            foreach (var extension in _extensions)
            {
                errorMessage += extension + "; ";
            }
            return errorMessage;
        }
    }
}
