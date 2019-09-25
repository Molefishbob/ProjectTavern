using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

[CustomEditor(typeof(GameInput.ControlsManager))]
public class ControlsManagerEditor : Editor
{
    private int deviceID = 1;

    public override void OnInspectorGUI()
    {
        if (!EditorApplication.isPlaying)
        {
            DrawDefaultInspector();
            return;
        }

        EditorGUILayout.BeginHorizontal();
        deviceID = EditorGUILayout.IntField("Device ID", deviceID);

        if (GUILayout.Button("Add Player"))
        {
            ((GameInput.ControlsManager)target).AddPlayer(deviceID);
        }

        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Log Available Devices"))
        {
            string tmp = "List of available devices:\nClick to Show\n";

            foreach (InputDevice device in InputSystem.devices)
            {
                tmp += device.path + ": " + device.deviceId + "\n";
            }

            Debug.Log(tmp);
        }
    }
}
