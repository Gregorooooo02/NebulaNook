using UnityEngine;

public class SearchForChair : ClientState
{
    public AproachBar nextState;

    public override ClientState RunState()
    {
        BarChairScript result = Controller.Spawner.barChairScript;
        result.Occupier = Controller;
        result.Occupied = true;
        nextState.SetDestination(result);
        return nextState;
    }
}
