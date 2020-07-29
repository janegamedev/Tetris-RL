using System;
using UnityEngine;

namespace Tetris_RL.Variables
{
    public class BoolVariable: ScriptableObject
    {
        public event Action<bool> OnValueChanged = delegate {  }; 
        private bool _value;

        public bool Value
        {
            get => _value;
            set
            {
                _value = value; 
                OnValueChanged.Invoke(_value);
            }

        }
    }
}