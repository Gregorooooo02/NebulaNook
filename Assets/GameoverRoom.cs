using TMPro;
using UnityEngine;

public class GameoverRoom : MonoBehaviour
{
    public static GameoverRoom Instance;
    public GameObject GameOverScreen;
    public TextMeshProUGUI TextGUI;

    public void SetText(string text)
    {
        TextGUI.text = text;
    }

    private void Start()
    {
        Instance = this;
        if (GameOverScreen != null)
        {
            GameOverScreen.SetActive(false);
        }
    }
}
