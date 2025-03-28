﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Security.Permissions;

using javax.microedition.lcdui;

namespace CellGameEdit.PM
{


    [Serializable]
    public partial class MapForm : Form, ISerializable, IEditForm, IEditFormVisible
    {

        public BufferdMiniMap MiniView;

        public string id { get; private set; }
        public String superName { get; private set; }
        public ImagesForm super { get; private set; }
        public void setID(string id, ProjectForm proj)
        {
            this.id = id;
        }

        public int CellW { get; private set; }
        public int CellH { get; private set; }

        private int KeyX = 0;
        private int KeyY = 0;

		private MapLayers layers;

		private string append_data = "";

        //Hashtable AnimTable;
        Hashtable AnimIndexTable;
        Hashtable AnimFlipTable;
        List<javax.microedition.lcdui.Image> Tiles;
        List<string> TileKeys;

        //private int XCount;
		//private int YCount;


        //---------------------------------------------------------
        int flipIndex = 0;
        int tagIndex = 0;
        int srcIndex = 0;
        int srcIndexR = 0;
        float srcScale = 1;

        System.Drawing.Rectangle srcRect;
        System.Drawing.Rectangle srcRectR;
        //----------------------------------------------------------
        int srcPX = 0;
        int srcPY = 0;
        int srcQX = 0;
        int srcQY = 0;
        System.Drawing.Rectangle srcRects;
        ArrayList srcTilesIndexBrush;
        ArrayList srcTilesOXBrush;
        ArrayList srcTilesOYBrush;

        //----------------------------------------------------------

        System.Drawing.Rectangle MapLoc = new System.Drawing.Rectangle(0,0,100,100);

        Boolean dstIsDown = false;
        int dstPX = 0;
        int dstPY = 0;
        int dstQX = 0;
        int dstQY = 0;
        int dstBX = 0;
        int dstBY = 0;
        System.Drawing.Rectangle dstRect;

        public static ImagesForm clipSuper;
        //public static int[][] dstClipTile;
        //public static int[][] dstClipTag;
        //public static int[][] dstClipAnim;
        //public static int[][] dstClipFlip;
		public static MapLayer dstClipLayer = new MapLayer(0,0);

        int animCount = 0;
        int curAnimate = -1;
        int curTime =0;
        int curFrame = -1;

        public MapForm(String name,int cellw,int cellh,ImagesForm images)
        {
            InitializeComponent();
            mapTerrains1.init(this) ;

            id = name; this.Text = id;
			super = images;

            layers = new MapLayers(
                Util.cycMod(images.getDstWidth(), cellw),
                Util.cycMod(images.getDstHeight(), cellh));
			layers.addLayer();

            CellW = cellw;
            CellH = cellh;
            numericUpDown4.Value = CellW;
            numericUpDown5.Value = CellH;

            srcRectR = new System.Drawing.Rectangle(0, 0, 0, 0);
            srcRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            dstRect = new System.Drawing.Rectangle(0, 0, 0, 0);

            Tiles = super.dstImages;
            TileKeys = super.dstDataKeys;

            for (int i = 0; i < getTileCount(); i++)
            {
                if (getTileImage(i)!=null)
                {
                    srcIndex = i;
                    //srcImage = getTileImage(i);
                    srcRect.X = getTileImage(i).x;
                    srcRect.Y = getTileImage(i).y;
                    srcRect.Width = getTileImage(i).getWidth();
                    srcRect.Height = getTileImage(i).getHeight();

                    srcIndexR = i;
                    srcRectR.X = getTileImage(i).x;
                    srcRectR.Y = getTileImage(i).y;
                    srcRectR.Width = getTileImage(i).getWidth();
                    srcRectR.Height = getTileImage(i).getHeight();
                    break;
                }
            }
			/*
            XCount = 20;
            YCount = 20;
            MatrixTile = new int[YCount][];
            MatrixTag = new int[YCount][];
            MatrixAnim = new int[YCount][];
            MatrixFlip = new int[YCount][];
            for (int i = 0; i < YCount;i++ )
            {
                MatrixTile[i] = new int[XCount];
                MatrixTag[i] = new int[XCount];
                MatrixAnim[i] = new int[XCount];
                MatrixFlip[i] = new int[XCount];
                //for (int j = 0; j < XCount; j++)
                //{
                //    MatrixAnim[i][j] = 0;
                //}
            }
			*/

			MapLoc.Width = CellW * layers.XCount;
			MapLoc.Height = CellH * layers.YCount;
			numericUpDown2.Value = layers.XCount;
			numericUpDown3.Value = layers.YCount;
            
			//dstClipLayer

			/*
            dstClipTile = new int[1][];
            dstClipTag = new int[1][];
            dstClipAnim = new int[1][];
            dstClipFlip = new int[1][];

            dstClipTile[0] = new int[1];
            dstClipTag[0] = new int[1];
            dstClipAnim[0] = new int[1];
            dstClipFlip[0] = new int[1];
			*/
            //AnimTable = new Hashtable();
            //Animates = new ArrayList();
            AnimIndexTable = new Hashtable();
            AnimFlipTable = new Hashtable();
           
            ArrayList frames = new ArrayList();
            frames.Add(0);
            ArrayList flips = new ArrayList();
            flips.Add(0);
            ListViewItem item = new ListViewItem(new String[] { animCount.ToString("(空)"), "1"});
            listView1.Items.Add(item);
            AnimIndexTable.Add(item, frames);
            AnimFlipTable.Add(item, flips);
           
            animCount++;

            pictureBox3.Width = CellW;
            pictureBox3.Height = CellH;

            pictureBox4.Left = pictureBox3.Location.X + 3 + CellW;
            pictureBox4.Width = CellW;
            pictureBox4.Height = CellH;

            trackBar1.Maximum = 0;

        }
        public void LoadOver(ProjectForm prj)
        {
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected MapForm(SerializationInfo info, StreamingContext context)
        {
            try
			{
				InitializeComponent();
				mapTerrains1.init(this);

				id = (String)info.GetValue("id", typeof(String));
				this.Text = id;
				/*
                if (!ProjectForm.IsCopy)
                {
                    super = (ImagesForm)info.GetValue("super", typeof(ImagesForm));
                }

                if (super == null)
                {
                    superName = (String)info.GetValue("SuperName", typeof(String));
                    super = ProjectForm.getInstance().getImagesFormByName(superName);
                }
				*/
				superName = (String)info.GetValue("SuperName", typeof(String));

				CellW = (int)info.GetValue("CellW", typeof(int));
				CellH = (int)info.GetValue("CellH", typeof(int));
				

				srcRectR = new System.Drawing.Rectangle(0, 0, 0, 0);
				srcRect = new System.Drawing.Rectangle(0, 0, 0, 0);
				dstRect = new System.Drawing.Rectangle(0, 0, 0, 0);


				

				try
				{
					layers = (MapLayers)info.GetValue("MapLayers", typeof(MapLayers));

				}
				catch (Exception err)
				{
				}
				try
				{
					if (layers == null)
					{
						int[][] MatrixTile = (int[][])info.GetValue("MatrixTile", typeof(int[][]));
						int[][] MatrixTag = (int[][])info.GetValue("MatrixTag", typeof(int[][]));
						int[][] MatrixAnim = (int[][])info.GetValue("MatrixAnim", typeof(int[][]));
						int[][] MatrixFlip = (int[][])info.GetValue("MatrixFlip", typeof(int[][]));
						int XCount = MatrixTile[0].Length;
						int YCount = MatrixTile.Length;

						MapLayer layer = new MapLayer(XCount, YCount);
						layer.MatrixTile = MatrixTile;
						layer.MatrixTag = MatrixTag;
						layer.MatrixAnim = MatrixAnim;
						layer.MatrixFlip = MatrixFlip;

						layers = new MapLayers(XCount, YCount);
						layers.addLayer(layer);
					}
				}
				catch (Exception err)
				{
				}

				{ 
					
				}

				MapLoc.Width = CellW * layers.XCount;
				MapLoc.Height = CellH * layers.YCount;
				numericUpDown2.Value = layers.XCount;
				numericUpDown3.Value = layers.YCount;


				numericUpDown4.Value = CellW;
				numericUpDown5.Value = CellH;
				pictureBox3.Width = CellW;
				pictureBox3.Height = CellH;

				pictureBox4.Left = pictureBox3.Location.X + 3 + CellW;

				pictureBox4.Width = CellW;
				pictureBox4.Height = CellH;
				/*
                dstClipTile = new int[1][];
                dstClipTag = new int[1][];
                dstClipAnim = new int[1][];
                dstClipFlip = new int[1][];

                dstClipTile[0] = new int[1];
                dstClipTag[0] = new int[1];
                dstClipAnim[0] = new int[1];
                dstClipFlip[0] = new int[1];

                */

				AnimIndexTable = new Hashtable();
				AnimFlipTable = new Hashtable();

				ArrayList animK = (ArrayList)info.GetValue("animK", typeof(ArrayList));
				ArrayList animV = (ArrayList)info.GetValue("animV", typeof(ArrayList));
				ArrayList animF = (ArrayList)info.GetValue("animF", typeof(ArrayList));

				//Console.WriteLine("KCount=" + animK.Count);

				for (int i = 0; i < animK.Count; i++)
				{
					ArrayList v = (ArrayList)animV[i];
					ArrayList f = (ArrayList)animF[i];
					ListViewItem k = new ListViewItem(new String[] { animK[i].ToString(), v.Count.ToString() });
					listView1.Items.Add(k);
					AnimIndexTable.Add(k, v);
					AnimFlipTable.Add(k, f);

				}

				animCount = (int)info.GetValue("animCount", typeof(int));

				trackBar1.Maximum = 0;



				try
				{
					this.IsTerrain.Checked = (Boolean)info.GetValue("IsTerrain", typeof(Boolean));
				}
				catch (Exception err) { }

				try
				{
					this.append_data = info.GetString("AppendData");
				}
				catch (Exception err)
				{
					this.append_data = "";
				}

			}
            catch (Exception err)
            {
                MessageBox.Show(err.StackTrace + "  at  " +err.Message);
            }
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Console.WriteLine("Serializ Map : " + id);

            try{
                info.AddValue("id",id);
				/*
                if (!ProjectForm.IsCopy)
                {
                    info.AddValue("super", super);
                }
				*/
                info.AddValue("SuperName", super.id);

                info.AddValue("CellW",CellW);
                info.AddValue("CellH",CellH);
                
				/*
                info.AddValue("MatrixTile",MatrixTile);
                info.AddValue("MatrixTag", MatrixTag);
                info.AddValue("MatrixAnim", MatrixAnim);
                info.AddValue("MatrixFlip", MatrixFlip);
				*/
				info.AddValue("MapLayers", layers);


                //info.AddValue("AnimTable", AnimTable);
                ArrayList animK = new ArrayList();
                ArrayList animV = new ArrayList();
                ArrayList animF = new ArrayList();
                
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    animK.Add(listView1.Items[i].Text);
                    animV.Add(getFrame(i));
                    animF.Add(getFrameFlip(i));
                }
                info.AddValue("animK", animK);
                info.AddValue("animV", animV);
                info.AddValue("animF", animF);
                //Console.WriteLine("NCount=" + AnimTable.Count);

                info.AddValue("animCount", animCount);

                info.AddValue("IsTerrain", IsTerrain.Checked);


				info.AddValue("AppendData", append_data);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.StackTrace + "  at  " +err.Message);
            }
        }

        public String getID()
        {
            return id;
        }

        public Form getForm()
        {
            return this;
        }

        public void ChangeSuper(List<ImagesForm> images)
        {
            Hashtable imagesHT = new Hashtable();

            for (int i = 0; i < images.Count; i++)
            {
                imagesHT.Add(((ImagesForm)images[i]).id, images[i]);
            }

            if (imagesHT.ContainsKey(superName))
            {
                ChangeSuper((ImagesForm)imagesHT[superName]);
                Console.WriteLine("Map ChangeImages : " + superName);
            }
        }

        public void ChangeSuper(ImagesForm super)
        {
            this.super = super;
            this.Tiles = super.dstImages;

            srcRectR = new System.Drawing.Rectangle(0, 0, 0, 0);
            srcRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            dstRect = new System.Drawing.Rectangle(0, 0, 0, 0);

			Tiles = super.dstImages;
			TileKeys = super.dstDataKeys;
            for (int i = 0; i < getTileCount(); i++)
            {
                if (getTileImage(i) != null)
                {
                    srcIndex = i;
                    //srcImage = getTileImage(i);
                    srcRect.X = getTileImage(i).x;
                    srcRect.Y = getTileImage(i).y;
                    srcRect.Width = getTileImage(i).getWidth();
                    srcRect.Height = getTileImage(i).getHeight();

                    srcIndexR = i;
                    srcRectR.X = getTileImage(i).x;
                    srcRectR.Y = getTileImage(i).y;
                    srcRectR.Width = getTileImage(i).getWidth();
                    srcRectR.Height = getTileImage(i).getHeight();

                    break;
                }
            }
		
            pictureBox3.Width = CellW;
            pictureBox3.Height = CellH;

            pictureBox4.Left = pictureBox3.Location.X + 3 + CellW;

            pictureBox4.Width = CellW;
            pictureBox4.Height = CellH;

            //XCount = MatrixTile[0].Length;
            //YCount = MatrixTile.Length;

            MapLoc.Width = CellW * layers.XCount;
			MapLoc.Height = CellH * layers.YCount;
			numericUpDown2.Value = layers.XCount;
			numericUpDown3.Value = layers.YCount;

			
        }

        public void ChangeSuperImageSize(ImagesForm src, List<ImageChange> events)
        {

        }



        private void initOutput(
			int[][][] OutputTileMatrix,
			int[][][] OutputFlipMatrix,
			int[][][] OutputFlagMatrix)
		{
            //init
			for (int L = 0; L < layers.getCount(); L++ )
			{
				MapLayer layer = layers.getLayer(L);

				OutputTileMatrix[L] = new int[layers.YCount][];
				OutputFlipMatrix[L] = new int[layers.YCount][];
				OutputFlagMatrix[L] = new int[layers.YCount][];

				for (int y = 0; y < layers.YCount; y++)
				{
					OutputTileMatrix[L][y] = new int[layers.XCount];
					OutputFlipMatrix[L][y] = new int[layers.XCount];
					OutputFlagMatrix[L][y] = new int[layers.XCount];

					for (int x = 0; x < layers.XCount; x++)
					{
						OutputTileMatrix[L][y][x] = (int)layer.MatrixTile[y][x];
						OutputFlipMatrix[L][y][x] = (int)layer.MatrixFlip[y][x];
						OutputFlagMatrix[L][y][x] = (int)layer.MatrixTag[y][x];
					}
				}
			}
		}

