namespace CleverCrow.Fluid.BTs.Testing {
    public static class A {
        public static TaskStubBuilder TaskStub () {
            return new TaskStubBuilder();
        }

        public static GraphBoxStubBuilder GraphBoxStub () {
            return new GraphBoxStubBuilder();
        }
    }
}