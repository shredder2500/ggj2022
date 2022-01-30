using UnityEngine;

namespace DefaultNamespace {
[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class SortOrderFromY : MonoBehaviour {
  [SerializeField] private bool isStatic;
  [SerializeField] private float offset;
  private SpriteRenderer _spriteRenderer;

  private void Start() {
    _spriteRenderer = GetComponent<SpriteRenderer>();
    _spriteRenderer.sortingOrder = Mathf.FloorToInt(-((transform.position.y + offset) * 100));
  }

  private void Update() {
    if (isStatic && Application.isPlaying) return;

    _spriteRenderer.sortingOrder = Mathf.FloorToInt(-((transform.position.y + offset) * 100));
  }
}
}