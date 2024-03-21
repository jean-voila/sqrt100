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
            var onTheGround  = _floorRayCast.GetCollider() != null;
            var canPlayNextStep = _stepTimer.TimeLeft <= 0;
            if (isMoving && _SEEnabled && onTheGround && canPlayNextStep)
            {
                
                // Select a random step sound from a list of different step sounds
                var randomStep = new Random().Next(0, _stepSounds.Count);

                // Set the pitch of the sound to a random value between 1 and 5
                _stepSounds[randomStep].PitchScale = new Random().Next(1, 5);

                // Play the selected step sound
                _stepSounds[randomStep].Play();

                // Restart the timer
                _stepTimer.Start(0.2f);
            }
        }



    }
