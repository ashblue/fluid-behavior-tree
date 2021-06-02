namespace CleverCrow.Fluid.BTs.Tasks.Actions {
    public class Wait : ActionBase {
        public int turns = 1;
        
        private int _ticks;
        
        public override string IconPath { get; } = $"{PACKAGE_ROOT}/HourglassFill.png";

        protected override void OnStart () {
            _ticks = 0;
        }

        protected override TaskStatus OnUpdate () {
            if (_ticks < turns) {
                _ticks++;
                return TaskStatus.Continue;                
            }

            return TaskStatus.Success;
        }
    }
}
