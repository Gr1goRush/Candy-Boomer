using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePanel : MonoBehaviour
{
    [SerializeField] private StarsFrame starsFrame;

    void Start()
    {
        starsFrame.Set(0);    
    }

    public void Restart()
    {
        GameScenesManager.Instance.LoadGame();
    }

    public void Levels()
    {
        GameScenesManager.Instance.LoadMenu("levels");
    }
}
