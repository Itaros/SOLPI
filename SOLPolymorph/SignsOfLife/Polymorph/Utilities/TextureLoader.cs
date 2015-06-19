using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SignsOfLife;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPolymorph.SignsOfLife.Polymorph.Utilities
{
    public class TextureLoader
    {

        private static TextureLoader _instance = new TextureLoader();
        public static TextureLoader Instance { get { return _instance; } }

        private List<Texture2D> _naiveIndex = new List<Texture2D>();

        public Texture2D GetTexture(int id)
        {
            return _naiveIndex[id];
        }

        public int LoadTextureAsPNG(string filename)
        {
            FileInfo finfo = ResolveTextureFile(filename);
            if (finfo != null)
            {
                Texture2D tex = TryLoadPNG(finfo);
                return Append(tex);
            }
            else
            {
                throw new FileNotFoundException("Can't load png: "+filename);
            }
        }

        private int Append(Texture2D tex)
        {
            _naiveIndex.Add(tex);
            return _naiveIndex.IndexOf(tex);
        }

        private Texture2D TryLoadPNG(FileInfo finfo)
        {
            Stream fileStream = finfo.Open(FileMode.Open,FileAccess.Read);
            var texture = Texture2D.FromStream(SpaceGame.GetGraphicsDevice(), fileStream);
            fileStream.Close();
            return texture;
        }

        private FileInfo ResolveTextureFile(string filename)
        {
            string path = "Polymorph" + Path.DirectorySeparatorChar + "Mods" + Path.DirectorySeparatorChar + filename;
            var file = new FileInfo(path);
            return file.Exists ? file : null;
        }


    }
}
