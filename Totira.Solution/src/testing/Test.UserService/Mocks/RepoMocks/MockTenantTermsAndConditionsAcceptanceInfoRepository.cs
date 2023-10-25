using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Domain;
using System.Linq.Expressions;

namespace Test.UserService.Mocks.RepoMocks;

public static class MockTenantTermsAndConditionsAcceptanceInfoRepository
{
    public static Mock<IRepository<TenantTermsAndConditionsAcceptanceInfo, Guid>> GetTenantTermsAndConditionsAcceptanceInfoRepository()
    {
        #region MockingData
        var tenantTermsAndConditionsAcceptanceInfo = new List<TenantTermsAndConditionsAcceptanceInfo>
    {
        new TenantTermsAndConditionsAcceptanceInfo
        {
            Id= new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
           TenantId=new Guid("CF0A8C1C-F2D1-41A1-A12C-53D9BE513A1C"),
           SigningDateTime=DateTime.Now,
        },

         new TenantTermsAndConditionsAcceptanceInfo
        {
            Id= new Guid("CF0A8C1C-FFFF-41A1-A12C-53D9BE513A1C"),
           TenantId=new Guid("FFFFFFFF-F2D1-41A1-A12C-53D9BE513A1C"),
           SigningDateTime=DateTime.Now,
        },
    };
        #endregion

        #region instanciateTheMock
        var mockRepo = new Mock<IRepository<TenantTermsAndConditionsAcceptanceInfo, Guid>>();
        #endregion

        #region SetupAllMethods

        mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<TenantTermsAndConditionsAcceptanceInfo, bool>>>()))
                    .ReturnsAsync((Expression<Func<TenantTermsAndConditionsAcceptanceInfo, bool>> expression) =>
                    tenantTermsAndConditionsAcceptanceInfo.Where(expression.Compile()).ToList());



        return mockRepo;
        #endregion

    }


}
