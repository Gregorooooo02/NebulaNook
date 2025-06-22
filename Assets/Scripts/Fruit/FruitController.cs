using UnityEngine;
using UnityEngine.Animations.Rigging;

public class FruitController : MonoBehaviour
{
    [Header("Fruit Prefabs")]
    [SerializeField] private GameObject slicedFruitPrefab;
    private Rigidbody rb;

    [Header("Fruit Slicing")]
    [SerializeField] private float sliceAmount = 4f;
    private float currentSlices = 0f;

    private bool canSlice = false;
    private CuttingBoardController cuttingBoard;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void EnableSlicing()
    {
        canSlice = true;
        cuttingBoard = FindObjectsByType<CuttingBoardController>(FindObjectsSortMode.None)[0];
    }
    public void DisableSlicing()
    {
        canSlice = false;
        cuttingBoard = null;
    }

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

    private void PerformSlice()
    {
        GameObject sliced = Instantiate(
            slicedFruitPrefab,
            transform.position,
            transform.rotation
        );

        TutorialManager.Instance?.NotifyCuttingBoardCut();

        if (cuttingBoard != null)
        {
            cuttingBoard.OnFruitSliced(gameObject, sliced);
        }

        FruitTracker tracker = GetComponent<FruitTracker>();
        if (tracker != null)
        {
            tracker.DestroyFruit();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
    }
}
