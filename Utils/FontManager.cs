using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Utils
{
    public class FontManager
    {

        public SpriteFont[] fonts;



        private Game1 game;


        public FontManager(Game1 game)
        {

            this.game = game;
            fonts = new SpriteFont[10];
            setFonts();
        }



        private SpriteFont getFontPath(string fontname)
        {
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string relativePath = Path.Combine(projectDirectory, "res", "fonts", fontname);

            return game.Content.Load<SpriteFont>(relativePath); ;
        }


        public void setFonts()
        {
            fonts[0] = getFontPath("Consolas12");
            fonts[1] = getFontPath("Consolas10");
        }
    }
}
