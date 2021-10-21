using Godot;

namespace Game
{
    class TimetravelHat : HeroHat
    {
        public override int GetHatId()
        {
            return 1;
        }

        public override void UseAbility()
        {
            QueueFree();
        }
    }
}