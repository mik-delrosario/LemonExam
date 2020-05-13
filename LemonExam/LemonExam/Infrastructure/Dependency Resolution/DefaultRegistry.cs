using LemonExam;
using LemonExam.Model;
using StructureMap;

namespace LemonExam.Infrastructure {
    public class DefaultRegistry : Registry {

        public DefaultRegistry() {
            Scan(
                scan => {
                    scan.AssemblyContainingType<Startup>();
                    scan.LookForRegistries();
                    scan.AssemblyContainingType<DefaultRegistry>();
                    scan.With(new ControllerConvention());
                });
            For<IDependencyResolver>().Use<StructureMapDependencyResolver>();
            //For<HttpContextBase>().Use(() => new HttpContextWrapper(HttpContext.Current));
            //For<ModelValidatorProvider>().Use<FluentValidationModelValidatorProvider>();
            //For<IValidatorFactory>().Use<StructureMapValidatorFactory>();
            //For<ModelMetadataProvider>().Use<CustomDataAnnotationsModelMetadataProvider>();

            //DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
        }
    }
}
