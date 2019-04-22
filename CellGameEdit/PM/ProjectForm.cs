using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Runtime.Serialization.Formatters;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using CellGameEdit.PM.plugin;
using System.Reflection;


namespace CellGameEdit.PM
{

    [Serializable]
    public partial class ProjectForm : Form, ISerializable
    {
        private static ProjectForm curInstance;
        static public ProjectForm getInstance()
        {
            return curInstance;
        }
        static public bool isDebug = false;
        static public Boolean is_console = false;
        static private DirectoryInfo workSpaceDir;
        static private FileInfo workCpjName;
        static public String workSpace
        {
            get { return workSpaceDir != null ? workSpaceDir.FullName : ""; }
            set { workSpaceDir = new DirectoryInfo(value); }
        }
        static public String workName
        {
            get { return workSpaceDir != null ? workCpjName.FullName : ""; }
            set { workCpjName = new FileInfo(value); }
        }
        static public DirectoryInfo WorkSpaceDir
        {
            get { return workSpaceDir; }
        }
        static public FileInfo WorkCpjFile
        {
            get { return workCpjName; }
        }

        static public String getEnumsDir()
        {
            return workSpace + @"\enums";
        }

        /*此状态不序列化子对象*/
        static private Boolean IsCopy = false;
        static public Boolean isCopying()
        {
            return IsCopy;
        }
        

        TreeNode nodeReses;
        TreeNode nodeLevels;
        TreeNode nodeObjects;
        TreeNode nodeCommands;

        //ArrayList formGroup;
        Dictionary<TreeNode, IEditForm> formTable;


        private EventTemplatePlugin current_event_plugin;
        private WorldAddUnitForm form_world_add_unit;


        //-----------------------------------------------------------------------------------------------------------------------------------
        // new 
        public ProjectForm()
        {
            curInstance = this;
            InitializeComponent();
            loadDLL();
            // formGroup = new ArrayList();
            formTable = new Dictionary<TreeNode, IEditForm>();

            nodeReses = new TreeNode("资源");
            nodeObjects = new TreeNode("对象");
            nodeLevels = new TreeNode("场景");
            nodeCommands = new TreeNode("命令");

            nodeReses.ContextMenuStrip = this.resMenu;
            nodeObjects.ContextMenuStrip = this.objMenu;
            nodeLevels.ContextMenuStrip = this.levelMenu;
            nodeCommands.ContextMenuStrip = this.commandMenu;

            treeView1.Nodes.Add(nodeReses);
            treeView1.Nodes.Add(nodeObjects);
            treeView1.Nodes.Add(nodeLevels);
            treeView1.Nodes.Add(nodeCommands);

            RefreshNodeName();
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected ProjectForm(SerializationInfo info, StreamingContext context)
        {
            curInstance = this;

            InitializeComponent();
            loadDLL();
            try
            {
                nodeReses = (TreeNode)info.GetValue("nodeReses", typeof(TreeNode));
                nodeLevels = (TreeNode)info.GetValue("nodeLevels", typeof(TreeNode));
                nodeObjects = (TreeNode)info.GetValue("nodeObjects", typeof(TreeNode));
                nodeCommands = (TreeNode)info.GetValue("nodeCommands", typeof(TreeNode));
                nodeReses.ContextMenuStrip = this.resMenu;
                nodeObjects.ContextMenuStrip = this.objMenu;
                nodeLevels.ContextMenuStrip = this.levelMenu;
                nodeCommands.ContextMenuStrip = this.commandMenu;

                //                 this.formTable = new Dictionary<TreeNode, IEditForm>();
                //                 try
                //                 {
                //                     Hashtable ft = (Hashtable)info.GetValue("formTable", typeof(Hashtable));
                //                     ft.OnDeserialization(this);
                //                     foreach (DictionaryEntry e in ft)
                //                     {
                //                         this.formTable.Add(e.Key as TreeNode, e.Value as IEditForm);
                //                     }
                //                 }
                //                 catch (Exception err) { }

                mLoading = new LoadingTask(info, context);


            }
            catch (Exception err)
            {
                MessageBox.Show("构造工程出错 !\n" + err.Message + "\n" + err.StackTrace + "  at  " + err.Message);
            }
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            RefreshNodeName();

            // info.AddValue("formGroup", formGroup);
            info.AddValue("nodeReses", nodeReses);
            info.AddValue("nodeObjects", nodeObjects);
            info.AddValue("nodeLevels", nodeLevels);
            info.AddValue("nodeCommands", nodeCommands);

            {
                Hashtable ft = new Hashtable();
                foreach (KeyValuePair<TreeNode, IEditForm> e in formTable)
                {
                    ft.Add(e.Key, e.Value);
                }
                info.AddValue("formTable", ft);
            }

        }

        private void ProjectForm_Load(object sender, EventArgs e)
        {

        }

        public void LoadOver()
        {
            if (mLoading != null)
            {
                mLoading.Init(this);
                mLoading = null;
            }
        }
        private LoadingTask mLoading;
        class LoadingTask
        {
            Hashtable ft;

            public LoadingTask(SerializationInfo info, StreamingContext context)
            {
                ft = (Hashtable)info.GetValue("formTable", typeof(Hashtable));
                ft.OnDeserialization(this);
            }

            public void Init(ProjectForm prj)
            {
                prj.formTable = new Dictionary<TreeNode, IEditForm>();

                foreach (DictionaryEntry e in ft)
                {
                    prj.formTable.Add(e.Key as TreeNode, e.Value as IEditForm);
                }

                foreach (DictionaryEntry e in ft)
                {
                    IEditForm form = e.Value as IEditForm;
                    form.LoadOver(prj);
                }

                foreach (TreeNode node in prj.nodeReses.Nodes)
                {
                    node.ContextMenuStrip = prj.tileMenu;

                    foreach (TreeNode subnode in node.Nodes)
                    {
                        subnode.ContextMenuStrip = prj.subMenu;
                    }
                }
                foreach (TreeNode node in prj.nodeLevels.Nodes)
                {
                    node.ContextMenuStrip = prj.subMenu;
                }
                foreach (TreeNode node in prj.nodeObjects.Nodes)
                {
                    node.ContextMenuStrip = prj.subMenu;
                }
                foreach (TreeNode node in prj.nodeCommands.Nodes)
                {
                    node.ContextMenuStrip = prj.subMenu;
                }

                prj.treeView1.Nodes.Add(prj.nodeReses);
                prj.treeView1.Nodes.Add(prj.nodeObjects);
                prj.treeView1.Nodes.Add(prj.nodeLevels);
                prj.treeView1.Nodes.Add(prj.nodeCommands);

                prj.RefreshNodeName();

            }
        }


        private void loadDLL()
        {
            current_event_plugin = new FormEventTemplate();

            try
            {
                //current_event_plugin = new KingII.KingIIEventTemplate();
                String pdir = Application.StartupPath + "\\plugins";
                if (System.IO.Directory.Exists(pdir))
                {
                    ArrayList loaddll = new ArrayList();
                    foreach (String file in System.IO.Directory.GetFiles(pdir))
                    {
                        if (file.ToLower().EndsWith(".dll"))
                        {
                            try
                            {
                                //Make an array for the list of assemblies.
                                Assembly assembly = Assembly.LoadFile(file);
                                loaddll.Add(assembly);
                            }
                            catch (Exception err)
                            {
                                MessageBox.Show(err.Message);
                            }
                        }
                    }

                    foreach (Assembly assembly in loaddll)
                    {
                        try
                        {
                            foreach (Type type in assembly.GetTypes())
                            {
                                if (typeof(EventTemplatePlugin).IsAssignableFrom(type))
                                {
                                    Object et = assembly.CreateInstance(type.FullName);
                                    addEventPlugin((EventTemplatePlugin)et);
                                }
                            }
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.Message);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("插件加载: " + err.Message);
            }
        }

        private void addEventPlugin(EventTemplatePlugin ep)
        {
            if (ep != null)
            {
                ep.initPlugin(Application.StartupPath + "\\plugins");
                current_event_plugin = ep;
            }
        }

        private void loadOver()
        {
            List<MapForm> form_maps = new List<MapForm>();
            List<SpriteForm> form_sprites = new List<SpriteForm>();
            List<SpriteXForm> form_spritexs = new List<SpriteXForm>();
            List<ImagesForm> form_images = new List<ImagesForm>();

            foreach (TreeNode node in nodeReses.Nodes)
            {
                Form imgf = getForm(node) as Form;

                if (imgf != null && imgf.GetType() == typeof(ImagesForm))
                {
                    form_images.Add(imgf as ImagesForm);
                }

                foreach (TreeNode subnode in node.Nodes)
                {
                    Form sf = getForm(subnode) as Form;
                    if (sf.GetType() == typeof(MapForm))
                    {
                        form_maps.Add(sf as MapForm);
                        MapForm subf = ((MapForm)sf);
                        subf.ChangeSuper(getImagesFormByName(subf.superName));
                    }
                    else if (sf.GetType() == typeof(SpriteForm))
                    {
                        form_sprites.Add(sf as SpriteForm);
                        SpriteForm subf = ((SpriteForm)sf);
                        subf.ChangeSuper(getImagesFormByName(subf.superName));
                    }
                    else if (sf.GetType() == typeof(SpriteXForm))
                    {
                        form_spritexs.Add(sf as SpriteXForm);
                        SpriteXForm subf = ((SpriteXForm)sf);
                        subf.ChangeSuper(getImagesFormByName(subf.superName));
                    }
                }
            }


            foreach (TreeNode node in nodeLevels.Nodes)
            {
                WorldForm wf = (WorldForm)getForm(node);
                wf.ChangeAllUnits(form_maps, form_sprites, form_spritexs, form_images);
            }

            getEventTemplateForm();

            foreach (TreeNode node in nodeLevels.Nodes)
            {
                WorldForm wf = (WorldForm)getForm(node);
                wf.loadOver();
            }
        }


        private void ProjectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Form f in Application.OpenForms)
            {
                f.TopMost = false;
            }

            if (!is_console)
            {
                if (MessageBox.Show(
                    "是否要关闭工程？",
                    "警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2
                    ) != DialogResult.OK)
                {
                    e.Cancel = true;
                }
            }

        }

        private void ProjectForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (TreeNode key in formTable.Keys)
            {
                Form form = getForm(key) as Form;
                if (form != null)
                {
                    form.Close();
                    form.Dispose();
                }
            }

        }
        private void ProjectForm_Shown(object sender, EventArgs e)
        {
            sortTreeView();
            loadOver();
        }

