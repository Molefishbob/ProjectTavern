using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Managers;

public class InGameUI : MonoBehaviour
{
    public TimeCounter _dayCounter;
    [SerializeField]
    protected Slider _happiness;
    [SerializeField]
    protected TMP_Text _money;

    private void OnEnable()
    {
        ChangeHappiness(LevelManager.Instance.Happiness);
        ChangeMoney(LevelManager.Instance.CurrentMoney);
        LevelManager.Instance.OnHappinessChanged += ChangeHappiness;
        LevelManager.Instance.OnMoneyChanged += ChangeMoney;
    }

    private void OnDestroy()
    {
        LevelManager.Instance.OnHappinessChanged -= ChangeHappiness;
        LevelManager.Instance.OnMoneyChanged -= ChangeMoney;
    }

    public void ChangeHappiness(float value)
    {
        _happiness.value = value;
    }
    public void ChangeMoney(float value)
    {
        _money.SetText("GOLD : " + value);
    }
}