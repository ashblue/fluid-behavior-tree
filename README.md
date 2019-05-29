# fluid-behavior-tree

A pure code behavior tree micro-framework built for Unity3D projects. 
Granting developers the power to dictate their GUI presentation.
Inspired by Fluent Behavior Tree.

**Features**

* Extendable, write your own custom re-usable nodes
* Pre-built library of tasks to kickstart your AI
* Heavily tested with TDD and unit tests
* Tracks the last position of your behavior tree and restores it the next frame
* Built for Unity (no integration overhead)

## Support

Join the [Discord Community](https://discord.gg/8QHFfzn) if you have questions or need help.

## Getting Started

Grab the latest `*.unitypackage` from the [releases page](https://github.com/ashblue/fluid-behavior-tree/releases).

### Creating a Behavior Tree

When creating trees you'll need to store them in a variable to properly cache all the necessary data.

```C#
using UnityEngine;
using Adnc.FluidBT.Tasks;
using Adnc.FluidBT.Trees;

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

### What a Returned TaskStatus Does

Depending on what you return for a task status different things will happen.

* Success: Node has finished, next `tree.Tick()` will restart the tree if no other nodes to run
* Failure: Same as success, except informs that the node failed
* Continue: Rerun this node the next time `tree.Tick()` is called. A pointer reference is tracked by the tree and can only be cleared if `tree.Reset()` is called.

### WTF is a Behavior Tree?

If you aren't super familiar with behavior trees you might want to watch this video.

https://www.youtube.com/watch?v=YCMvUCxzWz8

## Table of Contents

  * [Getting Started](#getting-started)
  * [Example Scene](#example-scene)
  * [Library](#library)
    + [Actions](#actions)
      - [Generic](#action-generic)
      - [Wait](#wait)
    + [Conditions](#conditions)
      - [Generic](#condition-generic)
      - [Random Chance](#random-chance)
    + [Composites](#composites)
      - [Sequence](#sequence)
      - [Selector](#selector)
      - [Parallel](#parallel)
    + [Decorators](#decorators)
      - [Generic](#decorator-generic)
      - [Inverter](#inverter)
      - [ReturnSuccess](#returnsuccess)
      - [ReturnFailure](#returnfailure)
  * [Creating Reusable Behavior Trees](#creating-reusable-behavior-trees)
  * [Creating Custom Reusable Nodes](#creating-custom-reusable-nodes)
    + [Your First Custom Node and Tree](#your-first-custom-node-and-tree)
    + [Custom Actions](#custom-actions)
    + [Custom Conditions](#custom-conditions)
    + [Custom Composites](#custom-composites)
    + [Custom Decorators](#custom-decorators)
  * [Submitting your own actions, conditions, ect](#submitting-code-to-this-project)

## Example Scene

You might want to look at the capture the flag example project `/Assets/FluidBehaviorTree/Examples/CaptureTheFlag/CaptureTheFlag.unity` 
for a working example of how Fluid Behavior Tree can be used in your project. It demonstrates real time usage
with units who attempt to capture the flag while grabbing power ups to try and gain the upper hand.

## Library

Fluid Behavior Tree comes with a robust library of pre-made actions, conditions, composites, and other nodes
to help speed up your development process.

### Actions

#### Action Generic

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

Skip a number of ticks on the behavior tree.

```C#
.Sequence()
    // Wait for 1 tick on the tree before continuing
    .Wait(1)
    .Do(MyAction)
.End()
```

### Conditions

#### Condition Generic

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

**NOTE** It's important that every composite is followed by a `.End()` statement. This makes sure that your nodes
are properly nested when the tree is built.

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

Decorators are parent elements that wrap any node to change the return value (or execute special logic). They are
extremely powerful and a great compliment to actions, conditions, and composites.

#### Decorator Generic

You can wrap any node with your own custom decorator code. This allows you to customize re-usable functionality.

**NOTE**: You must manually call `Update()` on the child node or it will not fire. Also every decorator must be followed
by a `.End()` statement. Otherwise the tree will not build correctly.

```C#
.Sequence()
    .Decorator("Return Success", child => {
        child.Update();
        return TaskStatus.Success;
    })
        .Do(() => { return TaskStatus.Failure; })
    .End()
    .Do(() => { return TaskStatus.Success; })
.End()
```

#### Inverter

Reverse the returned status of the child node if it's `TaskStatus.Success` or `TaskStatus.Failure`. 
Does not change `TaskStatus.Continue`.

```C#
.Sequence()
    .Inverter()
        .Do(() => { return TaskStatus.Success; })
    .End()
.End()
```

#### ReturnSuccess

Return `TaskStatus.Success` if the child returns `TaskStatus.Failure`.
Does not change `TaskStatus.Continue`.

```C#
.Sequence()
    .ReturnSuccess()
        .Do(() => { return TaskStatus.Failure; })
    .End()
.End()
```

#### ReturnFailure

Return `TaskStatus.Failure` if the child returns `TaskStatus.Success`.
Does not change `TaskStatus.Continue`.

```C#
.Sequence()
    .ReturnFailure()
        .Do(() => { return TaskStatus.Success; })
    .End()
.End()
```

## Creating Reusable Behavior Trees

Trees can be combined with just a few line of code. This allows you to create injectable behavior trees that bundles different
nodes for complex functionality such as searching or attacking.

Be warned that spliced trees require a newly built tree for injection, as nodes are only deep copied on `.Build()`.

```C#
using Adnc.FluidBT.Trees;
using Adnc.FluidBT.Tasks;
using UnityEngine;

public class MyCustomAi : MonoBehaviour {
    private BehaviorTree _tree;
    
    private void Awake () {
        var injectTree = new BehaviorTreeBuilder(gameObject)
            .Sequence()
                .Do("Custom Action", () => {
                    return TaskStatus.Success;
                })
            .End();

        _tree = new BehaviorTreeBuilder(gameObject)
            .Sequence()
                .Splice(injectTree.Build())
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

## Creating Custom Reusable Nodes

What makes Fluid Behavior Tree so powerful is the ability to write your own nodes and extend the tree builder to inject 
custom parameters. For example we can write a new tree builder method like this that sets the target of your AI system.

```C#
var tree = new TreeBuilderCustom(gameObject)
    .Sequence()
        .AgentDestination("Find Enemy", target)
        .Do(() => {
            // Activate chase enemy code
            return TaskStatus.Success; 
        })
    .End()
    .Build();
```

### Your First Custom Node and Tree

It should take about 10 minutes to setup your first custom
action and extend the pre-existing behavior tree builder script.

```C#
using Adnc.FluidBT.Tasks;
using Adnc.FluidBT.Tasks.Actions;
using UnityEngine;
using UnityEngine.AI;

public class AgentDestination : ActionBase {
    public NavMeshAgent agent;
    public Transform target;

    protected override void OnInit () {
        agent = Owner.GetComponent<NavMeshAgent>();
    }

    protected override TaskStatus OnUpdate () {
        agent.SetDestination(target.position);
        return TaskStatus.Success;
    }
}
```

New method added to the builder script. This is pretty simple and easy to customize since most of the overhead is
abstracted away.

```C#
using Adnc.FluidBT.Trees;
using UnityEngine;

public class TreeBuilderCustom : BehaviorTreeBuilderBase<TreeBuilderCustom> {
    // This is always required for class extension reasons (will error without)
    public TreeBuilderCustom (GameObject owner) : base(owner) {
    }
    
    public TreeBuilderCustom AgentDestination (Transform target) {
        _tree.AddNode(Pointer, new AgentDestination {
            Name = "Agent Destination",
            target = target
        });

        return this;
    }
}
```

Be sure when you create trees that you use your new `TreeBuilderCustom` class. For example:

```C#
_tree = new TreeBuilderCustom(gameObject)
    .Sequence()
        .AgentDestination("Find Target", target)
        ...
    .End()
    .Build();
```

And you're done! You've now created a custom action and extendable behavior tree builder. The following examples
will be more of the same. But each covers a different node type.

### Custom Actions

You can create your own custom actions with the following template. This is useful for bundling up code
that you're using constantly.

```C#
using UnityEngine;
using Adnc.FluidBT.Tasks;
using Adnc.FluidBT.Tasks.Actions;

public class CustomAction : ActionBase {
    // Triggers only the first time this node is run (great for caching data)
    protected override void OnInit () {
    }

    // Triggers every time this node starts running. Does not trigger if TaskStatus.Continue was last returned by this node
    protected override void OnStart () {
    }

    // Triggers every time `Tick()` is called on the tree and this node is run
    protected override TaskStatus OnUpdate () {
        // Points to the GameObject of whoever owns the behavior tree
        Debug.Log(Owner.name);
        return TaskStatus.Success;
    }

    // Triggers whenever this node exits after running
    protected override void OnExit () {
    }
}
```

Add your new node to a custom tree builder.

```C#
using Adnc.FluidBT.Trees;
using UnityEngine;

public class TreeBuilderCustom : BehaviorTreeBuilderBase<TreeBuilderCustom> {
    public TreeBuilderCustom (GameObject owner) : base(owner) {
    }

    public TreeBuilderCustom CustomAction (string name = "Custom Action") {
        _tree.AddNode(Pointer, new CustomAction {
            Name = name
        });

        return this;
    }
}
```

### Custom Conditions

Custom conditions can be added with the following example template. You'll want to use these for checks such as sight,
if the AI can move to a location, and other tasks that require a complex check.

```C#
using UnityEngine;
using Adnc.FluidBT.Tasks;

public class CustomCondition : ConditionBase {
    // Triggers only the first time this node is run (great for caching data)
    protected override void OnInit () {
    }

    // Triggers every time this node starts running. Does not trigger if TaskStatus.Continue was last returned by this node
    protected override void OnStart () {
    }

    // Triggers every time `Tick()` is called on the tree and this node is run
    protected override bool OnUpdate () {
        // Points to the GameObject of whoever owns the behavior tree
        Debug.Log(Owner.name);
        return true;
    }

    // Triggers whenever this node exits after running
    protected override void OnExit () {
    }
}
```

Add the new condition to your behavior tree builder with the following snippet.

```C#
using UnityEngine;
using Adnc.FluidBT.Trees;

public class TreeBuilderCustom : BehaviorTreeBuilderBase<TreeBuilderCustom> {
    public TreeBuilderCustom (GameObject owner) : base(owner) {
    }

    public TreeBuilderCustom CustomCondition (string name = "My Condition") {
        _tree.AddNode(Pointer, new CustomCondition {
            Name = name
        });

        return this;
    }
}
```

### Custom Composites

Fluid Behavior Tree isn't limited to just custom actions and conditions. You can create new composite types with a fairly
simple API. Here is an example of a new selector variation that randomly chooses an execution order.

```C#
using Adnc.FluidBT.TaskParents.Composites;
using Adnc.FluidBT.Tasks;
using Random = System.Random;

// Makes a selector randomize its order of running child tasks
public class SelectorRandom : CompositeBase {
    private bool _init;
    
    // Triggers each time this node is ticked
    protected override TaskStatus OnUpdate () {
        if (!_init) {
            ShuffleChildren();
            _init = true;
        }
        
        for (var i = ChildIndex; i < Children.Count; i++) {
            var child = Children[ChildIndex];

            switch (child.Update()) {
                case TaskStatus.Success:
                    return TaskStatus.Success;
                case TaskStatus.Continue:
                    return TaskStatus.Continue;
            }

            ChildIndex++;
        }

        return TaskStatus.Failure;
    }

    // Reset is triggered when the behavior tree ends, then runs again ticking this node
    public override void Reset (bool hardReset = false) {
        base.Reset(hardReset);
                    
        ShuffleChildren();
    }

    private void ShuffleChildren () {
        var rng = new Random();
        var n = Children.Count;  
        while (n > 1) {  
            n--;  
            var k = rng.Next(n + 1);  
            var value = Children[k];  
            Children[k] = Children[n];  
            Children[n] = value;  
        }
    }
}
```

Adding custom composites to your behavior tree only takes a line of code. Below is a commented out chunk
of code if you need more control over the parameters of your composite.

```C#
using UnityEngine;
using Adnc.FluidBT.Trees;

public class TreeBuilderCustom : BehaviorTreeBuilderBase<TreeBuilderCustom> {
    public TreeBuilderCustom (GameObject owner) : base(owner) {
    }

    public TreeBuilderCustom CustomComposite (string name = "My Custom Composite") {
        return ParentTask<CustomComposite>(name);

        // Or you can code this manually if you need more specifics
        // var parent = new CustomComposite { Name = name };
        // _tree.AddNode(Pointer, parent);
        // _pointer.Add(parent);
        // return this;
    }
}
```

### Custom Decorators

Decorators can also be custom written to cut down on repetitive code.

```C#
using Adnc.FluidBT.Decorators;
using Adnc.FluidBT.Tasks;

public class Inverter : DecoratorBase {
    protected override TaskStatus OnUpdate () {
        if (Child == null) {
            return TaskStatus.Success;
        }

        var childStatus = Child.Update();
        var status = childStatus;

        switch (childStatus) {
            case TaskStatus.Success:
                status = TaskStatus.Failure;
                break;
            case TaskStatus.Failure:
                status = TaskStatus.Success;
                break;
        }

        return status;
    }
}
```

Implementing decorators is similar to composites (alternative commented out code for more complex integration).
See the commented area for more details.

```C#
using UnityEngine;
using Adnc.FluidBT.Trees;

public class TreeBuilderCustom : BehaviorTreeBuilderBase<TreeBuilderCustom> {
    public TreeBuilderCustom (GameObject owner) : base(owner) {
    }

    public TreeBuilderCustom CustomDecorator (string name = "My Custom Decorator") {
        return ParentTask<CustomDecorator>(name);

        // Or you can code this manually if you need more specifics
        // var parent = new CustomComposite { Name = name };
        // _tree.AddNode(Pointer, parent);
        // _pointer.Add(parent);
        // return this;
    }
}
```

## Submitting code to this project

Please fill out the following details if you'd like to contribute new code to this project.

1. Clone this project for the core code with tests
2. Put your new code in a separate branch
3. Make sure your new code is reasonably tested to demonstrate it works (see `*Test.cs` files)
4. Submit a pull request to the `develop` branch
