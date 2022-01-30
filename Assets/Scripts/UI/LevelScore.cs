using DefaultNamespace.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI {
public class LevelScore : MonoBehaviour {
  [SerializeField] private Image[] ratingMarkers;
  [SerializeField] private Sprite rating;
  [SerializeField] private Sprite noRating;
  [SerializeField] private Image lockedIcon;
  [SerializeField] private string levelName;
  [SerializeField] private string prevLevelName;
  [SerializeField] private MyButton btn;

  public string LevelName => levelName;

  private void Update() {
    var scoreKey = $"{levelName}_score";
    var prevScoreKey = $"{prevLevelName}_score";
    var score = PlayerPrefs.GetInt(scoreKey, 0);
    var locked = !string.IsNullOrEmpty(prevLevelName) && PlayerPrefs.GetInt(prevScoreKey, 0) <= 0;

    for (var i = 0; i < ratingMarkers.Length; i++) {
      ratingMarkers[i].sprite = i < score ? rating : noRating;
    }
    
    lockedIcon.enabled = locked;
    btn.enabled = !locked;
  }
}
}