
using Moq;
using Microsoft.Extensions.Logging;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.PropertiesService.Domain;
using CrestApps.RetsSdk.Services;
using Totira.Bussiness.PropertiesService.Handlers.Commands;
using Totira.Bussiness.PropertiesService.Commands;
using Test.PropertiesService.RepoMocks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Totira.Bussiness.PropertiesService.Mappers;
using Totira.Support.Api.Options;
using Totira.Support.Api.Connection; 
using LanguageExt;
using Shouldly;
using CrestApps.RetsSdk.Models;

namespace Test.PropertiesService.Commands;
public class CreatePropertyfromRETSCommandHandlerTest
{
    private readonly Mock<IRepository<Property, string>> _propertydataRepositoryMock;
    private readonly Mock<ILogger<CreatePropertyfromRETSCommandHandler>> _loggerMock;
    private readonly IMapper _mapper; 
    private readonly Mock<IQueryRestClient> _queryRestClientMock;
    private readonly Mock<IOptions<RestClientOptions>> _restClientOptionsMock;

    public CreatePropertyfromRETSCommandHandlerTest()
    {
        _loggerMock = new Mock<ILogger<CreatePropertyfromRETSCommandHandler>>();
        _propertydataRepositoryMock = MockPropertyRepository.GetPropertyRepository("ResidentialProperty");
        _queryRestClientMock = MockAppConfiguration.GetQueryRestClient();
        _restClientOptionsMock = MockAppConfiguration.GetIOptionsMock();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new PropertyProfile());
            cfg.AddProfile(new ResidentialProfile());
            cfg.AddProfile(new CondoProfile());
        });
         _mapper = new Mapper(mapperConfig);
    }

    [Theory]
    [InlineData("ResidentialProperty")]
    [InlineData("CondoProperty")]
    public async Task HandleAsyncTest_RequestOk_ReturnsOk(string data)
    {
        //Arrange
        Mock<IRetsClient> _clientMock = MockPropertyRepository.GetClientRepository();
        Mock<IRepository<Property, string>> _propertydataRepository = MockPropertyRepository.GetPropertyRepository(data);
        CreatePropertyfromRETSCommand _commandTest = new CreatePropertyfromRETSCommand() { PropertyType = data };

        var handler = new CreatePropertyfromRETSCommandHandler(
            _propertydataRepository.Object,
            _loggerMock.Object,
            _clientMock.Object,
            _mapper,
            _queryRestClientMock.Object,
            _restClientOptionsMock.Object
        );
        //Act
        await handler.HandleAsync(null, _commandTest);

        //Assert
        var property = await _propertydataRepository.Object.Get(x=>true);
        Assert.True(property != null);
        Assert.True(property.FirstOrDefault().PropertyType == _commandTest.PropertyType); 
    }

    [Fact]
    public async Task HandleAsyncTest_RequestFail_DoesntCreateNewEntity()
    {
        //Arrange
        Mock<IRetsClient> _clientMock = MockPropertyRepository.GetClientRepository();
        _clientMock.Setup(tocs => tocs.Search(It.IsAny<SearchRequest>())).Throws(new Exception());
        var handler = new CreatePropertyfromRETSCommandHandler
            (
            _propertydataRepositoryMock.Object,
            _loggerMock.Object,
            _clientMock.Object,
            _mapper,
            _queryRestClientMock.Object,
            _restClientOptionsMock.Object
           );
        //Act
        Either<Exception, CreatePropertyfromRETSCommand> _commandwrong = new CreatePropertyfromRETSCommand()
        {
            PropertyType = string.Empty
        };
        await handler.HandleAsync(null, _commandwrong);

        //Assert
        var propertydata = await _propertydataRepositoryMock.Object.Get(x => true);
        _clientMock.Verify(tocs => tocs.Search(It.IsAny<SearchRequest>()), Times.Once());
        propertydata.Count().ShouldBe(1); 
         
    }   
}