using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace.Input {
[RequireComponent(typeof(Movement))]
public class InputReceiver : MonoBehaviour {
  private Movement _movement;

  private void Start() => _movement = GetComponent<Movement>();

  public void Move(InputAction.CallbackContext ctx) => _movement.SetMoveDir(ctx.ReadValue<Vector2>());
}
}