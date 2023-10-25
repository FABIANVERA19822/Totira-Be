using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Commands;
using Totira.Services.RootService.Commands;
using System.Linq.Expressions;

namespace Test.UserService.Mocks.RepoMocks;

public static class MockTenantApplicationRequestRepo
{
    public static Mock<IRepository<TenantApplicationRequest, Guid>> GetTenantApplicationRequestRepository()
    {
        #region MockingData
        var tenantTenantApplicationRequests = new List<TenantApplicationRequest>
            {
                new TenantApplicationRequest
                {
                    
                    TenantId= new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),

                    CreatedBy = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    CreatedOn = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedOn = null,
                    ApplicationDetailsId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    Id = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),

                    Coapplicants= new List<Totira.Bussiness.UserService.Domain.Coapplicant>
                    {
                        new Totira.Bussiness.UserService.Domain.Coapplicant() { Email="test@test.com", }
                    ,
                        new Totira.Bussiness.UserService.Domain.Coapplicant() { Email="test1@test.com" }
                    },

                    Guarantor=  new Totira.Bussiness.UserService.Domain.Coapplicant(){Id=new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1E"), Email ="test2@test.com"},

                }
            };
        #endregion

        #region instanciateTheMock
        var mockRepo = new Mock<IRepository<TenantApplicationRequest, Guid>>();
        #endregion

        #region SetupAllMethods

        //mockRepo.Setup(r => r.Get(x => true))
        //            .ReturnsAsync(tenantTenantApplicationRequests);

        //mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
        //         .ReturnsAsync((Guid id) => tenantTenantApplicationRequests.Where(x => x.Id == id).SingleOrDefault());


        mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<TenantApplicationRequest, bool>>>()))
                  .ReturnsAsync((Expression<Func<TenantApplicationRequest, bool>> expression) => tenantTenantApplicationRequests.Where(expression.Compile()).ToList());

        mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid id) => tenantTenantApplicationRequests.Where(x => x.Id == id).SingleOrDefault());


        #endregion
        return mockRepo;
    }
}
