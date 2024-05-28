using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.Entities.Types;
using Platformer.Graphics.Generators;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D9;
using System;
using System.Diagnostics;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using Color = Microsoft.Xna.Framework.Color;

namespace Platformer.Graphics.Menu
{
    public class DialogueBox : UIComponent
    {

        public Vector2 position { get; set; }
        public Vector2 scalePosition { get; set; }
        public float height { get; set; }
        public float width { get; set; }
        public float scaledHeight { get; set; }
        public float scaledwidth { get; set; }
        public Vector2 offset { get; set; }
        public Color color;
        public SpriteFont font;

        public float value;
        public string speaker1;
        public string speaker2;
        public string textDialogue;

        public Color speaker1img;
        public Color speaker2img;

        public UIComponent component;

        public int currentSpeaker;

        public Dialogue dialogue;
        public NPC npc;


        public DialogueBox(Vector2 pos, float width, float height, SpriteFont font, NPC npc, AssetSetter asetter)
        {



            this.offset = pos;
            this.width = width;
            this.height = height;
            this.color = new Color(0, 0, 0f, 0.2f);
            this.font = font;
            this.npc = npc;

            dialogue = asetter.dialogues[npc.currentDialogueId];
            
            this.speaker1 = dialogue.Speaker1;
            this.speaker2 = dialogue.Speaker2;
            this.textDialogue = dialogue.currentText;
            this.speaker1img = dialogue.Speaker1Color;
            this.speaker2img = dialogue.Speaker2Color;
            this.currentSpeaker = dialogue.currentSpeaker;
        }

        public void Draw(Sprites sprites, Shapes shapes, Camera camera)
        {

            // Speakers
            float speaker1Width = width / 3; // Adjust the width of the speakers as needed
            float speaker1Height = height*3;
            float speaker2Width = width / 3; // Adjust the width of the speakers as needed
            float speaker2Height = height * 3;// Use the height of the dialogue box for the speaker height
            float currentSpeakerScale = 1.2f;


            if(currentSpeaker == 1)
            {
                speaker1Width *= currentSpeakerScale;
                speaker1Height *= currentSpeakerScale;

                speaker2Width /= currentSpeakerScale;
                speaker2Height /= currentSpeakerScale;
            }
            else
            {
                speaker2Width *= currentSpeakerScale;
                speaker2Height *= currentSpeakerScale;

                speaker1Width /= currentSpeakerScale;
                speaker1Height /= currentSpeakerScale;
            }

            Vector2 speaker1Position = new Vector2(position.X - width / 2, position.Y+(height/2));
            shapes.DrawBoxFill(speaker1Position, speaker1Width, speaker1Height, speaker1img);

            // Position the second speaker to the right of the dialogue box
            Vector2 speaker2Position = new Vector2(position.X + width / 2, position.Y+(height/2));
            shapes.DrawBoxFill(speaker2Position, speaker2Width, speaker2Height, speaker2img);

            // Background
            shapes.DrawBoxFill(position, width, height, color);



            // Name of speaker1
            Vector2 textSize = font.MeasureString(speaker1) * 0.2f;
            Vector2 textPosition = new Vector2(position.X + (width - textSize.X) / 2 - width / 1.1f, position.Y + height / 2.25f);
            // Check if speaker1 is the current speaker
            Color speaker1Color = (currentSpeaker == 1) ? Color.White : Color.Gray;
            sprites.DrawString(font, speaker1, textPosition, 0f, Vector2.Zero, 0.2f, speaker1Color);

            // Name of speaker2
            textSize = font.MeasureString(speaker2) * 0.2f;
            // Calculate the position to be on the opposite side of speaker1
            textPosition = new Vector2(position.X - (width - textSize.X) / 2 + width / 1.1f - textSize.X, position.Y + height / 2.25f);
            // Check if speaker2 is the current speaker
            Color speaker2Color = (currentSpeaker == 2) ? Color.White : Color.Gray;
            sprites.DrawString(font, speaker2, textPosition, 0f, Vector2.Zero, 0.2f, speaker2Color);



            textSize = font.MeasureString(textDialogue) * 0.1f;
            textPosition = new Vector2(position.X + (width - textSize.X) / 2 - width / 2, position.Y);
            sprites.DrawString(font, textDialogue, textPosition, 0f, Vector2.Zero, 0.1f, Color.White);


            if (this.component != null)
            {
                component.Draw(sprites, shapes, camera);
            }
        }







        public void Update(Game1 game)
        {

            game.camera.GetExtents(out _, out _, out float bottom, out _);

            float x = game.camera.Position.X;
            float y = bottom + height;

            this.position = new Vector2(x, y);


            if (this.component != null)
            {
                component.Update(game);
            }

            
        }



        public void add(UIComponent component)
        {
            this.component = component;
        }
    }
}
