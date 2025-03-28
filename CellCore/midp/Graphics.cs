﻿using System.Drawing.Imaging;
using System.Drawing;
using System;
using Cell;
namespace javax.microedition.lcdui{

/**
 * Graphics 的摘要说明。
 */
public class Graphics
{
	public static System.Drawing.Font font = new System.Drawing.Font(System.Drawing.FontFamily.GenericMonospace, 8);



	public System.Drawing.Graphics dg;

	public System.Drawing.Pen pen = System.Drawing.Pens.Black;

	const  byte  TRANS_NONE 	 = 0;
	const  byte TRANS_90 		 = 1;
	const  byte TRANS_180 		 = 2; 
	const  byte TRANS_270 		 = 3;
	const  byte TRANS_H 		 = 4;
	const  byte TRANS_H90 		 = 5;
	const  byte TRANS_H180 		 = 6;
    const byte TRANS_H270       = 7;


	//    public static Graphics g;
	//public static int BASELINE	;
	//public static int BOTTOM	;
	//public static int DOTTED	;
	//public static int HCENTER	;
	public static int LEFT		;
	//public static int RIGHT		;
	//public static int SOLID		;
	public static int TOP		;
	//public static int VCENTER	;

	private System.Collections.Stack stack_transform = new System.Collections.Stack();
	private System.Collections.Stack stack_alpha = new System.Collections.Stack();
	private System.Drawing.Imaging.ImageAttributes imgAttr = new System.Drawing.Imaging.ImageAttributes();
	private float imgAlpha = -1;

	public Graphics(System.Drawing.Graphics sdg, bool smooth = false)
	{
        dg = sdg;
        dg.CompositingMode = (System.Drawing.Drawing2D.CompositingMode.SourceOver);
        dg.PageUnit = GraphicsUnit.Pixel;
        if (smooth)
        {
            dg.CompositingQuality = (System.Drawing.Drawing2D.CompositingQuality.HighQuality);
            dg.SmoothingMode = (System.Drawing.Drawing2D.SmoothingMode.HighQuality);
            dg.InterpolationMode = (System.Drawing.Drawing2D.InterpolationMode.High);
            dg.PixelOffsetMode = (System.Drawing.Drawing2D.PixelOffsetMode.HighQuality);
        }
        else
        {
            dg.CompositingQuality = (System.Drawing.Drawing2D.CompositingQuality.HighSpeed);
            dg.SmoothingMode = (System.Drawing.Drawing2D.SmoothingMode.None);
            dg.InterpolationMode = (System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor);
            dg.PixelOffsetMode = (System.Drawing.Drawing2D.PixelOffsetMode.Half);
        }

        pen = new System.Drawing.Pen(Color.Black);

		setAlpha(1);

	}

	//------------------------------------------------------------------------------------------------------------------

	public void multiplyAlpha(float alpha)
	{
		setAlpha(this.imgAlpha*alpha);
	}

	private void setAlpha(float alpha)
	{
		this.imgAlpha = alpha;
		// Initialize the color matrix.
		// Note the value 0.8 in row 4, column 4.
		float[][] matrixItems ={ 
				   new float[] {1, 0, 0, 0, 0},
				   new float[] {0, 1, 0, 0, 0},
				   new float[] {0, 0, 1, 0, 0},
				   new float[] {0, 0, 0, alpha, 0}, 
				   new float[] {0, 0, 0, 0, 1}};
		ColorMatrix colorMatrix = new ColorMatrix(matrixItems);

		// Create an ImageAttributes object and set its color matrix.
		ImageAttributes imageAtt = new ImageAttributes();
		imageAtt.SetColorMatrix(
		   colorMatrix,
		   ColorMatrixFlag.Default,
		   ColorAdjustType.Bitmap);

		this.imgAttr = imageAtt;
	}



	public void translate(float x, float y)
	{
		dg.TranslateTransform(x, y);
	}
	public void rotate(float angle)
	{
		dg.RotateTransform(angle);
	}

