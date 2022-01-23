using UnityEngine;

namespace DefaultNamespace {
[RequireComponent(typeof(SpriteRenderer))]
public class TowerBuild : MonoBehaviour {
  [SerializeField] private GameObject towerPrefab;
  [SerializeField] private Color canBuildColor;
  [SerializeField] private Color cantBuildColor;

  private SpriteRenderer _spriteRenderer;

  private void Start() => _spriteRenderer = GetComponent<SpriteRenderer>();

  public void Build() {
    Instantiate(towerPrefab, transform.position2D(), Quaternion.identity);
  }

  public void UpdateCanBuild(bool canBuild) {
    if (!_spriteRenderer) return;
    _spriteRenderer.color = canBuild ? canBuildColor : cantBuildColor;
  }
}
}