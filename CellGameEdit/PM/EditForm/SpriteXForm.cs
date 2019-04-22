using DeepCore;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Windows.Forms;

namespace CellGameEdit.PM
{

    [Serializable]
    public partial class SpriteXForm : Form, ISerializable, IEditForm, IEditFormVisible
    {
        public string id { get; private set; }
        public String superName { get; private set; }
        public ImagesForm super { get; private set; }
        public void setID(string id, ProjectForm proj)
        {
            this.id = id;
        }
        public String getID()
        {
            return id;
        }
        public Form getForm()
        {
            return this;
        }

        public SpriteXForm(String name, ImagesForm images)
        {
            InitializeComponent();
            this.Text = id;
            this.id = name;
            this.super = images;
        }
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected SpriteXForm(SerializationInfo info, StreamingContext context)
        {
            InitializeComponent();
            try
            {
                this.id = (String)info.GetValue("id", typeof(String));
                this.Text = id;
                this.superName = (String)info.GetValue("SuperName", typeof(String));
            }
            catch (Exception) { }
        }
        public void LoadOver(ProjectForm prj)
        {
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            try
            {
                info.AddValue("id", id);
                info.AddValue("SuperName", super.id);

            }
            catch (Exception) { }
        }

        public void ChangeSuper(List<ImagesForm> images)
        {
            HashMap<string, ImagesForm> imagesHT = new HashMap<string, ImagesForm>();
            for (int i = 0; i < images.Count; i++)
            {
                imagesHT.Add((images[i]).id, images[i]);
            }
            if (imagesHT.ContainsKey(superName))
            {
                ChangeSuper(imagesHT[superName]);
                Console.WriteLine("SpriteX ChangeImages : " + superName);
            }
        }
        public void ChangeSuper(ImagesForm super)
        {
            this.super = super;
            
        }
        public void ChangeSuperImageSize(ImagesForm src, List<ImageChange> events)
        {

        }
        public bool CheckTileUsed(int tileID)
        {
            return false;
        }

        private void SpriteXForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }

        }


        public void OutputCustom(int index, string script, System.IO.StringWriter output)
        {

        }


        //--------------------------------------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------------------------------------

    }

}