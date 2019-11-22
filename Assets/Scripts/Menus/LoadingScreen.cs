using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Managers;

public class LoadingScreen : MonoBehaviour
{
    private int _loadState = -1;
    private ScaledOneShotTimer _timer;

    private void Start()
    {
        if (GameManager.Instance.LoadingScreen != null && GameManager.Instance.LoadingScreen != this.gameObject)
        {
            print("kys");
            Destroy(gameObject);
        } else
        {
            GameManager.Instance.LoadingScreen = this.gameObject;
        }
        gameObject.SetActive(false);
        _timer = gameObject.AddComponent<ScaledOneShotTimer>();
        _timer.OnTimerCompleted += ShutDown;
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (_loadState < 0) return;

        if (_loadState < 1)
        {
            _loadState++;
            return;
        }

        if (_loadState == 1)
        {
            _loadState = -1;
            _timer.StartTimer(1f);
            return;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= Begin;
        _timer.OnTimerCompleted -= ShutDown;
    }

    private void ShutDown()
    {
        GameManager.Instance.TemporaryMuteOff();
        gameObject.SetActive(false);
    }

    public void BeginLoading()
    {
        _loadState = -1;
        GameManager.Instance.TemporaryMute();
        SceneManager.sceneLoaded += Begin;
        gameObject.SetActive(true);
    }

    private void Begin(Scene scene, LoadSceneMode mode)
    {
        //GameManager.Instance.Camera.PlayerControlled = false;

        _loadState = 0;
    }
}
