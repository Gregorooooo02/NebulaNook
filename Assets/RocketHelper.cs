using UnityEngine;

public class RocketHelper : MonoBehaviour
{
    public GameObject Model;
    public GameObject Parent;

    public void DisableModel()
    {
        Model.SetActive(false);
    }

    public void DestroyObject()
    {
        Destroy(Parent);
    }
}
