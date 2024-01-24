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
            private Vector2 _size = new Vector2(50, 100);

            private GraphNode CreateNode (ITask task, GraphNodeOptions options = null) {
                if (options == null) options = new GraphNodeOptions();
                options.Size = _size;
                return new GraphNode(task, _printer, options);
            }

            [SetUp]
            public void BeforeEach () {
                _printer = Substitute.For<IGraphNodePrinter>();
            }

            public class DefaultTests : SetPositionMethod {
                [Test]
                public void It_should_set_the_position () {
                    var task = A.TaskStub().Build();
                    var graphNode = CreateNode(task);

                    graphNode.SetPosition(Vector2.one);

                    Assert.AreEqual(Vector2.one, graphNode.Position);
                }


                [Test]
                public void It_should_offset_two_children_by_half_the_width () {
                    var task = A.TaskStub()
                        .SetChildren(new List<ITask> {
                            A.TaskStub().Build(),
                            A.TaskStub().Build(),
                        })
                        .Build();
                    var graphNode = CreateNode(task);
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
                    var graphNode = CreateNode(task);
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
                    var graphNode = CreateNode(task);
                    var childA = graphNode.Children[0];
                    var childB = graphNode.Children[1];
                    var childC = graphNode.Children[2];
                    var childD = graphNode.Children[3];
                    var shiftAmount = graphNode.Size.x;

                    graphNode.SetPosition(Vector2.zero);

                    Assert.AreEqual(new Vector2(graphNode.Position.x - shiftAmount * 1.5f, graphNode.Size.y),
                        childA.Position);
                    Assert.AreEqual(new Vector2(graphNode.Position.x - shiftAmount * 0.5f, graphNode.Size.y),
                        childB.Position);
                    Assert.AreEqual(new Vector2(graphNode.Position.x + shiftAmount * 0.5f, graphNode.Size.y),
                        childC.Position);
                    Assert.AreEqual(new Vector2(graphNode.Position.x + shiftAmount * 1.5f, graphNode.Size.y),
                        childD.Position);
                }
            }

            public class SingleChildTests : SetPositionMethod {
                private ITask _task;
                
                [SetUp]
                public void BeforeEachTest () {
                    _task = A.TaskStub()
                        .SetChildren(new List<ITask> {A.TaskStub().Build()})
                        .Build();
                }
                
                [Test]
                public void It_should_set_the_child_position_directly_below_the_parent () {
                    var graphNode = CreateNode(_task);
                    var child = graphNode.Children[0]; 
                    
                    graphNode.SetPosition(Vector2.zero);

                    Assert.AreEqual(new Vector2(0, graphNode.Size.y), child.Position);
                }

                [Test]
                public void It_should_set_the_child_position_below_a_vertical_connector_bottom () {
                    var graphNode = CreateNode(_task, new GraphNodeOptions {
                        VerticalConnectorBottomHeight = 10,
                    });
                    var child = graphNode.Children[0]; 

                    graphNode.SetPosition(Vector2.zero);

                    Assert.AreEqual(
                        new Vector2(0, graphNode.Size.y + graphNode.VerticalConnectorBottomHeight),
                        child.Position);
                }

                [Test]
                public void It_should_set_the_child_position_below_a_vertical_bottom_divider_plus_horizontal_line () {
                    var graphNode = CreateNode(_task, new GraphNodeOptions {
                        VerticalConnectorBottomHeight = 10,
                        HorizontalConnectorHeight = 1,
                    });
                    var child = graphNode.Children[0]; 
                    
                    graphNode.SetPosition(Vector2.zero);

                    var expectedY = graphNode.Size.y
                                    + graphNode.VerticalConnectorBottomHeight
                                    + graphNode.HorizontalConnectorHeight;

                    Assert.AreEqual(new Vector2(0, expectedY), child.Position);
                }
                
                [Test]
                public void It_should_set_the_child_position_below_a_vertical_connector_top () {
                    var graphNode = CreateNode(_task, new GraphNodeOptions {
                        VerticalConnectorBottomHeight = 10,
                        HorizontalConnectorHeight = 1,
                        VerticalConnectorTopHeight = 3,
                    });
                    var child = graphNode.Children[0]; 
                    
                    graphNode.SetPosition(Vector2.zero);

                    var expectedY = graphNode.Size.y
                                    + graphNode.VerticalConnectorBottomHeight
                                    + graphNode.HorizontalConnectorHeight
                                    + graphNode.VerticalConnectorTopHeight;

                    Assert.AreEqual(new Vector2(0, expectedY), child.Position);
                }
                
                [Test]
                public void It_should_make_the_child_inherit_the_container_size () {
                    var graphNode = CreateNode(_task, new GraphNodeOptions {
                        VerticalConnectorBottomHeight = 10,
                        HorizontalConnectorHeight = 1,
                        VerticalConnectorTopHeight = 3,
                    });
                    var child = graphNode.Children[0]; 

                    Assert.AreEqual(graphNode.ContainerHeight, child.ContainerHeight);
                }
            }
        }
    }
}