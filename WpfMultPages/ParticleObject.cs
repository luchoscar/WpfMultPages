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
    class ParticleObject
    {
        public WriteableBitmap partTex {set; get;}
        private WriteableBitmap orgText;
        
        public double speedPart = 200.0;
        public Point position;
        public Vector direction;
        double gravity = 10.0;
        private int size;
        double life = 255;
        double particleRotation;
        public Color partColor { set; get; }

        private Color blendAlpha = Color.FromScRgb(1.0f, 1.0f, 1.0f, 1.0f);

        private double[] boundBox = new double[4];
        private int maxSize = 100;
        private int sizeDelta = 5;
        private double currentSize = 50;
        

        public ParticleObject(double xB, double yB, double xB2, double yB2)
        {
            //partTex = WriteableBitmapExtensions.Resize(partTex, 100, 100, WriteableBitmapExtensions.Interpolation.Bilinear);
            partColor = Color.FromScRgb(0.0f, 1.0f, 1.0f, 1.0f);
            size = 5;
            boundBox[0] = xB;
            boundBox[1] = yB;
            boundBox[2] = xB2;
            boundBox[3] = yB2;
        }

        public void Init(WriteableBitmap In_tex, Point In_origin, Vector In_direction, double partRot)
        {
            orgText = In_tex;
            
            particleRotation = partRot;
            direction = In_direction;
            direction.Normalize();
            direction *= speedPart;
            position = In_origin;
        }

        public void UpdatePart(double deltaTime)
        {
            float alpha = (float)deltaTime + partColor.ScA;

            if (alpha > 1.0f) alpha = 1.0f;

            partColor = Color.FromScRgb(alpha, 1.0f, 1.0f, 1.0f);

            //particleRotation += 100 * deltaTime;
            //if (particleRotation >= 360.0) particleRotation = 10.0;

            //*** this takes to long & at some point goes nuts 
            //using (partTex.GetBitmapContext())
            //{
            //    //partTex = WriteableBitmapExtensions.RotateFree(partTex, particleRotation, false);
            //    partTex = WriteableBitmapExtensions.Rotate(partTex, (int)particleRotation);
            //}

            if (currentSize < maxSize)
            {
                currentSize += sizeDelta * deltaTime;
                if (currentSize > maxSize)
                    currentSize = maxSize;

                using (orgText.GetBitmapContext())
                {
                    partTex = WriteableBitmapExtensions.Resize(orgText, (int)currentSize, (int)currentSize, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
                }
            }

            position += direction * deltaTime;

            ChangeDirection();

            life -= deltaTime;
            blendAlpha.A = (byte)life;
        }

        public void ChangeDirection()
        {
            if (position.X <= boundBox[0] || position.X >= boundBox[2])
            {
                if (position.X <= boundBox[0]) position.X = boundBox[0];
                else position.X = boundBox[2];

                direction.X *= -1;

                //ChangeColor();
            }

            if (position.Y <= boundBox[1] || position.Y >= boundBox[3])
            {
                if (position.Y <= boundBox[1]) position.Y = boundBox[1];
                else position.Y = boundBox[3];

                direction.Y *= -1;

                //ChangeColor();
            }
        }

        private void ChangeColor()
        {
            float r = 0.0f, g = 0.0f, b = 0.0f;

            if (partColor.ScR < 1.0f)
            {
                r = partColor.ScR + 0.01f;
            }
            else
            {
                r = 1.0f;

                if (partColor.ScG < 1.0f)
                {
                    g = partColor.ScG + 0.01f;
                }
                else
                {
                    g = 1.0f;

                    if (partColor.ScB < 1.0f)
                    {
                        b = partColor.ScB + 0.01f;
                    }
                    else
                    {
                        
                        b = 1.0f;
                    }
                }
            }

            partColor = Color.FromScRgb(1.0f, r, g, b);
        }
    }
}
