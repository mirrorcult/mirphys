using System;

namespace mirphys
{
    public static class Constants
    {
        public const double BaseFriction = 0.2f;
        public const double BaseWidth = 1.0f;
        
        // https://web.iit.edu/sites/web/files/departments/academic-affairs/academic-resource-center/pdfs/Moment_Inertia.pdf
        // maybe?
        public const double RectangularInertiaDivisor = 1.0f / 12.0f;
        public const double CircularInertiaDivisor = Math.PI * (1.0f / 2.0f);
    }
}