        public void BeginOutputDirect()
        {
            this.sortTreeView();
            this.loadOver();
        }

        public void OutputCustom(String fileName)
        {
            lock (this)
            {
                new OutputTask(this).OutputCustom(fileName);
            }
        }


        class OutputTask
        {
            readonly ProjectForm project;
            readonly TreeView tree;

            string OutputName;
            string OutputDir;
            string OutputDirImage;

            string ImageType;			/* 输出图片格式 默认(*.png) */

            Boolean ImageTile;
            Boolean ImageTileData;

            Boolean ImageGroup;
            Boolean ImageGroupData;

            public List<ImagesForm> FormsImages = new List<ImagesForm>();
            public List<MapForm> FormsMap = new List<MapForm>();
            public List<SpriteForm> FormsSprite = new List<SpriteForm>();
            public List<SpriteXForm> FormsSpriteEX = new List<SpriteXForm>();
            public List<WorldForm> FormsWorld = new List<WorldForm>();
            public List<ObjectForm> FormsObjects = new List<ObjectForm>();
            public List<CommandForm> FormsCommands = new List<CommandForm>();

            public OutputTask(ProjectForm pf)
            {
                this.project = pf;
                this.tree = pf.treeView1;
            }

            public void CleanOutput(string OutputDir)
            {
                foreach (string file in Directory.GetFiles(OutputDir))
                {
                    File.Delete(file);
                }
            }

