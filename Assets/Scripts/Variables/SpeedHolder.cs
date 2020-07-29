using Sirenix.OdinInspector;
using UnityEngine;

namespace Tetris_RL.Variables
{
    [CreateAssetMenu(menuName = "Holders/Speed Configs")]
    public class SpeedHolder : ScriptableObject
    {
        [BoxGroup("Speed Settings")]
        public float deltaNormal, deltaHard, deltaSoft;
    }
}