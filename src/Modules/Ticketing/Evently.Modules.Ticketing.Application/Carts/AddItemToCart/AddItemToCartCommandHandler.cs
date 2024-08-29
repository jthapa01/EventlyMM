using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Events;

namespace Evently.Modules.Ticketing.Application.Carts.AddItemToCart;

internal sealed class AddItemToCartCommandHandler(
    CartService cartService, 
    ICustomerRepository customerRepository, 
    ITicketTypeRepository ticketTypeRepository)
    : ICommandHandler<AddItemToCartCommand>
{
    public async Task<Result> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        // 1. Get customer
        Customer? customer = await customerRepository.GetAsync(request.CustomerId, cancellationToken);

        if (customer is null)
        {
            return Result.Failure(CustomerErrors.NotFound(request.CustomerId));
        }

        // 2. Get ticket type
        TicketType? ticketType = await ticketTypeRepository.GetAsync(request.TicketTypeId, cancellationToken);

        if (ticketType is null)
        {
            return Result.Failure(TicketTypeErrors.NotFound(request.TicketTypeId));
        }
        
        if(ticketType.AvailableQuantity < request.Quantity)
        {
            return Result.Failure(TicketTypeErrors.NotEnoughQuantity(ticketType.AvailableQuantity));
        }

        // 3. Add item to cart
        var cartItem = new CartItem
        {
            TicketTypeId = request.TicketTypeId,
            Quantity = request.Quantity,
            Price = ticketType.Quantity,
            Currency = ticketType.Currency
        };

        await cartService.AddItemAsync(request.CustomerId, cartItem, cancellationToken);

        return Result.Success();
    }
}
