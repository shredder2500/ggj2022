using UnityEngine;

namespace AI.Behavior {
public abstract class Node {
  public abstract NodeState Execute(Bot ai);
}
}