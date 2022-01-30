using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace.Weapons {
[ExecuteInEditMode]
public class Laser : Ammo {
  [SerializeField] private float speed;
  [SerializeField] private Transform pivot;
  [SerializeField] private SpriteRenderer start;
  [SerializeField] private SpriteRenderer[] body;
  [SerializeField] private SpriteRenderer end;
  [SerializeField] private float lifeAfterHit = .1f;
  public override float LifetimeAfterHit => lifeAfterHit;
  
  private void SetSize(float size) {
    var startSprite = start.sprite;
    var endSprite = end.sprite;
    var startSize = startSprite.rect.width / startSprite.pixelsPerUnit;
    var endSize = endSprite.rect.width / endSprite.pixelsPerUnit;

    var bodySize = size - startSize - endSize;
    var bodySegmentSize = bodySize / body.Length;

    var startPos = startSize;

    foreach (var part in body) {
      var transform2 = part.transform;
      var position = transform2.localPosition;
      position = new Vector3(startPos, position.y, position.z);
      transform2.localPosition = position;
      part.size = new Vector2(bodySegmentSize, part.size.y);
      startPos += bodySegmentSize;
    }

    var transform1 = end.transform;
    var localPosition = transform1.localPosition;
    localPosition =
      new Vector3(startSize + bodySize, localPosition.y, localPosition.z);
    transform1.localPosition = localPosition;
  }

  public override IEnumerator FireAt(Vector2 point) {
    var dir = (point - Vector2.down * transform.localPosition.y ) - transform.position2D();
    var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    pivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    var dist = Vector2.Distance(transform.position2D(), point);
    SetSize(dist);
    transform.localScale = new Vector3(1, 0, 1);
    
    foreach (var spriteRenderer in body.Append(start).Append(end)) {
      var color = spriteRenderer.color;
      color = new Color(color.r, color.g, color.b, 0);
      spriteRenderer.color = color;
      spriteRenderer.DOFade(1, speed).SetEase(Ease.InQuad);
    }
    transform.DOScaleY(1, speed).SetEase(Ease.OutQuad).OnComplete(() => {
      foreach (var spriteRenderer in body.Append(start).Append(end)) {
        spriteRenderer.DOComplete();
        spriteRenderer.DOFade(0, speed).SetEase(Ease.OutQuad);
      }
    });
    Destroy(gameObject, speed * 2);
    yield return new WaitForSeconds(speed);
  }


  public override float CalcTimeToHit(Vector2 _) => speed;
}
}