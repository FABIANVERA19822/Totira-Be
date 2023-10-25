namespace Totira.Services.RootService.Extensions
{
    public static class ImageExtensions
    {
        public static IFormFile BuildImageFromBase64(this string base64Url, string nameImage, string contentType)
        {
            byte[] bytes = Convert.FromBase64String(base64Url);
            MemoryStream stream = new MemoryStream(bytes);

            var fileExtension = Path.GetExtension(nameImage);
            IFormFile file = new FormFile(stream, 0, bytes.Length, nameImage, nameImage)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

            return file;
        }
    }
}
