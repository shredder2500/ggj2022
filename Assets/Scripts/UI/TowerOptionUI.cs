using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DefaultNamespace.UI {
public class TowerOptionUI : MonoBehaviour {
  private Tower _towerPrefab;
  private TowerBuilder _builder;
  [SerializeField] private TMP_Text costTextElement;
  [SerializeField] private Image icon;
  private UnityEvent _onTowerSelected;

  public void Init(Tower prefab, TowerBuilder builder, UnityEvent onTowerSelected) => 
    (_towerPrefab, _builder, _onTowerSelected) = (prefab, builder, onTowerSelected);

  private void Start() {
    if (!_towerPrefab) return;
    costTextElement.text = _towerPrefab.Cost.ToString();
    icon.sprite = _towerPrefab.BuildIconSprite;
  }

  public void Select() {
    if (!_builder) return;
    _builder.SetTower(_towerPrefab);
    _onTowerSelected.Invoke();
  }
}
}