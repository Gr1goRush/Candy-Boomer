using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void GameActionFloat(float value);

public class GameScenesManager : Singleton<GameScenesManager>
{
    public string Argument { get; private set; }
    public bool SceneLoaded { get; private set; }

    private bool allowedSceneActivation = false;

    public event GameAction OnSceneLoaded;

    const string menuSceneName = "Menu", gameSceneName = "Game";

    public void LoadMenu(string argument = null, bool disallowSceneActivation = false)
    {
        Argument = argument;
        LoadScene(menuSceneName, disallowSceneActivation);
    }

    public void LoadGame()
    {
        Argument = null;
        LoadScene(gameSceneName, false);
    }

    private void LoadScene(string sceneName, bool disallowSceneActivation)
    {
        allowedSceneActivation = !disallowSceneActivation;

        StartCoroutine(SceneLoading(sceneName));
    }

    IEnumerator SceneLoading(string sceneName)
    {
        //loadingSceneName = sceneName;

        SceneLoaded = false;
        //sceneLoading = true;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = allowedSceneActivation;
        while (asyncOperation.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForEndOfFrame();

        SceneLoaded = true;
        OnSceneLoaded?.Invoke();

        Time.timeScale = 1f;

        if (!allowedSceneActivation)
        {
            yield return new WaitUntil(() => allowedSceneActivation);

            asyncOperation.allowSceneActivation = true;
        }
    }

    public void AllowSceneActivatetion()
    {
        allowedSceneActivation = true;
    }
}