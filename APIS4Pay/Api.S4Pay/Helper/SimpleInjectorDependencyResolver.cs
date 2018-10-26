using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Api.S4Pay.Helper
{
    public class SimpleInjectorDependencyResolver : IDependencyResolver
    {
        private readonly AsyncScopedLifestyle _scopedLifestyle = new AsyncScopedLifestyle();
        private readonly Container _container;
        private readonly DependencyResolverScopeOption _scopeOption;
        private readonly Scope _scope;

        public SimpleInjectorDependencyResolver(Container container)
            : this(container, DependencyResolverScopeOption.UseAmbientScope)
        {
        }

        public SimpleInjectorDependencyResolver(Container container,
            DependencyResolverScopeOption scopeOption)
            : this(container, beginScope: false)
        {

            if (scopeOption < DependencyResolverScopeOption.UseAmbientScope ||
                scopeOption > DependencyResolverScopeOption.RequiresNew)
            {
                throw new System.ComponentModel.InvalidEnumArgumentException(nameof(scopeOption), (int)scopeOption,
                    typeof(DependencyResolverScopeOption));
            }

            _scopeOption = scopeOption;
        }

        private SimpleInjectorDependencyResolver(Container container, bool beginScope)
        {
            _container = container;
            if (beginScope)
            {
                _scope = AsyncScopedLifestyle.BeginScope(container);
            }
        }

        private IServiceProvider ServiceProvider => _container;

        public void Dispose()
        {
            _scope?.Dispose();
        }

        public object GetService(Type serviceType)
        {
            if (!serviceType.IsAbstract && typeof(IHttpController).IsAssignableFrom(serviceType))
            {
                return _container.GetInstance(serviceType);
            }

            return ServiceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var collectionType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            var services = (IEnumerable<object>)ServiceProvider.GetService(collectionType);
            return services ?? Enumerable.Empty<object>();
        }

        public IDependencyScope BeginScope()
        {
            var beginScope = _scopeOption == DependencyResolverScopeOption.RequiresNew ||
                               _scopedLifestyle.GetCurrentScope(_container) == null;

            return new SimpleInjectorDependencyResolver(_container, beginScope);
        }
    }

    public enum DependencyResolverScopeOption
    {
        UseAmbientScope,
        RequiresNew
    }
}
