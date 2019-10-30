using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitMenu : MonoBehaviour
{
    public void ConfirmQuit()
    {
        Application.Quit();
#if UNITY_EDITOR
        Debug.Log("Game quitted");
#endif
    }

    public void CancelQuit()
    {
        gameObject.SetActive(false);
    }
}
