using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using CellGameEdit.PM;

namespace CellGameOutput
{
    class Output
    {
        string FileName;
        string[] Scripts;

        public Output(String file, String[] scripts)
        {
            FileName = Path.GetFullPath(file);
            Scripts = scripts;
        }

        public void run()
        {
            if (FileName != null && Scripts != null)
            {
                DirectOutput(FileName, Scripts);
            }
        }

        private void DirectOutput(string FileName, string[] Scripts)
        {
            try
            {
                if (FileName != null && Scripts != null)
                {
                    Console.WriteLine("Loading : " + FileName);

                    string name = System.IO.Path.GetFileName(FileName);
                    string dir = System.IO.Path.GetDirectoryName(FileName);

                    ProjectForm project = null;

                    ProjectForm.workSpace = dir;
                    ProjectForm.workName = FileName;
                    ProjectForm.is_console = true;
                    SoapFormatter formatter = new SoapFormatter();

                    byte[] data = File.ReadAllBytes(FileName);
                    if (data != null && data.Length != 0)
                    {
                        using (MemoryStream stream = new MemoryStream(data))
                        {
                            project = (ProjectForm)formatter.Deserialize(stream); 
                            project.LoadOver();
                        }
                        project.BeginOutputDirect();
                    }



                    if (project != null)
                    {
                        try
                        {
                            for (int i = 0; i < Scripts.Length; i++)
                            {
                                String script = Scripts[i];

                                if (!File.Exists(script))
                                {
                                    script = Application.StartupPath + @"\script\" + script;
                                }

                                Console.WriteLine("Output Script File : " + script);

                                try
                                {
                                    project.OutputCustom(script);
                                }
                                catch (Exception err) { Console.WriteLine(err.Message); }
                            }
                        }
                        finally
                        {
                            project.Close();
                            project.Dispose();
                        }

                    }

                }
            }
            finally
            {
                FileName = null;
                Scripts = null;
                Console.WriteLine("Complete !");
            }
             
        }
    }
}
