using System;
using System.Windows.Forms;

namespace SoftwareGraphicsSandbox {
    static class Program
    {
        private static Image image;
        private static bool _running = true;
       
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = new RenderWindow();
            form.Width = 1024;
            form.Height = 768;
            form.FormClosed += Form_FormClosed;
            form.Show();

            
            Renderer renderer = new Renderer(form.Handle);
            image = renderer.BackBuffer;

            void BresenhamLineOneQuadrant(int x0, int x1, int y0 , int y1, Color32 color)
            {



                //var exchangeXY = (Math.Abs(y1 - y0) > Math.Abs(x1 - x0));


                //if (exchangeXY)
                //{
                //    int e1 = x0;
                //    x0 = y0;
                //    y0 = e1;
                //    int e2 = x1;
                //    x1 = y1;
                //    y1 = e2;
                //}

                int _x0 =  x0;
                int _y0 = y0;
                int _x1 = x1;
                int _y1 = y1;

                int signX = _x0 < _x1 ? 1 : -1;
                int signY = _y0 < _y1 ? -1 : 1;
                

                float dx = _x1 - _x0;
                float dy = _y1 - _y0;

                float k = Math.Abs(dy / dx);
            
                float error = 1;


                if (dx > dy) { 
                int y = _y0;
                for (int x = _x0; x <= x1;) {
                        image.SetPixel(x, y, color);
                        error += k;
                        x++;
                        if (error >= 1)
                    {
                        
                        y -= 1;
                        error -= 1;
                    }
                    }
                }

                if (dy > dx)
                {
                    int x = _x0;
                    for (int y = _y0; y <= _y1; y++)
                    {
                        image.SetPixel(x, y, color);
                        error += 1/k;
                        
                        if (error >= 1)
                        {

                            x++;
                            error -= 1;
                        }
                    }
                }


            }




            int time = 1;
            int a = 1;

            while (_running){
                //Render image 
                image.FillColor(Color32.Black);
                //Process window messages
                Application.DoEvents();


                BresenhamLineOneQuadrant(400, 500, 400, 1000, Color32.Red);
                time++;
                if (time % 2 == 0) a++;



                //for (int x = 0; x < form.Width; x++){
                //    image.SetPixel(x, form.Height / 2, Color32.Red);
                //        }
                //for (int y = 0; y < form.Height; y++){
                //    image.SetPixel(form.Width / 2, y, Color32.Red);
                //}

                //Show image in window
                renderer.Present();
            }
        }


        private static void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            _running = false;
        }
    }
}
