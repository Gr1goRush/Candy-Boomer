using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    public void Restart()
    {
        GameScenesManager.Instance.LoadGame();
    }

    public void Levels()
    {
        GameScenesManager.Instance.LoadMenu("levels");
    }

    public void Continue()
    {
        GameController.Instance.UnPause();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
