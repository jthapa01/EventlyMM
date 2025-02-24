using System.Collections.Concurrent;
using System.Reflection;
using Evently.Common.Application.EventBus;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Common.Infrastructure.Inbox;

public static class IntegrationEventHandlersFactory
{
    private static readonly ConcurrentDictionary<string, Type[]> HandlersDictionary = new();

    public static IEnumerable<IIntegrationEventHandler> GetHandlers(
        Type type, IServiceProvider serviceProvider, Assembly assembly)
    {
        string key = $"{assembly.GetName().Name}-{type.Name}";
        Type[] integrationEventHandlerTypes = 
            HandlersDictionary.GetOrAdd(key, k => GetIntegrationEventHandlerTypes(type, assembly));
        
        List<IIntegrationEventHandler> handlers = [];
        handlers.AddRange(integrationEventHandlerTypes
            .Select(serviceProvider.GetRequiredService)
            .Cast<IIntegrationEventHandler>());

        return handlers;
    }

    private static Type[] GetIntegrationEventHandlerTypes(Type type, Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IIntegrationEventHandler<>).MakeGenericType(type)))
            .ToArray();
    }
}
