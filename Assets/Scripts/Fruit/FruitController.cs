using UnityEngine;

public class FruitController : MonoBehaviour
{
    [Header("Fruit Prefabs")]
    [SerializeField] private GameObject slicedFruitPrefab;

    [Header("Fruit Slicing")]
    [SerializeField] private float sliceAmount = 4f;
    private float currentSlices = 0f;

    private bool canSlice = false;

    public void EnableSlicing() {canSlice = true;}
    public void DisableSlicing() {canSlice = false;}

    void OnTriggerEnter(Collider other)
    {
        if (!canSlice) return;
        if (!other.CompareTag("CutTrigger")) return;
        currentSlices++;
        if (currentSlices >= sliceAmount)
        {
            PerformSlice();
        }
    }

    private void PerformSlice() {
        GameObject sliced = Instantiate(
            slicedFruitPrefab,
            transform.position,
            transform.rotation,
            transform.parent
        );

        Destroy(gameObject);
    }
}
