﻿
namespace CellGameEdit.PM
{
    partial class ProjectForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectForm));
            this.resMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加图片组ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.levelMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加场景ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.打开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.精灵ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.地图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.精灵XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.复制ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.剪贴板ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_COPY_TILES = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_PASTE_TILES = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.打开ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.nodeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.objMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加对象ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.commandMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加属性列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resMenu.SuspendLayout();
            this.levelMenu.SuspendLayout();
            this.tileMenu.SuspendLayout();
            this.subMenu.SuspendLayout();
            this.nodeMenu.SuspendLayout();
            this.objMenu.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.commandMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // resMenu
            // 
            this.resMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加图片组ToolStripMenuItem});
            this.resMenu.Name = "resMenu";
            this.resMenu.Size = new System.Drawing.Size(137, 26);
            // 
            // 添加图片组ToolStripMenuItem
            // 
            this.添加图片组ToolStripMenuItem.Name = "添加图片组ToolStripMenuItem";
            this.添加图片组ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.添加图片组ToolStripMenuItem.Text = "添加图片组";
            this.添加图片组ToolStripMenuItem.Click += new System.EventHandler(this.添加图片组ToolStripMenuItem_Click);
            // 
            // levelMenu
            // 
            this.levelMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加场景ToolStripMenuItem});
            this.levelMenu.Name = "levelMenu";
            this.levelMenu.Size = new System.Drawing.Size(125, 26);
            // 
            // 添加场景ToolStripMenuItem
            // 
            this.添加场景ToolStripMenuItem.Name = "添加场景ToolStripMenuItem";
            this.添加场景ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.添加场景ToolStripMenuItem.Text = "添加场景";
            this.添加场景ToolStripMenuItem.Click += new System.EventHandler(this.添加场景ToolStripMenuItem_Click);
            // 
            // tileMenu
            // 
            this.tileMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开ToolStripMenuItem,
            this.添加ToolStripMenuItem,
            this.复制ToolStripMenuItem1,
            this.剪贴板ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.删除ToolStripMenuItem});
            this.tileMenu.Name = "tileMenu";
            this.tileMenu.Size = new System.Drawing.Size(113, 120);
            // 
            // 打开ToolStripMenuItem
            // 
            this.打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
            this.打开ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.打开ToolStripMenuItem.Text = "打开";
            this.打开ToolStripMenuItem.Click += new System.EventHandler(this.打开ToolStripMenuItem_Click);
            // 
            // 添加ToolStripMenuItem
            // 
            this.添加ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.精灵ToolStripMenuItem,
            this.地图ToolStripMenuItem,
            this.精灵XToolStripMenuItem});
            this.添加ToolStripMenuItem.Name = "添加ToolStripMenuItem";
            this.添加ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.添加ToolStripMenuItem.Text = "添加";
            // 
            // 精灵ToolStripMenuItem
            // 
            this.精灵ToolStripMenuItem.Name = "精灵ToolStripMenuItem";
            this.精灵ToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.精灵ToolStripMenuItem.Text = "精灵";
            this.精灵ToolStripMenuItem.Click += new System.EventHandler(this.精灵ToolStripMenuItem_Click);
            // 
            // 地图ToolStripMenuItem
            // 
            this.地图ToolStripMenuItem.Name = "地图ToolStripMenuItem";
            this.地图ToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.地图ToolStripMenuItem.Text = "地图";
            this.地图ToolStripMenuItem.Click += new System.EventHandler(this.地图ToolStripMenuItem_Click);
            // 
            // 精灵XToolStripMenuItem
            // 
            this.精灵XToolStripMenuItem.Name = "精灵XToolStripMenuItem";
            this.精灵XToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.精灵XToolStripMenuItem.Text = "精灵X";
            this.精灵XToolStripMenuItem.Click += new System.EventHandler(this.精灵XToolStripMenuItem_Click);
            // 
            // 复制ToolStripMenuItem1
            // 
            this.复制ToolStripMenuItem1.Name = "复制ToolStripMenuItem1";
            this.复制ToolStripMenuItem1.Size = new System.Drawing.Size(112, 22);
            this.复制ToolStripMenuItem1.Text = "复制";
            this.复制ToolStripMenuItem1.Click += new System.EventHandler(this.复制ToolStripMenuItem1_Click);
            // 
            // 剪贴板ToolStripMenuItem
            // 
            this.剪贴板ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_COPY_TILES,
            this.toolStripMenuItem_PASTE_TILES});
            this.剪贴板ToolStripMenuItem.Name = "剪贴板ToolStripMenuItem";
            this.剪贴板ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.剪贴板ToolStripMenuItem.Text = "剪贴板";
            this.剪贴板ToolStripMenuItem.Visible = false;
            // 
            // toolStripMenuItem_COPY_TILES
            // 
            this.toolStripMenuItem_COPY_TILES.Name = "toolStripMenuItem_COPY_TILES";
            this.toolStripMenuItem_COPY_TILES.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem_COPY_TILES.Text = "复制";
            this.toolStripMenuItem_COPY_TILES.Click += new System.EventHandler(this.toolStripMenuItem_COPY_TILES_Click);
            // 
            // toolStripMenuItem_PASTE_TILES
            // 
            this.toolStripMenuItem_PASTE_TILES.Name = "toolStripMenuItem_PASTE_TILES";
            this.toolStripMenuItem_PASTE_TILES.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem_PASTE_TILES.Text = "粘贴";
            this.toolStripMenuItem_PASTE_TILES.Click += new System.EventHandler(this.toolStripMenuItem_PASTE_TILES_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(109, 6);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // subMenu
            // 
            this.subMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开ToolStripMenuItem1,
            this.复制ToolStripMenuItem,
            this.删除ToolStripMenuItem1});
            this.subMenu.Name = "mapMenu";
            this.subMenu.Size = new System.Drawing.Size(101, 70);
            // 
            // 打开ToolStripMenuItem1
            // 
            this.打开ToolStripMenuItem1.Name = "打开ToolStripMenuItem1";
            this.打开ToolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.打开ToolStripMenuItem1.Text = "打开";
            this.打开ToolStripMenuItem1.Click += new System.EventHandler(this.打开ToolStripMenuItem1_Click);
            // 
            // 复制ToolStripMenuItem
            // 
            this.复制ToolStripMenuItem.Name = "复制ToolStripMenuItem";
            this.复制ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.复制ToolStripMenuItem.Text = "复制";
            this.复制ToolStripMenuItem.Click += new System.EventHandler(this.复制ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem1
            // 
            this.删除ToolStripMenuItem1.Name = "删除ToolStripMenuItem1";
            this.删除ToolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.删除ToolStripMenuItem1.Text = "删除";
            this.删除ToolStripMenuItem1.Click += new System.EventHandler(this.删除ToolStripMenuItem1_Click);
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.ContextMenuStrip = this.nodeMenu;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.FullRowSelect = true;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.LabelEdit = true;
            this.treeView1.Location = new System.Drawing.Point(0, 25);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(234, 698);
            this.treeView1.TabIndex = 9;
            this.treeView1.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView1_BeforeLabelEdit);
            this.treeView1.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView1_AfterLabelEdit);
            this.treeView1.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCollapse);
            this.treeView1.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterExpand);
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView1_ItemDrag);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            // 
            // nodeMenu
            // 
            this.nodeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.刷新ToolStripMenuItem});
            this.nodeMenu.Name = "nodeMenu";
            this.nodeMenu.Size = new System.Drawing.Size(101, 26);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.刷新ToolStripMenuItem.Text = "刷新";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "close12.png");
            this.imageList1.Images.SetKeyName(1, "icon_action.png");
            this.imageList1.Images.SetKeyName(2, "icon_affect.png");
            this.imageList1.Images.SetKeyName(3, "icon_camera.png");
            this.imageList1.Images.SetKeyName(4, "icon_condition.png");
            this.imageList1.Images.SetKeyName(5, "icon_edit.png");
            this.imageList1.Images.SetKeyName(6, "icon_error.png");
            this.imageList1.Images.SetKeyName(7, "icon_event.png");
            this.imageList1.Images.SetKeyName(8, "icon_grid.png");
            this.imageList1.Images.SetKeyName(9, "icon_hd.png");
            this.imageList1.Images.SetKeyName(10, "icon_layer.png");
            this.imageList1.Images.SetKeyName(11, "icon_quest.ico");
            this.imageList1.Images.SetKeyName(12, "icon_quest.png");
            this.imageList1.Images.SetKeyName(13, "icon_quest_condition.png");
            this.imageList1.Images.SetKeyName(14, "icon_quest_event.png");
            this.imageList1.Images.SetKeyName(15, "icon_quest_result.png");
            this.imageList1.Images.SetKeyName(16, "icon_res.png");
            this.imageList1.Images.SetKeyName(17, "icon_res_1.png");
            this.imageList1.Images.SetKeyName(18, "icon_res_2.png");
            this.imageList1.Images.SetKeyName(19, "icon_res_3.png");
            this.imageList1.Images.SetKeyName(20, "icon_res_4.png");
            this.imageList1.Images.SetKeyName(21, "icon_res_5.png");
            this.imageList1.Images.SetKeyName(22, "icon_res_6.png");
            this.imageList1.Images.SetKeyName(23, "icon_res_7.png");
            this.imageList1.Images.SetKeyName(24, "icon_res_8.png");
            this.imageList1.Images.SetKeyName(25, "icon_res_9.png");
            this.imageList1.Images.SetKeyName(26, "icon_run.png");
            this.imageList1.Images.SetKeyName(27, "icon_scene.ico");
            this.imageList1.Images.SetKeyName(28, "icon_scene.png");
            this.imageList1.Images.SetKeyName(29, "icon_scene_graph.png");
            this.imageList1.Images.SetKeyName(30, "icon_talk.png");
            this.imageList1.Images.SetKeyName(31, "icon_trigger.png");
            this.imageList1.Images.SetKeyName(32, "icons_tool_bar1.png");
            this.imageList1.Images.SetKeyName(33, "icons_tool_bar2.png");
            // 
            // objMenu
            // 
            this.objMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加对象ToolStripMenuItem});
            this.objMenu.Name = "objMenu";
            this.objMenu.Size = new System.Drawing.Size(125, 26);
            // 
            // 添加对象ToolStripMenuItem
            // 
            this.添加对象ToolStripMenuItem.Name = "添加对象ToolStripMenuItem";
            this.添加对象ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.添加对象ToolStripMenuItem.Text = "添加对象";
            this.添加对象ToolStripMenuItem.Click += new System.EventHandler(this.添加对象ToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(234, 25);
            this.toolStrip1.TabIndex = 11;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(36, 22);
            this.toolStripButton1.Text = "刷新";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // commandMenu
            // 
            this.commandMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加属性列表ToolStripMenuItem});
            this.commandMenu.Name = "objMenu";
            this.commandMenu.Size = new System.Drawing.Size(149, 26);
            // 
            // 添加属性列表ToolStripMenuItem
            // 
            this.添加属性列表ToolStripMenuItem.Name = "添加属性列表ToolStripMenuItem";
            this.添加属性列表ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.添加属性列表ToolStripMenuItem.Text = "添加属性列表";
            this.添加属性列表ToolStripMenuItem.Click += new System.EventHandler(this.添加属性列表ToolStripMenuItem_Click);
            // 
            // ProjectForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(234, 723);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Location = new System.Drawing.Point(3, 3);
            this.Name = "ProjectForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ProjectForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProjectForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProjectForm_FormClosed);
            this.Load += new System.EventHandler(this.ProjectForm_Load);
            this.Shown += new System.EventHandler(this.ProjectForm_Shown);
            this.resMenu.ResumeLayout(false);
            this.levelMenu.ResumeLayout(false);
            this.tileMenu.ResumeLayout(false);
            this.subMenu.ResumeLayout(false);
            this.nodeMenu.ResumeLayout(false);
            this.objMenu.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.commandMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip resMenu;
        private System.Windows.Forms.ToolStripMenuItem 添加图片组ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip levelMenu;
        private System.Windows.Forms.ToolStripMenuItem 添加场景ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip tileMenu;
        private System.Windows.Forms.ToolStripMenuItem 打开ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 精灵ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 地图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip subMenu;
        private System.Windows.Forms.ToolStripMenuItem 打开ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip objMenu;
        private System.Windows.Forms.ToolStripMenuItem 添加对象ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip nodeMenu;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem 复制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 复制ToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip commandMenu;
        private System.Windows.Forms.ToolStripMenuItem 添加属性列表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 剪贴板ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_COPY_TILES;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_PASTE_TILES;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 精灵XToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
    }
}