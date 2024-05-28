using Platformer.Physics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Entities.Types
{
    public class StaticEntity : FlatEntity
    {

        public StaticEntity(float width, float height, FlatVector pos, float Rotation) : base(width, height, true, pos)
        {
            this.Body.Rotate(Rotation);
        }

        public StaticEntity(float radius, FlatVector pos, float Rotation) : base(radius, true, pos)
        {
            this.Body.Rotate(Rotation);
        }
    }
}