	public void scale(float x, float y)
	{
		dg.ScaleTransform(x, y);
	}

	public void shear(float x, float y)
	{
		System.Drawing.Drawing2D.Matrix mx = new System.Drawing.Drawing2D.Matrix();
		mx.Shear(x, y);
		dg.MultiplyTransform(mx);
	}

	public void pushState()
	{
		stack_transform.Push(this.dg.Transform);
		stack_alpha.Push(this.imgAlpha);
		stack_alpha.Push(this.imgAttr);
	}

	public void popState()
	{
		this.imgAttr = (System.Drawing.Imaging.ImageAttributes)stack_alpha.Pop();
		this.imgAlpha = (float)stack_alpha.Pop();
		this.dg.Transform = (System.Drawing.Drawing2D.Matrix)stack_transform.Pop();

	}

	//------------------------------------------------------------------------------------------------------------------

	private void _drawImage(System.Drawing.Image dimg,
		float dx, float dy, float dw, float dh,
		float sx, float sy, float sw, float sh,
		System.Drawing.RotateFlipType transform)
	{
		// Create parallelogram for drawing original image.
		PointF ulCorner1 = new PointF(0, 0);
		PointF urCorner1 = new PointF(dw, 0);
		PointF llCorner1 = new PointF(0, dh);
		PointF[] destPara1 = { ulCorner1, urCorner1, llCorner1 };

		// Create rectangle for source image.
		RectangleF srcRect = new RectangleF(sx, sy, sw, sh);

		System.Drawing.Drawing2D.Matrix mtx = dg.Transform;
		try
		{
			dg.TranslateTransform(dx, dy);
			switch (transform)
			{
				case System.Drawing.RotateFlipType.RotateNoneFlipX://4
					dg.TranslateTransform(sw, 0);
					dg.ScaleTransform(-1, 1);
					break;
				case System.Drawing.RotateFlipType.RotateNoneFlipY://6
					dg.TranslateTransform(0, sh);
					dg.ScaleTransform(1, -1);
					break;
				//case TRANS_180:
				case System.Drawing.RotateFlipType.RotateNoneFlipXY://2
					dg.TranslateTransform(sw, sh);
					dg.ScaleTransform(-1, -1);
					break;
				case System.Drawing.RotateFlipType.Rotate90FlipNone://1
					dg.TranslateTransform(sh, 0);
					dg.RotateTransform(90);
					break;
				case System.Drawing.RotateFlipType.Rotate270FlipNone://3
					dg.TranslateTransform(0, sw);
					dg.RotateTransform(270);
					break;
				case System.Drawing.RotateFlipType.Rotate90FlipX://5
					dg.TranslateTransform(sh, 0);
					dg.RotateTransform(90);
					dg.TranslateTransform(sw, 0);
					dg.ScaleTransform(-1, 1);
					break;
				case System.Drawing.RotateFlipType.Rotate270FlipX://7
					dg.TranslateTransform(0, sw);
					dg.RotateTransform(270);
					dg.TranslateTransform(sw, 0);
					dg.ScaleTransform(-1, 1);
					break;
			}
            dg.DrawImage(dimg, destPara1, srcRect, System.Drawing.GraphicsUnit.Pixel, imgAttr);
		}
		finally
		{
			dg.Transform = mtx;
		}

		
	}

	public void drawImage(javax.microedition.lcdui.Image img, float x, float y)
	{
		if (img.killed) return;
		_drawImage(
			img.dimg,
			x, y, img.getWidth(), img.getHeight(),
			0, 0, img.getWidth() , img.getHeight(),
			0);
	}
	