        public void OutputCustom(int index, String script, System.IO.StringWriter output)
        {
            lock (this)
            {
                try
				{
					int[][][] OutputTileMatrix = new int[layers.getCount()][][];
					int[][][] OutputFlipMatrix = new int[layers.getCount()][][];
					int[][][] OutputFlagMatrix = new int[layers.getCount()][][];

					//Animates OutputAnimates  = new Animates();
					//initOutput(OutputTileMatrix, OutputFlagMatrix, OutputAnimates);

					initOutput(OutputTileMatrix, OutputFlipMatrix, OutputFlagMatrix);

                    String map = Util.getFullTrunkScript(script, SC._MAP, SC._END_MAP);

                    bool fix = false;


                    // cd parts
					int[][] cds = new int[][]
					{
						 new int[]{1,0x00000000, 0, 0, CellW, CellH, CellW, CellH},//null
						 new int[]{1,0x00000001, 0, 0, CellW, CellH, CellW, CellH},//full

						 new int[]{1,0x00000002, 0,       0,       CellW,   1,       CellW ,   CellH},//
						 new int[]{1,0x00000004, 0,       CellH-1, CellW,   1,       CellW ,   CellH},//
						 new int[]{1,0x00000008, 0,       0,       1,       CellH,   CellW ,   CellH},//
						 new int[]{1,0x00000010, CellW-1, 0,       1,       CellH,   CellW ,   CellH},//

						 new int[]{2,0x00000020, 0,       0,       CellW,   CellH,   CellW-1 , CellH-1},//
						 new int[]{2,0x00000040, CellW-1, 0,       CellW,   CellH,   0 ,       CellH-1},//
					};

                    do
                    {
                        String[] cdParts = new string[8];
                        for (int i = 0; i < 8; i++)
                        {
                            string TYPE = cds[i][0] == 1 ? "rect" : "line";
                            string MASK = cds[i][1].ToString();
                            string X1 = cds[i][2].ToString();
                            string Y1 = cds[i][3].ToString();
                            string W = cds[i][4].ToString();
                            string H = cds[i][5].ToString();
                            string X2 = cds[i][6].ToString();
                            string Y2 = cds[i][7].ToString();
                            cdParts[i] = Util.replaceKeywordsScript(map, SC._CD_PART, SC._END_CD_PART,
                                new string[] { SC.INDEX, SC.TYPE, SC.MASK, SC.X1, SC.Y1, SC.W, SC.H, SC.X2, SC.Y2 },
                                new string[] { i.ToString(), TYPE, MASK, X1, Y1, W, H, X2, Y2 }
                                );
                        }
                        string temp = Util.replaceSubTrunksScript(map, SC._CD_PART, SC._END_CD_PART, cdParts);
                        if (temp == null)
                        {
                            fix = false;
                        }
                        else
                        {
                            fix = true;
                            map = temp;
                        }
                    } while (fix);


					//////////////////////////////////////////////////////////////////////////////
					// map layers
					do
					{
						String[] slayers = new String[layers.getCount()];
						for (int L = 0; L < layers.getCount(); L++)
						{
							MapLayer layer = layers.getLayer(L);

							// tile matrix
							String senceMatrix = Util.toNumberArray2D<int>(ref (OutputTileMatrix[L]));
							// flip matrix
							String flipMatrix = Util.toNumberArray2D<int>(ref (OutputFlipMatrix[L]));
							// cd matrix
							String cdMatrix = Util.toNumberArray2D<int>(ref (OutputFlagMatrix[L]));

							slayers[L] = Util.replaceKeywordsScript(map, SC._LAYER, SC._END_LAYER,
								new string[] { SC.INDEX, SC.TILE_MATRIX, SC.FLIP_MATRIX, SC.FLAG_MATRIX },
								new string[] { L.ToString(), senceMatrix, flipMatrix, cdMatrix }
								);
						}
						string temp = Util.replaceSubTrunksScript(map, SC._LAYER, SC._END_LAYER, slayers);
						if (temp == null)
						{
							fix = false;
						}
						else
						{
							fix = true;
							map = temp;
						}
					} while (fix);
					//////////////////////////////////////////////////////////////////////////////
                   

					string[] adata = Util.toStringMultiLine(append_data);
					string APPEND_DATA = Util.toStringArray1D(ref adata);

                    map = Util.replaceKeywordsScript(map, SC._MAP, SC._END_MAP,
                        new string[] { 
						SC.NAME, 
						 SC.MAP_INDEX,
						 SC.IMAGES_NAME,
						 SC.CELL_W,
						 SC.CELL_H, 
						 SC.X_COUNT,
						 SC.Y_COUNT,
						 SC.LAYER_COUNT,
						 //SC.TILE_MATRIX,
						 //SC.FLAG_MATRIX,
						 //SC.SCENE_PART_COUNT,
						 //SC.SCENE_FRAME_COUNT,
						 SC.CD_PART_COUNT,
						 SC.APPEND_DATA
					},
                        new string[] { 
						this.id, 
						index.ToString(),
						super.id,
						CellW.ToString(),
						CellH.ToString(),
						layers.XCount.ToString(),
						layers.YCount.ToString(),
						layers.getCount().ToString(),
						//senceMatrix,
						//cdMatrix,
						//OutputAnimates.subGetCount().ToString(),
						//OutputAnimates.frameGetCount().ToString(),
						"8",
						APPEND_DATA
						}
                        );

                    output.WriteLine(map);
                    //Console.WriteLine(map);
                }
                catch (Exception err) { Console.WriteLine(this.id + " : " + err.StackTrace + "  at  " + err.Message); }

            }
        }

