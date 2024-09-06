using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : Singleton<MenuController>
{
    public PanelSwitcher PanelSwitcher => panelSwitcher;
    [SerializeField] private PanelSwitcher panelSwitcher;

    [SerializeField] private Animator _playButtonAnimator;

    protected override void Awake()
    {
        base.Awake();

        panelSwitcher.ShowPanel("main");
    }

    private void Start()
    {
        string _argument = GameScenesManager.Instance.Argument;
        if (!string.IsNullOrWhiteSpace(_argument))
        {
            panelSwitcher.ShowPanel("levels");
        }
    }

    public void Play()
    {
        this.OnAnimation(_playButtonAnimator, "Explosion", LoadGame, 0.99f);
        _playButtonAnimator.SetTrigger("Explode");
    }

    public void LoadGame()
    {
        GameScenesManager.Instance.LoadGame();
    }
}
