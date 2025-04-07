using Evently.Common.Domain;
using Evently.Modules.Events.Application.Categories.UpdateCategory;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.IntegrationTests.Abstractions;
using FluentAssertions;

namespace Evently.Modules.Events.IntegrationTests.Categories;

public class UpdateCategoryTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    public static readonly TheoryData<UpdateCategoryCommand> InvalidCommands = new()
    {
        new UpdateCategoryCommand(Guid.Empty, Faker.Music.Genre()),
        new UpdateCategoryCommand(Guid.NewGuid(), string.Empty),
    };
    
    [Theory]
    [MemberData(nameof(InvalidCommands))]
    public async Task Should_ReturnFailure_When_CommandIsInvalid(UpdateCategoryCommand command)
    {
        // Arrange
        await CleanDatabaseAsync();
        
        // Act
        Result result = await Sender.Send(command);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
    }
    
    [Fact]
    public async Task Should_ReturnFailure_When_CategoryDoesNotExist()
    {
        // Arrange
        var command = new UpdateCategoryCommand(Guid.NewGuid(), Faker.Music.Genre());
        
        // Act
        Result result = await Sender.Send(command);
        
        // Assert
        result.Error.Should().Be(CategoryErrors.NotFound(command.CategoryId));
    }
    
    [Fact]
    public async Task Should_UpdateCategory_WhenCategoryExists()
    {
        // Arrange
        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());

        var command = new UpdateCategoryCommand(categoryId, Faker.Music.Genre());

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}
