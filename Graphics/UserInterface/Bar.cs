using Microsoft.Xna.Framework;
using Platformer.Entities.group;
using Platformer.Entities.Types;
using Platformer.Graphics.Generators;
using System;

namespace Platformer.Graphics.Menu
{
    public class Bar : UIComponent
    {

        public Vector2 position { get; set; }
        public float width { get; set; }
        public float height { get; set; }
        public float scaledHeight { get; set; }
        public float scaledwidth { get; set; }
        public Vector2 scalePosition { get; set; }
        public Vector2 offset { get; set; }
        public Color color;
        public int type;

        public Vector2 positionOffset;
        public float value;

        public bool isPlayer;

        public LiveEntity lent;

        public Bar(Vector2 positionOffset, float width, float height, int type, Color color, bool isPlayer, LiveEntity lent)
        {

            this.offset = positionOffset;
            this.position = Vector2.Zero;
            this.width = width;
            this.height = height;
            this.color = color;
            this.type = type;
            this.value = 0.3f; // Fully filled by default
            this.isPlayer = isPlayer;
            this.lent = lent;
        }


        public void Draw(Sprites sprites, Shapes shapes, Camera camera)
        {

            float adjustedWidth = scaledwidth;
            float adjustedHeight = scaledHeight;

            // Calculate the scaled position of the bar relative to the camera's zoom level and screen resolution
            Vector2 scaledPosition = scalePosition;

            // Calculate the filled width of the bar
            float filledWidth = adjustedWidth * this.value;
            float offsetX = type == 1 ? (adjustedWidth - filledWidth) / 2f : 0; // Calculate the offset for aligning the filled part

            // Adjust the position based on the type
            Vector2 adjustedPosition = type == 1 ? position : new Vector2(scaledPosition.X - (adjustedWidth - filledWidth) / 2f, scaledPosition.Y);

            // Draw the background bar
            shapes.DrawBoxFill(new Vector2(scaledPosition.X, scaledPosition.Y), adjustedWidth, adjustedHeight, Color.Gray);

            // Adjust the position and size based on the zoom level and draw the filled part of the bar
            shapes.DrawBoxFill(adjustedPosition + new Vector2(offsetX, 0), filledWidth, adjustedHeight, color);
        }




        public void setValue(float currentHP, float maxHP)
        {
            float percentage = currentHP / maxHP;
            this.value = MathHelper.Clamp(percentage, 0f, 1f);
        }



        public void Update(Game1 game)
        {

            if (type == 0)
            {
                setValue(lent.currentHP, lent.maxHP);
            }
            if (type == 1)
            {
                setValue(lent.currentWeapon.currentAmmoAmount, lent.currentWeapon.ammoAmount);
            }

            if (!isPlayer)
            {
                scalePosition = new Vector2(lent.Body.Position.X, lent.Body.Position.Y + lent.Body.Height + 0.5f);
            }
            
        }
    }
}
