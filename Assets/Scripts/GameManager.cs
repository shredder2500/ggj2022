using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace DefaultNamespace {
public class GameManager : MonoBehaviour {
  [SerializeField] private UnityEvent<int> onGameOver;
  [SerializeField] private UnityEvent onWin;
  [SerializeField] private UnityEvent onLose;
  [SerializeField] private Health mainTarget;

  private static GameManager _instance;
  public static GameManager Instance => _instance;
  private bool _isGameActive = true;

  public bool IsGameActive => _isGameActive;

  private void Awake() => _instance = this;
  
  public void GameOver(bool win) {
    _isGameActive = false;

    var stars = 0;

    if (mainTarget.Value >= mainTarget.MaxValue)
      stars = 3;
    else if (mainTarget.Value >= mainTarget.MaxValue / 2)
      stars = 2;
    else if (mainTarget.Value > 0)
      stars = 1;

    var key = $"{SceneManager.GetActiveScene().name}_score";
    var currentScore = PlayerPrefs.GetInt(key, 0);
    
    PlayerPrefs.SetInt(key, Mathf.Max(currentScore, stars));
    
    onGameOver.Invoke(stars);
    if (win)
      onWin.Invoke();
    else
      onLose.Invoke();
  }
}
}