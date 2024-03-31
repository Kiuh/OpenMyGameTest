using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace App.Scripts.Modules.SceneContainer.ServiceLocator
{
    public class ServiceContainer
    {
        private readonly Dictionary<Type, Container> _containers = new();
        private readonly List<object> _bufferArguments;

        public ServiceContainer()
        {
            _bufferArguments = new List<object>();
        }

        public void SetServiceSelf<TService>(TService value)
            where TService : class
        {
            SetService<TService, TService>(value);
        }

        public void SetService<TBind, TService>(TService value)
            where TService : TBind
            where TBind : class
        {
            Container container = FindContainer<TBind>();

            container.Add(value);
        }

        public void SetServiceInterfaces<TService>(TService value)
            where TService : class
        {
            Type type = typeof(TService);

            foreach (Type interfaceType in type.GetInterfaces())
            {
                Container container = FindContainer(interfaceType);
                container.Add(value);
            }
        }

        public TBind Get<TBind>()
            where TBind : class
        {
            Container container = FindContainer<TBind>();

            return container.GetService<TBind>();
        }

        public IEnumerable<TBind> GetServices<TBind>()
            where TBind : class
        {
            Container container = FindContainer<TBind>();

            return container.GetServices<TBind>();
        }

        public T CreateInstance<T>(object[] arguments = null)
            where T : class
        {
            Type buildType = typeof(T);

            return BuildConstructorArguments(buildType, arguments) as T;
        }

        public T CreateInstanceWithArguments<T>(params object[] arguments)
            where T : class
        {
            Type buildType = typeof(T);

            return BuildConstructorArguments(buildType, arguments) as T;
        }

        private object BuildConstructorArguments(Type type, object[] arguments)
        {
            ConstructorInfo[] constructors = type.GetConstructors();

            foreach (ConstructorInfo constructorInfo in constructors)
            {
                ParameterInfo[] parameters = constructorInfo.GetParameters();

                List<object> resolvedParams = GetResolvedParameters(parameters, arguments);

                if (resolvedParams is null)
                {
                    Debug.LogError(
                        $"Cant resolve params for type {type.FullName} for constructor {constructorInfo}"
                    );
                    continue;
                }

                return constructorInfo.Invoke(resolvedParams.ToArray());
            }

            Debug.LogError($"Cant resolve params for type {type.FullName}");

            return null;
        }

        private List<object> GetResolvedParameters(
            ParameterInfo[] parameterInfos,
            object[] arguments
        )
        {
            List<object> result = new();

            _bufferArguments.Clear();
            if (arguments != null)
            {
                _bufferArguments.AddRange(arguments);
            }

            foreach (ParameterInfo parameterInfo in parameterInfos)
            {
                object argument = FindFromArgument(parameterInfo, _bufferArguments);

                if (argument != null)
                {
                    result.Add(argument);
                    continue;
                }

                object resolveParam = ResolveForType(parameterInfo.ParameterType);

                if (resolveParam is null)
                {
                    Debug.LogError($"Cant resolve type for {parameterInfo.ParameterType}");
                    return null;
                }

                result.Add(resolveParam);
            }

            return result;
        }

        private object FindFromArgument(ParameterInfo parameterInfo, List<object> bufferArguments)
        {
            for (int i = 0; i < bufferArguments.Count; i++)
            {
                object argument = bufferArguments[i];

                if (parameterInfo.ParameterType.IsInstanceOfType(argument))
                {
                    bufferArguments.RemoveAt(i);
                    return argument;
                }
            }

            return null;
        }

        private object ResolveForType(Type resolveType)
        {
            Type selectType = resolveType;

            Type enumerableType = typeof(IEnumerable<>);

            Type genericType = selectType
                .GetInterfaces()
                .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == enumerableType)
                .Select(x => x.GetGenericArguments()[0])
                .FirstOrDefault();

            if (genericType != null)
            {
                return ResolveEnumerable(genericType);
            }

            foreach (KeyValuePair<Type, Container> serviceContainer in _containers)
            {
                if (resolveType.IsAssignableFrom(serviceContainer.Key))
                {
                    return serviceContainer.Value.GetValue();
                }
            }

            return null;
        }

        private IEnumerable<object> ResolveEnumerable(Type genericType)
        {
            foreach (KeyValuePair<Type, Container> serviceContainer in _containers)
            {
                if (genericType.IsAssignableFrom(serviceContainer.Key))
                {
                    return serviceContainer.Value.GetValues();
                }
            }

            return null;
        }

        private Container FindContainer<T>()
            where T : class
        {
            return FindContainer(typeof(T));
        }

        private Container FindContainer(Type typeBind)
        {
            if (_containers.TryGetValue(typeBind, out Container container))
            {
                return container;
            }

            Container bindContainer = new();
            _containers[typeBind] = bindContainer;

            return bindContainer;
        }
    }
}
