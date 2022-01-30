using System.Collections;
using AI.Behavior;
using UnityEngine;

namespace DefaultNamespace.Weapons {
public class BasicWeapon : BaseWeapon {
  [SerializeField] private Ammo[] ammoPrefabs;

  private float _time;

  private void Update() {
    _time += Time.deltaTime;
  }

  // TODO: object pooling ammo
  public override void FireAt(Transform targetObj) {
    if (_time < FireRate) return;
    _time = 0;

    var ammo = Level < ammoPrefabs.Length ? ammoPrefabs[Level] : ammoPrefabs[^1];
    var startTargetPos = targetObj.position2D();
    var timeToHit = ammo.CalcTimeToHit(startTargetPos);
    var bot = targetObj.GetComponent<Bot>();
    var target = bot ? bot.PredictPosition(timeToHit) : startTargetPos;
    StartCoroutine(FireAt(ammo, target, targetObj.GetComponent<Health>()));
  }

  private IEnumerator FireAt(Ammo ammoPrefab, Vector2 target, Health health) {
    var ammo = Instantiate(ammoPrefab, transform.position2D(), Quaternion.identity);
    ammo.transform.parent = transform;
    yield return StartCoroutine(ammo.FireAt(target));
    if (!health) yield break;
    
    health.Damage(Damage);
  }
}
}