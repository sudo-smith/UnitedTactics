﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(BoardCreator))]
public class BoardCreatorInspector : Editor
{
    public BoardCreator current
    {
        get
        {
            return (BoardCreator)target;
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Clear"))
            current.Clear();
        if (GUILayout.Button("Grow"))
            current.Grow();
        if (GUILayout.Button("Shrink"))
            current.Shrink();
        if (GUILayout.Button("Grow Highlighted Area"))
            current.GrowArea();
        if (GUILayout.Button("Shrink Highlighted Area"))
            current.ShrinkArea();
        if (GUILayout.Button("Grow Random Area"))
            current.GrowRandArea();
        if (GUILayout.Button("Shrink Random Area"))
            current.ShrinkRandArea();
        if (GUILayout.Button("Save"))
            current.Save();
        if (GUILayout.Button("Load"))
            current.Load();

        if (GUI.changed)
        {
            current.UpdateMarker();
            current.UpdateBorder();
        }

    }
}
