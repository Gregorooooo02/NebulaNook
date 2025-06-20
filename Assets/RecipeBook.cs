using UnityEngine;

public class RecipeBook : MonoBehaviour
{
    public Recipe[] Recipes;
    public bool Run = false;



    private void Update()
    {
        if (Run)
        {
            Run = false;
            Recipes[0].RunLeave();
        }
    }
}
