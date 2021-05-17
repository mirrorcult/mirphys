//
//        ^ y
//        |
//        e1
//   v2 ------ v1
//    |        |
// e2 |        | e4  --> x
//    |        |
//   v3 ------ v4
//        e3

using System;
using System.Collections.Generic;

namespace mirphys
{
    public static class Collide
    {
        enum Axis
        {
            FACE_A_X,
            FACE_A_Y,
            FACE_B_X,
            FACE_B_Y
        }

        enum EdgeNumbers
        {
            NO_EDGE = 0,
            EDGE1,
            EDGE2,
            EDGE3,
            EDGE4
        }

        struct ClipVertex
        {
            public Vec2 v;
            public FeaturePair fp;
        }

        public static void Flip(FeaturePair fp)
        {
            var temp1 = fp.e.inEdge2;
            fp.e.inEdge2 = fp.e.inEdge1;
            fp.e.inEdge1 = temp1;
            var temp2 = fp.e.outEdge2;
            fp.e.outEdge2 = fp.e.outEdge1;
            fp.e.outEdge1 = temp2;
        }

        public static List<Contact> AABBAABB()
        {
            return new List<Contact>();
        }

        public static List<Contact> RectRect()
        {
            return new List<Contact>();
        }
    }
}