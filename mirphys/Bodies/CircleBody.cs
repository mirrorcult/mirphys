using System;
using mirphys.Bodies;

namespace mirphys
{
    public class CircleBody : Body
    {
        public double Radius = 0.0f;

        public CircleBody(Vec2 p, double r, double m)
        {
            Position = p;
            Radius = Math.Abs(r);
            Mass = Math.Abs(m);
            if (Mass < double.MaxValue)
            {
                I = Mass * Math.Pow(Radius, 4) / Constants.CircularInertiaDivisor; 
            }
        }

    }
}