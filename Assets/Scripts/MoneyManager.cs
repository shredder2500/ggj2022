using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace {
public class MoneyManager : MonoBehaviour {
  [SerializeField] private int startingCoins;
  [SerializeField] private UnityEvent<int> onAmountChange;
  [SerializeField] private UnityEvent<string> onAmountTextChange;
  private int _coins;

  public int Amount => _coins;

  private void Start() {
    _coins = startingCoins;
    onAmountChange.Invoke(Amount);
    onAmountTextChange.Invoke(Amount.ToString());
  }

  public void Add(int amount) {
    _coins += amount;
    onAmountChange.Invoke(Amount);
    onAmountTextChange.Invoke(Amount.ToString());
  }

  public void Remove(int amount) {
    _coins -= amount;
    onAmountChange.Invoke(Amount);
    onAmountTextChange.Invoke(Amount.ToString());
  }
}
}