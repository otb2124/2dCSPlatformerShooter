using Platformer.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Entities.Types
{
    public class DecorationEntity : StaticEntity
    {
        public DecorationEntity(float width, float height, FlatVector pos, float Rotation) : base(width, height, pos, Rotation)
        {
            this.Body.owner = this;
        }
    }
}
