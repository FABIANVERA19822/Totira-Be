using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totira.Bussiness.UserService.DTO.Common
{
    public class FileInfoDisplayDto
    {
        public string FileName { get; set; } = default!;
        public long Size { get; set; }
        public string ContentType { get; set; } = default!;

        protected FileInfoDisplayDto(string fileName, string contentType, long size)
        {
            FileName = fileName;
            ContentType = contentType;
            Size = size;
        }

        public static FileInfoDisplayDto Create(string fileName, string contentType, long size)
            => new(fileName, contentType, size);
        public static FileInfoDisplayDto? AdaptFrom(Domain.Common.File? entity)
            => entity is null
            ? default
            : new(entity.FileName, entity.Extension, entity.Size);

        public static List<FileInfoDisplayDto> AdaptFrom(IEnumerable<Domain.Common.File> files)
        {
            var result = new List<FileInfoDisplayDto>();

            foreach (var file in files)
                result.Add(Create(file.FileName, file.Extension, file.Size));

            return result;
        }
    }
}
