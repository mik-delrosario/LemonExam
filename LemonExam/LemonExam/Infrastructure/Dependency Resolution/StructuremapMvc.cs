using StructureMap;

namespace LemonExam.Infrastructure {
    public class StructuremapMvc {
        #region Public Properties

        public static StructureMapDependencyScope ParentScope { get; set; }

        #endregion

        #region Public Methods and Operators

        public static void End() {
            ParentScope.Dispose();
        }

        public static void Start() {
            IContainer container = IoC.Initialize();
            ParentScope = new StructureMapDependencyScope(container);
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));
            //DynamicModuleUtility.RegisterModule(typeof(StructureMapScopeModule));
        }

        #endregion
    }
}
