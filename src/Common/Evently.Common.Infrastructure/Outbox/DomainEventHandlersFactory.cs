using System.Collections.Concurrent;
using System.Reflection;
using Evently.Common.Application.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Common.Infrastructure.Outbox;

public static class DomainEventHandlersFactory
{
    private static readonly ConcurrentDictionary<string, Type[]> HandlersDictionary = new();

    public static IEnumerable<IDomainEventHandler> GetHandlers(
        Type type, IServiceProvider serviceProvider, Assembly assembly)
    {
        Type[] domainEventHandlerTypes = HandlersDictionary.GetOrAdd(
            $"{assembly.GetName().Name}{type.Name}",
            _ =>
            {
                Type[] domainEventHanderTypes = assembly.GetTypes()
                    .Where(t => t.IsAssignableTo(typeof(IDomainEventHandler<>).MakeGenericType(type)))
                    .ToArray();
                return domainEventHanderTypes;
            });

        List<IDomainEventHandler> handlers = [];
        handlers.AddRange(domainEventHandlerTypes.Select(serviceProvider.GetRequiredService)
            .Select(domainEventHandler => (IDomainEventHandler)domainEventHandler));
        return handlers;
    }
}
