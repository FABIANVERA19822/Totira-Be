using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Domain;
using Totira.Business.ThirdPartyIntegrationService.Domain.Persona;
using System.Linq.Expressions;

namespace Test.UserService.Mocks.RepoMocks
{
    public class MockTenantPersonaValidationRepository
    {
        public static Mock<IRepository<TenantPersonaValidation, string>> GetTenantPersonaValidationRepository()
        {
            #region MockingData
            var tenantPersonaValidations = new List<TenantPersonaValidation>
            {
                new TenantPersonaValidation
                {
                    Id = "CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C",
                    TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    Status = "approved",
                    CreatedBy = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    CreatedOn = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedOn = null,
                },
                new TenantPersonaValidation
                {
                    Id = "CF0A8C1C-F2D0-41A1-A12C-53D9BE513333",
                    TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513333"),
                    Status = "failed",
                    CreatedBy = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513333"),
                    CreatedOn = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedOn = null,
                }
            };
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IRepository<TenantPersonaValidation, string>>();
            #endregion

            #region SetupAllMethods

            mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<TenantPersonaValidation, bool>>>()))
                        .ReturnsAsync((Expression<Func<TenantPersonaValidation, bool>> expression) => tenantPersonaValidations.Where(expression.Compile()).ToList());

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<string>()))
                    .ReturnsAsync((string id) => tenantPersonaValidations.Single(x => x.Id == id));

            mockRepo.Setup(r => r.Add(It.IsAny<TenantPersonaValidation>()))
                    .Callback<TenantPersonaValidation>(tenantPersonaValidations.Add);

            mockRepo.Setup(r => r.Update(It.IsAny<TenantPersonaValidation>()))
                    .Verifiable();

            #endregion
            return mockRepo;
        }

    }
}
