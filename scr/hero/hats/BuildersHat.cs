using Godot;

namespace Game
{
    class BuildersHat : HeroHat
    {
        private static readonly PackedScene propBuildersTool = ResourceLoader.Load<PackedScene>("res://props/abilities/BuildersTool.tscn");
        private int remainingTools = 3;
        private static AudioStream throwSound = ResourceLoader.Load<AudioStream>("res://sfx/Throw.wav");
        public override int GetHatId()
        {
            return 3;
        }

        public override void UseAbility()
        {

            BuildersTool nn = (BuildersTool)propBuildersTool.Instance();
            nn.SetFacing(nOwner.facing);
            nn.SetFrame(3 - remainingTools);
            nn.GlobalPosition = GlobalPosition;
            remainingTools--;
            if (remainingTools <= 0)
            {
                QueueFree();
            }
            Util.GetEntGrp(this).AddChild(nn);
            nOwner.PlaySound(throwSound);
        }

        public override void UseDuckedAbility()
        {

        }
    }
}