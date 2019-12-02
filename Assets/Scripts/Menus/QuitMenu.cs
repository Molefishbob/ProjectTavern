using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitMenu : MonoBehaviour
{
    public void ConfirmQuit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        Debug.Log("Game quitted");
#endif
    }

    public void CancelQuit()
    {
        gameObject.SetActive(false);

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(MainMenu.Menu._mainMenuDefaultBind);
            for (int i = 0; i < MainMenu.Menu.transform.childCount; i++)
            {
                MainMenu.Menu.transform.GetChild(i).GetComponent<UnityEngine.UI.Button>().interactable = true;
            }
        }
        else
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(PauseMenu.Menu.transform.GetChild(2).GetChild(3).gameObject);
            for (int i = 0; i < PauseMenu.Menu.transform.GetChild(2).childCount; i++)
            {
                PauseMenu.Menu.transform.GetChild(2).GetChild(i).GetComponent<UnityEngine.UI.Button>().interactable = true;
            }
        }
    }
}
