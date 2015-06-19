using Microsoft.Xna.Framework.Content;
using SOLPIBase;
using SOLPolymorph.SignsOfLife.Polymorph.Replacements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPolymorph.SignsOfLife.Polymorph.Hooks
{
    public static class ContentHandlerHooks
    {

        public static void InitializeOnStart(ContentManager manager){
            Encapsulator = new ContentManagerEncapsulator(manager);
        }

        public static void InitializeOnEnd()
        {
            
        }

        private static String _nameGetTextureRoughPass;
        public static void GetTextureOnStart(string texname)
        {
            //Console.WriteLine("SoL Requests Texture Acquisition: "+texname);
            _nameGetTextureRoughPass = texname;
        }

        public static Microsoft.Xna.Framework.Graphics.Texture2D GetTextureOnEnd(Microsoft.Xna.Framework.Graphics.Texture2D texture)
        {
            texture.Name = _nameGetTextureRoughPass.Replace('/','_').Replace('\\','_');
            //Console.WriteLine("TEXTURE!");
            var path = Workspace.MDKFolder + Path.DirectorySeparatorChar + texture.Name + ".png";
            if (!File.Exists(path))
            {
                using (Stream dumpstream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    texture.SaveAsPng(dumpstream, texture.Width, texture.Height);
                }
            }
            return texture;
        }


        public static ContentManagerEncapsulator Encapsulator { get; private set; }

        public sealed class ContentManagerEncapsulator
        {

            public ContentManager ContentManager { get; private set; }

            public ContentManagerEncapsulator(ContentManager manager)
            {
                ContentManager = manager;
            }
        }

    }
}
