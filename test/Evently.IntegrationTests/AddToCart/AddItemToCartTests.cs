using Evently.Common.Domain;
using Evently.IntegrationTests.Abstractions;
using Evently.Modules.Ticketing.Application.Carts.AddItemToCart;
using Evently.Modules.Ticketing.Application.Customers.GetCustomer;
using Evently.Modules.Users.Application.Users.RegisterUser;
using FluentAssertions;

namespace Evently.IntegrationTests.AddToCart;

public sealed class AddItemToCartTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    private const decimal Quantity = 10;

    [Fact]
    public async Task Customer_ShouldBeAbleTo_AddItemToCart()
    {
        // Register a user
        var command = new RegisterUserCommand(
            Faker.Internet.Email(),
            Faker.Internet.Password(6),
            Faker.Name.FirstName(),
            Faker.Name.LastName());
        
        Result<Guid> userResult = await Sender.Send(command);
        userResult.IsSuccess.Should().BeTrue();
        
        // Get a customer  Func<Task<Result<T>>> func
        Result<CustomerResponse> customerResult = await Poller.WaitAsync(
            TimeSpan.FromSeconds(15),
            async () =>
            {
                var query = new GetCustomerQuery(userResult.Value);
                return await Sender.Send(query);
            });
        customerResult.IsSuccess.Should().BeTrue();

        var ticketTypeId = Guid.NewGuid();
        await Sender.CreateEventAsync(Guid.NewGuid(), ticketTypeId, Quantity);
        
        // Add item to cart
        var addItemToCartCommand = new AddItemToCartCommand(
            customerResult.Value.Id,
            ticketTypeId,
            Quantity);
        
        Result addItemToCartResult = await Sender.Send(addItemToCartCommand);
        addItemToCartResult.IsSuccess.Should().BeTrue();
    }
}
