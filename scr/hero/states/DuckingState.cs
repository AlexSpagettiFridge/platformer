using Godot;

namespace Game.HeroStates
{
    public class DuckingState : HeroState
    {
        private const float hopStrength = 145;
        private float coyoteTime = 0;
        private static AudioStream landSound = ResourceLoader.Load<AudioStream>("res://sfx/SoftLand.wav");
        private static AudioStream jumpSound = ResourceLoader.Load<AudioStream>("res://sfx/HatJump.wav");

        public override void _Ready()
        {
            nHero.EnableEyes(false);
        }

        public override void _PhysicsProcess(float delta)
        {

            if (nHero.IsOnFloor())
            {
                if (coyoteTime <=0){
                    nHero.PlaySound(landSound);
                }
                coyoteTime = Util.coyoteTime;
            }
            else
            {
                coyoteTime -= delta;
            }
            if (!IsQueuedForDeletion())
            {
                nHero.xAxisFriction = nHero.IsOnFloor();
            }
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionReleased("gm_down"))
            {
                nHero.ResumeNormalState();
            }
            if (@event.IsActionPressed("gm_hataction"))
            {
                nHero.CurrentHat.UseAbility();
                nHero.ResumeNormalState();
            }
            if (@event.IsActionPressed("gm_jump") && coyoteTime > 0)
            {
                nHero.PlaySound(jumpSound);
                int moveDir = 0;
                if (Input.IsActionPressed("gm_left")) { moveDir = -1; }
                if (Input.IsActionPressed("gm_right")) { moveDir = 1; }
                nHero.moveSpeed.y = -hopStrength;
                nHero.moveSpeed.x += moveDir * hopStrength;
                nHero.xAxisFriction = false;
            }
        }

        public override void EndState()
        {
            nHero.xAxisFriction = true;
            nHero.Position += new Vector2(0, -5);
            nHero.EnableEyes(true);
        }
    }
}