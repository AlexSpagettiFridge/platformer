using Godot;
using System;

namespace Game.ScEvents
{
    public class FallJoke : Area2D
    {
        public override void _Ready()
        {
            Connect("body_entered", this, nameof(OnBodyEntered));
            GetNode<Timer>("PunchlineTimer").Connect("timeout", this, nameof(Punchline));
        }

        private void OnBodyEntered(Node body)
        {
            if (!(body is Hero))
            {
                return;
            }
            Hero nHero = (Hero)body;
            nHero.ChangeState(null);
            GameController gameController = GetNode<GameController>("/root/GameController");
            gameController.ForceCameraToPosition(nHero.Position);
            InfoWindow nInfoWindow = gameController.ShowInfoWindow("Durch den Fall hast du dir all deine Knochen gebrochen.");
            nInfoWindow.Connect("tree_exiting", this, nameof(StartPunchlineTimer));

        }

        private void StartPunchlineTimer()
        {
            GetNode<Timer>("PunchlineTimer").Start();
        }

        private void Punchline()
        {
            GameController gameController = GetNode<GameController>("/root/GameController");
            gameController.ShowInfoWindow("Jedoch hast du keine Knochen, bist also komplett unbeschadet.", true);
            gameController.UnforceCameraPosition();
            gameController.MarkEventAsPassed("FallJk");
            QueueFree();
        }
    }
}