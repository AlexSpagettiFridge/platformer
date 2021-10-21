using Godot;
using System.Collections.Generic;

namespace Game
{
    public class PushableEntity : KinematicBody2D
    {
        private AudioStreamPlayer2D nAudioPlayer;
        private const float gravity = 850, pushSpeed = 180;
        private int pushDirection = 0;
        private Vector2 upVector;
        private float fallSpeed = 0;
        private AudioStream pushSound, hitSound;

        public override void _Ready()
        {
            upVector = new Vector2(0, -1);
            pushSound = ResourceLoader.Load<AudioStream>("res://sfx/Push.wav");
            hitSound = ResourceLoader.Load<AudioStream>("res://sfx/Land.wav");
            nAudioPlayer = new AudioStreamPlayer2D();
            nAudioPlayer.Name = "PushableEntityAudioPlayer";
            nAudioPlayer.Bus = "Sound";
            AddChild(nAudioPlayer);
        }

        public override void _PhysicsProcess(float delta)
        {
            bool airborn = !TestMove(Transform, -upVector);
            if (airborn)
            {
                KinematicCollision2D collision = MoveAndCollide(upVector * -fallSpeed * delta);
                fallSpeed += gravity * delta;
                if (collision != null)
                {
                    fallSpeed = 0;
                    SnapToGrid();
                }
                if (pushDirection != 0)
                {
                    pushDirection = 0;
                    SnapToGrid();
                }
            }
            else
            {
                fallSpeed = 0;
            }
            if (pushDirection != 0)
            {
                if (MoveAndCollide(new Vector2(pushDirection * pushSpeed * delta, 0)) != null)
                {
                    nAudioPlayer.Stream = hitSound;
                    nAudioPlayer.Play();
                    pushDirection = 0;
                }
            }
        }

        private void SnapToGrid()
        {
            Position = new Vector2(Mathf.Round(Position.x / 8) * 8, Mathf.Round(Position.y / 8) * 8);
        }

        public void Push(Vector2 from)
        {
            nAudioPlayer.Stream = pushSound;
            nAudioPlayer.Play();
            pushDirection = Mathf.Sign(Position.x - from.x);
        }
    }
}