using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Behavior {
public partial class BehaviorTree {
  private readonly Node _root;
  private readonly List<Bot> _mobs = new();

  public BehaviorTree(Node root) => _root = root;

  public void AddBot(Bot ai) {
    if (!ai) return;
    _mobs.Add(ai);
  }

  public void RemoveBot(Bot ai) => _mobs.Remove(ai);

  public void Execute() {
    foreach (var ai in _mobs) {
      _root.Execute(ai);
    }
  }

  public static BehaviorBuilder Selector(params BehaviorBuilder[] builders) => BehaviorBuilder.Selector(builders);
  public static BehaviorBuilder Sequence(params BehaviorBuilder[] builders) => BehaviorBuilder.Sequence(builders);
  
  public static BehaviorBuilder Action(Func<Bot, NodeState> action) => new ActionNodeBuilder(action);
  public static BehaviorBuilder Action<T>(Func<T, NodeState> action) where T : Bot =>
    new ActionNodeBuilder(bot => action((T)bot));
  
  public static BehaviorBuilder Action(Action action) => new ActionNodeBuilder(action);
  
  public static BehaviorBuilder Action(Action<Bot> action) => new ActionNodeBuilder(action);
  public static BehaviorBuilder Action<T>(Action<T> action) where T : Bot =>
    new ActionNodeBuilder(bot => action((T)bot));
  
  public static BehaviorBuilder Action(Func<NodeState> action) => new ActionNodeBuilder(action);

  public static BehaviorBuilder Action(Func<bool> action) =>
    new ActionNodeBuilder(() => action() ? NodeState.Success : NodeState.Failure);

  public static BehaviorBuilder Action(Func<Bot, bool> action) =>
    new ActionNodeBuilder(bot => action(bot) ? NodeState.Success : NodeState.Failure);
  public static BehaviorBuilder Action<T>(Func<T, bool> action) where T : Bot =>
    new ActionNodeBuilder(bot => action((T)bot) ? NodeState.Success : NodeState.Failure);

  public static BehaviorBuilder Fail() => Action(() => NodeState.Failure);
  public static BehaviorBuilder Success() => Action(() => NodeState.Success);
  public static BehaviorBuilder Log(string log) => new ActionNodeBuilder(() => Debug.Log(log));
  public static BehaviorBuilder LogWarning(string log) => new ActionNodeBuilder(() => Debug.LogWarning(log));
  public static BehaviorBuilder LogError(string error) => new ActionNodeBuilder(() => Debug.LogError(error));
}
}