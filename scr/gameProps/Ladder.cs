using Godot;
using Game.HeroStates;

namespace Game
{
    public class Ladder : Area2D
    {
        bool isPlayerPresent = false;
        GameController gameController;
        public override void _Ready()
        {
            Connect("body_entered", this, nameof(OnBodyVisited), new Godot.Collections.Array { true });
            Connect("body_exited", this, nameof(OnBodyVisited), new Godot.Collections.Array { false });
            gameController = Util.GetGameController(this);
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!isPlayerPresent) { return; }
            if (gameController.MainPlayer.CurrentState is StandardState)
            {
                if (@event.IsActionPressed("gm_up") || @event.IsActionPressed("gm_down"))
                {
                    gameController.MainPlayer.ChangeState(new ClimbingState(Position.x));
                }
            }
        }

        private void OnBodyVisited(Node body, bool entered)
        {
            if (body is Hero)
            {
                isPlayerPresent = entered;
                if (entered == false)
                {
                    Hero nHero = (Hero)body;
                    if (nHero.CurrentState is ClimbingState)
                    {
                        nHero.ResumeNormalState();
                    }
                }
            }
        }

        public void SetLength(int yTiles)
        {
            GetNode<TextureRect>("TextureRect").RectSize = new Vector2(16, 16 * yTiles);
            CollisionShape2D colShape = GetNode<CollisionShape2D>("ColShape");
            colShape.Position = new Vector2(0, yTiles * 8 - 8);
            ((RectangleShape2D)colShape.Shape).Extents = new Vector2(8, yTiles * 8);
        }
    }
}