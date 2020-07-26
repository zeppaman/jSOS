using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace JsOS.APP.Services
{
    public class MessageBusService
    {
        Dictionary<string, List<Action<object>>> events = new Dictionary<string, List<Action<object>>>();

        public void RegisterEvent(string name, Action<object> eventHandler)
        {
            List<Action<object>> eventActions = new List<Action<object>>();
            if (events.ContainsKey(name))
            {
                eventActions = events[name];
            }
            eventActions.Add(eventHandler);

            events[name] = eventActions;
        }


        object sequence="";
        public void Emit(string eventName, object argument)
        {
            lock (sequence)
            {
                if (events.ContainsKey(eventName))
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var eventActions = events[eventName];
                        eventActions.ForEach(x => x.Invoke(argument));
                    });
                }
            }
        }
        
    }
}
