using System;
using System.Collections.Generic;

namespace mirphys
{
    /// <summary>
    /// Key used to reference arbiters in a dict for World
    /// </summary>
    public struct ArbiterKey
    {
        public Body Body1; 
        public Body Body2;
        
        public ArbiterKey(Body b1, Body b2)
        {
            Body1 = b1;
            Body2 = b2;
        }
    }
    
    /// <summary>
    /// Mediates and holds data about a colliding pair of bodies.
    /// Handles persistence of acc. impulses and coherence
    /// </summary>
    public class Arbiter
    {
        // TODO better data structure
        public List<Contact> Contacts = new();

        private Body Body1;
        private Body Body2;

        // combined friction
        public double Friction;

        public Arbiter(Body b1, Body b2)
        {
            Body1 = b1;
            Body2 = b2;
            // temp
            Contacts = Collide.RectRect((RectBody)Body1, (RectBody)Body2);
            Friction = Math.Sqrt(Body1.Friction * Body2.Friction);
        }

        public void Update(List<Contact> contacts)
        {
            
        }
        
        public void PreStep(double inv_dt)
        {
            
        }

        public void ApplyImpulse()
        {
            
        }
    }
}