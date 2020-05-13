using StructureMap;

namespace LemonExam.Infrastructure {
    public static class IoC {
        public static IContainer Initialize() {
            return new Container(c => c.AddRegistry<DefaultRegistry>());
        }
    }
}
