
namespace AuthService.Application.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T message) where T: class;
    }
}
