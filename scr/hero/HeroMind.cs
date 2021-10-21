using Godot;
using System;

namespace Game
{
    public class HeroMind : KinematicBody2D
    {
        private Sprite nSprite;
        private const float acceleration = 300, maxSpeed = 260, friction = 45, bounciness = 0.5f;
        private float wobble = 0, wobbleSpeed = 0, wobblePull = 1.2f;
        private Vector2 moveSpeed;
        public override void _Ready()
        {
            nSprite = GetNode<Sprite>("Sprite");
        }

        public override void _Process(float delta)
        {
            Vector2 moveDirection = new Vector2();
            if (Input.IsActionPressed("gm_up"))
            {
                moveDirection.y = -1;
                wobble += delta * wobblePull;
            }
            if (Input.IsActionPressed("gm_down"))
            {
                moveDirection.y = +1;
                wobble += delta * wobblePull;
            }
            if (Input.IsActionPressed("gm_left"))
            {
                moveDirection.x = -1;
                wobble -= delta * wobblePull;
            }
            if (Input.IsActionPressed("gm_right"))
            {
                moveDirection.x = +1;
                wobble -= delta * wobblePull;
            }
            moveSpeed += moveDirection * acceleration * delta;
            moveSpeed = moveSpeed.Normalized() * Mathf.Clamp(moveSpeed.Length() - friction * delta, 0, maxSpeed);
            nSprite.Rotation = Mathf.Deg2Rad(moveSpeed.x / maxSpeed * 25);
            MoveAndSlide(moveSpeed, new Vector2(0, -1));
            if (IsOnWall())
            {
                moveSpeed.x *= -bounciness;
                wobbleSpeed = 15;
            }
            if (IsOnFloor() || IsOnCeiling())
            {
                moveSpeed.y *= -bounciness;
                wobbleSpeed = -15;
            }

            wobble += wobbleSpeed * delta;
            if (wobble > 1 || wobble < -1)
            {
                wobble = Mathf.Clamp(wobble, -1, 1);
                wobbleSpeed *= -0.8f;
            }
            wobbleSpeed = Math.Max(0, Math.Abs(wobbleSpeed) - delta * 6) * Math.Sign(wobbleSpeed);
            wobble = Math.Max(0, Math.Abs(wobble) - delta * 0.75f) * Math.Sign(wobble);
            nSprite.Scale = new Vector2(1 - wobble / 5, 1 + wobble / 5);
        }
    }
}
