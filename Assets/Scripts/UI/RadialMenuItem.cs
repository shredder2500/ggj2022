using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DefaultNamespace.UI {
public class RadialMenuItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

  [SerializeField] private UnityEvent onClick;
  private RectTransform _rect;

  private void Start() => _rect = GetComponent<RectTransform>();
  
  public void OnPointerEnter(PointerEventData eventData) {
    _rect.DOComplete();
    _rect.DOScale(1.3f, .3f).SetEase(Ease.OutQuad);
  }

  public void OnPointerExit(PointerEventData eventData) {
    _rect.DOComplete();
    _rect.DOScale(1, .3f).SetEase(Ease.InQuad);
  }

  public void OnPointerClick(PointerEventData eventData) => onClick.Invoke();
}
}