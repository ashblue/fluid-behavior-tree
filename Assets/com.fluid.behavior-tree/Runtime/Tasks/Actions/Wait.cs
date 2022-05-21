using System.IO;

namespace CleverCrow.Fluid.BTs.Tasks.Actions
{
    public class Wait : ActionBase
    {

        public override string IconPath => $"{PackageRoot}{Path.DirectorySeparatorChar}HourglassFill.png";

        // TODO: Convert into property
        public int Turns = 1;

        private int _ticks;

        protected override void OnStart()
        {
            _ticks = 0;
        }

        protected override TaskStatus OnUpdate()
        {
            if (_ticks >= Turns)
            {
                return TaskStatus.Success;
            }

            _ticks++;
            return TaskStatus.Continue;

        }
    }
}
