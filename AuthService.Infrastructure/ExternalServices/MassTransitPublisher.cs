using AuthService.Application.Interfaces;
using MassTransit;

namespace AuthService.Infrastructure.ExternalServices
{
    public class MassTransitPublisher(IPublishEndpoint publishEndpoint) : IEventPublisher
    {
        public async Task PublishAsync<T>(T message) where T : class
        {
            await publishEndpoint.Publish(message);
        }
    }
}
