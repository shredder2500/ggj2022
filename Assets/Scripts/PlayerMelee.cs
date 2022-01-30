using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace {
public class PlayerMelee : MonoBehaviour {
  [SerializeField] private SpriteRenderer sword;
  [SerializeField] private int damage = 2;
  [SerializeField] private Animator animator;
  [SerializeField] private LayerMask layerMask;
  [SerializeField] private float fireRate = .1f;
  private static readonly int FireTrigger = Animator.StringToHash("Attack");

  private float _time;
  private Vector2 _aimDir = Vector2.down;

  private readonly Dictionary<Vector2, (bool, bool, float)> _attackFlipLookup = new() {
    { Vector2.up, (true, true, 0) },
    { Vector2.down, (false, false, 0) },
    { Vector2.right, (false, false, 90) },
    { Vector2.left, (true, false, -90) }
  };
  
  private readonly Dictionary<Vector2, (bool, bool, float)> _flipLookup = new() {
    { Vector2.up, (true, false, 0) },
    { Vector2.down, (false, false, 0) },
    { Vector2.right, (true, false, 0) },
    { Vector2.left, (false, false, 0) }
  };

  private void Update() => _time += Time.deltaTime;

  private void FlipSprite(bool attacking) {
    var flipLookup = attacking ? _attackFlipLookup : _flipLookup;
    var key = flipLookup.Keys.OrderByDescending(x => Vector2.Dot(x, _aimDir)).First();

    var (flipX, flipY, rotation) = flipLookup[key];

    sword.flipY = flipY;
    sword.flipX = flipX;
    sword.transform.rotation = Quaternion.Euler(0, 0, rotation);
  }

  public void Aim(Vector2 dir) {
    if (dir.magnitude < .1f) return;
    _aimDir = dir.normalized;
    FlipSprite(false);
  }

  public void Fire() {
    if (_time < fireRate) return;
    FlipSprite(true);
    animator.SetTrigger(FireTrigger);
    var hit = Physics2D.OverlapCircle(transform.position2D() + _aimDir / 2, 1, layerMask);
    if (!hit) return;
    var health = hit.GetComponent<Health>();
    if (!health) return;
    health.Damage(damage);
  }

  public void AttackDone() {
    FlipSprite(false);
  }
}
}