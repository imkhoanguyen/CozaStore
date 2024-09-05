using System.ComponentModel.DataAnnotations;

namespace CozaStore.ValidationAttr
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
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
