using System.Linq;
using AI.Pathing;
using DefaultNamespace;
using UnityEngine;

namespace AI.Behavior {
public partial class BehaviorTree {
  public static BehaviorTree SimpleAI = Selector(
      Sequence(
        Action(() => !GameManager.Instance.IsGameActive),
        Action(ai => Object.Destroy(ai.gameObject))),
      Sequence(
        Action(bot => bot.Memory.Get<GridNode[]>("Path").Any(x => x.Blocked)),
        Action<Enemy>(bot => {
          var path = bot.Pathfinder.GetPath(bot.transform.position2D(),
            bot.Memory.Get<Transform>("Target").position2D());

          bot.Memory.Set("Path", path);
          bot.Memory.Set("PathStart", bot.transform.position2D());
        }),
        Action(bot => bot.Memory.Set("PathComplete", (0, 0f)))),
      Sequence(
        Action<Enemy>(bot => bot.NearTarget()),
        Action<Enemy>(bot => bot.Weapon.FireAt(bot.Memory.Get<Transform>("Target"))),
        Action<Enemy>(bot => {
          if (bot.Animator) bot.Animator.SetBool(Moving, false);
        })),
      Sequence(
        Action<Enemy>(bot => {
          var path = bot.Memory.Get<GridNode[]>("Path");
          var (pathIdx, journey) = bot.Memory.Get<(int, float)>("PathComplete");
          if (pathIdx + 1 == path.Length) return NodeState.Success;
            
          journey += Time.deltaTime;

          var start = pathIdx == 0 ? bot.Memory.Get<Vector2>("PathStart") : path[pathIdx].Point;
          var end = path[pathIdx + 1].Point;
          var fullTime = Vector2.Distance(start, end) / bot.Speed;
          var completed = journey / fullTime;
          var dir = (end - start).normalized;
          if (bot.Animator) {
            bot.Animator.SetFloat(DirX, dir.x);
            bot.Animator.SetFloat(DirY, dir.y);
            bot.Animator.SetBool(Moving, true);
          }

          bot.transform.position = Vector2.Lerp(start, end, completed);

          if (completed >= 1) {
            pathIdx += 1;
            journey = 0f;
          }
          
          bot.Memory.Set("PathComplete", (pathIdx, journey));

          return pathIdx == path.Length ? NodeState.Success : NodeState.Pending;
        })),
      LogWarning("AI doesn't know what to do"));

  private static readonly int DirX = Animator.StringToHash("dirX");
  private static readonly int DirY = Animator.StringToHash("dirY");
  private static readonly int Moving = Animator.StringToHash("moving");
}
}