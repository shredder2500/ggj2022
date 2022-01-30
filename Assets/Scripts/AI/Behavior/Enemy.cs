using System;
using AI.Pathing;
using DefaultNamespace;
using DefaultNamespace.Weapons;
using UnityEngine;

namespace AI.Behavior {
public class Enemy : Bot {
  [SerializeField] private BotLevel[] levels;
  [SerializeField] private BaseWeapon weapon;
  [SerializeField] private Animator animator;
  [SerializeField] private float closeEnough = 1;

  protected override BehaviorTree BehaviorTree => BehaviorTree.SimpleAI;
  private Pathfinder _pathfinder;
  private float _speed;
  public float Speed => _speed;
  public Pathfinder Pathfinder => _pathfinder;
  public Animator Animator => animator;
  public BaseWeapon Weapon => weapon;

  public void Init(PathGrid pathGrid, int level) {
    _pathfinder = new(pathGrid);
    weapon.SetDamage(levels[level].Damage);
    weapon.SetFireRate(levels[level].FireRate);
    _speed = levels[level].Speed;
  }

  public bool NearTarget() =>
    Vector2.Distance(transform.position2D(), Memory.Get<Transform>("Target").position2D()) < closeEnough;

  public override Vector2 PredictPosition(float time) {
    if (NearTarget())
      return transform.position2D();
    
    var path = Memory.Get<GridNode[]>("Path");
    var (pathIdx, journey) = Memory.Get<(int, float)>("PathComplete");
    if (pathIdx + 1 == path.Length) return transform.position2D();

    var start = pathIdx == 0 ? Memory.Get<Vector2>("PathStart") : path[pathIdx].Point;
    var end = path[pathIdx + 1].Point;
    var fullTime = Vector2.Distance(start, end) / Speed;
    var completed = (journey + time) / fullTime;

    return Vector2.Lerp(start, end, completed);
  }

  [Serializable]
  private class BotLevel {
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private float fireRate;

    public float Speed => speed;
    public int Damage => damage;
    public float FireRate => fireRate;
  }

}
}