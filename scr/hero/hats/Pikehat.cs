using Godot;

namespace Game
{
    class Pikehat : HeroHat
    {
        public override int GetHatId()
        {
            return 2;
        }

        public override void UseAbility()
        {
            QueueFree();
        }
    }
}