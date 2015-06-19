using Mono.Cecil;
using SOLPI.Instrumentations;
using SOLPIBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPI
{
    public sealed class Instrumentor
    {

        private string _path;

        public Workspace Workspace { get; private set; }

        public Instrumentor(string path, Workspace workspace)
        {
            _path = path;
            Workspace = workspace;

            _procList.Add(new RedefineGameVersionString());
            _procList.Add(new AccessTransformerNoTarget());
            _procList.Add(new RedefineGameLocalStorage());
            _procList.Add(new ExtractAllVanillaItems());
            _procList.Add(new ExtractAllVanillaProjects());
            _procList.Add(new GameLaunchHooker());
            _procList.Add(new GameOnResloadHooker());
            _procList.Add(new ItemHandlerGetNewItemOverride());
            _procList.Add(new StaticPrefabGetNewStaticPrefabByStaticPrefabTypeOverride());
            _procList.Add(new ContentHandlerSheetExtractor());
            _procList.Add(new MapGenerateHooker());
            _procList.Add(new ContentHandlerGetTextureHook());

        }

        private List<InstrumentationTaskBase> _procList = new List<InstrumentationTaskBase>();

        public void Process()
        {

            AssemblyDefinition assdef = AssemblyDefinition.ReadAssembly(_path);

            assdef.Name.Name += " - SOLPI";

            foreach (var handler in _procList)
            {
                handler.Process(this, assdef);
            }

            assdef.Write("Signs Of Life - SOLPI.exe");

        }

    }
}
