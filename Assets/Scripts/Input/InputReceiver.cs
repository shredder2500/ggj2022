using DefaultNamespace.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DefaultNamespace.Input {
[RequireComponent(typeof(Movement), typeof(TowerBuilder))]
public class InputReceiver : MonoBehaviour {
  private Movement _movement;
  private TowerBuilder _builder;
  private Camera _camera;

  [SerializeField] private UnityEvent onEnterBuildMode;
  [SerializeField] private UnityEvent onEnterMeleeMode;
  [SerializeField] private UnityEvent onEnterTurretMode;

  [SerializeField] private RadialMenu buildMenu;

  private void Start() {
    _movement = GetComponent<Movement>();
    _builder = GetComponent<TowerBuilder>();

    foreach (var device in InputSystem.devices) {
      Debug.Log($"{device.name} - {device.description.deviceClass}");
    }
  }

  public void OnInputControls(PlayerInput input) => _camera = input.camera;

  public void Move(InputAction.CallbackContext ctx) {
    var value = ctx.ReadValue<Vector2>();
    _movement.SetMoveDir(value);
  }

  public void MousePosition(InputAction.CallbackContext ctx) {
    var mousePos = ctx.ReadValue<Vector2>();
    var mouseWorldPos = _camera.ScreenToWorldPoint(new (mousePos.x, mousePos.y, 10));
    _builder.SetBuildPos(mouseWorldPos);
  }

  public void Build(InputAction.CallbackContext ctx) {
    if (ctx.performed)
      _builder.Build();
  }

  public void EnterBuildMode(InputAction.CallbackContext ctx) {
    if (ctx.performed)
      onEnterBuildMode.Invoke();
  }

  public void EnterMeleeMode(InputAction.CallbackContext ctx) {
    if (ctx.performed)
      onEnterMeleeMode.Invoke();
  }

  public void EnterTurretMode(InputAction.CallbackContext ctx) {
    if (ctx.performed)
      onEnterTurretMode.Invoke();
  }
}
}