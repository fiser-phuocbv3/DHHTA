using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace DHHTA.Core
{
    public class Matrix
    {
        public int TopLeft = 0, TopMid = 0, TopRight = 0;
        public int MidLeft = 0, Pixel = 1, MidRight = 0;
        public int BottomLeft = 0, BottomMid = 0, BottomRight = 0;
        public int Factor = 1;
        public int Offset = 0;
        public void SetAll(int nVal)
        {
            TopLeft = TopMid = TopRight = MidLeft = Pixel = MidRight = BottomLeft = BottomMid = BottomRight = nVal;
        }
    }

    public struct FloatPoint
    {
        public double X;
        public double Y;
    }

    public class Process
    {
        public const short EDGE_DETECT_KIRSH = 1;
        public const short EDGE_DETECT_PREWITT = 2;
        public const short EDGE_DETECT_SOBEL = 3;

        public static bool Invert(Bitmap b)
        {
            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            IntPtr Scan0 = bmData.Scan0;
            
            unsafe
            {
                byte * p = (byte*)(void*)Scan0;

                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width * 3;

                for (int y = 0; y < b.Height; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        p[0] = (byte)(255 - p[0]);
                        ++p;
                    }
                    p += nOffset;
                }
            }
            b.UnlockBits(bmData);            
            return true;
        }

        public static bool GrayScale(Bitmap b)
        {
            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - b.Width * 3;
                byte red, green, blue;

                for (int y = 0; y < b.Height; ++y)
                {
                    for (int x = 0; x < b.Width; ++x)
                    {
                        blue = p[0];
                        green = p[1];
                        red = p[2];
                        p[0] = p[1] = p[2] = (byte)(.299 * red + .587 * green + .114 * blue);
                        p += 3;
                    }
                    p += nOffset;
                }
            }

            b.UnlockBits(bmData);
            return true;
        }

        //increate bright of image
        public static bool Brightness(Bitmap b, int nBrightness)
        {
            if (nBrightness < -255 || nBrightness > 255)
                return false;
  
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            int nVal = 0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width * 3;
                for (int y = 0; y < b.Height; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        nVal = (int)(p[0] + nBrightness);
                        if (nVal < 0) nVal = 0;
                        if (nVal > 255) nVal = 255;
                        p[0] = (byte)nVal;
                        ++p;
                    }
                    p += nOffset;
                }
            }
            b.UnlockBits(bmData);
            return true;
        }

        public static bool Contrast(Bitmap b, sbyte nContrast)
        {
            if (nContrast < -100) return false;
            if (nContrast > 100) return false;

            double pixel = 0, contrast = (100.0 + nContrast) / 100.0;
            contrast *= contrast;
            int red, green, blue;

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - b.Width * 3;
                for (int y = 0; y < b.Height; ++y)
                {
                    for (int x = 0; x < b.Width; ++x)
                    {
                        blue = p[0];
                        green = p[1];
                        red = p[2];

                        pixel = red / 255.0;
                        pixel -= 0.5;
                        pixel *= contrast;
                        pixel += 0.5;
                        pixel *= 255;
                        if (pixel < 0) pixel = 0;
                        if (pixel > 255) pixel = 255;
                        p[2] = (byte)pixel;

                        pixel = green / 255.0;
                        pixel -= 0.5;
                        pixel *= contrast;
                        pixel += 0.5;
                        pixel *= 255;
                        if (pixel < 0) pixel = 0;
                        if (pixel > 255) pixel = 255;
                        p[1] = (byte)pixel;

                        pixel = blue / 255.0;
                        pixel -= 0.5;
                        pixel *= contrast;
                        pixel += 0.5;
                        pixel *= 255;
                        if (pixel < 0) pixel = 0;
                        if (pixel > 255) pixel = 255;
                        p[0] = (byte)pixel;

                        p += 3;
                    }
                    p += nOffset;
                }
            }
            b.UnlockBits(bmData);
            return true;
        }

        public static bool Color(Bitmap b, int red, int green, int blue)
        {
            if (red < -255 || red > 255) return false;
            if (green < -255 || green > 255) return false;
            if (blue < -255 || blue > 255) return false;

            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - b.Width * 3;
                int nPixel;

                for (int y = 0; y < b.Height; ++y)
                {
                    for (int x = 0; x < b.Width; ++x)
                    {
                        nPixel = p[2] + red;
                        nPixel = Math.Max(nPixel, 0);
                        p[2] = (byte)Math.Min(255, nPixel);

                        nPixel = p[1] + green;
                        nPixel = Math.Max(nPixel, 0);
                        p[1] = (byte)Math.Min(255, nPixel);

                        nPixel = p[0] + blue;
                        nPixel = Math.Max(nPixel, 0);
                        p[0] = (byte)Math.Min(255, nPixel);

                        p += 3;
                    }
                    p += nOffset;
                }
            }
            b.UnlockBits(bmData);
            return true;
        }

        public static bool Smooth(Bitmap b, int nWeight)
        {
            Matrix m = new Matrix();
            m.SetAll(1);
            m.Pixel = nWeight;
            m.Factor = nWeight + 8;
            return Process.Convert3x3(b, m);
        }

        public static bool EdgeDetectConvolution(Bitmap b, short nType, byte nThreshold)
        {
            Matrix m = new Matrix();

            // I need to make a copy of this bitmap BEFORE I alter it 80)
            Bitmap bTemp = (Bitmap)b.Clone();

            switch (nType)
            {
                case EDGE_DETECT_SOBEL:
                    m.SetAll(0);
                    m.TopLeft = m.BottomLeft = 1;
                    m.TopRight = m.BottomRight = -1;
                    m.MidLeft = 2;
                    m.MidRight = -2;
                    m.Offset = 0;
                    break;
                case EDGE_DETECT_PREWITT:
                    m.SetAll(0);
                    m.TopLeft = m.MidLeft = m.BottomLeft = -1;
                    m.TopRight = m.MidRight = m.BottomRight = 1;
                    m.Offset = 0;
                    break;
                case EDGE_DETECT_KIRSH:
                    m.SetAll(-3);
                    m.Pixel = 0;
                    m.TopLeft = m.MidLeft = m.BottomLeft = 5;
                    m.Offset = 0;
                    break;
            }

            Process.Convert3x3(b, m);

            switch (nType)
            {
                case EDGE_DETECT_SOBEL:
                    m.SetAll(0);
                    m.TopLeft = m.TopRight = 1;
                    m.BottomLeft = m.BottomRight = -1;
                    m.TopMid = 2;
                    m.BottomMid = -2;
                    m.Offset = 0;
                    break;
                case EDGE_DETECT_PREWITT:
                    m.SetAll(0);
                    m.BottomLeft = m.BottomMid = m.BottomRight = -1;
                    m.TopLeft = m.TopMid = m.TopRight = 1;
                    m.Offset = 0;
                    break;
                case EDGE_DETECT_KIRSH:
                    m.SetAll(-3);
                    m.Pixel = 0;
                    m.BottomLeft = m.BottomMid = m.BottomRight = 5;
                    m.Offset = 0;
                    break;
            }

            Process.Convert3x3(bTemp, m);

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmData2 = bTemp.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr Scan02 = bmData2.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* p2 = (byte*)(void*)Scan02;

                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width * 3;
                int nPixel = 0;
                for (int y = 0; y < b.Height; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        nPixel = (int)Math.Sqrt((p[0] * p[0]) + (p2[0] * p2[0]));
                        if (nPixel < nThreshold) nPixel = nThreshold;
                        if (nPixel > 255) nPixel = 255;
                        p[0] = (byte)nPixel;
                        ++p;
                        ++p2;
                    }
                    p += nOffset;
                    p2 += nOffset;
                }
            }
            b.UnlockBits(bmData);
            bTemp.UnlockBits(bmData2);
            return true;
        }

        public static bool EdgeDetectHorizontal(Bitmap b)
        {
            Bitmap bmTemp = (Bitmap)b.Clone();
        
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmData2 = bmTemp.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr Scan02 = bmData2.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* p2 = (byte*)(void*)Scan02;

                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width * 3;
                int nPixel = 0;
                p += stride;
                p2 += stride;
                for (int y = 1; y < b.Height - 1; ++y)
                {
                    p += 9;
                    p2 += 9;
                    for (int x = 9; x < nWidth - 9; ++x)
                    {
                        nPixel = ((p2 + stride - 9)[0] +
                            (p2 + stride - 6)[0] +
                            (p2 + stride - 3)[0] +
                            (p2 + stride)[0] +
                            (p2 + stride + 3)[0] +
                            (p2 + stride + 6)[0] +
                            (p2 + stride + 9)[0] -
                            (p2 - stride - 9)[0] -
                            (p2 - stride - 6)[0] -
                            (p2 - stride - 3)[0] -
                            (p2 - stride)[0] -
                            (p2 - stride + 3)[0] -
                            (p2 - stride + 6)[0] -
                            (p2 - stride + 9)[0]);
                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;
                        (p + stride)[0] = (byte)nPixel;
                        ++p;
                        ++p2;
                    }
                    p += 9 + nOffset;
                    p2 += 9 + nOffset;
                }
            }
            b.UnlockBits(bmData);
            bmTemp.UnlockBits(bmData2);
            return true;
        }

        public static bool EdgeDetectVertical(Bitmap b)
        {
            Bitmap bmTemp = (Bitmap)b.Clone();

            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmData2 = bmTemp.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr Scan02 = bmData2.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* p2 = (byte*)(void*)Scan02;

                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width * 3;
                int nPixel = 0;
                int nStride2 = stride * 2;
                int nStride3 = stride * 3;

                p += nStride3;
                p2 += nStride3;

                for (int y = 3; y < b.Height - 3; ++y)
                {
                    p += 3;
                    p2 += 3;
                    for (int x = 3; x < nWidth - 3; ++x)
                    {
                        nPixel = ((p2 + nStride3 + 3)[0] +
                            (p2 + nStride2 + 3)[0] +
                            (p2 + stride + 3)[0] +
                            (p2 + 3)[0] +
                            (p2 - stride + 3)[0] +
                            (p2 - nStride2 + 3)[0] +
                            (p2 - nStride3 + 3)[0] -
                            (p2 + nStride3 - 3)[0] -
                            (p2 + nStride2 - 3)[0] -
                            (p2 + stride - 3)[0] -
                            (p2 - 3)[0] -
                            (p2 - stride - 3)[0] -
                            (p2 - nStride2 - 3)[0] -
                            (p2 - nStride3 - 3)[0]);
                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;
                        p[0] = (byte)nPixel;
                        ++p;
                        ++p2;
                    }
                    p += 3 + nOffset;
                    p2 += 3 + nOffset;
                }
            }
            b.UnlockBits(bmData);
            bmTemp.UnlockBits(bmData2);
            return true;
        }

        //xoay anh theo chieu doc hoac chieu ngang
        public static bool FlipHorzOrVert(Bitmap b, bool horz, bool vert)
        {
            Point[,] ptFlip = new Point[b.Width, b.Height];

            int nWidth = b.Width;
            int nHeight = b.Height;
            for (int x = 0; x < nWidth; ++x)
                for (int y = 0; y < nHeight; ++y)
                {
                    ptFlip[x, y].X = (horz) ? nWidth - (x + 1) : x;
                    ptFlip[x, y].Y = (vert) ? nHeight - (y + 1) : y;
                }
            return OffsetFilterAbs(b, ptFlip);      
        }

        public static bool Swirl(Bitmap b, double fDegree, bool bSmoothing /* default fDegree to .05 */)
        {
            int nWidth = b.Width;
            int nHeight = b.Height;

            FloatPoint[,] fp = new FloatPoint[nWidth, nHeight];
            Point[,] pt = new Point[nWidth, nHeight];

            Point mid = new Point();
            mid.X = nWidth / 2;
            mid.Y = nHeight / 2;

            double theta, radius;
            double newX, newY;

            for (int x = 0; x < nWidth; ++x)
                for (int y = 0; y < nHeight; ++y)
                {
                    int trueX = x - mid.X;
                    int trueY = y - mid.Y;
                    theta = Math.Atan2((trueY), (trueX));

                    radius = Math.Sqrt(trueX * trueX + trueY * trueY);

                    newX = mid.X + (radius * Math.Cos(theta + fDegree * radius));
                    if (newX > 0 && newX < nWidth)
                    {
                        fp[x, y].X = newX;
                        pt[x, y].X = (int)newX;
                    }
                    else
                        fp[x, y].X = pt[x, y].X = x;

                    newY = mid.Y + (radius * Math.Sin(theta + fDegree * radius));
                    if (newY > 0 && newY < nHeight)
                    {
                        fp[x, y].Y = newY;
                        pt[x, y].Y = (int)newY;
                    }
                    else
                        fp[x, y].Y = pt[x, y].Y = y;
                }

            if (bSmoothing)
                OffsetFilterAntiAlias(b, fp);
            else
                OffsetFilterAbs(b, pt);
            return true;
        }

        //taohieu ung guong cau loi
        public static bool Sphere(Bitmap b, bool bSmoothing)
        {
            int nWidth = b.Width;
            int nHeight = b.Height;

            FloatPoint[,] fp = new FloatPoint[nWidth, nHeight];
            Point[,] pt = new Point[nWidth, nHeight];

            Point mid = new Point();
            mid.X = nWidth / 2;
            mid.Y = nHeight / 2;

            double theta, radius;
            double newX, newY;

            for (int x = 0; x < nWidth; ++x)
                for (int y = 0; y < nHeight; ++y)
                {
                    int trueX = x - mid.X;
                    int trueY = y - mid.Y;
                    theta = Math.Atan2((trueY), (trueX));

                    radius = Math.Sqrt(trueX * trueX + trueY * trueY);

                    double newRadius = radius * radius / (Math.Max(mid.X, mid.Y));

                    newX = mid.X + (newRadius * Math.Cos(theta));

                    if (newX > 0 && newX < nWidth)
                    {
                        fp[x, y].X = newX;
                        pt[x, y].X = (int)newX;
                    }
                    else
                    {
                        fp[x, y].X = fp[x, y].Y = 0.0;
                        pt[x, y].X = pt[x, y].Y = 0;
                    }

                    newY = mid.Y + (newRadius * Math.Sin(theta));

                    if (newY > 0 && newY < nHeight && newX > 0 && newX < nWidth)
                    {
                        fp[x, y].Y = newY;
                        pt[x, y].Y = (int)newY;
                    }
                    else
                    {
                        fp[x, y].X = fp[x, y].Y = 0.0;
                        pt[x, y].X = pt[x, y].Y = 0;
                    }
                }

            if (bSmoothing)
                OffsetFilterAbs(b, pt);
            else
                OffsetFilterAntiAlias(b, fp);

            return true;
        }

        public static bool Water(Bitmap b, short nWave, bool bSmoothing)
        {
            int nWidth = b.Width;
            int nHeight = b.Height;

            FloatPoint[,] fp = new FloatPoint[nWidth, nHeight];
            Point[,] pt = new Point[nWidth, nHeight];

            Point mid = new Point();
            mid.X = nWidth / 2;
            mid.Y = nHeight / 2;

            double newX, newY;
            double xo, yo;

            for (int x = 0; x < nWidth; ++x)
                for (int y = 0; y < nHeight; ++y)
                {
                    xo = ((double)nWave * Math.Sin(2.0 * 3.1415 * (float)y / 128.0));
                    yo = ((double)nWave * Math.Cos(2.0 * 3.1415 * (float)x / 128.0));

                    newX = (x + xo);
                    newY = (y + yo);

                    if (newX > 0 && newX < nWidth)
                    {
                        fp[x, y].X = newX;
                        pt[x, y].X = (int)newX;
                    }
                    else
                    {
                        fp[x, y].X = 0.0;
                        pt[x, y].X = 0;
                    }


                    if (newY > 0 && newY < nHeight)
                    {
                        fp[x, y].Y = newY;
                        pt[x, y].Y = (int)newY;
                    }
                    else
                    {
                        fp[x, y].Y = 0.0;
                        pt[x, y].Y = 0;
                    }
                }

            if (bSmoothing)
                OffsetFilterAbs(b, pt);
            else
                OffsetFilterAntiAlias(b, fp);
            return true;
        }

        public static bool Convert3x3(Bitmap b, Matrix m)
        {
            // Avoid divide by zero errors
            if (0 == m.Factor) return false;

            Bitmap bSrc = (Bitmap)b.Clone();

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            int stride2 = stride * 2;
            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr SrcScan0 = bmSrc.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* pSrc = (byte*)(void*)SrcScan0;

                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width - 2;
                int nHeight = b.Height - 2;

                int nPixel;

                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        nPixel = ((((pSrc[2] * m.TopLeft) + (pSrc[5] * m.TopMid) + (pSrc[8] * m.TopRight) +
                            (pSrc[2 + stride] * m.MidLeft) + (pSrc[5 + stride] * m.Pixel) + (pSrc[8 + stride] * m.MidRight) +
                            (pSrc[2 + stride2] * m.BottomLeft) + (pSrc[5 + stride2] * m.BottomMid) + (pSrc[8 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[5 + stride] = (byte)nPixel;

                        nPixel = ((((pSrc[1] * m.TopLeft) + (pSrc[4] * m.TopMid) + (pSrc[7] * m.TopRight) +
                            (pSrc[1 + stride] * m.MidLeft) + (pSrc[4 + stride] * m.Pixel) + (pSrc[7 + stride] * m.MidRight) +
                            (pSrc[1 + stride2] * m.BottomLeft) + (pSrc[4 + stride2] * m.BottomMid) + (pSrc[7 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[4 + stride] = (byte)nPixel;

                        nPixel = ((((pSrc[0] * m.TopLeft) + (pSrc[3] * m.TopMid) + (pSrc[6] * m.TopRight) +
                            (pSrc[0 + stride] * m.MidLeft) + (pSrc[3 + stride] * m.Pixel) + (pSrc[6 + stride] * m.MidRight) +
                            (pSrc[0 + stride2] * m.BottomLeft) + (pSrc[3 + stride2] * m.BottomMid) + (pSrc[6 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[3 + stride] = (byte)nPixel;

                        p += 3;
                        pSrc += 3;
                    }
                    p += nOffset;
                    pSrc += nOffset;
                }
            }

            b.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);

            return true;
        }

        public static bool OffsetFilterAbs(Bitmap b, Point[,] offset)
        {
            Bitmap bSrc = (Bitmap)b.Clone();

            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int scanline = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr SrcScan0 = bmSrc.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* pSrc = (byte*)(void*)SrcScan0;

                int nOffset = bmData.Stride - b.Width * 3;
                int nWidth = b.Width;
                int nHeight = b.Height;

                int xOffset, yOffset;

                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        xOffset = offset[x, y].X;
                        yOffset = offset[x, y].Y;
                        if (yOffset >= 0 && yOffset < nHeight && xOffset >= 0 && xOffset < nWidth)
                        {
                            p[0] = pSrc[(yOffset * scanline) + (xOffset * 3)];
                            p[1] = pSrc[(yOffset * scanline) + (xOffset * 3) + 1];
                            p[2] = pSrc[(yOffset * scanline) + (xOffset * 3) + 2];
                        }
                        p += 3;
                    }
                    p += nOffset;
                }
            }
            b.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);
            return true;
        }

        public static bool OffsetFilterAntiAlias(Bitmap b, FloatPoint[,] fp)
        {
            Bitmap bSrc = (Bitmap)b.Clone();

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int scanline = bmData.Stride;

            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr SrcScan0 = bmSrc.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* pSrc = (byte*)(void*)SrcScan0;

                int nOffset = bmData.Stride - b.Width * 3;
                int nWidth = b.Width;
                int nHeight = b.Height;

                double xOffset, yOffset;

                double fraction_x, fraction_y, one_minus_x, one_minus_y;
                int ceil_x, ceil_y, floor_x, floor_y;
                Byte p1, p2;

                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        xOffset = fp[x, y].X;
                        yOffset = fp[x, y].Y;

                        // Setup

                        floor_x = (int)Math.Floor(xOffset);
                        floor_y = (int)Math.Floor(yOffset);
                        ceil_x = floor_x + 1;
                        ceil_y = floor_y + 1;
                        fraction_x = xOffset - floor_x;
                        fraction_y = yOffset - floor_y;
                        one_minus_x = 1.0 - fraction_x;
                        one_minus_y = 1.0 - fraction_y;

                        if (floor_y >= 0 && ceil_y < nHeight && floor_x >= 0 && ceil_x < nWidth)
                        {
                            // Blue

                            p1 = (Byte)(one_minus_x * (double)(pSrc[floor_y * scanline + floor_x * 3]) +
                                fraction_x * (double)(pSrc[floor_y * scanline + ceil_x * 3]));

                            p2 = (Byte)(one_minus_x * (double)(pSrc[ceil_y * scanline + floor_x * 3]) +
                                fraction_x * (double)(pSrc[ceil_y * scanline + 3 * ceil_x]));

                            p[x * 3 + y * scanline] = (Byte)(one_minus_y * (double)(p1) + fraction_y * (double)(p2));

                            // Green

                            p1 = (Byte)(one_minus_x * (double)(pSrc[floor_y * scanline + floor_x * 3 + 1]) +
                                fraction_x * (double)(pSrc[floor_y * scanline + ceil_x * 3 + 1]));

                            p2 = (Byte)(one_minus_x * (double)(pSrc[ceil_y * scanline + floor_x * 3 + 1]) +
                                fraction_x * (double)(pSrc[ceil_y * scanline + 3 * ceil_x + 1]));

                            p[x * 3 + y * scanline + 1] = (Byte)(one_minus_y * (double)(p1) + fraction_y * (double)(p2));

                            // Red

                            p1 = (Byte)(one_minus_x * (double)(pSrc[floor_y * scanline + floor_x * 3 + 2]) +
                                fraction_x * (double)(pSrc[floor_y * scanline + ceil_x * 3 + 2]));

                            p2 = (Byte)(one_minus_x * (double)(pSrc[ceil_y * scanline + floor_x * 3 + 2]) +
                                fraction_x * (double)(pSrc[ceil_y * scanline + 3 * ceil_x + 2]));

                            p[x * 3 + y * scanline + 2] = (Byte)(one_minus_y * (double)(p1) + fraction_y * (double)(p2));
                        }
                    }
                }
            }

            b.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);

            return true;
        }
    }
}
