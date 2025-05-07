using System;
using System.IO;

public static class FileValidationHelper
{
    public static bool IsFileTypeAllowed(string fileName)
    {
        var forbiddenExtensions = new[] { ".exe", ".zip", ".bat" };
        var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

        return !Array.Exists(forbiddenExtensions, ext => ext.Equals(fileExtension, StringComparison.OrdinalIgnoreCase));
    }
}