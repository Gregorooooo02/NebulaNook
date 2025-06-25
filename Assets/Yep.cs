using UnityEngine;

public class Yep : MonoBehaviour
{
    public ToungeCordinator cordinator;
    public void RestoreAnimation()
    {
        cordinator.Resume();
    }
}
