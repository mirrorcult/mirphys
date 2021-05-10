using System;

namespace mirphys
{
    public struct Mat22
    {
        public Vec2 Col1;
        public Vec2 Col2;

        Mat22(double angle)
        {
            double c = Math.Cos(angle);
            double s = Math.Sin(angle);

            Col1.X = c;
            Col2.X = -s;
            Col1.Y = s;
            Col2.Y = c;
        }

        public Mat22(Vec2 a, Vec2 b)
        {
            Col1 = new Vec2(a.X, b.X);
            Col2 = new Vec2(a.Y, b.Y);
        }
        
        public Mat22 Transpose()
        {
            return new Mat22(new Vec2(Col1.X, Col2.X), new Vec2(Col1.Y, Col2.Y));
        }

        public Mat22 Invert()
        {
            double det = Col1.X * Col2.Y - Col2.X * Col1.Y;
            Mat22 b = new Mat22();
            if (det != 0.0f)
            {
                det = 1.0f / det;
                // matrix math is so unreadable
                b.Col1.X = det * Col1.Y;
                b.Col2.X = -det * Col2.Y;
                b.Col1.Y = -det * Col2.X;
                b.Col2.Y = det * Col1.X;
                return b;
            }
            throw new DivideByZeroException();
        }

        public Mat22 Abs()
        {
            return new Mat22(Col1.Abs(), Col2.Abs());
        }

        public static Mat22 operator +(Mat22 a, Mat22 b) => new Mat22(a.Col1 + b.Col1, a.Col2 + b.Col2);
        public static Mat22 operator *(Mat22 a, Mat22 b) => new Mat22(a * b.Col1, a * b.Col2);
    }
}