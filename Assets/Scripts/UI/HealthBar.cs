using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace DefaultNamespace.UI {
public class HealthBar : MonoBehaviour {
  [SerializeField] private Image bar;
  [SerializeField] private Ease ease = Ease.Flash;
  [SerializeField] private float speed = .2f;
  [SerializeField] private Vector3 punchScale;
  [SerializeField] private Color damageColor = Color.red;
  
  public void OnHealthValueChange(Health health) {
    bar.DOComplete();
    // bar.transform.DOComplete();
    bar.DOColor(damageColor, speed / 2).SetEase(ease)
      .OnComplete(() => bar.DOColor(Color.white, speed / 2).SetEase(ease));
    bar.DOFillAmount(health.Value / (float)health.MaxValue, speed).SetEase(ease);
    // bar.transform.DOPunchScale(punchScale, speed).SetEase(ease);
  }
}
}