using TMPro;
using UnityEngine;

public class GameoverRoom : MonoBehaviour
{
    public static GameoverRoom Instance;

    public TextMeshProUGUI TextGUI;

    public void SetText(string text)
    {
        TextGUI.text = text;
    }

    private void Start()
    {
        Instance = this;
    }
}
