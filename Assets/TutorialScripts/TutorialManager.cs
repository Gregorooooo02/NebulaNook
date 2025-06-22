using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public ClientSpawner ClientSpawner;
    public TutorialPIP Pip;

    private void Start()
    {
        Instance = this;
    }

    public void NotifyClientDoneGood()
    {
        Pip.ClientDoneGood = true;
    }

    public void NotifyClientDoneBad()
    {
        Pip.ClientDoneBad = true;
    }

    public void NotifyClientAproached()
    {
        Pip.ClientAproached = true;
    }

    public void NotifyFruitPicked()
    {
        Pip.FruitPicked = true;
    }

    public void NotifyGlassPicked()
    {
        Pip.GlassPicked = true;
    }

    public void NotifyOilPicked()
    {
        Pip.OilPicked = true;
    }

    public void NotifyBlowtorchPicked()
    {
        Pip.BlowtorchPicked = true;
    }

    public void NotifyBlowtorchTriggerPulled()
    {
        Pip.BlowtorchTriggerPulled = true;
    }

    public void NotifyShakerClosed()
    {
        Pip.ShakerClosed = true;
    }

    public void NotifyShakerOpened()
    {
        Pip.ShakerOpened = true;
    }

    public void NotifyShakerShook()
    {
        Pip.ShakerShook = true;
    }

    public void NotifyCuttingBoardPlaced()
    {
        Pip.CuttingBoardPlaced = true;
    }

    public void NotifyCuttingBoardCut()
    {
        Pip.CuttingBoardCut = true;
    }

    public void NotifyCleaverPicked()
    {
        Pip.CleaverPickedUp = true;
    }

    public void SpawnNextClient()
    {
        ClientSpawner.SpawnClient();
    }

}
