using Bootloader.Bootloader;
using SOLPI;
using SOLPIBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootloader
{
    class Program
    {
        static void Main(string[] args)
        {

            
            //Instrumentor
            Workspace workspace = new Workspace();
            workspace.MakeReady();
            var instrumetor = new SOLPI.Instrumentor(AppDomainLoader.SOLPath+AppDomainLoader.SOLExe,workspace);
            instrumetor.Process();

            //Loader
            var loader = new AppDomainLoader();
            loader.LinkWithSOL();
            loader.Boot();

        }
    }
}
