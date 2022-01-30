using System;
using UnityEngine;

namespace AI.Behavior {
public class AIRunner : MonoBehaviour {
  private void Update() {
    BehaviorTree.SimpleAI.Execute();
    BehaviorTree.SimpleTurret.Execute();
  }
}
}