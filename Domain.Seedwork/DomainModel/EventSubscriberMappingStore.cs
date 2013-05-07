using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Domain.Seedwork.DomainModel
{
    public class EventSubscriberMappingStore
    {
        #region Private Variables

        private static EventSubscriberMappingStore current = new EventSubscriberMappingStore();
        private Dictionary<Type, List<Type>> eventSubscriberTypeMappings = new Dictionary<Type, List<Type>>();
        private Dictionary<Type, IEnumerable<MethodInfo>> subscriberEventHandlers = new Dictionary<Type, IEnumerable<MethodInfo>>();

        #endregion

        #region Public Properties

        public static EventSubscriberMappingStore Current
        {
            get { return current; }
        }

        #endregion

        #region Public Methods

        public void ResolveEventSubscriberTypeMappings(Assembly assembly)
        {
            foreach (var subscriberType in assembly.GetTypes().Where(type => type.IsClass && type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Count() == 0))
            {
                var eventHandlers = subscriberType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(IsEventHandler);
                subscriberEventHandlers.Add(subscriberType, eventHandlers);
                foreach (var eventHandler in eventHandlers)
                {
                    var eventType = eventHandler.GetParameters()[0].ParameterType;
                    List<Type> subscriberTypes = null;
                    if (!eventSubscriberTypeMappings.TryGetValue(eventType, out subscriberTypes))
                    {
                        subscriberTypes = new List<Type>();
                        eventSubscriberTypeMappings.Add(eventType, subscriberTypes);
                    }
                    if (!subscriberTypes.Exists(existingSubscriberType => existingSubscriberType == subscriberType))
                    {
                        subscriberTypes.Add(subscriberType);
                    }
                }
            }
        }
        public List<Type> GetSubscriberTypesList(Type eventType)
        {
            //Get all the subscribers which subscribe the current event type
            //or subscribe the event types which can be assigned from the current event type.
            var subscriberTypesList = eventSubscriberTypeMappings.ToList().FindAll(pair => pair.Key.IsAssignableFrom(eventType)).Select(pair => pair.Value).ToList();

            //Merge all the subscriber types into one list.
            List<Type> mergedSubscriberTypes = new List<Type>();
            foreach (var subscriberTypes in subscriberTypesList)
            {
                foreach (var subscriberType in subscriberTypes)
                {
                    if (!mergedSubscriberTypes.Exists(mst => mst == subscriberType))
                    {
                        mergedSubscriberTypes.Add(subscriberType);
                    }
                }
            }

            //Return all the merged subscriber types.
            return mergedSubscriberTypes;
        }
        public IEnumerable<MethodInfo> GetEventHandlers(Type subscriberType)
        {
            IEnumerable<MethodInfo> eventHandlers = null;
            if (!subscriberEventHandlers.TryGetValue(subscriberType, out eventHandlers))
            {
                eventHandlers = new List<MethodInfo>();
            }
            return eventHandlers;
        }

        #endregion

        #region Private Methods

        private bool IsEventHandler(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            return parameters.Count() == 1 && typeof(IDomainEvent).IsAssignableFrom(parameters[0].ParameterType);
        }

        #endregion
    }
}
