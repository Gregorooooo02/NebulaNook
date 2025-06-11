using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FadeIntoScene(int sceneIndex)
    {
        StartCoroutine(FadeToSceneAsync(sceneIndex));
    }

    public void FadeIntoScene(string sceneName, Action onComplete = null)
    {
        StartCoroutine(FadeToSceneAsync(sceneName, onComplete));
    }

    public void FadeIntoScene(int sceneIndex, Action onComplete = null)
    {
        StartCoroutine(FadeToSceneAsync(sceneIndex, onComplete));
    }

    private IEnumerator FadeToSceneAsync(string sceneName, Action onComplete = null)
    {
        yield return new WaitUntil(() => FadeScreenController.Instance.IsFadingComplete);
        FadeScreenController.Instance.FadeOut();
        yield return new WaitUntil(() => FadeScreenController.Instance.IsFadingComplete);

        Scene currentGameScene = SceneManager.GetSceneByName("GameScene");
        if (currentGameScene.isLoaded)
        {
            yield return SceneManager.UnloadSceneAsync("GameScene");
        }

        var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!op.isDone)
        {
            yield return null;
        }

        FadeScreenController.Instance.FadeIn();
        yield return new WaitUntil(() => FadeScreenController.Instance.IsFadingComplete);

        onComplete?.Invoke();
    }

    private IEnumerator FadeToSceneAsync(int sceneIndex, Action onComplete = null)
    {
        yield return new WaitUntil(() => FadeScreenController.Instance.IsFadingComplete);
        FadeScreenController.Instance.FadeOut();
        yield return new WaitUntil(() => FadeScreenController.Instance.IsFadingComplete);

        var op = SceneManager.LoadSceneAsync(sceneIndex);
        while (!op.isDone)
        {
            yield return null;
        }

        FadeScreenController.Instance.FadeIn();
        yield return new WaitUntil(() => FadeScreenController.Instance.IsFadingComplete);

        onComplete?.Invoke();
    }
}