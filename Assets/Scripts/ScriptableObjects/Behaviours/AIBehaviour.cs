using UnityEngine;

[CreateAssetMenu(fileName = "New AIBehaviour", menuName = "Actions/Behaviour",order = 1)]
public class AIBehaviour : ScriptableObject
{
    public Race _race;
    public Action[] _actions;
}
