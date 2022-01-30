using System.Linq;
using DefaultNamespace;
using UnityEngine;

namespace AI.Behavior {
public partial class BehaviorTree {
  public static BehaviorTree SimpleTurret = Selector(
    Sequence(
      Action(() => !GameManager.Instance.IsGameActive),
      Action(ai => Object.Destroy(ai.gameObject))),
    Sequence(
      Action(bot => !bot.Memory.Contains("Target")),
      Action<Tower>(bot => bot.Sensor.Scan()),
      Action<Tower>(bot => bot.Memory.Set("Target", bot.Sensor.Detected[0].transform))),
    // No Target found. Do Nothing
    Action(bot => !bot.Memory.Contains("Target")),
    Sequence(
      Action(bot => !bot.Memory.Get<Transform>("Target")),
      Action(bot => bot.Memory.Remove("Target"))),
    Sequence(
      Action<Tower>(bot => !bot.Sensor.ScanFor(bot.Memory.Get<Transform>("Target"))),
      Action(bot => bot.Memory.Remove("Target"))),
    Action<Tower>(bot => bot.Weapon.FireAt(bot.Memory.Get<Transform>("Target")))
  );
}
}