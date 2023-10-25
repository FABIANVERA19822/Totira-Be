namespace Totira.Business.TenantService.DTO
{
    public class GetTenantDto
    {
        public GetTenantDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset CreatedOn { get; set; }
    }
}
