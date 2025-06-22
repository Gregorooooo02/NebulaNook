using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
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
    public AnimationCurve leaveSpeedCurve;

    [Header("Catch up")]
    public float catchUpSpeed;
    public float catchUpDelay;

    private void Start()
    {
        foreach (var item in IngridientsAndOperators)
        {
            item.localPosition = EffectIcon.localPosition;
            item.gameObject.SetActive(false);
        }
        foreach (var blur in Blurs)
        {
            blur.SetActive(true);
        }
    }

    private void OnDisable()
    {
        foreach (var item in IngridientsAndOperators)
        {
            item.localPosition = EffectIcon.localPosition;
            item.gameObject.SetActive(false);
        }
        foreach(var blur in Blurs)
        {
            blur.SetActive(true);
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

        //Prepare available indices list
        List<int> indices = new List<int>();
        List<int> indicesToRemove = new List<int>();
        for (int i = 0;i < Blurs.Length; i++)
        {
            indices.Add(i);
        }

        for(int i = 0;i < unblurCount; i++)
        {
            int index = Random.Range(0, indices.Count);
            indicesToRemove.Add(indices[index]);
            indices.RemoveAt(index);
        }
        indicesToRemove.Sort();
        foreach (int index in indicesToRemove)
        {
            Blurs[index].SetActive(false);
        }

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
            float offsetPer = currentOffset / leaveOffset;
            float mul = leaveSpeedCurve.Evaluate(offsetPer);
            MainObject.localPosition = startPos + new Vector3(currentOffset * mul,0,0);
            yield return new WaitForFixedUpdate();
        }
        gameObject.SetActive(false);
    }

    public void MoveToNewPosition(int newIndex)
    {
        if(newIndex == positionIndex) return;
        StartCoroutine(MoveToIndex(newIndex));
    }

    IEnumerator MoveToIndex(int newIndex)
    {
        yield return new WaitForSeconds(catchUpDelay);
        float currentY = positionIndex * mainYOffset;
        float newY = newIndex * mainYOffset;
        while(currentY < newY)
        {
            currentY = Mathf.Min(currentY + catchUpSpeed * Time.fixedDeltaTime, newY);
            MainObject.localPosition = new Vector3(0,currentY,0);
            yield return new WaitForFixedUpdate();
        }
        positionIndex = newIndex;
    }

}
