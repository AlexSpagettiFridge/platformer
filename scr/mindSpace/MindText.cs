using Godot;
using System;

namespace Game.MindSpace
{
    public class MindText : Label
    {
        private Vector2 startingPosition;
        private Random rr = new Random();
        private float liveTime = 10;

        public void RemoveLiveTime(float amount)
        {
            liveTime -= amount;
        }

        public void SetValues(String text, Vector2 position)
        {
            Text = text;
            startingPosition = position;
            liveTime += (float)rr.NextDouble() * 4;
        }

        public override void _Process(float delta)
        {
            RectPosition = startingPosition + new Vector2(0.125f - 0.25f * (float)rr.NextDouble(), 0.125f - 0.25f * (float)rr.NextDouble());
            if (liveTime > 3 && liveTime - delta <= 3)
            {
                AnimationPlayer nAnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
                nAnimationPlayer.CurrentAnimation = "fadeout";
                nAnimationPlayer.Play();
            }
            liveTime -= delta;
            if (liveTime <= 0)
            {
                QueueFree();
            }
        }
    }
}