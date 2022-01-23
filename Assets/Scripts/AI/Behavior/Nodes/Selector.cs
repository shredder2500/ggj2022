using System.Collections.Generic;
using UnityEngine;

namespace AI.Behavior {
public class Selector : Node {
  private readonly IEnumerable<Node> _childNodes;

  public Selector(IEnumerable<Node> childNodes) => _childNodes = childNodes;

  public override NodeState Execute(Bot ai) {
    foreach (var node in _childNodes) {
      var state = node.Execute(ai);

      if (state == NodeState.Success) return NodeState.Success;
      if (state == NodeState.Pending) return NodeState.Pending;
    }

    return NodeState.Failure;
  }
}
}