	public void drawImageScale(
		javax.microedition.lcdui.Image img,
		float x, float y,
		System.Drawing.RotateFlipType transform,
		float scale)
	{
		if (img.killed) return;

		if (scale == 1) 
		{
			_drawImage(img.dimg,
					x , y ,
                    img.getWidth() ,
                    img.getHeight() ,
					0, 0, 
                    img.getWidth(),
                    img.getHeight(),
					transform);
		}
        else if (img.IsCacheScale(scale))
        {
            _drawImage(
                img._cacheImg,
                    x, y, 
                    img._cacheImg.Width,
                    img._cacheImg.Height,
                    0, 0,
                    img._cacheImg.Width,
                    img._cacheImg.Height,
                    transform);
        }
        else
		{
			_drawImage(img.dimg,
					x , y , 
                    img.getWidth() * scale, 
                    img.getHeight() * scale,
					0, 0,
                    img.getWidth(), 
                    img.getHeight(), 
					transform);
		}
	}

	public void drawImageRegion(
		javax.microedition.lcdui.Image img,
		float x, float y, float sx, float sy, float sw, float sh,
		System.Drawing.RotateFlipType transform)
	{
		if (img.killed) return;

		_drawImage(img.dimg,
			x, y, sw, sh,
			sx, sy, sw, sh,
			transform);
	}
	
	
	public void drawImageTrans(javax.microedition.lcdui.Image src, float x, float y, int transform)
    {
		drawImageRegion(src, x, y, 0, 0, src.getWidth(), src.getHeight(), FlipTable[transform]);
    }

    public void drawImageTrans(javax.microedition.lcdui.Image src, float x, float y, ImageTrans transform)
    {
        switch (transform)
        {
            case ImageTrans.NONE:
                drawImageRegion(src, x, y, 0, 0, src.getWidth(), src.getHeight(),
                    System.Drawing.RotateFlipType.RotateNoneFlipNone);
                break;
            case ImageTrans.R_90:
                drawImageRegion(src, x, y, 0, 0, src.getWidth(), src.getHeight(), 
                    System.Drawing.RotateFlipType.Rotate90FlipNone);
                break;
            case ImageTrans.R_180:
                drawImageRegion(src, x, y, 0, 0, src.getWidth(), src.getHeight(),
                    System.Drawing.RotateFlipType.Rotate180FlipNone);
                break;
            case ImageTrans.R_270:
                drawImageRegion(src, x, y, 0, 0, src.getWidth(), src.getHeight(),
                    System.Drawing.RotateFlipType.Rotate270FlipNone);
                break;
            case ImageTrans.MIRROR:
                drawImageRegion(src, x, y, 0, 0, src.getWidth(), src.getHeight(),
                    System.Drawing.RotateFlipType.RotateNoneFlipX);
                break;
            case ImageTrans.MR_90:
                drawImageRegion(src, x, y, 0, 0, src.getWidth(), src.getHeight(),
                    System.Drawing.RotateFlipType.Rotate90FlipX);
                break;
            case ImageTrans.MR_180:
                drawImageRegion(src, x, y, 0, 0, src.getWidth(), src.getHeight(),
                    System.Drawing.RotateFlipType.Rotate180FlipX);
                break;
            case ImageTrans.MR_270:
                drawImageRegion(src, x, y, 0, 0, src.getWidth(), src.getHeight(),
                    System.Drawing.RotateFlipType.Rotate270FlipX);
                break;
        }
        
    }

    public void drawStringBorder(string str, float x, float y, uint color, uint bcolor)
    {
        setColor(bcolor);
        dg.DrawString(str, font, pen.Brush, x - 1, y - 1);
        dg.DrawString(str, font, pen.Brush, x    , y - 1);
        dg.DrawString(str, font, pen.Brush, x + 1, y - 1);

        dg.DrawString(str, font, pen.Brush, x - 1, y);
        dg.DrawString(str, font, pen.Brush, x + 1, y);

        dg.DrawString(str, font, pen.Brush, x - 1, y + 1);
        dg.DrawString(str, font, pen.Brush, x    , y + 1);
        dg.DrawString(str, font, pen.Brush, x + 1, y + 1);

        setColor(color);
        dg.DrawString(str, font, pen.Brush, x, y);
    }

