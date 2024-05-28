using Flat.Input;
using Microsoft.VisualBasic.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Platformer.Graphics.Generators;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;

namespace Platformer.Graphics.Menu
{
    public class ScrollPane : UIComponent
    {

        public Vector2 position { get; set; }
        public float height { get; set; }
        public float width { get; set; }
        public float scaledHeight { get; set; }
        public float scaledwidth { get; set; }
        public Vector2 scalePosition { get; set; }
        public Vector2 offset { get; set; }
        public Color color;
        public SpriteFont font;

        public float value;
        public string text;

        public List<UIComponent> components;

        // Viewport properties
        private int startIndex; // Index of the first visible component
        private int endIndex;   // Index of the last visible component
        private int maxVisibleComponents; // Maximum number of visible components
        private float componentHeight; // Height of each component
        private float scrollValue; // Current scroll position (0 to 1)


        public ScrollPane(string text, SpriteFont font, float width, float height)
        {
            this.position = Vector2.Zero;
            this.offset = new Vector2(0, -2f);
            this.width = width;
            this.height = height;
            this.font = font;
            this.text = text;
            components = new List<UIComponent>();

            // Initialize viewport properties
            startIndex = 0;
            endIndex = 0;
            maxVisibleComponents = (int)(height / 12); // Assuming each component has a height of 20 (adjust as needed)
            componentHeight = 12; // Height of each component (adjust as needed)
            scrollValue = 0;
        }

        public void Draw(Sprites sprites, Shapes shapes, Camera camera)
        {
            Vector2 textSize = font.MeasureString(text) * 0.2f; // Apply the scale factor
            Vector2 textPosition = new Vector2(position.X + (width - textSize.X) / 2 - width / 2, position.Y + height / 2.25f);

            // Draw the text
            sprites.DrawString(font, text, textPosition, 0f, Vector2.Zero, 0.1f, Color.White);

            // Calculate the visible range of components based on scroll position
            startIndex = (int)(scrollValue * (components.Count - maxVisibleComponents));
            endIndex = Math.Min(startIndex + maxVisibleComponents, components.Count) + 1;
            

                for (int i = startIndex; i < endIndex; i++)
                {
                  

                if(i <  components.Count)
                {
                    if (components[i] != null)
                    {
                        // Adjust the position of the component based on its index and scroll position
                        components[i].offset = new Vector2(components[i].offset.X, position.Y - (i - startIndex) * componentHeight + 5f);
                        components[i].Draw(sprites, shapes, camera);
                    }

                }
                    
                
            }
        }






        public void Update(Game1 game)
        {
            // Get the current mouse state
            FlatMouse mouse = game.mouse;

            // Calculate the mouse wheel delta
            int mouseWheelDelta = mouse.ScrollDelta;

            // Check if the mouse wheel was scrolled
            if (mouseWheelDelta != 0)
            {
                // Assuming you have a reference to the ScrollPane instance called scrollPane
                this.HandleMouseWheel(mouseWheelDelta);

                // Reset the previous scroll wheel value after handling
                mouse.PreviousScrollWheelValue = mouse.CurrentScrollWheelValue;
            }


            

            // Update the components
            foreach (var component in this.components)
            {
                component.Update(game);
            }
        }


        public void HandleMouseWheel(int delta)
        {
            // Adjust the scrolling speed as needed
            Scroll(delta * 0.1f);
        }





        public void add(UIComponent component)
        {
            this.components.Add(component);
        }


        public void Scroll(float delta)
        {
            scrollValue = MathHelper.Clamp(scrollValue + delta, 0, 1);
        }


    }
}
