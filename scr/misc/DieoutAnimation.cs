using Godot;

namespace Game{
    public class DieoutAnimation: AnimatedSprite{
        public DieoutAnimation()
        {
            Connect("animation_finished",this,nameof(OnAnimationFinished));
        }

        private void OnAnimationFinished(){
            QueueFree();
        }
    }
}