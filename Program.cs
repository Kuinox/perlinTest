using System;
using System.Collections.Generic;
using System.Text;
using SFML.Audio;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using SFML;

namespace PerlinNoise
{
    class Program
    {
        static void Main(string[] args)
        {
            Noise noise = new Noise();
            float[,] _table;
            RectangleShape[,] _rectangles;
            RenderWindow window = new RenderWindow(new VideoMode(1000, 1000), "LOL", Styles.Titlebar | Styles.Close);
            //int octave = 6;
            //float frequence = 1;
            //float persistance = 0.5f;
            _table = noise.GenerateNoise(300, 1, 6, 4f, 0.5f);
            _rectangles = noise.GraphicPerlin(_table);

            int maxX = _table.GetLength(0);
            int maxY = _table.GetLength(1);
            //char mode = 'F';
            //Random rand = new Random();
            while (true)
            {
                for (int x = 0; x < maxX; x++)
                {
                    for (int y = 0; y < maxY; y++)
                    {
                        window.Draw(_rectangles[x, y]);
                    }
                }
                /*if (Keyboard.IsKeyPressed(Keyboard.Key.F)){
                    mode = 'f';
                    window.Clear(Color.Red);
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.O)) { 
                    mode = 'o';
                    window.Clear(Color.Red);
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.P)) { 
                    mode = 'p';
                    window.Clear(Color.Red);
                }

                if(Keyboard.IsKeyPressed(Keyboard.Key.Add))
                {
                    switch(mode)
                    {
                        case 'f':
                            frequence += 0.1f;
                            break;
                        case 'o':
                            octave += 1;
                            break;
                        case 'p':
                            persistance += 0.1f;
                            break;

                    }
                   // window.Clear(Color.Blue);
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.Subtract))
                {
                    switch (mode)
                    {
                        case 'f':
                            frequence -= 0.1f;
                            break;
                        case 'o':
                            octave -= 1;
                            break;
                        case 'p':
                            persistance -= 0.1f;
                            break;

                    }
                    //.Clear(Color.Blue);
                }

                
                if (rand.Next() % 5 == 0)
                {
                    _table = noise.GenerateNoise(300, 1, octave, frequence, persistance);
                    _rectangles = noise.GraphicPerlin(_table);
                    window.Clear(Color.Green);
                }*/
                window.Display();
                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}
