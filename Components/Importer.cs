using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Zenith.Components {
    public static class Importer {
        public static Texture2D GetTexture2DFromFile(GraphicsDevice gd, string filePath) {
            Texture2D result = null;
            using (FileStream fs = new(filePath, FileMode.Open, FileAccess.Read)) {
                result = Texture2D.FromStream(gd, fs);
                fs.Close();
            }
            return result;
        }

        public static string GetStringFromFile(string filePath) {
            string result = string.Empty;
            using (StreamReader reader = new(filePath)) {
                result = reader.ReadToEnd();
                reader.Close();
            }
            return result;
        }

        public static Texture2D CreateTexture(this Texture2D texture2D, GraphicsDevice graphics, Rectangle sourceRectangle) {
            Texture2D t = new(graphics, sourceRectangle.Width, sourceRectangle.Height);
            int count = sourceRectangle.Width * sourceRectangle.Height;
            Color[] data = new Color[count];
            texture2D.GetData(0, sourceRectangle, data, 0, count);
            t.SetData(data);
            return t;
        }
    }
}
