using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace.Weapons {
public class Projectile : Ammo {
  [SerializeField] private float speed;
  public override float LifetimeAfterHit => 0;

  public override IEnumerator FireAt(Vector2 point) {
    var time = 0f;
    var startPos = transform.position2D();
    var totalTime = CalcTimeToHit(point);

    while (time < totalTime) {
      yield return null;
      time += Time.deltaTime;

      transform.position = Vector2.Lerp(startPos, point, time / totalTime);
    }
    
    Destroy(gameObject);
  }


  public override float CalcTimeToHit(Vector2 target) {
    var dist = Vector2.Distance(transform.position2D(), target);
    return dist / speed;
  }
}
}