            public void OutputCustom(String fileName)
            {
                {
                    try
                    {

                        initOutputForms();


                        if (System.IO.File.Exists(fileName))
                        {
                            Encoding encoding = Util.GetEncoding(fileName);

                            Util.CurEncoding = encoding;

                            Console.WriteLine("Encoding : " + encoding.EncodingName);
                            string script = File.ReadAllText(fileName, encoding);
                           //                             StreamReader sr = new StreamReader(fileName, encoding);
                           //                             string script = sr.ReadToEnd();
                           //                             sr.Close();


                           string ret = new string(new char[] { '\r', '\n' });

                            // var
                            script = Util.fillVar(script,
                                new String[] {
                                SC.VAR_FILE_NAME,
                                SC.VAR_PATH_NAME,
                            },
                                new String[] {
                                Path.GetFileNameWithoutExtension(workName),
                                Path.GetDirectoryName(workName),
                            }
                                );




                            // build command
                            OutputName = Util.getCommandScript(script, SC.OUTPUT);
                            //OutputName = Path.GetFullPath(OutputName);
                            try
                            {
                                if (System.IO.Path.IsPathRooted(OutputName))
                                {
                                    OutputDir = System.IO.Path.GetDirectoryName(OutputName);
                                }
                                else
                                {
                                    OutputDir = workSpace + "\\" + System.IO.Path.GetDirectoryName(OutputName);
                                }
                            }
                            catch (Exception err)
                            {
                                OutputDir = System.IO.Path.GetDirectoryName(workSpace);
                            }
                            if (!System.IO.Directory.Exists(OutputDir))
                            {
                                System.IO.Directory.CreateDirectory(OutputDir);
                            }
                            else { CleanOutput(OutputDir); }
                            OutputName = OutputDir + "\\" + System.IO.Path.GetFileName(OutputName);
                            OutputName = Path.GetFullPath(OutputName);

                            Console.WriteLine("OutputName : " + OutputName);

                            // out image
                            OutputDirImage = Util.getCommandScript(script, SC.IMAGE_OUTPUT);
                            if (OutputDirImage == "") OutputDirImage = null;
                            try
                            {
                                if (System.IO.Path.IsPathRooted(OutputDirImage))
                                {
                                    OutputDirImage = System.IO.Path.GetDirectoryName(OutputDirImage);
                                }
                                else
                                {
                                    OutputDirImage = workSpace + "\\" + System.IO.Path.GetDirectoryName(OutputDirImage);
                                }
                                if (!System.IO.Directory.Exists(OutputDirImage))
                                {
                                    System.IO.Directory.CreateDirectory(OutputDirImage);
                                }
                                ImageType = Util.getCommandScript(script, SC.IMAGE_TYPE);
                                ImageTile = Util.getCommandScript(script, SC.IMAGE_TILE).Equals("true", StringComparison.CurrentCultureIgnoreCase);
                                ImageTileData = Util.getCommandScript(script, SC.IMAGE_TILE_DATA).Equals("true", StringComparison.CurrentCultureIgnoreCase);
                                ImageGroup = Util.getCommandScript(script, SC.IMAGE_GROUP).Equals("true", StringComparison.CurrentCultureIgnoreCase);
                                ImageGroupData = Util.getCommandScript(script, SC.IMAGE_GROUP_DATA).Equals("true", StringComparison.CurrentCultureIgnoreCase);
                            }
                            catch (Exception err)
                            {
                                OutputDirImage = System.IO.Path.GetDirectoryName(workSpace);
                                ImageType = "";
                                ImageTile = false;
                                ImageTileData = false;
                                ImageGroup = false;
                                ImageGroupData = false;
                            }

                            // build format
                            Util.setFormatNumberArray1D(Util.getCommandScript(script, SC.FORMAT_NUMBER_ARRAY_1D), "<>");
                            Util.setFormatStringArray1D(Util.getCommandScript(script, SC.FORMAT_STRING_ARRAY_1D), "<>");

                            Util.setFormatArray1D(Util.getCommandScript(script, SC.FORMAT_ARRAY_1D), "<>");
                            Util.setFormatArray2D(Util.getCommandScript(script, SC.FORMAT_ARRAY_2D), "<>");

                            Util.setFixedStringArray(Util.getCommandScript(script, SC.FIXED_STRING_ARRAY));

                            ///////////////////////////////////////////////////////////////////
                            script = fillScriptNode(script);
                            ///////////////////////////////////////////////////////////////////

                            script = Util.replaceFuncScript(script);

                            // complete
                            if (CellGameEdit.Config.Default.IsOutEncodingInfo)
                            {
                                script = script.Insert(0, "/* Email : wazazhang@gmail.com */" + ret);
                                script = script.Insert(0, "/* Cell Game Editor by WAZA Zhang */" + ret);
                                script = script.Insert(0, "/* Encoding : " + encoding.EncodingName + " */" + ret);
                            }


                            Console.WriteLine("");
                            Console.WriteLine(script);

                            System.IO.File.WriteAllText(
                                OutputName,
                                script,
                                encoding
                                );

                            Console.WriteLine(ret + "Output --> " + OutputName + " --> " + script.Length + "(Chars)");
                            Console.WriteLine("");
                        }
                        else
                        {
                            Console.WriteLine("Error : " + fileName + " : 不存在!");
                        }
                    }
                    catch (Exception err) { MessageBox.Show(err.StackTrace + "  at  " + err.Message); }
                }
            }

