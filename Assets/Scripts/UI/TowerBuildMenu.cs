using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace.UI {
public class TowerBuildMenu : MonoBehaviour {
  [SerializeField] private Tower[] towerPrefabs;
  [SerializeField] private TowerBuilder builder;
  [SerializeField] private TowerOptionUI optionPrefab;
  [SerializeField] private RectTransform content;

  [SerializeField] private UnityEvent onTowerSelected;

  private void Awake() {
    foreach (var towerPrefab in towerPrefabs) {
      var option = Instantiate(optionPrefab, content).GetComponent<TowerOptionUI>();
      option.Init(towerPrefab, builder, onTowerSelected);
    }
  }
}
}