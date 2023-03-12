using CommunityToolkit.Mvvm.Messaging;
using Socially.Mobile.Logic.Models;
using Socially.Mobile.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApp.Services
{
    public class PubSubService : IPubSubService
    {

        public void Publish(PublishMessage message)
        {
            WeakReferenceMessenger.Default.Send(message, (int)message.Type);
        }

        public void Subscribe<TMessage>(object recipient, Action<TMessage> action)
            where TMessage : PublishMessage, new()
        {
            WeakReferenceMessenger.Default
                                  .Register<TMessage, int>(recipient, GetTypeAsInt<TMessage>(), (r, m) => action(m));
        }

        public void UnSubscribe<TMessage>(object recipient)
            where TMessage : PublishMessage, new()
        {
            WeakReferenceMessenger.Default.Unregister<TMessage, int>(recipient, GetTypeAsInt<TMessage>());
        }

        static int GetTypeAsInt<TMessage>()
            where TMessage : PublishMessage, new()
            => (int)new TMessage().Type;

    }
}