            public string fillScriptSub<T>(string src, string start, string end, List<T> forms) where T : class, IEditForm
            {
                string script = src.Substring(0, src.Length);
                string sub = Util.getTopTrunk(script, start, end);
                if (sub == null) return null;

                ArrayList scripts = new ArrayList();
                try
                {
                    for (int i = 0; i < forms.Count; i++)
                    {

                        IEditForm form = ((IEditForm)forms[i]);

                        String ignoreKey = null;
                        if (Util.testIgnore("<IGNORE>", sub, form.getID(), ref ignoreKey) == true)
                        {
                            continue;
                        }

                        String keepKey = null;
                        if (Util.testKeep("<KEEP>", sub, form.getID(), ref keepKey) == false)
                        {
                            continue;
                        }



                        StringWriter output = new StringWriter();
                        //
                        if (forms[i].GetType().Equals(typeof(ImagesForm)))
                        {
                            ((ImagesForm)form).OutputCustom(i, sub, output, OutputDirImage, ImageType, ImageTile, ImageTileData, ImageGroup, ImageGroupData);
                            //Console.WriteLine("Output Images : " + ((ImagesForm)forms[i]).id + " -> " + output.ToString().Length + "(Chars)");
                        }
                        else if (forms[i].GetType().Equals(typeof(MapForm)))
                        {
                            ((MapForm)form).OutputCustom(i, sub, output);
                            //Console.WriteLine("Output Map : " + ((MapForm)forms[i]).id + " -> " + output.ToString().Length + "(Chars)");
                        }
                        else if (forms[i].GetType().Equals(typeof(SpriteForm)))
                        {
                            ((SpriteForm)form).OutputCustom(i, sub, output);
                            //Console.WriteLine("Output Sprite : " + ((SpriteForm)forms[i]).id + " -> " + output.ToString().Length + "(Chars)");
                        }
                        else if (forms[i].GetType().Equals(typeof(SpriteXForm)))
                        {
                            ((SpriteXForm)form).OutputCustom(i, sub, output);
                            //Console.WriteLine("Output Sprite : " + ((SpriteForm)forms[i]).id + " -> " + output.ToString().Length + "(Chars)");
                        }
                        else if (forms[i].GetType().Equals(typeof(WorldForm)))
                        {
                            ((WorldForm)form).OutputCustom(i, sub, output);
                            //Console.WriteLine("Output World : " + ((WorldForm)forms[i]).id + " -> " + output.ToString().Length + "(Chars)");
                        }
                        //
                        else if (forms[i].GetType().Equals(typeof(CommandForm)))
                        {
                            ((CommandForm)form).OutputCustom(i, sub, output);
                            //Console.WriteLine("Output Command : " + ((CommandForm)forms[i]).id + " -> " + output.ToString().Length + "(Chars)");
                        }

                        scripts.Add(output.ToString());

                        Console.WriteLine("Output : " + form.GetType().ToString() + " : " + form.getID() + " -> " + output.ToString().Length + "(Chars)");
                    }
                }
                catch (Exception err) { MessageBox.Show(err.StackTrace + "  at  " + err.Message); }

                script = Util.replaceSubTrunksScript(script, start, end, (string[])scripts.ToArray(typeof(string)));

                return script;
            }

            public string fillScriptNode(string src)
            {
                string script = src.Substring(0, src.Length);

                #region build resource trunk
                Console.WriteLine("build resource trunk");
                try
                {
                    string resource = Util.getTopTrunk(script, SC._RESOURCE, SC._END_RESOURCE);
                    if (resource != null)
                    {
                        bool fix = false;
                        do
                        {
                            fix = false;
                            string images = fillScriptSub(resource, SC._IMAGES, SC._END_IMAGES, FormsImages);
                            if (images != null) { resource = images; fix = true; }

                            string map = fillScriptSub(resource, SC._MAP, SC._END_MAP, FormsMap);
                            if (map != null) { resource = map; fix = true; }

                            string sprite = fillScriptSub(resource, SC._SPRITE, SC._END_SPRITE, FormsSprite);
                            if (sprite != null) { resource = sprite; fix = true; }

                            string spritex = fillScriptSub(resource, SC._SPRITE_EX, SC._END_SPRITE_EX, FormsSpriteEX);
                            if (spritex != null) { resource = spritex; fix = true; }

                        } while (fix);

                        resource = Util.replaceKeywordsScript(resource,
                            SC._RESOURCE,
                            SC._END_RESOURCE,
                         new string[] {
                         SC.RES_IMAGES_COUNT,
                         SC.RES_MAP_COUNT,
                         SC.RES_SPRITE_COUNT,
                         SC.RES_SPRITE_EX_COUNT },
                         new string[] {
                         FormsImages.Count.ToString(),
                         FormsMap.Count.ToString(),
                         FormsSprite.Count.ToString(),
                         FormsSpriteEX.Count.ToString()
                     });
                        script = Util.replaceSubTrunksScript(script,
                            SC._RESOURCE,
                            SC._END_RESOURCE,
                            new string[] { resource });
                    }
                }
                catch (Exception err) { MessageBox.Show(err.StackTrace + "  at  " + err.Message); }
                #endregion

                #region build world trunk
                Console.WriteLine("build world trunk");
                try
                {
                    string level = Util.getTopTrunk(script, SC._LEVEL, SC._END_LEVEL);
                    if (level != null)
                    {
                        bool fix = false;
                        do
                        {
                            fix = false;
                            string world = fillScriptSub(level, SC._WORLD, SC._END_WORLD, FormsWorld);
                            if (world != null) { level = world; fix = true; }

                        } while (fix);

                        level = Util.replaceKeywordsScript(level, SC._LEVEL, SC._END_LEVEL,
                        new string[] { SC.LEVEL_WORLD_COUNT },
                        new string[] { FormsWorld.Count.ToString() });
                        script = Util.replaceSubTrunksScript(script, SC._LEVEL, SC._END_LEVEL, new string[] { level });
                    }
                }
                catch (Exception err) { MessageBox.Show(err.StackTrace + "  at  " + err.Message); }
                #endregion

                #region build command trunk
                Console.WriteLine("build command trunk");
                try
                {
                    string command = Util.getTopTrunk(script, SC._COMMAND, SC._END_COMMAND);
                    if (command != null)
                    {
                        bool fix = false;
                        do
                        {
                            fix = false;
                            string table = fillScriptSub(command, SC._TABLE_GROUP, SC._END_TABLE_GROUP, FormsCommands);
                            if (table != null) { command = table; fix = true; }

                        } while (fix);

                        command = Util.replaceKeywordsScript(command, SC._COMMAND, SC._END_COMMAND,
                          new string[] { SC.COMMAND_TABLE_GROUP_COUNT },
                          new string[] { FormsCommands.Count.ToString() });
                        script = Util.replaceSubTrunksScript(script, SC._COMMAND, SC._END_COMMAND, new string[] { command });
                    }
                }
                catch (Exception err) { MessageBox.Show(err.StackTrace + "  at  " + err.Message); }
                #endregion

                return script;
            }



            public void initOutputForms()
            {
                FormsImages.Clear();
                FormsMap.Clear();
                FormsSprite.Clear();
                FormsSpriteEX.Clear();
                FormsWorld.Clear();
                FormsObjects.Clear();
                FormsCommands.Clear();

                foreach (TreeNode tn in tree.Nodes)
                {
                    initOutputForms(tn);
                }
            }

