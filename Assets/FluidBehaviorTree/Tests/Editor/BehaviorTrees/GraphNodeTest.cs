using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors.Testing {
    public class GraphNodeTest {
        public class SetPositionMethod {
            private IGraphNodePrinter _printer;

            [SetUp]
            public void Setup () {
                _printer = Substitute.For<IGraphNodePrinter>();
            }
            
            [Test]
            public void It_should_set_the_position () {
                var task = A.TaskStub().Build();
                var graphNode = new GraphNode(task, Vector2.one, _printer);

                graphNode.SetPosition(Vector2.one);
                
                Assert.AreEqual(Vector2.one, graphNode.Position);
            }
            
            [Test]
            public void It_should_set_the_child_position_directly_below_the_parent () {
                var task = A.TaskStub()
                    .SetChildren(new List<ITask>{A.TaskStub().Build()})
                    .Build();
                var graphNode = new GraphNode(task, new Vector2(50, 100), _printer);
                var child = graphNode.Children[0];

                graphNode.SetPosition(Vector2.zero);
                
                Assert.AreEqual(new Vector2(0, graphNode.Size.y), child.Position);
            }
            
            [Test]
            public void It_should_offset_two_children_by_half_the_width () {
                var task = A.TaskStub()
                    .SetChildren(new List<ITask> {
                        A.TaskStub().Build(),
                        A.TaskStub().Build(),
                    })
                    .Build();
                var graphNode = new GraphNode(task, new Vector2(50, 100), _printer);
                var childA = graphNode.Children[0];
                var childB = graphNode.Children[1];
                var shiftAmount = graphNode.Size.x * 0.5f;
                
                graphNode.SetPosition(Vector2.zero);
                
                Assert.AreEqual(new Vector2(graphNode.Position.x - shiftAmount, graphNode.Size.y), childA.Position);
                Assert.AreEqual(new Vector2(graphNode.Position.x + shiftAmount, graphNode.Size.y), childB.Position);
            }
            
            [Test]
            public void It_should_offset_three_children_to_line_up_in_the_middle () {
                var task = A.TaskStub()
                    .SetChildren(new List<ITask> {
                        A.TaskStub().Build(),
                        A.TaskStub().Build(),
                        A.TaskStub().Build(),
                    })
                    .Build();
                var graphNode = new GraphNode(task, new Vector2(50, 100), _printer);
                var childA = graphNode.Children[0];
                var childB = graphNode.Children[1];
                var childC = graphNode.Children[2];
                var shiftAmount = graphNode.Size.x * 1f;
                
                graphNode.SetPosition(Vector2.zero);
                
                Assert.AreEqual(new Vector2(graphNode.Position.x - shiftAmount, graphNode.Size.y), childA.Position);
                Assert.AreEqual(new Vector2(graphNode.Position.x, graphNode.Size.y), childB.Position);
                Assert.AreEqual(new Vector2(graphNode.Position.x + shiftAmount, graphNode.Size.y), childC.Position);
            }
            
            [Test]
            public void It_should_offset_four_children_to_line_up_in_the_middle () {
                var task = A.TaskStub()
                    .SetChildren(new List<ITask> {
                        A.TaskStub().Build(),
                        A.TaskStub().Build(),
                        A.TaskStub().Build(),
                        A.TaskStub().Build(),
                    })
                    .Build();
                var graphNode = new GraphNode(task, new Vector2(50, 100), _printer);
                var childA = graphNode.Children[0];
                var childB = graphNode.Children[1];
                var childC = graphNode.Children[2];
                var childD = graphNode.Children[3];
                var shiftAmount = graphNode.Size.x;
                
                graphNode.SetPosition(Vector2.zero);
                
                Assert.AreEqual(new Vector2(graphNode.Position.x - shiftAmount * 1.5f, graphNode.Size.y), childA.Position);
                Assert.AreEqual(new Vector2(graphNode.Position.x - shiftAmount * 0.5f, graphNode.Size.y), childB.Position);
                Assert.AreEqual(new Vector2(graphNode.Position.x + shiftAmount * 0.5f, graphNode.Size.y), childC.Position);
                Assert.AreEqual(new Vector2(graphNode.Position.x + shiftAmount * 1.5f, graphNode.Size.y), childD.Position);
            }
        }
    }
}
