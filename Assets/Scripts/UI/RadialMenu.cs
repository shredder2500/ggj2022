using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    [ExecuteInEditMode]
    public class RadialMenu : MonoBehaviour
    {
        [SerializeField] private RectTransform content;
        [SerializeField] private float radius = 300;
        [SerializeField] private UnityEvent onOpen;
        [SerializeField] private UnityEvent onClose;

        private Color _openColor;
        private Color _closedColor;

        private bool _open;

        private void Start() {
            if (!Application.isPlaying) return;
            _open = false;
            for (var i = 0; i < content.childCount; i++) {
                var child = content.GetChild(i).GetComponent<RectTransform>();
                child.localScale = Vector3.zero;
                child.anchoredPosition = Vector2.zero;
            }
        }

        public void Open() {
            _open = true;
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
            onOpen.Invoke();
        }

        public void Close() => Close(true);
        public void CloseWithoutEvents() => Close(false);

        private void Close(bool triggerEvents) {
            _open = false;
            for (var i = 0; i < content.childCount; i++) {
                var child = content.GetChild(i).GetComponent<RectTransform>();
                var menuItem = child.GetComponent<RadialMenuItem>();
                menuItem.enabled = false;
                child.DOComplete();
                child.DOScale(Vector3.zero, .3f).SetEase(Ease.InQuad).SetDelay(.05f * i);
                child.DOAnchorPos(Vector2.zero, .3f).SetEase(Ease.InQuad).SetDelay(.05f * i);
            }
            if (triggerEvents)
                onClose.Invoke();
        }

        private void Update() {
            if (Application.isPlaying) return;
            
            var radiansOfSeperation = (Mathf.PI * 2) / content.childCount;
            for (var i = 0; i < content.childCount; i++) {
                var x = Mathf.Sin(radiansOfSeperation * i) * radius;
                var y = Mathf.Cos(radiansOfSeperation * i) * radius;

                var child = content.GetChild(i).GetComponent<RectTransform>();
                var menuItem = child.GetComponent<RadialMenuItem>();
                menuItem.enabled = true;
                child.localScale = Vector3.one;
                child.anchoredPosition = new Vector2(x, y);
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