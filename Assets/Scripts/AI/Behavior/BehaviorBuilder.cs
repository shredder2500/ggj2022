using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI.Behavior {
public abstract class BehaviorBuilder {
  private List<BehaviorBuilder> _builders = new();
  
  public BehaviorBuilder Selector(Action<BehaviorBuilder> builder) {
    var x = new SelectorBuilder();
    builder(x);
    _builders.Add(x);
    return this;
  }

  public BehaviorBuilder Sequence(Action<BehaviorBuilder> builder) {
    var x = new SequenceBuilder();
    builder(x);
    _builders.Add(x);
    return this;
  }

  public BehaviorBuilder Action(Func<Bot, NodeState> action) {
    var x = new ActionNodeBuilder(action);
    _builders.Add(x);
    return this;
  }

  public BehaviorBuilder Log(string message) => Action(_ => {
    Debug.Log(message);
    return NodeState.Success;
  });

  public BehaviorBuilder Log(Func<Bot, string> messageBuilder) => Action(ai => {
    var msg = messageBuilder(ai);
    Debug.Log(msg);
    return NodeState.Success;
  });

  public BehaviorBuilder LogWarning(string message) => Action(_ => {
    Debug.LogWarning(message);
    return NodeState.Success;
  });

  public BehaviorBuilder MoveTo(string key) => Action(ai => {
    ai.MoveTo(ai.Memory.Get<Vector2>(key));
    return NodeState.Success;
  });

  public BehaviorBuilder Attack(string key) => Action(ai => {
    // TODO: Attack
    return NodeState.Success;
  });

  public BehaviorBuilder MemoryContains(string key) =>
    Action(ai => ai.Memory.Contains(key) ? NodeState.Success : NodeState.Failure);

  public Node Build() => BuildNodes(_builders.Select(x => x.Build()));
  
  protected abstract Node BuildNodes(IEnumerable<Node> nodes);
}

public class SelectorBuilder : BehaviorBuilder {
  protected override Node BuildNodes(IEnumerable<Node> nodes) => new Selector(nodes);
}

public class SequenceBuilder : BehaviorBuilder {
  protected override Node BuildNodes(IEnumerable<Node> nodes) => new Sequence(nodes);
}

public class ActionNodeBuilder : BehaviorBuilder {
  private readonly ActionNode _node;
  public ActionNodeBuilder(Func<Bot, NodeState> action) => _node = new(action);
  protected override Node BuildNodes(IEnumerable<Node> nodes) => _node;
}
}