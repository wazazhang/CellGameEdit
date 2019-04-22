using Cell.IO;
using DeepEditor.Common;
using DeepEditor.Common.G2D;
using javax.microedition.lcdui;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CellGameEdit.PM
{
    [Serializable]
    public partial class ImagesForm : Form, ISerializable, IEditForm
    {
        //public static System.Drawing.Color ColorKey = System.Drawing.Color.FromArgb(0xff,0xff,0xff,0);
        //public static System.Drawing.Color ColorTileID = System.Drawing.Color.FromArgb(0xff, 0, 0xff, 0);

        public string id { get; private set; }
        public void setID(string id, ProjectForm proj)
        {
            this.id = id;
        }

        private int TotalSplit = 0;
        private bool IsAnyImageTouched = false;
        int CellW = 16;
        int CellH = 16;
        public List<javax.microedition.lcdui.Image> dstImages { get; private set; }
        public List<string> dstDataKeys { get; private set; }

        //[NoneSerializable]
        javax.microedition.lcdui.Image srcImage;
        System.Drawing.Rectangle srcRect;
        Boolean srcRectIsDown = false;
        int srcPX;
        int srcPY;
        int srcQX;
        int srcQY;
        int srcSize = 1;

        System.Drawing.Color backColor = System.Drawing.Color.Magenta;
        System.Drawing.Color keyColor = System.Drawing.Color.MediumBlue;

        private Boolean is_change_image = false;

        private string append_data = "";

        private string image_convert_script_file = "";
        private string[] image_convert_script = null;

        public ImagesForm(String name)
        {
            InitializeComponent();

            id = name; this.Text = id;
            CellW = 16;
            CellH = 16;
            dstImages = new List<Image>();
            dstDataKeys = new List<string>();
            toolStripTextBox1.Text = CellW.ToString();
            toolStripTextBox2.Text = CellH.ToString();

            // src
            pictureBox1.Width = 1;
            pictureBox1.Height = 1;
            pictureBox1.Image = new System.Drawing.Bitmap(1, 1);
            srcImage = Image.createImage(pictureBox1.Width, pictureBox1.Height);


            srcRectIsDown = false;
            srcRect = new System.Drawing.Rectangle(0, 0, 1, 1);


            //
            pictureBox2.Width = 1;
            pictureBox2.Height = 1;

            dstSelected = null;

            dstRect = new System.Drawing.Rectangle(0, 0, 1, 1);

            is_change_image = true;

            pictureBox1.BackColor = toolStripButton10.BackColor;
            pictureBox2.BackColor = toolStripButton10.BackColor;

        }
        public void LoadOver(ProjectForm prj)
        {
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected ImagesForm(SerializationInfo info, StreamingContext context)
        {
            is_change_image = false;
            dstImages = new List<Image>();
            dstDataKeys = new List<string>();
            try
            {
                InitializeComponent();
                pictureBox2.Width = 1;
                pictureBox2.Height = 1;
                // load start
                id = (String)info.GetValue("id", typeof(String));
                this.Text = id;
                CellW = (int)info.GetValue("CellW", typeof(int));
                CellH = (int)info.GetValue("CellH", typeof(int));
                ArrayList output = (ArrayList)info.GetValue("output", typeof(ArrayList));
                ArrayList outX = (ArrayList)info.GetValue("outX", typeof(ArrayList));
                ArrayList outY = (ArrayList)info.GetValue("outY", typeof(ArrayList));
                ArrayList outK;
                try
                {
                    TotalSplit = (int)info.GetValue("TotalSplit", typeof(int));
                    TotalSplit = Util.ccNextPOT(TotalSplit);
                }
                catch { }
                try
                {
                    btn_Pow2.Checked = (bool)info.GetValue("btn_Pow2", typeof(bool));
                    btn_Quard2.Checked = (bool)info.GetValue("btn_Quard2", typeof(bool));
                }
                catch { }
                try
                {
                    this.append_data = info.GetString("AppendData");
                }
                catch
                {
                    this.append_data = "";
                }
                try
                {
                    chk_custom_output.Checked = (Boolean)info.GetValue("custom_output", typeof(Boolean));
                    chk_output_group.Checked =
                        (Boolean)info.GetValue("output_group", typeof(Boolean));
                    chk_output_tile.Checked =
                        (Boolean)info.GetValue("output_tile", typeof(Boolean));
                }
                catch { }
                finally
                {
                    chk_output_group.Enabled = !chk_custom_output.Checked;
                    chk_output_tile.Enabled = !chk_custom_output.Checked;
                }

                try
                {
                    chk_custom_filetype.Checked = (Boolean)info.GetValue("custom_filetype", typeof(Boolean));
                    chk_output_file_bmp.Checked =
                        (Boolean)info.GetValue("output_file_bmp", typeof(Boolean));
                    chk_output_file_jpg.Checked =
                        (Boolean)info.GetValue("output_file_jpg", typeof(Boolean));
                    chk_output_file_png.Checked =
                        (Boolean)info.GetValue("output_file_png", typeof(Boolean));
                }
                catch (Exception err) { }
                finally
                {
                    chk_output_file_bmp.Enabled = !chk_custom_filetype.Checked;
                    chk_output_file_jpg.Enabled = !chk_custom_filetype.Checked;
                    chk_output_file_png.Enabled = !chk_custom_filetype.Checked;
                }

                try
                {
                    outK = (ArrayList)info.GetValue("outK", typeof(ArrayList));
                }
                catch
                {
                    outK = new ArrayList();
                    for (int i = 0; i < output.Count; i++)
                    {
                        outK.Add(false);
                    }
                }

                try
                {
                    ArrayList data = (ArrayList)info.GetValue("dstDataKeys", typeof(ArrayList));
                    foreach (var k in data)
                    {
                        dstDataKeys.Add(k + "");
                    }
                }
                catch
                {
                    for (int i = 0; i < output.Count; i++)
                    {
                        dstDataKeys.Add("");
                    }
                }

                image_convert_script_file = (string)Util.GetValue(info, "image_convert_script", typeof(string), "");
                if (image_convert_script_file != null && image_convert_script_file.Length > 0)
                {
                    comboImageConvert.Text = image_convert_script_file;
                }


                try
                {
                    toolStripButton10.BackColor =
                        (System.Drawing.Color)info.GetValue("BackColor", typeof(System.Drawing.Color));
                    BtnSelectKeyColor.BackColor =
                        (System.Drawing.Color)info.GetValue("TileKeyColor", typeof(System.Drawing.Color));
                    BtnSelectTileIDColor.BackColor =
                        (System.Drawing.Color)info.GetValue("TileIDColor", typeof(System.Drawing.Color));
                }
                catch { }
                finally
                {
                    pictureBox1.BackColor = toolStripButton10.BackColor;
                    pictureBox2.BackColor = toolStripButton10.BackColor;
                }




                System.IO.FileStream images_fs = null;
                {
                    String dir1 = ProjectForm.workSpace + "\\tiles\\" + this.id + ".tiles";
                    String dir2 = ProjectForm.workSpace + "\\" + this.id + ".tiles";
                    if (System.IO.File.Exists(dir2))
                    {
                        images_fs = new System.IO.FileStream(dir2, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    }
                    else if (System.IO.File.Exists(dir1))
                    {
                        images_fs = new System.IO.FileStream(dir1, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    }
                }
                using (images_fs)
                {
                    if (ImageIO.IsNewHeader(images_fs))
                    {
                        ImageIO.DecodeNewFromStream(images_fs, (i, data, err) =>
                        {
                            if (err != null)
                            {
                                Console.WriteLine(this.id + " : Tile[" + i + "] : at  " + err.Message);
                            }
                            try
                            {
                                String name = (String)output[i];
                                int x = (int)outX[i];
                                int y = (int)outY[i];
                                Boolean kill = (Boolean)outK[i];
                                if (data != null)
                                {
                                    var image = new Image(data);
                                    image.x = x;
                                    image.y = y;
                                    image.killed = kill;
                                    image.indexOfImages = i;
                                    if (!image.killed)
                                    {
                                        SetDstSize(
                                            Math.Max(pictureBox2.Width, image.x + image.getWidth()),
                                            Math.Max(pictureBox2.Height, image.y + image.getHeight()));
                                    }
                                    dstImages.Add(image);
                                }
                                else
                                {
                                    dstImages.Add(null);
                                }
                            }
                            catch (Exception err2)
                            {
                                dstImages.Add(null);
                                Console.WriteLine(this.id + " : Tile[" + i + "] : at  " + err2.Message);
                            }
                        });
                    }
                    else
                    {
                        is_change_image = true;
                        ArrayList outStreamLen = null;
                        try
                        {
                            outStreamLen = (ArrayList)info.GetValue("outStreamLen", typeof(ArrayList));
                        }
                        catch { }
                        for (int i = 0; i < output.Count; i++)
                        {
                            try
                            {
                                String name = (String)output[i];
                                int x = (int)outX[i];
                                int y = (int)outY[i];
                                Boolean kill = (Boolean)outK[i];
                                int len = -1;
                                if (outStreamLen != null)
                                {
                                    len = (int)outStreamLen[i];
                                }
                                Image img;
                                if (len < 0)
                                {
                                    dstImages.Add(null);
                                }
                                else if (len > 0)
                                {
                                    byte[] data = new byte[len];
                                    images_fs.Read(data, 0, len);
                                    img = new Image(data);
                                    img.x = x;
                                    img.y = y;
                                    img.killed = kill;
                                    img.indexOfImages = i;
                                    if (!img.killed)
                                    {
                                        SetDstSize(
                                            Math.Max(pictureBox2.Width, img.x + img.getWidth()),
                                            Math.Max(pictureBox2.Height, img.y + img.getHeight()));
                                    }
                                    dstImages.Add(img);
                                }
                            }
                            catch (Exception err)
                            {
                                dstImages.Add(null);
                                Console.WriteLine(this.id + " : Tile[" + i + "] : at  " + err.Message);
                            }
                        }
                    }

                }
                // load end

                toolStripTextBox1.Text = CellW.ToString();
                toolStripTextBox2.Text = CellH.ToString();

                // src
                pictureBox1.Width = 1;
                pictureBox1.Height = 1;
                pictureBox1.Image = new System.Drawing.Bitmap(1, 1);
                srcImage = Image.createImage(pictureBox1.Width, pictureBox1.Height);

                srcRectIsDown = false;
                srcRect = new System.Drawing.Rectangle(0, 0, 1, 1);


                //
                //pictureBox2.Width = 1;
                //pictureBox2.Height = 1;

                dstSelected = null;

                dstRect = new System.Drawing.Rectangle(0, 0, 1, 1);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.StackTrace + "  at  " + err.Message);
            }

            refreshClipMenu();
            RefreshDstMaskColor();
        }
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Console.WriteLine("Serializ Images : " + id);
            RefreshDstMaskColor();
            try
            {
                info.AddValue("id", id);
                info.AddValue("CellW", CellW);
                info.AddValue("CellH", CellH);
                info.AddValue("TotalSplit", Util.ccNextPOT(TotalSplit));

                info.AddValue("AppendData", append_data);

                info.AddValue("image_convert_script", image_convert_script_file);

                ArrayList output = new ArrayList();
                ArrayList outX = new ArrayList();
                ArrayList outY = new ArrayList();
                ArrayList outK = new ArrayList();

                //String dir =  "\\tiles\\" + this.id;
                //System.IO.Directory.CreateDirectory(ProjectForm.workSpace + dir);
                String tiles_file_name = ProjectForm.workSpace + "\\" + this.id + ".tiles";
                if (System.IO.File.Exists(tiles_file_name))
                {
                }
                else
                {
                    is_change_image = true;
                }
                if (ProjectForm.isDebug)
                {
                    is_change_image = true;
                }

                // first put all image to stream
                if (is_change_image)
                {
                    using (var fs = new System.IO.FileStream(tiles_file_name, System.IO.FileMode.Create))
                    {
                        ImageIO.EncodeNewToStream(fs, dstImages.Count, (i) =>
                        {
                            try
                            {
                                Image img = getDstImage(i);
                                if (img != null)
                                {
                                    var data = img.ToRawData();
                                    if (ProjectForm.isDebug)
                                    {
                                        try
                                        {
                                            System.IO.Directory.CreateDirectory(ProjectForm.workSpace + "\\" + this.id);
                                            String tid = ProjectForm.workSpace + "\\" + this.id + "\\" + i.ToString() + ".png";
                                            System.IO.File.WriteAllBytes(tid, data);
                                        }
                                        catch { }
                                    }
                                    return data;
                                }
                            }
                            catch (Exception err)
                            {
                                MessageBox.Show(this.id + " : " + i + " : " + err.StackTrace + "  at  " + err.Message);
                            }
                            return null;
                        });
                    }
                    
                }

                // put image property
                for (int i = 0; i < dstImages.Count; i++)
                {
                    try
                    {
                        Image img = getDstImage(i);

                        String name = i.ToString() + ".tile";

                        if (img != null)
                        {
                            output.Add(name);
                            outX.Add(img.x);
                            outY.Add(img.y);
                            outK.Add(img.killed);
                        }
                        else
                        {
                            output.Add("");
                            outX.Add(0);
                            outY.Add(0);
                            outK.Add(false);
                        }
                    }
                    catch (Exception err)
                    {
                        output.Add("");
                        outX.Add(0);
                        outY.Add(0);
                        outK.Add(false);

                        Console.WriteLine(this.id + " : " + i + " : " + err.StackTrace + "  at  " + err.Message);
                    }
                }

                info.AddValue("output", output);
                info.AddValue("outX", outX);
                info.AddValue("outY", outY);
                info.AddValue("outK", outK);
                {
                    info.AddValue("custom_output", chk_custom_output.Checked);
                    info.AddValue("output_tile", chk_output_tile.Checked);
                    info.AddValue("output_group", chk_output_group.Checked);
                }
                {
                    info.AddValue("custom_filetype", chk_custom_filetype.Checked);
                    info.AddValue("output_file_bmp", chk_output_file_bmp.Checked);
                    info.AddValue("output_file_jpg", chk_output_file_jpg.Checked);
                    info.AddValue("output_file_png", chk_output_file_png.Checked);
                }
                //info.AddValue("outStreamLen", outStreamLen);
                {
                    ArrayList list = new ArrayList(dstDataKeys);
                    info.AddValue("dstDataKeys", list);
                }

                info.AddValue("BackColor", toolStripButton10.BackColor);
                info.AddValue("TileKeyColor", BtnSelectKeyColor.BackColor);
                info.AddValue("TileIDColor", BtnSelectTileIDColor.BackColor);

                try
                {
                    info.AddValue("btn_Pow2", btn_Pow2.Checked);
                    info.AddValue("btn_Quard2", btn_Quard2.Checked);
                }
                catch { }

                is_change_image = false;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.StackTrace + "  at  " + err.Message);
            }
            RefreshStatus();
        }

        public void changeImage()
        {
            is_change_image = true;
        }

        public void DelAllImages()
        {
            for (int i = 0; i < dstImages.Count; ++i)
            {
                delDstImage(i);
            }
        }

        private void SetDstSize(int w, int h)
        {
            w = Util.ccNextPOT(w);
            h = Util.ccNextPOT(h);
            pictureBox2.Width = w;
            pictureBox2.Height = h;
        }

        private string outputAllImages(String dir, String type, Boolean tile, Boolean group)
        {
            string ret = "";
            try
            {
                image_convert_script = null;

                if (image_convert_script_file != null && image_convert_script_file.Length > 0)
                {
                    image_convert_script = System.IO.File.ReadAllLines(
                         Application.StartupPath + "\\converter\\" + image_convert_script_file);
                    ret = image_convert_script_file;
                }
                string default_image_convert_script = Form1.getGobalImageConvertScriptFile();
                if (image_convert_script == null &&
                    default_image_convert_script != null && default_image_convert_script.Length > 0)
                {
                    image_convert_script = System.IO.File.ReadAllLines(
                        Application.StartupPath + "\\converter\\" + default_image_convert_script);
                    ret = default_image_convert_script;
                }
            }
            catch (Exception err)
            {
                image_convert_script = null;
                ret = "";
                MessageBox.Show(this.id + " : " + err.Message + "\n" + err.StackTrace);
            }

            try
            {

                //info.
                var format = ImageUtils.GetImageFormat(type);
                if (format == null)
                    return ret;

                if (group)
                {
                    if (TotalSplit > 0)
                    {
                        outputAllAsGropSplit(dir + "\\", type, format);
                    }
                    else
                    {
                        outputAllAsGrop(dir + "\\" + this.id + "." + type, format);
                    }
                }
                if (tile)
                {
                    outputAllTiles(dir + "\\" + this.id + "\\", type, format);
                }
            }
            catch (Exception err) { Console.WriteLine(this.id + " : " + err.StackTrace + "  at  " + err.Message); }

            return ret;
        }

        private void outputAllTiles(string tileDir, string type, System.Drawing.Imaging.ImageFormat format)
        {
            if (!System.IO.Directory.Exists(tileDir))
            {
                System.IO.Directory.CreateDirectory(tileDir);
            }
            for (int i = 0; i < getDstImageCount(); i++)
            {
                if (getDstImage(i) == null || getDstImage(i).killed) continue;
                try
                {
                    string filepath = tileDir + i + "." + type.ToLower();
                    var tile = getDstImage(i);//.getDImage();
                    int outW = tile.Width;
                    int outH = tile.Height;
                    if (btn_Quard2.Checked)
                    {
                        outW = outH = Util.ccNextPOT(Math.Max(outW, outH));
                    }
                    else if (Form1.isOutputImage2M() || btn_Pow2.Checked)
                    {
                        outW = Util.ccNextPOT(outW);
                        outH = Util.ccNextPOT(outH);
                    }
                    if (outW != tile.Width || outH != tile.Height)
                    {
                        Image outputImage = Image.createImage(outW, outH);
                        Graphics g = outputImage.getGraphics();
                        g.setColor(pictureBox2.BackColor.ToArgb());
                        g.fillRect(0, 0, outW, outH);
                        g.drawImage(getDstImage(i), 0, 0);
                        outputImage.Save(filepath, CellGameEdit.Config.Default.EnablePremultiplyAlpha, format);
                    }
                    else
                    {
                        tile.Save(filepath, CellGameEdit.Config.Default.EnablePremultiplyAlpha, format);
                    }
                    runImageConvertScript(image_convert_script, filepath);
                }
                catch (Exception err) { Console.WriteLine(this.id + " : save tile : " + err.StackTrace + "  at  " + err.Message); }
            }
        }

        private void outputAllTilesDirect(string tileDir, string type, System.Drawing.Imaging.ImageFormat format)
        {
            if (!System.IO.Directory.Exists(tileDir))
            {
                System.IO.Directory.CreateDirectory(tileDir);
            }
            for (int i = 0; i < getDstImageCount(); i++)
            {
                if (getDstImage(i) == null || getDstImage(i).killed) continue;
                try
                {
                    string filepath = tileDir + "\\" + i + "." + type.ToLower();
                    getDstImage(i).Save(filepath, CellGameEdit.Config.Default.EnablePremultiplyAlpha, format);
                }
                catch (Exception err)
                {
                    Console.WriteLine(this.id + " : save tile : " + err.StackTrace + "  at  " + err.Message);
                }
            }
        }


        private void outputAllAsGropSplit(string outdir, string type, System.Drawing.Imaging.ImageFormat format)
        {
            try
            {
                System.Drawing.Rectangle rect = getDstBounds();
                int bcx = Util.cycMod(rect.Width, TotalSplit);
                int bcy = Util.cycMod(rect.Height, TotalSplit);
                for (int bx = 0; bx < bcx; bx++)
                {
                    for (int by = 0; by < bcy; by++)
                    {
                        System.Drawing.Rectangle groupBounds = new System.Drawing.Rectangle(
                            bx * TotalSplit, by * TotalSplit, TotalSplit, TotalSplit);
                        Image outputImage = Image.createImage(TotalSplit, TotalSplit);
                        Graphics g = outputImage.getGraphics();
                        bool painted = false;
                        for (int i = 0; i < getDstImageCount(); i++)
                        {
                            Image dimg = getDstImage(i);
                            if (dimg == null || dimg.killed) continue;
                            if (groupBounds.IntersectsWith(new System.Drawing.Rectangle(
                                dimg.x, dimg.y, dimg.getWidth(), dimg.getHeight())))
                            {
                                g.drawImage(dimg, dimg.x - groupBounds.X, dimg.y - groupBounds.Y);
                                painted = true;
                            }
                        }
                        if (painted)
                        {
                            string path = outdir + this.id + string.Format("_{0}_{1}.{2}", bx, by, type);
                            outputImage.Save(path, CellGameEdit.Config.Default.EnablePremultiplyAlpha, format);
                            runImageConvertScript(image_convert_script, path);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(this.id + " : save group : " + err.StackTrace + "  at  " + err.Message);
            }
        }

        private void outputAllAsGrop(string filepath, System.Drawing.Imaging.ImageFormat format)
        {
            try
            {
                int outW = 0;
                int outH = 0;

                for (int i = 0; i < getDstImageCount(); i++)
                {
                    if (getDstImage(i) != null && !getDstImage(i).killed)
                    {
                        outW = Math.Max(outW, getDstImage(i).x + getDstImage(i).getWidth());
                        outH = Math.Max(outH, getDstImage(i).y + getDstImage(i).getHeight());
                    }
                }

                if (btn_Quard2.Checked)
                {
                    outW = outH = Util.ccNextPOT(Math.Max(outW, outH));
                }
                else if (Form1.isOutputImage2M() || btn_Pow2.Checked)
                {
                    outW = Util.ccNextPOT(outW);
                    outH = Util.ccNextPOT(outH);
                }

                Image outputImage = Image.createImage(outW, outH);
                Graphics g = outputImage.getGraphics();
                for (int i = 0; i < getDstImageCount(); i++)
                {
                    if (getDstImage(i) == null || getDstImage(i).killed) continue;
                    g.drawImage(getDstImage(i), getDstImage(i).x, getDstImage(i).y);
                }
                //
                outputImage.Save(filepath, CellGameEdit.Config.Default.EnablePremultiplyAlpha, format);

                runImageConvertScript(image_convert_script, filepath);

            }
            catch (Exception err)
            {
                Console.WriteLine(this.id + " : save group : " + err.StackTrace + "  at  " + err.Message);
            }
        }

        private void copyToClipboard()
        {
            try
            {
                ArrayList all_images = new ArrayList(dstImages.Count);
                ArrayList all_clips = new ArrayList(dstImages.Count);

                for (int i = 0; i < dstImages.Count; i++)
                {
                    Image img = getDstImage(i);

                    if (img != null && !img.killed)
                    {
                        byte[] data = img.ToRawData();
                        System.Drawing.Rectangle clip = new System.Drawing.Rectangle(
                            img.x, img.y, img.getWidth(), img.getHeight());

                        all_images.Add(data);
                        all_clips.Add(clip);
                    }
                    else
                    {
                        all_images.Add(null);
                        all_clips.Add(null);
                    }
                }

                Hashtable saved = new Hashtable();
                saved.Add("all_clips", all_clips);
                saved.Add("all_images", all_images);

                Clipboard.SetData(DataFormats.Serializable, saved);
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        private void pasteFromClipboard()
        {
            try
            {
                Hashtable saved = (Hashtable)Clipboard.GetData(DataFormats.Serializable);
                if (saved != null)
                {
                    ArrayList all_images = (ArrayList)saved["all_images"];
                    ArrayList all_clips = (ArrayList)saved["all_clips"];

                    DelAllImages();

                    for (int i = 0; i < all_clips.Count; i++)
                    {
                        if (all_clips[i] != null)
                        {
                            System.Drawing.Rectangle clip = (System.Drawing.Rectangle)all_clips[i];
                            byte[] data = (byte[])all_images[i];
                            Image img = new Image(data);
                            img.x = clip.X;
                            img.y = clip.Y;
                            addDstAtIndex(img, i);
                        }
                        else
                        {
                            addDstAtIndex(null, i);
                        }
                    }

                    resetDstBounds();
                    pictureBox2.Refresh();
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void runImageConvertScript(string[] script, string input)
        {
            if (script == null || script.Length == 0)
            {
                return;
            }
            //append\ImageMagick\convertimg.exe -resize 50% <INPUT> png8:<OUTPUT>
            try
            {
                string workdir = System.IO.Path.GetDirectoryName(input);

                string inputfile = System.IO.Path.GetFileName(input);

                foreach (string cmd in script)
                {
                    string cmdline = cmd.Trim();

                    if (cmdline.Length > 0)
                    {
                        runImageConvertExt(cmdline, workdir, inputfile);
                    }
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void runImageConvertExt(string cmd, string workdir, string input)
        {
            string[] exeargs = Regex.Split(cmd, "\\s+");
            string exec = System.IO.Path.GetFullPath(Application.StartupPath + exeargs[0]);
            string args = Util.stringLink(exeargs, 1, exeargs.Length - 1, " ");
            args = args.Replace("<INPUT>", input);
            Console.WriteLine(exec + " " + args);
            //實例一個Process類，啟動一個獨立進程
            Process p = new Process();
            //Process類有一個StartInfo屬性，這個是ProcessStartInfo類，
            //包括了一些屬性和方法，下面我們用到了他的幾個屬性：
            p.StartInfo.FileName = exec;  //設定程序名
            p.StartInfo.Arguments = args;    //設定程式執行參數
            p.StartInfo.WorkingDirectory = workdir;
            p.StartInfo.UseShellExecute = false;        //關閉Shell的使用
            p.StartInfo.RedirectStandardInput = false;   //重定向標準輸入
            p.StartInfo.RedirectStandardOutput = false;  //重定向標準輸出
            p.StartInfo.RedirectStandardError = false;   //重定向錯誤輸出
            p.StartInfo.CreateNoWindow = false;          //設置不顯示窗口
            //啟動
            if (p.Start())
            {
            }
            p.WaitForExit();
            //String result = p.StandardOutput.ReadToEnd();        //從輸出流取得命令執行結果
            //Console.WriteLine(result);
            //p.StandardInput.WriteLine(command);       //也可以用這種方式輸入要執行的命令
            //p.StandardInput.WriteLine("exit");        //不過要記得加上Exit要不然下一行程式執行的時候會當機
        }


        //--------------------------------------------------------------------------------------

        public System.Drawing.Color getCurrentClickColor()
        {
            return toolStripColor.BackColor;
        }

        public void onProcessImageSizeChanged(List<ImageChange> events)
        {
            var childs = ProjectForm.getInstance().getImangesFormChilds(this);

            foreach (var ef in childs)
            {
                ef.ChangeSuperImageSize(this, events);
            }
        }

        //        public void Output(System.IO.StringWriter sw)
        //        {
        //            String head = "/" + this.id+"/tile_";

        //            sw.WriteLine("final static public void buildImages_" + this.id + "(IImages stuff)");
        //            sw.WriteLine("{");
        //            sw.WriteLine("    stuff.buildImages(null, " + this.getDstImageCount() + ");");
        //            sw.WriteLine("    for(int i=0;i<" + this.getDstImageCount() + ";i++){");
        //            sw.WriteLine("        stuff.setTileImage(CIO.loadImage(\"" + head + "\"+i+\".png" + "\"));");
        //            sw.WriteLine("        stuff.addTile();");
        //            sw.WriteLine("    }");
        //            sw.WriteLine("    stuff.gc();");
        //            sw.WriteLine("}");

        //            sw.WriteLine("final static public void buildClipImages_" + this.id + "(IImages stuff)");
        //            sw.WriteLine("{");
        //            sw.WriteLine("    stuff.buildImages(CIO.loadImage(\"/" + this.id + ".png\"), " + this.getDstImageCount() + ");");
        //for (int i = 0; i < getDstImageCount(); i++){if (getDstImage(i) != null){//
        //            sw.WriteLine("    stuff.addTile(" + getDstImage(i).x + "," + getDstImage(i).y + "," + getDstImage(i).getWidth() + "," + getDstImage(i).getHeight() + ");");
        //}}//
        //            sw.WriteLine("    stuff.gc();");
        //            sw.WriteLine("}");

        //            SaveAllImages(ProjectForm.workSpace,"png",false,true);

        //        }

        public void OutputCustom(
            int index,
            String script,
            System.IO.StringWriter output,
            String outDir,
            String imageType,
            bool imageTile,
            bool imageTileData,
            bool imageGroup,
            bool imageGroupData)
        {
            RefreshDstMaskColor();
            lock (this)
            {
                try
                {
                    String images = Util.getFullTrunkScript(script, SC._IMAGES, SC._END_IMAGES);

                    Boolean isIgnoreNullTile = false;
                    try
                    {
                        isIgnoreNullTile = Util.getCommandScript(images, SC.IGNORE_NULL_CLIP).Equals("true", StringComparison.CurrentCultureIgnoreCase);
                    }
                    catch (Exception err) { }

                    bool fix = false;
                    do
                    {
                        String[] clips = new string[getDstImageCount()];
                        for (int i = 0; i < getDstImageCount(); i++)
                        {
                            if (getDstImage(i) != null && getDstImage(i).killed == false)
                            {
                                string X = getDstImage(i).killed ? "0" : getDstImage(i).x.ToString();
                                string Y = getDstImage(i).killed ? "0" : getDstImage(i).y.ToString();
                                string W = getDstImage(i).killed ? "0" : getDstImage(i).getWidth().ToString();
                                string H = getDstImage(i).killed ? "0" : getDstImage(i).getHeight().ToString();
                                string DATA = getDstImage(i).killed ? "" : (String)dstDataKeys[i];
                                clips[i] = Util.replaceKeywordsScript(images, SC._CLIP, SC._END_CLIP,
                                    new string[] { SC.INDEX, SC.X, SC.Y, SC.W, SC.H, SC.DATA },
                                    new string[] { i.ToString(), X, Y, W, H, DATA }
                                    );
                            }
                            else
                            {
                                if (isIgnoreNullTile == false)
                                {
                                    clips[i] = Util.replaceKeywordsScript(images, SC._CLIP, SC._END_CLIP,
                                          new string[] { SC.INDEX, SC.X, SC.Y, SC.W, SC.H, SC.DATA },
                                          new string[] { i.ToString(), "0", "0", "0", "0", "" }
                                          );
                                }
                                else
                                {
                                    clips[i] = "";
                                    Console.WriteLine("Ignore null clip : " + i);
                                }

                            }
                        }
                        string temp = Util.replaceSubTrunksScript(images, SC._CLIP, SC._END_CLIP, clips);

                        if (temp == null)
                        {
                            fix = false;
                        }
                        else
                        {
                            fix = true;
                            images = temp;
                        }

                    } while (fix);

                    String ofiletype = imageType;
                    Boolean ogroup = imageGroup;
                    Boolean otile = imageTile;

                    String custom_output_image_type = "";
                    if (!chk_custom_output.Checked)
                    {
                        custom_output_image_type =
                            (chk_output_group.Checked ? "group" : "") + "," +
                            (chk_output_tile.Checked ? "tile" : "");
                        ogroup = chk_output_group.Checked;
                        otile = chk_output_tile.Checked;
                    }

                    String custom_output_image_file = "";
                    if (!chk_custom_filetype.Checked)
                    {
                        if (chk_output_file_bmp.Checked)
                        {
                            custom_output_image_file = "bmp";
                            ofiletype = "bmp";
                        }
                        else if (chk_output_file_jpg.Checked)
                        {
                            custom_output_image_file = "jpg";
                            ofiletype = "jpg";
                        }
                        else if (chk_output_file_png.Checked)
                        {
                            custom_output_image_file = "png";
                            ofiletype = "png";
                        }
                    }

                    string convert_script = outputAllImages(outDir, ofiletype, otile, ogroup);

                    System.Drawing.Rectangle bounds = getDstBounds();
                    string[] adata = Util.toStringMultiLine(append_data);
                    string APPEND_DATA = Util.toStringArray1D(ref adata);
                    images = Util.replaceKeywordsScript(images, SC._IMAGES, SC._END_IMAGES,
                        new string[] {
                            SC.NAME,
                            SC.IMAGES_INDEX,
                            SC.COUNT,
                            SC.OUTPUT_IMAGE_TYPE,
                            SC.OUTPUT_IMAGE_FILE,
                            SC.OUTPUT_IMAGE_SPLIT,
                            SC.ALL_WIDTH,
                            SC.ALL_HEIGHT,
                            SC.APPEND_DATA,
                            SC.IMAGE_INFO
                        },
                        new string[] {
                            this.id,
                            index.ToString(),
                            this.getDstImageCount().ToString(),
                            custom_output_image_type,
                            custom_output_image_file,
                            TotalSplit.ToString(),
                            bounds.Width.ToString(),
                            bounds.Height.ToString(),
                            APPEND_DATA,
                            convert_script
                        }
                        );

                    output.WriteLine(images);
                    //Console.WriteLine(images);



                }
                catch (Exception err) { Console.WriteLine(this.id + " : " + err.StackTrace + "  at  " + err.Message); }
            }
        }


        private void ImagesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }
        private void ImagesForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        public String getID()
        {
            return id;
        }

        public Form getForm()
        {
            return this;
        }

        public void setSrcImage()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*";
            try
            {
                openFileDialog1.InitialDirectory = System.IO.Path.GetDirectoryName(Config.Default.LastImageOpenDir);
            }
            catch (System.Exception e) { }
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Config.Default.LastImageOpenDir = System.IO.Path.GetDirectoryName(openFileDialog1.FileName);
                Config.Default.Save();
                Image buf = Image.createImage(openFileDialog1.FileName);

                srcImage = Image.createImage(buf.getWidth(), buf.getHeight());
                srcImage.getGraphics().drawImage(buf, 0, 0);

                buf.dimg.Dispose();
                buf = null;

                pictureBox1.Width = srcImage.getWidth() * srcSize;
                pictureBox1.Height = srcImage.getHeight() * srcSize;

                toolStripButton2.Checked = false;
                toolStripButton3.Checked = false;

                toolStripStatusLabel3.Text =
                  " : 宽=" + srcImage.getWidth() +
                  " : 高=" + srcImage.getHeight()
                ;
            }

        }

        public void addDstAtIndex(Image img, int index)
        {
            is_change_image = true;

            while (index >= dstImages.Count)
            {
                dstImages.Add(null);
                dstDataKeys.Add("");
            }

            dstImages[index] = img;
            if (img != null)
            {
                img.indexOfImages = index;
            }
        }

        public void addDst(Image img)
        {
            is_change_image = true;

            for (int i = 0; i < dstImages.Count; i++)
            {
                if (dstImages[i] == null || getDstImage(i).killed)
                {
                    dstImages[i] = img;
                    if (img != null)
                    {
                        img.indexOfImages = i;
                    }
                    return;
                }
            }
            if (img != null)
            {
                img.indexOfImages = dstImages.Count;
            }
            dstImages.Add(img);
            dstDataKeys.Add("");
        }

        public System.Drawing.Rectangle getDstBounds()
        {
            return getMinSize();
        }

        public System.Drawing.Rectangle resetDstBounds()
        {
            System.Drawing.Rectangle dbounds = getDstBounds();
            SetDstSize(
                (int)(dbounds.Width * dstScaleF),
                (int)(dbounds.Height * dstScaleF));
            return dbounds;
        }

        public List<Image> addDirImages()
        {
            List<Image> ret = new List<Image>();

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*";
            try
            {
                openFileDialog1.InitialDirectory = System.IO.Path.GetDirectoryName(Config.Default.LastImageOpenDir);
            }
            catch (System.Exception e)
            {

            }

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Config.Default.LastImageOpenDir = System.IO.Path.GetDirectoryName(openFileDialog1.FileName);
                Config.Default.Save();
                System.Drawing.Rectangle dbounds = getDstBounds();
                int maxW = dbounds.Width;
                int maxH = dbounds.Height;
                int curX = 0;
                int curY = maxH;

                for (int i = 0; i < openFileDialog1.FileNames.Length; i++)
                {
                    try
                    {
                        Image img = Image.createImage(openFileDialog1.FileNames[i]);
                        if (img.getWidth() > maxW)
                        {
                            maxW = img.getWidth();
                        }
                        if (curX + img.getWidth() > maxW)
                        {
                            curX = 0;
                            curY = maxH;
                            img.x = curX;
                            img.y = curY;
                            curX += img.getWidth();
                        }
                        else
                        {
                            img.x = curX;
                            img.y = curY;
                            curX += img.getWidth();
                        }
                        addDst(img);
                        ret.Add(img);
                        maxW = Math.Max(maxW, img.x + img.getWidth());
                        maxH = Math.Max(maxH, img.y + img.getHeight());
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine(err.StackTrace + "  at  " + err.Message);
                    }
                }
                SetDstSize((int)(maxW * dstScaleF), (int)(maxH * dstScaleF));
            }

            return ret;
        }


        public void addDstImage()
        {
            if (srcImage == null)
            {
                System.Windows.Forms.MessageBox.Show("设置原始图片先");
            }
            else
            {
                System.Drawing.Rectangle dbounds = getDstBounds();

                if (srcRect.Width != 0 && srcRect.Height != 0)
                {
                    Image img = Image.createImage(
                        srcImage,
                        srcRect.X,
                        srcRect.Y,
                        srcRect.Width,
                        srcRect.Height);
                    img.x = 0;
                    img.y = dbounds.Height;
                    addDst(img);

                    resetDstBounds();
                }
            }
        }

        public void addDstImages()
        {
            if (srcImage == null)
            {
                System.Windows.Forms.MessageBox.Show("设置原始图片先");
            }
            else
            {
                if (srcRect.Width != 0 && srcRect.Height != 0)
                {
                    System.Drawing.Rectangle dbounds = getDstBounds();
                    int xcount = Util.cycMod(srcRect.Width, CellW);
                    int ycount = Util.cycMod(srcRect.Height, CellH);

                    for (int y = 0; y < ycount; y++)
                    {
                        for (int x = 0; x < xcount; x++)
                        {
                            if (CellW != 0 && CellH != 0)
                            {
                                Image img = Image.createImage(
                                    srcImage,
                                    srcRect.X + x * CellW,
                                    srcRect.Y + y * CellH,
                                    CellW,
                                    CellH);
                                img.x = x * (CellW);
                                img.y = y * (CellH) + dbounds.Height;
                                addDst(img);
                            }

                        }
                    }


                    resetDstBounds();

                }
            }

        }

        public int getDstWidth()
        {
            return (int)(pictureBox2.Width / dstScaleF);
        }

        public int getDstHeight()
        {
            return (int)(pictureBox2.Height / dstScaleF);
        }

        public Image getSrcImage()
        {
            return srcImage;
        }

        public Image getDstImage(int index)
        {
            if (index >= dstImages.Count || index < 0) return null;
            return (((Image)(dstImages[index])));
        }
        public Image getOrAddDstImage(int index)
        {
            if (index < 0)
            {
                return null;
            }
            if (index >= dstImages.Count)
            {
                int addCount = index - dstImages.Count + 1;
                for (int i = 0; i < addCount; i++)
                {
                    dstImages.Add(null);
                    dstDataKeys.Add("");
                }
                return null;
            }
            return (((Image)(dstImages[index])));
        }

        public Image getSelectedImage()
        {
            return dstSelected;
        }

        public int getAvaliableImageIndex()
        {
            Image img = getDstImage(dstSelectIndex);
            if (img == null)
            {
                int count = getDstImageCount();
                for (int i = 0; i < count; i++)
                {
                    img = getDstImage(i);
                    if (img != null)
                    {
                        return i;
                    }
                }
            }
            return dstSelectIndex;
        }

        public int getDstImageCount()
        {
            return dstImages.Count;
        }

        public void RefreshDstMaskColor()
        {
            bool touched = false;
            int count = getDstImageCount();
            for (int i = 0; i < count; i++)
            {
                Image srcimg = getDstImage(i);
                if (srcimg != null)
                {
                    if (!srcimg.killed && IsIntersectImage(srcimg))
                    {
                        srcimg.Touched = true;
                        touched = true;
                    }
                    else
                    {
                        srcimg.Touched = false;
                    }
                }
            }
            IsAnyImageTouched = touched;
        }

        public bool moveDstImage(int index, int dx, int dy, bool ignoreCollides)
        {
            Image img_i = getDstImage(index);

            if (img_i == null) return false;
            if (img_i.Touched)
            {
                ignoreCollides = true;
            }

            img_i.x += dx;
            img_i.y += dy;

            System.Drawing.Rectangle dbounds = getDstBounds();

            if (dbounds.Width < img_i.x + img_i.getWidth())
            {
                dbounds.Width += (int)(dx);
            }

            if (dbounds.Height < img_i.y + img_i.getHeight())
            {
                dbounds.Height += (int)(dy);
            }

            if (ignoreCollides)
            {
                resetDstBounds();

                return true;
            }
            else
            {
                if (IsIntersectSplitLine(img_i))
                {
                    img_i.x -= dx;
                    img_i.y -= dy;
                    return false;
                }

                System.Drawing.Rectangle src = new System.Drawing.Rectangle(
                    img_i.x,
                    img_i.y,
                    img_i.getWidth(),
                    img_i.getHeight()
                    );

                System.Drawing.Rectangle scope = new System.Drawing.Rectangle(
                    0, 0,
                    dbounds.Width,
                    dbounds.Height
                    );

                if (scope.Contains(src) == false)
                {
                    img_i.x -= dx;
                    img_i.y -= dy;
                    return false;
                }


                for (int i = 0; i < getDstImageCount(); i++)
                {
                    Image img_d = getDstImage(i);

                    if (img_d != null && img_d.killed == false && i != index)
                    {
                        System.Drawing.Rectangle dst = new System.Drawing.Rectangle(
                           img_d.x,
                           img_d.y,
                           img_d.getWidth(),
                           img_d.getHeight()
                           );

                        if (src.IntersectsWith(dst))
                        {
                            img_i.x -= dx;
                            img_i.y -= dy;
                            return false;
                        }

                    }
                }
                resetDstBounds();

                return true;
            }
        }

        public bool moveDstImages(int dx, int dy, bool ignoreCollides)
        {
            if (dx == 0 && dy == 0) return true;

            if (!ignoreCollides)
            {
                System.Drawing.Rectangle src = new System.Drawing.Rectangle();
                System.Drawing.Rectangle dst = new System.Drawing.Rectangle();

                for (int i = 0; i < getDstImageCount(); i++)
                {
                    Image img_i = getDstImage(i);

                    if (img_i != null && img_i.killed == false && img_i.selected)
                    {
                        src.X = img_i.x + dx;
                        src.Y = img_i.y + dy;
                        src.Width = img_i.getWidth();
                        src.Height = img_i.getHeight();

                        if (IsIntersectSplitLine(src.X, src.Y, src.Width, src.Height))
                        {
                            return false;
                        }
                        if (src.X < 0 || src.Y < 0)
                        {
                            return false;
                        }
                        for (int j = 0; j < getDstImageCount(); j++)
                        {
                            Image img_j = getDstImage(j);

                            if (i != j && img_j != null && img_j.killed == false && img_j.selected == false)
                            {
                                dst.X = img_j.x;
                                dst.Y = img_j.y;
                                dst.Width = img_j.getWidth();
                                dst.Height = img_j.getHeight();

                                if (src.IntersectsWith(dst))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < getDstImageCount(); i++)
            {
                Image img_i = getDstImage(i);

                if (img_i != null && img_i.killed == false && img_i.selected)
                {
                    img_i.x += dx;
                    img_i.y += dy;
                }
            }

            resetDstBounds();
            return true;
        }


        private Boolean moveImage(Image srcImage, System.Drawing.Rectangle region, int dx, int dy)
        {
            if (IsIntersectSplitLine(
                srcImage.x + dx,
                srcImage.y + dy,
                srcImage.getWidth(),
                srcImage.getHeight()))
            {
                return false;
            }

            System.Drawing.Rectangle srcRect = new System.Drawing.Rectangle(
                srcImage.x + dx,
                srcImage.y + dy,
                srcImage.getWidth(),
                srcImage.getHeight()
                );
            System.Drawing.Rectangle dstRect = new System.Drawing.Rectangle();

            if (!region.Contains(srcRect))
            {
                return false;
            }

            for (int i = dstImages.Count - 1; i >= 0; --i)
            {
                int di = Util.cycNum(i, srcImage.indexOfImages, dstImages.Count);
                Image dstimg = (Image)dstImages[di];
                if (dstimg != null)
                {
                    if (!dstimg.killed && dstimg != srcImage)
                    {
                        dstRect.X = dstimg.x;
                        dstRect.Y = dstimg.y;
                        dstRect.Width = dstimg.getWidth();
                        dstRect.Height = dstimg.getHeight();

                        if (srcRect.IntersectsWith(dstRect))
                        {
                            return false;
                        }
                    }
                }
            }

            srcImage.x += dx;
            srcImage.y += dy;

            return true;
        }

        private void moveAllImages(int dx, int dy)
        {
            if (dx == 0 && dy == 0) return;

            System.Drawing.Rectangle region = getDstBounds();

            while (true)
            {
                Boolean ismoved = false;

                for (int i = 0; i < dstImages.Count; i++)
                {
                    Image img = (Image)dstImages[i];
                    if (img != null && !img.killed)
                    {
                        if (moveImage(img, region, dx * img.getWidth(), dy * img.getHeight()))
                        {
                            ismoved = true;
                        }
                        else if (moveImage(img, region, dx, dy))
                        {
                            ismoved = true;
                        }
                    }
                }
                if (!ismoved)
                {
                    break;
                }
            }
        }



        public void spaceAllDstImages(int space)
        {
            while (true)
            {
                if (!spaceHasIntersects(space, 0))
                {
                    resetDstBounds();
                    break;
                }
            }
            while (true)
            {
                if (!spaceHasIntersects(0, space))
                {
                    resetDstBounds();
                    break;
                }
            }
            pictureBox2.Refresh();
        }

        private bool spaceHasIntersects(int spaceX, int spaceY)
        {
            int totalCount = getDstImageCount();
            System.Drawing.Rectangle src = new System.Drawing.Rectangle();
            System.Drawing.Rectangle dst = new System.Drawing.Rectangle();

            for (int i = 0; i < totalCount; i++)
            {
                Image img_i = getDstImage(i);

                if (img_i != null && img_i.killed == false)
                {
                    src.X = img_i.x;
                    src.Y = img_i.y;
                    src.Width = img_i.getWidth() + spaceX;
                    src.Height = img_i.getHeight() + spaceY;

                    for (int j = 0; j < totalCount; j++)
                    {
                        Image img_j = getDstImage(j);

                        if (i != j && img_j != null && img_j.killed == false)
                        {
                            dst.X = img_j.x;
                            dst.Y = img_j.y;
                            dst.Width = img_j.getWidth() + spaceX;
                            dst.Height = img_j.getHeight() + spaceY;

                            if (src.IntersectsWith(dst))
                            {
                                if (spaceX != 0)
                                {
                                    if (img_j.x > img_i.x)
                                    {
                                        img_j.x++;
                                    }
                                    else
                                    {
                                        img_i.x++;
                                    }
                                }
                                else if (spaceY != 0)
                                {
                                    if (img_j.y > img_i.y)
                                    {
                                        img_j.y++;
                                    }
                                    else
                                    {
                                        img_i.y++;
                                    }
                                }

                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }


        public void changeDstImage(int index, Image changed)
        {
            Image srcImage = getOrAddDstImage(index);
            dstImages[index] = changed;
            //Image srcImage = getDstImage(index);
            int oy = 0;
            int ox = 0;
            System.Drawing.Rectangle dbounds = getDstBounds();
            if (srcImage != null)
            {
                oy = srcImage.y;
                ox = srcImage.x;
                if (changed.getWidth() <= srcImage.getWidth() &&
                    changed.getHeight() <= srcImage.getHeight())
                {
                    changed.x = ox;
                    changed.y = oy;
                    return;
                }
            }

            bool isSameSize = true;
            System.Drawing.Rectangle srcRect = new System.Drawing.Rectangle(ox, oy, changed.getWidth(), changed.getHeight());
            for (int i = 0; i < getDstImageCount(); i++)
            {
                Image dstImage = getDstImage(i);

                if (dstImage != null && !dstImage.killed && i != index)
                {
                    System.Drawing.Rectangle dstRect = new System.Drawing.Rectangle(
                       dstImage.x,
                       dstImage.y,
                       dstImage.getWidth(),
                       dstImage.getHeight()
                       );

                    if (srcRect.IntersectsWith(dstRect))
                    {
                        isSameSize = false;
                        break;
                    }

                }
            }
            if (isSameSize)
            {
                changed.x = ox;
                changed.y = oy;
            }
            else
            {
                changed.x = dbounds.Width;
                changed.y = oy;
            }

            resetDstBounds();
        }

        public void changeDstImageFromDir(int index)
        {

            if (getDstImage(index) == null) return;

            is_change_image = true;

            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*";
                try
                {
                    openFileDialog1.InitialDirectory = System.IO.Path.GetDirectoryName(Config.Default.LastImageOpenDir);
                }
                catch (System.Exception e)
                {

                }
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Config.Default.LastImageOpenDir = System.IO.Path.GetDirectoryName(openFileDialog1.FileName);
                    Config.Default.Save();

                    Image changed = Image.createImage(openFileDialog1.FileName);
                    changeDstImage(index, changed);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(this.id + " : " + err.StackTrace + "  at  " + err.Message);
            }


        }

        public void changeDstImageFormSrc(int index)
        {
            if (getDstImage(index) == null) return;
            if (srcImage == null) return;

            is_change_image = true;

            try
            {
                if (srcPX != srcQX && srcPY != srcQY)
                    if (srcRect.Width > 0 && srcRect.Height > 0 && srcImage.getWidth() > 0 && srcImage.getHeight() > 0)
                    {
                        Image changed = Image.createImage(
                                       srcImage,
                                       srcRect.X,
                                       srcRect.Y,
                                       srcRect.Width,
                                       srcRect.Height);

                        changeDstImage(index, changed);

                        //changed.dimg.Save(ProjectForm.workSpace + name, System.Drawing.Imaging.ImageFormat.Png);
                    }
            }
            catch (Exception err)
            {
                Console.WriteLine(this.id + " : " + err.StackTrace + "  at  " + err.Message);
            }


        }

        private bool IsIntersectImage(Image srcImage)
        {
            if (srcImage.x < 0 || srcImage.y < 0)
            {
                return true;
            }
            if (IsIntersectSplitLine(srcImage))
            {
                return true;
            }
            System.Drawing.Rectangle srcRect = new System.Drawing.Rectangle(
                srcImage.x,
                srcImage.y,
                srcImage.getWidth(),
                srcImage.getHeight()
                );
            System.Drawing.Rectangle dstRect = new System.Drawing.Rectangle();

            for (int i = dstImages.Count - 1; i >= 0; --i)
            {
                int di = Util.cycNum(i, srcImage.indexOfImages, dstImages.Count);
                Image dstimg = (Image)dstImages[di];
                if (dstimg != null)
                {
                    if (!dstimg.killed && dstimg != srcImage)
                    {
                        dstRect.X = dstimg.x;
                        dstRect.Y = dstimg.y;
                        dstRect.Width = dstimg.getWidth();
                        dstRect.Height = dstimg.getHeight();

                        if (srcRect.IntersectsWith(dstRect))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool IsIntersectSplitLine(Image dimg)
        {
            if (TotalSplit > 0)
            {
                int ttsx = dimg.x % TotalSplit;
                int ttsy = dimg.y % TotalSplit;
                if ((TotalSplit > ttsx && (ttsx + dimg.getWidth()) > TotalSplit) ||
                    (TotalSplit > ttsy && (ttsy + dimg.getHeight()) > TotalSplit))
                {
                    return true;
                }
            }
            return false;
        }



        private bool IsIntersectSplitLine(int x, int y, int w, int h)
        {
            if (TotalSplit > 0)
            {
                int ttsx = x % TotalSplit;
                int ttsy = y % TotalSplit;
                if ((TotalSplit > ttsx && (ttsx + w) > TotalSplit) ||
                    (TotalSplit > ttsy && (ttsy + h) > TotalSplit))
                {
                    return true;
                }
            }
            return false;
        }

        public void delDstImage(int index)
        {
            Image img = getDstImage(index);
            if (img != null)
            {
                img.killed = true;
            }
        }

        private void cacheDstScale()
        {
            for (int i = 0; i < getDstImageCount(); i++)
            {
                Image dimg = getDstImage(i);
                if (dimg != null)
                {
                    dimg.CacheScale(dstScaleF);
                }
            }
        }

        public void renderSrcImage(Graphics g, int x, int y)
        {
            if (srcImage == null) return;
            g.drawImageScale(srcImage, x, y, 0, srcSize);
        }

        public void renderDstImage(Graphics g)
        {
            int sw = TotalSplit;
            int sh = TotalSplit;
            uint ftidColor = (uint)getColorTileID().ToArgb();
            uint fkeyColor = (uint)getColorKey().ToArgb();

            for (int i = 0; i < getDstImageCount(); i++)
            {
                Image dimg = getDstImage(i);

                if (dimg != null && dimg.killed == false)
                {
                    float tx = dimg.x * dstScaleF;
                    float ty = dimg.y * dstScaleF;
                    float tw = dimg.getWidth() * dstScaleF;
                    float th = dimg.getHeight() * dstScaleF;

                    g.drawImageScale(dimg, tx, ty, 0, dstScaleF);

                    if (IsIntersectSplitLine(dimg) || dimg.Touched)
                    {
                        g.setColor(0x7fff0000);
                        g.fillRect(tx, ty, tw, th);
                    }
                    if (toolStripButton13.Checked)
                    {
                        g.setColor(0x7fffffff);
                        g.drawRect(tx, ty, tw, th);
                    }
                    if (chkIsShowkey.Checked)
                    {
                        g.drawStringBorder((String)dstDataKeys[i], tx, ty, fkeyColor, 0xffffffff);
                    }
                    if (chkIsShowTileID.Checked)
                    {
                        float fy = ty + th - (Graphics.font.GetHeight());
                        g.drawStringBorder(i.ToString(), tx, fy, ftidColor, 0xffffffff);
                    }
                    if (multiSelect.Checked)
                    {
                        if (dimg.selected)
                        {
                            g.setColor(0x40ffffff);
                            g.fillRect(tx, ty, tw, th);
                            g.setColor(0x7fffffff);
                            g.drawRect(tx, ty, tw, th);
                        }
                    }
                    if (checkTileUsed.Checked)
                    {
                        if (!dimg.used)
                        {
                            g.setColor(0xff, 0xff0000);
                            g.pen.Width = 3;
                            g.drawLine(tx, ty, tx + tw, ty + th);
                            g.drawLine(tx + tw, ty, tx, ty + th);
                            g.drawRect(tx, ty, tw, th);

                            g.setColor(0xff, 0x0000ff);
                            g.pen.Width = 1;
                            g.drawLine(tx, ty, tx + tw, ty + th);
                            g.drawLine(tx + tw, ty, tx, ty + th);
                            g.drawRect(tx, ty, tw, th);
                        }
                    }

                }
            }

        }

        public System.Drawing.Color getColorKey()
        {
            return BtnSelectKeyColor.BackColor;
        }

        public System.Drawing.Color getColorTileID()
        {
            return BtnSelectTileIDColor.BackColor;
        }

        public System.Drawing.Rectangle getMinSize()
        {
            int outW = 0;
            int outH = 0;
            for (int i = 0; i < getDstImageCount(); i++)
            {
                Image dimg = getDstImage(i);
                if (dimg != null && !dimg.killed)
                {
                    outW = Math.Max(outW, dimg.x + dimg.getWidth());
                    outH = Math.Max(outH, dimg.y + dimg.getHeight());
                }
            }
            return new System.Drawing.Rectangle(0, 0, outW, outH);
        }

        public int GetDstWeight()
        {
            int ret = 0;
            for (int i = 0; i < getDstImageCount(); i++)
            {
                Image dimg = getDstImage(i);
                if (dimg != null && !dimg.killed)
                {
                    ret += (dimg.Width * dimg.Height);
                }
            }
            return ret;
        }

        #region PictureBox Src
        // src edit
        private void toolStripTextBox1_Leave(object sender, EventArgs e)
        {
            if (Cell.Util.stringIsDigit(toolStripTextBox1.Text, 0, toolStripTextBox1.Text.Length) >= toolStripTextBox1.Text.Length &&
                Cell.Util.stringDigitToInt(toolStripTextBox1.Text, 0, toolStripTextBox1.Text.Length) >= 1)
            {
                CellW = Cell.Util.stringDigitToInt(toolStripTextBox1.Text, 0, toolStripTextBox1.Text.Length);
            }
            else
            {
                MessageBox.Show("只能输入大于0的数字！");
                toolStripTextBox1.Focus();
            }
        }
        private void toolStripTextBox2_Leave(object sender, EventArgs e)
        {
            if (Cell.Util.stringIsDigit(toolStripTextBox2.Text, 0, toolStripTextBox2.Text.Length) >= toolStripTextBox2.Text.Length &&
              Cell.Util.stringDigitToInt(toolStripTextBox2.Text, 0, toolStripTextBox2.Text.Length) >= 1)
            {
                CellH = Cell.Util.stringDigitToInt(toolStripTextBox2.Text, 0, toolStripTextBox2.Text.Length);
            }
            else
            {
                MessageBox.Show("只能输入大于0的数字！");
                toolStripTextBox2.Focus();
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            setSrcImage();
        }
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            List<javax.microedition.lcdui.Image> added = addDirImages();

            ImageProcessDialog ipd = new ImageProcessDialog(this, added, true);

            if (ipd.ShowDialog() == DialogResult.OK)
            {
                changeImage();

                pictureBox2.Refresh();
            }
        }
        private void toolStripButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripButton2.Checked)
            {
                toolStripButton3.Checked = false;
                toolStripRightSize.Checked = false;
            }

            refreshSrcRect();
            srcRectIsDown = false;
            pictureBox1.Refresh();
        }
        private void toolStripButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripButton3.Checked)
            {
                toolStripButton2.Checked = false;
                toolStripRightSize.Checked = false;
            }
            refreshSrcRect();
            srcRectIsDown = false;
            pictureBox1.Refresh();
        }

        private void toolStripRightSize_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripRightSize.Checked)
            {
                toolStripButton2.Checked = false;
                toolStripButton3.Checked = false;
            }
            refreshSrcRect();
            srcRectIsDown = false;
            pictureBox1.Refresh();
        }

        private void toolStripRightSize_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (toolStripButton2.Checked || toolStripRightSize.Checked)
            {
                addDstImage();
            }
            else if (toolStripButton3.Checked)
            {
                addDstImages();
            }
            //pictureBox2.Refresh();
        }

        private void btnSrcSelectAll_Click(object sender, EventArgs e)
        {
            srcPX = 0;
            srcPY = 0;
            srcQX = srcImage.getWidth();
            srcQY = srcImage.getHeight();

            refreshSrcRect();
            srcRectIsDown = false;
            pictureBox1.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //MapRegion.Image = new System.Drawing.Bitmap(MapRegion.Width, MapRegion.Height);
            //System.Drawing.Graphics dg = System.Drawing.Graphics.FromImage(MapRegion.Image);
            System.Drawing.Graphics dg = e.Graphics;
            Graphics g = new Graphics(dg);
            renderSrcImage(g, 0, 0);


            //System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0x80, 0xff, 0xff, 0xff));
            // System.Drawing.Brush brush = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0x80, 0x80, 0x80, 0x80)).Brush;

            g.setColor(0xFF, 0, 0, 0);
            if (btnIsShowGrid.Checked)
            {
                g.setColor(0x7f, 0xff, 0xff, 0xff);
                for (int x = 0; x < pictureBox1.Width; x += CellW * srcSize)
                {
                    g.drawLine(x, 0, x, pictureBox1.Height);
                }
                for (int y = 0; y < pictureBox1.Height; y += CellH * srcSize)
                {
                    g.drawLine(0, y, pictureBox1.Width, y);
                }
            }

            if (toolStripButton2.Checked || toolStripRightSize.Checked)
            {
                g.setColor(0x20, 0xff, 0xff, 0xff);
                dg.FillRectangle(g.pen.Brush,
                    srcRect.X * srcSize, srcRect.Y * srcSize, (srcRect.Width) * srcSize, (srcRect.Height) * srcSize);
            }
            else if (toolStripButton3.Checked)
            {
                g.setColor(0x20, 0xff, 0xff, 0xff);
                dg.FillRectangle(g.pen.Brush,
                    srcRect.X * srcSize, srcRect.Y * srcSize, (srcRect.Width) * srcSize, (srcRect.Height) * srcSize);
                for (int x = srcRect.X; x < srcRect.X + srcRect.Width; x += CellW)
                {
                    dg.DrawLine(System.Drawing.Pens.White,
                        x * srcSize, srcRect.Y * srcSize, x * srcSize, srcRect.Y * srcSize + (srcRect.Height) * srcSize);
                }
                for (int y = srcRect.Y; y < srcRect.Y + srcRect.Height; y += CellH)
                {
                    dg.DrawLine(System.Drawing.Pens.White,
                        srcRect.X * srcSize, y * srcSize, srcRect.X * srcSize + (srcRect.Width) * srcSize, y * srcSize);
                }
            }

            g.setColor(0xff, 0x80, 0xff, 0x80);
            g.drawLine(srcPX * srcSize, 0, srcPX * srcSize, pictureBox1.Height);
            g.drawLine(0, srcPY * srcSize, pictureBox1.Width, srcPY * srcSize);

            g.setColor(0xff, 0x80, 0x80, 0xFF);
            g.drawLine(srcQX * srcSize, 0, srcQX * srcSize, pictureBox1.Height);
            g.drawLine(0, srcQY * srcSize, pictureBox1.Width, srcQY * srcSize);

        }


        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            toolStripStatusLabel3.Text =
                        " : X=" + e.X / srcSize +
                        " : Y=" + e.Y / srcSize +
                        " : 宽=" + pictureBox1.Width / srcSize +
                        " : 高=" + pictureBox1.Height / srcSize
                    ;


            if (toolStripButton2.Checked || toolStripButton3.Checked || toolStripRightSize.Checked)
            {
                if (srcRectIsDown)
                {
                    srcQX = (e.X < 0 ? 0 : (e.X > pictureBox1.Width ? pictureBox1.Width : e.X)) / srcSize;
                    srcQY = (e.Y < 0 ? 0 : (e.Y > pictureBox1.Height ? pictureBox1.Height : e.Y)) / srcSize;
                }
                refreshSrcRect();
            }

            pictureBox1.Refresh();
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            textFocus.Focus();

            srcPX = e.X / srcSize;
            srcPY = e.Y / srcSize;
            srcQX = e.X / srcSize;
            srcQY = e.Y / srcSize;

            if (toolStripButton2.Checked || toolStripButton3.Checked || toolStripRightSize.Checked)
            {
                srcRect.X = srcPX;
                srcRect.Width = 0;
                srcRect.Y = srcPY;
                srcRect.Height = 0;

                refreshSrcRect();
            }


            srcRectIsDown = true;
            pictureBox1.Refresh();


        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {

            srcQX = e.X / srcSize;
            srcQY = e.Y / srcSize;

            refreshSrcRect();

            srcRectIsDown = false;
            pictureBox1.Refresh();
        }


        private void textFocus_KeyDown(object sender, KeyEventArgs e)
        {
            if (srcRect.Width != 0 && srcRect.Height != 0)
            {
                if (e.Shift)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Up:
                            srcQY -= 1; break;
                        case Keys.Down:
                            srcQY += 1; break;
                        case Keys.Left:
                            srcQX -= 1; break;
                        case Keys.Right:
                            srcQX += 1; break;
                    }
                }
                else
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Up:
                            srcPY -= 1; break;
                        case Keys.Down:
                            srcPY += 1; break;
                        case Keys.Left:
                            srcPX -= 1; break;
                        case Keys.Right:
                            srcPX += 1; break;
                    }
                }

                refreshSrcRect();
                pictureBox1.Refresh();
            }
        }

        private void statusStrip1_KeyDown(object sender, KeyEventArgs e)
        {


        }

        private void refreshSrcRect()
        {
            srcPX = Math.Max(0, srcPX);
            srcPY = Math.Max(0, srcPY);
            srcPX = Math.Min(pictureBox1.Width, srcPX);
            srcPY = Math.Min(pictureBox1.Height, srcPY);

            Image dstImage = getSelectedImage();
            if (toolStripRightSize.Checked && dstImage != null)
            {
                srcQX = srcPX + dstImage.getWidth();
                srcQY = srcPY + dstImage.getHeight();
            }
            else
            {
                srcQX = Math.Max(0, srcQX);
                srcQY = Math.Max(0, srcQY);
                srcQX = Math.Min(pictureBox1.Width, srcQX);
                srcQY = Math.Min(pictureBox1.Height, srcQY);
            }

            srcRect.X = Math.Min(srcPX, srcQX);
            srcRect.Width = Math.Abs(srcPX - srcQX);
            srcRect.Y = Math.Min(srcPY, srcQY);
            srcRect.Height = Math.Abs(srcPY - srcQY);

            toolStripStatusLabel2.Text =
                " : 选择宽=" + srcRect.Width +
                " : 选择高=" + srcRect.Height;

            if (toolStripButton3.Checked)
            {
                toolStripStatusLabel2.Text +=
                " : 横向数量=" + srcRect.Width / CellW +
                " : 纵向数量=" + srcRect.Height / CellH
                ;
            }
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            srcSize += 1;
            if (srcSize > 8) srcSize = 8;

            try
            {
                if (getSrcImage() != null)
                {
                    pictureBox1.Width = getSrcImage().getWidth() * srcSize;
                    pictureBox1.Height = getSrcImage().getHeight() * srcSize;
                    pictureBox1.Refresh();
                }
            }
            catch (Exception err) { }

        }
        private void toolStripButton8_Click(object sender, EventArgs e)
        {

            srcSize -= 1;
            if (srcSize < 1) srcSize = 1;

            try
            {
                if (getSrcImage() != null)
                {
                    pictureBox1.Width = getSrcImage().getWidth() * srcSize;
                    pictureBox1.Height = getSrcImage().getHeight() * srcSize;
                    pictureBox1.Refresh();
                }
            }
            catch (Exception err) { }
        }


        #endregion

        //----------------------------------------------------------------------------------------------
        // dst edit

        Image dstSelected = null;
        int dstSelectIndex = -1;
        System.Drawing.Rectangle dstRect = new System.Drawing.Rectangle();

        bool dstDown = false;

        float dstPX;
        float dstPY;
        float dstSX;
        float dstSY;

        float dstScaleF = 1;



        System.Drawing.Rectangle ScopeRect = new System.Drawing.Rectangle();
        Boolean IsScopeSelected = false;
        float ScopePX;
        float ScopePY;

        // 1x
        private void toolStripButton15_Click_2(object sender, EventArgs e)
        {
            dstScaleF = 1;
            resetDstBounds();
            cacheDstScale();
            pictureBox2.Refresh();
            toolStripButton15.ToolTipText =
                "当前缩放：" + (dstScaleF > 1 ? dstScaleF.ToString() : ("1/" + (1 / dstScaleF)));
        }

        // scope +
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            const float MAX_SCALE = 20;
            dstScaleF *= 2;
            if (dstScaleF > MAX_SCALE) dstScaleF = MAX_SCALE;
            resetDstBounds();
            cacheDstScale();
            pictureBox2.Refresh();
            toolStripButton15.ToolTipText =
                "当前缩放：" + (dstScaleF > 1 ? dstScaleF.ToString() : ("1/" + (1 / dstScaleF)));
        }

        // scope -
        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            const float MIN_SCALE = (1f / 20f);
            dstScaleF /= 2;
            if (dstScaleF < MIN_SCALE) dstScaleF = MIN_SCALE;
            resetDstBounds();
            cacheDstScale();
            pictureBox2.Refresh();
            toolStripButton15.ToolTipText =
                "当前缩放：" + (dstScaleF > 1 ? dstScaleF.ToString() : ("1/" + (1 / dstScaleF)));
        }

        // change image
        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            try
            {
                changeDstImageFromDir(dstSelectIndex);
                dstRect.X = getDstImage(dstSelectIndex).x;
                dstRect.Y = getDstImage(dstSelectIndex).y;
                dstRect.Width = getDstImage(dstSelectIndex).getWidth();
                dstRect.Height = getDstImage(dstSelectIndex).getHeight();
                pictureBox2.Refresh();
            }
            catch (Exception err) { }
        }
        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            try
            {
                changeDstImageFormSrc(dstSelectIndex);
                dstRect.X = getDstImage(dstSelectIndex).x;
                dstRect.Y = getDstImage(dstSelectIndex).y;
                dstRect.Width = getDstImage(dstSelectIndex).getWidth();
                dstRect.Height = getDstImage(dstSelectIndex).getHeight();
                pictureBox2.Refresh();
            }
            catch (Exception err) { }
        }

        //select image
        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = new Graphics(e.Graphics);

            renderDstImage(g);

            if (TotalSplit > 0)
            {
                renderDstSplit(g);
            }

            renderDstSelected(e);

        }

        private void renderDstSplit(Graphics g)
        {
            int sw = TotalSplit;
            int sh = TotalSplit;
            int tw = getDstWidth();
            int th = getDstHeight();

            g.setColor(0xffffffff);

            for (int x = 0; x < tw; x += sw)
            {
                g.drawLine(x * dstScaleF, 0, x * dstScaleF, th * dstScaleF);
            }

            for (int y = 0; y < th; y += sh)
            {
                g.drawLine(0, y * dstScaleF, tw * dstScaleF, y * dstScaleF);
            }
        }

        private void renderDstSelected(PaintEventArgs e)
        {
            System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
            System.Drawing.Brush brush = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0x80, 0xff, 0xff, 0xff)).Brush;

            float dstX = dstRect.X * dstScaleF;
            float dstY = dstRect.Y * dstScaleF;
            float dstW = dstRect.Width * dstScaleF;
            float dstH = dstRect.Height * dstScaleF;

            if (dstRect != null)
            {
                e.Graphics.FillRectangle(brush,
                    dstX, dstY, dstW, dstH);
                e.Graphics.DrawRectangle(pen,
                    dstX, dstY, dstW, dstH);
            }

            if (ScopeRect != null)
            {
                pen.Color = System.Drawing.Color.FromArgb(0xff, 0, 0xff, 0);
                e.Graphics.DrawRectangle(pen,
                    ScopeRect.X * dstScaleF,
                    ScopeRect.Y * dstScaleF,
                    ScopeRect.Width * dstScaleF,
                    ScopeRect.Height * dstScaleF);
            }
        }

        private Image selectDst(int tileID, bool scroll_to = false)
        {
            Image dst = getDstImage(tileID);
            if (dst != null)
            {
                dstSelected = dst;
                dstSelectIndex = tileID;
                dstRect = new System.Drawing.Rectangle();
                dstRect.X = dst.x;
                dstRect.Y = dst.y;
                dstRect.Width = dst.getWidth();
                dstRect.Height = dst.getHeight();

                toolStripStatusLabel1.Text =
                  "目标Tile：[" + dstSelectIndex + "]" +
                  " X=" + dstSelected.x +
                  " Y=" + dstSelected.y +
                  " W=" + dstSelected.getWidth() +
                  " H=" + dstSelected.getHeight() +
                  " Key=\"" + ((String)dstDataKeys[dstSelectIndex]) + "\""
                  ;

                if (scroll_to)
                {
                    int sy = (int)Math.Max((dst.y + dst.Height / 2) * dstScaleF - panelDstImage.Height / 2, panelDstImage.VerticalScroll.Minimum);
                    int sx = (int)Math.Max((dst.x + dst.Width / 2) * dstScaleF - panelDstImage.Width / 2, panelDstImage.HorizontalScroll.Minimum);
                    sy = Math.Min(sy, panelDstImage.VerticalScroll.Maximum);
                    sx = Math.Min(sx, panelDstImage.HorizontalScroll.Maximum);
                    panelDstImage.VerticalScroll.Value = sy;
                    panelDstImage.HorizontalScroll.Value = sx;
                }
            }
            return dst;
        }

        private void clearSelected()
        {
            ScopeRect.X = -1;
            ScopeRect.Y = -1;
            ScopeRect.Width = 1;
            ScopeRect.Height = 1;

            dstSelected = null;
            dstSelectIndex = -1;

            dstRect.X = -1;
            dstRect.Y = -1;
            dstRect.Width = 1;
            dstRect.Height = 1;

            dstRect.X = -1;
            dstRect.Y = -1;
            dstRect.Width = 1;
            dstRect.Height = 1;

            dstSelected = null;
            dstSelectIndex = -1;

        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            float mouseXi = (e.X / dstScaleF);
            float mouseYi = (e.Y / dstScaleF);

            dstDown = true;

            if (multiSelect.Checked)
            {
                if (e.Button == MouseButtons.Left)
                {
                    dstRect.X = -1;
                    dstRect.Y = -1;
                    dstRect.Width = 1;
                    dstRect.Height = 1;

                    dstSelected = null;
                    dstSelectIndex = -1;

                    if (Control.ModifierKeys == Keys.Shift || Control.ModifierKeys == Keys.Control)
                    {
                        dstPX = mouseXi;
                        dstPY = mouseYi;
                    }
                    else
                    {
                        dstSX = mouseXi;
                        dstSY = mouseYi;
                        dstPX = dstSX;
                        dstPY = dstSY;

                        if (ScopeRect.Contains((int)dstSX, (int)dstSY))
                        {
                            ScopePX = dstSX - ScopeRect.X;
                            ScopePY = dstSY - ScopeRect.Y;

                            IsScopeSelected = true;
                        }
                        else
                        {
                            IsScopeSelected = false;

                            ScopeRect.X = (int)dstSX;
                            ScopeRect.Y = (int)dstSY;
                            ScopeRect.Width = 1;
                            ScopeRect.Height = 1;

                            System.Drawing.Rectangle dst = new System.Drawing.Rectangle(0, 0, 1, 1);

                            for (int i = 0; i < getDstImageCount(); i++)
                            {
                                if (getDstImage(i) != null)
                                {
                                    dst.X = getDstImage(i).x;
                                    dst.Y = getDstImage(i).y;
                                    dst.Width = getDstImage(i).getWidth();
                                    dst.Height = getDstImage(i).getHeight();

                                    getDstImage(i).selected = ScopeRect.IntersectsWith(dst);

                                }
                            }
                        }
                    }

                }
            }
            else
            {

                ScopeRect.X = -1;
                ScopeRect.Y = -1;
                ScopeRect.Width = 1;
                ScopeRect.Height = 1;

                dstSelected = null;
                dstSelectIndex = -1;

                dstRect.X = -1;
                dstRect.Y = -1;
                dstRect.Width = 1;
                dstRect.Height = 1;

                dstPX = mouseXi;
                dstPY = mouseYi;

                System.Drawing.Rectangle dst = new System.Drawing.Rectangle();
                for (int i = 0; i < getDstImageCount(); i++)
                {
                    Image dstimg = getDstImage(i);
                    if (dstimg != null && dstimg.killed == false)
                    {
                        dst.X = dstimg.x;
                        dst.Y = dstimg.y;
                        dst.Width = dstimg.getWidth();
                        dst.Height = dstimg.getHeight();

                        if (dst.Contains((int)dstPX, (int)dstPY))
                        {
                            dstDown = true;
                            selectDst(i);

                            System.Drawing.Color color = dstimg.getPixel(
                                (int)(dstPX - dstimg.x),
                                (int)(dstPY - dstimg.y));


                            toolStripColor.BackColor = color;
                            toolStripColor.Text = "当前像素颜色=\"" + color + "\"";

                            if (e.Button == MouseButtons.Left)
                            {
                            }
                            else if (e.Button == MouseButtons.Right)
                            {
                                clipMenu.Show(pictureBox2, e.X, e.Y);
                            }

                            break;
                        }
                    }
                }
            }

            RefreshDstMaskColor();


            pictureBox2.Refresh();


        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            dstDown = false;
            dstPX = (e.X / dstScaleF);
            dstPY = (e.Y / dstScaleF);

            RefreshDstMaskColor();

            pictureBox2.Refresh();
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (dstDown)
            {
                bool ignoreCollides = Control.ModifierKeys == Keys.Alt;

                if (multiSelect.Checked)
                {
                    if (IsScopeSelected)
                    {
                        int px = Util.getDirect(e.X / dstScaleF - dstPX);
                        int py = Util.getDirect(e.Y / dstScaleF - dstPY);
                        dstPX = (e.X / dstScaleF);
                        dstPY = (e.Y / dstScaleF);

                        //Console.WriteLine(" px="+px+" py="+py);

                        if (moveDstImages(px, 0, ignoreCollides))
                        {
                            ScopeRect.X += px;
                        }
                        if (moveDstImages(0, py, ignoreCollides))
                        {
                            ScopeRect.Y += py;
                        }
                    }
                    else
                    {
                        dstPX = (e.X / dstScaleF);
                        dstPY = (e.Y / dstScaleF);

                        ScopeRect.X = (int)Math.Min(dstSX, dstPX);
                        ScopeRect.Y = (int)Math.Min(dstSY, dstPY);
                        ScopeRect.Width = (int)((dstSX - dstPX == 0) ? 1 : Math.Abs(dstSX - dstPX));
                        ScopeRect.Height = (int)((dstSY - dstPY == 0) ? 1 : Math.Abs(dstSY - dstPY));

                        System.Drawing.Rectangle dst = new System.Drawing.Rectangle(0, 0, 1, 1);

                        for (int i = 0; i < getDstImageCount(); i++)
                        {
                            if (getDstImage(i) != null)
                            {
                                dst.X = getDstImage(i).x;
                                dst.Y = getDstImage(i).y;
                                dst.Width = getDstImage(i).getWidth();
                                dst.Height = getDstImage(i).getHeight();

                                getDstImage(i).selected = ScopeRect.IntersectsWith(dst);
                            }
                        }
                    }

                }
                else
                {
                    if (dstSelectIndex >= 0)
                    {
                        int px = Util.getDirect(e.X / dstScaleF - dstPX);
                        int py = Util.getDirect(e.Y / dstScaleF - dstPY);
                        dstPX = (e.X / dstScaleF);
                        dstPY = (e.Y / dstScaleF);

                        if (moveDstImage(dstSelectIndex, px, 0, ignoreCollides))
                        {
                            dstRect.X += px;
                        }
                        if (moveDstImage(dstSelectIndex, 0, py, ignoreCollides))
                        {
                            dstRect.Y += py;
                        }

                        toolStripStatusLabel1.Text =
                                       "目标Tile：[" + dstSelectIndex + "]" +
                                       " X=" + dstSelected.x +
                                       " Y=" + dstSelected.y +
                                       " W=" + dstSelected.getWidth() +
                                       " H=" + dstSelected.getHeight() +
                                       " Key=\"" + ((String)dstDataKeys[dstSelectIndex]) + "\""
                                       ;
                    }
                }

                RefreshDstMaskColor();

                pictureBox2.Refresh();
            }

        }

        private void pictureBox2_SizeChanged(object sender, EventArgs e)
        {
            RefreshStatus();
        }

        public void RefreshStatus()
        {
            int dstW = getDstWidth();
            int dstH = getDstHeight();
            int outW = Util.ccNextPOT(dstW);
            int outH = Util.ccNextPOT(dstH);
            int pct = (int)(100f * GetDstWeight() / ((float)(outW * outH)));
            toolStripStatusLabel5.Text =
                " SIZE=[" + dstW + "," + dstH + "][" + outW + "," + outH + "] 利用率:" + pct + "%";
        }

        // del image
        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            try
            {
                if (multiSelect.Checked)
                {
                    if (MessageBox.Show("确认删除？", "警告", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        for (int i = 0; i < getDstImageCount(); i++)
                        {
                            if (getDstImage(i) != null && getDstImage(i).selected)
                            {
                                delDstImage(i);
                            }
                        }

                        dstRect.X = -1;
                        dstRect.Y = -1;
                        dstRect.Width = 1;
                        dstRect.Height = 1;
                        dstSelectIndex = -1;
                        pictureBox2.Refresh();
                    }
                }
                else
                {
                    if (MessageBox.Show("确认删除？ Tile = " + dstSelectIndex, "警告", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        delDstImage(dstSelectIndex);
                        dstRect.X = -1;
                        dstRect.Y = -1;
                        dstRect.Width = 1;
                        dstRect.Height = 1;
                        dstSelectIndex = -1;
                        pictureBox2.Refresh();
                    }
                }

            }
            catch (Exception err) { }
        }


        private void 查找ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string txt = G2DTextDialog.Show("0", "查找图块");
            int tileID = 0;
            if (int.TryParse(txt, out tileID))
            {
                Image img = selectDst(tileID, true);
                if (img == null)
                {
                    MessageBox.Show(tileID + " 不存在!");
                }
                pictureBox2.Refresh();
            }
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScopeRect.X = 0;
            ScopeRect.Y = 0;
            ScopeRect.Width = pictureBox2.Width;
            ScopeRect.Height = pictureBox2.Height;

            for (int i = 0; i < getDstImageCount(); i++)
            {
                if (getDstImage(i) != null)
                {
                    getDstImage(i).selected = true;
                }
            }

            pictureBox2.Refresh();
        }

        private void 从目录替换ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton9_Click(null, null);
        }

        private void 从左边替换ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton14_Click(null, null);
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton12_Click(null, null);
        }

        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                String name = (String)dstDataKeys[dstSelectIndex];
                TextDialog nameDialog = new TextDialog(name);
                if (nameDialog.ShowDialog() == DialogResult.OK)
                {
                    dstDataKeys[dstSelectIndex] = nameDialog.getText();
                    pictureBox2.Refresh();
                }
            }
            catch (Exception err) { }
        }

        // 清理透明色
        private void 清理透明色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageProcessDialog ipd = new ImageProcessDialog(this, dstImages, false);

            if (ipd.ShowDialog() == DialogResult.OK)
            {
                changeImage();

                pictureBox2.Refresh();
            }
        }



        //-------------------------------------------------------------------------------------------------------------------------------------

        public MapForm createMapForm(String name)
        {
            if (dstImages.Count < 1)
            {
                MessageBox.Show("Tile容量不能为0！");
                return null;
            }

            //for (int i = 0; i < dstImages.Count; i++)
            //{
            //    if (((Image)dstImages[i]) == null || ((Image)dstImages[i]).killed) continue;
            //    if (((Image)dstImages[i]).getWidth() != CellW ||
            //        ((Image)dstImages[i]).getHeight() != CellH)
            //    {
            //        MessageBox.Show("地图的Tile大小必须相等！");
            //        return null;
            //    }
            //}

            MapForm ret = new MapForm(name, CellW, CellH, this);
            return ret;
        }

        public SpriteXForm createSpriteExForm(String name)
        {
            //             for (int i = 0; i < dstImages.Count; i++)
            //             {
            //                 if (((Image)dstImages[i]) == null) continue;
            //             }
            SpriteXForm ret = new SpriteXForm(name, this);
            return ret;
        }
        public SpriteForm createSpriteForm(String name)
        {
            //             if (dstImages.Count < 1)
            //             {
            //                 MessageBox.Show("Tile容量不能为0！");
            //                 return null;
            //             }
            //             for (int i = 0; i < dstImages.Count; i++)
            //             {
            //                 if (((Image)dstImages[i]) == null) continue;
            //                 //if (((Image)dstImages[i]).getWidth() != CellW ||
            //                 //    ((Image)dstImages[i]).getHeight() != CellH)
            //                 //{
            //                 //    MessageBox.Show("地图的Tile大小必须相等！");
            //                 //    return null;
            //                 //}
            //             }

            SpriteForm ret = new SpriteForm(name, this);
            return ret;
        }

        //change color
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = true;
            MyDialog.ShowHelp = true;
            MyDialog.Color = pictureBox1.BackColor;
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.BackColor = MyDialog.Color;
                pictureBox1.BackColor = MyDialog.Color;
                toolStripButton10.BackColor = MyDialog.Color;
            }
        }
        private void BtnSelectKeyColor_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = true;
            MyDialog.ShowHelp = true;
            MyDialog.Color = BtnSelectKeyColor.BackColor;
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                BtnSelectKeyColor.BackColor = MyDialog.Color;
            }
        }
        private void BtnSelectTileIDColor_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = true;
            MyDialog.ShowHelp = true;
            MyDialog.Color = BtnSelectTileIDColor.BackColor;
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                BtnSelectTileIDColor.BackColor = MyDialog.Color;
            }
        }

        //
        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            pictureBox2.Refresh();
        }

        private void ImagesForm_TextChanged(object sender, EventArgs e)
        {
            this.id = this.Text;
        }

        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            pictureBox2.Refresh();
        }
        private void toolStripButton15_Click_1(object sender, EventArgs e)
        {
            pictureBox2.Refresh();
        }
        private void 添加切片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (toolStripButton2.Checked)
            {
                addDstImage();
            }
            if (toolStripButton3.Checked)
            {
                addDstImages();
            }
        }

        private void 添加多张图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addDirImages();
        }

        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void 导出图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // String name = (String)dstDataKeys[dstSelectIndex];
                Image image = (Image)dstImages[dstSelectIndex];
                //TextDialog nameDialog = new TextDialog(name);
                //if (nameDialog.ShowDialog() == DialogResult.OK)
                //{
                //    dstDataKeys[dstSelectIndex] = nameDialog.getText();
                //    pictureBox2.Refresh();
                //}
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.DefaultExt = ".png";
                sfd.AddExtension = true;
                sfd.Filter = "PNG file (*.png)|*.png";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    image.Save(sfd.FileName, CellGameEdit.Config.Default.EnablePremultiplyAlpha, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            catch (Exception err) { }
        }


        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnUpAllImage_Click(object sender, EventArgs e)
        {
            clearSelected();
            moveAllImages(0, -1);
            RefreshDstMaskColor();
            pictureBox2.Refresh();
        }

        private void btnLeftAllImage_Click(object sender, EventArgs e)
        {
            clearSelected();
            moveAllImages(-1, 0);
            RefreshDstMaskColor();
            pictureBox2.Refresh();
        }


        //------------------------------------------------------------------------------------------------
        // custom output menu

        private void nAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chk_output_group.Enabled = !chk_custom_output.Checked;
            chk_output_tile.Enabled = !chk_custom_output.Checked;
        }

        private void output_type_click(object sender, EventArgs e)
        {
        }

        private void output_type_changed(object sender, EventArgs e)
        {

        }

        private void chk_custom_filetype_Click(object sender, EventArgs e)
        {

            chk_output_file_bmp.Enabled = !chk_custom_filetype.Checked;
            chk_output_file_jpg.Enabled = !chk_custom_filetype.Checked;
            chk_output_file_png.Enabled = !chk_custom_filetype.Checked;
        }

        private void chk_output_file_png_Click(object sender, EventArgs e)
        {
            chk_output_file_bmp.Checked = sender == chk_output_file_bmp;
            chk_output_file_jpg.Checked = sender == chk_output_file_jpg;
            chk_output_file_png.Checked = sender == chk_output_file_png;
        }

        //------------------------------------------------------------------------------------------------

        class ImagesCompareY : IComparer
        {

            public int Compare(Object a, Object b)
            {
                if (a != null && b != null)
                {
                    return ((Image)a).y - ((Image)b).y;
                }
                return -1;
            }
        }

        private void 附加数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder(this.append_data);

            DataEdit dataedit = new DataEdit(sb);

            dataedit.ShowDialog(this);

            this.append_data = sb.ToString();
        }

        private void btnChangeDstToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*";
                openFileDialog1.InitialDirectory = System.IO.Path.GetDirectoryName(Config.Default.LastImageOpenDir);

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    is_change_image = true;

                    Image changed = Image.createImage(openFileDialog1.FileName);
                    System.Drawing.Rectangle ms = getMinSize();
                    if (changed.getWidth() < ms.Width || changed.getHeight() < ms.Height)
                    {
                        MessageBox.Show("图片太小，不能覆盖所有! ");
                        return;
                    }

                    for (int i = 0; i < dstImages.Count; i++)
                    {
                        Image img = (Image)dstImages[i];
                        if (!img.killed)
                        {
                            Image clip = changed.subImage(img.x, img.y, img.getWidth(), img.getHeight());
                            if (clip != null)
                            {
                                img.changeDimg(clip.dimg);
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(this.id + " : " + err.StackTrace + "  at  " + err.Message);
                MessageBox.Show(err.Message);
            }


        }

        private void btnOutputDstToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.DefaultExt = ".png";
                sfd.AddExtension = true;
                sfd.Filter = "PNG file (*.png)|*.png";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    outputAllAsGrop(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            catch (Exception err) { }
        }

        private void toolImageComvert_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder(this.image_convert_script_file);

            DataEdit dataedit = new DataEdit(sb);
            dataedit.Text = "图片转换脚本";
            dataedit.ShowDialog(this);

            this.image_convert_script_file = sb.ToString();
        }

        private void comboImageConvert_DropDown(object sender, EventArgs e)
        {
            comboImageConvert.Items.Clear();
            String[] scriptFiles = Form1.getImageConvertScriptList();
            comboImageConvert.Items.AddRange(scriptFiles);
        }

        private void comboImageConvert_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                image_convert_script_file = comboImageConvert.Text;
            }
            catch (Exception err)
            {
            }
        }

        private void comboImageConvert_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                image_convert_script_file = comboImageConvert.Text;
            }
            catch (Exception err)
            {
            }
        }

        /// <summary>
        /// 检测图片使用度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkTileUsed_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkTileUsed.Checked)
            {
                for (int t = getDstImageCount() - 1; t >= 0; --t)
                {
                    Image tile = getDstImage(t);
                    if (tile != null && !tile.killed)
                    {
                        tile.used = false;
                    }
                }

                var childs = ProjectForm.getInstance().getImangesFormChilds(this);

                foreach (Object ef in childs)
                {
                    if (ef.GetType() == typeof(SpriteForm))
                    {
                        SpriteForm sf = (SpriteForm)ef;
                        for (int t = getDstImageCount() - 1; t >= 0; --t)
                        {
                            Image tile = getDstImage(t);
                            if (tile != null && !tile.killed && !tile.used && sf.CheckTileUsed(t))
                            {
                                tile.used = true;
                            }
                        }
                    }
                    else if (ef.GetType() == typeof(MapForm))
                    {
                        MapForm mf = (MapForm)ef;
                        for (int t = getDstImageCount() - 1; t >= 0; --t)
                        {
                            Image tile = getDstImage(t);
                            if (tile != null && !tile.killed && !tile.used && mf.CheckTileUsed(t))
                            {
                                tile.used = true;
                            }
                        }
                    }
                }
            }
            pictureBox2.Refresh();



        }

        private void addAllTilesFromTilesFile(string file)
        {

            System.IO.FileStream images_fs = new System.IO.FileStream(file,
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
            try
            {
                if (ImageIO.IsNewHeader(images_fs))
                {
                    ImageIO.DecodeNewFromStream(images_fs, (index, data, err) =>
                    {
                        if (data != null)
                        {
                            addDst(new Image(data));
                        }
                        if (err != null)
                        {
                            Console.WriteLine(file + " : Tile[" + index + "] : at  " + err.Message);
                        }
                    });
                }
                else
                {
                    while (images_fs.Position < images_fs.Length)
                    {
                        try
                        {
                            System.IO.MemoryStream ms = ImageIO.decodePNGStream(images_fs);
                            System.Drawing.Image dimg = System.Drawing.Image.FromStream(ms);
                            ms.Close();
                            if (dimg != null)
                            {
                                addDst(new Image(dimg));
                            }
                            else
                            {
                                break;
                            }
                        }
                        catch (Exception err)
                        {
                            break;
                        }
                    }
                }
            }
            finally
            {
                images_fs.Close();
            }
            pictureBox2.Refresh();
        }

        private void resetAllTilesFromTilesFile(string file)
        {
            System.IO.FileStream images_fs = new System.IO.FileStream(file,
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
            try
            {
                if (ImageIO.IsNewHeader(images_fs))
                {
                    ImageIO.DecodeNewFromStream(images_fs, (index, data, err) =>
                    {
                        if (data != null)
                        {
                            changeDstImage(index, new Image(data));
                        }
                        if (err != null)
                        {
                            Console.WriteLine(file + " : Tile[" + index + "] : at  " + err.Message);
                        }
                    });
                }
                else
                {
                    int count = getDstImageCount();
                    for (int i = 0; i < count; i++)
                    {
                        try
                        {
                            System.IO.MemoryStream ms = ImageIO.decodePNGStream(images_fs);
                            System.Drawing.Image dimg = System.Drawing.Image.FromStream(ms);
                            ms.Close();
                            if (dimg != null)
                            {
                                changeDstImage(i, new Image(dimg));
                            }
                        }
                        catch (Exception err)
                        {
                            Console.WriteLine(this.id + " : Tile[" + i + "] : at  " + err.Message);
                        }
                    }
                }
            }
            finally
            {
                images_fs.Close();
            }
            pictureBox2.Refresh();
        }

        private void resetAllTilesFromDir(string dir)
        {
            foreach (var file in new System.IO.DirectoryInfo(dir).GetFiles())
            {
                if (file.Extension.ToLower() == ".png")
                {
                    try
                    {
                        int i;
                        string si = file.Name.Substring(0, file.Name.Length - file.Extension.Length);
                        if (int.TryParse(si, out i))
                        {
                            System.Drawing.Image dimg = System.Drawing.Image.FromFile(file.FullName);
                            if (dimg != null)
                            {
                                changeDstImage(i, new Image(dimg));
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine(this.id + " : Tile[" + file.Name + "] : at  " + err.Message);
                    }
                }
            }
            //             int count = getDstImageCount();
            //             for (int i = 0; i < count; i++)
            //             {
            //                 string path = dir + "\\" + i + ".png";
            //                 try
            //                 {
            //                     if (System.IO.File.Exists(path))
            //                     {
            //                         System.Drawing.Image dimg = System.Drawing.Image.FromFile(path);
            //                         if (dimg != null)
            //                         {
            //                             changeDstImage(i, new Image(dimg));
            //                         }
            //                     }
            //                 }
            //                 catch (Exception err)
            //                 {
            //                     Console.WriteLine(this.id + " : Tile[" + i + "] : at  " + err.Message);
            //                 }
            //             }

            pictureBox2.Refresh();
        }

        private void tilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "tiles files (*.tiles)|*.tiles|All files (*.*)|*.*";
                openFileDialog1.InitialDirectory = ProjectForm.workSpace;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    resetAllTilesFromTilesFile(openFileDialog1.FileName);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(this.id + " : " + err.StackTrace + "  at  " + err.Message);
            }

        }

        private void pngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.ShowNewFolderButton = false;
                fbd.SelectedPath = ProjectForm.workSpace;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    resetAllTilesFromDir(fbd.SelectedPath);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(this.id + " : " + err.StackTrace + "  at  " + err.Message);
            }
        }

        private void 导出所有图块ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.ShowNewFolderButton = true;
                fbd.SelectedPath = ProjectForm.workSpace;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    outputAllTilesDirect(fbd.SelectedPath, "png", System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(this.id + " : " + err.StackTrace + "  at  " + err.Message);
            }
        }
        private void 导入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "tiles files (*.tiles)|*.tiles|All files (*.*)|*.*";
                openFileDialog1.InitialDirectory = ProjectForm.workSpace;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    addAllTilesFromTilesFile(openFileDialog1.FileName);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(this.id + " : " + err.StackTrace + "  at  " + err.Message);
            }
        }

        private void 间隔图块ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string txt = G2DTextDialog.Show("1", "间隔图块像素");
                int space = 1;
                if (int.TryParse(txt, out space))
                {
                    spaceAllDstImages(space);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void toolStripMenuItem_COPY_ALL_Click(object sender, EventArgs e)
        {
            copyToClipboard();
        }

        private void toolStripMenuItem_PASTE_ALL_Click(object sender, EventArgs e)
        {
            pasteFromClipboard();
        }

        // ---------------------------------------------------------------------------------

        #region clip_picture

        private void tool_clip512_Click(object sender, EventArgs e)
        {
            TotalSplit = 512;
            RefreshDstMaskColor();
            refreshClipMenu();
            pictureBox2.Refresh();
        }
        private void tool_clip1024__Click(object sender, EventArgs e)
        {
            TotalSplit = 1024;
            RefreshDstMaskColor();
            refreshClipMenu();
            pictureBox2.Refresh();
        }
        private void tool_clip2048_Click(object sender, EventArgs e)
        {
            TotalSplit = 2048;
            RefreshDstMaskColor();
            pictureBox2.Refresh();
        }
        private void tool_clip4096_Click(object sender, EventArgs e)
        {
            TotalSplit = 4096;
            RefreshDstMaskColor();
            refreshClipMenu();
            pictureBox2.Refresh();
        }
        private void tool_clip8192_Click(object sender, EventArgs e)
        {
            TotalSplit = 8192;
            RefreshDstMaskColor();
            refreshClipMenu();
            pictureBox2.Refresh();
        }
        private void tool_ClipNA_Click(object sender, EventArgs e)
        {
            TotalSplit = 0;
            RefreshDstMaskColor();
            refreshClipMenu();
            pictureBox2.Refresh();
        }

        private void refreshClipMenu()
        {
            tool_ClipNA.Checked = false;
            tool_clip8192.Checked = false;
            tool_clip4096.Checked = false;
            tool_clip2048.Checked = false;
            tool_clip1024.Checked = false;
            tool_clip512.Checked = false;
            switch (TotalSplit)
            {
                case 0: tool_ClipNA.Checked = true; break;
                case 512: tool_clip512.Checked = true; break;
                case 1024: tool_clip1024.Checked = true; break;
                case 2048: tool_clip2048.Checked = true; break;
                case 4096: tool_clip4096.Checked = true; break;
                case 8192: tool_clip8192.Checked = true; break;
            }

        }
        #endregion


        private void 整理图集ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 按尺寸ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpitimizeDstTiles(TileOptimizerSet.SortStyle.WEIGHT);
        }

        private void 按宽ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpitimizeDstTiles(TileOptimizerSet.SortStyle.WIDTH);
        }

        private void 按高ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpitimizeDstTiles(TileOptimizerSet.SortStyle.HEIGHT);
        }


        private void btn_Pow2_Click(object sender, EventArgs e)
        {
            if (btn_Pow2.Checked)
            {
                btn_Quard2.Checked = false;
            }
        }
        private void btn_Quard2_Click(object sender, EventArgs e)
        {
            if (btn_Quard2.Checked)
            {
                btn_Pow2.Checked = false;
            }
        }


        public void OpitimizeDstTiles(TileOptimizerSet.SortStyle style)
        {
            TileOptimizerSet set = new TileOptimizerSet(dstImages);
            set.Opitimize(style);
            resetDstBounds();
            RefreshDstMaskColor();
            pictureBox2.Refresh();
        }


        // ---------------------------------------------------------------------------------

        public class TileOptimizerSet
        {
            public enum SortStyle
            {
                WIDTH,
                HEIGHT,
                WEIGHT,
            }

            private List<Image> WeightSorted = new List<Image>();
            private List<Image> WidthSorted = new List<Image>();
            private List<Image> HeightSorted = new List<Image>();
            private int mUsedWight;

            public int Count { get { return WeightSorted.Count; } }

            public TileOptimizerSet(List<Image> images)
            {
                mUsedWight = 0;
                foreach (object o in images)
                {
                    if (o != null)
                    {
                        Image img = o as Image;
                        if (img.killed == false)
                        {
                            mUsedWight += (img.getWidth() * img.getHeight());
                            WeightSorted.Add(img);
                            WidthSorted.Add(img);
                            HeightSorted.Add(img);
                        }
                    }
                }
                WeightSorted.Sort(new WeightSorter());
                WidthSorted.Sort(new WidthSorter());
                HeightSorted.Sort(new HeightSorter());
            }

            public Image PopMaxWeight()
            {
                if (WeightSorted.Count > 0)
                {
                    Image ret = WeightSorted[WeightSorted.Count - 1];
                    WeightSorted.Remove(ret);
                    WidthSorted.Remove(ret);
                    HeightSorted.Remove(ret);
                    return ret;
                }
                return null;
            }

            public Image PopMaxWidth()
            {
                if (WidthSorted.Count > 0)
                {
                    Image ret = WidthSorted[WidthSorted.Count - 1];
                    WeightSorted.Remove(ret);
                    WidthSorted.Remove(ret);
                    HeightSorted.Remove(ret);
                    return ret;
                }
                return null;
            }

            public Image PopMaxHeight()
            {
                if (HeightSorted.Count > 0)
                {
                    Image ret = HeightSorted[HeightSorted.Count - 1];
                    WeightSorted.Remove(ret);
                    WidthSorted.Remove(ret);
                    HeightSorted.Remove(ret);
                    return ret;
                }
                return null;
            }

            public void Opitimize(TileOptimizerSet.SortStyle style)
            {
                int pow = Util.ccNextPOT((int)Math.Sqrt(mUsedWight));
                Range range = new Range(pow);
                int count = this.Count;
                if (style == SortStyle.WEIGHT)
                {
                    for (int i = 0; i < count; i++)
                    {
                        range.addImage(PopMaxWeight());
                    }
                }
                else if (style == SortStyle.WIDTH)
                {
                    for (int i = 0; i < count; i++)
                    {
                        range.addImageW(PopMaxWidth());
                    }
                }
                else if (style == SortStyle.HEIGHT)
                {
                    for (int i = 0; i < count; i++)
                    {
                        range.addImageH(PopMaxHeight());
                    }
                }
            }

            class Range
            {
                readonly private List<Image> images = new List<Image>();

                readonly private int TotalSize = 0;

                public Range(int split)
                {
                    this.TotalSize = split;
                }

                public void addImage(Image img)
                {
                    int dw = TotalSize - img.getWidth();
                    int dh = TotalSize - img.getHeight();
                    for (int y = 0; y < dh; y++)
                    {
                        for (int x = 0; x < dw; x++)
                        {
                            img.x = x;
                            img.y = y;
                            Image dst = touch(img);
                            if (dst == null)
                            {
                                images.Add(img);
                                return;
                            }
                            else
                            {
                                x = dst.x + dst.getWidth();
                            }
                        }
                    }
                    img.x = 0;
                    img.y = 0;
                    images.Add(img);
                }

                public void addImageW(Image img)
                {
                    int dw = TotalSize - img.getWidth();
                    int dh = TotalSize * 2;
                    for (int y = 0; y < dh; y++)
                    {
                        for (int x = 0; x < dw; x++)
                        {
                            img.x = x;
                            img.y = y;
                            Image dst = touch(img);
                            if (dst == null)
                            {
                                images.Add(img);
                                return;
                            }
                            else
                            {
                                x = dst.x + dst.getWidth();
                            }
                        }
                    }
                    img.x = 0;
                    img.y = 0;
                    images.Add(img);
                }

                public void addImageH(Image img)
                {
                    int dw = TotalSize * 2;
                    int dh = TotalSize - img.getHeight();
                    for (int x = 0; x < dw; x++)
                    {
                        for (int y = 0; y < dh; y++)
                        {
                            img.x = x;
                            img.y = y;
                            Image dst = touch(img);
                            if (dst == null)
                            {
                                images.Add(img);
                                return;
                            }
                            else
                            {
                                y += dst.getHeight();
                            }
                        }
                    }
                    img.x = 0;
                    img.y = 0;
                    images.Add(img);
                }

                public Image touch(Image img)
                {
                    int count = images.Count;
                    for (int i = 0; i < count; ++i)
                    {
                        Image dst = images[i];
                        if (Util.intersect2DWH(
                            img.x, img.y, img.getWidth(), img.getHeight(),
                            dst.x, dst.y, dst.getWidth(), dst.getHeight()))
                        {
                            return dst;
                        }
                    }
                    return null;
                }
            }

            class WeightSorter : IComparer<Image>
            {
                public int Compare(Image x, Image y)
                {
                    int w1 = x.getWidth() * x.getHeight();
                    int w2 = y.getWidth() * y.getHeight();
                    return w1 - w2;
                }
            }
            class WidthSorter : IComparer<Image>
            {
                public int Compare(Image x, Image y)
                {
                    return x.getWidth() - y.getWidth();
                }
            }
            class HeightSorter : IComparer<Image>
            {
                public int Compare(Image x, Image y)
                {
                    return x.getHeight() - y.getHeight();
                }
            }

        }

    }
}