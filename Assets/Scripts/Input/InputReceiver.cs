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
  private PlayerMelee _melee;

  [SerializeField] private UnityEvent onEnterBuildMode;
  [SerializeField] private UnityEvent onEnterMeleeMode;
  [SerializeField] private UnityEvent onEnterBuildMenu;
  [SerializeField] private UnityEvent onSkipToWave;

  private void Start() {
    _movement = GetComponent<Movement>();
    _builder = GetComponent<TowerBuilder>();
    _melee = GetComponent<PlayerMelee>();
  }

  public void OnInputControls(PlayerInput input) => _camera = input.camera;

  public void Move(InputAction.CallbackContext ctx) {
    var value = ctx.ReadValue<Vector2>();
    _movement.SetMoveDir(value);
    _melee.Aim(value);
  }

  public void MousePosition(InputAction.CallbackContext ctx) {
    var mousePos = ctx.ReadValue<Vector2>();
    var mouseWorldPos = _camera.ScreenToWorldPoint(new (mousePos.x, mousePos.y, 10));
    _builder.SetBuildPos(mouseWorldPos);
    // _melee.Aim(mouseWorldPos - transform.position);
  }

  public void Attack(InputAction.CallbackContext ctx) {
    if (ctx.performed)
      _melee.Fire();
  }

  public void Build(InputAction.CallbackContext ctx) {
    if (ctx.performed)
      _builder.Build();
  }

  public void EnterBuildMode() => onEnterBuildMode.Invoke();
  public void EnterBuildMode(InputAction.CallbackContext ctx) {
    if (ctx.performed)
      EnterBuildMode();
  }

  public void EnterBuildMenu() => onEnterBuildMenu.Invoke();

  public void EnterBuildMenu(InputAction.CallbackContext ctx) {
    if (ctx.performed)
      EnterBuildMenu();
  }

  public void EnterMeleeMode() => onEnterMeleeMode.Invoke();
  public void EnterMeleeMode(InputAction.CallbackContext ctx) {
    if (ctx.performed)
      EnterMeleeMode();
  }

  public void SkipToNextWave(InputAction.CallbackContext ctx) {
    if (ctx.performed)
      onSkipToWave.Invoke();
  }
}
}