using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour {
  [SerializeField] private float speed = 1;
  [SerializeField]
  private Animator animator;

  [SerializeField] private Rect bounds;

  private Rigidbody2D _rigidbody2D;
  private Vector2 _moveValue;
  private static readonly int DirX = Animator.StringToHash("dirX");
  private static readonly int DirY = Animator.StringToHash("dirY");
  private static readonly int Speed = Animator.StringToHash("speed");

  private void Start()
  {
    _rigidbody2D = GetComponent<Rigidbody2D>();
  }

  public void SetMoveDir(Vector2 value) => _moveValue = value.normalized;

  private void LateUpdate() {
    var moveDir = _moveValue * speed * Time.deltaTime;
    if (!bounds.Contains(_rigidbody2D.position + moveDir)) return;
    _rigidbody2D.position += moveDir;

    if (!animator) return;
    if (_moveValue.magnitude > .1f) {
      animator.SetFloat(DirX, _moveValue.x);
      animator.SetFloat(DirY, _moveValue.y);
    }
    animator.SetFloat(Speed, _moveValue.magnitude);
  }

  private void OnDrawGizmosSelected() {
    Gizmos.DrawWireCube(bounds.center, bounds.size);
  }
}
