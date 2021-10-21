using Godot;
using System;

namespace Game.MindSpace
{
    public class Minddoor : Area2D
    {
        [Export]
        private int doorId = 0;
        private AnimatedSprite nSprite;
        private MindController mindController;
        public override void _Ready()
        {
            nSprite = GetNode<AnimatedSprite>("Sprite");
            mindController = GetNode<MindController>("/root/MindSpaceController");
            Connect("body_entered", this, nameof(OnBodyEntered));
            Connect("body_exited", this, nameof(OnBodyExited));
        }

        private void OnBodyEntered(Node body)
        {
            nSprite.Animation = "open";
            nSprite.Frame = 0;
            nSprite.Play();
            mindController.currentDoorId = doorId;
        }

        private void OnBodyExited(Node body)
        {
            nSprite.Animation = "close";
            nSprite.Frame = 0;
            nSprite.Play();
            mindController.currentDoorId = -1;
        }
    }
}