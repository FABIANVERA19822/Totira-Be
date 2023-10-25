using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Bussiness.UserService.Common;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Services.RootService.Commands;
using Totira.Support.Api.Options;

namespace Test.UserService.Mocks.CommonMocks
{
    public static class MockCommonFunction
    {
        public static Mock<ICommonFunctions> GetCommonFunctionsMock()
        {
            #region MockingData
            var mockedFile = new Totira.Bussiness.UserService.DTO.GetTenantProfileImageDto.ProfileImageFile("test.jpg", "image/jpeg", "");
            var mockedTenantProfileImageDto = new GetTenantProfileImageDto(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"), mockedFile);
            var mockedData = new List<GetTenantProfileImageDto>() {  mockedTenantProfileImageDto };
            #endregion

            #region CreateMock
            var commonFunctionMock = new Mock<ICommonFunctions>();
            #endregion

            #region Setup
            commonFunctionMock.Setup(r => r.GetProfilePhoto(It.IsAny<QueryTenantProfileImageById>()))
                              .ReturnsAsync((QueryTenantProfileImageById query)=> mockedData.Where(x=>x.Id==query.Id).FirstOrDefault());
            #endregion

            return commonFunctionMock;
        }
    }
}
