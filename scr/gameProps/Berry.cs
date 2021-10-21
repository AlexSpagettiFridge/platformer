using Godot;
using System;

namespace Game
{
    public class Berry : Area2D
    {
        private int id;
        private static AudioStream collectSound = ResourceLoader.Load<AudioStream>("res://sfx/Berry.wav");
        private bool graceProtection = false;
        public override void _Ready()
        {
            Connect("body_entered", this, nameof(OnBodyEntered));
        }

        private void OnBodyEntered(Node body)
        {
            if (graceProtection) { return; }
            if (body is Hero)
            {
                GameController nController = GetNode<GameController>("/root/GameController");
                nController.CollectBerry(id);
                QueueFree();
                nController.CreateLoseAudioSteamPlayerAt(collectSound, Position);
            }
        }

        public void SetId(int id)
        {
            this.id = id;
        }

        public void GiveGracePeriod(float seconds = 0.75f)
        {
            graceProtection = true;
            Timer nTimer = new Timer();
            nTimer.WaitTime = seconds;
            nTimer.OneShot = true;
            nTimer.Autostart = true;
            nTimer.Connect("timeout", this, nameof(GraceTimeout));
            AddChild(nTimer);
        }

        private void GraceTimeout()
        {
            graceProtection = false;
        }
    }
}
