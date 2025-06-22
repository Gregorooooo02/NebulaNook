using UnityEngine;

public class PIPHelper : MonoBehaviour
{
    public static PIPHelper Instance;

    public Transform MeteorSpawn;
    public Transform[] CloneSpawningLocations;

    public GameObject TableHandle;
    public Rigidbody TableRB;

    public bool enableCollisionsForMiniTable = false;
 
    private void Start()
    {
        Instance = this;
    }

    public void ToggleTablePhysics(bool isActive)
    {
        TableRB.isKinematic = !isActive;
        if(!enableCollisionsForMiniTable)TableRB.useGravity = !isActive;
        if(enableCollisionsForMiniTable)TableHandle.GetComponent<BoxCollider>().enabled = isActive;
    }

    public void ResetTable()
    {
        ToggleTablePhysics(false);
        TableHandle.transform.localPosition = Vector3.zero;
        TableHandle.transform.localRotation = Quaternion.identity;
    }
}
