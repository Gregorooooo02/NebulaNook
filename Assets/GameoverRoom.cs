using UnityEngine;

public class GameoverRoom : MonoBehaviour
{
    public static GameoverRoom Instance;

    private void Start()
    {
        Instance = this;
    }
}
