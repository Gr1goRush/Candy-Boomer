using System.Collections;
using UnityEngine;

public class GameUIContainer : MonoBehaviour
{
    public MainPanel MainPanel => mainPanel;
    [SerializeField] private MainPanel mainPanel;

    public PausePanel PausePanel => pausePanel;
    [SerializeField] private PausePanel pausePanel;

    public SuperGamePanel SuperGamePanel => superGamePanel;
    [SerializeField] private SuperGamePanel superGamePanel;

    [SerializeField] private LosePanel losePanel;
    [SerializeField] private WinPanel winPanel;

    public void ShowLosePanel()
    {
        losePanel.gameObject.SetActive(true);
        mainPanel.gameObject.SetActive(false);
    }

    public void ShowWinPanel(int starsCount, int score, int coins)
    {
        winPanel.Show(starsCount, score, coins);
        mainPanel.gameObject.SetActive(false);
    }
}