using Socially.Mobile.Logic.Models;

namespace Socially.Mobile.Logic.Services
{
    public interface IPubSubService
    {
        void Publish(PublishMessage message);
        void Subscribe<TMessage>(object recipient, Action<TMessage> action) where TMessage : PublishMessage, new();
        void UnSubscribe<TMessage>(object recipient) where TMessage : PublishMessage, new();
    }
}