	public void drawString(string str, float x, float y, int anchor)
	{
		
		dg.DrawString(str, font, pen.Brush, x , y );
	}

	public void drawLine(float x1, float y1, float x2, float y2)
	{
		dg.DrawLine(pen, x1, y1, x2, y2);
	}

	public void drawArc(int x, int y, int width, int height, int startAngle, int arcAngle)
	{
		dg.DrawArc(pen, x, y, width, height, startAngle, arcAngle);
	}

	public void drawRect(float x, float y, float width, float height)
	{
		dg.DrawRectangle(pen, x, y, width, height);
	}
	public void fillArc(float x, float y, float width, float height, float startAngle, float arcAngle)
	{
        dg.FillPie(pen.Brush, x, y, width, height, startAngle, arcAngle);
	}
	public void fillRect(float x, float y, float width, float height)
	{
        dg.FillRectangle(pen.Brush, x, y, width, height);
	}
	// float


	public void drawArc(float x, float y, float width, float height, float startAngle, float arcAngle)
	{
		dg.DrawArc(pen, x, y, width, height, startAngle, arcAngle);
	}

	
	
	public int getClipHeight()
	{
		return (int)dg.ClipBounds.Height;
	}
	public int getClipWidth()
	{
		return (int)dg.ClipBounds.Width;
	}
	public int getClipX()
	{
		return (int)dg.ClipBounds.X;
	}
	public int getClipY()
	{
		return (int)dg.ClipBounds.Y;
	}

	public void setClip(float x, float y, float width, float height)
	{
		dg.SetClip(new System.Drawing.RectangleF(x , y , width , height ));
	}
	public void setColor(int RGB)
	{
        pen.Color = System.Drawing.Color.FromArgb(RGB);
    }
    public void setColor(uint ARGB)
    {
        pen.Color = System.Drawing.Color.FromArgb((int)ARGB);
    }

	public void setColor(int red, int green, int blue)
	{
        pen.Color = System.Drawing.Color.FromArgb(red, green, blue);
	}
	public void setColor(int alpha,int red, int green, int blue)
	{
        pen.Color = System.Drawing.Color.FromArgb(alpha,red, green, blue);
	}
	public void setColor(int alpha, int rgb)
	{
		int r = (rgb & 0xff0000) >> 16;
		int g = (rgb & 0x00ff00) >> 8;
		int b = (rgb & 0x0000ff) >> 0;
	
        pen.Color = System.Drawing.Color.FromArgb(alpha, r, g, b);
	}

	public void setDColor(System.Drawing.Color c)  {
        pen.Color = c;
	}
	public void setDColor(int alpha, System.Drawing.Color c)
	{
        pen.Color = System.Drawing.Color.FromArgb(alpha, c);
	}


    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    //
    /////////////////////////////////////////////////////////////////////////////////////////////////////////

	public System.Drawing.Font getFont()
	{
		return font;
	}

    static public System.Drawing.RotateFlipType[] FlipTable = new System.Drawing.RotateFlipType[]
        {
            System.Drawing.RotateFlipType.RotateNoneFlipNone,//
            System.Drawing.RotateFlipType.Rotate90FlipNone,//
            System.Drawing.RotateFlipType.Rotate180FlipNone,
            System.Drawing.RotateFlipType.Rotate270FlipNone,//

            System.Drawing.RotateFlipType.RotateNoneFlipX,
            System.Drawing.RotateFlipType.Rotate270FlipX,//
            System.Drawing.RotateFlipType.Rotate180FlipX,
            System.Drawing.RotateFlipType.Rotate90FlipX,//
        };
     
    static public string[] FlipTextTable = new string[]
        {
            "无",
            "90",
            "180",
            "270",
            "水平",
            "H 90",
            "H 180",
            "H 270",
        };

}
}