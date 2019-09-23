using UnityEngine;

[CreateAssetMenu(fileName = "Custom", menuName = "AIActions/Action",order = 1)]
public class AIAction : ScriptableObject
{
    public enum State
    {
        None = 0,
        Moving = 1,
        Waiting = 2,
        Served = 3,
        PassedOut = 4,
        Fighting = 5
    }

    public State _actionState;
    
}
