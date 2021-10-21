using Godot;
using System.Collections.Generic;

namespace Game
{
    public abstract class HeroHat : CollisionShape2D
    {
        internal AnimatedSprite nSprite;
        internal Hero nOwner;

        public HeroHat()
        {
            Shape = new RectangleShape2D();
            ((RectangleShape2D)Shape).Extents = new Vector2(5, 5.5f);
            Position = new Vector2(0, -2.5f);
            nSprite = new AnimatedSprite();
            nSprite.Frames = ResourceLoader.Load<SpriteFrames>("res://gfx/entities/hatframes.tres");
            nSprite.Position = new Vector2(0, 2.5f);
            AddChild(nSprite);
        }

        public override void _EnterTree()
        {
            nSprite.Play(GetHatId() + "idle");
            Connect("tree_exiting", this, nameof(OnTreeExiting));
        }

        public abstract void UseAbility();
        public abstract int GetHatId();

        public virtual void UseDuckedAbility(){
            QueueFree();          
        }

        private void OnTreeExiting()
        {
            nOwner.RemoveHat(this);
        }
        public void Wiggle(float rotation)
        {
            nSprite.Rotation = Mathf.Deg2Rad(Mathf.Clamp(Rotation, -35, 35) * 50);
        }

        public static HeroHat CreateHatById(int hatId)
        {
            switch (hatId)
            {
                case 1: return new TimetravelHat();
                case 2: return new Pikehat();
                case 3: return new BuildersHat();
                case 0: return new StandardHat();
            }
            return null;
        }

        public void SetOwner(Hero nHero){
            nOwner = nHero;
        }
    }
}