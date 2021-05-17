using System.Collections.Generic;
using System.Net.Sockets;

namespace mirphys
{
    public class World
    {
        public List<Body> Bodies;
        public List<Joint> Joints;
        public Dictionary<ArbiterKey, Arbiter> Arbiters;
        
        public int Iterations;
        public Vec2 Gravity;
        public static bool AccumulateImpulses;
        public static bool WarmStarting;
        public static bool PositionCorrection;

        public World(Vec2 gravity, int iterations)
        {
            Gravity = gravity;
            Iterations = iterations;
        }

        public void Add(Body b)
        {
            
        }

        public void Add(Joint j)
        {
            
        }

        public void Clear()
        {
            
        }

        public void Step(double dt)
        {
            
        }

        public void Broadphase()
        {
            
        }
    }
}