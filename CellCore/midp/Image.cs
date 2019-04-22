using DeepCore.IO;
using DeepEditor.Common;
using System;
using System.Drawing.Imaging;

namespace javax.microedition.lcdui
{

    public class Image
    {

        private System.Drawing.Bitmap _dimg;

        public System.Drawing.Bitmap dimg
        {
            get { return _dimg; }
        }
        public int Width
        {
            get
            {
                if (_dimg != null) { return _dimg.Width; }
                return 0;
            }
        }
        public int Height
        {
            get
            {
                if (_dimg != null) { return _dimg.Height; }
                return 0;
            }
        }

        public bool used = false;

        public bool selected = false;
        public int x = 0;
        public int y = 0;

        public int indexOfImages;
        public bool killed = false;

        private bool hasColorKey = false;
        private System.Drawing.Color ColorKey;

        System.Drawing.Brush brush = null;

        internal System.Drawing.Image _cacheImg;
        private float _cacheScaleF = 1.0f;

        public bool Touched = false;

        public Image(System.Drawing.Image src)
        {
            _dimg = ImageUtils.AsBitmap(src);
        }
        public Image(byte[] rawdata)
        {
            if (Cell.IO.ImageIO.IsPngHead(rawdata))
            {
                using (var ms = new System.IO.MemoryStream(rawdata))
                {
                    System.Drawing.Image src = System.Drawing.Image.FromStream(ms);
                    _dimg = ImageUtils.AsBitmap(src);
                }
            }
            else
            {
                using (var ms = new System.IO.MemoryStream(rawdata, 4, rawdata.Length - 4))
                using (var zip = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Decompress, false))
                {
                    var input = new InputStream(zip, null);
                    int w = input.GetS32();
                    int h = input.GetS32();
                    _dimg = new System.Drawing.Bitmap(w, h, PixelFormat.Format32bppArgb);
                    for (int x = 0; x < w; x++)
                    {
                        for (int y = 0; y < h; y++)
                        {
                            int argb = input.GetS32();
                            _dimg.SetPixel(x, y, System.Drawing.Color.FromArgb(argb));
                        }
                    }
                }
            }
        }
        public byte[] ToRawData()
        {
            using (var ms = new System.IO.MemoryStream())
            {
                byte[] head = new byte[] { 1, 2, 3, 4 };
                ms.Write(head, 0, 4);
                ms.Flush();
                using (var zip = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionLevel.Fastest, false))
                {
                    var output = new OutputStream(zip, null);
                    output.PutS32(dimg.Width);
                    output.PutS32(dimg.Height);
                    for (int x = 0; x < dimg.Width; x++)
                    {
                        for (int y = 0; y < dimg.Height; y++)
                        {
                            int argb = _dimg.GetPixel(x, y).ToArgb();
                            output.PutS32(argb);
                        }
                    }
                    zip.Flush();
                }
                return ms.ToArray();
                //                 dimg.Save(ms, ImageFormat.Png);
                //                 ms.Position = 0;
                //                 byte[] data = ms.ToArray();
                //                 ms.Close();
                //                 return data;
            }
        }

        public static javax.microedition.lcdui.Image createImage(int width, int height)
        {
            System.Drawing.Image image = new System.Drawing.Bitmap(width, height,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Image ret = new Image(image);
            return ret;
        }

        public static javax.microedition.lcdui.Image createImage(string name)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(name);
            Image ret = new Image(image);
            return ret;
        }

        public static javax.microedition.lcdui.Image createRGBImage(int[] rgb, int width, int height, bool processAlpha)
        {
            System.Drawing.Image image = new System.Drawing.Bitmap(width, height);

            Image ret = new Image(image);

            return ret;
        }

        public static javax.microedition.lcdui.Image createImage(javax.microedition.lcdui.Image image,
            int x, int y, int width, int height)
        {

            Image ret = Image.createImage(width, height);

            ret.getGraphics().drawImageRegion(image, 0, 0, x, y, width, height, 0);


            return ret;

        }


        //-----------------------------------------------------------------------------------------------------------------------------------
        public void changeDimg(System.Drawing.Image image)
        {
            _dimg = ImageUtils.AsBitmap(image);
        }

        public Image subImage(int x, int y, int w, int h)
        {
            if (x >= 0 && (x + w) <= getWidth() && y >= 0 && (y + h) <= getHeight())
            {
                Image buff = createImage(w, h);
                Graphics bg = buff.getGraphics();
                bg.drawImage(this, -x, -y);
                return buff;
            }
            return null;
        }

        public bool IsCacheScale(float scale)
        {
            return _cacheScaleF == scale && _cacheImg != null;
        }

        public void CacheScale(float scale)
        {
            if (scale > 0 && scale < 1)
            {
                if (_cacheScaleF != scale || _cacheImg == null)
                {
                    _cacheScaleF = scale;
                    if (_dimg != null && !killed)
                    {
                        int dw = (int)(getWidth() * scale);
                        int dh = (int)(getHeight() * scale);
                        _cacheImg = null;
                        Image buff = createImage(dw, dh);
                        Graphics bg = buff.getGraphics();
                        bg.scale(scale, scale);
                        bg.drawImage(this, 0, 0);
                        _cacheImg = buff.dimg;
                    }
                }
            }
            else
            {
                _cacheScaleF = 1.0f;
                _cacheImg = null;
            }
        }



        public void swapColor(int src_argb, int dstcolor)
        {
            System.Drawing.Bitmap image = dimg;
            System.Drawing.Color zeroc = System.Drawing.Color.FromArgb(dstcolor);

            for (int x = image.Width - 1; x >= 0; --x)
            {
                for (int y = image.Height - 1; y >= 0; --y)
                {
                    System.Drawing.Color c = image.GetPixel(x, y);

                    if (c.ToArgb() == src_argb)
                    {
                        image.SetPixel(x, y, zeroc);
                    }
                }
            }

            _dimg = image;
        }

        public void flipSelf(int transform)
        {
            int width = getWidth();
            int height = getHeight();

            switch (Graphics.FlipTable[transform])
            {
                case System.Drawing.RotateFlipType.Rotate90FlipNone://1
                case System.Drawing.RotateFlipType.Rotate270FlipNone://3
                case System.Drawing.RotateFlipType.Rotate90FlipX://5
                case System.Drawing.RotateFlipType.Rotate270FlipX://7
                    width = getHeight();
                    height = getWidth();
                    break;
            }

            Image dst = createImage(width, height);
            Graphics g = dst.getGraphics();
            g.drawImageTrans(this, 0, 0, transform);
            _dimg = dst.dimg;
        }

        public System.Drawing.Rectangle cutTransparentImageSize(int broadPixel)
        {
            System.Drawing.Bitmap image = dimg;

            System.Drawing.Rectangle ret = new System.Drawing.Rectangle();

            int left = 0;
            int right = image.Width - 1;
            int top = 0;
            int bottom = image.Height - 1;
            int x = 0;
            int y = 0;
            System.Drawing.Color c;

            #region findTBLR
            bool finded = false;
            // find left
            for (x = 0; x < image.Width; x++)
            {
                for (y = image.Height - 1; y >= 0; --y)
                {
                    c = image.GetPixel(x, y);
                    if (c.A != 0)
                    {
                        left = x;
                        finded = true;
                        break;
                    }
                }
                if (finded)
                {
                    break;
                }
            }

            finded = false;
            // right
            for (x = image.Width - 1; x >= 0; --x)
            {
                for (y = image.Height - 1; y >= 0; --y)
                {
                    c = image.GetPixel(x, y);
                    if (c.A != 0)
                    {
                        right = x;
                        finded = true;
                        break;
                    }
                }
                if (finded)
                {
                    break;
                }
            }

            finded = false;
            // top
            for (y = 0; y < image.Height; y++)
            {
                for (x = image.Width - 1; x >= 0; --x)
                {
                    c = image.GetPixel(x, y);
                    if (c.A != 0)
                    {
                        top = y;
                        finded = true;
                        break;
                    }
                }
                if (finded)
                {
                    break;
                }
            }

            finded = false;
            // bottom
            for (y = image.Height - 1; y >= 0; --y)
            {
                for (x = image.Width - 1; x >= 0; --x)
                {
                    c = image.GetPixel(x, y);
                    if (c.A != 0)
                    {
                        bottom = y;
                        finded = true;
                        break;
                    }
                }
                if (finded)
                {
                    break;
                }
            }
            #endregion

            left = Math.Max(0, left - broadPixel);
            right = Math.Min(image.Width - 1, right + broadPixel);
            top = Math.Max(0, top - broadPixel);
            bottom = Math.Min(image.Height - 1, bottom + broadPixel);

            ret.X = left;
            ret.Y = top;
            ret.Width = right - left + 1;
            ret.Height = bottom - top + 1;
            Image dst = createImage(ret.Width, ret.Height);
            Graphics dstg = dst.getGraphics();
            dstg.drawImage(this, -left, -top);
            _dimg = dst.dimg;
            dst = null;

            return ret;
        }


        public System.Drawing.Color getPixel(int x, int y)
        {
            return dimg.GetPixel(x, y);
        }


        //-----------------------------------------------------------------------------------------------------------------------------------

        public Graphics getGraphics()
        {
            Graphics g = new Graphics(System.Drawing.Graphics.FromImage(dimg));

            return g;
        }



        public int getHeight()
        {
            return dimg.Height;
        }
        public int getWidth()
        {
            return dimg.Width;
        }


        public System.Drawing.Color getColorKey()
        {
            if (!hasColorKey)
            {
                hasColorKey = true;
                try
                {
                    System.Drawing.Bitmap bm = new System.Drawing.Bitmap(1, 1);
                    System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bm);

                    g.DrawImage(dimg,
                        new System.Drawing.Rectangle(0, 0, 1, 1),
                        new System.Drawing.Rectangle(dimg.Width / 2, dimg.Height / 2, 1, 1),
                        System.Drawing.GraphicsUnit.Pixel
                        );

                    ColorKey = bm.GetPixel(0, 0);

                    g = null;
                    bm = null;
                }
                catch (Exception err) { }
            }

            return ColorKey;
        }


        public System.Drawing.Brush getColorKeyBrush()
        {
            if (brush == null)
            {
                brush = (new System.Drawing.Pen(getColorKey())).Brush;
            }
            return brush;
        }

        public System.Drawing.Bitmap Save(string path, bool EnablePremultiplyAlpha, System.Drawing.Imaging.ImageFormat format)
        {
            if (EnablePremultiplyAlpha)
            {
                var image = ImageUtils.PremultiplyAlpha(dimg);
                image.Save(path, format);
                return image;
            }
            else
            {
                dimg.Save(path, format);
            }
            return dimg;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------

        //         public System.Drawing.Bitmap getDImage()
        //         {
        //             return dimg;
        //         }
        


    }

}