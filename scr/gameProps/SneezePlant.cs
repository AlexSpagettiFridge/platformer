using Godot;
using System;
using System.Collections.Generic;

namespace Game
{
    public class SneezePlant : Area2D
    {
        private PackedScene propBorries = ResourceLoader.Load<PackedScene>("res://props/misc/Borries.tscn");
        private Timer nSneezeTimer;
        private Dictionary<int, Node2D> nBorries = new Dictionary<int, Node2D>();
        private List<int> berryIds = new List<int>();

        public override void _Ready()
        {
            nSneezeTimer = GetNode<Timer>("SneezeTimer");
            nSneezeTimer.Connect("timeout", this, nameof(Sneeze));
            Connect("body_entered", this, nameof(OnBodyEntered));

        }

        private void OnBodyEntered(Node body)
        {
            if (body is Hero)
            {
                AnimatedSprite nAnimatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
                nAnimatedSprite.Play("Sneeze");
                nSneezeTimer.Start();
                Disconnect("body_entered", this, nameof(OnBodyEntered));
            }

        }

        private void Sneeze()
        {
            GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();
            Random rr = new Random();
            foreach (int berryId in berryIds)
            {
                PlatformerEntity nBorry = (PlatformerEntity)propBorries.Instance();
                nBorry.moveSpeed = new Vector2((float)rr.NextDouble() * 600 - 300, -(float)rr.NextDouble() * 300);
                nBorry.maxSpeed = 300;
                nBorry.friction = 450;
                nBorry.Position = Position + new Vector2((float)rr.NextDouble() * 26 - 13, -8);
                nBorry.SetBounciness(0.65f);
                Util.GetGameController(this).AddChild(nBorry);
                int borryId = nBorries.Count;
                nBorries.Add(borryId, nBorry);
                Berry nBerry = nBorry.GetNode<Berry>("Berry");
                nBerry.SetId(berryId);
                nBerry.GiveGracePeriod();
                nBerry.Connect("tree_exited", this, nameof(RemoveBorry), new Godot.Collections.Array { borryId });
            }
        }

        private void RemoveBorry(int borryId)
        {
            nBorries.Remove(borryId);
        }

        public void setBerries(List<int> berryIds)
        {
            this.berryIds = berryIds;
            if (berryIds.Count == 0)
            {
                Disconnect("body_entered", this, nameof(OnBodyEntered));
                AnimatedSprite nAnimatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
                nAnimatedSprite.Play("Sneeze");
            }
        }
    }
}