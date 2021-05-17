using System;

namespace mirphys
{
    public class RectBody : Body
    {
        public Vec2 Size = new Vec2(0.0f, 0.0f);
        
        public RectBody(Vec2 p, Vec2 s, double m)
        {
            Position = p;
            Size = s;
            Mass = Math.Abs(m);
            if (Mass < double.MaxValue)
            {
                I = Mass * (Size.X * Size.X + Size.Y * Size.Y) / Constants.RectangularInertiaDivisor;
            }
        }
    }
}