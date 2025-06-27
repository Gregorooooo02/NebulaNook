using System.Collections;
using UnityEngine;

public class TriggerBlack : MonoBehaviour
{
    public static TriggerBlack Instance;
    public GameObject ToHide;
    public AudioSource jukebox;

    void Awake()
    {
        Instance = this;
    }

    public void ToTheBlackRoom()
    {
        gameObject.transform.position = GameoverRoom.Instance.transform.position;
        GameoverRoom.Instance.GameOverScreen.SetActive(true);
        jukebox.Stop();

        ToHide.SetActive(false);
        gameObject.transform.position = GameoverRoom.Instance.transform.position;
        GameoverRoom.Instance.GameOverScreen.SetActive(true);
        GameoverRoom.Instance.SetText("Your manager got a little angry huh?");
        jukebox.Stop();

        StartCoroutine(DelayedRestart());
    }

    private IEnumerator DelayedRestart()
    {
        yield return new WaitForSeconds(5f);
        LevelManager.Instance.FadeIntoScene(2);
    }

    private void OnTriggerEnter(Collider other)
    {
        var component = other.gameObject.GetComponent<Blackhole_expand>();
        if (component != null)
        {
            gameObject.transform.position = GameoverRoom.Instance.transform.position;
            GameoverRoom.Instance.GameOverScreen.SetActive(true);
            jukebox.Stop();
        }

        if (other.CompareTag("GameOver"))
        {
            ToHide.SetActive(false);
            gameObject.transform.position = GameoverRoom.Instance.transform.position;
            GameoverRoom.Instance.GameOverScreen.SetActive(true);
            GameoverRoom.Instance.SetText("Your manager got a little angry huh?");
            jukebox.Stop();
        }
    }
}
