using UnityEngine;

namespace AI.Pathing {
public class GridNode {
  public readonly int X, Y;
  private float _cost;
  private bool _blocked;

  public float Cost => _cost;
  public bool Blocked => _blocked;
  public Vector2 Point => new(X, Y);

  public GridNode(int x, int y, float cost, bool blocked) => (X, Y, _cost, _blocked) = (x, y, cost, blocked);

  public void Block() => _blocked = true;
  public void UnBlock() => _blocked = false;
  public void SetCost(float value) => _cost = value; 

  public override int GetHashCode() {
    unchecked
    {
      var hashCode = X;
      hashCode = (hashCode * 397) ^ Y;
      return hashCode;
    }
  }
}
}