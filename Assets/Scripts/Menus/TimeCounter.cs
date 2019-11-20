using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;

public class TimeCounter : MonoBehaviour
{
    [SerializeField]
    protected Image _dayImage;
    [SerializeField]
    protected Transform _indicator;
    protected GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (_gameManager.GamePaused) return;
        ChangeTime(LevelManager.Instance.LevelTime.NormalizedTimeElapsed);
    }

    public void ChangeTime(float normTimeElapsed)
    {
        _dayImage.fillAmount = Mathf.Clamp(1f - normTimeElapsed, 0f, 1f);

        _indicator.rotation = Quaternion.identity;
        _indicator.Rotate(new Vector3(0, 0, 90f - (180f * normTimeElapsed)));
    }
}
