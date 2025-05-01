using UnityEngine;

public class ChairManager : MonoBehaviour
{
    public BarChairScript[] BarChairs;
    public GameObject ExitPoint;
    public static ChairManager Instance;

    void Start()
    {
        BarChairs = FindObjectsByType<BarChairScript>(FindObjectsInactive.Exclude,FindObjectsSortMode.None);
        Instance = this;
    }

    public BarChairScript ReserveAnyAvailableChair()
    {
        foreach (var chair in BarChairs)
        {
            if (!chair.Occupied)
            {
                chair.Occupied = true;
                return chair;
            }
        }
        return null;
    }

    public void VacateChair(BarChairScript chair)
    {
        chair.Occupied = false;
    }
}
