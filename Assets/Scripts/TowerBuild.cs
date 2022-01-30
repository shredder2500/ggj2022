using DG.Tweening;
using TMPro;
using UnityEngine;

namespace DefaultNamespace {
[RequireComponent(typeof(SpriteRenderer))]
public class TowerBuild : MonoBehaviour {
  private Tower _towerPrefab;
  [SerializeField] private Color canBuildColor;
  [SerializeField] private Color cantBuildColor;
  [SerializeField] private TMP_Text cantBuildText;

  private SpriteRenderer _spriteRenderer;
  public Tower TowerPrefab => _towerPrefab;

  public int Cost => _towerPrefab == null ? 0 : _towerPrefab.Cost;

  public void SetTowerPrefab(Tower tower) {
    _spriteRenderer.sprite = tower.BuildSprite;
    _towerPrefab = tower;
  }

  private void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();

  public Tower Build() {
    if (!_towerPrefab || !enabled || !gameObject.activeSelf) return null;
    return Instantiate(_towerPrefab, transform.position2D(), Quaternion.identity);
  }

  public void UpdateCanBuild(bool canBuild, string reason) {
    cantBuildText.text = reason;
    Debug.Log(cantBuildText.alpha);

    if (!_spriteRenderer) return;
    _spriteRenderer.color = canBuild ? canBuildColor : cantBuildColor;
  }
}
}