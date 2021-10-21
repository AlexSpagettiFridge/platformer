using Godot;
using System;

namespace Game
{
    public class MapLoadFadeOut : CanvasLayer
    {
        AnimationPlayer animationPlayer;
        Marmalade marmalade;
        public void StartFadeOut(Marmalade marmalade)
        {
            this.marmalade = marmalade;
            CallDeferred(nameof(DeferredFadeOut));
            Timer nTimer = GetNode<Timer>("LevelLoadTimer");
            nTimer.Connect("timeout", this, nameof(OnLevelLoadTimeTimeOut));
            nTimer.Start();
        }

        private void DeferredFadeOut()
        {
            Viewport viewport = GetNode<Viewport>("Control/ViewportContainer/Viewport");
            marmalade.GetParent().RemoveChild(marmalade);
            viewport.AddChild(marmalade);
            marmalade.Owner = viewport;
            marmalade.Position = new Vector2();
            (animationPlayer = GetNode<AnimationPlayer>("Control/ViewportContainer/Viewport/AnimationPlayer")).Play("fadeOut");
        }

        private void OnLevelLoadTimeTimeOut()
        {
            Util.GetGameController(this).UnforceCameraPosition();
            marmalade.LoadExitMap();
            animationPlayer.Play("fadeIn");
            animationPlayer.Connect("animation_finished", this, nameof(OnFadeInFinished));
        }

        private void OnFadeInFinished(String name)
        {
            QueueFree();
        }
    }
}
