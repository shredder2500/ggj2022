using UnityEngine;

namespace DefaultNamespace.Weapons {
public class SuicideWeapon : BaseWeapon {
  [SerializeField] private GameObject effectPrefab;
  [SerializeField] private Health health;
  
  public override void FireAt(Transform target) {
    Instantiate(effectPrefab, transform.position, Quaternion.identity);
    var targetHealth = target.GetComponent<Health>();
    targetHealth.Damage(Damage);
    health.Kill(false);
  }
}
}