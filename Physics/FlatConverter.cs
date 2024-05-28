using Microsoft.Xna.Framework;
using Platformer.Physics;
using Platformer.Graphics.Generators;

public static class FlatConverter
{
    public static Vector2 ToScreenVector(FlatVector flatVector, Screen screen, Camera camera)
    {
        float x = (flatVector.X - camera.Position.X) * camera.Zoom + screen.Width / 2f;
        float y = screen.Height - ((flatVector.Y - camera.Position.Y) * camera.Zoom + screen.Height / 2f);
        return new Vector2(x, y);
    }

    public static FlatVector ToFlatVector(Vector2 vector2)
    {
        return new FlatVector(vector2.X, vector2.Y);
    }

    public static Vector2 ToVector2(FlatVector flatVector)
    {
        return new Vector2(flatVector.X, flatVector.Y);
    }
}
