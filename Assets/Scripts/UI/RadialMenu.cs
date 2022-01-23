using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class RadialMenu : MonoBehaviour
    {
        [SerializeField] private RectTransform content;
        [SerializeField] private Image bg;
        [SerializeField] private float radius = 300;

        private Color _openColor;
        private Color _closedColor;

        private bool _open;

        private void Start() {
            _open = false;
            for (var i = 0; i < content.childCount; i++) {
                var child = content.GetChild(i).GetComponent<RectTransform>();
                child.localScale = Vector3.zero;
                child.anchoredPosition = Vector2.zero;
            }

            if (!bg) return;
            _openColor = bg.color;
            _closedColor = new Color(bg.color.a, bg.color.g, bg.color.b, 0);
            bg.color = _closedColor;
        }

        public void Open() {
            _open = true;
            bg.DOColor(_openColor, .2f).SetEase(Ease.OutQuad);
            var radiansOfSeperation = (Mathf.PI * 2) / content.childCount;
            for (var i = 0; i < content.childCount; i++) {
                var x = Mathf.Sin(radiansOfSeperation * i) * radius;
                var y = Mathf.Cos(radiansOfSeperation * i) * radius;

                var child = content.GetChild(i).GetComponent<RectTransform>();
                var menuItem = child.GetComponent<RadialMenuItem>();
                menuItem.enabled = true;
                child.DOComplete();
                child.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad).SetDelay(.05f * i);
                child.DOAnchorPos(new Vector2(x, y), .3f).SetEase(Ease.OutQuad).SetDelay(.05f * i);
            }
        }

        public void Close() {
            _open = false;
            bg.DOColor(_closedColor, .2f).SetEase(Ease.InQuad).SetDelay(.05f * content.childCount);
            for (var i = 0; i < content.childCount; i++) {
                var child = content.GetChild(i).GetComponent<RectTransform>();
                var menuItem = child.GetComponent<RadialMenuItem>();
                menuItem.enabled = false;
                child.DOComplete();
                child.DOScale(Vector3.zero, .3f).SetEase(Ease.InQuad).SetDelay(.05f * i);
                child.DOAnchorPos(Vector2.zero, .3f).SetEase(Ease.InQuad).SetDelay(.05f * i);
            }
        }

        public void Toggle() {
            if (_open) {
                Close();
            }
            else {
                Open();
            }
        }
    }
}