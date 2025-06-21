using System.Collections;
using UnityEngine;

public class TutorialPIP : MonoBehaviour
{
    public static TutorialPIP Instance;

    public SpeechBubble bubble;

    private int textIndex = 0;

    private string[] tutorial_lines =
    {
        "Hello there!",
        "General Kenobi!"
    };

    public float initialDelay;
    public float lineDelay;
    private bool LineDone;
    private bool ActionDone = false;

    private void Start()
    {
        Instance = this;
        bubble.SetNotifyAction(LineEnded);
        StartCoroutine("Tutorial");
    }

    public void LineEnded()
    {
        LineDone = true;
    }

    private void ShowNextLine()
    {
        bubble.SetText(tutorial_lines[textIndex]);
        textIndex++;
        LineDone = false;
        ActionDone = false;
    }

    IEnumerator Tutorial()
    {
        yield return new WaitForSeconds(initialDelay);
        ShowNextLine();
        while (!LineDone)
        {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(lineDelay);
        ShowNextLine();
        while (!LineDone)
        {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(lineDelay);
        bubble.SetText("Habibi");
    }
}
