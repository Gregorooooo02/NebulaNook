using System.Collections;
using UnityEngine;

public class PipGameOver : MonoBehaviour
{
    [SerializeField] private float StartHeight;
    [SerializeField] private float FinalHeight;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float startDelay;

    [SerializeField] private Animator animatorMain;
    [SerializeField] private Animator animatorTounge;

    [SerializeField] private float secondAnimationDelay;

    private IEnumerator AnimateGameOver()
    {
        yield return new WaitForSeconds(startDelay);
        Vector3 startPos = new Vector3(transform.localPosition.x, StartHeight, transform.localPosition.z);
        while (startPos.y < FinalHeight)
        {
            startPos.y = Mathf.Min(startPos.y + moveSpeed * Time.fixedDeltaTime,FinalHeight);
            transform.localPosition = startPos;
            yield return new WaitForFixedUpdate();
        }
        animatorMain.SetBool("GameOver", true);
    }


    private void OnEnable()
    {
        StartCoroutine(AnimateGameOver());
    }
}
