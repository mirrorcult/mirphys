using System;

namespace mirphys
{
    public class Body
    {
        // State
        public Vec2 Position = new Vec2(0.0f, 0.0f);
        public double Rotation = 0.0f;
        public Vec2 LinearVelocity = new Vec2(0.0f, 0.0f);
        public double AngularVelocity = 0.0f;
        
        // Applied forces
        public Vec2 Force = new Vec2(0.0f, 0.0f);
        public double Torque = 0.0f;
        
        // Body properties
        public double Friction = Constants.BaseFriction;
        public double Mass = Double.MaxValue;
        public double InvMass => Mass < double.MaxValue ? 1.0f / Mass : 0.0f;
        public double I = Double.MaxValue;
        public double InvI => I < double.MaxValue ? 1.0f / I : 0.0f;

        public void AddForce(Vec2 f)
        {
            Force += f;
        }
    }
}