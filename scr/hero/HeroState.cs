using Godot;

namespace Game
{
    public class HeroState : Node2D{

        internal Hero nHero;
        public void AddToHero(Hero nHero){
            this.nHero = nHero;
            nHero.AddChild(this);
        }

        public virtual void EndState(){}
    }
}