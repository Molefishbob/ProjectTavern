using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUI : MonoBehaviour
{
    public TimeCounter _dayCounter;
    [SerializeField]
    protected float _levelTime;
    [SerializeField]
    protected Image _happiness;
    [SerializeField]
    protected TMP_Text _money;

}