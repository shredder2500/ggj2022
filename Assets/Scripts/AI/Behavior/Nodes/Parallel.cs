using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI.Behavior {
public class Parallel : Node {
  public readonly IEnumerable<Node> _childNodes;

  public Parallel(IEnumerable<Node> childNodes) => _childNodes = childNodes;
  
  public override NodeState Execute(Bot ai) {
    var result = NodeState.Success;

    foreach (var node in _childNodes) {
      var state = node.Execute(ai);
      if (state == NodeState.Success) continue;
      if (result == NodeState.Failure) continue;
      result = state;
    }

    return result;
  }
}
}