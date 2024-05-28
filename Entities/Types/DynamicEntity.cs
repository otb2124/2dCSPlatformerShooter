using Platformer.Physics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Entities.Types
{
    public class DynamicEntity : FlatEntity
    {


        public DynamicEntity(float width, float height, FlatVector pos, float Rotation) : base(width, height, false, pos)
        {
            this.Body.Rotate(Rotation);
        }

        public DynamicEntity(float radius, FlatVector pos, float Rotation) : base(radius, false, pos)
        {
            this.Body.Rotate(Rotation);
        }
    }
}
