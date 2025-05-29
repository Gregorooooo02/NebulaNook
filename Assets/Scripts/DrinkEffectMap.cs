using UnityEngine;

public class DrinkEffectMap : MonoBehaviour
{
    public static GameObject MapObject;
    public static DrinkEffectMap Instance;

    public Color[] colorTable = new Color[6];

    private void Start()
    {
        MapObject = this.gameObject;
        Instance = this;
    }
}
