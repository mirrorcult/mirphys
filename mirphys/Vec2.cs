using System;
using System.Configuration;

namespace mirphys
{
    public struct Vec2
    { 
        public double X;
        public double Y;

        public double Length => Math.Sqrt(X * X + Y * Y);

        public Vec2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vec2 Abs()
        {
            return new Vec2(Math.Abs(X), Math.Abs(Y));
        }

        public static Vec2 operator +(Vec2 a, Vec2 b) => new Vec2(a.X + b.X, a.Y + b.Y);
        public static Vec2 operator -(Vec2 a, Vec2 b) => new Vec2(a.X - b.X, a.Y - b.Y);
        public static Vec2 operator *(double s, Vec2 a) => new Vec2(s * a.X, s * a.Y);
        
        // dot product
        public static double operator *(Vec2 a, Vec2 b) => a.X * b.X + a.Y * b.Y;
        
        // cross products in 2D https://stackoverflow.com/questions/243945/calculating-a-2d-vectors-cross-product
        public static double operator %(Vec2 a, Vec2 b) => a.X * b.Y - a.Y * b.X;
        public static Vec2 operator %(double s, Vec2 a) => new Vec2(s * a.Y, -s * a.X);
        public static Vec2 operator %(Vec2 a, double s) => new Vec2(-s * a.Y, s * a.X);
        
        // matrix stuff
        public static Vec2 operator *(Mat22 a, Vec2 v) =>
            new Vec2(a.Col1.X * v.X + a.Col2.X * v.Y, a.Col1.Y * v.X + a.Col2.Y * v.Y);
    }
}