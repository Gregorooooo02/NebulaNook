using Unity.Collections;
using UnityEngine;

public class BarChairScript : MonoBehaviour
{
    public GameObject AccessPoint;
    public GameObject SeatPoint;

    public ClientController Occupier;

    //[HideInInspector]
    public bool Occupied = false;
}