        private void MapForm_Load(object sender, EventArgs e)
		{
			syncMapLayer();
			refreshAnimateList();
            timer1.Start();
        }
        private void MapForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }
        private void MapForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
        private void MapForm_TextChanged(object sender, EventArgs e)
        {
            this.id = this.Text;
        }

		/////////////////////////////////////////////////////////////////////////////////////////////////////////
		public MapLayer getCurLayer()
		{
			return layers.getLayer(layer_index);
		}

        public Boolean isTerrain() { return this.IsTerrain.Checked; }

        public Image getTileImage(MapLayer layer, int x, int y) {
			return (((Image)(Tiles[layer.MatrixTile[y][x]])));
        }
        public Image getTileImage(int index)
        {
            return (((Image)(Tiles[index])));
        }
		public String getTileKey(MapLayer layer, int x, int y)
        {
			return (((String)(TileKeys[layer.MatrixTile[y][x]])));
        }
        public String getTileKey(int index)
        {
            return (((String)(TileKeys[index])));
        }

        public int getTileCount()
        {
            return Tiles.Count;
        }

        public int getWidth()
        {
            return layers.XCount * this.CellW;
        }
        public int getHeight()
        {
			return layers.YCount * this.CellH;
        }

        /// <summary>
        /// 检测该TILE是否被使用
        /// </summary>
        /// <param name="tileID"></param>
        /// <returns></returns>
        public bool CheckTileUsed(int tileID)
        {
            for (int L = 0; L < layers.getCount(); L++)
            {
                MapLayer layer = layers.getLayer(L);

                for (int by = 0; by < layers.YCount; by++)
                {
                    for (int bx = 0; bx < layers.XCount; bx++)
                    {
                        int tilei = layer.MatrixTile[by][bx];
                        if (tileID == tilei)
                        {
                            return true;
                        }
                    }
                }
            }
            
            return false;
        }

		public int XCount
		{
			get
			{
				return layers.XCount;
			}
		}

		public int YCount
		{
			get
			{
				return layers.YCount;
			}
		}

		public int getTileID(MapLayer layer, int x, int y)
        {
            try {
				return layer.MatrixTile[y][x];
            }
            catch (Exception err) {
                return -1;
            }
        }
		public int getTileFlip(MapLayer layer, int x, int y)
        {
            try
            {
				return layer.MatrixFlip[y][x];
            }
            catch (Exception err)
            {
                return -1;
            }
        }
		public int getTagID(MapLayer layer, int x, int y)
        {
            try
            {
				return layer.MatrixTag[y][x];
            }
            catch (Exception err)
            {
                return -1;
            }
        }
		public int getAnimateID(MapLayer layer, int x, int y)
        {
            try
            {
				return layer.MatrixAnim[y][x];
            }
            catch (Exception err)
            {
                return 0;
            }
        }
        

        private void addAnimate()
        {
            int pos = listView1.Items.Count;
            if (listView1.SelectedItems != null && listView1.SelectedItems.Count > 0 )
            {
                pos = listView1.Items.IndexOf(listView1.SelectedItems[0]);
            }
            if (pos == 0 && listView1.Items.Count>1)
            {
                pos = 1;
            }

            ArrayList frames = new ArrayList();
            frames.Add(srcIndex);
            ArrayList flips = new ArrayList();
            flips.Add(flipIndex);
          
            ListViewItem item = new ListViewItem(new String[] { animCount.ToString("d4"), "1" });
            
            //Animates.Add(frames);
            listView1.Items.Insert(pos,item);
            AnimIndexTable.Add(item, frames);
            AnimFlipTable.Add(item,flips);
        

            exchangeAnimate(pos,pos+1);

            item.Selected = true;

            animCount++;
            //curAnimate = listView1.Items.IndexOf(item);
           
        }
        private void delAnimate()
        {
            if (listView1.Items.Count > 1 &&
                listView1.SelectedItems != null &&
                listView1.SelectedItems.Count > 0 && 
                listView1.Items.IndexOf(listView1.SelectedItems[0])>0)
            {
                //Animates.Remove(AnimTable[listView1.SelectedItems[0]]);

                AnimIndexTable.Remove(listView1.SelectedItems[0]);
                AnimFlipTable.Remove(listView1.SelectedItems[0]);
                listView1.Items.Remove(listView1.SelectedItems[0]);


            }
          
        }
        private void exchangeAnimate(int src, int dst)
        {
            if (listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)
            {
                if (listView1.Items.Count > 1 )
                {
                    ListViewItem tempsrc = listView1.Items[src];
                    ListViewItem tempdst = listView1.Items[dst];
                    listView1.Items.Remove(tempdst);
                    listView1.Items.Remove(tempsrc);
                    if (src < dst)
                    {
                        listView1.Items.Insert(src, tempdst);
                        listView1.Items.Insert(dst, tempsrc);
                    }
                    else
                    {
                        listView1.Items.Insert(dst, tempsrc);
                        listView1.Items.Insert(src, tempdst);
                    }
                    tempsrc.Focused = true;
                    
                }
            }
        }

       
        private void addFrame(int pos, int index, int flip)
        {
            if (listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)
            {
                ((ArrayList)AnimIndexTable[listView1.SelectedItems[0]]).Insert(pos,index);
                ((ArrayList)AnimFlipTable[listView1.SelectedItems[0]]).Insert(pos, flip);
                listView1.SelectedItems[0].SubItems[1].Text = (((ArrayList)AnimIndexTable[listView1.SelectedItems[0]])).Count.ToString();
                exchangeFrame(pos,pos+1);
            }

        }
        private void delFrame(int pos)
        {
            if (listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)
            {
                if (((ArrayList)AnimIndexTable[listView1.SelectedItems[0]]).Count > 1)
                {
                    ((ArrayList)AnimIndexTable[listView1.SelectedItems[0]]).RemoveAt(pos);
                    ((ArrayList)AnimFlipTable[listView1.SelectedItems[0]]).RemoveAt(pos);
                    listView1.SelectedItems[0].SubItems[1].Text = (((ArrayList)AnimIndexTable[listView1.SelectedItems[0]])).Count.ToString();
                }
            }
        }
        private void exchangeFrame(int src, int dst)
        {
            if (listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)
            {
                if (((ArrayList)AnimIndexTable[listView1.SelectedItems[0]]).Count > 1)
                {
                    Object obj1 = ((ArrayList)AnimIndexTable[listView1.SelectedItems[0]])[src];
                    ((ArrayList)AnimIndexTable[listView1.SelectedItems[0]])[src] = ((ArrayList)AnimIndexTable[listView1.SelectedItems[0]])[dst];
                    ((ArrayList)AnimIndexTable[listView1.SelectedItems[0]])[dst] = obj1;

                    Object obj2 = ((ArrayList)AnimFlipTable[listView1.SelectedItems[0]])[src];
                    ((ArrayList)AnimFlipTable[listView1.SelectedItems[0]])[src] = ((ArrayList)AnimFlipTable[listView1.SelectedItems[0]])[dst];
                    ((ArrayList)AnimFlipTable[listView1.SelectedItems[0]])[dst] = obj2;
                }
            }
        }

        private ArrayList getFrame(int animate)
        {
            if (animate>=0 && animate < listView1.Items.Count)
            {
                return ((ArrayList)AnimIndexTable[listView1.Items[animate]]);
            }
            return null;
        }
        private ArrayList getFrameFlip(int animate)
        {
            if (animate >= 0 && animate < listView1.Items.Count)
            {
                return ((ArrayList)AnimFlipTable[listView1.Items[animate]]);
            }
            return null;
        }

        private void fillAnimate(MapLayer layer, int index)
        {
            for (int y = dstRect.Y; y < dstRect.Y + dstRect.Height;y+=CellH )
            {
                for (int x = dstRect.X; x < dstRect.X + dstRect.Width; x+=CellW)
                {
					putAnimate(layer, index, x / CellW, y / CellH, IsMultiLayer.Checked);
                }
            }
        }
		private void fillTile(MapLayer layer, int index, int flip)
        {
            for (int y = dstRect.Y; y < dstRect.Y + dstRect.Height;y+=CellH )
            {
                for (int x = dstRect.X; x < dstRect.X + dstRect.Width; x+=CellW)
                {
					putTile(layer, index, x / CellW, y / CellH);
					putFlip(layer, flip, x / CellW, y / CellH);
                }
            }
        }
		private void fillTag(MapLayer layer, int index)
        {
            for (int y = dstRect.Y; y < dstRect.Y + dstRect.Height; y += CellH)
            {
                for (int x = dstRect.X; x < dstRect.X + dstRect.Width; x += CellW)
                {
					putTag(layer, index, x / CellW, y / CellH);
                }
            }
        }
		private void copyDst(MapLayer curlayer)
        {
            int sbx = dstRect.X / CellW;
            int sby = dstRect.Y / CellH;
            int xcount = dstRect.Width / CellW;
            int ycount = dstRect.Height / CellH;

            if (xcount <= 0 || ycount <= 0) return;

            clipSuper = this.super;
			dstClipLayer = curlayer.copyDst(sbx, sby, xcount, ycount);
			/*
            dstClipTile = new int[ycount][];
            dstClipTag = new int[ycount][];
            dstClipAnim = new int[ycount][];
            dstClipFlip = new int[ycount][];
            for (int by = 0; by < ycount; by++)
            {
                dstClipTile[by] = new int[xcount];
                dstClipTag[by] = new int[xcount];
                dstClipAnim[by] = new int[xcount];
                dstClipFlip[by] = new int[xcount];
                for (int bx = 0; bx < xcount; bx++)
                {
                    if (sby + by < MatrixTile.Length && sbx + bx < MatrixTile[by].Length)
                    {
                        dstClipTile[by][bx] = MatrixTile[sby + by][sbx + bx];
                        dstClipTag[by][bx] = MatrixTag[sby + by][sbx + bx];
                        dstClipAnim[by][bx] = MatrixAnim[sby + by][sbx + bx];
                        dstClipFlip[by][bx] = MatrixFlip[sby + by][sbx + bx];
                    }
                }
            }
            */
        }
        private void clipDst(MapLayer curlayer)
        {
            int sbx = dstRect.X / CellW;
            int sby = dstRect.Y / CellH;
            int xcount = dstRect.Width / CellW;
            int ycount = dstRect.Height / CellH;

            if (xcount <= 0 || ycount <= 0) return;

			clipSuper = this.super;
			dstClipLayer = curlayer.copyDst(sbx, sby, xcount, ycount);

            for (int by = 0; by < ycount; by++)
            {
                for (int bx = 0; bx < xcount; bx++)
                {
					putTile(curlayer, srcIndexR, sbx + bx, sby + by);
					putFlip(curlayer, flipIndex, sbx + bx, sby + by);
					putTag(curlayer, 0, sbx + bx, sby + by);
					putAnimate(curlayer, 0, sbx + bx, sby + by, false);
                    
                }
            }

        }
		private void paseDst(MapLayer curlayer)
        {
            if (clipSuper != this.super) return;

            
            int sbx = dstRect.X / CellW;
            int sby = dstRect.Y / CellH;
            //int xcount = dstRect.Width / CellW;
            //int ycount = dstRect.Height / CellH;
            int xcount = dstRect.Width / CellW;
            int ycount = dstRect.Height / CellH;

			if (dstClipLayer != null)
            {
				xcount = Math.Max(xcount, dstClipLayer.xcount());
				ycount = Math.Max(ycount, dstClipLayer.ycount());
            }

            if (xcount <= 0 || ycount <= 0) return;

            for (int by = 0; by < ycount; by++)
            {
                for (int bx = 0; bx < xcount; bx++)
                {
					int sy = by % dstClipLayer.ycount();
					int sx = bx % dstClipLayer.xcount();
					int dy = sby + by;
					int dx = sbx + bx;
                    putTile(curlayer,
						dstClipLayer.MatrixTile[sy][sx], 
                        dx, dy);
					putFlip(curlayer,
					   dstClipLayer.MatrixFlip[sy][sx],
						dx, dy);
					putTag(curlayer,
					   dstClipLayer.MatrixTag[sy][sx],
						dx, dy);
					int anim = dstClipLayer.MatrixAnim[sy][sx];
					putAnimate(curlayer,
                        anim,
					   dx, dy,
                        anim<0);
                }
            }
        }
		private void clearDst(MapLayer curlayer)
        {
            int sbx = dstRect.X / CellW;
            int sby = dstRect.Y / CellH;
            int xcount = dstRect.Width / CellW;
            int ycount = dstRect.Height / CellH;

            if (xcount <= 0 || ycount <= 0) return;

            for (int by = 0; by < ycount; by++)
            {
                for (int bx = 0; bx < xcount; bx++)
                {

					putTile(curlayer, srcIndexR, sbx + bx, sby + by);
					putFlip(curlayer, flipIndex, sbx + bx, sby + by);
					putTag(curlayer, 0, sbx + bx, sby + by);
					putAnimate(curlayer, 0, sbx + bx, sby + by, false);
                    
                }
            }

        }


        public void putTile(MapLayer layer, int index, int bx, int by)
        {
			if (!layer.visible) { return; }
			if (index < getTileCount() && by < layer.MatrixTile.Length && bx < layer.MatrixTile[by].Length)
            {
				layer.MatrixTile[by][bx] = index;

                if (MiniView != null)
                {
                    MiniView.rebuff(bx,by,getTileImage(layer,bx,by));
                }
            }
        }
		public void putTag(MapLayer layer, int tag, int bx, int by)
		{
			if (!layer.visible) { return; }
			if (by < layer.MatrixTag.Length && bx < layer.MatrixTag[by].Length)
            {
				layer.MatrixTag[by][bx] = tag;
            }
        }
		public void putAnimate(MapLayer layer, int index, int bx, int by, Boolean multiLayer)
		{
			if (!layer.visible) { return; }
			if (by < layer.MatrixAnim.Length && bx < layer.MatrixAnim[by].Length)
            {
                index = Math.Abs(index);

                if (index > 0 && index < listView1.Items.Count)
                {
                    if (multiLayer)//duo ceng
                    {
						layer.MatrixAnim[by][bx] = -index;
                    }
                    else
                    {
						layer.MatrixAnim[by][bx] = index;
                    }
                }
                else
                {
					layer.MatrixAnim[by][bx] = 0;
                }
                
            }
        }
		public void putFlip(MapLayer layer, int flip, int bx, int by)
		{
			if (!layer.visible) { return; }
			if (flip < 8 && by < layer.MatrixFlip.Length && bx < layer.MatrixFlip[by].Length)
            {
				layer.MatrixFlip[by][bx] = flip;
            }
        }

		private void fillSrcTiles(MapLayer layer, int bx, int by)
        {
            if (srcRects != null && srcTilesIndexBrush != null)
            {
                for (int i = srcTilesIndexBrush.Count - 1; i >= 0; --i)
                {
                    int index = (int)srcTilesIndexBrush[i];
                    int ox = (int)srcTilesOXBrush[i];
                    int oy = (int)srcTilesOYBrush[i];

					putTile(layer, index, bx + ox, by + oy);
                }
            }
        }

        private void renderSrcTile(Graphics g, int index, int flip, int x, int y)
        {
            Image img = getTileImage(index);
            if (img != null && flip < Graphics.FlipTable.Length)
            {
				g.drawImageTrans(img, x, y, flip);
            }
            
        }
        private void renderDstTile(Graphics g, int index, int flip, int x, int y)
        {
			Image img = getTileImage(index);
			
				if (img != null && flip < Graphics.FlipTable.Length)
				{
					if (D45.Checked)
					{
						g.drawImageTrans(img, x, y, flip);
					}
					else
					{
						g.drawImageRegion(img, x, y, KeyX, KeyY, CellW, CellH, Graphics.FlipTable[flip]);
					}
				}
			
        }
        private void renderDstKeyAndTile(Graphics g, bool key, bool tile, int index, int x, int y)
        {
            if (key)
            {
                String str = getTileKey(index);
                if (str != null)
                {
                    g.setColor(0xff, 0, 0, 0);
                    g.drawString(str, x + 1, y + 1, 0);
                    g.setColor(super.getColorKey().ToArgb());
                    g.drawString(str, x, y, 0);
                }
            }
            if (tile)
            {
                y = y + CellH / 2;
                String str = index.ToString();
                g.setColor(0xff, 0, 0, 0);
                g.drawString(str, x + 1, y + 1, 0);
                g.setColor(super.getColorTileID().ToArgb());
                g.drawString(str, x, y, 0);
            }
        }

        private void renderSrcTiles(Graphics g, int x, int y)
        {
            if (srcRects != null && srcTilesIndexBrush != null)
            {
                for (int i = srcTilesIndexBrush.Count - 1; i >= 0; --i)
                {
                    int index = (int)srcTilesIndexBrush[i];
                    int ox = (int)srcTilesOXBrush[i] * CellW;
                    int oy = (int)srcTilesOYBrush[i] * CellH;

                    renderSrcTile(g, index, 0, x + ox, y + oy);
                }
            }
        }

        private void renderTag(Graphics g, int type, int x, int y)
        {
            g.setColor(0xff, 0, 0, 0);
            switch (type)
            {
                case 0:
                    break;
                case 1:
                    g.drawRect(1 + x, 1 + y, CellW - 3, CellH - 3);
                    break;
                case 2:
                    g.drawLine(x, 1 + y, x + CellW - 1, 1 + y);
                    break;
                case 3:
                    g.drawLine(x, y + CellH - 1 - 1, x + CellW - 1, y + CellH - 1 - 1);
                    break;
                case 4:
                    g.drawLine(x + 1, y, x + 1, y + CellH - 1);
                    break;
                case 5:
                    g.drawLine(x + CellW - 1 - 1, y, x + CellW - 1 - 1, y + CellH - 1);
                    break;
                case 6:
                    g.drawLine(x, 1 + y, x + CellW - 1, 1 + y + CellH - 1);
                    break;
                case 7:
                    g.drawLine(x + CellW - 1, 1 + y, x, 1 + y + CellH - 1);
                    break;
            }
            g.setColor(0xff, 0xff, 0, 0);
            switch (type)
            {
                case 0:
                    break;
                case 1:
                    g.drawRect(x, y, CellW - 1, CellH - 1);
                    break;
                case 2:
                    g.drawLine(x, y, x + CellW - 1, y);
                    break;
                case 3:
                    g.drawLine(x, y + CellH - 1, x + CellW - 1, y + CellH - 1);
                    break;
                case 4:
                    g.drawLine(x, y, x, y + CellH - 1);
                    break;
                case 5:
                    g.drawLine(x + CellW - 1, y, x + CellW - 1, y + CellH - 1);
                    break;
                case 6:
                    g.drawLine(x, y, x + CellW - 1, y + CellH - 1);
                    break;
                case 7:
                    g.drawLine(x + CellW - 1, y, x, y + CellH - 1);
                    break;
            }

        }
        private void renderAnimate(Graphics g, int anim, int frame, int x, int y, Boolean tag)
        {
            try
            {
                if (listView1.Items.Count <= 0) return;

                if (anim >= 0 && anim < listView1.Items.Count)
                {
                    ArrayList animate = (ArrayList)(AnimIndexTable[listView1.Items[anim]]);
                    ArrayList flips = (ArrayList)(AnimFlipTable[listView1.Items[anim]]);
                    if (animate.Count <= 0) return;
                    if (flips.Count <= 0) return;

                    int tid = (int)(animate[frame % animate.Count]);
                    int tif = (int)(flips[frame % flips.Count]);

                    renderDstTile(g,
                        tid,
                        tif,
                        x, y);

                    
                    renderDstKeyAndTile(g, isShowKey.Checked, isShowTileID.Checked ,tid, x, y);
                    

                    if (tag)
                    {
                        g.setColor(0x00, 0x00, 0xff);
                        g.drawRect(x, y, CellW - 1, CellH - 1);
                    }
                }
            }
            catch (Exception err)
            {
            }
               
        }
        private void renderCombo(Graphics g, int anim, int x, int y, Boolean tag)
        {
            if (listView1.Items.Count <= 0) return;

            ArrayList animate = (ArrayList)(AnimIndexTable[listView1.Items[anim]]);
            ArrayList flips = (ArrayList)(AnimFlipTable[listView1.Items[anim]]);
            if (animate.Count <= 0) return;
            if (flips.Count <= 0) return;
            for (int frame = animate.Count - 1; frame >= 0; frame--)
            {
                int tid = (int)(animate[frame % animate.Count]);
                int tif = (int)(flips[frame % flips.Count]);
                renderDstTile(g,
                  tid,
                  tif,
                  x, y);
                if (frame==0){
                    renderDstKeyAndTile(g, isShowKey.Checked, isShowTileID.Checked, tid, x, y);
                }
            }
            if (tag)
            {
                g.setColor(0x00, 0xff, 0xff);
                g.drawRect(x, y, CellW - 1, CellH - 1);
            }
        }


        private System.Drawing.Image getAnimateImage(int anim, int frame)
        {
            try
            {
                if (listView1.Items.Count <= 0) return null;

                if (anim >= 0 && anim < listView1.Items.Count)
                {
                    ArrayList animate = (ArrayList)(AnimIndexTable[listView1.Items[anim]]);
                    if (animate.Count <= 0) return null;
                    return getTileImage((int)(animate[frame % animate.Count])).dimg;
                }
            }
            catch (Exception err)
            {
            }
            return null;
        }

        private System.Drawing.RotateFlipType getAnimateFlip(int anim, int frame)
        {
            try
            {
                if (listView1.Items.Count <= 0) return 0;

                if (anim >= 0 && anim < listView1.Items.Count)
                {
                    ArrayList flips = (ArrayList)(AnimFlipTable[listView1.Items[anim]]);
                    if (flips.Count <= 0) return 0;
                    return Graphics.FlipTable[(int)(flips[frame % flips.Count])];
                }
            }
            catch (Exception err)
            {
            }
            return 0;
        }

        private void refreshAnimateList()
        {
            try
            {
                //listView1.TileSize = new System.Drawing.Size(CellW + 1, CellH + 1);
               

                imageList1.ImageSize = new System.Drawing.Size(CellW, CellH);
                imageList1.Images.Clear();
               
                foreach (ListViewItem item in listView1.Items)
                {
                    item.Text = item.Index.ToString();

                    System.Drawing.Image icon = getAnimateImage(item.Index, 0);

                    icon = new System.Drawing.Bitmap(icon);
                    icon.RotateFlip(getAnimateFlip(item.Index, 0));

                    if (icon != null)
                    {
                        imageList1.Images.Add(icon);
                        item.ImageIndex = imageList1.Images.Count - 1;
                    }

                    switch (listView1.View)
                    {
                        case View.Tile:
                            break;
                    }

                }
            }
            catch (Exception err) { }
        }


        private void renderSrc(Graphics g, int x, int y, System.Drawing.Rectangle screen)
        {
            int tc = getTileCount();
            System.Drawing.Rectangle srect = new System.Drawing.Rectangle( );
            for (int i = 0; i < tc; i++)
            {
                srect.X = x + getTileImage(i).x;
                srect.Y = y + getTileImage(i).y;
                srect.Width  = getTileImage(i).getWidth();
                srect.Height = getTileImage(i).getHeight();

                if (getTileImage(i) != null && screen.IntersectsWith(srect))
                {
                    renderSrcTile(g, i, 0, srect.X, srect.Y);
                }
            }
            if (srcIndex < getTileCount())
            {
                if (getTileImage(srcIndex) != null)
                {
                    renderSrcTile(g, srcIndex, flipIndex, x + getTileImage(srcIndex).x, y + getTileImage(srcIndex).y);
                }
            }
            if (srcIndexR < getTileCount())
            {
                if (getTileImage(srcIndexR) != null)
                {
                    renderSrcTile(g, srcIndexR, flipIndex, x + getTileImage(srcIndexR).x, y + getTileImage(srcIndexR).y);
                }
            }
        }
		private void renderDst(Graphics g, int x, int y, System.Drawing.Rectangle screen,
		   Boolean grid,
		   Boolean tag,
		   Boolean tagAnim,
		   Boolean anim,
		   int timer)
		{
			renderDst(g, x, y, screen, grid, tag, tagAnim, anim, timer, false);
		}

        private void renderDst(
			Graphics g, int x, int y, 
			System.Drawing.Rectangle screen, 
            Boolean grid, 
            Boolean tag, 
            Boolean tagAnim,
            Boolean anim,
            int timer,
			Boolean layerHilight)
        {
            int sx = (screen.X >= 0) ? (screen.X / CellW) : 0;
            int sy = (screen.Y >= 0) ? (screen.Y / CellH) : 0;
            int sw = (screen.Width / CellW + 2 );
            int sh = (screen.Height / CellH + 2 );

            //Console.WriteLine(" sx="+sx+" sy"+sy+" sw"+sw+" sh"+sh);
			for (int L = 0; L < layers.getCount(); L++)
			{
				MapLayer layer = layers.getLayer(L);
				if (layer.visible)
				{
					g.pushState();

					if (layerHilight && getCurLayer() != layer)
					{
						g.multiplyAlpha(0.5f);
						//g.setColor(0x80, 0x808080);
						//g.fillRect(screen.X, screen.Y, screen.Width, screen.Height);
					}

					for (int by = sy; by < sy + sh && by < layers.YCount; by++)
					{
						for (int bx = sx; bx < sx + sw && bx < layers.XCount; bx++)
						{
							int drawx = x + bx * CellW;
							int drawy = y + by * CellH;
							//int animi = layer.MatrixAnim[by][bx];
							int tilei = layer.MatrixTile[by][bx];
							int flipi = layer.MatrixFlip[by][bx];
							int tagi = layer.MatrixTag[by][bx];
							/*
							if (animi > 0 && animi < listView1.Items.Count)
							{
								renderAnimate(g, animi, anim ? timer : 0, drawx, drawy, tagAnim);
							}
							else if (animi < 0 && (-animi) < listView1.Items.Count)
							{
								renderCombo(g, -animi, drawx, drawy, tagAnim);
							}
							else */
							if (tilei >= 0 && tilei < getTileCount())
							{
								renderDstTile(g, tilei, flipi, drawx, drawy);
								renderDstKeyAndTile(g, isShowKey.Checked, isShowTileID.Checked, tilei, drawx, drawy);
							}
							
// 							if (tag && animi != 0)
// 							{
// 								g.setColor(0, 0, 0xff);
// 								g.drawRect(drawx, drawy, CellW, CellH);
// 							}

							if (tag && tagi != 0)
							{
								renderTag(g, tagi, drawx, drawy);
							}
						}
					}

					g.popState();
					
				}
			}
            if (grid)
            {
                g.setColor(0x80, 0xff, 0xff, 0xff);
               
                for (int bx = sx; bx < sx + sw; bx++)
                {
                    g.drawLine(
                        x + bx * CellW, 
                        y + sy * CellH, 
                        x + bx * CellW, 
                        y + sy * CellH + sh * CellH);
                }
                for (int by = sy; by < sy + sh; by++)
                {
                    g.drawLine(
                        x + sx * CellW, 
                        y + by * CellH, 
                        x + sx * CellW + sw * CellW, 
                        y + by * CellH);
                }
            }
        }

        public void Render(Graphics g, int x, int y, System.Drawing.Rectangle screen, Boolean grid, Boolean tag, Boolean anim,int timer)
        {
            renderDst(g, x, y, screen, grid, tag, false, anim, timer);
        }
		public void renderToMiniMap(System.Drawing.Graphics g)
		{
			for (int L = 0; L < layers.getCount(); L++)
			{
				MapLayer layer = layers.getLayer(L);
				if (layer.visible) {
					for (int x = 0; x < layers.XCount; x++)
					{
						for (int y = 0; y < layers.YCount; y++)
						{
							javax.microedition.lcdui.Image img = getTileImage(layer, x, y);

							if (img != null)
							{
								g.FillRectangle(img.getColorKeyBrush(), x, y, 1, 1);
							}
						}
					}
				}
			}

		}
        // src
        #region src tiles

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            for (int i = getTileCount() - 1; i >= 0; i--)
            {
                if (getTileImage(i)!=null)
                {
                    pictureBox1.Width = Math.Max(
                        pictureBox1.Width,
                        getTileImage(i).x + getTileImage(i).getWidth()
                        );
                    pictureBox1.Height = Math.Max(
                        pictureBox1.Height,
                        getTileImage(i).y + getTileImage(i).getHeight()
                        );
                    //break;
                }
            }
                

            Graphics g = new Graphics(e.Graphics);

            g.pushState();
            g.scale(srcScale, srcScale);

            renderSrc(g, 0, 0 ,
                new System.Drawing.Rectangle(
                    -pictureBox1.Location.X,
                    -pictureBox1.Location.Y,
                   (int)( panel3.Width / srcScale),
                   (int)( panel3.Height / srcScale))
            );



            if (toolTilesBrush.Checked)
            {
                System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0xff, 0, 0, 0));
                System.Drawing.Brush brush = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0x80, 0xff, 0xff, 0xff)).Brush;
                e.Graphics.FillRectangle(brush, srcRects);
                e.Graphics.DrawRectangle(pen, srcRects.X, srcRects.Y, srcRects.Width, srcRects.Height);
            }
            else if (toolTileBrush.Checked || toolAnimBrush.Checked || toolSelecter.Checked)
            {

                System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0xff, 0, 0, 0));
                System.Drawing.Brush brush = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0x80, 0xff, 0xff, 0xff)).Brush;
                e.Graphics.FillRectangle(brush, srcRect);
                e.Graphics.DrawRectangle(pen, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height);

                System.Drawing.Pen penR = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0xff, 0xff, 0xff, 0xff));
                System.Drawing.Brush brushR = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0x80, 0x80, 0x80, 0x80)).Brush;
                e.Graphics.FillRectangle(brushR, srcRectR);
                e.Graphics.DrawRectangle(penR, srcRectR.X, srcRectR.Y, srcRectR.Width, srcRectR.Height);
            }

            g.popState();
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int eX = (int)(e.X / srcScale);
            int eY = (int)(e.Y / srcScale);

            if (toolTilesBrush.Checked)
            {
                if (e.Button == MouseButtons.Left)
                {
                    srcPX = (eX / CellW * CellW);
                    srcPY = (eY / CellH * CellW);
                    srcPX = Math.Max(srcPX, 0);
                    srcPY = Math.Max(srcPY, 0);
                    srcPX = Math.Min(srcPX, super.getDstWidth());
                    srcPY = Math.Min(srcPY, super.getDstHeight());

                    srcRects = new System.Drawing.Rectangle(srcPX, srcPY, CellW, CellH);
                }
                pictureBox1.Refresh();
            }
            else if (toolTileBrush.Checked || toolAnimBrush.Checked || toolSelecter.Checked) 
            {
                System.Drawing.Rectangle dst = new System.Drawing.Rectangle(0, 0, 1, 1);
                for (int i = 0; i < getTileCount(); i++)
                {
                    if (getTileImage(i) != null && getTileImage(i).killed == false)
                    {
                        dst.X = getTileImage(i).x;
                        dst.Y = getTileImage(i).y;
                        dst.Width = getTileImage(i).getWidth();
                        dst.Height = getTileImage(i).getHeight();

                        if (dst.Contains(eX, eY))
                        {
                            if (e.Button == MouseButtons.Left)
                            {
                                srcRect.X = dst.X;
                                srcRect.Y = dst.Y;
                                srcRect.Width = dst.Width;
                                srcRect.Height = dst.Height;

                                srcIndex = i;
                                //srcImage = getTileImage(i);

                                toolStripLabel1.Text =
                                    "第" + i + "号" +
                                    " 宽：" + getTileImage(i).getWidth() +
                                    " 高：" + getTileImage(i).getHeight();

                                pictureBox1.Refresh();
                                break;
                            }
                            if (e.Button == MouseButtons.Right)
                            {
                                srcRectR.X = dst.X;
                                srcRectR.Y = dst.Y;
                                srcRectR.Width = dst.Width;
                                srcRectR.Height = dst.Height;

                                srcIndexR = i;
                                //srcImage = getTileImage(i);

                                toolStripLabel1.Text =
                                    "第" + i + "号" +
                                    " 宽：" + getTileImage(i).getWidth() +
                                    " 高：" + getTileImage(i).getHeight();

                                pictureBox1.Refresh();
                                break;
                            }

                        }
                    }
                }
                textBox1.Focus();
            }
        }
       
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            int eX = (int)(e.X / srcScale);
            int eY = (int)(e.Y / srcScale);
            if (toolTilesBrush.Checked)
            {
                if (srcRects!=null && e.Button==MouseButtons.Left)
                {
                    srcQX = ((int)eX / CellW * CellW);
                    srcQY = ((int)eY / CellH * CellW);
                    srcQX = Math.Max(srcQX, 0);
                    srcQY = Math.Max(srcQY, 0);
                    srcQX = Math.Min(srcQX, super.getDstWidth());
                    srcQY = Math.Min(srcQY, super.getDstHeight());

                    srcRects.X = Math.Min(srcPX, srcQX);
                    srcRects.Y = Math.Min(srcPY, srcQY);
                    srcRects.Width  = Math.Abs(srcPX - srcQX) + CellW;
                    srcRects.Height = Math.Abs(srcPY - srcQY) + CellW;

                    pictureBox1.Refresh();
                }
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            int eX = (int)(e.X / srcScale);
            int eY = (int)(e.Y / srcScale);
            if (toolTilesBrush.Checked)
            {
                if (srcRects != null && e.Button == MouseButtons.Left)
                {
                    srcTilesIndexBrush = new ArrayList();
                    srcTilesOXBrush = new ArrayList();
                    srcTilesOYBrush = new ArrayList();

                    for (int i = 0; i < getTileCount(); i++)
                    {
                        Image tile = getTileImage(i);

                        if (tile != null && tile.killed == false)
                        {
                            if (srcRects.IntersectsWith(new System.Drawing.Rectangle(tile.x, tile.y, tile.getWidth(), tile.getHeight())))
                            {
                                int ox = (int)(tile.x - srcRects.X) / CellW;
                                int oy = (int)(tile.y - srcRects.Y) / CellH;

                                srcTilesIndexBrush.Add(i);
                                srcTilesOXBrush.Add(ox);
                                srcTilesOYBrush.Add(oy);
                            }
                        }
                    }

                    pictureBox1.Refresh();
                }
            }
        }

        private void toolStripSrcZoomIn_Click(object sender, EventArgs e)
        {
            if (srcScale > (1.0f / 128))
            {
                srcScale /= 2;
                pictureBox1.Refresh();
            }
        }

        private void toolStripZoomOut_Click(object sender, EventArgs e)
        {
            if (srcScale < 1)
            {
                srcScale *= 2;
            } 
            else 
            {
                srcScale = 1;
            }
            pictureBox1.Refresh();
        }


        private void toolStripSelectSrcAll_Click(object sender, EventArgs e)
        {
            srcRects = new System.Drawing.Rectangle(0, 0, super.getDstWidth(), super.getDstHeight());

            srcTilesIndexBrush = new ArrayList();
            srcTilesOXBrush = new ArrayList();
            srcTilesOYBrush = new ArrayList();

            for (int i = 0; i < getTileCount(); i++)
            {
                Image tile = getTileImage(i);

                if (tile != null && tile.killed == false)
                {
                    if (srcRects.IntersectsWith(new System.Drawing.Rectangle(tile.x, tile.y, tile.getWidth(), tile.getHeight())))
                    {
                        int ox = (int)(tile.x - srcRects.X) / CellW;
                        int oy = (int)(tile.y - srcRects.Y) / CellH;

                        srcTilesIndexBrush.Add(i);
                        srcTilesOXBrush.Add(ox);
                        srcTilesOYBrush.Add(oy);
                    }
                }
            }

            pictureBox1.Refresh();
        }


        #endregion

        //property
        private void dstChangeMapSize(int xcount, int ycount)
        {
			if (ycount != layers.YCount || xcount != layers.XCount)
            {
				layers.dstChangeMapSize(xcount, ycount);
				MapLoc.Width = CellW * layers.XCount;
				MapLoc.Height = CellH * layers.YCount;
            }

            refreshMap();
        }
        private void numericUpDown2_Enter_1(object sender, EventArgs e)
        {
			numericUpDown2.Value = layers.XCount;
        }
        private void numericUpDown3_Enter_1(object sender, EventArgs e)
        {
			numericUpDown3.Value = layers.YCount;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            dstChangeMapSize((int)numericUpDown2.Value, (int)numericUpDown3.Value);
        }

        private void dstChageCellSize(int cellw,int cellh)
        {
            CellW = cellw;
            CellH = cellh;

            pictureBox3.Width = CellW;
            pictureBox3.Height = CellH;

            pictureBox4.Left = pictureBox3.Location.X + 3 + CellW;

            pictureBox4.Width = CellW;
            pictureBox4.Height = CellH;

			MapLoc.Width = CellW * layers.XCount;
			MapLoc.Height = CellH * layers.YCount;

            refreshMap();
        }
        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            dstChageCellSize((int)numericUpDown4.Value, CellH);
        }
        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            dstChageCellSize(CellW, (int)numericUpDown5.Value);
        }

        // dst
        private void toolStriptoolStripButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                dstRect.X = dstPX / CellW * CellW;
                dstRect.Y = dstPY / CellH * CellH;
                dstRect.Width = 0;
                dstRect.Height = 0;

                if (!sender.Equals(toolSelecter)) toolSelecter.Checked = false;
                if (!sender.Equals(toolTileBrush)) toolTileBrush.Checked = false;
                if (!sender.Equals(toolTilesBrush)) toolTilesBrush.Checked = false;
                if (!sender.Equals(toolCDBrush)) toolCDBrush.Checked = false;
                if (!sender.Equals(toolAnimBrush)) toolAnimBrush.Checked = false;
                ((ToolStripButton)sender).Checked = true;

                refreshMap();
            }
            catch (Exception err)
            {
            }

        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!sender.Equals(toolStripMenuItem2)) toolStripMenuItem2.Checked = false;
                if (!sender.Equals(toolStripMenuItem3)) toolStripMenuItem3.Checked = false;
                if (!sender.Equals(toolStripMenuItem4)) toolStripMenuItem4.Checked = false;
                if (!sender.Equals(toolStripMenuItem5)) toolStripMenuItem5.Checked = false;
                if (!sender.Equals(toolStripMenuItem6)) toolStripMenuItem6.Checked = false;
                if (!sender.Equals(toolStripMenuItem7)) toolStripMenuItem7.Checked = false;
                if (!sender.Equals(toolStripMenuItem8)) toolStripMenuItem8.Checked = false;
                if (!sender.Equals(toolStripMenuItem9)) toolStripMenuItem9.Checked = false;

                ((ToolStripMenuItem)sender).Checked = true;
                toolStripDropDownButton1.Image = ((ToolStripMenuItem)sender).Image;

                //
                if (sender.Equals(toolStripMenuItem2)) tagIndex = 0;
                if (sender.Equals(toolStripMenuItem3)) tagIndex = 1;
                if (sender.Equals(toolStripMenuItem4)) tagIndex = 2;
                if (sender.Equals(toolStripMenuItem5)) tagIndex = 3;
                if (sender.Equals(toolStripMenuItem6)) tagIndex = 4;
                if (sender.Equals(toolStripMenuItem7)) tagIndex = 5;
                if (sender.Equals(toolStripMenuItem8)) tagIndex = 6;
                if (sender.Equals(toolStripMenuItem9)) tagIndex = 7;
            }
            catch (Exception err)
            {
            }

            
        }
        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            refreshMap();
        }

        private void 填充ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fillTile(getCurLayer(),srcIndex,flipIndex);
            
        }
        private void 填充当前动画ToolStripMenuItem_Click(object sender, EventArgs e)
        {
			fillAnimate(getCurLayer(), curAnimate);
        }
        private void 填充当前地形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
			fillTag(getCurLayer(), tagIndex);
        }
        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyDst(getCurLayer());
        }
        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            paseDst(getCurLayer());
        }
  //minimap
        private void toolStripButton23_Click(object sender, EventArgs e)
        {
            try
            {
                System.Drawing.Image image = new System.Drawing.Bitmap(getWidth(), getHeight());
                System.Drawing.Graphics dg = System.Drawing.Graphics.FromImage(image);
                Render(new Graphics(dg), 0, 0, new System.Drawing.Rectangle(0, 0, getWidth(), getHeight()), false, false, false, 0);

                MapMini mini = new MapMini(image);
                mini.Show();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.StackTrace);
            }
        }

        //animate
        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            addAnimate();
            refreshAnimateList(); 
        }
        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("删除后，动画顺序将重新调整！", "确定要删除？", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                delAnimate();
                refreshAnimateList();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!this.Visible) return;

            if (toolStripButton5.Checked)
            {
                if (trackBar1.Value < trackBar1.Maximum)
                {
                    trackBar1.Value++;
                }
                else
                {
                    trackBar1.Value = trackBar1.Minimum;
                }
                
                pictureBox4.Width = trackBar1.Maximum * CellW + CellW;

                timer1.Interval = (int)numericUpDown1.Value;

            }

            if (toolStripButton14.Checked)
            {
                refreshMap();

                timer1.Interval = (int)numericUpDown1.Value;

            }


            curTime++;
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {

        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)
            {
                curAnimate = listView1.Items.IndexOf(listView1.SelectedItems[0]);
                toolStripLabel2.Text = "动画："+curAnimate.ToString();
                curFrame = 0;

                trackBar1.Maximum = getFrame(curAnimate).Count - 1;
                trackBar1.Value = 0;

                pictureBox4.Width = getFrame(curAnimate).Count * CellW;
                trackBar1_ValueChanged(sender, e);
            }

        }
        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (getFrame(curAnimate) != null)
                {
                    addFrame(curFrame, srcIndex, flipIndex);

                    curFrame = Math.Max(curFrame + 1, 0);

                    trackBar1.Maximum = getFrame(curAnimate).Count - 1;
                    trackBar1.Value = curFrame;

                    pictureBox4.Width = getFrame(curAnimate).Count * CellW;
                   
                }
            }
            catch (Exception err) { }
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (getFrame(curAnimate) != null)
                {
                    delFrame(curFrame);

                    curFrame = Math.Min(curFrame, getFrame(curAnimate).Count - 1);

                    trackBar1.Maximum = getFrame(curAnimate).Count - 1;
                    trackBar1.Value = curFrame;

                    pictureBox4.Width = getFrame(curAnimate).Count * CellW;
                 
                }
            }
            catch (Exception err) { }
        }
        
        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = new Graphics(e.Graphics);
            if (IsMultiLayer.Checked)
            {
                if (curAnimate >= 0)
                {
                    renderCombo(g, curAnimate, 0, 0, false);
                }
            }
            else
            {
                if (curAnimate >= 0 && curFrame >= 0)
                {
                    renderAnimate(g, curAnimate, curFrame, 0, 0, false);
                }
            }

        }


        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (getFrame(curAnimate) != null)
            {
                trackBar1.Value = trackBar1.Minimum;
            }
            
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (getFrame(curAnimate) != null)
            {
                if (trackBar1.Value > trackBar1.Minimum)
                {
                    trackBar1.Value -= 1;
                }
                else
                {
                    trackBar1.Value = trackBar1.Maximum;
                }
                
            }
        }
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            //if (toolStripButton5.Checked)
            //{
            //    timer1.Start();
            //}
            //else
            //{
            //    timer1.Stop();
            //}
        }
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (getFrame(curAnimate) != null)
            {
                if (trackBar1.Value < trackBar1.Maximum)
                {
                    trackBar1.Value += 1;
                }
                else
                {
                    trackBar1.Value = trackBar1.Minimum;
                }
            }
        }
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            if (getFrame(curAnimate) != null)
            {
                trackBar1.Value = trackBar1.Maximum;
            }
            
        }

        private void toolStripButton19_Click(object sender, EventArgs e)
        {
            if (trackBar1.Value > trackBar1.Minimum)
            {
                exchangeFrame(trackBar1.Value, trackBar1.Value - 1);
                trackBar1.Value = trackBar1.Value - 1;
            }
        }
        private void toolStripButton18_Click(object sender, EventArgs e)
        {
            if (trackBar1.Value < trackBar1.Maximum)
            {
                exchangeFrame(trackBar1.Value, trackBar1.Value + 1);
                trackBar1.Value = trackBar1.Value + 1;
            }
        }

        private void toolStripButton20_Click(object sender, EventArgs e)
        {
            if (curAnimate > 1)
            {
                exchangeAnimate(curAnimate, curAnimate-1);
            }
            refreshAnimateList(); 
        }
        private void toolStripButton21_Click(object sender, EventArgs e)
        {
            if (curAnimate < listView1.Items.Count - 1 && curAnimate > 0)
            {
                exchangeAnimate(curAnimate, curAnimate + 1);
            }
            refreshAnimateList(); 
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            curFrame = trackBar1.Value;
        }
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            curFrame = trackBar1.Value;

            pictureBox4.Refresh();
            pictureBox3.Refresh();

            toolStripStatusLabel2.Text =
                " 帧：" + (trackBar1.Value ) + "/" + (trackBar1.Maximum + 1 ) +
                " FPS=" + (((float)1000) / ((float)timer1.Interval));


        }

        private void pictureBox4_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = new Graphics(e.Graphics);

            for (int i = trackBar1.Minimum; i <= trackBar1.Maximum; i++)
            {
                renderAnimate(g, curAnimate, i, CellW * i, 0, false);

                if (trackBar1.Value == i)
                {
                    g.setColor(0x20, 0xff, 0xff, 0xff);
                    g.fillRect(CellW * i, 0, CellW - 1, CellW - 1);
                    g.setColor(0xff, 0, 0, 0);
                    g.drawRect(CellH * i, 0, CellH - 1, CellH - 1);
                }
            }
            
        }
        private void pictureBox4_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (getFrame(curAnimate) != null)
                {
                    trackBar1.Value = e.X / CellW;
                }
            }
        }

        private void toolStripButton22_Click(object sender, EventArgs e)
        {

        }


        //adjust flip
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                case Keys.PageUp:
                    /*
                    if (toolStripMenuItem10.Checked) toolStripMenuItem10_Click(toolStripMenuItem17, null);
                    else if (toolStripMenuItem11.Checked) toolStripMenuItem10_Click(toolStripMenuItem10, null);
                    else if (toolStripMenuItem12.Checked) toolStripMenuItem10_Click(toolStripMenuItem11, null);
                    else if (toolStripMenuItem13.Checked) toolStripMenuItem10_Click(toolStripMenuItem12, null);
                    else if (toolStripMenuItem14.Checked) toolStripMenuItem10_Click(toolStripMenuItem13, null);
                    else if (toolStripMenuItem15.Checked) toolStripMenuItem10_Click(toolStripMenuItem14, null);
                    else if (toolStripMenuItem16.Checked) toolStripMenuItem10_Click(toolStripMenuItem15, null);
                    else if (toolStripMenuItem17.Checked) toolStripMenuItem10_Click(toolStripMenuItem16, null);*/
                    this.flipIndex = imageFlipToolStripButton2.prewFlipIndex();
                    break;

                case Keys.PageDown: /*
                    if (toolStripMenuItem10.Checked) toolStripMenuItem10_Click(toolStripMenuItem11, null);
                    else if (toolStripMenuItem11.Checked) toolStripMenuItem10_Click(toolStripMenuItem12, null);
                    else if (toolStripMenuItem12.Checked) toolStripMenuItem10_Click(toolStripMenuItem13, null);
                    else if (toolStripMenuItem13.Checked) toolStripMenuItem10_Click(toolStripMenuItem14, null);
                    else if (toolStripMenuItem14.Checked) toolStripMenuItem10_Click(toolStripMenuItem15, null);
                    else if (toolStripMenuItem15.Checked) toolStripMenuItem10_Click(toolStripMenuItem16, null);
                    else if (toolStripMenuItem16.Checked) toolStripMenuItem10_Click(toolStripMenuItem17, null);
                    else if (toolStripMenuItem17.Checked) toolStripMenuItem10_Click(toolStripMenuItem10, null);*/
                    this.flipIndex = imageFlipToolStripButton2.nextFlipIndex();
                    break;
            }
            switch (e.KeyCode)
            {
                case Keys.PageUp:
                case Keys.PageDown:
                    refreshMap();
                    pictureBox1.Refresh();
                    break;
            }


            if (e.Control && e.KeyCode == Keys.A)
            {
                dstPX = 0;
                dstPY = 0;
                dstQX = getWidth();
                dstQY = getHeight();
            
                dstBX = 0;
                dstBY = 0;


                dstRect.X = 0;
                dstRect.Y = 0;
                dstRect.Width = getWidth();
                dstRect.Height = getHeight();

                refreshMap();
            }
            else if (e.Control && e.KeyCode == Keys.X) 
            {
                clipDst(getCurLayer()); 
                refreshMap();
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                copyDst(getCurLayer()); 
                refreshMap();
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                paseDst(getCurLayer()); 
                refreshMap();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                clearDst(getCurLayer());
                refreshMap();
            }
        }

        private void CellKeyPixelX_ValueChanged(object sender, EventArgs e)
        {
            KeyX = (int)CellKeyPixelX.Value;

            refreshMap();
        }

        private void CellKeyPixelY_ValueChanged(object sender, EventArgs e)
        {
            KeyY = (int)CellKeyPixelY.Value;

            refreshMap();
        }

        private void 统计相同的地块ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
				MapLayer curlayer = getCurLayer();
                int dy = dstRect.Y / CellW;
                int dx = dstRect.X / CellH;

				int curTile = getTileID(curlayer, dx, dy);
				int curFlip = getTileFlip(curlayer, dx, dy);
				int curFlag = getTagID(curlayer, dx, dy);
				int curAnim = getAnimateID(curlayer, dx, dy);

                long tileCount = 0;
                long flagCount = 0;
                long animCount = 0;

                for (int x = 0; x < layers.XCount; x++)
                {
                    for (int y = 0; y < layers.YCount; y++)
                    {
						if (getTileID(curlayer, x, y) == curTile && getTileFlip(curlayer, x, y) == curFlip)
                        {
                            tileCount++;
                        }

						if (getTagID(curlayer, x, y) == curFlag)
                        {
                            flagCount++;
                        }

						if (getAnimateID(curlayer, x, y) == curAnim)
                        {
                            animCount++;
                        }

                    }
                }

                MessageBox.Show(
                    "本图层\n" + 
					"相同的 地块 : " + tileCount + "\n" +
                    "相同的 判定 : " + flagCount + "\n" +
                    "相同的 动画 : " + animCount + "\n"
                    );


            }
            catch (Exception err)
            {
                MessageBox.Show(err.StackTrace);
            }




        }

        private void 详细ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listViewAnim.Text = "详细";
            refreshAnimateList();
        }

        private void 列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.List;
            listViewAnim.Text = "列表";
            refreshAnimateList();
        }

        private void 大图标ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.LargeIcon;
            listViewAnim.Text = "大图标";
            refreshAnimateList();
        }

        private void 小图标ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.SmallIcon;
            listViewAnim.Text = "小图标";
            refreshAnimateList();
        }

        private void 平铺ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.Tile;
            listViewAnim.Text = "平铺";
            refreshAnimateList();
            //listView1.TileSize = new System.Drawing.Size(CellW, CellH);
        }

        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {
            refreshAnimateList();
        }

        private void toolStripButton4_Click_1(object sender, EventArgs e)
        {
            try {
                BufferdMiniMap bf = new BufferdMiniMap(this);
                bf.Show();
            }
            catch (Exception err) { }
        }

	

        private void btnSaveMiniMap_Click(object sender, EventArgs e)
        {
            try
            {
                System.Drawing.Bitmap minimap = new System.Drawing.Bitmap(
					layers.XCount, layers.YCount//,
                    //System.Drawing.Imaging.PixelFormat.Format16bppArgb1555
                    );
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(minimap);
				/*for (int L = 0; L < layers.getCount(); L++)
				{
					MapLayer layer = layers.getLayer(L);
					for (int x = 0; x < layers.XCount; x++)
					{
						for (int y = 0; y < layers.YCount; y++)
						{
							javax.microedition.lcdui.Image img = getTileImage(layer, x, y);

							if (img != null)
							{
								g.FillRectangle(img.getColorKeyBrush(), x, y, 1, 1);
							}
						}
					}
				}*/
				renderToMiniMap(g);
                
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.InitialDirectory = ProjectForm.workSpace;
                sfd.Filter = "bmp图片(*.bmp)|*.bmp";
                sfd.FileName = this.id + ".bmp";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    minimap.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }


        //public Panel getViewPanel() { return panel2; }

        public System.Drawing.Size getMapSize()
        {
            return MapLoc.Size;
        }

        public System.Drawing.Rectangle getMapViewRectangle()
        {
            return new System.Drawing.Rectangle(
                hScrollBar1.Value,
                vScrollBar1.Value, 
                panel2.Width, 
                panel2.Height);
        }


        public void setMapViewLoc(int x, int y)
        {
            hScrollBar1.Value = x;
            vScrollBar1.Value = y;
            MapLoc.X = -x;
            MapLoc.Y = -y;
        }

        private void panel2_Scroll(object sender, ScrollEventArgs e)
        {
            if(MiniView!=null){
                MiniView.relocation(this);
            }
        }
        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }
        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            MapLoc.X = -hScrollBar1.Value;
            refreshMap();
        }
        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            MapLoc.Y = -vScrollBar1.Value;
            refreshMap();
        }


        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            hScrollBar1.Maximum = MapLoc.Width;
            hScrollBar1.Minimum = 0;
            hScrollBar1.LargeChange = panel2.Width;

            vScrollBar1.Maximum = MapLoc.Height;
            vScrollBar1.Minimum = 0;
            vScrollBar1.LargeChange = panel2.Height;
        }
        private void panel2_Resize(object sender, EventArgs e)
        {
            hScrollBar1.Maximum = MapLoc.Width;
            hScrollBar1.Minimum = 0;
            hScrollBar1.LargeChange = panel2.Width;

            vScrollBar1.Maximum = MapLoc.Height;
            vScrollBar1.Minimum = 0;
            vScrollBar1.LargeChange = panel2.Height;

        }



        private void isShowKey_Click(object sender, EventArgs e)
        {
            refreshMap();
        }

        private void isShowTileID_Click(object sender, EventArgs e)
        {
            refreshMap();
        }

        #region Dest Map 
        
        public void refreshMap()
        {
            hScrollBar1.Maximum = MapLoc.Width;
            hScrollBar1.Minimum = 0;
            hScrollBar1.LargeChange = panel2.Width;

            vScrollBar1.Maximum = MapLoc.Height;
            vScrollBar1.Minimum = 0;
            vScrollBar1.LargeChange = panel2.Height;

            /*MapRegion.Location = new System.Drawing.Point(
                hScrollBar1.Value,
                vScrollBar1.Value
                );*/

            /*MapRegion.Location = new System.Drawing.Point(
                -MapLoc.Location.X,
                -MapLoc.Location.Y
                );*/

            MapRegion.Size = new System.Drawing.Size(
                panel2.Width,
                panel2.Height
                );
            MapRegion.Refresh();

        }

        private void MapLoc_Paint(object sender, PaintEventArgs e)
        {
            refreshMap();
        }

        private void renderMapSelecter(Graphics g, PaintEventArgs e, System.Drawing.Rectangle viewRect)
        {

            System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0x80, 0, 0, 0));
            System.Drawing.Brush brush = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0x40, 0xff, 0xff, 0xff)).Brush;

            int px = -viewRect.X;
            int py = -viewRect.Y;

            int bx = dstQX / CellW * CellW + px;
            int by = dstQY / CellH * CellH + py;
            int bw = CellW - 1;
            int bh = CellH - 1;

            if (tabControl1.SelectedTab == tabPageTerrain)
            {
                mapTerrains1.renderSelectRegion(e.Graphics, bx, by, dstBX, dstBY, this);

                e.Graphics.DrawRectangle(System.Drawing.Pens.White, bx, by, bw, bh);

            }
            else if (tabControl1.SelectedTab == tabPageTile)
            {
                if (toolSelecter.Checked)
                {
                    e.Graphics.FillRectangle(
                        brush,
                        px + dstRect.X,
                        py + dstRect.Y,
                        dstRect.Width - 1,
                        dstRect.Height - 1
                        );
                    e.Graphics.DrawRectangle(
                        pen,
                        px + dstRect.X,
                        py + dstRect.Y,
                        dstRect.Width - 1,
                        dstRect.Height - 1
                        );
                }
                else if (toolTileBrush.Checked)
                {
                    System.Drawing.Pen lpen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0xff, 0, 0, 0));

                    renderDstTile(g, srcIndex, flipIndex, bx, by);

                    if (D45.Checked)
                    {
                        int x = dstQX / CellW * CellW;
                        int y = dstQY / CellH * CellH;
                        int w = CellW * 2;
                        int h = CellH * 2;

                        e.Graphics.DrawLine(lpen, px + x + 0x000, py + y + CellH, px + x + CellW, py + y + 0x000);//         /
                        e.Graphics.DrawLine(lpen, px + x + CellW, py + y + 0x000, px + x + w + 0, py + y + CellH);//       \
                        e.Graphics.DrawLine(lpen, px + x + 0x000, py + y + CellH, px + x + CellW, py + y + h + 0);//     \
                        e.Graphics.DrawLine(lpen, px + x + CellW, py + y + h + 0, px + x + w + 0, py + y + CellH);//   /
                    }
                    else
                    {
                        e.Graphics.DrawRectangle(lpen, bx, by, bw, bh);
                    }
                }
                else if (toolTilesBrush.Checked)
                {
                    renderSrcTiles(g, bx, by);
                }
                else if (toolCDBrush.Checked)
                {
                    System.Drawing.Pen lpen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0xff, 0xff, 0, 0));
                    e.Graphics.DrawRectangle(lpen, bx, by, bw, bh);
                }
                else if (toolAnimBrush.Checked)
                {
                    System.Drawing.Pen lpen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0xff, 0, 0, 0xff));

                    int x = dstQX / CellW * CellW;
                    int y = dstQY / CellH * CellH;
                    int w = CellW * 2;
                    int h = CellH * 2;
                    if (D45.Checked)
                    {
                        e.Graphics.DrawLine(lpen, px + x + 0x000, py + y + CellH, px + x + CellW, py + y + 0x000);//         /
                        e.Graphics.DrawLine(lpen, px + x + CellW, py + y + 0x000, px + x + w + 0, py + y + CellH);//       \
                        e.Graphics.DrawLine(lpen, px + x + 0x000, py + y + CellH, px + x + CellW, py + y + h + 0);//     \
                        e.Graphics.DrawLine(lpen, px + x + CellW, py + y + h + 0, px + x + w + 0, py + y + CellH);//   /
                    }
                    else
                    {
                        e.Graphics.DrawRectangle(lpen, bx, by, bw, bh);
                    }
                }
            }

            

        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = new Graphics(e.Graphics);

            try
            {
                System.Drawing.Rectangle viewRect = getMapViewRectangle();

                renderDst(g,
                    -viewRect.X,
                    -viewRect.Y,
                    viewRect,
                    toolStripButton11.Checked,
                    toolStripButton12.Checked || toolCDBrush.Checked,
                    toolAnimBrush.Checked,
                    toolStripButton14.Checked,
                    curTime,
					checkLayerHilight.Checked
                    );

                renderMapSelecter(g, e, viewRect);
            }
            catch (Exception err)
            {
            }


            // Console.WriteLine(""+pictureBox2.Location.Y);
        }
        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "地图";
        }
        private void pictureBox2_MouseMove(object sender, MouseEventArgs ed)
        {
            System.Drawing.Rectangle viewRect = getMapViewRectangle();

			MapLayer curlayer = getCurLayer();

            int wx = ed.X + viewRect.X;
            int wy = ed.Y + viewRect.Y;


            dstQX = wx < 0 ? 0 : (wx > MapLoc.Width - 1 ? MapLoc.Width - 1 : wx);
            dstQY = wy < 0 ? 0 : (wy > MapLoc.Height - 1 ? MapLoc.Height - 1 : wy);

            dstBX = dstQX / CellW;
            dstBY = dstQY / CellH;


            toolStripStatusLabel1.Text = "格:" + dstBX + "," + dstBY;

         
            if (tabControl1.SelectedTab == tabPageTerrain)
            {
                if (ed.Button == MouseButtons.Left)
                {
                    mapTerrains1.putSelectRegion(this, dstBX, dstBY);
                } 
                else if (ed.Button == MouseButtons.Right)
                {
                    //mapTerrains1.clearSelectRegion(this, srcIndexR, flipIndex, dstBX, dstBY);
					putTile(curlayer, srcIndexR, dstBX, dstBY);
					putFlip(curlayer, flipIndex, dstBX, dstBY);
					putTag(curlayer, 0, dstBX, dstBY);
					putAnimate(curlayer, 0, dstBX, dstBY, IsMultiLayer.Checked);
                }
            }
            else
            {
                if (toolSelecter.Checked)
                {
                    if (dstIsDown)
                    {
                        int l = Math.Min(dstPX, dstQX) / CellW;
                        int r = Math.Max(dstPX, dstQX) / CellW;
                        int t = Math.Min(dstPY, dstQY) / CellH;
                        int b = Math.Max(dstPY, dstQY) / CellH;

                        dstRect.X = l * CellW;
                        dstRect.Y = t * CellH;
                        dstRect.Width = (r - l + 1) * CellW;
                        dstRect.Height = (b - t + 1) * CellH;

                        toolStripStatusLabel1.Text += " 选择:" + (r - l + 1) + "x" + (b - t + 1) + "格";
                    }
                }
                else if (ed.Button == MouseButtons.Left)
                {

                    if (toolTileBrush.Checked)
                    {
						putTile(curlayer, srcIndex, dstBX, dstBY);
						putFlip(curlayer, flipIndex, dstBX, dstBY);
                    }
                    else if (toolTilesBrush.Checked)
                    {
						fillSrcTiles(curlayer, dstBX, dstBY);
                    }
                    else if (toolCDBrush.Checked)
                    {
						putTag(curlayer, tagIndex, dstBX, dstBY);
                    }
                    else if (toolAnimBrush.Checked)
                    {
						putAnimate(curlayer, curAnimate, dstBX, dstBY, IsMultiLayer.Checked);
                    }
                    

                }
                else if (ed.Button == MouseButtons.Right)
                {
                    if (toolTileBrush.Checked)
                    {
						putTile(curlayer, srcIndexR, dstBX, dstBY);
						putFlip(curlayer, flipIndex, dstBX, dstBY);
                    }
                    else if (toolCDBrush.Checked)
                    {
						putTag(curlayer, 0, dstBX, dstBY);
                    }
                    else if (toolAnimBrush.Checked)
                    {
						putAnimate(curlayer, 0, dstBX, dstBY, IsMultiLayer.Checked);
                    }
                }

            }

            refreshMap();
        }
        private void pictureBox2_MouseDown(object sender, MouseEventArgs ed)
        {
            System.Drawing.Rectangle viewRect = getMapViewRectangle();

            int wx = ed.X + viewRect.X;
            int wy = ed.Y + viewRect.Y;

			MapLayer curlayer = getCurLayer();
           

            if (ed.Button == MouseButtons.Left || ed.Button == MouseButtons.Right)
            {
                dstPX = wx;
                dstPY = wy;
                dstQX = wx;
                dstQY = wy;
                dstIsDown = true;

                dstBX = dstQX / CellW;
                dstBY = dstQY / CellH;
            }

            if (tabControl1.SelectedTab == tabPageTerrain)
            {
                if (ed.Button == MouseButtons.Left)
                {
                    mapTerrains1.putSelectRegion(this, dstBX, dstBY);
                }
                else if (ed.Button == MouseButtons.Right)
                {
                    //mapTerrains1.clearSelectRegion(this, srcIndexR, flipIndex, dstBX, dstBY);
					putTile(curlayer, srcIndexR, dstBX, dstBY);
					putFlip(curlayer, flipIndex, dstBX, dstBY);
					putTag(curlayer, 0, dstBX, dstBY);
					putAnimate(curlayer, 0, dstBX, dstBY, IsMultiLayer.Checked);
                }
            }
            else
            {
                if (ed.Button == MouseButtons.Left)
                {
                    if (toolSelecter.Checked)
                    {
                        dstRect.X = dstPX / CellW * CellW;
                        dstRect.Y = dstPY / CellH * CellH;
                        dstRect.Width = 0;
                        dstRect.Height = 0;
                    }
                    else if (toolTileBrush.Checked)
                    {
						putTile(curlayer, srcIndex, dstBX, dstBY);
						putFlip(curlayer, flipIndex, dstBX, dstBY);
                    }
                    else if (toolTilesBrush.Checked)
                    {
						fillSrcTiles(curlayer, dstBX, dstBY);
                    }
                    else if (toolCDBrush.Checked)
                    {
						putTag(curlayer, tagIndex, dstBX, dstBY);
                    }
                    else if (toolAnimBrush.Checked)
                    {
						putAnimate(curlayer, curAnimate, dstBX, dstBY, IsMultiLayer.Checked);
                    }
                }
                else if (ed.Button == MouseButtons.Right)
                {
                    if (toolSelecter.Checked)
                    {
                        if (dstRect.Contains(wx, wy))
                        {
                            try
                            {
                                contextMenuStrip1.Opacity = 0.5;
                                contextMenuStrip1.Show(MapRegion, ed.Location);
                            }
                            catch (Exception err)
                            {
                                Console.WriteLine(err.StackTrace + "  at  " + err.Message);
                            }
                        }
                    }
                    else if (toolTileBrush.Checked)
                    {
						putTile(curlayer, srcIndexR, dstBX, dstBY);
						putFlip(curlayer, flipIndex, dstBX, dstBY);
                    }
                    else if (toolCDBrush.Checked)
                    {
						putTag(curlayer, 0, dstBX, dstBY);
                    }
                    else if (toolAnimBrush.Checked)
                    {
						putAnimate(curlayer, 0, dstBX, dstBY, IsMultiLayer.Checked);
                    }

                }
            }
            refreshMap();

            textBox1.Focus();
        }
        private void pictureBox2_MouseUp(object sender, MouseEventArgs ed)
        {
            System.Drawing.Rectangle viewRect = getMapViewRectangle();

			MapLayer curlayer = getCurLayer();

            int wx = ed.X + viewRect.X;
            int wy = ed.Y + viewRect.Y;

            if (dstIsDown)
            {
                dstQX = wx;
                dstQY = wy;
                dstIsDown = false;
            }
            refreshMap();
        }

        #endregion



        #region Script

        //

        abstract class IScriptOP
        {
            public class MapBlock
            {
                public int X;
                public int Y;
                public int TileID;
                public int AnimID;

                public MapBlock(int x, int y, int tileID, int animID)
                {
                    X = x;
                    Y = y;
                    TileID = tileID;
                    AnimID = animID;
                }
            }

            //
            public const int sc_TypeTile = 0;
            public const int sc_TypeAnim = 1;
            public const int sc_TypeLayer = 2;

            static public int sc_getTileType(String str)
            {
                str = str.Trim().ToLower();
                if (str.Equals("tile")) return sc_TypeTile;
                if (str.Equals("anim")) return sc_TypeAnim;
                if (str.Equals("layer")) return sc_TypeLayer;
                return -1;
            }

            static public String sc_putTileType(int dstType)
            {
                if (dstType == sc_TypeTile) return "tile";
                if (dstType == sc_TypeAnim) return "anim";
                if (dstType == sc_TypeLayer) return "layer";
                return "";
            }

            static public String[] sc_getArgs(String str)
            {
                int args = str.IndexOf("(") + 1;
                int arge = str.LastIndexOf(")") - 1;
                int argl = arge - args + 1;
                String arg = str.Substring(args, argl);
                String[] ret = sc_split(arg);
                for (int i = 0; i < ret.Length; i++ )
                {
                    ret[i] = ret[i].Trim();
                }
                return ret;
            }

            static String[] sc_split(String str)
            {
                String[] ret;

                // split group
                while (true) 
                {
                    int gi = str.IndexOf("{");

                    if (gi<0)
                    {
                        ret = str.Split(new char[] { ',' });
                        break;
                    }
                    else
                    {
                        int ge = str.IndexOf("}", gi);
                        int gn = ge - gi + 1;
                        String sub = str.Substring(gi, gn);
                        String newsub = sub.Replace(',', '?');
                        newsub = newsub.Replace('{', ' ');
                        newsub = newsub.Replace('}', ' ');
                        str = str.Replace(sub, newsub);
                    }
                }

                for (int i = 0; i < ret.Length; i++ )
                {
                    ret[i] = ret[i].Replace('?', ',');
                }

                return ret;
            }

            static public int[] sc_getGroupArg(String str)
            {
                int[] SrcGroup;

                if (str.Contains("~"))
                {
                    String[] ss = str.Split(new char[] { '~' });
                    int min = int.Parse(ss[0].Trim());
                    int max = int.Parse(ss[1].Trim());
                    min = Math.Min(min, max);
                    max = Math.Max(min, max);
                    int len = max - min + 1;

                    SrcGroup = new int[len];
                    for (int i = 0; i < len; i++)
                    {
                        SrcGroup[i] = min + i;
                    }
                }
                else
                {
                    String[] ss = str.Split(new char[] { ',' });
                    SrcGroup = new int[ss.Length];
                    for (int i = 0; i < ss.Length; i++)
                    {
                        SrcGroup[i] = int.Parse(ss[i].Trim());
                    }
                }

                return SrcGroup;
            }

            static public String sc_putGroupArg(int[] group)
            {
                String ret = "{";
                for (int i = 0; i < group.Length; i++ )
                {
                    if (i == group.Length - 1)
                    {
                        ret += group[i];
                    }
                    else
                    {
                        ret += group[i] + ",";
                    }
                }
                ret += "}";

                return ret;
            }


            static public int sc_indexOfGroup(int[] group, int value) 
            {
                for (int i = 0; i < group.Length; i++ )
                {
                    if (group[i] == value)
                    {
                        return i;
                    }
                }

                return -1;
            }

            static public MapBlock[] sc_randomMapBlocks(ArrayList list)
            {
                MapBlock[] ret = new MapBlock[list.Count];
                list.CopyTo(ret);

                for (int i = 0; i < ret.Length; i++)
                {
                    int dst = Math.Abs(sc_Random.Next() % ret.Length);

                    MapBlock temp = ret[i];
                    ret[i] = ret[dst];
                    ret[dst] = temp;
                }

                return ret;
            }


            static public MapBlock sc_getFillableMapBlock(IScriptOP op, int srcType, int[] srcGroup, int x, int y )
            {
                int srcIndex = -1;
				MapLayer curlayer = op.CurMap.getCurLayer();

                switch (srcType)
                {
                    case sc_TypeTile:
						srcIndex = sc_indexOfGroup(srcGroup, op.CurMap.getTileID(curlayer, x, y));
						if (srcIndex >= 0 && op.CurMap.getAnimateID(curlayer, x, y) == 0)
                        {
                            MapBlock block = new MapBlock(x, y, srcGroup[srcIndex], -1);
                            return block;
                        }
                        break;
                    case sc_TypeAnim:
						srcIndex = sc_indexOfGroup(srcGroup, op.CurMap.getAnimateID(curlayer, x, y));
                        if (srcIndex >= 0)
                        {
                            MapBlock block = new MapBlock(x, y, -1, srcGroup[srcIndex]);
                            return block;
                        }
                        break;
                    case sc_TypeLayer:
						srcIndex = sc_indexOfGroup(srcGroup, -op.CurMap.getAnimateID(curlayer, x, y));
                        if (srcIndex >= 0)
                        {
                            MapBlock block = new MapBlock(x, y, -1, srcGroup[srcIndex]);
                            return block;
                        }
                        break;
                }

                return null;
            }


            static public void sc_FillDstBlocks(IScriptOP op, MapBlock[] blocks, int dstType, int[] dstGroup, int dstCount)
			{
				MapLayer curlayer = op.CurMap.getCurLayer();
                for (int i = 0; i < dstCount && i < blocks.Length; i++)
                {
                    int dst = dstGroup[i % dstGroup.Length];

                    switch (dstType)
                    { 
                        case sc_TypeTile:
							op.CurMap.putTile(curlayer, dst, blocks[i].X, blocks[i].Y);
                            break;
                        case sc_TypeAnim:
							op.CurMap.putAnimate(curlayer, dst, blocks[i].X, blocks[i].Y, false);
                            break;
                        case sc_TypeLayer:
							op.CurMap.putAnimate(curlayer, dst, blocks[i].X, blocks[i].Y, true);
                            break;
                    }
                }

            }

            // gobal


            static public Random sc_Random = new Random();

            static public int sc_RegionX = 0, sc_RegionY = 0, sc_RegionW, sc_RegionH;
            static public String sc_RegionShape = "rect";

            //


            //

            protected MapForm CurMap;

            public IScriptOP(MapForm mapform)
            {
                CurMap = mapform;
            }

            //


            abstract public String getFuncKey();

            abstract public void doScript();

            abstract protected String[] getArgs();

            abstract protected void setArgs(String[] args);

            abstract public String getCommet();

            //
           
            public String createScript()
            {
                String ret = getCommet() + "\n" + getFuncKey() + "(";
                String[] args = getArgs();
                for (int i = 0; i < args.Length; i++ )
                {
                    if (i == args.Length - 1)
                    {
                        ret += args[i];
                    }
                    else
                    {
                        ret += args[i] + ", ";
                    }
                }
                ret += ");\n";
                return ret;
            }

            public Boolean tryDoScript(String line) 
            { 
                line = line.Trim();

                int func = line.IndexOf(getFuncKey());

                if (func>=0)
                {
                    line = line.Substring(func + getFuncKey().Length).Trim();

                    if (line.StartsWith("("))
                    {
                        if (line.EndsWith(")"))
                        {
                            String[] sargs = sc_getArgs(line);
                            setArgs(sargs);
                            String[] dargs = getArgs();

                            if (sargs.Length == dargs.Length)
                            {
                                doScript();
                                return true;
                            }
                            else
                            {
                                throw new Exception("function\"" + getFuncKey() + "\" not support " + sargs.Length + " args!");
                            }
                        }
                        else
                        {
                            throw new Exception("function ( ) not Match!");
                        }
                    }
                }
                return false;
            }


        }

        //
        //--------------------------------------------------------------------------------------------------------

        class ScriptSetRandomSeed : IScriptOP
        {
            //
            private int seed;

            public ScriptSetRandomSeed(MapForm map, int s)
                : base(map)
            {
                seed = s;
            }
            public ScriptSetRandomSeed(MapForm map)
                : base(map)
            { 
            }

            override public String getCommet()
            {
                return "/// 设置当前随机数种子";
            }
            override public String getFuncKey()
            {
                return "SetRandomSeed";
            }


            override public void doScript() 
            {
                sc_Random = new Random(seed);
            }

            override protected String[] getArgs()
            {
                return new String[] { seed.ToString() };
            }

            override protected void setArgs(String[] args)
            {
                seed = int.Parse(args[0].Trim());
            }
        }


        class ScriptSetRegion : IScriptOP
        {
           

            //
            private int x,y,w,h;
            private String shape;

            public ScriptSetRegion(MapForm map, int x, int y, int w, int h, String shape)
                : base(map)
            {
                CurMap = map;
                this.x = x;
                this.y = y;
                this.w = w;
                this.h = h;
                this.shape = shape;
            }
            public ScriptSetRegion(MapForm map)
                : base(map)
            { 
            }

            override public String getCommet()
            {
                return "/// 设置当前脚本影响到的区域，即脚本的有效范围";
            }
            override public String getFuncKey()
            {
                return "SetRegion";
            }

            override public void doScript()
            {
                sc_RegionX = x;
                sc_RegionY = y;
                sc_RegionW = w;
                sc_RegionH = h;
                sc_RegionShape = shape;
            }

            override protected String[] getArgs()
            {
                return new String[] { x.ToString(), y.ToString(), w.ToString(), h.ToString(), shape };
            }

            override protected void setArgs(String[] args)
            {
                x = int.Parse(args[0].Trim());
                y = int.Parse(args[1].Trim());
                w = int.Parse(args[2].Trim());
                h = int.Parse(args[3].Trim());
                shape = args[4].Trim();
            }
        }


        class ScriptReplace : IScriptOP
        {
            int SrcType;
            int Src; 
            int DstType; 
            int Dst;

            public ScriptReplace(MapForm map, int srcType, int src, int dstType, int dst)
                : base(map)
            {
                this.SrcType = srcType;
                this.Src = src;
                this.DstType = dstType;
                this.Dst = dst;
            }
            public ScriptReplace(MapForm map)
                : base(map)
            { 
            }

            override public String getCommet()
            {
                return
                    "/// 替换指定的tile或anim成为新的tile或anim \n" +
                    "/// SrcType - 被替换的类型(tile或anim或layer); ps: (anim和layer编号是一样的，只是渲染方式不同，anim为正，layer为负)\n" +
                    "/// Src     - 被替换的tile编号或anim编号\n" +
                    "/// DstType - 要替换的类型\n" +
                    "/// Dst     - 要替换的tile编号或anim编号";
            }
            override public String getFuncKey()
            {
                return "Replace";
            }

            override public void doScript()
			{
				MapLayer curlayer = CurMap.getCurLayer();

                Boolean isSrcTile = SrcType == sc_TypeTile;
                Boolean isSrcAnim = SrcType == sc_TypeAnim || SrcType == sc_TypeLayer;

                Boolean isDstTile = DstType == sc_TypeTile;
                Boolean isDstAnim = DstType == sc_TypeAnim;
                Boolean isDstLayer = DstType == sc_TypeLayer;

				int sw = Math.Min(sc_RegionX + sc_RegionW, CurMap.layers.XCount);
				int sh = Math.Min(sc_RegionY + sc_RegionH, CurMap.layers.YCount);
                for (int x = sc_RegionX; x < sw; x++)
                {
                    for (int y = sc_RegionY; y < sh; y++)
                    {
						if ((isSrcTile && CurMap.getTileID(curlayer, x, y) == Src) ||
							(isSrcAnim && CurMap.getAnimateID(curlayer, x, y) == Src))
                        {
                            if (isDstTile)
                            {
								CurMap.putTile(curlayer, Dst, x, y);
                            }
                            else if (isDstAnim)
                            {
								CurMap.putAnimate(curlayer, Dst, x, y, false);
                            }
                            else if (isDstLayer)
                            {
								CurMap.putAnimate(curlayer, Dst, x, y, true);
                            }
                        }
                    }
                }
            }

            override protected String[] getArgs()
            {
                return new String[] { 
                    sc_putTileType(SrcType), 
                    Src.ToString(), 
                    sc_putTileType(DstType),
                    Dst.ToString()
                };
            }

            override protected void setArgs(String[] args)
            {
                SrcType = sc_getTileType(args[0]);
                Src = int.Parse(args[1].Trim());
                DstType = sc_getTileType(args[2]);
                Dst = int.Parse(args[3].Trim());
            }
        }


        class ScriptFill : IScriptOP
        {

            int SrcType;
            int[] SrcGroup;
            int DstType;
            int[] DstGroup;
            int DstCount;

            public ScriptFill(MapForm map, int srcType, int[] srcGroup, int dstType, int[] dstGroup, int dstCount)
                : base(map)
            {
                SrcType = srcType;
                SrcGroup = srcGroup;
                DstType = dstType;
                DstGroup = dstGroup;
                DstCount = dstCount;
            }
            public ScriptFill(MapForm map)
                : base(map)
            {
            }
            override public String getCommet()
            {
                return 
                    "/// 随机填充指定的tile或anim，为新的tile或anim \n" +
                    "/// SrcType  - 原地图中可被填充的类型(tile或anim或layer)\n" +
                    "/// SrcGroup - 原地图中可被填充的tile编号或anim编号组合{n,n...,n}\n" +
                    "/// DstType  - 要填充的类型\n" +
                    "/// DstGroup - 要填充的tile编号或anim编号组合\n" +
                    "/// DstCount - 要最大填充多少个" 
                    ;
            }
            override public String getFuncKey()
            {
                return "Fill";
            }

            override public void doScript()
            {
                ArrayList srcBlocks = new ArrayList();
                {
					int sw = Math.Min(sc_RegionX + sc_RegionW, CurMap.layers.XCount);
					int sh = Math.Min(sc_RegionY + sc_RegionH, CurMap.layers.YCount);
                    for (int x = sc_RegionX; x < sw; x++)
                    {
                        for (int y = sc_RegionY; y < sh; y++)
                        {
                            MapBlock block = sc_getFillableMapBlock(this, SrcType, SrcGroup, x, y);
                            if (block != null) srcBlocks.Add(block);
                        }
                    }
                }

                MapBlock[] blocks = sc_randomMapBlocks(srcBlocks);

                sc_FillDstBlocks(this, blocks, DstType, DstGroup, DstCount);



            }

            override protected String[] getArgs()
            {
                return new String[] { 
                    sc_putTileType(SrcType),
                    sc_putGroupArg(SrcGroup),
                    sc_putTileType(DstType),
                    sc_putGroupArg(DstGroup),
                    DstCount.ToString()
                };
            }

            override protected void setArgs(String[] args)
            {
                SrcType = sc_getTileType(args[0]);
                SrcGroup = sc_getGroupArg(args[1]);              
                DstType = sc_getTileType(args[2]);
                DstGroup = sc_getGroupArg(args[3]);
                DstCount = int.Parse(args[4].Trim());
            }
        }



        class ScriptFillGrid : IScriptOP
        {
            int GridW;
            int GridH;

            int SubX;
            int SubY;
            int SubW;
            int SubH;

            int SrcType;
            int[] SrcGroup;
            int DstType;
            int[] DstGroup;
            int DstCount;

            public ScriptFillGrid(MapForm map, 
                int gridW, int gridH, 
                int subX,int subY, int subW, int subH,
                int srcType, int[] srcGroup, 
                int dstType, int[] dstGroup, 
                int dstCount)
                : base(map)
            {
                GridW = gridW;
                GridH = gridH;

                SubX = subX;
                SubY = subY;
                SubW = subW;
                SubH = subH;

                SrcType = srcType;
                SrcGroup = srcGroup;
                DstType = dstType;
                DstGroup = dstGroup;
                DstCount = dstCount;
            }
            public ScriptFillGrid(MapForm map)
                : base(map)
            {
            }
            override public String getCommet()
            {
                return
                    "/// 在指定的网格范围内，随机填充指定范围内的tile或anim，为新的tile或anim \n" +
                    "/// GridW    - 原网格尺寸\n" +
                    "/// GridH    - \n" +
                    "/// SubX     - 在该网格尺寸中的矩形范围\n" +
                    "/// SubY     - \n" +
                    "/// SubW     - \n" +
                    "/// SubH     - \n" +
                    "/// SrcType  - 原地图中可被填充的类型(tile或anim或layer)\n" +
                    "/// SrcGroup - 原地图中可被填充的tile编号或anim编号组合{n,n...,n}\n" +
                    "/// DstType  - 要填充的类型\n" +
                    "/// DstGroup - 要填充的tile编号或anim编号组合\n" +
                    "/// DstCount - 要最大填充多少个"
                ;
            }
            override public String getFuncKey()
            {
                return "FillGrid";
            }

            override public void doScript()
            {
                System.Drawing.Rectangle gridRegion = new System.Drawing.Rectangle(
                       SubX, SubY, SubW, SubH
                       );

                int rw = (sc_RegionW) / GridW;
                int rh = (sc_RegionH) / GridH;
                for (int rx = 0; rx < rw; rx++)
                {
                    for (int ry = 0; ry < rh; ry++)
                    {
                        ArrayList srcBlocks = new ArrayList();
                        {
                            int sx = sc_RegionX + rx * GridW + SubX;
                            int sy = sc_RegionY + ry * GridH + SubY;
							int sw = Math.Min(sx + SubW, CurMap.layers.XCount);
							int sh = Math.Min(sy + SubH, CurMap.layers.YCount);

                            for (int x = sx; x < sw; x++)
                            {
                                for (int y = sy; y < sh; y++)
                                {
                                   // if (gridRegion.Contains(x % GridW, y % GridH))
                                    {
                                        MapBlock block = sc_getFillableMapBlock(this, SrcType, SrcGroup, x, y);
                                        if (block != null) srcBlocks.Add(block);
                                    }
                                }
                            }
                        }

                        MapBlock[] blocks = sc_randomMapBlocks(srcBlocks);

                        sc_FillDstBlocks(this, blocks, DstType, DstGroup, DstCount);

                    }
                }
            }

            override protected String[] getArgs()
            {
                return new String[] { 
                    GridW.ToString(),
                    GridH.ToString(),
                    SubX.ToString(),
                    SubY.ToString(),
                    SubW.ToString(),
                    SubH.ToString(),
                    sc_putTileType(SrcType),
                    sc_putGroupArg(SrcGroup),
                    sc_putTileType(DstType),
                    sc_putGroupArg(DstGroup),
                    DstCount.ToString()
                };
            }

            override protected void setArgs(String[] args)
            {
                GridW = int.Parse(args[0].Trim());
                GridH = int.Parse(args[1].Trim());
                SubX = int.Parse(args[2].Trim());
                SubY = int.Parse(args[3].Trim());
                SubW = int.Parse(args[4].Trim());
                SubH = int.Parse(args[5].Trim());
                SrcType = sc_getTileType(args[6]);
                SrcGroup = sc_getGroupArg(args[7]);
                DstType = sc_getTileType(args[8]);
                DstGroup = sc_getGroupArg(args[9]);
                DstCount = int.Parse(args[10].Trim());
            }
        }



        class ScriptSpawn : IScriptOP
        {
            int KeyType;
            int[] KeyGroup;
            int X;
            int Y;
            int W;
            int H;
            String Shape = "rect";

            int SrcType;
            int[] SrcGroup;
            int DstType;
            int[] DstGroup;
            int DstCount;

            public ScriptSpawn(MapForm map,
                int keyType, int[] keyGroup,
                int x, int y, int w, int h, String shape,
                int srcType, int[] srcGroup,
                int dstType, int[] dstGroup,
                int dstCount)
                : base(map)
            {
                KeyType = keyType;
                KeyGroup = keyGroup;
                X = x;
                Y = y;
                W = w;
                H = h;
                Shape = shape;

                SrcType = srcType;
                SrcGroup = srcGroup;
                DstType = dstType;
                DstGroup = dstGroup;
                DstCount = dstCount;
            }
            public ScriptSpawn(MapForm map)
                : base(map)
            {

            }

            override public String getCommet()
            {
                return
                    "/// 在指定的tile或anim周围随机生成若干个tile或anim \n" +
                    "/// KeyType  - 周围生成的参考点类型(tile或anim或layer)\n" +
                    "/// KeyGroup - 周围生成的参考点的tile编号或anim编号组合{n,n...,n}\n" +
                    "/// X        - 基于参考点的范围 eg:(-2,-2,4,4)即围绕该点周围2个半径范围内生成\n" +
                    "/// Y        - \n" +
                    "/// W        - \n" +
                    "/// H        - \n" + 
                    "/// Shape    - 当前只能指定 rect 范围\n" +
                    "/// SrcType  - 原地图中可被填充的类型(tile或anim或layer)\n" +
                    "/// SrcGroup - 原地图中可被填充的tile编号或anim编号组合{n,n...,n}\n" +
                    "/// DstType  - 要填充的类型\n" +
                    "/// DstGroup - 要填充的tile编号或anim编号组合\n" +
                    "/// DstCount - 要最大填充多少个"
                    ;
            }
            override public String getFuncKey()
            {
                return "Spawn";
            }

            override public void doScript()
            {

                ArrayList keyBlocks = new ArrayList();

                {
					int sw = Math.Min(sc_RegionX + sc_RegionW, CurMap.layers.XCount);
					int sh = Math.Min(sc_RegionY + sc_RegionH, CurMap.layers.YCount);
                    for (int x = sc_RegionX; x < sw; x++)
                    {
                        for (int y = sc_RegionY; y < sh; y++)
                        {
                            MapBlock block = sc_getFillableMapBlock(this, KeyType, KeyGroup, x, y);
                            if (block != null) keyBlocks.Add(block);
                        }
                    }
                }

                foreach (MapBlock block in keyBlocks)
                {
                    ArrayList srcBlocks = new ArrayList(DstCount);

                    int sx = Math.Max(block.X + X, 0);
                    int sy = Math.Max(block.Y + Y, 0);
					int sw = Math.Min(sx + W, CurMap.layers.XCount);
					int sh = Math.Min(sy + H, CurMap.layers.YCount);
                    for (int x = sx; x < sw; x++)
                    {
                        for (int y = sy; y < sh; y++)
                        {
                            MapBlock b = sc_getFillableMapBlock(this, SrcType, SrcGroup, x, y);
                            if (b != null) srcBlocks.Add(b);
                        }
                    }

                    MapBlock[] blocks = sc_randomMapBlocks(srcBlocks);

                    sc_FillDstBlocks(this, blocks, DstType, DstGroup, DstCount);

                }
            }

            override protected String[] getArgs()
            {
                return new String[] { 
                    sc_putTileType(KeyType),
                    sc_putGroupArg(KeyGroup),
                    X.ToString(),
                    Y.ToString(),
                    W.ToString(),
                    H.ToString(),
                    Shape,
                    sc_putTileType(SrcType),
                    sc_putGroupArg(SrcGroup),
                    sc_putTileType(DstType),
                    sc_putGroupArg(DstGroup),
                    DstCount.ToString()
                };
            }

            override protected void setArgs(String[] args)
            {
                KeyType = sc_getTileType(args[0]);
                KeyGroup = sc_getGroupArg(args[1]);
                X = int.Parse(args[2].Trim());
                Y = int.Parse(args[3].Trim());
                W = int.Parse(args[4].Trim());
                H = int.Parse(args[5].Trim());
                Shape = args[6];
                SrcType = sc_getTileType(args[7]);
                SrcGroup = sc_getGroupArg(args[8]);
                DstType = sc_getTileType(args[9]);
                DstGroup = sc_getGroupArg(args[10]);
                DstCount = int.Parse(args[11].Trim());
            }
        }



        class ScriptCopy : IScriptOP
        {
            int SX, SY, SW, SH;
            int DX, DY, DW, DH;

            int SrcType;
            int[] SrcGroup;

            public ScriptCopy(MapForm map, 
                int sx, int sy, int sw, int sh,
                int dx, int dy, int dw, int dh,
                int srcType, int[] srcGroup)
                : base(map)
            {
                SX = sx;
                SY = sy;
                SW = sw;
                SH = sh;

                DX = dx;
                DY = dy;
                DW = dw;
                DH = dh;

                SrcType = srcType;
                SrcGroup = srcGroup;
            }
            public ScriptCopy(MapForm map)
                : base(map)
            {
            }
            override public String getCommet()
            {
                return 
                    "/// 将制定范围的地图数据复制到另一范围\n" +
                    "/// SX, SY, SW, SH - 被复制的范围\n" +
                    "/// DX, DY, DW, DH - 复制到的范围\n" +
                    "/// SrcType  - 原地图中可被填充的类型(tile或anim或layer)\n" +
                    "/// SrcGroup - 原地图中可被填充的tile编号或anim编号组合{n,n...,n}"
                    ;
            }
            override public String getFuncKey()
            {
                return "Copy";
            }

            override public void doScript()
            {
                Boolean isSrcTile = SrcType == sc_TypeTile;
                Boolean isSrcAnim = SrcType == sc_TypeAnim || SrcType == sc_TypeLayer;
				MapLayer curlayer = CurMap.getCurLayer();
                {
					int sw = Math.Min(SX + SW, CurMap.layers.XCount);
					int sh = Math.Min(SY + SH, CurMap.layers.YCount);

                    int[][] clipTile = new int[SW][];
                    int[][] clipFlip = new int[SW][];
                    int[][] clipAnim = new int[SW][];

                    for (int x = SX, fx = 0; x < sw; x++, fx++)
                    {
                        clipTile[fx] = new int[SH];
                        clipFlip[fx] = new int[SH];
                        clipAnim[fx] = new int[SH];

                        for (int y = SY, fy=0; y < sh; y++, fy++)
                        {
							clipTile[fx][fy] = CurMap.getTileID(curlayer,x, y);
							clipFlip[fx][fy] = CurMap.getTileFlip(curlayer,x, y);
							clipAnim[fx][fy] = CurMap.getAnimateID(curlayer,x, y);
                        }
                    }

					int dw = Math.Min(DX + DW, CurMap.layers.XCount);
					int dh = Math.Min(DY + DH, CurMap.layers.YCount);

                    for (int x = DX, fx = 0; x < dw; x++, fx++)
                    {
                        for (int y = DY, fy = 0; y < dh; y++, fy++)
                        {
                            int srcIndex = -1;

                            if (isSrcTile)
                            {
								srcIndex = sc_indexOfGroup(SrcGroup, CurMap.getTileID(curlayer, x, y));
                            }
                            else if (isSrcAnim)
                            {
								srcIndex = sc_indexOfGroup(SrcGroup, CurMap.getAnimateID(curlayer, x, y));
                            }

                            if (srcIndex >= 0)
                            {
                                fx = fx % clipTile.Length;
                                fy = fy % clipTile[fx].Length;

								CurMap.putTile(curlayer, clipTile[fx][fy], x, y);
								CurMap.putFlip(curlayer, clipFlip[fx][fy], x, y);
								CurMap.putAnimate(curlayer, clipAnim[fx][fy], x, y, clipAnim[fx][fy] < 0);
                            }

                        }
                    }

                }
            }

            override protected String[] getArgs()
            {
                return new String[] { 
                    SX.ToString(),
                    SY.ToString(),
                    SW.ToString(),
                    SH.ToString(),
                    DX.ToString(),
                    DY.ToString(),
                    DW.ToString(),
                    DH.ToString(),
                    sc_putTileType(SrcType),
                    sc_putGroupArg(SrcGroup),
                };
            }

            override protected void setArgs(String[] args)
            {
                SX = int.Parse(args[0]);
                SY = int.Parse(args[1]);
                SW = int.Parse(args[2]);
                SH = int.Parse(args[3]);
                DX = int.Parse(args[4]);
                DY = int.Parse(args[5]);
                DW = int.Parse(args[6]);
                DH = int.Parse(args[7]);
                SrcType = sc_getTileType(args[8]);
                SrcGroup = sc_getGroupArg(args[9]);
            }
        }



    //--------------------------------------------------------------------------------------------------------
  

        /// <summary>
        /// 
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public String scriptRun(String script)
        {
            // remove rem
            while (true)
            {
                int gi = script.IndexOf("//");

                if (gi >= 0)
                {
                    int ge = script.IndexOf("\n", gi);
                    int gn = ge - gi + 1;
                    script = script.Remove(gi, gn);
                }
                else { break; }
            }


            String res = "";
            String line = "";
            int i = 0;

            try
            {
                String[] lines = script.Split(new char[] { ';' });

                for (i = 0; i < lines.Length; i++)
                {
                    line = lines[i];

                    if (new ScriptSetRandomSeed(this).tryDoScript(lines[i]))
                    { }
                    else if (new ScriptSetRegion(this).tryDoScript(lines[i]))
                    { }
                    else if (new ScriptReplace(this).tryDoScript(lines[i]))
                    { }
                    else if (new ScriptFill(this).tryDoScript(lines[i]))
                    { }
                    else if (new ScriptFillGrid(this).tryDoScript(lines[i]))
                    { }
                    else if (new ScriptSpawn(this).tryDoScript(lines[i]))
                    { }
                    else if (new ScriptCopy(this).tryDoScript(lines[i]))
                    { }
                }

                res = "Run script succeed !";
            }
            catch (Exception err) 
            { 
                res = 
                    "Error at func:" + i + "\n" + 
                    line + "\n\n" +
                    err.Message + "\n" + err.StackTrace; 
            }
           
            refreshMap();

            return res;
        }

        #endregion


        private void BtnFunc_Click(object sender, EventArgs e)
        {
                MapFormFunc mf = new MapFormFunc(this);
                mf.Show(this);
        }

        private void 替换ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (toolSelecter.Checked)
            {
                int sbx = dstRect.X / CellW;
                int sby = dstRect.Y / CellH;
                int xcount = dstRect.Width / CellW;
                int ycount = dstRect.Height / CellH;

                MapFormFunc mf = new MapFormFunc(this,
                    new ScriptSetRegion(this, sbx, sby, xcount, ycount, "rect").createScript() +
                    new ScriptReplace(this, 0, 0, 0, 0).createScript()
                    );
                mf.Show(this);
            }

           
        }

        private void 填充ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (toolSelecter.Checked)
            {
                int sbx = dstRect.X / CellW;
                int sby = dstRect.Y / CellH;
                int xcount = dstRect.Width / CellW;
                int ycount = dstRect.Height / CellH;

                MapFormFunc mf = new MapFormFunc(this,
                    new ScriptSetRandomSeed(this, IScriptOP.sc_Random.Next()).createScript() +
                    new ScriptSetRegion(this, sbx, sby, xcount, ycount, "rect").createScript() +
                    new ScriptFill(this, 0, new int[] { 0, }, 0, new int[] { 0 }, 0).createScript()
                    );
                mf.Show(this);
            }
        }

        private void 生成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (toolSelecter.Checked)
            {
                int sbx = dstRect.X / CellW;
                int sby = dstRect.Y / CellH;
                int xcount = dstRect.Width / CellW;
                int ycount = dstRect.Height / CellH;

                MapFormFunc mf = new MapFormFunc(this,
                    new ScriptSetRandomSeed(this, IScriptOP.sc_Random.Next()).createScript() +
                    new ScriptSetRegion(this, sbx, sby, xcount, ycount, "rect").createScript() +
                    new ScriptSpawn(this,
                    0, new int[] { 0, },
                    -2, -2, 4, 4, "rect",
                    0, new int[] { 0, },
                    0, new int[] { 0 }, 0).createScript()
                    );
                mf.Show(this);
            }
        }

        private void 网格填充ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (toolSelecter.Checked)
            {
                int sbx = dstRect.X / CellW;
                int sby = dstRect.Y / CellH;
                int xcount = dstRect.Width / CellW;
                int ycount = dstRect.Height / CellH;

                MapFormFunc mf = new MapFormFunc(this,
                   new ScriptSetRandomSeed(this, IScriptOP.sc_Random.Next()).createScript() +
                   new ScriptSetRegion(this, sbx, sby, xcount, ycount, "rect").createScript() +
                   new ScriptFillGrid(this, 
                   5, 5, 
                   1, 1, 4, 4, 
                   0, new int[] { 0, }, 
                   0, new int[] { 0 }, 0).createScript()
                   );
                mf.Show(this);
            }
        }

        private void 复制填充ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (toolSelecter.Checked)
            {
                int sbx = dstRect.X / CellW;
                int sby = dstRect.Y / CellH;
                int xcount = dstRect.Width / CellW;
                int ycount = dstRect.Height / CellH;

                MapFormFunc mf = new MapFormFunc(this,
                   new ScriptCopy(this, 
                   sbx, sby, xcount, ycount,
				   0, 0, layers.XCount, layers.YCount,
                   0, new int[] { 0 }).createScript()
                 );
                mf.Show(this);
            }
        }


        private void imageFlipToolStripButton2_DropDownClosed(object sender, EventArgs e)
        {
            flipIndex = imageFlipToolStripButton2.getFlipIndex();

            pictureBox1.Refresh();
        }

		private void button2_Click(object sender, EventArgs e)
		{

			StringBuilder sb = new StringBuilder(this.append_data);

			DataEdit dataedit = new DataEdit(sb);

			dataedit.ShowDialog(this);

			this.append_data = sb.ToString();
		}

		//-------------------------------------------------------------------------------------------------
		// map layers

		private int layer_index = 0;

		private void 添加图层ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			layers.addLayer();
			syncMapLayer();
		}

		private void 图层高亮ToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void 上移图层ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			layers.swarpLayer(layer_index, layer_index + 1);
			syncMapLayer();
		}

		private void 下移图层ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			layers.swarpLayer(layer_index, layer_index-1);
			syncMapLayer();
		}

		private void 删除图层ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (layers.getCount() > 1)
			{
				if (MessageBox.Show("确认要删除当前图层？", "确认", MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					layers.removeLayer(layer_index);
					layer_index--; 
					syncMapLayer();
				}
			}
			
		}

		private void 隐藏图层ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MapLayer layer = getCurLayer();
			layer.visible = !layer.visible; 
			syncMapLayer();
		}

		private void MenuLayer_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			foreach (ToolStripMenuItem tm in MenuLayer.DropDownItems)
			{
				tm.Checked = false;
			}
			((ToolStripMenuItem)e.ClickedItem).Checked = true;
			layer_index = (int)e.ClickedItem.Tag;
			MenuLayer.Text = ((ToolStripMenuItem)e.ClickedItem).Text;
			MenuLayer.ForeColor = e.ClickedItem.ForeColor;
			MenuLayer.BackColor = e.ClickedItem.BackColor;
		}

		private void layerItem_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) {
				ToolStripMenuItem item = ((ToolStripMenuItem)sender);
				int index = (int)item.Tag;
				MapLayer layer = layers.getLayer(index);
				layer.visible = !layer.visible;
				syncMapLayer();
			}
		}

		private void syncMapLayer()
		{
			layer_index = Math.Min(layer_index, layers.getCount()-1);
			layer_index = Math.Max(layer_index, 0);

			MenuLayer.DropDownItems.Clear();

			for (int L = 0; L < layers.getCount(); L++)
			{
				MapLayer layer = layers.getLayer(L);
				ToolStripMenuItem item_layer = new ToolStripMenuItem("图层 - " + L);
				item_layer.ToolTipText = "右键开关显示状态";
				item_layer.CheckOnClick = false;
				item_layer.Checked = (layer_index == L);
				item_layer.Tag = L;
				item_layer.MouseDown += new MouseEventHandler(layerItem_MouseDown);
				MenuLayer.DropDownItems.Add(item_layer);
				if (!layer.visible)
				{
					item_layer.ForeColor = System.Drawing.Color.Gray;
					item_layer.BackColor = System.Drawing.Color.DarkGray;
				}
				if (layer_index == L)
				{
					MenuLayer.Text = item_layer.Text;
					MenuLayer.ForeColor = item_layer.ForeColor;
					MenuLayer.BackColor = item_layer.BackColor;
				}
			}
			
			refreshMap();
		}

        //--------------------------------------------------------------------------------------------------------









    }
	/*
    class Animates 
    {

        ArrayList Frames = new ArrayList();

        public ArrayList SubPart = new ArrayList();
        public ArrayList SubFlip = new ArrayList();

        public Animates()
        {
           
        }

        public void frameAdd(ArrayList frame)
        {
            Frames.Add(frame);
        }

        public int frameGetCount()
        {
            return Frames.Count;
        }

        public int frameIndexOf(ArrayList frame)
        {
            for (int i = 0; i < Frames.Count;i++ )
            {
                if (Frames[i].Equals(frame))return i;

                if (((ArrayList)Frames[i]).Count != frame.Count) continue;

                Boolean ok = true;

                for (int j = 0; j < frame.Count;j++ )   
                {
                    if (((int)((ArrayList)Frames[i])[j]) != ((int)frame[j]))
                    {
                        ok = false;
                        break;
                    }
                }

                if (ok) return i;
            }

            return -1;
        }

        public ArrayList frameGetFrame(int index)
        {
            return (ArrayList)(Frames[index]);
        }

        public void subAdd(int part, int flip)
        {
            SubPart.Add(part);
            SubFlip.Add(flip);
        }

        public int subGetCount()
        {
            return SubPart.Count;
        }

        public int subIndexOf(int part, int flip)
        {
            if (!SubPart.Contains(part)) return -1;
            if (!SubFlip.Contains(flip)) return -1;

            for(int i=0;i<subGetCount();i++)
            {
                int p1 = SubPart.IndexOf(part, i, 1);
                int p2 = SubFlip.IndexOf(flip, i, 1);
                if (p1>=0 && p2>=0 && p1==p2)
                {
                    return i;
                }
            }
            return -1;
        }

    }
	*/
	//----------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class MapLayer : ISerializable
	{
		public int[][] MatrixTile;
		public int[][] MatrixAnim;
		public int[][] MatrixFlip;
		public int[][] MatrixTag;
		public bool visible = true;
		public MapLayer(int XCount, int YCount)
		{
			MatrixTile = new int[YCount][];
			MatrixTag = new int[YCount][];
			MatrixAnim = new int[YCount][];
			MatrixFlip = new int[YCount][];
			for (int i = 0; i < YCount; i++)
			{
				MatrixTile[i] = new int[XCount];
				MatrixTag[i] = new int[XCount];
				MatrixAnim[i] = new int[XCount];
				MatrixFlip[i] = new int[XCount];
			}
		}
		protected MapLayer(SerializationInfo info, StreamingContext context)
		{
			MatrixTile = (int[][])info.GetValue("MatrixTile", typeof(int[][]));
			MatrixTag = (int[][])info.GetValue("MatrixTag", typeof(int[][]));
			MatrixAnim = (int[][])info.GetValue("MatrixAnim", typeof(int[][]));
			MatrixFlip = (int[][])info.GetValue("MatrixFlip", typeof(int[][]));
		}
		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("MatrixTile", MatrixTile);
			info.AddValue("MatrixTag", MatrixTag);
			info.AddValue("MatrixAnim", MatrixAnim);
			info.AddValue("MatrixFlip", MatrixFlip);
		}

		public MapLayer copyDst(int sbx, int sby, int xcount, int ycount)
		{
			if (xcount <= 0 || ycount <= 0) return null;

			MapLayer ret = new MapLayer(xcount, ycount);
			
			for (int by = 0; by < ycount; by++)
			{
				for (int bx = 0; bx < xcount; bx++)
				{
					if (sby + by < MatrixTile.Length && sbx + bx < MatrixTile[by].Length)
					{
						ret.MatrixTile[by][bx] = this.MatrixTile[sby + by][sbx + bx];
						ret.MatrixTag[by][bx] = this.MatrixTag[sby + by][sbx + bx];
						ret.MatrixAnim[by][bx] = this.MatrixAnim[sby + by][sbx + bx];
						ret.MatrixFlip[by][bx] = this.MatrixFlip[sby + by][sbx + bx];
					}
				}
			}

			return ret;
		}

		public int xcount()
		{
			return MatrixTile[0].Length;
		}

		public int ycount()
		{
			return MatrixTile.Length;
		}

		public void resize(int XCount, int YCount)
		{
			if (YCount != MatrixTile.Length || XCount != MatrixTile[0].Length)
			{
				int[][] matrixTile = new int[YCount][];
				int[][] matrixTag = new int[YCount][];
				int[][] matrixAnim = new int[YCount][];
				int[][] matrixFlip = new int[YCount][];
				for (int y = 0; y < YCount; y++)
				{
					matrixTile[y] = new int[XCount];
					matrixTag[y] = new int[XCount];
					matrixAnim[y] = new int[XCount];
					matrixFlip[y] = new int[XCount];
					for (int x = 0; x < XCount; x++)
					{
						if (y < MatrixTile.Length && x < MatrixTile[y].Length)
						{
							matrixTile[y][x] = MatrixTile[y][x];
							matrixTag[y][x] = MatrixTag[y][x];
							matrixAnim[y][x] = MatrixAnim[y][x];
							matrixFlip[y][x] = MatrixFlip[y][x];
						}
					}
				}

				MatrixTile = matrixTile;
				MatrixTag = matrixTag;
				MatrixAnim = matrixAnim;
				MatrixFlip = matrixFlip;
			}
		}
	
	};
	//---------------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class MapLayers : ISerializable
	{
		public int XCount;
		public int YCount;
		public ArrayList layers;

		public MapLayers(int xcount, int ycount)
		{
			this.XCount = xcount;
			this.YCount = ycount;
			this.layers = new ArrayList();
		}
		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
		protected MapLayers(SerializationInfo info, StreamingContext context)
		{
			this.XCount = info.GetInt32("XCount");
			this.YCount = info.GetInt32("YCount");
			this.layers = (ArrayList)info.GetValue("layers", typeof(ArrayList));
		}
		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("XCount", XCount);
			info.AddValue("YCount", YCount);
			info.AddValue("layers", layers);
		}

		public int getCount()
		{
			return layers.Count;
		}
		public MapLayer getLayer(int i)
		{
			return (MapLayer)layers[i];
		}
		public void addLayer()
		{
			addLayer(new MapLayer(XCount, YCount));
		}
		public void addLayer(MapLayer layer)
		{
			layers.Add(layer);
		}
		public void removeLayer(int index)
		{
			layers.RemoveAt(index);
		}

		public void dstChangeMapSize(int xcount, int ycount)
		{
			XCount = xcount;
			YCount = ycount;

			for (int L = 0; L < layers.Count; L++ )
			{
				((MapLayer)layers[L]).resize(xcount, ycount);
			}
		}

		public void swarpLayer(int src, int dst)
		{
			if (src >= 0 && src < layers.Count &&
				dst >= 0 && dst < layers.Count)
			{
				Object src_o = layers[src];
				Object dst_o = layers[dst];
				layers[src] = dst_o;
				layers[dst] = src_o;
			}
		}
	};

}