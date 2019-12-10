using UnityEngine;
using static Managers.IngredientManager;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Ingredient", order = 4)]
public class Ingredient : ScriptableObject
{
    public DrinkIngredient _ingredient = DrinkIngredient.None;
    public Sprite _sprite;
}
