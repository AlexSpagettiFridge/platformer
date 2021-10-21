using Godot;
using System;

namespace Game
{
    public class Checkpoint : Area2D
    {
        private int saveSpotId;
        private AnimatedSprite nSprite;
        public override void _Ready()
        {
            Connect("body_entered", this, nameof(OnBodyEntered));
            nSprite = GetNode<AnimatedSprite>("Sprite");
            nSprite.Connect("animation_finished",this,nameof(OnOpeningAnimationFinished));
            nSprite.Play("idleClosed");
        }

        private void OnBodyEntered(Node body)
        {
            if (body is Hero){
                Util.GetGameController(this).SaveCurrentGame(saveSpotId);
                nSprite.Play("opening");
                Disconnect("body_entered", this, nameof(OnBodyEntered));
            }
        }

        private void OnOpeningAnimationFinished(){
            if (nSprite.Animation=="idleClosed"){
                return;
            }
            nSprite.Disconnect("animation_finished",this,nameof(OnOpeningAnimationFinished));
            nSprite.Connect("animation_finished",this,nameof(OnIdleAnimationFinished));
            nSprite.Play("idleOpen");
        }

        private void OnIdleAnimationFinished(){
            Random rr = new Random();
            nSprite.Frame = 0;
            if (rr.NextDouble()>0.95){
                nSprite.Play("ominous");
            }else{
                nSprite.Play("idleOpen");
            }
        }

        public void SetId(int id){
            saveSpotId = id;
        }
    }
}