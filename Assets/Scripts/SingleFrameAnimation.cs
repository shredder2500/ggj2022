using UnityEngine;

namespace DefaultNamespace {
public class SingleFrameAnimation : MonoBehaviour {
  [SerializeField] private SpriteRenderer spriteRenderer;
  [SerializeField] private Sprite triggeredFrame;
  [SerializeField] private Sprite normalFrame;
  [SerializeField] private float lifetime;

  private float _time;

  public void Trigger() {
    spriteRenderer.sprite = triggeredFrame;
    _time = 0;
  }

  private void Update() {
    _time += Time.deltaTime;

    if (_time < lifetime) return;

    spriteRenderer.sprite = normalFrame;
  }
}
}