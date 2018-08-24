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

### What a returned TaskStatus does

Depending on what you return for a task status different things will happen.

* Success: Node has finished, next `tree.Tick()` will restart the tree if no other nodes to run
* Failure: Same as success, except informs that the node failed
* Continue: Rerun this node the next time `tree.Tick()` is called. A pointer reference is tracked by the tree and can only be cleared if `tree.Reset()` is called.

### WTF is a Behavior Tree?

If you aren't super familiar with Behavior Trees you might want to watch this video.

https://www.youtube.com/watch?v=YCMvUCxzWz8

## Example Scene

You might want to look at the capture the flag example project located at `/Assets/FluidBehaviorTree/Examples/CaptureTheFlag/CaptureTheFlag.unity` 
for a working example of how Fluid Behavior Tree can be used in your project. It demonstrates real time usage of this library
with units who will attempt to capture the flag while grabbing power ups to try and gain the upper hand.

## Library

Fluid Behavior Tree comes with a robust library of pre-made actions, conditions, composites, and other nodes
to help speed up your development process.

### Actions

#### Generic

You can create a generic action on the fly. If you find yourself re-using the same actions you might want to
look into the section on writing your own custom actions.

```C#
.Sequence()
    .Do("Custom Action", () => {
        return TaskStatus.Success;
    })
.End()
```

#### Wait

Skip a number of ticks on the Behavior Tree.

```C#
.Sequence()
    // Wait for 1 tick on the tree before continuing
    .Wait(1)
    .Do(MyAction)
.End()
```

### Conditions

#### Generic

You can create a generic condition on the fly. If you find yourself re-using the same actions you might want to
look into the section on writing your own custom conditions.

```C#
.Sequence()
    .Condition("Custom Condtion", () => {
        return true;
    })
    .Do(MyAction)
.End()
```

#### Random Chance

Randomly evaluate a node as true or false based upon the passed chance.

```C#
.Sequence()
    // 50% chance this will return success
    .RandomChance(1, 2)
    .Do(MyAction)
.End()
```

### Composites

#### Sequence

Runs each child node in order and expects a *Success* status to tick the next node. If *Failure* is returned, the sequence will stop executing child nodes and return *Failure* to the parent.

```C#
.Sequence()
    .Do(() => { return TaskStatus.Success; })
    .Do(() => { return TaskStatus.Success; })
    
    // All tasks after this will not run and the sequence will exit
    .Do(() => { return TaskStatus.Failure; })

    .Do(() => { return TaskStatus.Success; })
.End()
```

#### Selector

Runs each child node until *Success* is returned.

```C#
.Selector()
    // Runs but fails
    .Do(() => { return TaskStatus.Failure; })

    // Will stop here since the node returns success
    .Do(() => { return TaskStatus.Success; })
    
    // Does not run
    .Do(() => { return TaskStatus.Success; })
.End()
```

#### Parallel

Runs all child nodes at the same time until they all return *Success*. Exits and stops all running nodes if ANY of them return *Failure*.

```C#
.Parallel()
    // Both of these tasks will run every frame
    .Do(() => { return TaskStatus.Continue; })
    .Do(() => { return TaskStatus.Continue; })
.End()
```

### Decorators

#### Generic

#### Inverter

#### ReturnSuccess

#### ReturnFailure

## Creating your own custom nodes

### Actions

### Conditions

### Composites

### Decorators

## Submitting your own actions, conditions, ect

Please fill out the following details if you'd like to contribute new code to this project.

1. Clone this project for the core code with tests
2. Make sure your new code is reasonably tested to demonstrate it works (see `*Test.cs` files)
3. Submit a PR
