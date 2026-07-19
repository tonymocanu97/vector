namespace Vector.IntegrationTests
{
    [CollectionDefinition(Name)]
    public class ApiTestCollection : ICollectionFixture<ApiWebApplicationFactory>
    {
        public const string Name = "Api";
    }
}
