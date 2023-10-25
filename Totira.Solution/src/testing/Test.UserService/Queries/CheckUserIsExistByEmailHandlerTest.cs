﻿using Microsoft.Extensions.DependencyModel;
using Moq;
using Shouldly;
using Test.UserService.Mocks.RepoMocks;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Handlers.Queries;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.CommonLibrary.Interfaces;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Queries;

public class CheckUserIsExistByEmailHandlerTest
{
    private readonly Mock<IRepository<TenantBasicInformation, Guid>> _tenantBasicInformationRepositoryMock;
    private readonly Mock<IRepository<TenantApplicationRequest, Guid>> _tenantApplicationRequestRepositoryMock;
    private readonly Mock<IRepository<TenantGroupApplicationProfile, Guid>> _tenantGroupApplicationProfileRepositoryMock;
   
    public CheckUserIsExistByEmailHandlerTest()
    {
        _tenantBasicInformationRepositoryMock = MockTenantBasicInformationRepository.GetTenantBasicInformationRepository();
        _tenantApplicationRequestRepositoryMock = MockTenantApplicationRequestRepository.GetTenantApplicationRequestRepository();
        _tenantGroupApplicationProfileRepositoryMock = MockGroupApplicationRepository.GetGroupApplicationRepository();
    }
    [Fact]
    public async Task HandleAsyncTest_Ok()
    {

        //Arrange
        var handler = new CheckUserIsExistByEmailHandler(_tenantBasicInformationRepositoryMock.Object,
                                                         _tenantApplicationRequestRepositoryMock.Object,
                                                         _tenantGroupApplicationProfileRepositoryMock.Object);
        var query = new QueryCheckUserIsExistByEmail("testymactest@test.test");

        //Act
        var result = await handler.HandleAsync(query);

        //Assert
        result.ShouldBeOfType<GetUserIsExistByEmailDto>();
        result.IsExistUser.ShouldBe(true);
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task HandleAsyncTest_BadId()
    {
        //Arrange
        //Arrange
        var handler = new CheckUserIsExistByEmailHandler(_tenantBasicInformationRepositoryMock.Object,
                                                         _tenantApplicationRequestRepositoryMock.Object,
                                                         _tenantGroupApplicationProfileRepositoryMock.Object);
        var query = new QueryCheckUserIsExistByEmail("testymactest142@test.test");

        //Act
        var result = await handler.HandleAsync(query);

        //Assert
        result.ShouldBeOfType<GetUserIsExistByEmailDto>();
        result.IsExistUser.ShouldBe(false);
        result.ShouldNotBeNull();
    }
}
