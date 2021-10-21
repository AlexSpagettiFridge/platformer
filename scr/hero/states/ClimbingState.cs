using Godot;

namespace Game.HeroStates
{
    public class ClimbingState : HeroState
    {
        private const float climbSpeed = 25, jumpStrength = 305;
        private float climbX;

        public ClimbingState(float x){
            climbX = x;
        }

        public override void _Ready()
        {
            nHero.PlatformerProcessEnabled = false;
        }

        public override void _PhysicsProcess(float delta)
        {
            int climbDirection = 0;
            if (Input.IsActionPressed("gm_up")) { climbDirection = -1; }
            if (Input.IsActionPressed("gm_down")) { climbDirection = 1; }
            Vector2 movement = new Vector2(0, climbDirection * climbSpeed);
            if (nHero.Position.x != climbX)
            {
                movement.x = Mathf.Min(1200, Mathf.Abs(climbX - nHero.Position.x)) * Mathf.Sign(climbX - nHero.Position.x);
            }
            nHero.MoveAndSlide(movement, nHero.upVector);
            if (Input.IsActionJustPressed("gm_left") || Input.IsActionJustPressed("gm_right") || nHero.IsOnFloor())
            {
                nHero.ResumeNormalState();
            }
            if (Input.IsActionJustPressed("gm_jump"))
            {
                nHero.ResumeNormalState();
                nHero.moveSpeed.y = -jumpStrength;
            }
            if (Input.IsActionJustPressed("gm_hataction"))
            {
                nHero.CurrentHat.UseAbility();
            }
        }

        public override void EndState()
        {
            nHero.PlatformerProcessEnabled = true;
        }
    }
}