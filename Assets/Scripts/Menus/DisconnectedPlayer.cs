using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisconnectedPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _controllerText = null;
    [SerializeField] private GameObject _disabledOverLay = null;

    public string ControllerText { get => _controllerText.text; set => _controllerText.text = value; }
    public bool OverlayEnabled { get => _disabledOverLay.activeSelf; set => SetActiveIfInactive(value); }

    private void SetActiveIfInactive(bool enabled)
    {
        if (!_disabledOverLay.activeSelf && enabled)
            _disabledOverLay.SetActive(true);
        else if (_disabledOverLay.activeSelf && !enabled)
            _disabledOverLay.SetActive(false);
    }
}
