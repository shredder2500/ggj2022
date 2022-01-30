using System;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace.UI {

public class UpgradeMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
  [SerializeField] private Transform menu;
  [SerializeField] private Tower tower;
  [SerializeField] private Transform menuHoverArea;
  [SerializeField] private Image damageImage1, damageImage2, damageImage3;
  [SerializeField] private Image fireRateImage1, fireRateImage2, fireRateImage3;
  [SerializeField] private Image rangeImage1, rangeImage2, rangeImage3;
  [SerializeField] private TMP_Text levelElement;
  [SerializeField] private TMP_Text costElement;
  [SerializeField] private TMP_Text costLabel;
  [SerializeField] private Color upgradeColor;
  [SerializeField] private Color downgradeColor;
  [SerializeField] private UnityEvent notEnoughMoney;
  private Color _levelTextColor;
  private MoneyManager _moneyManager;

  private Vector3 _targetScale;

  public void ShowDowngrade() {
    UpdateDamage(tower.PrevDamage / (float)Tower.MaxDamage, tower.Damage / (float)Tower.MaxDamage, 0);
    UpdateRange(tower.PrevRange / Tower.MaxRange, tower.Range / Tower.MaxRange, 0);
    UpdateRate(Tower.MaxFireRate / tower.PrevRate, Tower.MaxFireRate / tower.FireRate, 0);
    costElement.DOComplete();
    costLabel.DOComplete();
    costElement.DOFade(1, .3f).SetEase(Ease.OutQuad);
    costLabel.DOFade(1, .3f).SetEase(Ease.OutQuad);
    costElement.text = tower.Refund.ToString();
    costElement.DOColor(upgradeColor, .3f).SetEase(Ease.OutQuad);
    if (tower.Level > 1) {
      levelElement.DOColor(downgradeColor, .3f).SetEase(Ease.OutQuad);
      levelElement.text = $"Lv {tower.Level - 1}";
    }
    else {
      levelElement.DOColor(downgradeColor, .3f).SetEase(Ease.OutQuad);
      levelElement.text = $"Remove";
    }
  }

  public void ShowUpgrade() {
    UpdateDamage(tower.Damage / (float)Tower.MaxDamage, 0, tower.NextDamage / (float)Tower.MaxDamage);
    UpdateRange(tower.Range / Tower.MaxRange, 0, tower.NextRange / Tower.MaxDamage);
    UpdateRate(Tower.MaxFireRate / tower.FireRate, 0, Tower.MaxFireRate / tower.NextRate);
    costElement.DOComplete();
    costLabel.DOComplete();
    costElement.DOFade(1, .3f).SetEase(Ease.OutQuad);
    costLabel.DOFade(1, .3f).SetEase(Ease.OutQuad);
    if (tower.Level < tower.MaxLevel && tower.Level > 0) {
      levelElement.DOColor(upgradeColor, .3f).SetEase(Ease.OutQuad);
      levelElement.text = $"Lv {tower.Level + 1}";
      costElement.text = tower.NextCost.ToString();
      costElement.DOColor(downgradeColor, .3f).SetEase(Ease.OutQuad);
    }
    else {
      levelElement.DOColor(_levelTextColor, .3f).SetEase(Ease.OutQuad);
      levelElement.text = $"Lv {tower.Level}";
      costElement.text = "At Max Lv";
      costElement.DOColor(upgradeColor, .3f).SetEase(Ease.OutQuad);
    }
  }

  public void ShowCurrentLevel() {
    UpdateDamage(tower.Damage / (float)Tower.MaxDamage, 0, 0);
    UpdateRange(tower.Range / Tower.MaxRange, 0, 0);
    UpdateRate(Tower.MaxFireRate / tower.FireRate, 0, 0);
    costElement.DOComplete();
    costLabel.DOComplete();
    levelElement.DOComplete();
    costElement.DOFade(0, .3f).SetEase(Ease.InQuad);
    costLabel.DOFade(0, .3f).SetEase(Ease.InQuad);
    levelElement.DOColor(_levelTextColor, .3f).SetEase(Ease.OutQuad);
    levelElement.text = $"Lv {tower.Level}";
  }

  public void Downgrade() {
    _moneyManager.Add(tower.Refund);
    tower.Downgrade();
    ShowDowngrade();
  }

  public void Upgrade() {
    if (tower.Level >= tower.MaxLevel) return;
    if (_moneyManager.Amount >= tower.NextCost) {
      _moneyManager.Remove(tower.NextCost);
      tower.Upgrade();
      ShowUpgrade();
    }
    else {
      notEnoughMoney.Invoke();
    }
  }

  private void UpdateDamage(float a, float b, float c) => UpdateBar(damageImage1, damageImage2, damageImage3, a, b, c);

  private void UpdateRate(float a, float b, float c) =>
    UpdateBar(fireRateImage1, fireRateImage2, fireRateImage3, a, b, c);

  private void UpdateRange(float a, float b, float c) => UpdateBar(rangeImage1, rangeImage2, rangeImage3, a, b, c);

  private void UpdateBar(Image a, Image b, Image c, float valueA, float valueB, float valueC) {
    a.DOComplete();
    b.DOComplete();
    c.DOComplete();

    a.DOFillAmount(valueA, .2f).SetEase(Ease.OutQuad);
    b.DOFillAmount(valueB, .05f).SetEase(Ease.OutQuad);
    c.DOFillAmount(valueC, .2f).SetEase(Ease.OutQuad);
  }

  private void OnWaveStart() {
    this.enabled = false;
  }

  private void OnWaveEnd() {
    this.enabled = true;
  }

  private void OnDisable() {
    Hide();
  }

  private void Start() {
    _moneyManager = FindObjectOfType<MoneyManager>();
    _targetScale = menu.localScale;
    _levelTextColor = levelElement.color;
    menu.localScale = Vector3.zero;
    menuHoverArea.localScale = Vector3.zero;
    FindObjectOfType<EnemySpawner>().OnWaveStart += OnWaveStart;
    FindObjectOfType<EnemySpawner>().OnWaveEnd += OnWaveEnd;
  }

  public void Show() {
    menuHoverArea.DOComplete();
    menuHoverArea.localScale = _targetScale;
    menu.DOComplete();
    menu.DOScale(_targetScale, .3f).SetEase(Ease.OutQuad);
    ShowCurrentLevel();
  }
  
  public void Hide() {
    menu.DOComplete();
    menuHoverArea.DOComplete();
    menu.DOScale(Vector3.zero, .3f).SetEase(Ease.InQuad).SetDelay(.5f);
    menuHoverArea.DOScale(Vector3.zero, .01f).SetDelay(.8f);
  }

  public void OnPointerEnter(PointerEventData eventData) {
    Show();
  }

  public void OnPointerExit(PointerEventData eventData) {
    Hide();
  }
}
}