using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Graphics.Menu
{
    public class Dialogue
    {
        public string Speaker1;
        public string Speaker2;
        public Color Speaker1Color;
        public Color Speaker2Color;
        public string currentText;
        public int currentSpeaker;



        public Dialogue(string Speaker1, string Speaker2, Color Speaker1Color, Color Speaker2Color, string currentText, int currentSpeaker) 
        { 
            this.Speaker1 = Speaker1;
            this.Speaker2 = Speaker2;
            this.Speaker1Color = Speaker1Color;
            this.Speaker2Color = Speaker2Color;
            this.currentText = currentText;
            this.currentSpeaker = currentSpeaker;
        }
    }
}