            private void initOutputForms(TreeNode node)
            {
                IEditForm form = project.getForm(node);
                if (form is IEditForm)
                {
                    //
                    if (form is ImagesForm)
                    {
                        FormsImages.Add(((ImagesForm)form));
                    }
                    if (form is MapForm)
                    {
                        FormsMap.Add(((MapForm)form));
                    }
                    if (form is SpriteForm)
                    {
                        FormsSprite.Add(((SpriteForm)form));
                    }
                    if (form is SpriteXForm)
                    {
                        FormsSpriteEX.Add(((SpriteXForm)form));
                    }
                    //
                    if (form is WorldForm)
                    {
                        FormsWorld.Add(((WorldForm)form));
                    }
                    //
                    if (form is ObjectForm)
                    {
                        FormsObjects.Add(((ObjectForm)form));
                    }
                    //
                    if (form is CommandForm)
                    {
                        FormsCommands.Add(((CommandForm)form));
                    }
                }

                if (node.Nodes.Count >= 0)
                {
                    foreach (TreeNode sub in node.Nodes)
                    {
                        initOutputForms(sub);
                    }
                }

            }

        }

        public void lockForms()
        {

            lockForms(nodeReses);
            lockForms(nodeLevels);
            lockForms(nodeObjects);
            lockForms(nodeCommands);

            //this.Hide();
            this.Enabled = false;

        }
        private void lockForms(TreeNode node)
        {
            IEditForm form = getForm(node);
            if (form != null)
            {
                form.getForm().Hide();
                form.getForm().Enabled = false;
            }

            if (node.Nodes.Count >= 0)
            {
                foreach (TreeNode sub in node.Nodes)
                {
                    lockForms(sub);
                }
            }
        }
        public void unlockForms()
        {
            unlockForms(nodeReses);
            unlockForms(nodeLevels);
            unlockForms(nodeObjects);
            unlockForms(nodeCommands);
            //this.Show();
            this.Enabled = true;

        }
        private void unlockForms(TreeNode node)
        {
            IEditForm form = getForm(node);
            if (form != null)
            {
                form.getForm().Enabled = true;
            }

            if (node.Nodes.Count >= 0)
            {
                foreach (TreeNode sub in node.Nodes)
                {
                    unlockForms(sub);
                }
            }
        }

        private void setTreeNodeImage(TreeNode node)
        {
            if (node == nodeReses || node == nodeObjects || node == nodeLevels || node == nodeCommands)
            {
                node.ImageKey = node.SelectedImageKey = "icons_tool_bar2.png";
            }
            else
            {
                IEditForm form = getForm(node);

                if (form is ImagesForm)
                {
                    node.ImageKey = node.SelectedImageKey = "icon_res.png";
                }
                else if (form is MapForm)
                {
                    node.ImageKey = node.SelectedImageKey = "icon_scene.png";
                }
                else if (form is SpriteForm)
                {
                    node.ImageKey = node.SelectedImageKey = "icon_res_2.png";
                }
                else if (form is SpriteXForm)
                {
                    node.ImageKey = node.SelectedImageKey = "icon_hd.png";
                }
                else if (form is WorldForm)
                {
                    node.ImageKey = node.SelectedImageKey = "icon_edit.png";
                }
                else
                {
                    node.ImageKey = node.SelectedImageKey = "icons_tool_bar1.png";
                }
            }
        }


        public void RefreshNodeName()
        {
            RefreshNodeName(nodeReses);
            RefreshNodeName(nodeLevels);
            RefreshNodeName(nodeObjects);
            RefreshNodeName(nodeCommands);
        }

        public void RefreshNodeName(TreeNode node)
        {
            setTreeNodeImage(node);
            if (formTable.ContainsKey(node))
            {
                node.Name = node.Text;

                IEditForm editform = formTable[node];
                editform.getForm().Text = node.Text;
                editform.setID(node.Text, this);
            }

            if (node.Nodes.Count >= 0)
            {
                foreach (TreeNode sub in node.Nodes)
                {
                    RefreshNodeName(sub);
                }
            }

        }

        private IEditForm getForm(TreeNode key)
        {
            IEditForm ret = null;
            try
            {
                if (formTable.TryGetValue(key, out ret))
                {
                    return ret;
                }
            }
            catch (Exception err)
            {
            }
            return ret;
        }


        public List<MapForm> getMaps(ImagesForm images)
        {
            List<MapForm> list = new List<MapForm>();
            foreach (TreeNode node in nodeReses.Nodes)
            {
                foreach (TreeNode subnode in node.Nodes)
                {
                    IEditForm frm = getForm(subnode);
                    if (frm.GetType().Equals(typeof(MapForm)))
                    {
                        MapForm map = (MapForm)frm;

                        if (map.super == images)
                        {
                            list.Add(map);
                        }
                    }
                }
            }
            return list;
        }

        public List<SpriteForm> getSpritess(ImagesForm images)
        {
            List<SpriteForm> list = new List<SpriteForm>();

            foreach (TreeNode node in nodeReses.Nodes)
            {
                foreach (TreeNode subnode in node.Nodes)
                {
                    IEditForm frm = getForm(subnode);

                    if (frm.GetType().Equals(typeof(SpriteForm)))
                    {
                        SpriteForm spr = (SpriteForm)frm;

                        if (spr.super == images)
                        {
                            list.Add(spr);
                        }
                    }
                }
            }

            return list;
        }


        public List<SpriteXForm> getSpriteEXs(ImagesForm images)
        {
            List<SpriteXForm> list = new List<SpriteXForm>();

            foreach (TreeNode node in nodeReses.Nodes)
            {
                foreach (TreeNode subnode in node.Nodes)
                {
                    IEditForm frm = getForm(subnode);

                    if (frm.GetType().Equals(typeof(SpriteXForm)))
                    {
                        SpriteXForm spr = (SpriteXForm)frm;

                        if (spr.super == images)
                        {
                            list.Add(spr);
                        }
                    }
                }
            }

            return list;
        }

        public List<ImagesForm> getAllImages()
        {
            List<ImagesForm> list = new List<ImagesForm>();

            foreach (Object frm in formTable.Values)
            {
                if (frm.GetType() == typeof(ImagesForm))
                {
                    list.Add(frm as ImagesForm);
                }
            }

            return list;
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.treeView1.SelectedNode = e.Node;
        }


