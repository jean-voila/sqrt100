using Godot;


public partial class Player
    {
        [Export] private bool _jeansModEnabled;

        public void _jeansModInit()
        {
            _jeansModEnabled = false;
        }
        public void SwitchJeansMod(bool value)
        {
            _jeansModEnabled = value;
        }
    }
