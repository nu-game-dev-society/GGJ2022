using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CandleController))]
public class CandleControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Turn all on") && Application.isPlaying)
        {
            CandleController.SetAll(true);
        }

        if (GUILayout.Button("Turn all off") && Application.isPlaying)
        {
            CandleController.SetAll(false);
        }

        if (GUILayout.Button("Turn random off") && Application.isPlaying)
        {
            CandleController.BlowOutRandom();
        }
    }
}