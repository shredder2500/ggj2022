using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour {
  [SerializeField] private float speed = 1;

  private Rigidbody2D _rigidbody2D;
  private Vector2 _moveValue;

  private void Start()
  {
    _rigidbody2D = GetComponent<Rigidbody2D>();
  }

  public void SetMoveDir(Vector2 value) => _moveValue = value.normalized;

  private void LateUpdate() {
    var moveDir = _moveValue * speed * Time.deltaTime;
    _rigidbody2D.position += moveDir;
  }
}
