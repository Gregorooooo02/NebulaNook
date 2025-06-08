using System.Collections;
using UnityEngine;

public class Boom : MonoBehaviour
{
    public GameObject BoomPrefab;
    private GameObject boom;

    public float boomDuration;

    public GameObject[] meteor;

    public void Explode()
    {
        foreach(GameObject m in meteor)
        {
            m.SetActive(false);
        }
        if(boom != null) Destroy(boom);
        boom = Instantiate(BoomPrefab,gameObject.transform);
        StartCoroutine("RemoveBoom");
    }

    IEnumerator RemoveBoom()
    {
        yield return new WaitForSeconds(boomDuration);
        foreach (GameObject m in meteor)
        {
            m.SetActive(true);
        }
        Destroy(boom);
    }
}
