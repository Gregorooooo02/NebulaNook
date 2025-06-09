using UnityEngine;

public class FreezePIP : PipState
{
    public override PipState RunState()
    {
        return this;
    }
}
