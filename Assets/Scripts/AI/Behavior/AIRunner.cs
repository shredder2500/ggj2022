using System;
using UnityEngine;

namespace AI.Behavior {
public class AIRunner : MonoBehaviour {
  [SerializeField] private float interval = .25f;
  private float _time;
  
  private void Update() {
    _time += Time.deltaTime;
    if (_time < interval) return;
    _time = 0;
    BehaviorTree.SimpleAI.Execute();
  }
}
}