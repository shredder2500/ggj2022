using System.Collections;
using AI.Behavior;
using UnityEngine;

namespace DefaultNamespace.Weapons {
public class AoeWeapon : BaseWeapon {
  [SerializeField] private Ammo[] ammoPrefabs;
  [SerializeField] private float damageRadius;
  [SerializeField] private float splashDamageModifier;
  [SerializeField] private LayerMask layerMask;
  [SerializeField] private GameObject hitEffectPrefab;

  private float _time;
  private readonly Collider2D[] _hits = new Collider2D[10];

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

  private void Clear() {
    for (var i = 0; i < _hits.Length; i++) {
      _hits[i] = null;
    }
  }

  private IEnumerator FireAt(Ammo ammoPrefab, Vector2 target, Health health) {
    var ammo = Instantiate(ammoPrefab, transform.position2D(), Quaternion.identity);
    ammo.transform.parent = transform;
    yield return StartCoroutine(ammo.FireAt(target));
    if (!health) yield break;
    if (hitEffectPrefab)
      Instantiate(hitEffectPrefab, target, Quaternion.identity);
    health.Damage(Damage);

    Clear();
    var hits = Physics2D.OverlapCircleNonAlloc(target, damageRadius, _hits, layerMask);
    for (var i = 0; i < hits; i++) {
      var hit = _hits[i];
      var h = hit.GetComponent<Health>();
      if (!h) continue;
      h.Damage(Mathf.FloorToInt(Damage * splashDamageModifier));
    }
  }
}
}