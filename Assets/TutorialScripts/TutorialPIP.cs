using System.Collections;
using UnityEngine;

public class TutorialPIP : MonoBehaviour
{
    public static TutorialPIP Instance;

    public SpeechBubble bubble;

    private int textIndex = 0;

    private string[] tutorial_lines =
    {
        "Hello there! My name is Pip and I'll introduce You to Your new, wonderful job!",
        "All of the previous bartenders didn't last very long... But don't worry, I doubt You'll share their fate!",
        "I will teach You how things work around here. Let's start with the basics.",
        "Oh, look here! Here goes Our first client! I will walk You through the process.",
        "This customer wants a drink with an explosive effect, so let's prepare them one!",
        "First, open the shaker by grabbing the lid.",
        "Great! I can already see that You have a knack!",
        "Behind You, on the counter, You can find the finest ingredients in this side of the galaxy!",
        "To make it easier for You, we'll focus on only one for now - the Motor oil. Go get it and I'll show you how we prepare drinks around here!",
        "I can see you have the oil, awesome! Let's pour it about halfway and fill the rest with fire - use the burner which You can find in the drawer on the right.",
        "One important thing - You can't just pour out the fire like that! Push the trigger and see the magic by yourself!",
        "Great! Now fill the rest of the shaker with fire! If You fill the shaker too much with oil, do not worry! Pour out some and add the fire. When You're done, close the shaker by putting the lid back on.",
        "Now, use the big muscles of Yours and mix it vigorously!",
        "Thats the spirit! Keep going!",
        "Great job! Now, let's grab some glass and serve the goods. Notice that in front of the client, there is a hologram of the glass.",
        "It shows the type of the glass client wants the drink in. You don't have to use the exact glass, but the client will pay extra if You do.",
        "You can find various glasses on the rack above you, grab one and let's keep going.",
        "Now, just open the shaker, pour it's contents into the glass and serve it to the customer.",
        "Oh, another customer! Let's serve them too!",
        "Explosives again, huh? This one is popular these days... Can't blame them, the explosion of flavor is out of this world!",
        "Anyways... This time the customer also wants the fruit with the drink. You can see the type of the desired fruit as the hologram, above the glass hologram.",
        "Fruits are not required either, but the extra cash is always good.",
        "I've already prepared the drink for You, let's focus on the fruit.",
        "You can find fruits on the left side of the bar.",
        "Grab the red one and put it on the cutting board. One thing though, this fruit requires extra care, due to it's fragile and explosive nature!",
        "I've never seen this level of competence with explosives before! You must've done it before!",
        "Next, You'll need something to cut it with. I think I've left the cleaver somewhere next to the board...",
        "I can see You found it. An extraodinary piece of technology, isn't it?",
        "Now, chop the fruit up!",
        "Good job! Grab the fruit slice and let's decorate the glass!",
        "Already looking more elegant, I could chug it up myself!",
        "You did great, but keep in mind that the real world scenario is much more complicated than this - way more ingredients, way more outcomes!",
        "I could show you how the tap in the front tap works, but the damn thing broke...",
        "I think I have one more tip for you - You could notice the screen with the recipes in the front. It used to show bartenders the requested recipes.",
        "For some reason the database is corrupted and the recipes are incomplete. It ain't much but it may be useful.",
        "I'll supervise you for the first couple of weeks. If you have prepared a drink and you are too afraid to serve it to the customer, try it on me first!",
        "That's it for now. Good luck on your new career path, You'll definetly need it!"
    };

    public float initialDelay;
    public float lineDelay;
    private bool LineDone;

    public bool ClientDoneGood = false;
    public bool ClientDoneBad = false;

    public bool ClientAproached = false;

    public bool FruitPicked = false;
    public bool GlassPicked = false;

    public bool OilPicked = false;
    public bool BlowtorchPicked = false;

    public bool BlowtorchTriggerPulled = false;

    public bool ShakerOpened = false;
    public bool ShakerClosed = false;
    public bool ShakerShook = false;
    public bool ShakerDone = false;

    public bool CuttingBoardPlaced = false;
    public bool CuttingBoardCut = false;

    public bool CleaverPickedUp = false;

    public bool SlicePutOnGlass = false;

    public bool SpilledGlass = false;
    public bool BrokenGlass = false;

    public bool GlassShouldRespawn = false;
    private bool ProgressLine = false;

    public int wastedGlassCount = 0;
    public int wastedGlassesLimit = 10;

    public bool yesFlag = false;

    public Material Glass1Outline;
    public Material OilBottleOutline;
    public Material BlowtorchBottleOutline;
    public Material ShakerOutline;
    public Material FruitOutline;
    public Material CuttingBoardOutline;
    public Material CleaverOutline;

    public GameObject explosiveGlass;

