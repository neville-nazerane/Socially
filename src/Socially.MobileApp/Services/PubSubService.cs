using CommunityToolkit.Mvvm.Messaging;
using Socially.Mobile.Logic.Models;
using Socially.Mobile.Logic.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApp.Services
{
    public class PubSubService : IPubSubService
    {

        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Guid, Func<object, Task>>> _subscribes;

        public PubSubService()
        {
            _subscribes = new();
        }

        public async Task PublishAsync<TMessage>(TMessage message)
        {
            if(_subscribes.TryGetValue(typeof(TMessage), out var res))
                foreach (var fun in res.Values)
                    await fun(message);
        }

        public void Subscribe<TMessage>(Guid id, Func<object, Task> func)
        {
            var res = _subscribes.GetOrAdd(typeof(TMessage), k => new());
            res.AddOrUpdate(id, func, (k, v) => func);
        }

        public void Unsubscribe<TMessage>(Guid id)
        {
            if (_subscribes.TryGetValue(typeof(TMessage), out var subs))
                subs.TryRemove(id, out _);
        }
        
        
        //public void Publish(PublishMessage message)
        //{
        //    WeakReferenceMessenger.Default.Send(message);
        //}

        //public void Subscribe<TMessage>(object recipient, Action<TMessage> action)
        //    where TMessage : PublishMessage, new()
        //{
        //    WeakReferenceMessenger.Default
        //                          .Register<TMessage>(recipient, (r, m) => action(m));
        //}

        //public void UnSubscribe<TMessage>(object recipient)
        //    where TMessage : PublishMessage, new()
        //{
        //    WeakReferenceMessenger.Default.Unregister<TMessage, int>(recipient, GetTypeAsInt<TMessage>());
        //}

        //static int GetTypeAsInt<TMessage>()
        //    where TMessage : PublishMessage, new()
        //    => (int)new TMessage().Type;

    }
}
