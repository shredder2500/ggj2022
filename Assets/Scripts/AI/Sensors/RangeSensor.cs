using System;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace.AI.Sensors {
public class RangeSensor : BaseSensor {
  [SerializeField] private LayerMask mask;
  [SerializeField] private int maxDetected = 3;

  [SerializeField]
  private Collider2D[] _detected;
  private int _size;

  public override Collider2D[] Detected => _detected;
  public override int DetectedCount => _size;

  private void Start() {
    _detected = new Collider2D[maxDetected];
  }

  private void Clear() {
    if (_detected == null) return;
    for (var i = 0; i < _detected.Length; i++) {
      _detected[i] = null;
    }
  }

  public override bool Scan() {
    Clear();
    _size = Physics2D.OverlapCircleNonAlloc(transform.position2D(), Range, _detected, mask);
    return _size > 0;
  }

  public override bool ScanFor(Transform target) =>
    Vector2.Distance(target.position2D(), transform.position2D()) < Range;

  private void OnDrawGizmosSelected() {
    Gizmos.DrawWireSphere(transform.position, Range);
  }
}
}