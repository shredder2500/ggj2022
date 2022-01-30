using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace DefaultNamespace.UI {
public class MyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler {
  [SerializeField] private UnityEvent onClick;
  [SerializeField] private UnityEvent onEnter;
  [SerializeField] private UnityEvent onExit;
  [SerializeField] private UnityEvent onUp;
  [SerializeField] private float scaleOnHover = 1.4f;
  
  public void OnPointerEnter(PointerEventData eventData) {
    transform.DOComplete();
    transform.DOScale(Vector3.one * scaleOnHover, .3f).SetEase(Ease.OutQuad);
    onEnter.Invoke();
  }

  public void OnPointerExit(PointerEventData eventData) {
    transform.DOComplete();
    transform.DOScale(Vector3.one, .3f).SetEase(Ease.InQuad);
    onExit.Invoke();
  }

  public void OnPointerClick(PointerEventData eventData) {
    transform.DOPunchScale(Vector3.one * .2f, .3f).SetEase(Ease.OutQuad);
    onClick.Invoke();
  }

  public void OnPointerUp(PointerEventData eventData) {
    onUp.Invoke();
  }
}
}