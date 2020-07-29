using System;
using UnityEngine;

namespace Tetris_RL.Variables
{
    public class IntVariable : ScriptableObject
    {
        public event Action<int> OnValueChanged = delegate {  }; 
        private int _value;
        
        public int Value
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