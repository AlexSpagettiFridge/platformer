using Godot;
using System;

namespace Game
{
    public class Scroll : Area2D
    {
        private AnimatedSprite nSprite;
        private Timer nTimer;
        private int id;
        public override void _Ready()
        {
            nSprite = GetNode<AnimatedSprite>("AnimatedSprite");
            nTimer = GetNode<Timer>("Timer");
            Connect("body_entered", this, nameof(OnBodyEntered));
            nTimer.Connect("timeout", this, nameof(OnTimeout));
        }

        private void OnBodyEntered(Node body)
        {
            if (body is Hero)
            {
                GetNode<GameController>("/root/GameController").CollectScroll(id);
                QueueFree();
            }
        }

        private void OnTimeout()
        {
            Random rr = new Random();
            nTimer.WaitTime = 0.5f + (float)rr.NextDouble() * 1.5f;
            nTimer.Start();

            if (rr.NextDouble() <= 0.9)
            {
                if(rr.NextDouble()>0.5){
                    nSprite.Animation = "flicker";
                }else{
                    nSprite.Animation = "revflicker";
                }
            }
            else
            {
                nSprite.Animation = "watch";
            }
            nSprite.Frame = 0;
            nSprite.Play();
        }

        public void SetId(int id){
            this.id = id;
        }
    }
}