        private void treeView1_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node.Parent == null)
            {
                e.CancelEdit = true;
                nodeReses.EndEdit(true);
            }
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {

            if (getForm(e.Node) != null && e.Label != null && e.Label.Length > 0)
            {
                e.CancelEdit = true;

                String name = e.Label;


                while (true)
                {
                    int index = e.Node.Parent.Nodes.IndexOfKey(name);
                    if (index >= 0 && index != e.Node.Parent.Nodes.IndexOf(e.Node))
                    {
                        TextDialog nameDialog = new TextDialog(name);
                        nameDialog.Text = "已经有" + name + "这个名字了";
                        if (nameDialog.showDialog() == DialogResult.OK)
                        {
                            name = nameDialog.getText();
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        IEditForm editform = getForm(e.Node);
                        if (editform is ImagesForm)
                        {
                            ((ImagesForm)editform).setID(name, this);
                            ((ImagesForm)editform).changeImage();
                            Console.WriteLine("change image : " + editform.id);
                        }
                        else if (editform != null)
                        {
                            editform.setID(name, this);
                        }

                        e.Node.Name = name;
                        e.Node.Text = name;

                        RefreshNodeName();
                        sortTreeView();

                        Console.WriteLine("New Name = " + e.Node.Name);
                        break;
                    }
                }
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            IEditForm editform = getForm(treeView1.SelectedNode);
            if (editform != null)
            {
                editform.getForm().MdiParent = this.MdiParent;
                editform.getForm().Show();
                editform.getForm().Select();
            }


        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            IEditForm dform = getForm((TreeNode)e.Item);
            if (dform != null)
            {
                if (dform.GetType().Equals(typeof(SpriteForm)))
                {
                    DoDragDrop((SpriteForm)dform, DragDropEffects.Link);
                    //Console.WriteLine("Spr drag");
                }
                if (dform.GetType().Equals(typeof(SpriteXForm)))
                {
                    DoDragDrop((SpriteXForm)dform, DragDropEffects.Link);
                    //Console.WriteLine("Spr drag");
                }
                if (dform.GetType().Equals(typeof(MapForm)))
                {
                    DoDragDrop((MapForm)dform, DragDropEffects.Link);
                    //Console.WriteLine("map drag");
                }
                if (dform.GetType().Equals(typeof(ImagesForm)))
                {
                    DoDragDrop((ImagesForm)dform, DragDropEffects.Link);
                    //Console.WriteLine("map drag");
                }
            }

        }


        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshNodeName();
            sortTreeView();
        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshNodeName();
            sortTreeView();
        }

        private void treeView1_AfterCollapse(object sender, TreeViewEventArgs e)
        {
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
        }

        //------------------------------------------------------------------------------------------------------------------------------------

        class NodesSorter : IComparer
        {
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            int IComparer.Compare(Object x, Object y)
            {
                return comparer.Compare(((TreeNode)x).Name, ((TreeNode)y).Name);
            }

        }

        public void sortTreeView()
        {
            treeView1.SelectedNode = null;
            treeView1.TreeViewNodeSorter = new NodesSorter();
            treeView1.Sort();
            treeView1.SelectedNode = null;
        }

        public EventTemplatePlugin getEventTemplateForm()
        {
            return current_event_plugin;
        }

        public EventTemplatePlugin getEventTemplateForm(string clsname)
        {
            if (clsname == current_event_plugin.getClassName())
            {
                return current_event_plugin;
            }
            return null;
        }

        public WorldAddUnitForm getWorldAddUnitForm()
        {
            if (form_world_add_unit == null)
            {
                form_world_add_unit = new WorldAddUnitForm();
            }
            return form_world_add_unit;
        }

        //------------------------------------------------------------------------------------------------------------------------------------

        #region resMenu
        private void 添加图片组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshNodeName();
            String name = "unamed_Tile";
            TextDialog nameDialog = new TextDialog(name);
            while (nameDialog.ShowDialog() == DialogResult.OK)
            {
                name = nameDialog.getText();
                if (treeView1.SelectedNode.Nodes.ContainsKey(name))
                {
                    MessageBox.Show("已经有　" + name + " 这个名字了");
                    continue;
                }

                ImagesForm form = new ImagesForm(name);
                TreeNode node = new TreeNode(name);
                node.Name = name;
                node.Text = name;
                node.Tag = form;
                formTable.Add(node, form);

                node.ContextMenuStrip = this.tileMenu;
                this.treeView1.SelectedNode.Nodes.Add(node);
                this.treeView1.SelectedNode.ExpandAll();
                form.MdiParent = this.MdiParent;
                form.Show();

                try
                {
                    String dir = workSpace + "\\" + form.id;
                    if (System.IO.Directory.Exists(dir))
                    {
                        System.IO.Directory.Delete(dir, true);
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.TargetSite + ":" + err.StackTrace + "  at  " + err.Message);
                }
                sortTreeView();
                RefreshNodeName();
                break;
            }

        }


        #endregion

        #region levelMenu

