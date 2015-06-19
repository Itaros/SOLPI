using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bootloader.Bootloader
{
    public sealed class AppDomainLoader
    {

        public static readonly string SOLPath = @"D:\SteamLibrary\SteamApps\common\Signs of Life\";
        public static readonly string SOLExe = @"Signs Of Life.exe";

        internal static readonly string[] AdditionalAssemblies = new string[]{
            "DeminaRuntime.dll",
            "LibNoise.dll",
            "Lidgren.Network.dll",
            "log4net.dll",
            "ProjectMercury.dll",
            "Steamworks.NET.dll",
            "System.Data.SQLite.dll",
            "System.Data.SQLite.Linq.dll"
        };

        public Assembly SOLAssembly { get; private set; }

        public AppDomainLoader()
        {

        }

        internal void LinkWithSOL()
        {
            var exe = "Signs Of Life - SOLPI.exe";
            Directory.SetCurrentDirectory(SOLPath);
            Console.WriteLine("Dir is set to: "+Directory.GetCurrentDirectory());

            var sol = Assembly.LoadFrom(SOLPath+exe);
            Console.WriteLine("SOL Injected: "+sol.CodeBase);
            SOLAssembly = sol;

            LoadAdditionalAssemblies();
        }

        /// <summary>
        /// Not actually needed, but it is better to have them preloaded for SOLPI. Plus this is good startup error control.
        /// </summary>
        private void LoadAdditionalAssemblies()
        {
            foreach (string ass in AdditionalAssemblies)
            {
                var path = SOLPath + ass;
                var a = Assembly.LoadFrom(path);
            }
        }

        internal void Boot()
        {
            try
            {
                var entry = SOLAssembly.EntryPoint;
                var initializer = SOLAssembly.CreateInstance(entry.Name);
                entry.Invoke(initializer, new object[] { new string[] { } });
            }
            catch (Exception e)
            {
                Console.WriteLine("Critical fault!");
                Console.WriteLine(e.ToString());
                throw;
            }
        }


 
    }
}
