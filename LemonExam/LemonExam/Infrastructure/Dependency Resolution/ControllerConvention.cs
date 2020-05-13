using Microsoft.AspNetCore.Mvc;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;
using StructureMap.Pipeline;
using StructureMap.TypeRules;

namespace LemonExam.Infrastructure {
    public class ControllerConvention : IRegistrationConvention {

        #region Public Methods and Operators

        public void ScanTypes(TypeSet types, Registry registry) {
            foreach (var type in types.AllTypes())
                if (type.CanBeCastTo<Controller>() && !type.IsInterfaceOrAbstract())
                    registry.For(type).LifecycleIs(new UniquePerRequestLifecycle());
        }

        #endregion
    }
}
