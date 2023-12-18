using Godot;


    public partial class Player
    {
        private string _pauseMenuPath;
        private Control _pauseMenu;
        public ulong _lastPauseTime;

        public void _pauseMenuInit()
        {
            _pauseMenuPath = "CanvasLayer2/MenuPause";
            _pauseMenu = GetNode<Control>(_pauseMenuPath);
            _pauseMenu.Visible = false;
            _lastPauseTime = Time.GetTicksMsec();
        }
        public void Pause()
        {
            _pauseMenu.Visible = true;
            Input.MouseMode = Input.MouseModeEnum.Visible;
            GetTree().Paused = true;
            _lastPauseTime = Time.GetTicksMsec();
        }

        public void UnPause()
        {
            if (Time.GetTicksMsec() - _lastPauseTime > 10)
            {
                _pauseMenu.Visible = false;
                GetTree().Paused = false;
                Input.MouseMode = Input.MouseModeEnum.Captured;
            }
        }
    }
