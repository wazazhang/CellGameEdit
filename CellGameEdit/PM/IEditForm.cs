using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace CellGameEdit.PM
{
    public interface IEditForm
    {
        Form getForm();

        String getID();

        void setID(string id, ProjectForm proj);

        string id { get;}

        void LoadOver(ProjectForm prj);
    }

    public interface IEditFormVisible
    {
        string superName { get; }
        ImagesForm super { get; }

        void ChangeSuper(List<ImagesForm> images);

        void ChangeSuper(ImagesForm super);

        bool CheckTileUsed(int tileID);

        void ChangeSuperImageSize(ImagesForm src, List<ImageChange> events);
    }
}
