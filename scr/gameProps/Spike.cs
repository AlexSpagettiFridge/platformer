using Godot;

namespace Game
{
    public class Spike : Area2D
    {
        public override void _Ready()
        {
            Connect("body_entered",this,nameof(OnBodyEntered));
        }

        private void OnBodyEntered(Node body)
        {
            if (body is Hero){
                Hero nHero = (Hero)body;
                nHero.Kill();
            }
    }
    }
}