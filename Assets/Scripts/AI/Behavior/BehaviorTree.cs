using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Behavior {
public partial class BehaviorTree {
  private readonly Node _root;
  private readonly List<Bot> _mobs = new();

  public BehaviorTree(Node root) => _root = root;

  public void AddBot(Bot ai) => _mobs.Add(ai);
  public void RemoveBot(Bot ai) => _mobs.Remove(ai);

  public void Execute() {
    foreach (var ai in _mobs) {
      _root.Execute(ai);
    }
  }

  public static BehaviorTree Build(Action<BehaviorBuilder> builder) {
    var x = new SelectorBuilder();
    builder(x);
    return new(x.Build());
  }
}
}