using Adnc.FluidBT.Tasks;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace Adnc.FluidBT.Testing {
    public class TaskStubBuilder {
        private bool _enabled = true;
        private TaskStatus _status = TaskStatus.Success;
        private bool _abortConditionSelf;

        public TaskStubBuilder WithEnabled (bool enabled) {
            _enabled = enabled;
            
            return this;
        }

        public TaskStubBuilder WithUpdateStatus (TaskStatus status) {
            _status = status;

            return this;
        }

        public TaskStubBuilder WithAbortConditionSelf (bool abortCondition) {
            _abortConditionSelf = abortCondition;

            return this;
        }
        
        public ITask Build () {
            var task = Substitute.For<ITask>();
            task.Enabled.Returns(_enabled);
            task.Update().Returns(_status);

            if (_abortConditionSelf) {
                task.GetAbortCondition().Returns(task);
            } else {
                task.GetAbortCondition().ReturnsNull();
            }

            return task;
        }
    }
}