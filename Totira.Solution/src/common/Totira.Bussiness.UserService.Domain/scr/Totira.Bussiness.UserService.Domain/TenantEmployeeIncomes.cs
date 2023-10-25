using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Bussiness.UserService.Domain
{
    public sealed class TenantEmployeeIncomes : Document, IAuditable, IEquatable<TenantBasicInformation>
    {
        public bool IsStudent { get; set; }
        public List<TenantEmployeeIncome>? EmployeeIncomes { get; set; }
        public List<TenantStudentFinancialDetail>? StudentIncomes { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public bool Equals(TenantBasicInformation? other)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initialize a new instance of <see cref="TenantEmployeeIncomes" /> class.
        /// </summary>
        /// <param name="tenantId">The tenant id.</param>
        private TenantEmployeeIncomes(Guid tenantId)
        {
            Id = tenantId;
            EmployeeIncomes = new();
            StudentIncomes = new();
        }

        /// <summary>
        /// Creates a new <see cref="TenantEmployeeIncomes"/> object.
        /// </summary>
        /// <param name="tenantId">Tenant id.</param>
        /// <returns>A new <see cref="TenantEmployeeIncomes"/> object.</returns>
        public static TenantEmployeeIncomes Create(Guid tenantId)
            => new(tenantId);
    }
}
