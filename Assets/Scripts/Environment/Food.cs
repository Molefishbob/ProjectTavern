using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : PlayerUseable
{
    [SerializeField][Range (15, 40)]
    private int _temperature = 40;
    [SerializeField]
    public int MiniumServeableTemp = 25;
    private ScaledOneShotTimer _tempTimer = null;
    [SerializeField]
    private float _baseTempTime = 2f;
    [SerializeField]
    private int _baseTempDrop = 5;
    [SerializeField]
    private SpriteRenderer _selfRenderer = null;

    protected override void Awake()
    {
        base.Awake();

        _timer.OnTimerCompleted += PotAction;

        _tempTimer = gameObject.AddComponent<ScaledOneShotTimer>();
        _tempTimer.OnTimerCompleted += ManipulateTemperature;
        _tempTimer.TimerTag = "Temperature Timer";
        _tempTimer.StartTimer(_baseTempTime);
    }

    private void ManipulateTemperature()
    {
        _temperature -= _baseTempDrop;
        _temperature = Mathf.Clamp(_temperature, 15, 40);

        _tempTimer.StartTimer(_baseTempTime * Random.Range(0.5f, 1.5f));
        _selfRenderer.color = Color.Lerp(Color.blue, Color.red, (float)(_temperature - 15) / (40 - 15));
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _timer.OnTimerCompleted -= PotAction;
        _tempTimer.OnTimerCompleted -= ManipulateTemperature;
    }

    public override void Use(PlayerState player)
    {
        base.Use(player);

        _tempTimer.StopTimer();
    }

    public override void InterruptAction()
    {
        base.InterruptAction();
        _tempTimer.StartTimer(_baseTempTime * Random.Range(0.5f, 1.5f));
    }

    private void PotAction()
    {
        if (_temperature > MiniumServeableTemp)
            GetFood();
        else
            ConfuseThePot();

        _tempTimer.StartTimer(_baseTempTime * Random.Range(0.5f, 1.5f));
        _selfRenderer.color = Color.Lerp(Color.blue, Color.red, (float)(_temperature - 15) / (40 - 15));
    }

    private void GetFood()
    {
        User.CurrentlyHeld = PlayerState.Holdables.Food;
        Debug.Log("You got some yumyum");
    }

    private void ConfuseThePot()
    {
        _temperature += _baseTempDrop * 2;
    }
}