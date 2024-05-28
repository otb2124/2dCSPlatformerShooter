using Platformer.Physics;
using System.Collections.Generic;

namespace Platformer.Entities.Types
{
    public class Boss
    {

        public string name;
        public List<FlatEntity> bodyparts;
        public FlatVector pos;

        public Boss(string name, FlatVector pos)
        {
            bodyparts = new List<FlatEntity>();
            this.pos = pos;
            this.name = name;
            setBoss();
        }

        public void setBoss()
        {
            DynamicEntity body = new DynamicEntity(5, 3, pos, 0);

            Mob leftHand = new Mob(2, 2, 50, 50, 1, false, Mob.mobType.GrabbingMob, new FlatVector(pos.X - 2.5f, pos.Y + 3));
            

            Mob rightHand = new Mob(2, 2, 50, 50, 1, false, Mob.mobType.GrabbingMob, new FlatVector(pos.X + 2.5f, pos.Y + 3));

            bodyparts.Add(body);
            bodyparts.Add(leftHand);
            bodyparts.Add(rightHand);

            for (int i = 0; i < bodyparts.Count; i++)
            {
                bodyparts[i].isBossPart = true;
            }
        }


        public void Update(Game1 game)
        {
            for (int i = 0; i < bodyparts.Count; i++)
            {
                bodyparts[i].Update(game);
            }
        }

    }
}
