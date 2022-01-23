using UnityEngine;

public class Movement : MonoBehaviour {
  [SerializeField] private float speed = 1;

  private Vector2 _moveValue;

  public void SetMoveDir(Vector2 value) => _moveValue = value.normalized;

  private void Update() {
    var moveDir = _moveValue * speed * Time.deltaTime;
    transform.position += new Vector3(moveDir.x, moveDir.y);
  }
}
