using UnityEngine;

namespace DefaultNamespace.AI.Sensors {
public abstract class BaseSensor : MonoBehaviour {
  protected float Range { get; private set; }
  
  public void SetRange(float range) => Range = range;
  public abstract Collider2D[] Detected { get; }
  public abstract int DetectedCount { get; }
  public abstract bool Scan();
  public abstract bool ScanFor(Transform target);
}
}