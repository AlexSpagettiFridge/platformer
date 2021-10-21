using Godot;
using Game.HeroStates;

namespace Game
{
    class StandardHat : HeroHat
    {
        private readonly static PackedScene propHatPlatform = (PackedScene)ResourceLoader.Load("res://props/abilities/HatPlatform.tscn");
        private static AudioStream dashSound = ResourceLoader.Load<AudioStream>("res://sfx/Dash.wav");
        public override int GetHatId()
        {
            return 0;
        }

        public override void UseAbility()
        {
            HatPlatform nHatPlatform = (HatPlatform)propHatPlatform.Instance();
            Util.GetEntGrp(this).AddChild(nHatPlatform);
            nHatPlatform.GlobalPosition = GlobalPosition;
            if (Input.IsActionPressed("gm_up"))
            {
                nHatPlatform.GlobalPosition += new Vector2(0, 12);
            }
            nOwner.ChangeState(new DashState());
            nOwner.PlaySound(dashSound);
            QueueFree();
            Util.GetGameController(this).ScreenShake(0.25f, 4);
        }
    }
}