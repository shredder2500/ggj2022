using System.Collections;
using UnityEngine;

namespace DefaultNamespace.Weapons {
public abstract class Ammo : MonoBehaviour {
  public abstract IEnumerator FireAt(Vector2 point);
  public abstract float LifetimeAfterHit { get; }

  public abstract float CalcTimeToHit(Vector2 target);
}
}