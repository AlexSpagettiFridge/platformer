using Godot;

namespace Game.HeroStates
{
    public class DashState : HeroState
    {
        private const float dashSpeed = 500;
        private float timer = 0.15f;
        private Vector2 direction;

        public DashState()
        {
            if (Input.IsActionPressed("gm_left")) { direction.x = -1; }
            if (Input.IsActionPressed("gm_right")) { direction.x = 1; }
            if (Input.IsActionPressed("gm_up")) { direction.y = -1; }
            if (Input.IsActionPressed("gm_down")) { direction.y = 1; }
            if (direction == new Vector2())
            {
                direction = new Vector2(0, -1);
            }

        }

        public override void _Ready()
        {
            nHero.moveSpeed = direction * dashSpeed;
            nHero.gravity = false;
            nHero.xAxisFriction = true;
            nHero.yAxisFriction = true;
        }

        public override void _PhysicsProcess(float delta)
        {
            if (timer <= 0)
            {
                nHero.ResumeNormalState();
            }
            timer -= delta;
        }

        public override void EndState()
        {
            nHero.gravity = true;
            nHero.xAxisFriction = true;
            nHero.yAxisFriction = false;
        }
    }
}