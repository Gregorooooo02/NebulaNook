using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerConnector : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] public Button resetButton;
    [SerializeField] public Button resetButton2;
    [SerializeField] public Button nextDayButton;

    [Header("References")]
    [SerializeField] private GameObject goodEnding;
    [SerializeField] private GameObject badEnding;
    [SerializeField] private GameObject jukebox;
    [SerializeField] private AudioClip goodClip;
    [SerializeField] private AudioClip badClip;

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

        if (goodEnding != null)
        {
            goodEnding.SetActive(false);
            GameManager.Instance.goodEnding = goodEnding;
            Debug.Log("Good ending connected to GameManager.");
        }

        if (badEnding != null)
        {
            badEnding.SetActive(false);
            GameManager.Instance.badEnding = badEnding;
            Debug.Log("Bad ending connected to GameManager.");
        }

        if (jukebox != null)
        {
            GameManager.Instance.jukebox = jukebox;
            Debug.Log("Jukebox connected to GameManager.");
        }

        if (goodClip != null)
        {
            GameManager.Instance.goodClip = goodClip;
            Debug.Log("Good clip connected to GameManager.");
        }

        if (badClip != null)
        {
            GameManager.Instance.badClip = badClip;
            Debug.Log("Bad clip connected to GameManager.");
        }
    }
}
