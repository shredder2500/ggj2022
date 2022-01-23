using UnityEngine;

namespace DefaultNamespace {
public static class TransformExts {
  public static Vector2 position2D(this Transform transform) => transform.position;
}
}