using System.Collections.Generic;
using UnityEngine;

public class RecipeBook : MonoBehaviour
{
    public static RecipeBook Instance;

    public Recipe[] Recipes;
    public bool LeaveRandom = false;
    public bool AddRandom = false;

    private List<int> currentlyActiveRecipies = new List<int>();
    private List<ClientController> clients = new List<ClientController>();

    private void Start()
    {
        Instance = this;
    }

    public void AddRecipe(DrinkEffect effectRecipe, ClientController asker)
    {
        int actualIndex = (int)effectRecipe - 1;
        if(currentlyActiveRecipies.Contains(actualIndex))return;
        int activeRecipeCount = currentlyActiveRecipies.Count;
        currentlyActiveRecipies.Add(actualIndex);
        Recipes[actualIndex].positionIndex = activeRecipeCount;
        Recipes[actualIndex].gameObject.SetActive(true);
        if(asker != null)clients.Add(asker);
    }

    public void RemoveRecipe(DrinkEffect effectRecipe, ClientController asker)
    {
        int actualIndex = (int)effectRecipe - 1;
        if (!currentlyActiveRecipies.Contains(actualIndex)) return;
        if(asker != null)
        {
            clients.Remove(asker);
            foreach (ClientController client in clients)
            {
                if (client.DesiredDrinkEffect == effectRecipe) return;
            }
        }
        int activeRecipeCount = currentlyActiveRecipies.Count;
        int listIndex = currentlyActiveRecipies.IndexOf(actualIndex);
        for(int i = listIndex + 1;i < currentlyActiveRecipies.Count; i++)
        {
            Recipes[currentlyActiveRecipies[i]].MoveToNewPosition(i - 1);
        }
        Recipes[currentlyActiveRecipies[listIndex]].RunLeave();
        currentlyActiveRecipies.RemoveAt(listIndex);
    }

    private void Update()
    {
        if (LeaveRandom)
        {
            LeaveRandom = false;
            if (currentlyActiveRecipies.Count == 0) return;
            int index = Random.Range(0, currentlyActiveRecipies.Count);
            DrinkEffect effect = (DrinkEffect)(currentlyActiveRecipies[index] + 1);
            RemoveRecipe(effect,null);
        }
        if (AddRandom)
        {
            AddRandom = false;
            int index = Random.Range(0, Recipes.Length);
            DrinkEffect effect = (DrinkEffect)(index + 1);
            AddRecipe(effect, null);
        }
    }
}
