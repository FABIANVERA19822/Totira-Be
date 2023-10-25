namespace Totira.Bussiness.UserService.DTO
{
    public class GetIncomeFilesDto
    {
        public Guid IncomeId { get; set; }
        public List<File> Files { get; set; }

    }

    public class File
    {
        public byte[] Content { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
    }
}
