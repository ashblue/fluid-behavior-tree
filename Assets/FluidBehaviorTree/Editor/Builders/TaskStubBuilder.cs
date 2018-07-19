using Adnc.FluidBT.Tasks;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace Adnc.FluidBT.Testing {
    public class TaskStubBuilder {
        private bool _enabled = true;
        private TaskStatus _status = TaskStatus.Success;

        public TaskStubBuilder WithEnabled (bool enabled) {
            _enabled = enabled;
            
            return this;
        }

        public TaskStubBuilder WithUpdateStatus (TaskStatus status) {
            _status = status;

            return this;
        }

        public ITask Build () {
            var task = Substitute.For<ITask>();
            task.Enabled.Returns(_enabled);
            task.Update().ReturnsForAnyArgs(_status);

            return task;
        }
    }
}