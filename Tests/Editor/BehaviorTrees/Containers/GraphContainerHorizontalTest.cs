using CleverCrow.Fluid.BTs.Testing;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class GraphContainerHorizontalTest {
        private GraphContainerHorizontal _container;
        private IGraphBox _childBox;

        [SetUp]
        public void BeforeEach () {
            _container = new GraphContainerHorizontal();
            _childBox = A.GraphBoxStub().WithSize(100, 50).Build();
        }
        
        public class AddBoxMethod {
            public class DefaultTests : GraphContainerHorizontalTest {
                [Test]
                public void It_should_add_a_child_container () {
                    _container.AddBox(_childBox);
                
                    Assert.IsTrue(_container.ChildContainers.Contains(_childBox));
                }

                [Test]
                public void It_should_set_the_local_position_of_a_child_box () {
                    _container.AddBox(_childBox);

                    _childBox.Received(1).SetLocalPosition(0, 0);
                }
            
                [Test]
                public void It_should_stack_multiple_child_containers_side_by_side () {
                    for (var i = 0; i < 3; i++) {
                        var child = A.GraphBoxStub().WithSize(100, 50).Build();
                        _container.AddBox(child);
                    }
                
                    for (var i = 0; i < _container.ChildContainers.Count; i++) {
                        var child = _container.ChildContainers[i];
                        child.Received(1).SetLocalPosition(100f * i, 0);
                    }
                }

                [Test]
                public void It_should_update_Width_when_a_new_child_container_is_added () {
                    _container.AddBox(_childBox);

                    Assert.AreEqual(100f, _container.Width);
                }
            }

            public class SettingHeight : GraphContainerHorizontalTest {
                [Test]
                public void It_should_set_the_height_to_the_child () {
                    _container.AddBox(_childBox);

                    Assert.AreEqual(50, _container.Height);
                }
            
                [Test]
                public void It_should_set_the_Height_if_the_next_child_is_larger () {
                    var childBoxAlt = A.GraphBoxStub().WithSize(0, 100).Build();
                
                    _container.AddBox(_childBox);
                    _container.AddBox(childBoxAlt);

                    Assert.AreEqual(100, _container.Height);
                }
            
                [Test]
                public void It_should_not_set_the_Height_if_the_next_child_is_smaller () {
                    var childBoxAlt = A.GraphBoxStub().WithSize(0, 20).Build();
                
                    _container.AddBox(_childBox);
                    _container.AddBox(childBoxAlt);

                    Assert.AreEqual(50, _container.Height);
                }
            }

            public class ItShouldUpdateGlobalPositioning : GraphContainerHorizontalTest {
                [Test]
                public void On_added_boxes () {
                    _container.SetGlobalPosition(50, 100);
                
                    _container.AddBox(_childBox);

                    _childBox.Received(1).AddGlobalPosition(50, 100);
                }
                
                [Test]
                public void On_nested_positions_on_added_boxes () {
                    var box = new GraphBox();
                    box.SetSize(50, 50);
                
                    var nestedContainer = new GraphContainerHorizontal();
                    nestedContainer.AddBox(box);

                    _container.SetGlobalPosition(50, 100);
                    _container.AddBox(nestedContainer);

                    Assert.AreEqual(100, box.GlobalPositionY);
                }
            
                [Test]
                public void On_nested_child_positions_on_added_boxes () {
                    var boxA = new GraphBox();
                    boxA.SetSize(50, 50);
                
                    var boxB = new GraphBox();
                    boxB.SetSize(50, 50);

                    var nestedContainer = new GraphContainerHorizontal();
                    nestedContainer.AddBox(boxA);
                    nestedContainer.AddBox(boxB);

                    _container.SetGlobalPosition(50, 100);
                    _container.AddBox(nestedContainer);

                    Assert.AreEqual(100, boxB.GlobalPositionX);
                }
            
                [Test]
                public void On_deeply_nested_child_positions_on_added_boxes () {
                    var boxA = new GraphBox();
                    boxA.SetSize(50, 50);
                
                    var boxB = new GraphBox();
                    boxB.SetSize(50, 50);

                    var nestedContainer = new GraphContainerHorizontal();
                    nestedContainer.SetGlobalPosition(100, 50);
                    nestedContainer.AddBox(boxA);
                    nestedContainer.AddBox(boxB);

                    _container.SetGlobalPosition(50, 100);
                    _container.AddBox(nestedContainer);

                    Assert.AreEqual(200, boxB.GlobalPositionX);
                }
            }
        }

        public class CenterAlignChildrenMethod : GraphContainerHorizontalTest {
            [Test]
            public void It_should_align_a_child_in_the_center () {
                _childBox.Width.Returns(50);
                
                var children = new GraphContainerHorizontal();
                children.AddBox(A.GraphBoxStub().WithSize(50, 50).Build());
                children.AddBox(A.GraphBoxStub().WithSize(50, 50).Build());
                
                var wrapper = new GraphContainerVertical();
                wrapper.AddBox(_childBox);
                wrapper.AddBox(children);
                
                _container.AddBox(wrapper);
                _container.CenterAlignChildren();
                
                _childBox.Received(1).AddGlobalPosition(25, 0);
            }
        }
    }

    public class BreakingExampleCases {
        [Test]
        public void Root_should_push_down_the_next_node () {
            var tree = new GraphContainerVertical();
            tree.SetGlobalPosition(300, 0);
            var nodeWrapper = new GraphContainerVertical();
            var box = new GraphBox();
            box.SetSize(50, 50);
            var childrenContainer = new GraphContainerHorizontal();
            
            var childWrapper = new GraphContainerVertical();
            var childBox = new GraphBox();
            childBox.SetSize(50, 50);

            nodeWrapper.AddBox(box);
            Assert.AreEqual(50, nodeWrapper.Height);
            Assert.AreEqual(50, nodeWrapper.Width);
            Assert.AreEqual(0, box.GlobalPositionY);

            childWrapper.AddBox(childBox);
            Assert.AreEqual(50, nodeWrapper.Height);
            Assert.AreEqual(50, nodeWrapper.Width);

            childrenContainer.AddBox(childWrapper);
            Assert.AreEqual(50, nodeWrapper.Height);
            Assert.AreEqual(50, nodeWrapper.Width);
            Assert.AreEqual(0, childrenContainer.GlobalPositionY);

            nodeWrapper.AddBox(childrenContainer);
            Assert.AreEqual(100, nodeWrapper.Height);
            Assert.AreEqual(50, nodeWrapper.Width);
            Assert.AreEqual(50, childrenContainer.GlobalPositionY);

            tree.AddBox(nodeWrapper);
            Assert.AreEqual(0, nodeWrapper.LocalPositionY);
            Assert.AreEqual(0, nodeWrapper.GlobalPositionY);
            Assert.AreEqual(50, childrenContainer.GlobalPositionY);
            Assert.AreEqual(0, box.GlobalPositionY);
        }
    }
}