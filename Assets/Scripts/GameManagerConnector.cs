using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerConnector : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] public Button resetButton;
    [SerializeField] public Button resetButton2;
    [SerializeField] public Button nextDayButton;

    private void Start()
    {
        StartCoroutine(ConnectToGameManager());
    }

    private IEnumerator ConnectToGameManager()
    {
        while (GameManager.Instance == null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        if (resetButton != null)
        {
            resetButton.onClick.RemoveAllListeners();
            resetButton.onClick.AddListener(() =>
            {
                GameManager.Instance.ResetGame();
            });
            Debug.Log("Reset button connected to GameManager.");
        }

        if (resetButton2 != null)
        {
            resetButton2.onClick.RemoveAllListeners();
            resetButton2.onClick.AddListener(() =>
            {
                GameManager.Instance.ResetGame();
            });
            Debug.Log("Reset button 2 connected to GameManager.");
        }

        if (nextDayButton != null)
        {
            nextDayButton.onClick.RemoveAllListeners();
            nextDayButton.onClick.AddListener(() =>
            {
                GameManager.Instance.NextDay();
            });
            Debug.Log("Next Day button connected to GameManager.");
        }
    }
}
