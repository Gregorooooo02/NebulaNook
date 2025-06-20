using System.Collections;
using UnityEngine;

public class Recipe : MonoBehaviour
{
    public RectTransform MainObject;
    public RectTransform[] IngridientsAndOperators;
    public RectTransform EffectIcon;
    
    public int positionIndex;
    public float mainYOffset;

    [Header("Blur settings")]
    public GameObject[] Blurs;
    public int unblurCount;

    [Header("Initial icon entrance")]
    public float sizeIncreaseSpeed;
    private float currentSize = 0;

    [Header("Ingredients and operators entrance")]
    public float horizontalOffset;
    public float horizontalSpeed;

    [Header("Leave")]
    public float leaveOffset;
    public float leaveSpeed;

    private void OnDisable()
    {
        foreach (var item in IngridientsAndOperators)
        {
            item.localPosition = EffectIcon.localPosition;
            item.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        StartCoroutine("Apear");
    }

    IEnumerator Apear()
    {
        MainObject.localPosition = new Vector3(0, positionIndex * mainYOffset, 0);
        currentSize = 0.0f;
        while (currentSize < 1.0f)
        {
            currentSize = Mathf.Clamp01(currentSize + Time.fixedDeltaTime * sizeIncreaseSpeed);
            EffectIcon.localScale = new Vector3(currentSize, currentSize,currentSize);
            yield return new WaitForFixedUpdate();
        }
        int unblurLeft = unblurCount;



        foreach (var item in IngridientsAndOperators)
        {
            item.gameObject.SetActive(true);
        }
        float currentProgress = 0;
        float startXOffset = EffectIcon.localPosition.x;
        Vector3 offset = new Vector3(startXOffset, EffectIcon.localPosition.y, 0);
        while(currentProgress < 1.0f)
        {
            currentProgress = Mathf.Clamp01(currentProgress + horizontalSpeed * Time.fixedDeltaTime);
            for(int i = 0;i< IngridientsAndOperators.Length; i++)
            {
                float totalOffsetLenght = Mathf.Abs(horizontalOffset * (i + 1));
                totalOffsetLenght *= horizontalOffset < 0 ? -1 : 1;
                Vector3 horizontalOffsetVector = new Vector3(totalOffsetLenght, 0, 0);
                IngridientsAndOperators[i].localPosition = (offset + horizontalOffsetVector * currentProgress);
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public void RunLeave()
    {
        StartCoroutine("Leave");
    }

    IEnumerator Leave()
    {
        Vector3 startPos = MainObject.localPosition;
        
        float currentOffset = 0.0f;
        while(currentOffset > leaveOffset)
        {
            currentOffset = Mathf.Clamp(currentOffset - leaveSpeed * Time.fixedDeltaTime, leaveOffset, 0);
            MainObject.localPosition = startPos + new Vector3(currentOffset,0,0);
            yield return new WaitForFixedUpdate();
        }
        gameObject.SetActive(false);
    }
}
