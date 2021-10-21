using Godot;

namespace Game
{
    public class Trampoline : PushableEntity
    {
        private AnimatedSprite nSprite;
        private AudioStreamPlayer2D nAudioPlayer;
        private const float springStrength = 500;

        public override void _Ready()
        {
            base._Ready();
            nAudioPlayer = GetNode<AudioStreamPlayer2D>("AudioPlayer");
            nSprite = GetNode<AnimatedSprite>("AnimatedSprite");
            Area2D nSpringArea = GetNode<Area2D>("SpringArea");
            nSpringArea.Connect("body_entered", this, nameof(OnEnterSpringArea));
            nSprite.Play();
        }

        private void OnEnterSpringArea(Node body)
        {
            if (body is PlatformerEntity)
            {
                PlatformerEntity springBoy = (PlatformerEntity)body;
                if (springBoy.moveSpeed.y>=0){
                    springBoy.moveSpeed.y = -springStrength;
                    nSprite.Frame = 0;
                    nSprite.Play();
                    nAudioPlayer.Play();
                }
            }
        }
    }
}