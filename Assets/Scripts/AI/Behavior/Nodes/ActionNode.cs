using System;
using UnityEngine;

namespace AI.Behavior {
public class ActionNode : Node {
  private readonly Func<Bot, NodeState> _func;

  public ActionNode(Func<Bot, NodeState> func) => _func = func;

  public override NodeState Execute(Bot ai) => _func(ai);
}
}