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

    public void SpawnNextClient()
    {
        ClientSpawner.SpawnClient();
    }

}
