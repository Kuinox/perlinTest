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
    class Noise
    {
        // INITIALISE LE TABLEAU DE FLOAT RANDOM POUR LA GENERATION
        public float[,] GenerateWhiteNoise(int width, int height, int seed)
        {
            Random random = new Random(seed); //INITIALISE LE SEED
            float[,] noise = new float[width, height]; //INITIALISE UN TABLEAU 2D DE FLOAT

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    noise[i, j] = (float)random.NextDouble() % 1; // REMPLI LE TABLEAU DE FLOAT ENTRE 0 ET 1
                }
            }
            return noise; // RETURN LE TABLEAU
        }

        // FONCTION QUI VA LISSER LES VALEURS (INTERPOLATION DES POINTS)
        public void GenerateSmoothNoise(float[,] baseNoise, int octave, float frequency, float[,,] smoothNoise)
        {
            int width = baseNoise.GetLength(0);
            int height = baseNoise.GetLength(1);

            int samplePeriod = 1 << octave; // CALCULE 2 PUISSANCE OCTAVE
            float sampleFrequency = 1.0f / (samplePeriod * frequency); // CALCULE LA FREQUENCE
            //sampleFrequency = 0.1f / samplePeriod;

            for (int i = 0; i < width; i++)
            {
                // CALCULE LE VECTEUR HORIZONTAL
                int sample_i0 = (i / samplePeriod) * samplePeriod; // truncate number
                int sample_i1 = (sample_i0 + samplePeriod) % width;
                float horizontal_blend = (i - sample_i0) * sampleFrequency;

                for (int j = 0; j < height; j++)
                {
                    // CALCULE LE VECTEUR VERTICAL
                    int sample_j0 = (j / samplePeriod) * samplePeriod; // truncate number
                    int sample_j1 = (sample_j0 + samplePeriod) % height;
                    float vertical_blend = (j - sample_j0) * sampleFrequency;

                    //INTERPOLATION DES 2 POINTS DU HAUT
                    float top = Interpolate(baseNoise[sample_i0, sample_j0], baseNoise[sample_i1, sample_j0], horizontal_blend);

                    //INTERPOLATION DES 2 POINTS DU BAS
                    float bottom = Interpolate(baseNoise[sample_i0, sample_j1], baseNoise[sample_i1, sample_j1], horizontal_blend);

                    //INTERPOLATION DES 4 POINTS
                    smoothNoise[octave, i, j] = Interpolate(top, bottom, vertical_blend);
                }
            }

        }

        // FONCTION D'INTERPOLATION
        private float Interpolate(float x0, float x1, float alpha)
        {
            return x0 * (1 - alpha) + x1 * alpha;
        }

        // FONCTION QUI VA GENERER LE BRUIT FINAL
        public float[,] GeneratePerlinNoise(float[,] baseNoise, int octaveCount, float persistence, float frequency)
        {
            int width = baseNoise.GetLength(0);
            int height = baseNoise.GetLength(1);

            float[,,] smoothNoise = new float[octaveCount, width, height];

            //GENERE LE BRUIT LISSE
            for (int k = 0; k < octaveCount; k++)
            {
                GenerateSmoothNoise(baseNoise, k, frequency, smoothNoise);
            }

            float[,] perlinNoise = new float[width, height];
            float amplitude = 1.0f;
            float totalAmplitude = 0.0f;

            //blend noise together
            for (int octave = octaveCount - 1; octave >= 0; octave--)
            {
                amplitude *= persistence;
                totalAmplitude += amplitude;

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        perlinNoise[i, j] += smoothNoise[octave, i, j] * amplitude;
                    }
                }
            }

            //normalisation
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    perlinNoise[i, j] /= totalAmplitude;
                }
            }

            return perlinNoise;
        }

        // FONCTION DE GENERATION DU BRUIT DE PERLIN

        // PERSISTANCE MUST BE BETWEEN 0 AND 1 SINON FCK U // 0.5 BY DEFAULT
        // FREQUENCE MUST BE BETWEEN 1 AND 16 SINON FCK U // 1 BY DEFAULT
        // OCTAVES MUST BE BETWEEN 1 AND 6 SINON FCK U // 6 BY DEFAULT
        public float[,] GenerateNoise(ulong taille, int seed, int octaves = 6, float frequence = 1, float persistance = (float)0.5)
        {
            ulong _taille;
            Noise _noise = new Noise();
            float[,] _table;

            if (persistance > 1 || persistance < 0)
            {
                throw new ArgumentException("Persistance must be between 0 and 1");
            }
            if (frequence > 16 || frequence < 0)
            {
                throw new ArgumentException("Frequence must be between 1 and 16");
            }
            if (octaves > 8 || octaves < 1)
            {
                throw new ArgumentException("Octaves must be between 1 and 8");
            }

            _taille = taille;

            _table = _noise.GenerateWhiteNoise((int)_taille, (int)_taille, seed);

            _table = _noise.GeneratePerlinNoise(_table, octaves, persistance, frequence);

            return _table;
        }

        public RectangleShape[,] GraphicPerlin(float[,] table)
        {
            RectangleShape[,] _rectangles;
            Noise noise = new Noise();



            int maxX = table.GetLength(0);
            int maxY = table.GetLength(1);


            _rectangles = new RectangleShape[maxX, maxY];

            for (int i = 0; i < maxX; i++)
            {
                for (int j = 0; j < maxY; j++)
                {
                    Color color = new Color((byte)(table[i, j] * 255), 0, 0);

                    _rectangles[i, j] = new RectangleShape();
                    _rectangles[i, j].Size = new Vector2f(14, 14);
                    _rectangles[i, j].Position = new Vector2f(i * 14, j * 14);
                    _rectangles[i, j].FillColor = color;
                }
            }
            return _rectangles;
        }
    }
}