    public ZoneScript zone;

    private void Start()
    {
        Instance = this;
        bubble.SetNotifyAction(LineEnded);
        StartCoroutine("Tutorial");
        zone.Enabled = false;

        Glass1Outline.SetFloat("_Active", 0);
        OilBottleOutline.SetFloat("_Active", 0);
        BlowtorchBottleOutline.SetFloat("_Active", 0);
        ShakerOutline.SetFloat("_Active", 0);
        FruitOutline.SetFloat("_Active", 0);
        CuttingBoardOutline.SetFloat("_Active", 0);
        CleaverOutline.SetFloat("_Active", 0);
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
        FruitPicked = false;
        GlassPicked = false;
        OilPicked = false;
        BlowtorchPicked = false;
        BlowtorchTriggerPulled = false;
        ShakerOpened = false;
        ShakerClosed = false;
        ShakerShook = false;
        ShakerDone = false;
        CuttingBoardPlaced = false;
        CuttingBoardCut = false;
        CleaverPickedUp = false;
        //SlicePutOnGlass = false;
    }

    IEnumerator Tutorial()
    {
        yield return new WaitForSeconds(initialDelay);
        ShowNextLine();
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(lineDelay);
        ShowNextLine();
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(lineDelay);
        ShowNextLine();
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(lineDelay);
        TutorialManager.Instance.SpawnNextClient();
        ShowNextLine();
        yield return new WaitUntil(() => LineDone && ClientAproached);
        yield return new WaitForSeconds(lineDelay);
        ShowNextLine();
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(lineDelay);
        ShakerOutline.SetFloat("_Active", 1);
        ShowNextLine();
        yield return new WaitUntil(() => LineDone && ShakerOpened);
        ShakerOutline.SetFloat("_Active", 0);
        ShowNextLine();
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(0.5f);
        ShowNextLine();
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(lineDelay);
        ShowNextLine();
        OilBottleOutline.SetFloat("_Active", 1);
        yield return new WaitUntil(() => LineDone && OilPicked);
        OilBottleOutline.SetFloat("_Active", 0);
        yield return new WaitForSeconds(lineDelay);
        ShowNextLine();
        BlowtorchBottleOutline.SetFloat("_Active", 1);
        yield return new WaitUntil(() => LineDone && BlowtorchPicked);
        BlowtorchBottleOutline.SetFloat("_Active", 0);
        yield return new WaitForSeconds(lineDelay);
        ShowNextLine();
        yield return new WaitUntil(() => LineDone && BlowtorchTriggerPulled);
        yield return new WaitForSeconds(lineDelay);
        ShowNextLine();
        yield return new WaitUntil(() => LineDone && ShakerClosed);
        yield return new WaitForSeconds(lineDelay);
        ShowNextLine();
        yield return new WaitUntil(() => LineDone && ShakerShook);
        yield return new WaitForSeconds(0.5f);
        ShowNextLine();
        yield return new WaitUntil(() => LineDone && ShakerDone);
        yield return new WaitForSeconds(lineDelay);
        ShowNextLine();
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(lineDelay);
        ShowNextLine();
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(3);
        ShowNextLine();
        Glass1Outline.SetFloat("_Active", 1);
        yield return new WaitUntil(() => LineDone && GlassPicked);
        yield return new WaitForSeconds(lineDelay);
        Glass1Outline.SetFloat("_Active", 0);
        ShowNextLine();
        zone.Enabled = true;
        yield return new WaitUntil(() => LineDone && ShakerOpened && (ClientDoneBad || ClientDoneGood));
        yield return new WaitForSeconds(1.0f);
        if (ClientDoneGood)
        {
            bubble.SetText("You did awesome!");
            LineDone = false;
        }
        else if (ClientDoneBad)
        {
            bubble.SetText("Well, nobody's perfect the first time.");
            LineDone = false;
        }
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(1.0f);
        zone.Enabled = false;
        TutorialManager.Instance.SpawnNextClient();
        ShowNextLine(); //oh another client
        yield return new WaitUntil(() => LineDone && ClientAproached);
        ShowNextLine(); //popular effect
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(1);
        ShowNextLine(); //anyways
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(lineDelay);
        ShowNextLine(); //fruits and glasses
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(1.5f);
        TutorialManager.Instance?.SpawnNewGlass();
        GlassShouldRespawn = true;
        ShowNextLine(); //pre prepared
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(1.5f);
        ShowNextLine(); //fruit containters
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(1.0f);
        FruitOutline.SetFloat("_Active", 1);
        CuttingBoardOutline.SetFloat("_Active", 1);
        ShowNextLine(); //grab fruit and place on board
        yield return new WaitUntil(() => LineDone && FruitPicked && CuttingBoardPlaced);
        yield return new WaitForSeconds(lineDelay);
        FruitOutline.SetFloat("_Active", 0);
        CuttingBoardOutline.SetFloat("_Active", 0);
        ShowNextLine(); //good at handling explosives
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(1.0f);
        CleaverOutline.SetFloat("_Active", 1);
        ShowNextLine(); //Shinji get into the Cleaver!
        yield return new WaitUntil(() => LineDone && CleaverPickedUp);
        yield return new WaitForSeconds(0.5f);
        CleaverOutline.SetFloat("_Active", 0);
        ShowNextLine(); //Civilized tool
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(2.5f);
        ShowNextLine(); // Smack it!
        yield return new WaitUntil(() => LineDone && CuttingBoardCut);
        yield return new WaitForSeconds(0.5f);
        do
        {
            if (BrokenGlass || SpilledGlass)
            {
                if (BrokenGlass)
                {
                    bubble.SetText("This is where I would tell you to put the slice on the glass... Where is the glass?! Fortunately I have a spare!");
                    LineDone = false;
                    GlassShouldRespawn = false;
                    TutorialManager.Instance?.SpawnNewGlass();
                    wastedGlassCount++;
                    yield return new WaitUntil(() => LineDone);
                    yield return new WaitForSeconds(1);
                    bubble.SetText("Now put the slice on the glass and give it to the customer.");
                    LineDone = false;
                    ProgressLine = true;
                }
                else if (SpilledGlass)
                {
                    bubble.SetText("The glass was more than half full last time I looked at it... Well, that doesn't matter. Here's another one!");
                    LineDone = false;
                    GlassShouldRespawn = false;
                    TutorialManager.Instance?.SpawnNewGlass();
                    wastedGlassCount++;
                    yield return new WaitUntil(() => LineDone);
                    yield return new WaitForSeconds(1);
                    bubble.SetText("Now, put the slice on the glass and give it to the customer.");
                    LineDone = false;
                    ProgressLine = true;
                }
            }
            else
            {
                ShowNextLine();// Put the slice on!
            }
            yield return new WaitUntil(() => LineDone && (SlicePutOnGlass || BrokenGlass || SpilledGlass));
            yield return new WaitForSeconds(1.5f);
            if (wastedGlassCount >= wastedGlassesLimit) GetTheFuckOut();
        } while (BrokenGlass || SpilledGlass);
        if (ProgressLine)
        {
            ProgressLine = false;
            textIndex++;
        }

        zone.Enabled = true;
        do
        {
            if (BrokenGlass || SpilledGlass)
            {
                if (BrokenGlass)
                {
                    bubble.SetText("Where is the glass?! Well no matter just give it to the customer.");
                    LineDone = false;
                    GlassShouldRespawn = false;
                    TutorialManager.Instance?.SpawnNewGlassWithFruit();
                    wastedGlassCount++;
                    ProgressLine = true;
                }
                else if (SpilledGlass)
                {
                    bubble.SetText("The glass was more than half full last time I looked at it... Well no matter just give it to the customer.");
                    LineDone = false;
                    GlassShouldRespawn = false;
                    TutorialManager.Instance?.SpawnNewGlassWithFruit();
                    wastedGlassCount++;
                    ProgressLine = true;
                }
            }
            else
            {
                ShowNextLine(); // All done!
            }
            yield return new WaitUntil(() => LineDone && (ClientDoneBad || ClientDoneGood || BrokenGlass || SpilledGlass));
            yield return new WaitForSeconds(1);
            if (wastedGlassCount >= wastedGlassesLimit) GetTheFuckOut();
        } while (BrokenGlass || SpilledGlass);
        GlassShouldRespawn = false;
        zone.Enabled = false;
        ShowNextLine(); // Good job!
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(lineDelay);
        ShowNextLine(); // Liquor tap
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(lineDelay);
        ShowNextLine(); // Recipe screen 
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(lineDelay);
        ShowNextLine(); // Recipes incomplete
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(lineDelay);
        ShowNextLine(); // Effect testing
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(lineDelay);
        ShowNextLine(); // Good luck!
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(lineDelay);
        // After this line, deactivate the bubble
        bubble.gameObject.SetActive(false);
        LevelManager.Instance.FadeIntoScene(2);
    }

    IEnumerator GetTheFuckOut()
    {
        bubble.SetText("You wasted so many drinks... I am done with you!");
        LineDone = false;
        yield return new WaitUntil(() => LineDone);
        bubble.gameObject.SetActive(false);
        LevelManager.Instance.FadeIntoScene(2);
    }

    public void SetAllDone()
    {
        ShakerOpened = true;
        ShakerClosed = true;
        ShakerShook = true;
        ShakerDone = true;
    }

    public void SetShakeDone(bool done)
    {
        ShakerDone = done;
    }
}
