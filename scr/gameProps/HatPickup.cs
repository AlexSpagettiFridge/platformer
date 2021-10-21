using Godot;
using System;

namespace Game
{
    public class HatPickup : Area2D
    {
        private int hatIndex = 0;
        public override void _Ready()
        {
            Connect("body_entered", this, nameof(OnBodyEntered));
        }

        public void OnBodyEntered(Node body)
        {
            if (body is Hero)
            {
                Hero nHero = (Hero)body;
                if (nHero.CurrentHat == null)
                {
                    Hero hero = (Hero)body;
                    hero.AddHat(hatIndex);
                    QueueFree();
                }
            }
        }

        public int HatIndex{
            get{return hatIndex;}
            set{
                hatIndex = value;
                GetNode<AnimatedSprite>("Sprite").Animation = value+"idle";
            }
        }
    }
}