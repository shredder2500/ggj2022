using UnityEngine;

namespace DefaultNamespace.Weapons {
public abstract class BaseWeapon : MonoBehaviour {
  protected int Damage { get; private set; }
  protected float FireRate { get; private set; }
  private int _level;
  protected int Level => _level;

  public void SetDamage(int damage) => Damage = damage;
  public void SetFireRate(float fireRate) => FireRate = fireRate;
  public void SetLevel(int level) => _level = level;
  
  public abstract void FireAt(Transform target);
  
}
}