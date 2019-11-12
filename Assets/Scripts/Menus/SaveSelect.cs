using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Managers;

public class SaveSelect : MonoBehaviour
{
    [SerializeField]
    protected TMP_Text _save1, _save2, _save3;
    [SerializeField]
    protected GameObject _mainmenu;

    private void OnEnable()
    {
        string[] saveFiles = SerializationManager.GetAllSaveFilenames();

        foreach (string save in saveFiles)
        {
            switch (save)
            {
                case "save1":
                    _save1.text = "Continue";
                    break;
                case "save2":
                    _save2.text = "Continue";
                    break;
                case "save3":
                    _save3.text = "Continue";
                    break;
            }
        }
    }

    public void BackToMainMenu()
    {
        _mainmenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SetCurrentSave(string save)
    {
        GameManager.Instance.SelectSave(save);
    }
}
