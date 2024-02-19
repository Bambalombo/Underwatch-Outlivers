using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    menuName = "Variables/Float Variable",
    fileName = "FloatVariable")]
public class FloatVariable : ScriptableObject
{
    public float value;
}

[CreateAssetMenu(
    menuName = "Variables/Vector3 Variable",
    fileName = "Vector3Variable")]
public class Vector3Variable : ScriptableObject
{
    public Vector3 value;
}