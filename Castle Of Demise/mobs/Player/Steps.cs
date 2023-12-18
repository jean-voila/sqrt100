using Godot;
using System;


public partial class Player
    {
        private RayCast _floorRayCast;
        private Timer _stepTimer;

        private void _stepsInit()
        {
            _stepTimer = GetNode<Timer>("StepSoundsEffetcs/TimerBetweenStep");
            _floorRayCast = GetNode<RayCast>("checkFloor");
        }
        private void HandleStepSounds(bool isMoving)
        {
            if (isMoving && _SEEnabled && _floorRayCast.GetCollider() != null && _stepTimer.TimeLeft <= 0)
            {
                var randomStep = new Random().Next(0, _stepSounds.Count);
                _stepSounds[randomStep].PitchScale = new Random().Next(1, 5);
                _stepSounds[randomStep].Play();
                _stepTimer.Start(0.2f);
            }
        }
    }
