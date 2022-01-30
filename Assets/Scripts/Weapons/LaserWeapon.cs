using System;
using UnityEngine;

namespace DefaultNamespace.Weapons {
public class LaserWeapon : BaseWeapon {
  [SerializeField] private LaserSettings[] settings;
  [SerializeField] private Animator animator;
  private static readonly int Fire = Animator.StringToHash("Fire");

  public override void FireAt(Transform target) {
    
    
    if (animator)
      animator.SetTrigger(Fire);
  }

  [Serializable]
  private class LaserSettings {
    [SerializeField] private Sprite[] laserParts;

    public Sprite[] LaserParts => laserParts;
  }
}
}