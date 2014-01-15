using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace WpfMultPages
{
    class ParticleEmitter
    {
        public int totalPart { set; get; }
        public int partRate { set; get; }
        public WriteableBitmap partTexture { set; get; }
        public Point emitterPosition = new Point(250,250);
        public WriteableBitmap TextureDestination { set; get;}
        private List<ParticleObject> particles = new List<ParticleObject>();
        public double particleRate { set; get; }

        Random rand = new Random();

        public ParticleEmitter(int particlesPerSecond, WriteableBitmap In_partTexture)
        {
            using (In_partTexture.GetBitmapContext())
            {
                partTexture = WriteableBitmapExtensions.Resize(In_partTexture, 100, 100, WriteableBitmapExtensions.Interpolation.Bilinear);
            }
            particleRate = 1.0 / ((double)particlesPerSecond);
        }
        
        public void DrawParticles(double deltaTime)
        {
            using (TextureDestination.GetBitmapContext())
            {
                for (int i = particles.Count - 1; i >= 0; i--)
                //for (int i = 0; i < particles.Count; i++)
                {
                    particles[i].UpdatePart(deltaTime);
                    //WriteableBitmapExtensions.FillEllipseCentered(TextureDestination, (int)particles[i].position.X, (int)particles[i].position.Y, 14, 20, particles[i].partColor);
                    TextureDestination.Blit(particles[i].position, particles[i].partTex, new Rect(0.0, 0.0, particles[i].partTex.Width, particles[i].partTex.Height), particles[i].partColor, WriteableBitmapExtensions.BlendMode.Alpha);
                }
            }
        }

        public void InitEmitter(int In_totalPart, WriteableBitmap In_partTex)
        {
            totalPart = In_totalPart;
        }

        double xOffset = 700, yOffset = 500;
        public void CreatePartilce()
        {
            ParticleObject particle = new ParticleObject(emitterPosition.X - xOffset, emitterPosition.Y - yOffset, emitterPosition.X + xOffset, emitterPosition.Y + yOffset);
            double xDir = rand.NextDouble() * (1.0 - (-1)) + (-1);
            double yDir = rand.NextDouble() * (1.0 - (-1)) + (-1);
            particle.Init(partTexture, emitterPosition, new Vector(xDir, yDir), (rand.NextDouble() * 360.0));
            particles.Add(particle);
        }

        public int GetTotalParticles()
        {
            return particles.Count;
        }
    }
}
