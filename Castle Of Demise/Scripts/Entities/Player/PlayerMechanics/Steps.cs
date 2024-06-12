using System;
using Godot;

namespace CastleOfDemise.mobs.Player;

public partial class Player
{
    private Timer _stepTimer;
    [Export] private RayCast3D _floorRayCast;

    private void _stepsInit()
    {
        _stepTimer = GetNode<Timer>("StepSoundsEffetcs/TimerBetweenStep");
    }
        
    private void HandleStepSounds(bool isMoving)
    {
        var onTheGround  = _floorRayCast.GetCollider() != null;
        var canPlayNextStep = _stepTimer.TimeLeft <= 0;
        if (isMoving && onTheGround && canPlayNextStep)
        {

            _sfxPlayer.EmitSignal("PlaySFXSignal", "steps");

            // Restart the timer
            _stepTimer.Start(0.2f);
        }
    }



}