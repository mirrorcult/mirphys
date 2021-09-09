using System.Collections.Generic;
using System.Linq;
using mirphys.Bodies;

namespace mirphys
{
    public class World
    {
        public List<Body> Bodies = new();
        public List<Joint> Joints = new();
        private Dictionary<ArbiterKey, Arbiter> Arbiters = new();
        
        private int Iterations;
        private Vec2 Gravity;
        public static bool AccumulateImpulses = true;
        public static bool WarmStarting = true;
        public static bool PositionCorrection = true;

        public World(Vec2 gravity, int iterations)
        {
            Gravity = gravity;
            Iterations = iterations;
        }

        public void Add(Body b)
        {
            Bodies.Add(b);
        }

        public void Add(Joint j)
        {
            Joints.Add(j);
        }

        public void Clear()
        {
            Bodies.Clear();
            Joints.Clear();
            Arbiters.Clear();
        }
        
        /// <summary>
        /// Broadphase, handles finding potential colliding pairs of bodies
        /// </summary>
        // todo replace with K-D tree or BVH
        private void Broadphase()
        {
            for (var i = 0; i < Bodies.Count; i++)
            {
                var bi = Bodies[i];

                for (var j = i + 1; j < Bodies.Count; j++)
                {
                    var bj = Bodies[j];

                    if (bj.InvMass == 0.0f && bj.InvMass == 0.0f)
                        continue;

                    var arb = new Arbiter(bi, bj);
                    var key = new ArbiterKey(bi, bj);

                    if (arb.Contacts.Count > 0)
                    {
                        if (!Arbiters.ContainsKey(key))
                        {
                            Arbiters.Add(key, arb);
                        }
                        else
                        {
                            Arbiters.TryGetValue(key, out var oldarb);
                            oldarb.Update(arb.Contacts);
                        }
                    }
                    else
                    {
                        Arbiters.Remove(key);
                    }
                }
            }
        }

        public void Step(double dt)
        {
            double inv_dt = dt > 0.0f ? 1.0f / dt : 0.0f;
            
            Broadphase();

            // Integrate forces
            foreach (Body b in Bodies)
            {
                if (b.InvMass == 0.0f)
                    continue;

                b.LinearVelocity += dt * (Gravity + b.InvMass * b.Force);
                b.AngularVelocity += dt * b.InvI * b.Torque;
            }
            
            // Pre-steps
            foreach (Arbiter a in Arbiters.Values)
            {
                a.PreStep(inv_dt);
            }
            
            // todo joint pre-step
            
            // Iterations ( who cares about convergence tbh )
            for (var i = 0; i < Iterations; i++)
            {
                foreach (Arbiter a in Arbiters.Values)
                {
                    a.ApplyImpulse();
                }
                
                // todo joint impulses
            }
            
            // Integrate velocities
            foreach (Body b in Bodies)
            {
                b.Position += dt * b.LinearVelocity;
                b.Rotation += dt * b.AngularVelocity;

                b.Force = new(0.0f, 0.0f);
                b.Torque = 0.0f;
            }
        }
    }
}