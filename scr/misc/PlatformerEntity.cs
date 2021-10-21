using System;
using Godot;

namespace Game
{
    public class PlatformerEntity : KinematicBody2D
    {
        private static PackedScene propDirtParticles = ResourceLoader.Load<PackedScene>("res://props/misc/DirtParticle.tscn");
        public Vector2 moveSpeed = new Vector2();
        private bool enabled = true;
        public bool xAxisFriction = true, yAxisFriction = false, gravity = true, createsLandingParticles = false;
        internal float friction, maxSpeed;
        private const float gravityStrength = 850;
        internal Vector2 upVector = new Vector2(0, -1);
        internal bool oldOnFloor = false;
        private float bounciness;

        public PlatformerEntity()
        {
            AddUserSignal("bounce");
        }
        public override void _PhysicsProcess(float delta)
        {
            if (!enabled) { return; }
            if (gravity) { moveSpeed += gravityStrength * delta * -upVector; }
            if (xAxisFriction) { moveSpeed.x = useFrictionOnAxis(moveSpeed.x, delta); }
            if (yAxisFriction) { moveSpeed.y = useFrictionOnAxis(moveSpeed.y, delta); }
            MoveAndSlide(moveSpeed, upVector, false, 4, 0.785398f, false);
            if (IsOnFloor())
            {
                moveSpeed.y = Math.Min(-Math.Abs(moveSpeed.y * bounciness), moveSpeed.y);
                if (bounciness != 0) { EmitSignal("bounce"); }
            }
            if (IsOnCeiling())
            {
                moveSpeed.y = Math.Max(Math.Abs(moveSpeed.y * bounciness), moveSpeed.y);
                if (bounciness != 0) { EmitSignal("bounce"); }
            }
            if (IsOnWall())
            {
                moveSpeed.x = -moveSpeed.x * bounciness;
                if (bounciness != 0) { EmitSignal("bounce"); }
            }
            if (IsOnFloor() != oldOnFloor)
            {
                oldOnFloor = IsOnFloor();
                if (oldOnFloor == true)
                {
                    if (createsLandingParticles) { CreateDirtParticles(); }
                }
            }
        }

        private void CreateDirtParticles()
        {
            Particles2D nDirtParticles = (Particles2D)propDirtParticles.Instance();
            nDirtParticles.Position = Position;

            GetViewport().GetTexture().GetData();
            Util.GetEntGrp(this).AddChild(nDirtParticles);
        }

        private float useFrictionOnAxis(float axis, float delta)
        {
            axis = Math.Max(0, Math.Abs(axis) - friction * delta) * Math.Sign(axis);
            if (Math.Abs(axis) > maxSpeed)
            {
                float bonusFriction = (float)Math.Pow(Math.Abs(axis) / maxSpeed, 2) * friction;
                axis = Math.Max(0, Math.Abs(axis) - bonusFriction * delta) * Math.Sign(axis);
            }
            return axis;
        }

        public bool PlatformerProcessEnabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public void SetBounciness(float bounciness)
        {
            this.bounciness = bounciness;
        }
    }
}