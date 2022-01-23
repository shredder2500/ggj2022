using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace.UI {
public class PlayerInputDebugUI : MonoBehaviour {
  [SerializeField] private TMPro.TMP_Text textElement;

  private PlayerInput _playerInput;

  public void OnChange(PlayerInput input) {
    _playerInput = input;
  }
  private void Update() {
    textElement.text = $"Current ActionMap: {_playerInput.currentActionMap.name}";
  }
}
}