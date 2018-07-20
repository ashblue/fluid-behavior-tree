using Adnc.FluidBT.TaskParents.Composites;
using Adnc.FluidBT.Tasks;
using NUnit.Framework;

namespace Adnc.FluidBT.Trees.Testing {
    public class BehaviorTreeBuilderTest {
        public class SequenceMethod {
            private int _invokeCount;
            private BehaviorTreeBuilder _builder;
            
            [SetUp]
            public void BeforeEach () {
                _invokeCount = 0;
                _builder = new BehaviorTreeBuilder(null);
            }
            
            [Test]
            public void Create_a_sequence () {
                var tree = _builder
                    .Sequence("sequence")
                        .Do("action", () => {
                            _invokeCount++;
                            return TaskStatus.Success;
                        })
                    .Build();
    
                var sequence = tree.Root.Children[0] as Sequence;
                
                Assert.AreEqual(tree.Root.Children.Count, 1);
                Assert.AreEqual(sequence.Children.Count, 1);
                Assert.AreEqual(TaskStatus.Success, tree.Tick());
                Assert.AreEqual(1, _invokeCount);
            }
            
            [Test]
            public void Create_a_nested_sequence () {
                var tree = _builder
                    .Sequence("sequence")
                        .Do("action", () => {
                            _invokeCount++;
                            return TaskStatus.Success;
                        })
                        .Sequence("nested")
                            .Do("actionNested", () => {
                                _invokeCount++;
                                return TaskStatus.Success;
                            })
                    .Build();
    
                var sequence = tree.Root.Children[0] as Sequence;
                Assert.AreEqual(2, sequence.Children.Count);
                
                var nested = sequence.Children[1] as Sequence;
                Assert.AreEqual(nested.Children.Count, 1);
                Assert.AreEqual(TaskStatus.Success, tree.Tick());
                Assert.AreEqual(2, _invokeCount);
            }
            
            [Test]
            public void Create_a_nested_sequence_then_add_an_action_to_the_parent () {
                var tree = _builder
                    .Sequence("sequence")
                        .Do("action", () => {
                            _invokeCount++;
                            return TaskStatus.Success;
                        })
                        .Sequence("nested")
                            .Do("actionNested", () => {
                                _invokeCount++;
                                return TaskStatus.Success;
                            })
                        .End()
                        .Do("lastAction", () => {
                        _invokeCount++;
                            return TaskStatus.Success;
                        })
                    .Build();
    
                var sequence = tree.Root.Children[0] as Sequence;
                Assert.AreEqual(3, sequence.Children.Count);
                
                var nested = sequence.Children[1] as Sequence;
                Assert.AreEqual(nested.Children.Count, 1);
                Assert.AreEqual(TaskStatus.Success, tree.Tick());
                Assert.AreEqual(3, _invokeCount);
            }
    
            [Test]
            public void Create_two_nested_sequences_with_actions () {
                var tree = _builder
                    .Sequence("sequence")
                        .Sequence("nested")
                            .Do("actionNested", () => {
                                _invokeCount++;
                                return TaskStatus.Success;
                            })
                        .End()
                        .Sequence("nested")
                            .Do("actionNested", () => {
                                _invokeCount++;
                                return TaskStatus.Success;
                            })
                    .Build();
    
                var sequence = tree.Root.Children[0] as Sequence;
                Assert.AreEqual(2, sequence.Children.Count);
                
                var nested = sequence.Children[0] as Sequence;
                Assert.AreEqual(nested.Children.Count, 1);
                
                var nestedAlt = sequence.Children[1] as Sequence;
                Assert.AreEqual(nestedAlt.Children.Count, 1);
                
                Assert.AreEqual(TaskStatus.Success, tree.Tick());
                Assert.AreEqual(2, _invokeCount);
            }
        }
    }
}