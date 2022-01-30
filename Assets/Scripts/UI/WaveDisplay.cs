using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI {
public class WaveDisplay : MonoBehaviour {
  [SerializeField] private TMP_Text waveCountElement;
  [SerializeField] private TMP_Text waveCountdownElement;
  [SerializeField] private TMP_Text enemyCountElement;
  [SerializeField] private Image waveProgressImage;

  public void OnWaveCount(int waveCount, int maxWaves) => waveCountElement.text = $"Wave {waveCount + 1} / {maxWaves}";

  public void OnWaveCountdown(float time) {
    if (time > .3f) {
      waveCountdownElement.DOFade(1, .3f).SetEase(Ease.OutQuad);
      waveCountdownElement.text = $"Next Wave in {Mathf.CeilToInt(time)}";
    }
    else {
      waveCountdownElement.DOFade(0, .3f)
        .OnComplete(() => {
          waveCountdownElement.text = "Wave Start!";
          waveCountdownElement.DOFade(1, .3f)
            .OnComplete(() => waveCountdownElement.DOFade(0, .3f).SetDelay(1));
        });
    }
  }

  public void OnEnemyUpdate(int current, int total) {
    waveProgressImage.DOComplete();
    waveProgressImage.DOFillAmount(1f - (current / (float)total), .3f).SetEase(Ease.OutQuad);
    enemyCountElement.text = $"{current} / {total}";
  }
}
}