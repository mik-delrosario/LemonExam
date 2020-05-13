using Microsoft.AspNetCore.Http;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LemonExam.Infrastructure {
    /// <summary>
    /// The structure map dependency scope.
    /// </summary>
    public class StructureMapDependencyScope {
        #region Constants and Fields

        private const string NestedContainerKey = "Nested.Container.Key";

        #endregion

        #region Constructor

        public StructureMapDependencyScope(IContainer container) {
            if (container == null)
                throw new ArgumentNullException("container");

            Container = container;
        }

        #endregion

        #region Public Properties

        public IContainer Container { get; set; }

        public IContainer CurrentNestedContainer { get; set; }
                //return (IContainer)HttpContext.Items[NestedContainerKey];
                //HttpContext.Items[NestedContainerKey] = value;

        #endregion

        #region Private Properties

        //private HttpContextBase HttpContext {
        //    get {
        //        var ctx = Container.TryGetInstance<HttpContextBase>();
        //        return ctx ?? new HttpContextWrapper(System.Web.HttpContext.Current);
        //    }
        //}

        #endregion

        #region Public Methods and Operators

        public void CreateNestedContainer() {
            if (CurrentNestedContainer != null)
                return;

            CurrentNestedContainer = Container.GetNestedContainer();
        }

        public void Dispose() {
            DisposeNestedContainer();
            Container.Dispose();
        }

        public void DisposeNestedContainer() {
            if (CurrentNestedContainer != null) {
                CurrentNestedContainer.Dispose();
                CurrentNestedContainer = null;
            }
        }

        public IEnumerable<object> GetServices(Type type) {
            return DoGetAllInstances(type);
        }

        #endregion

        #region Get Instance Methods

        protected IEnumerable<object> DoGetAllInstances(Type type) {
            return (CurrentNestedContainer ?? Container).GetAllInstances(type).Cast<object>();
        }

        protected object DoGetInstance(Type type, string key) {
            IContainer container = (CurrentNestedContainer ?? Container);

            if (string.IsNullOrEmpty(key))
                return type.GetTypeInfo().IsAbstract || type.GetTypeInfo().IsInterface
                    ? container.TryGetInstance(type)
                    : container.GetInstance(type);

            return container.GetInstance(type, key);
        }

        #endregion
    }
}
