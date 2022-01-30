using System;
using AI.Behavior;
using DefaultNamespace.AI.Sensors;
using DefaultNamespace.UI;
using DefaultNamespace.Weapons;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DefaultNamespace {
public class Tower : Bot {
  public const int MaxDamage = 10;
  public const float MaxRange = 10;
  public const float MaxFireRate = .15f;
  [SerializeField] private Sprite buildSprite;
  [SerializeField] private Sprite buildIconSprite;
  [SerializeField] private BaseWeapon weapon;
  [SerializeField] private BaseSensor sensor;
  [SerializeField] private TowerLevel[] levels;
  [SerializeField] private UnityEvent onUpgrade;
  [SerializeField] private UnityEvent onDowngrade;
  protected override BehaviorTree BehaviorTree => BehaviorTree.SimpleTurret;

  public BaseSensor Sensor => sensor;
  public BaseWeapon Weapon => weapon;

  public Sprite BuildSprite => buildSprite;
  public Sprite BuildIconSprite => buildIconSprite;

  private int _level;
  public int Level => _level + 1;
  public int MaxLevel => levels.Length;

  public int Damage => levels[_level].Damage;
  public float Range => levels[_level].Range;
  public float FireRate => levels[_level].FireRate;
  public int Cost => levels[_level].Cost;

  public int NextDamage => Level < MaxLevel ? levels[_level + 1].Damage : Damage;
  public float NextRate => Level < MaxLevel ? levels[_level + 1].FireRate : FireRate;
  public float NextRange => Level < MaxLevel ? levels[_level + 1].Range : Range;
  public int NextCost => levels[_level + 1].Cost;

  public int PrevDamage => _level > 0 ? levels[_level - 1].Damage : Damage;
  public float PrevRate => _level > 0 ? levels[_level - 1].FireRate : FireRate;
  public float PrevRange => _level > 0 ? levels[_level - 1].Range : Range;
  public int Refund => Mathf.FloorToInt(levels[_level].Cost * .8f);

  private void Start() {
    UpdateStats();
  }

  public void Upgrade() {
    if (_level + 1 >= MaxLevel) return;
    _level += 1;
    onUpgrade.Invoke();
    UpdateStats();
  }

  public void Downgrade() {
    _level -= 1;
    onDowngrade.Invoke();
    if (_level >= 0) {
      UpdateStats();
    }
    else {
      Destroy(gameObject);
    }
  }

  private void UpdateStats() {
    weapon.SetDamage(levels[_level].Damage);
    weapon.SetFireRate(levels[_level].FireRate);
    sensor.SetRange(levels[_level].Range);
  }


  [Serializable]
  public class TowerLevel {
    [SerializeField] private int cost;
    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private float fireRate;

    public int Cost => cost;
    public int Damage => damage;
    public float Range => range;
    public float FireRate => fireRate;
  }

}
}