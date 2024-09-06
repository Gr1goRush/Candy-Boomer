using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    [SerializeField] private SwitchingPanel[] panels;

    public void ShowPanel(string panelName)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            bool isCurrentPanel = panels[i].PanelName == panelName;
            panels[i].gameObject.SetActive(isCurrentPanel);
        }
    }

    public void HideAll()
    {
        ShowPanel(null);
    }

    private void Reset()
    {
        panels = GetComponentsInChildren<SwitchingPanel>();
    }
}
