namespace Test.PropertiesService;

public abstract class BaseQueryHandlerTest<TQueryHandler>
{
    protected abstract TQueryHandler QueryHandler { get; }
    protected BaseQueryHandlerTest()
    {
    }
}