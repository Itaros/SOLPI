using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPIBase
{
    public sealed class Workspace
    {

        public static readonly string FolderName = "Polymorph";
        public static readonly string InternalsFolder = FolderName+Path.DirectorySeparatorChar+"SOLPI";

        public static readonly string MDKFolder = InternalsFolder + Path.DirectorySeparatorChar + "MDK";

        private DirectoryInfo _generalDir;
        private DirectoryInfo _internalsDir;
        public bool IsReady { get; private set; }

        public Workspace()
        {

        }

        public void MakeReady()
        {
            _generalDir = Directory.CreateDirectory(FolderName);
            _internalsDir = Directory.CreateDirectory(InternalsFolder);
            Directory.CreateDirectory(MDKFolder);
            IsReady = true;
        }


        public void SaveAllLines(string[] strings, string filename)
        {
            File.WriteAllLines(InternalsFolder + Path.DirectorySeparatorChar + filename+".spl", strings);
        }

        public string[] ReadAllLines(string filename)
        {
            return File.ReadAllLines(InternalsFolder + Path.DirectorySeparatorChar + filename + ".spl");
        }

    }
}
