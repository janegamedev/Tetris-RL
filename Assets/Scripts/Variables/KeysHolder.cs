using UnityEngine;

namespace Tetris_RL.Variables
{
    [CreateAssetMenu(menuName = "Holders/Input Keys")]
    public class KeysHolder : ScriptableObject
    {
        public float keyNothing = 0, keyLeft = -1, keyRight = 1, keyHardDrop = 1;
    }
}