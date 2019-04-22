namespace CellGameEdit
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新建ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.清理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.另存为toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.关闭ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导入脚本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.脚本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.combo_GlobalScriptList = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.自定义脚本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.currentPrjScript_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.javaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.显示输出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.项目ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.选项ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolCfgOutputEncoding = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImageOutput2M = new System.Windows.Forms.ToolStripMenuItem();
            this.toolToolPremultiplyAlphaStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemGobalImgeConvertScript = new System.Windows.Forms.ToolStripMenuItem();
            this.枚举查看器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.改变渲染字体ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.导入脚本ToolStripMenuItem,
            this.combo_GlobalScriptList,
            this.toolStripMenuItem4,
            this.帮助ToolStripMenuItem,
            this.项目ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1059, 38);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建ToolStripMenuItem,
            this.打开ToolStripMenuItem,
            this.toolStripMenuItem2,
            this.清理ToolStripMenuItem,
            this.保存ToolStripMenuItem,
            this.另存为toolStripMenuItem8,
            this.toolStripMenuItem3,
            this.关闭ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.退出ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(58, 32);
            this.文件ToolStripMenuItem.Text = "文件";
            this.文件ToolStripMenuItem.DropDownOpening += new System.EventHandler(this.fileToolStripMenuItem_DropDownOpening);
            // 
            // 新建ToolStripMenuItem
            // 
            this.新建ToolStripMenuItem.Name = "新建ToolStripMenuItem";
            this.新建ToolStripMenuItem.Size = new System.Drawing.Size(298, 30);
            this.新建ToolStripMenuItem.Text = "新建";
            this.新建ToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // 打开ToolStripMenuItem
            // 
            this.打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
            this.打开ToolStripMenuItem.Size = new System.Drawing.Size(298, 30);
            this.打开ToolStripMenuItem.Text = "打开";
            this.打开ToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(295, 6);
            // 
            // 清理ToolStripMenuItem
            // 
            this.清理ToolStripMenuItem.Name = "清理ToolStripMenuItem";
            this.清理ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.清理ToolStripMenuItem.Size = new System.Drawing.Size(298, 30);
            this.清理ToolStripMenuItem.Text = "保存并清理";
            this.清理ToolStripMenuItem.Click += new System.EventHandler(this.cleanToolStripMenuItem_Click);
            // 
            // 保存ToolStripMenuItem
            // 
            this.保存ToolStripMenuItem.Name = "保存ToolStripMenuItem";
            this.保存ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.保存ToolStripMenuItem.Size = new System.Drawing.Size(298, 30);
            this.保存ToolStripMenuItem.Text = "保存";
            this.保存ToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // 另存为toolStripMenuItem8
            // 
            this.另存为toolStripMenuItem8.Name = "另存为toolStripMenuItem8";
            this.另存为toolStripMenuItem8.Size = new System.Drawing.Size(298, 30);
            this.另存为toolStripMenuItem8.Text = "另存为";
            this.另存为toolStripMenuItem8.Click += new System.EventHandler(this.saveOtherToolStripMenuItem8_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(295, 6);
            // 
            // 关闭ToolStripMenuItem
            // 
            this.关闭ToolStripMenuItem.Enabled = false;
            this.关闭ToolStripMenuItem.Name = "关闭ToolStripMenuItem";
            this.关闭ToolStripMenuItem.Size = new System.Drawing.Size(298, 30);
            this.关闭ToolStripMenuItem.Text = "关闭";
            this.关闭ToolStripMenuItem.Visible = false;
            this.关闭ToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(295, 6);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(298, 30);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // 导入脚本ToolStripMenuItem
            // 
            this.导入脚本ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.脚本ToolStripMenuItem,
            this.删除ToolStripMenuItem});
            this.导入脚本ToolStripMenuItem.Name = "导入脚本ToolStripMenuItem";
            this.导入脚本ToolStripMenuItem.Size = new System.Drawing.Size(94, 32);
            this.导入脚本ToolStripMenuItem.Text = "公共脚本";
            // 
            // 脚本ToolStripMenuItem
            // 
            this.脚本ToolStripMenuItem.Name = "脚本ToolStripMenuItem";
            this.脚本ToolStripMenuItem.Size = new System.Drawing.Size(128, 30);
            this.脚本ToolStripMenuItem.Text = "添加";
            this.脚本ToolStripMenuItem.Click += new System.EventHandler(this.importScriptToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(128, 30);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.deleteScriptToolStripMenuItem_Click);
            // 
            // combo_GlobalScriptList
            // 
            this.combo_GlobalScriptList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_GlobalScriptList.Name = "combo_GlobalScriptList";
            this.combo_GlobalScriptList.Size = new System.Drawing.Size(180, 32);
            this.combo_GlobalScriptList.DropDown += new System.EventHandler(this.toolStripComboBox1_DropDown);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.自定义脚本ToolStripMenuItem,
            this.currentPrjScript_ToolStripMenuItem,
            this.toolStripMenuItem5,
            this.javaToolStripMenuItem});
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(58, 32);
            this.toolStripMenuItem4.Text = "导出";
            this.toolStripMenuItem4.DropDownOpening += new System.EventHandler(this.currentPrjScript_ToolStripMenuItem_DropDownOpening);
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // 自定义脚本ToolStripMenuItem
            // 
            this.自定义脚本ToolStripMenuItem.Name = "自定义脚本ToolStripMenuItem";
            this.自定义脚本ToolStripMenuItem.Size = new System.Drawing.Size(200, 30);
            this.自定义脚本ToolStripMenuItem.Text = "当前公共脚本";
            this.自定义脚本ToolStripMenuItem.Click += new System.EventHandler(this.customScriptToolStripMenuItem_Click);
            // 
            // currentPrjScript_ToolStripMenuItem
            // 
            this.currentPrjScript_ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem6});
            this.currentPrjScript_ToolStripMenuItem.Name = "currentPrjScript_ToolStripMenuItem";
            this.currentPrjScript_ToolStripMenuItem.Size = new System.Drawing.Size(200, 30);
            this.currentPrjScript_ToolStripMenuItem.Text = "当前工程脚本";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(69, 6);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(197, 6);
            // 
            // javaToolStripMenuItem
            // 
            this.javaToolStripMenuItem.Enabled = false;
            this.javaToolStripMenuItem.Name = "javaToolStripMenuItem";
            this.javaToolStripMenuItem.Size = new System.Drawing.Size(200, 30);
            this.javaToolStripMenuItem.Text = "Java代码";
            this.javaToolStripMenuItem.Visible = false;
            this.javaToolStripMenuItem.Click += new System.EventHandler(this.javaToolStripMenuItem_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关于ToolStripMenuItem,
            this.toolStripMenuItem7,
            this.显示输出ToolStripMenuItem});
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(58, 32);
            this.帮助ToolStripMenuItem.Text = "帮助";
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.关于ToolStripMenuItem.Text = "关于";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(164, 30);
            this.toolStripMenuItem7.Text = "使用说明";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.toolStripMenuItem7_Click);
            // 
            // 显示输出ToolStripMenuItem
            // 
            this.显示输出ToolStripMenuItem.Name = "显示输出ToolStripMenuItem";
            this.显示输出ToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.显示输出ToolStripMenuItem.Text = "显示输出";
            this.显示输出ToolStripMenuItem.Click += new System.EventHandler(this.showOutputToolStripMenuItem_Click);
            // 
            // 项目ToolStripMenuItem
            // 
            this.项目ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.选项ToolStripMenuItem,
            this.menuItemGobalImgeConvertScript,
            this.枚举查看器ToolStripMenuItem,
            this.改变渲染字体ToolStripMenuItem});
            this.项目ToolStripMenuItem.Name = "项目ToolStripMenuItem";
            this.项目ToolStripMenuItem.Size = new System.Drawing.Size(58, 32);
            this.项目ToolStripMenuItem.Text = "工具";
            // 
            // 选项ToolStripMenuItem
            // 
            this.选项ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolCfgOutputEncoding,
            this.toolStripMenuItemImageOutput2M,
            this.toolToolPremultiplyAlphaStripMenuItem});
            this.选项ToolStripMenuItem.Name = "选项ToolStripMenuItem";
            this.选项ToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.选项ToolStripMenuItem.Text = "输出";
            // 
            // CfgOutputEncoding
            // 
            this.toolCfgOutputEncoding.CheckOnClick = true;
            this.toolCfgOutputEncoding.Name = "CfgOutputEncoding";
            this.toolCfgOutputEncoding.Size = new System.Drawing.Size(252, 30);
            this.toolCfgOutputEncoding.Text = "输出编码字符信息";
            this.toolCfgOutputEncoding.CheckedChanged += new System.EventHandler(this.CfgOutputEncoding_CheckedChanged);
            this.toolCfgOutputEncoding.Click += new System.EventHandler(this.CfgOutputEncoding_Click);
            // 
            // toolStripMenuItemImageOutput2M
            // 
            this.toolStripMenuItemImageOutput2M.Checked = true;
            this.toolStripMenuItemImageOutput2M.CheckOnClick = true;
            this.toolStripMenuItemImageOutput2M.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItemImageOutput2M.Name = "toolStripMenuItemImageOutput2M";
            this.toolStripMenuItemImageOutput2M.Size = new System.Drawing.Size(252, 30);
            this.toolStripMenuItemImageOutput2M.Text = "输出图片2的冥";
            this.toolStripMenuItemImageOutput2M.CheckedChanged += new System.EventHandler(this.toolStripMenuItemImageOutput2M_CheckedChanged);
            this.toolStripMenuItemImageOutput2M.Click += new System.EventHandler(this.toolStripMenuItemImageOutput2M_Click);
            // 
            // premultiplyAlphaToolStripMenuItem
            // 
            this.toolToolPremultiplyAlphaStripMenuItem.Checked = true;
            this.toolToolPremultiplyAlphaStripMenuItem.CheckOnClick = true;
            this.toolToolPremultiplyAlphaStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolToolPremultiplyAlphaStripMenuItem.Enabled = false;
            this.toolToolPremultiplyAlphaStripMenuItem.Name = "premultiplyAlphaToolStripMenuItem";
            this.toolToolPremultiplyAlphaStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.toolToolPremultiplyAlphaStripMenuItem.Text = "PremultiplyAlpha";
            this.toolToolPremultiplyAlphaStripMenuItem.CheckedChanged += new System.EventHandler(this.premultiplyAlphaToolStripMenuItem_CheckedChanged);
            this.toolToolPremultiplyAlphaStripMenuItem.Click += new System.EventHandler(this.premultiplyAlphaToolStripMenuItem_Click);
            // 
            // menuItemGobalImgeConvertScript
            // 
            this.menuItemGobalImgeConvertScript.Name = "menuItemGobalImgeConvertScript";
            this.menuItemGobalImgeConvertScript.Size = new System.Drawing.Size(252, 30);
            this.menuItemGobalImgeConvertScript.Text = "默认图片转换脚本";
            // 
            // 枚举查看器ToolStripMenuItem
            // 
            this.枚举查看器ToolStripMenuItem.Name = "枚举查看器ToolStripMenuItem";
            this.枚举查看器ToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.枚举查看器ToolStripMenuItem.Text = "枚举查看器";
            this.枚举查看器ToolStripMenuItem.Click += new System.EventHandler(this.enumViewerToolStripMenuItem_Click);
            // 
            // 改变渲染字体ToolStripMenuItem
            // 
            this.改变渲染字体ToolStripMenuItem.Name = "改变渲染字体ToolStripMenuItem";
            this.改变渲染字体ToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.改变渲染字体ToolStripMenuItem.Text = "改变渲染字体";
            this.改变渲染字体ToolStripMenuItem.Click += new System.EventHandler(this.changeFontToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 632);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1059, 34);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1059, 666);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cell Game Editor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关闭ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新建ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem javaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 自定义脚本ToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox combo_GlobalScriptList;
        private System.Windows.Forms.ToolStripMenuItem 显示输出ToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem 导入脚本ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 脚本ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem currentPrjScript_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ToolStripMenuItem 另存为toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem 清理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 项目ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 选项ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolCfgOutputEncoding;
        private System.Windows.Forms.ToolStripMenuItem 枚举查看器ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 改变渲染字体ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemGobalImgeConvertScript;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageOutput2M;
        private System.Windows.Forms.ToolStripMenuItem toolToolPremultiplyAlphaStripMenuItem;
    }
}

