using UnityEngine;
using UnityEngine.UI;

public class DrinkEffectMap : MonoBehaviour
{
    public static GameObject MapObject;
    public static DrinkEffectMap Instance;

    public Color[] colorTable = new Color[6];
    public Texture2D[] effectIcons;
    public Sprite[] effectSprites;

    private void Start()
    {
        MapObject = this.gameObject;
        Instance = this;
    }
}
