using System.Collections;
using UnityEngine;

public class TutorialPIP : MonoBehaviour
{
    public static TutorialPIP Instance;

    public SpeechBubble bubble;

    private int textIndex = 0;

    private string[] tutorial_lines =
    {
        "Hello there! I am Pip I see that you are our new barman!",
        "All the previous barmans didn't last very long... But! I belive you will!",
        "I will teach you how things work around here. Let's start from the basics.",
        "Oh! Here goes our first client! I will walk you through the process."
    };

    public float initialDelay;
    public float lineDelay;
    private bool LineDone;
    private bool ActionDone = false;

    public bool ClientDoneGood = false;
    public bool ClientDoneBad = false;

    public bool ClientAproached = false;

    public bool FruitPicked = false;
    public bool GlassPicked = false;    


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
        ClientAproached = false;
        ClientDoneGood = false;
        ClientDoneBad = false;
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
        ShowNextLine();
        while (!LineDone)
        {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(lineDelay);
        TutorialManager.Instance.SpawnNextClient();
        ShowNextLine();
        while (!LineDone || !ClientAproached)
        {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(lineDelay);
        
    }
}
