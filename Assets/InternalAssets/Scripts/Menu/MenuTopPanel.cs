using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTopPanel : MonoBehaviour
{
    public void Back()
    {
        MenuController.Instance.PanelSwitcher.ShowPanel("main");
    }
}
