using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Holders/Speed Configs")]
public class SpeedHolder : ScriptableObject
{
    [BoxGroup("Speed Settings")]
    public float deltaNormal, deltaHard, deltaSoft;
}