namespace MyFriendsV3.Services
{
    public static class FileService
    {
        public static bool IsValidImageFile(IFormFile file)
        {
            // Check the file size, type, etc.
            var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension) || file.Length > 10 * 1024 * 1024) // Example: 10 MB limit
            {
                return false;
            }

            return true;
        }

        public static byte[] ConvertToByteArray(IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
