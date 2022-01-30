using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
public class TitleScreen : MonoBehaviour {
  [SerializeField] private float lifetime = 4;

  private void Start() {
    var image = GetComponent<Image>();

    image.DOFade(0, .5f).SetEase(Ease.OutQuad).SetDelay(lifetime)
      .OnComplete(() => Destroy(image.gameObject));
  }
}
}