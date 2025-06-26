using UnityEngine;

public class TriggerBlack : MonoBehaviour
{
    public GameObject ToHide;
    private void OnTriggerEnter(Collider other)
    {
        var component = other.gameObject.GetComponent<Blackhole_expand>();
        if (component != null)
        {
            gameObject.transform.position = GameoverRoom.Instance.transform.position;
            GameoverRoom.Instance.GameOverScreen.SetActive(true);
        }

        if (other.CompareTag("GameOver"))
        {
            ToHide.SetActive(false);
            gameObject.transform.position = GameoverRoom.Instance.transform.position;
            GameoverRoom.Instance.GameOverScreen.SetActive(true);
            GameoverRoom.Instance.SetText("Your manager got a little angry huh?");
        }
    }
}
