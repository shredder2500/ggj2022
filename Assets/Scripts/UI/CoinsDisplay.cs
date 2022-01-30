using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI {
public class CoinsDisplay : MonoBehaviour {
  [SerializeField] private TMP_Text textElement;
  private MoneyManager _moneyManager;

  private void Start() => _moneyManager = FindObjectOfType<MoneyManager>();

  private void Update() {
    if (!_moneyManager || !textElement) return;
    textElement.text = $"Coins: {_moneyManager.Amount}";
  }
}
}