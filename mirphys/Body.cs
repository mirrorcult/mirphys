using System;

namespace mirphys
{
    public class Body
    {
        public Vec2 Position;
        public double Rotation;

        public Vec2 Velocity;
        public double AngularVelocity;

        public Vec2 Force;
        public double Torque;

        public Vec2 Size;

        public double Friction;
        public double Mass;
        public double InvMass => Mass < double.MaxValue ? 1.0f / Mass : 0.0f;

        public double I;
        public double InvI => I < double.MaxValue ? 1.0f / I : 0.0f;

        public Body()
        {
            Position = new Vec2(0.0f, 0.0f);
            Rotation = 0.0f;
            Velocity = new Vec2(0.0f, 0.0f);
            AngularVelocity = 0.0f;
            Force = new Vec2(0.0f, 0.0f);
            Torque = 0.0f;
            Friction = Constants.BaseFriction;

            Size = new Vec2(Constants.BaseWidth, Constants.BaseWidth);
            Mass = double.MaxValue;
            I = double.MaxValue;
        }

        public Body(Vec2 s, double m)
        {
            Position = new Vec2(0.0f, 0.0f);
            Rotation = 0.0f;
            Velocity = new Vec2(0.0f, 0.0f);
            AngularVelocity = 0.0f;
            Force = new Vec2(0.0f, 0.0f);
            Torque = 0.0f;
            Friction = Constants.BaseFriction;

            Size = s;
            Mass = m;
            if (Mass < double.MaxValue)
            {
                I = Mass * (Size.X * Size.X + Size.Y * Size.Y) / Constants.InertiaDivisionThing;
            }
        }

        public void AddForce(Vec2 f)
        {
            Force += f;
        }
    }
}