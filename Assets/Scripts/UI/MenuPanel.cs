using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UI {
public class MenuPanel : MonoBehaviour {
  private RectTransform _rectTransform;

  private void Start() {
    _rectTransform = GetComponent<RectTransform>();
    var y = -(Screen.height) - 100;
    _rectTransform.DOAnchorPosY(y, 0);
  }

  public void Show() {
    _rectTransform.DOAnchorPosY(0, .3f).SetEase(Ease.OutQuad);
  }

  public void Hide() {
    var y = -(Screen.height / 2) - 100;
    _rectTransform.DOAnchorPosY(y, .3f).SetEase(Ease.InQuad);
  }
}
}