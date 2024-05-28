using Platformer.Graphics.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Entities.group
{
    public class Group
    {

        public List<GroupMember> CurrentGroup;



        public Group()
        {
            CurrentGroup = new List<GroupMember>();
        }


        public void Update(Game1 game)
        {

            for (int i = 0; i < CurrentGroup.Count; i++)
            {
                if (!CurrentGroup[i].isDead) {
                    CurrentGroup[i].Update(game);
                    
                   
                }

            }            
        }



        public void Draw(Shapes shapes)
        {
            for (int i = 0; i < CurrentGroup.Count; i++)
            {
                CurrentGroup[i].Draw(shapes);
            }
        }
    }
}
