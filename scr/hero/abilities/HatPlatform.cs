using Godot;

namespace Game
{
    public class HatPlatform : StaticBody2D
    {
        private float lifetime = 10, fallSpeed = 0, gravity = 0;
        private bool isPlayerPresent = false, isMorphed = false;
        private AnimatedSprite nAnimatedSprite;

        public override void _Ready()
        {
            nAnimatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
            nAnimatedSprite.Connect("animation_finished", this, nameof(OnAnimationFinished));
            Area2D nOnArea = GetNode<Area2D>("OnArea");
            nOnArea.Connect("body_entered", this, nameof(OnEnterPlatform));
            nOnArea.Connect("body_exited", this, nameof(OnExitPlatform));
            nAnimatedSprite.Play("morph");
        }
        public override void _Process(float delta)
        {
            Position += new Vector2(0, fallSpeed) * delta;
            fallSpeed += gravity * delta;
            lifetime -= delta;
            if (lifetime < 0)
            {
                Destroy();
            }
            if (isPlayerPresent)
            {
                if (Input.IsActionJustPressed("gm_jump")) { Destroy(); }
                if (isMorphed) { gravity = 4; }
            }
        }
        private void OnExitPlatform(Node body)
        {
            if (body is PlatformerEntity)
            {
                isPlayerPresent = false;
            }
        }
        private void OnEnterPlatform(Node body)
        {
            if (body is PlatformerEntity)
            {
                isPlayerPresent = true;
            }
        }
        private void OnAnimationFinished()
        {
            isMorphed = true;
            nAnimatedSprite.Disconnect("animation_finished", this, nameof(OnAnimationFinished));
            nAnimatedSprite.Play("idle");
        }

        private void Destroy()
        {
            RemoveChild(nAnimatedSprite);
            Node2D entsGrp = Util.GetEntGrp(this);
            entsGrp.AddChild(nAnimatedSprite);
            nAnimatedSprite.Owner = entsGrp;
            nAnimatedSprite.Play("explode");
            nAnimatedSprite.Position = GlobalPosition + new Vector2(0, 7);
            nAnimatedSprite.SetScript(ResourceLoader.Load<Script>("scr/misc/DieoutAnimation.cs"));
            QueueFree();
        }
    }
}