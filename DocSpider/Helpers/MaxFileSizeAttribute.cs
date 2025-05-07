using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly int _maxFileSizeInBytes;

    public MaxFileSizeAttribute(int maxFileSizeInBytes)
    {
        _maxFileSizeInBytes = maxFileSizeInBytes;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var file = value as IFormFile;
        if (file != null && file.Length > _maxFileSizeInBytes)
        {
            var tamanhoMB = _maxFileSizeInBytes / (1024 * 1024);
            return new ValidationResult($"O tamanho máximo permitido é de {tamanhoMB} MB.");
        }

        return ValidationResult.Success;
    }
}