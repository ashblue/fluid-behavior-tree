using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class TreeContainerHorizontalTest {
        private TreeContainerHorizontal _container;
        private ITreeBox _childBox;

        [SetUp]
        public void BeforeEach () {
            _container = new TreeContainerHorizontal();
            _childBox = Substitute.For<ITreeBox>();
        }
        
        public class AddBoxMethod : TreeContainerHorizontalTest {
            [Test]
            public void It_should_add_a_child_container () {
                _container.AddBox(_childBox);
                
                Assert.IsTrue(_container.ChildContainers.Contains(_childBox));
            }

            [Test]
            public void It_should_set_the_local_position_of_the_child_container () {
                _childBox.Width.Returns(100);
                _childBox.Height.Returns(50);
                
                _container.AddBox(_childBox);

                _childBox.Received(1).SetLocalPosition(0, 0);
            }
            
            [Test]
            public void It_should_stack_multiple_child_containers_side_by_side () {
                for (var i = 0; i < 3; i++) {
                    var child = Substitute.For<ITreeContainer>();
                    child.Width.Returns(100);
                    child.Height.Returns(50);
                    _container.AddBox(child);
                }
                
                for (var i = 0; i < _container.ChildContainers.Count; i++) {
                    var child = _container.ChildContainers[i];
                    child.Received(1).SetLocalPosition(100f * i, 0);
                }
            }

            [Test]
            public void It_should_update_Width_when_a_new_child_container_is_added () {
                _childBox.Width.Returns(100);
                _childBox.Height.Returns(50);
                
                _container.AddBox(_childBox);

                Assert.AreEqual(100f, _container.Width);
            }
            
            [Test]
            public void It_should_update_global_position_on_added_boxes () {
                _container.SetGlobalPosition(50, 100);
                
                _container.AddBox(_childBox);

                _childBox.Received(1).SetGlobalPosition(50, 100);
            }

            [Test]
            public void It_should_set_the_height_to_the_child () {
                _childBox.Height.Returns(50);
                
                _container.AddBox(_childBox);

                Assert.AreEqual(50, _container.Height);
            }
            
            [Test]
            public void It_should_set_the_Height_if_the_next_child_is_larger () {
                _childBox.Height.Returns(50);
                var childBoxAlt = Substitute.For<ITreeBox>();
                childBoxAlt.Height.Returns(100);
                
                _container.AddBox(_childBox);
                _container.AddBox(childBoxAlt);

                Assert.AreEqual(100, _container.Height);
            }
            
            [Test]
            public void It_should_not_set_the_Height_if_the_next_child_is_smaller () {
                _childBox.Height.Returns(50);
                var childBoxAlt = Substitute.For<ITreeBox>();
                childBoxAlt.Height.Returns(20);
                
                _container.AddBox(_childBox);
                _container.AddBox(childBoxAlt);

                Assert.AreEqual(50, _container.Height);
            }
            
            [Test]
            public void Should_resize_parent_when_resizing () {
                var containerParent = new TreeContainerHorizontal();
                containerParent.AddBox(_container);

                _childBox.Width.Returns(100);
                _childBox.Height.Returns(50);
                
                _container.AddBox(_childBox);
                
                Assert.AreEqual(100, containerParent.Width);
                Assert.AreEqual(50, containerParent.Height);
            }

            [Test]
            public void Should_resize_nested_parent_when_resizing () {
                var containerParent = new TreeContainerHorizontal();
                containerParent.AddBox(_container);
                
                var containerParentTop = new TreeContainerHorizontal();
                containerParentTop.AddBox(containerParent);

                _childBox.Width.Returns(100);
                _childBox.Height.Returns(50);
                
                _container.AddBox(_childBox);
                
                Assert.AreEqual(100, containerParentTop.Width);
                Assert.AreEqual(50, containerParentTop.Height);
            }
        }

        public class SetGlobalPositionMethod : TreeContainerHorizontalTest {
            [Test]
            public void It_should_set_child_positions () {
                _container.AddBox(_childBox);
                _container.SetGlobalPosition(100, 200);

                _childBox.Received(1).SetGlobalPosition(100, 200);
            }
            
            [Test]
            public void It_should_shift_child_positions_with_size () {
                _childBox.Width.Returns(100);
                _childBox.Height.Returns(50);
                _childBox.GlobalPositionX.Returns(100);
                _childBox.GlobalPositionY.Returns(50);
                
                _container.AddBox(_childBox);
                _container.SetGlobalPosition(100, 200);

                _childBox.Received(1).SetGlobalPosition(200, 250);
            }
        }
    }

    public class BreakingExampleCases {
        [Test]
        public void Root_should_push_down_the_next_node () {
            var tree = new TreeContainerVertical();
            var nodeWrapper = new TreeContainerVertical();
            var box = new TreeBox();
            box.SetSize(50, 50);
            var childrenContainer = new TreeContainerHorizontal();
            
            var childWrapper = new TreeContainerVertical();
            var childBox = new TreeBox();
            childBox.SetSize(50, 50);
            
            tree.AddBox(nodeWrapper);
            nodeWrapper.AddBox(box);
            nodeWrapper.AddBox(childrenContainer);
            childrenContainer.AddBox(childWrapper);
            childWrapper.AddBox(childBox);
            
            Assert.AreEqual(100, nodeWrapper.Height);
            Assert.AreEqual(50, childrenContainer.LocalPositionY);
            Assert.AreEqual(50, childrenContainer.GlobalPositionY);
            Assert.AreEqual(50, childBox.GlobalPositionY);
        }
    }
}