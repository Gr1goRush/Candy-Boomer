using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootstrapController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private bool animationFinished = false, sceneLoaded = false;

    void Start()
    {
        this.OnAnimation(_animator, "Explosion", OnAnimationFinished, 0.99f);
        _animator.SetTrigger("Explode");

        GameScenesManager.Instance.OnSceneLoaded += OnSceneLoaded;
        GameScenesManager.Instance.LoadMenu(null, true);
    }

    void OnAnimationFinished()
    {
        animationFinished = true;
        CheckLoading();
    }

    void OnSceneLoaded()
    {
        sceneLoaded = true;
        CheckLoading();
    }

    private void CheckLoading()
    {
        if(sceneLoaded && animationFinished) 
        {
            GameScenesManager.Instance.AllowSceneActivatetion();
        }
    }

    private void OnDestroy()
    {
        if (GameScenesManager.Instance != null)
        {
            GameScenesManager.Instance.OnSceneLoaded -= OnSceneLoaded;
        }
    }
}
