using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace {
public class TowerBuilder : MonoBehaviour {
  [SerializeField] private float buildRadius;
  [SerializeField] private UnityEvent onBuild;
  private TowerBuild _towerBuild;
  private Vector2 _buildPos;

  public void SetTowerBuild(TowerBuild towerBuild) {
    if (_towerBuild)
      Destroy(_towerBuild);
    _towerBuild = Instantiate(towerBuild, transform, true);
  }

  public void SetBuildPos(Vector2 pos) {
    if (_towerBuild == null) return;
    _towerBuild.transform.position = new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
    _buildPos = pos - transform.position2D();
  }

  private void Update() {
    if (_towerBuild == null) return;
    SetBuildPos(transform.position2D() + _buildPos);
    _towerBuild.UpdateCanBuild(CanBuild());
  }

  public void Build() {
    if (!_towerBuild && CanBuild()) return;
    _towerBuild.Build();
    onBuild.Invoke();
  }

  public void StopBuildMode() {
    if (!_towerBuild) return;
    Destroy(_towerBuild);
  }

  private bool CanBuild() {
    if (Vector2.Distance(_towerBuild.transform.position2D(), transform.position2D()) > buildRadius) return false;
    if (Physics2D.OverlapBox(_towerBuild.transform.position2D(), Vector2.one, 0)) return false;
    return true;
  }
}
}