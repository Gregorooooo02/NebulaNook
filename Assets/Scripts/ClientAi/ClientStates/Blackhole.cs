using UnityEngine;

public class Blackhole : ClientState
{
    public GameObject BlackHole;

    private bool triggered = false; 
    public override ClientState RunState()
    {
        if (!triggered)
        {
            Instantiate(BlackHole, transform);
            triggered = true;
        }
        return this;
    }
}
