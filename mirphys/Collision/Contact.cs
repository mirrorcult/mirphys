namespace mirphys
{
    public struct Edges
    {
        public char inEdge1;
        public char outEdge1;
        public char inEdge2;
        public char outEdge2;
    }

    public struct FeaturePair
    {
        public Edges e;
        public int value;
    }

    /// <summary>
    /// Holds data for contact points, including their position, normal, accumulated impulses, etc
    /// Basically a 'manifold' (?)
    /// </summary>
    public struct Contact
    {
        public Vec2 position;
        public Vec2 normal;
        public Vec2 r1, r2;
        public double separation;
        public double Pn; // accumulated normal impulse
        public double Pt; // acc. tangent impulse
        public double Pnb; // acc. norm impulse biased
        public double massNormal, massTangent;
        public double bias;
        public FeaturePair feature;
    }
}