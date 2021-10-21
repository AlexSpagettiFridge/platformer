using Godot;

namespace Game.HeroStates
{
    public class StandardState : HeroState
    {
        private const float acceleration = 1600, jumpStrength = 305, sinkSpeed = 900;
        private float coyoteTime = 0;
        private static AudioStream jumpSound = ResourceLoader.Load<AudioStream>("res://sfx/Jump.wav")
        , landSound = ResourceLoader.Load<AudioStream>("res://sfx/Land.wav");
        private GameController nGameController;

        public override void _Ready()
        {
            nGameController = Util.GetGameController(this);
            //nHero.Connect("bounce", this, nameof(StopSinking));
        }
        public override void _PhysicsProcess(float delta)
        {
            int moveDirection = 0;
            if (Input.IsActionPressed("gm_left")) { moveDirection = -1; }
            if (Input.IsActionPressed("gm_right")) { moveDirection = 1; }

            nHero.moveSpeed.x += acceleration * delta * moveDirection;
            if (nHero.IsOnFloor())
            {
                if (coyoteTime <= 0)
                {
                    nHero.PlaySound(landSound);
                    //nGameController.ScreenShake(0.15f, 3);
                }
                coyoteTime = Util.coyoteTime;
            }
            else
            {
                coyoteTime -= delta;
            }
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed("gm_jump") && coyoteTime > 0)
            {
                nHero.PlaySound(jumpSound);
                nHero.moveSpeed.y = -jumpStrength;
                coyoteTime = 0;
            }
            if (nHero.CurrentHat != null)
            {
                if (@event.IsActionPressed("gm_down"))
                {
                    if (nHero.IsOnFloor())
                    {
                        nHero.ChangeState(new DuckingState());
                    }
                }
                if (@event.IsActionPressed("gm_hataction"))
                {
                    nHero.CurrentHat.UseAbility();
                }
            }
        }
    }
}