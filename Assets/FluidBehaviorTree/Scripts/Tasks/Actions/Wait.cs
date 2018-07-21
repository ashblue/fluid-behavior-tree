namespace Adnc.FluidBT.Tasks.Actions {
    public class Wait : ActionBase {
        public int turns = 1;
        
        private int _ticks;
        
        protected override TaskStatus OnUpdate () {
            if (_ticks < turns) {
                _ticks++;
                return TaskStatus.Continue;                
            }

            return TaskStatus.Success;
        }
    }
}
