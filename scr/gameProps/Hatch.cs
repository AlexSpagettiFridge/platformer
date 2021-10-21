using Godot;

namespace Game
{
    public class Hatch : StaticBody2D
    {
        private AnimatedSprite nSprite;
        public override void _Ready()
        {
            nSprite = GetNode<AnimatedSprite>("AnimatedSprite");
            Area2D nArea = GetNode<Area2D>("Area2D");
            nArea.Connect("body_entered", this, nameof(OnAreaEntered));
        }

        private void OnAreaEntered(Node body){
            nSprite.Frame=0;
            nSprite.Play();
        }
    }
}