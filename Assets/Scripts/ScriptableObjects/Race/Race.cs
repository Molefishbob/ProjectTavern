using UnityEngine;
using static Managers.BeverageManager;

[CreateAssetMenu(fileName = "New Race", menuName = "Race", order = 2)]
public class Race : ScriptableObject
{
    public enum Type
    {
        None = 0,
        Human = 1,
        Dwarf = 2,
        Elf = 3,
        Orc = 4
    }

    public AIBehaviour[] _aiBehaviours;
    public int MaxAlcoholTolerance = 10;
    public int MinAlcoholTolerance = 0;
    public Type _race = Type.None;
    public int _alcoholTolerance = 0;
    public int _agressiveness = 0;
    public Beverage _preferredBeverage = Beverage.None;
}
