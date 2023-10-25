namespace Totira.Services.RootService.DTO.Landlord.GetDtos
{
    public class GetApplicationListPageDto
    {
        public SortApplicationsBy SortBy { get; set; } = SortApplicationsBy.SubmissionDate;
        public long Count { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool Asc { get; set; }
        public PropertyApplied PropertyApplied { get; set; } = new();
        public List<ApplicationDashboard> Applications { get; set; } = new List<ApplicationDashboard>();
    }
    public class ApplicationDashboard
    {

        public Guid PropertyApplicationId { get; set; }
        public DateTimeOffset SubmissionDate { get; set; }
        public string StatusApplication { get; set; } = default!;
        public Applicant Applicant { get; set; } = new Applicant();
        public ApplicationDetailsDto ApplicationDetails { get; set; } = new();
    }
    public class PropertyApplied
    {
        public string PropertyId { get; set; } = default!;
        public PropertyMainImage PropertyImageFile { get; set; } = default!;
        public string AreaProvince { get; set; } = default!;
        public string Address { get; set; } = default!;
        public decimal ListPrice { get; set; }
        public bool isApplied { get; set; }
    }
    public class Applicant
    {
        public string MainApplicantName { get; set; } = default!;
        public bool isMulti { get; set; }
        public int NumberCosigners { get; set; }
        public List<Cosigners>? Cosigners { get; set; }
        public GetTenantProfileImageDto? MainApplicantProfileImage { get; set; }
        public GetApplicationListPageDetails BackgroundMainTenantDetail { get; set; } = new();
    }
    public class ApplicationDetailsDto
    {
        public Guid ApplicationDetailsId { get; set; }
        public double TotalIncome { get; set; }
        public ApplicationDetailEstimatedMove? EstimatedMove { get; set; }
        public string EstimatedRent { get; set; } = default!;
        public ApplicationDetailOccupants Occupants { get; set; } = new ApplicationDetailOccupants(0, 0);

    }
    public class ApplicationDetailOccupants
    {
        public ApplicationDetailOccupants(int adults, int childrens)
        {
            Adults = adults;
            Children = childrens;
        }

        public int Children { get; set; }
        public int Adults { get; set; }
    }
    public class Cosigners
    {
        public string CosignerName { get; set; } = default!;
        public GetTenantProfileImageDto? CosignersProfileImage { get; set; }
        public GetApplicationListPageDetails BackgroundCosignerDetail { get; set; } = new();
    }
    public class GetApplicationListPageDetails
    {
        public Guid ApplicationRequest { get; set; }
        public string TypeIncome { get; set; } = default!;
        public double TotalIncome { get; set; }
        public int PropertyPriceIncomeScale { get; set; }
        public CreditRate CreditScore { get; set; } = new();
        public string BackgroundCheckStatus { get; set; } = default!;
    }

    public class CreditRate
    {
        public string Status { get; set; } = default!;
        public string Score { get; set; } = default!;
    }
    public enum SortApplicationsBy
    {
        SubmissionDate = 0,
        Applicant = 1,
        TotalIncome = 2,
        MoveInDate = 3,
        Duration = 4,
        Occupants = 5
    }
}
