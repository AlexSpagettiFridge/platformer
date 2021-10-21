using Godot;

namespace Game
{
    public class Marmalade : StaticBody2D
    {
        private AnimationPlayer animationPlayer;
        private bool playerIsPresent = false;
        private int id, exitId;
        private string exitMap;
        public bool waitUntilLeft = false;
        public override void _Ready()
        {
            animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            Area2D openJarZone = GetNode<Area2D>("OpenJarZone");

            animationPlayer.Connect("animation_finished", this, nameof(OnAnimationFinished));
            openJarZone.Connect("body_entered", this, nameof(OnEnterOpenJarZone));
            openJarZone.Connect("body_exited", this, nameof(OnExitOpenJarZone));
            GetNode<Area2D>("GoalArea").Connect("body_exited", this, nameof(OnGoalLeft));
            if (waitUntilLeft)
            {
                animationPlayer.Play("openJar");
            }
        }

        public void Setup(int glassId, int exitId, string exitMap)
        {
            id = glassId;
            this.exitId = exitId;
            this.exitMap = exitMap;
            GetNode<Area2D>("GoalArea").Connect("body_entered", this, nameof(OnEnterGoalArea));
        }

        public void OnEnterGoalArea(Node body)
        {
            if (!(body is Hero) || waitUntilLeft)
            {
                return;
            }
            GetNode<Area2D>("GoalArea").Disconnect("body_entered", this, nameof(OnEnterGoalArea));
            Hero nHero = (Hero)body;
            Sprite nFakeHero = GetNode<Sprite>("FakeHero");
            nFakeHero.Position += new Vector2(nHero.Position.x - Position.x, 0);
            nFakeHero.Visible = true;
            nHero.QueueFree();
            MapLoadFadeOut fadeEffect = (MapLoadFadeOut)ResourceLoader.Load<PackedScene>("res://props/ui/MapLoadFadeOut.tscn").Instance();
            GameController gameController = Util.GetGameController(this);
            gameController.AddChild(fadeEffect);
            gameController.SaveCurrentGame(id, true);
            gameController.ForceCameraToPosition(Position);
            fadeEffect.StartFadeOut(this);
        }

        public void OnGoalLeft(Node body)
        {
            waitUntilLeft = false;
        }

        public void LoadExitMap()
        {
            GetNode<GameController>("/root/GameController").LoadTiledMap(exitMap, exitId);
        }

        private void OnEnterOpenJarZone(Node body)
        {
            if (!(body is Hero))
            {
                return;
            }
            if (!animationPlayer.IsPlaying())
            {
                animationPlayer.Play("openJar");
            }
            playerIsPresent = true;
        }

        private void OnExitOpenJarZone(Node body)
        {
            if (!(body is Hero))
            {
                return;
            }
            playerIsPresent = false;
        }

        private void OnAnimationFinished(string animName)
        {
            if (animName == "openJar")
            {
                if (playerIsPresent)
                {
                    animationPlayer.Play("idleOpen");
                }
                else
                {
                    animationPlayer.Play("closeJar");
                }
            }
            if (animName == "closeJar" && playerIsPresent)
            {
                animationPlayer.Play("openJar");
            }
            if (animName == "idleOpen")
            {
                if (playerIsPresent)
                {
                    animationPlayer.Play("idleOpen");
                }
                else
                {
                    animationPlayer.Play("closeJar");
                }
            }
        }
    }
}