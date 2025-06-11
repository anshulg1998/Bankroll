using System;
using System.Collections.Generic;
using UnityEngine;

    /// <summary>
    /// ServiceLocator provides a simple static service registry for dependency resolution.
    /// </summary>
    public static class ServiceLocator
    {
        // Dictionary to hold registered services by their type
        private static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        /// <summary>
        /// Registers a service instance for the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the service to register.</typeparam>
        /// <param name="service">Service instance to register.</param>
        public static void Register<T>(T service) where T : class
        {
            var type = typeof(T);
            if (service == null)
            {
                Debug.LogError($"Cannot register null service for type {type}");
                return;
            }
            services[type] = service;
        }

        /// <summary>
        /// Retrieves a registered service of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the service to retrieve.</typeparam>
        /// <returns>The registered service instance, or null if not found.</returns>
        public static T Get<T>() where T : class
        {
            var type = typeof(T);
            if (services.TryGetValue(type, out var service))
                return service as T;
            Debug.LogError($"Service of type {type} not found in ServiceLocator.");
            return null;
        }
}

