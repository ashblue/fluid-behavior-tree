namespace CleverCrow.Fluid.BTs.Tasks.Actions {
    public class Wait : ActionBase {
        public int turns = 1;
        
        private int _ticks;

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
