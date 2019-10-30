using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Customer))]
public class CustomerEditor : Editor
{
    Customer _customer = null;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        _customer = (Customer)target;
        if (GUILayout.Button("Order override"))
        {
            _customer.OrderOverride();
        }

        EditorGUILayout.LabelField("Current state: " + _customer.CurrentState);

        if (_customer.DrinkTimerRunning)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(string.Format("Drink Time: {0:00.0}", _customer.DrinkTimerElapsed));
            GUILayout.EndHorizontal();
            Repaint();
        }
        
        if (_customer.CurrentState == Managers.AIManager.State.Ordered)
        {
            EditorGUILayout.LabelField("Current order: " + _customer.OrderedDrink);
        }

        EditorGUILayout.LabelField("Next state: " + _customer.NextState);
        EditorGUILayout.LabelField("Drunkness: " + _customer.Drunkness);
    }
}
