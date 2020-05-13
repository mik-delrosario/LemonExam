using Microsoft.AspNetCore.Hosting.Server;

namespace LemonExam.Infrastructure {
    public class StructureMapScopeModule {
        #region Public Methods and Operators

        public void Dispose() {
        }

        //public void Init(IHttpApplication context) {
        //    context.BeginRequest += (sender, e) => StructuremapMvc.ParentScope.CreateNestedContainer();
        //    context.EndRequest += (sender, e) => {
        //        HttpContextLifecycle.DisposeAndClearAll();
        //        StructuremapMvc.ParentScope.DisposeNestedContainer();
        //    };
        //}

        #endregion
    }
}
