using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Zenith.Components {
    public class Importer {
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
    }
}
