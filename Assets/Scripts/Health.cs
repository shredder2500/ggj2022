using System;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace {
public class Health : MonoBehaviour {
  [SerializeField] private int maxHealth;
  private int _health;

  [SerializeField] private UnityEvent onDeath;
  [SerializeField] private UnityEvent<Health> onValueChange;
  [SerializeField] private GameObject effectPrefab;

  public event Action OnDeath;
  public int Value => _health;
  public int MaxValue => maxHealth;

  private void Start() => _health = maxHealth;

  public void Add(int value) => _health = Mathf.Min(_health + value, maxHealth);

  public void Damage(int value) {
    if (_health <= 0) return;
    _health -= value;
    onValueChange.Invoke(this);
    if (_health <= 0) {
      onDeath.Invoke();
      OnDeath?.Invoke();
      if (effectPrefab) {
        Instantiate(effectPrefab, transform.position, Quaternion.identity);
      }
    }
  }

  public void DestroyMe() => Destroy(gameObject);

  public void Kill(bool showEffect) {
    _health = 0;
    onDeath.Invoke();
    OnDeath?.Invoke();
    if (effectPrefab && showEffect) {
      Instantiate(effectPrefab, transform.position2D(), Quaternion.identity);
    }
  }
}
}