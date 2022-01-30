using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.AI.Sensors;
using UnityEngine;

namespace AI.Behavior {
public abstract class Bot : MonoBehaviour {
  [SerializeField] private bool aIEnabled = true;
  
  protected abstract BehaviorTree BehaviorTree { get; }
  public readonly Memory Memory = new();
  
  private void OnEnable() {
    if (aIEnabled)
      BehaviorTree.AddBot(this);
  }
  
  private void OnDestroy() {
    Memory.Clear();
    BehaviorTree.RemoveBot(this);
  }

  public virtual Vector2 PredictPosition(float time) => transform.position2D();
}
}