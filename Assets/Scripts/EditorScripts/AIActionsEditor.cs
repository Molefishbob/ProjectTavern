using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AIAction))]
public class AIActionsEditor : Editor
{
    #region Member Variables
    private SerializedObject _actionSO;

    #endregion

    #region Member Properties
    #endregion

    #region Editor Methods
    void OnEnable()
    {
        _actionSO = new SerializedObject(base.target);
    }

    public override void OnInspectorGUI()
    {
        _actionSO.Update();

        SerializedProperty aiState = _actionSO.FindProperty("_actionState");

        aiState.enumValueIndex = (int)(AIAction.State)EditorGUILayout.EnumPopup("Action Type:", (AIAction.State)System.Enum.GetValues(typeof(AIAction.State)).GetValue(aiState.enumValueIndex));

        //gridsize.vector2IntValue = EditorGUILayout.Vector2IntField(new GUIContent("Map Size", "Determines the size of the board\nMinimum size 5\nMaximum size 10"), temp);

        _actionSO.ApplyModifiedProperties();
    }
    #endregion

}
