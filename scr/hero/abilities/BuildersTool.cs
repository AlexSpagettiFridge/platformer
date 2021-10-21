using Godot;
using System;

namespace Game
{
    public class BuildersTool : Area2D
    {
        private int facing = 0;
        private float gravityLessTime = 0.6f;
        private Vector2 moveSpeed;
        private const float gravity = 425, throwSpeed = 250;
        public override void _Ready()
        {
            GetNode<Area2D>("GenerousArea").Connect("body_entered", this, nameof(OnCollide), new Godot.Collections.Array { true });
            Connect("body_entered", this, nameof(OnCollide), new Godot.Collections.Array { false });
            GetNode<AnimationPlayer>("AnimationPlayer").Play("Spin");
        }

        public override void _EnterTree()
        {
            moveSpeed = new Vector2(facing * throwSpeed, 0);
        }

        public override void _Process(float delta)
        {
            if (gravityLessTime > 0)
            {
                gravityLessTime -= delta;
            }
            else
            {
                moveSpeed.y += gravity * delta;
            }
            Position += moveSpeed * delta;
        }

        private void OnCollide(Node body, bool generous = false)
        {
            if (body is Hero)
            {
                return;
            }
            if (body is PushableEntity)
            {
                ((PushableEntity)body).Push(Position);
                QueueFree();
            }
            if (!generous)
            {
                QueueFree();
            }
        }

        public void SetFacing(int facing)
        {
            this.facing = facing;
        }

        public void SetFrame(int frame)
        {
            GetNode<Sprite>("Sprite").Frame = frame;
        }
    }
}