        private void 添加场景ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshNodeName();
            String name = "unamed_Level";
            TextDialog nameDialog = new TextDialog(name);
            while (nameDialog.ShowDialog() == DialogResult.OK)
            {
                name = nameDialog.getText();
                if (treeView1.SelectedNode.Nodes.ContainsKey(name))
                {
                    MessageBox.Show("已经有　" + name + " 这个名字了");
                    continue;
                }

                WorldForm form = new WorldForm(name);
                TreeNode node = new TreeNode(name);
                node.Name = name;
                node.Text = name;
                node.Tag = form;
                formTable.Add(node, form);

                node.ContextMenuStrip = this.subMenu;
                this.treeView1.SelectedNode.Nodes.Add(node);
                this.treeView1.SelectedNode.ExpandAll();
                form.MdiParent = this.MdiParent;
                form.Show();
                sortTreeView();
                RefreshNodeName();
                break;
            }

        }
        #endregion

        #region objMenu

        private void 添加对象ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshNodeName();
            String name = "unamed_Object";
            TextDialog nameDialog = new TextDialog(name);
            while (nameDialog.ShowDialog() == DialogResult.OK)
            {
                name = nameDialog.getText();
                if (treeView1.SelectedNode.Nodes.ContainsKey(name))
                {
                    MessageBox.Show("已经有　" + name + " 这个名字了");
                    continue;
                }

                ObjectForm form = new ObjectForm(name);
                TreeNode node = new TreeNode(name);
                node.Name = name;
                node.Text = name;
                node.Tag = form;
                formTable.Add(node, form);

                node.ContextMenuStrip = this.subMenu;
                this.treeView1.SelectedNode.Nodes.Add(node);
                this.treeView1.SelectedNode.ExpandAll();
                form.MdiParent = this.MdiParent;
                form.Show();
                sortTreeView();
                RefreshNodeName();
                break;
            }
        }

        #endregion

        //------------------------------------------------------------------------------------------------------------------------------------

        #region commandMenu
        private void 添加属性列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshNodeName();
            String name = "unamed_Command";
            TextDialog nameDialog = new TextDialog(name);
            while (nameDialog.ShowDialog() == DialogResult.OK)
            {
                name = nameDialog.getText();
                if (treeView1.SelectedNode.Nodes.ContainsKey(name))
                {
                    MessageBox.Show("已经有　" + name + " 这个名字了");
                    continue;
                }

                CommandForm form = new CommandForm(name);
                TreeNode node = new TreeNode(name);
                node.Name = name;
                node.Text = name;
                node.Tag = form;
                formTable.Add(node, form);

                node.ContextMenuStrip = this.subMenu;
                this.treeView1.SelectedNode.Nodes.Add(node);
                this.treeView1.SelectedNode.ExpandAll();
                form.MdiParent = this.MdiParent;
                form.Show();
                sortTreeView();
                RefreshNodeName();
                break;
            }
        }
        #endregion

        //------------------------------------------------------------------------------------------------------------------------------------

        #region tileMenu

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEditForm form = getForm(treeView1.SelectedNode);
            if (form != null)
            {
                form.getForm().MdiParent = this.MdiParent;
                form.getForm().Show();
                form.getForm().Select();
            }

        }
        private void 复制ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            IsCopy = true;
            try
            {
                copySub(treeView1.SelectedNode, null);
                sortTreeView();
                RefreshNodeName();
            }
            finally
            {
                IsCopy = false;
            }
        }
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = getForm(treeView1.SelectedNode) as Form;
            if (form != null)
            {
                while (treeView1.SelectedNode.Nodes.Count > 0)
                {
                    Form sform = getForm(treeView1.SelectedNode.Nodes[0]) as Form;
                    sform.Enabled = false;
                    sform.Dispose();
                    //getForm(treeView1.SelectedNode.Nodes[0]).Hide();
                    formTable.Remove(treeView1.SelectedNode.Nodes[0]);
                    treeView1.SelectedNode.Nodes.RemoveAt(0);
                }

                form.Enabled = false;
                form.Dispose();
                formTable.Remove(treeView1.SelectedNode);
                treeView1.SelectedNode.Parent.Nodes.Remove(treeView1.SelectedNode);

                sortTreeView();
                RefreshNodeName();
            }

        }

        private void 精灵ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshNodeName();
            String name = "unamed_Sprite";
            TextDialog nameDialog = new TextDialog(name);
            while (nameDialog.ShowDialog() == DialogResult.OK)
            {
                name = nameDialog.getText();
                if (treeView1.SelectedNode.Nodes.ContainsKey(name))
                {
                    MessageBox.Show("已经有　" + name + " 这个名字了");
                    continue;
                }
                SpriteForm form = ((ImagesForm)getForm(treeView1.SelectedNode)).createSpriteForm(name);
                if (form != null)
                {
                    TreeNode node = new TreeNode(name);
                    node.Name = name;
                    formTable.Add(node, form);
                    node.ContextMenuStrip = this.subMenu;
                    node.Tag = form;
                    this.treeView1.SelectedNode.Nodes.Add(node);
                    this.treeView1.SelectedNode.ExpandAll();
                    form.MdiParent = this.MdiParent;
                    form.Show();
                    sortTreeView();
                    RefreshNodeName();
                }
                break;
            }
        }

        private void 精灵XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshNodeName();
            String name = "unamed_SpriteEX";
            TextDialog nameDialog = new TextDialog(name);
            while (nameDialog.ShowDialog() == DialogResult.OK)
            {
                name = nameDialog.getText();
                if (treeView1.SelectedNode.Nodes.ContainsKey(name))
                {
                    MessageBox.Show("已经有　" + name + " 这个名字了");
                    continue;
                }
                SpriteXForm form = ((ImagesForm)getForm(treeView1.SelectedNode)).createSpriteExForm(name);
                if (form != null)
                {
                    TreeNode node = new TreeNode(name);
                    node.Name = name;
                    formTable.Add(node, form);
                    node.ContextMenuStrip = this.subMenu;
                    node.Tag = form;
                    this.treeView1.SelectedNode.Nodes.Add(node);
                    this.treeView1.SelectedNode.ExpandAll();
                    form.MdiParent = this.MdiParent;
                    form.Show();
                    sortTreeView();
                    RefreshNodeName();
                }
                break;
            }
        }

        private void 地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshNodeName();
            String name = "unamed_Map";
            TextDialog nameDialog = new TextDialog(name);
            while (nameDialog.ShowDialog() == DialogResult.OK)
            {
                name = nameDialog.getText();
                if (treeView1.SelectedNode.Nodes.ContainsKey(name))
                {
                    MessageBox.Show("已经有　" + name + " 这个名字了");
                    continue;
                }
                MapForm form = ((ImagesForm)getForm(treeView1.SelectedNode)).createMapForm(name);
                if (form != null)
                {
                    TreeNode node = new TreeNode(name);
                    node.Name = name;
                    node.Tag = form;
                    formTable.Add(node, form);
                    node.ContextMenuStrip = this.subMenu;
                    this.treeView1.SelectedNode.Nodes.Add(node);
                    this.treeView1.SelectedNode.ExpandAll();
                    form.MdiParent = this.MdiParent;
                    form.Show();
                    sortTreeView();
                    RefreshNodeName();
                }
                break;
            }

        }

        private void toolStripMenuItem_COPY_TILES_Click(object sender, EventArgs e)
        {
            copyToClipBoard(treeView1.SelectedNode);
        }

        private void toolStripMenuItem_PASTE_TILES_Click(object sender, EventArgs e)
        {
            pasteFromClipBoard(nodeReses);
        }




        #endregion

        //------------------------------------------------------------------------------------------------------------------------------------

        #region subMenu

        private void 打开ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            IEditForm form = getForm(treeView1.SelectedNode);
            if (form != null)
            {
                form.getForm().MdiParent = this.MdiParent;
                form.getForm().Show();
                form.getForm().Select();
            }

        }
        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsCopy = true;
            try
            {
                copySub(treeView1.SelectedNode, null);
                sortTreeView();
            }
            finally
            {
                IsCopy = false;
            }
        }
        private void 删除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form form = getForm(treeView1.SelectedNode) as Form;
            if (form != null)
            {
                form.Enabled = false;
                form.Dispose();

                formTable.Remove(treeView1.SelectedNode);
                treeView1.SelectedNode.Parent.Nodes.Remove(treeView1.SelectedNode);

                sortTreeView();
            }
        }

        #endregion

        //------------------------------------------------------------------------------------------------------------------------------------
        public ImagesForm getImagesFormByName(string name)
        {
            foreach (TreeNode node in formTable.Keys)
            {
                if (node.Text.Equals(name))
                {
                    if (formTable[node].GetType().Equals(typeof(ImagesForm)))
                    {
                        return (ImagesForm)formTable[node];
                    }
                }
            }
            return null;
        }

        private void copySub(TreeNode super, TreeNode superCopy)
        {
            IEditForm srcform;
            if (formTable.TryGetValue(super, out srcform))
            {
                RefreshNodeName();

                try
                {
                    SoapFormatter formatter = new SoapFormatter();

                    using (MemoryStream stream = new MemoryStream())
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        formatter.Serialize(stream, srcform);

                        stream.Seek(0, SeekOrigin.Begin);
                        try
                        {
                            Form form = (Form)formatter.Deserialize(stream);
                            pasteForm(super, form as IEditForm, superCopy);
                            if (form is WorldForm)
                            {
                                ((WorldForm)form).loadOver();
                            }
                        }
                        catch (Exception errr) { }
                    }


                }
                catch (Exception err)
                {
                    Console.WriteLine(err.HelpLink);
                    Console.WriteLine(err.Source);
                    Console.WriteLine(err.Message);
                    Console.WriteLine(err.StackTrace + "  at  " + err.Message);
                    MessageBox.Show(err.Message);
                }

                RefreshNodeName();
            }



        }

        private void pasteForm(TreeNode super, IEditForm new_form, TreeNode superCopy)
        {
            IEditForm superForm = getForm(superCopy);
            if (new_form is ImagesForm)
            {
                ((ImagesForm)new_form).changeImage();
                Console.WriteLine("change image : " + ((ImagesForm)new_form).id);
            }
            if (new_form is IEditFormVisible)
            {
                if (superCopy != null && formTable[superCopy] is ImagesForm)
                {
                    ((IEditFormVisible)new_form).ChangeSuper((ImagesForm)formTable[superCopy]);
                    Console.WriteLine("change super : " + ((ImagesForm)formTable[superCopy]).id);
                }
                else if (super.Parent != null && formTable[super.Parent] is ImagesForm)
                {
                    ((IEditFormVisible)new_form).ChangeSuper((ImagesForm)formTable[super.Parent]);
                    Console.WriteLine("change super : " + ((ImagesForm)formTable[super.Parent]).id);
                }
            }
            if (new_form is WorldForm)
            {
                OutputTask output = new OutputTask(this);
                output.initOutputForms();
                ((WorldForm)new_form).ChangeAllUnits(output.FormsMap, output.FormsSprite, output.FormsSpriteEX, output.FormsImages);
            }

            new_form.getForm().MdiParent = this.MdiParent;



            // clone node
            TreeNode copy = new TreeNode(super.Name + "_Copy");
            copy.Name = super.Name + "_Copy";
            copy.ContextMenuStrip = super.ContextMenuStrip;
            copy.Tag = new_form;

            // add 
            formTable.Add(copy, new_form);
            if (superCopy != null)
            {
                // adjust
                while (true)
                {
                    if (superCopy.Nodes.ContainsKey(copy.Name))
                    {
                        copy.Name = copy.Name + "_Copy";
                        copy.Text = copy.Name;
                        continue;
                    }
                    else
                    {
                        superCopy.Nodes.Add(copy);
                        break;
                    }
                }
            }
            else
            {
                // adjust
                while (true)
                {
                    if (super.Parent.Nodes.ContainsKey(copy.Name))
                    {
                        copy.Name = copy.Name + "_Copy";
                        copy.Text = copy.Name;
                        continue;
                    }
                    else
                    {
                        super.Parent.Nodes.Add(copy);
                        break;
                    }
                }
            }



            // sub nodes
            if (super.Nodes.Count >= 0)
            {
                foreach (TreeNode sub in super.Nodes)
                {
                    if (new_form.GetType().Equals(typeof(ImagesForm)))
                    {
                        copySub(sub, copy);
                    }
                    else
                    {
                        copySub(sub, null);
                    }

                }
            }
        }

        private void copyToClipBoard(TreeNode super)
        {
            IEditForm form = getForm(super);
            if (form != null)
            {
                RefreshNodeName();
                try
                {
                    Clipboard.SetData(DataFormats.Serializable, formTable[super]);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }

            }
        }

        public void pasteFromClipBoard(TreeNode parent)
        {
            try
            {
                IEditForm iform = (IEditForm)Clipboard.GetData(DataFormats.Serializable);



                RefreshNodeName();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

        }


        public List<IEditFormVisible> getImangesFormChilds(ImagesForm src)
        {
            var ret = new List<IEditFormVisible>();
            foreach (TreeNode node in formTable.Keys)
            {
                if (node.Text.Equals(src.getID()))
                {
                    if (formTable[node].GetType().Equals(typeof(ImagesForm)))
                    {
                        foreach (TreeNode subNode in node.Nodes)
                        {
                            ret.Add(formTable[subNode] as IEditFormVisible);
                        }
                    }
                }
            }
            return ret;
        }







    }
}