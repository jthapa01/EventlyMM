namespace Evently.Modules.Attendance.IntegrationTests.Abstractions;

[CollectionDefinition(nameof(IntegrationTestCollection))]
public class IntegrationTestCollection: ICollectionFixture<IntegrationTestWebAppFactory>;
