using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SoftwareGraphicsSandbox {
    class Mesh {

        // Field of class
        public Point3D[] Vertices;

        // Constructor
        public Mesh(IEnumerable<Point3D> vertices) {
            Vertices = vertices.ToArray();
        }
        
        // rows, columns - number of CELLS in a grid. NOT vertices, blyat.
        public static Mesh Plane(int rows, int columns) {
            int verticesNumber = (rows + 1) * (columns + 1);
            int triangleNumber = rows * columns * 2;
            int triangleVerticesNumber = triangleNumber * 3;

            // List is like a array
            Point3D[] triangleVertices = new Point3D[triangleVerticesNumber];

            float cellRow = 1f / rows;
            float cellColumn = 1f / columns;
            int i = 0;
            for (int c = 0; c < columns; c++) {
                float startZ = 0.5f - c * cellColumn;
                for (int r = 0; r < rows; r++) {
                    float startX = -0.5f + r * cellRow;
                    

                    Point3D LeftTop = new Point3D(startX, 0, startZ);
                    Point3D RightTop = new Point3D(startX + cellRow, 0, startZ);
                    Point3D LeftBottom = new Point3D(startX, 0, startZ - cellColumn);
                    Point3D RightBottom = new Point3D(startX + cellRow, 0, startZ - cellColumn);

                    triangleVertices[i] = LeftTop;
                    triangleVertices[i + 1] = RightTop;
                    triangleVertices[i + 2] = LeftBottom;

                    triangleVertices[i + 3] = RightTop;
                    triangleVertices[i + 4] = LeftBottom;
                    triangleVertices[i + 5] = RightBottom;

                    i += 6;



                }
            }


            

            
            return new Mesh(triangleVertices);

        }

        public static Mesh Cube() {
            var vertices = new Point3D[] {


                new Point3D(-0.5f, 0.5f, 0.5f),
                new Point3D(0.5f, 0.5f, 0.5f),
                new Point3D(0.5f, 0.5f, -0.5f),

                new Point3D(-0.5f, 0.5f, 0.5f),
                new Point3D(0.5f, 0.5f, -0.5f),
                new Point3D(-0.5f, 0.5f, -0.5f),

                new Point3D(-0.5f, -0.5f, 0.5f),
                new Point3D(0.5f, -0.5f, 0.5f),
                new Point3D(0.5f, -0.5f, -0.5f),

                new Point3D(-0.5f, -0.5f, 0.5f),
                new Point3D(0.5f, -0.5f, -0.5f),
                new Point3D(-0.5f, -0.5f, -0.5f),

                new Point3D(-0.5f, 0.5f, 0.5f),
                new Point3D(0.5f, 0.5f, 0.5f),
                new Point3D(0.5f, -0.5f, 0.5f),

                new Point3D(-0.5f, 0.5f, 0.5f),
                new Point3D(-0.5f, -0.5f, 0.5f),
                new Point3D(0.5f, -0.5f, 0.5f),

                new Point3D(0.5f, 0.5f, 0.5f),
                new Point3D(0.5f, -0.5f, 0.5f),
                new Point3D(0.5f, -0.5f, -0.5f),

                new Point3D(0.5f, 0.5f, 0.5f),
                new Point3D(0.5f, 0.5f, -0.5f),
                new Point3D(0.5f, -0.5f, -0.5f),

                new Point3D(0.5f, 0.5f, 0.5f),
                new Point3D(0.5f, 0.5f, -0.5f),
                new Point3D(0.5f, -0.5f, -0.5f),

                new Point3D(-0.5f, 0.5f, -0.5f),
                new Point3D(0.5f, -0.5f, -0.5f),
                new Point3D(-0.5f, -0.5f, -0.5f),

                new Point3D(-0.5f, 0.5f, -0.5f),
                new Point3D(-0.5f, -0.5f, 0.5f),
                new Point3D(-0.5f, -0.5f, -0.5f),

                new Point3D(-0.5f, 0.5f, 0.5f),
                new Point3D(-0.5f, 0.5f, -0.5f),
                new Point3D(-0.5f, -0.5f, 0.5f),

                };
            return new Mesh(vertices);
        }

        public static Mesh Sphere(int rows, int columns) {
            int verticesNumber = (rows + 1) * (columns + 1);

            int triangleNumber = rows * columns * 2;
            int triangleVerticesNumber = triangleNumber * 3;

            Point3D[] triangleVertices = new Point3D[triangleVerticesNumber];

            float cellRow = MathF.PI / rows;
            float cellColumn = 2 * MathF.PI / columns;

            float teta = MathF.PI;
            float psi = 2 * MathF.PI;

            int i = 0;

            for (int c = 0; c < columns; c++) {
                teta -= c * cellColumn;
                for (int r = 0; r < rows; r++) {
                    psi -= r * cellRow;
                    Point3D firstTop = new Point3D(MathF.Sin(teta) * MathF.Cos(psi), MathF.Sin(teta) * MathF.Sin(psi), MathF.Cos(teta));
                    Point3D secondTop = new Point3D(MathF.Sin(teta - cellColumn) * MathF.Cos(psi), MathF.Sin(teta - cellColumn) * MathF.Sin(psi), MathF.Cos(teta - cellColumn));
                    Point3D firstBottom = new Point3D(MathF.Sin(teta) * MathF.Cos(psi - cellRow), MathF.Sin(teta) * MathF.Sin(psi - cellRow), MathF.Cos(teta));
                    Point3D secondBottom = new Point3D(MathF.Sin(teta - cellColumn) * MathF.Cos(psi - cellRow), MathF.Sin(teta - cellColumn) * MathF.Sin(psi - cellRow), MathF.Cos(teta - cellColumn));

                    triangleVertices[i] = firstTop;
                    triangleVertices[i + 1] = secondTop;
                    triangleVertices[i + 2] = firstBottom;

                    triangleVertices[i + 3] = secondTop;
                    triangleVertices[i + 4] = firstBottom;
                    triangleVertices[i + 5] = secondBottom;

                    i += 6;

                }
            }

            return new Mesh(triangleVertices);


        }

        




    }
}

