using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareGraphicsSandbox {
    struct Point4D {
        public float X;
        public float Y;
        public float Z;
        public float W;
        
        public Point4D(float x, float y, float z, float w) {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        public Point4D(Point3D p) {
            X = p.X;
            Y = p.Y;
            Z = p.Z;
            W = 1.0f;
        }
    }
}
