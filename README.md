# fluid-behavior-tree

A pure code behavior tree micro-framework built for Unity3D projects. 
Granting developers the power to dictate their GUI presentation.
Inspired by Fluent Behavior Tree.

@TODO Table of contents

## Getting Started

Grab the latest `*.unitypackage` from the [releases page](https://github.com/ashblue/fluid-behavior-tree/releases).

### Creating a Behavior Tree

When creating trees you'll need to store them in a variable to properly cache all the necessary data.

```C#
public class MyCustomAi : MonoBehaviour {
    private BehaviorTree _tree;
    
    private void Awake () {
        _tree = new BehaviorTreeBuilder(gameObject)
            .Sequence()
                .Condition("Custom Condition", () => {
                    return true;
                })
                .Do("Custom Action", () => {
                    return TaskStatus.Success;
                })
            .End()
            .Build();
    }

    private void Update () {
        // Update our tree every frame
        _tree.Tick();
    }
}
```

### WTF is a Behavior Tree?

If you aren't super familiar with Behavior Trees you might want to watch these videos.

### What a returned TaskStatus does

Depending on what you return for a task status different things will happen.

* Success: Node has finished, next `tree.Tick()` will restart the tree if no other nodes to run
* Failure: Same as success, except informs that the node failed
* Continue: Rerun this node the next time `tree.Tick()` is called. A pointer reference is tracked by the tree and can only be cleared if `tree.Reset()` is called.

## Example Scene

@TODO Document example scene

## Library

Fluid Behavior Tree comes with a robust library of pre-made actions, conditions, composites, and other nodes
to help speed up your development process.

@TODO Node library docs go here

### Actions

### Conditions

### Composites

### Decorators

## Modifying the TreeBuilder code

## Creating your own custom nodes

### Actions

### Conditions

### Composites

### Decorators

## Submitting your own actions, conditions, ect

@TODO Instructions for submitting your own stuff
