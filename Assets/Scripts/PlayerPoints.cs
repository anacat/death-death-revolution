using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPoints")]
public class PlayerPoints : ScriptableObject
{
    public float points;
    public bool lost;
    public bool win;

    public float Points
    {
        get { return points; }
        set
        {
            points = Mathf.Clamp(value, 0f, 100f);
        }
    }
}
