using System.Collections.Generic;
using UnityEngine;

namespace AI.Behavior {
public class Sequence : Node {
  private readonly IEnumerable<Node> _childNodes;

  public Sequence(IEnumerable<Node> childNodes) => _childNodes = childNodes;
  
  public override NodeState Execute(Bot ai) {
    foreach (var node in _childNodes) {
      var state = node.Execute(ai);

      if (state == NodeState.Failure) return NodeState.Failure;
      if (state == NodeState.Pending) return NodeState.Pending;
    }

    return NodeState.Success;
  }
}
}