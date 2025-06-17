using UnityEngine;

public class FruitTracker : MonoBehaviour
{
    [Header("Fruit Info")]
    [SerializeField] private FruitType fruitType = FruitType.JUMPY;

    private FruitSpawner fruitSpawner;
    private bool hasNotifiedSpawner = false;
    private bool isOnCuttingBoard = false;
    private float spawnTime;

    public void Initialize(FruitSpawner spawner, FruitType type)
    {
        fruitSpawner = spawner;
        fruitType = type;
        spawnTime = Time.time;
    }

    public void SetOnCuttingBoard(bool onBoard)
    {
        isOnCuttingBoard = onBoard;
    }

    public bool IsOnCuttingBoard()
    {
        return isOnCuttingBoard;
    }

    public float GetTimeAlive()
    {
        return Time.time - spawnTime;
    }

    public FruitType GetFruitType()
    {
        return fruitType;
    }

    public void DestroyFruit()
    {
        if (!hasNotifiedSpawner && fruitSpawner != null)
        {
            hasNotifiedSpawner = true;
            fruitSpawner.OnFruitDestroyed(gameObject);
        }

        Destroy(gameObject);
    }

    private void Oestroy()
    {
        if (fruitSpawner != null && !hasNotifiedSpawner)
        {
            hasNotifiedSpawner = true;
            fruitSpawner.OnFruitDestroyed(gameObject);
        }       
    }
}
