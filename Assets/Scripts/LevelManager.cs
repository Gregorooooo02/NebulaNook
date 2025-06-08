using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void FadeIntoScene(int index)
    {
        StartCoroutine(FadeToSceneAsync(index));
    }

    private IEnumerator FadeToSceneAsync(int index)
    {
        yield return new WaitUntil(() => FadeScreenController.Instance.IsFadingComplete);
        FadeScreenController.Instance.FadeOut();
        yield return new WaitUntil(() => FadeScreenController.Instance.IsFadingComplete);
        
        var op = SceneManager.LoadSceneAsync(index);
        while (!op.isDone)
        {
            yield return null;
        }
    }
}
