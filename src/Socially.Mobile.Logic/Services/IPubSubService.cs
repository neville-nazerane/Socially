using Socially.Mobile.Logic.Models;

namespace Socially.Mobile.Logic.Services
{
    public interface IPubSubService
    {
        Task PublishAsync<TMessage>(TMessage message);
        void Subscribe<TMessage>(Guid id, Func<object, Task> func);
        void Unsubscribe<TMessage>(Guid id);
    }
}