using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Moq;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.DTO;
using System.Linq.Expressions;
using Shouldly;
using Test.UserService.Mocks.RepoMocks;

namespace Test.UserService.Commands
{
    public class CreateGroupApplicationCommandHandlerTest
    {
        private readonly Mock<ILogger<CreateGroupApplicationCommandHandler>> _loggerMock;
        private readonly Mock<IRepository<TenantGroupApplicationProfile, Guid>> _tenantGroupApplicationProfileMockRepository;
        private readonly CreateGroupApplicationCommand _command;
        public CreateGroupApplicationCommandHandlerTest()
        {
            _tenantGroupApplicationProfileMockRepository= MockGroupApplicationRepository.GetGroupApplicationRepository();
            _command = new CreateGroupApplicationCommand()
            {
                GroupApplicationProfiles = new List<TenantGroupApplicationProfileDto>()
                {
                    new TenantGroupApplicationProfileDto()
                    {
                       TenantId=new Guid("CF0A8C1C-F2D1-41A1-A12C-53D9BE513A1C"),
                       FirstName="Azza",
                       Email="Azza@totira.com",
                    },
                     new TenantGroupApplicationProfileDto()
                    {
                     TenantId=new Guid("FFFFFFFF-F2D1-41A1-A12C-53D9BE513A1C"),
                      FirstName="Mona",
                      Email="Mona12@Gmail.com",
                    }
                },
            };
            _loggerMock = new Mock<ILogger<CreateGroupApplicationCommandHandler>>();
           
        }

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            var handler = new CreateGroupApplicationCommandHandler
                ( _loggerMock.Object, _tenantGroupApplicationProfileMockRepository.Object,null,null);

            //Act
            await handler.HandleAsync(null, _command);

            //Assert
            var x= _command.GroupApplicationProfiles.FirstOrDefault();
            Expression<Func<TenantGroupApplicationProfile, bool>> filter = f => f.TenantId == x.TenantId;
            var tenantGroupApplicationProfileInfo = await _tenantGroupApplicationProfileMockRepository
                .Object.Get(filter);


            tenantGroupApplicationProfileInfo.ShouldBeOfType<List<TenantGroupApplicationProfile>>();  
            tenantGroupApplicationProfileInfo?.FirstOrDefault()?.TenantId.ShouldBeEquivalentTo(new Guid("CF0A8C1C-F2D1-41A1-A12C-53D9BE513A1C"));
            tenantGroupApplicationProfileInfo?.FirstOrDefault()?.Email.ShouldBeEquivalentTo("Azza@totira.com");
            tenantGroupApplicationProfileInfo?.FirstOrDefault()?.FirstName.ShouldBeEquivalentTo("Azza");
            tenantGroupApplicationProfileInfo.Count().ShouldBe(1);
        }
        [Fact]
        public async Task HandleAsyncTest_RepeatedId()
        {
            //Arrange
            var handler = new CreateGroupApplicationCommandHandler
                (_loggerMock.Object, _tenantGroupApplicationProfileMockRepository.Object,null,null);

            //Act
            await handler.HandleAsync(null, _command);

            //Assert
            var employmentReferenceList = await _tenantGroupApplicationProfileMockRepository.Object.Get(x => true);
            employmentReferenceList.Count().ShouldBe(2);
        
        }

    }
}
