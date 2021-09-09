using System;

namespace mirphys.Bodies
{
    public class RectBody : Body
    {
        public Vec2 Size;

        public Vec2 BottomLeft => new Vec2(Position.X - Size.X / 2, Position.Y - Size.Y / 2);
        public Vec2 BottomRight => new Vec2(Position.X + Size.X / 2, Position.Y - Size.Y / 2);
        public Vec2 TopLeft => new Vec2(Position.X - Size.X / 2, Position.Y + Size.Y / 2);
        public Vec2 TopRight => new Vec2(Position.X + Size.X / 2, Position.Y + Size.Y / 2);

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