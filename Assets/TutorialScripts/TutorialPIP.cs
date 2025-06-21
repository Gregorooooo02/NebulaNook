using System.Collections;
using UnityEngine;

public class TutorialPIP : MonoBehaviour
{
    public static TutorialPIP Instance;

    public SpeechBubble bubble;

    private int textIndex = 0;

    private string[] tutorial_lines =
    {
        "Hello there! I am Pip I see that you are our new bartender!",
        "All the previous bartenders didn't last very long... But! I belive you will!",
        "I will teach you how things work around here. Let's start from the basics.",
        "Oh! Here goes our first client! I will walk you through the process.",
        "This customer wants a drink with explosive effect so give him one!",
        "First grab the shaker and open it.",
        "Great!",
        "Behind you there is a counter with the finest liquors on this side of galaxy!",
        "For now there is only one bottle there. Grab it!",
        "Now pour it into the shaker! When you're done pick up the blowtorch from the drawer on the right side of the bar.",
        "This one you can't just pour. Try pulling the trigger.",
        "Great! Now use it to pour into the shaker! When you finish close the shaker.",
        "Now shake it with all you got!!",
        "Thats the spirit!",
        "Now it's time to pour the drink into the glass. Notice that in front of the client there is a hologram of the glass.",
        "It shows the type of the glass client wants the drink in. You don't have to use the exact glass but the client will pay extra if you will.",
        "Now grab the glass from the rack above.",
        "Now just open the shaker, pour it's contents into the glass and give it to the customer.",
        "Oh there comes another customer!",
        "The same effect again huh? Explosives are popular these days...",
        "Anyways this time customer also wants the fruit with the drink. You can see the type of the desired fruit as the hologram.",
        "The fruits are the same as the glasses. As in you get a bonus for giving the right one.",
        "I pre prepared the drink for you. This time your task is to prepare the fruit.",
        "The containers with the fruits are on the left side of the bar.",
        "Now grab the red fruit and place it on the cutting board on the right. But be careful! It's extremly volatile and will explode if handled too roughly.",
        "Good job! You have a talent handling explosives I see! You are perfect for this job then.",
        "Now grab the cleaver! It should be next to the cutting board.",
        "Ah yes an elegant tool, for more civilized bar...",
        "Now! Smack the fruit with it!!",
        "*Ehm* excuse me. Now pick up the slice and put it on the glass.",
        "All done! Give finished drink to the customer.",
        "Good job! Just remember that there are more liquors available and a LOT more possible effects.",
        "For example there is a liquor tap on the right, thought it's out of order today.",
        "And if you're ever in doubt about the drink effect you can test it on me! Even if I'd rather you didn't!",
        "Also there is a screen with the recipes on the counter. It was supposed to show you the recipes for the effects that the customers ask for.",
        "Unfortunately effect database got damaged and now recipes are incomplete. Still it may prove useful.",
        "Well that's it for now. Good luck on your job. You'll need it!"
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

    public Material Glass1Outline;
    public Material OilBottleOutline;
    public Material BlowtorchBottleOutline;
    public Material ShakerOutline;
    public Material FruitOutline;
    public Material CuttingBoardOutline;
    public Material CleaverOutline;

    public GameObject explosiveGlass;

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
        SlicePutOnGlass = false;
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
        yield return new WaitUntil(() => LineDone && ShakerOpened && (ClientDoneBad || ClientDoneGood));
        yield return new WaitForSeconds(1.0f);
        if (ClientDoneGood)
        {
            bubble.SetText("Good job! You're a natural!");
            LineDone = false;
        } 
        else if (ClientDoneBad)
        {
            bubble.SetText("Well, better luck next time.");
            LineDone = false;
        }
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(1.0f);
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
        explosiveGlass.SetActive(true);
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
        ShowNextLine(); // Put the slice on!
        yield return new WaitUntil(() => LineDone && SlicePutOnGlass);
        yield return new WaitForSeconds(1.5f);
        ShowNextLine(); // All done!
        yield return new WaitUntil(() => LineDone && (ClientDoneBad || ClientDoneGood));
        yield return new WaitForSeconds(1);
        ShowNextLine(); // Good job!
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(lineDelay);
        ShowNextLine(); // Liquor tap
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(lineDelay);
        ShowNextLine(); // Effect testing
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(lineDelay);
        ShowNextLine(); // Recipe screen
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(lineDelay);
        ShowNextLine(); // Recipes incomplete
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(lineDelay);
        ShowNextLine(); // Good luck!
        yield return new WaitUntil(() => LineDone);
        yield return new WaitForSeconds(lineDelay);
    }
}
