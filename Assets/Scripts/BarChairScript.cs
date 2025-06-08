using Unity.Collections;
using UnityEngine;

public class BarChairScript : MonoBehaviour
{
    public GameObject AccessPoint;
    public GameObject SeatPoint;
    public GameObject ZonePoint;

    public ClientController Occupier;

    //[HideInInspector]
    public bool Occupied = false;

    private void Update()
    {
        if (Occupied)
        {
            ZonePoint.SetActive(true);
        }
        else
        {
            if (ZonePoint.activeSelf)
            {
                var anim = ZonePoint.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.Play("Hide");
                }
                // If the animation finishes, disable the ZonePoint
                ZonePoint.SetActive(false);
            }
        }
    }
}
