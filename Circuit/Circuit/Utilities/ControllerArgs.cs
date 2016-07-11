using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Circuit.Utilities
{
    public class ControllerArgs : EventArgs
    {
        public readonly PlayerIndex ControllerIndex;
        public readonly object ControllerState;
        public readonly bool PressedButton;
        public readonly int Milliseconds;

        public ControllerArgs(PlayerIndex _index, float _state, bool _pressed, int _time)
        {
            ControllerIndex = _index;
            ControllerState = _state;
            PressedButton = _pressed;
            Milliseconds = _time;
        }

        public ControllerArgs(PlayerIndex _index, Vector2 _state, bool _pressed, int _time)
        {
            ControllerIndex = _index;
            ControllerState = _state;
            PressedButton = _pressed;
            Milliseconds = _time;
        }

        public ControllerArgs(PlayerIndex _index, bool _pressed, int _time)
        {
            ControllerIndex = _index;
            PressedButton = _pressed;
            Milliseconds = _time;
        }
    }
}
