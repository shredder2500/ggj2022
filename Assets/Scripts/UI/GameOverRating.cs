using UnityEngine;
using UnityEngine.UI;

namespace UI {
public class GameOverRating : MonoBehaviour {
  [SerializeField] private Image[] scoreMarkers;
  [SerializeField] private Sprite scoreSprite;
  
  public void UpdateRating(int score) {
    for (var i = 0; i < score; i++) {
      scoreMarkers[i].sprite = scoreSprite;
    }
  }
}
}