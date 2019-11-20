using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static Managers.BeverageManager;

[CustomEditor(typeof(Race))]
public class RaceEditor : Editor
{
    #region Member Variables
    private SerializedObject _actionSO;
    private Race _target;

    #endregion

    #region Member Properties
    #endregion

    #region Editor Methods
    void OnEnable()
    {
        _actionSO = new SerializedObject(base.target);
        _target = (Race)target;
    }

    public override void OnInspectorGUI()
    {
        _actionSO.Update();

        SerializedProperty race = _actionSO.FindProperty("_race");
        SerializedProperty beverage = _actionSO.FindProperty("_preferredBeverage");
        SerializedProperty aTolerance = _actionSO.FindProperty("_alcoholTolerance");
        SerializedProperty agressiveness = _actionSO.FindProperty("_agressiveness");

        race.enumValueIndex = (int)(Race.Type)EditorGUILayout.EnumPopup("Race:", (Race.Type)System.Enum.GetValues(typeof(Race.Type)).GetValue(race.enumValueIndex));
        beverage.enumValueIndex = (int)(Beverage)EditorGUILayout.EnumPopup("Preferred beverage:", (Beverage)System.Enum.GetValues(typeof(Beverage)).GetValue(beverage.enumValueIndex));
        aTolerance.intValue = EditorGUILayout.IntSlider(new GUIContent("Alcohol Tolerance:", "How well they handle their alcohol"), _target._alcoholTolerance, _target.MinAlcoholTolerance, _target.MaxAlcoholTolerance);
        agressiveness.intValue = EditorGUILayout.IntSlider(new GUIContent("Agressiveness:", "How easily starts fights when getting drunk"), _target._agressiveness, _target.MinAlcoholTolerance, _target.MaxAlcoholTolerance);

        //gridsize.vector2IntValue = EditorGUILayout.Vector2IntField(new GUIContent("Map Size", "Determines the size of the board\nMinimum size 5\nMaximum size 10"), temp);

        _actionSO.ApplyModifiedProperties();
    }
    #endregion

}
