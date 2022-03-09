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

            static void BresenhamLineOneQuadrant(int x0, int y0, int x1, int y1, Color32 color)
            {
                int k;
                
                int _x0 = x0;
                int _y0 = y0;
                int _x1 = x1;
                int _y1 = y1;

                int y = _y0;

                

                // reversed angle coefficient
                k = (x1 - x0) / (y1 - y0);
                
                int error = 0;


                for (int x = x0; x <= x1; x++)
                {
                    image.SetPixel(x, y, color);
                    error++;
                    if (error % k == 0) y--;

                }

            }

            while(_running){
                //Render image 
                image.FillColor(Color32.Black);
                //Process window messages
                Application.DoEvents();


                //example draw horizontal line
                for (int x = 0; x < 100; ++x) {
                    image.SetPixel(x + 100, 100, Color32.Green);
                }


                BresenhamLineOneQuadrant(250, 250, 750, 500, Color32.Red);

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
