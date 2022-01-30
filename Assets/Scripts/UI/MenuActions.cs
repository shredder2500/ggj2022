using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace {
public class MenuActions : MonoBehaviour {
  public void Quit() => Application.Quit();
  public void LoadScene(string scene) => SceneManager.LoadScene(scene);

  public void ResetProgress() => PlayerPrefs.DeleteAll();
}
}