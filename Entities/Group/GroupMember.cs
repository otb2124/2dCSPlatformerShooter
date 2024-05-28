using Platformer.Entities.items;
using Platformer.Physics;
using System.Collections.Generic;
using Platformer.Entities.Types;
using Flat.Input;
using Platformer.Graphics.Generators;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Platformer.Entities.group
{
    public class GroupMember : LiveEntity
    {
        public string name;
        public string personality;
        public string natioanality;
        public string Class;
        public bool isPlayer;

        public FlatEntity isInterracting = null;

        public GroupMember(float width, float height, float maxHP, float currentHP, string name, bool isPlayer, FlatVector pos, Weapon[] weapons) : base(width, height, maxHP, currentHP, pos)
        {
            this.maxHP = maxHP;
            this.currentHP = currentHP;
            this.Body.MoveTo(pos);

            direction = FlatVector.Zero;
            bodyCrouchHeight = Body.Height/2;

            this.name = name;
            this.Body.owner = this;
            jumpForce = 1.5f;
            currentJumpForce = jumpForce;
            

            this.isPlayer = isPlayer;
            this.weapons = weapons;
            this.inventory = new List<Item>();

            setStats();
        }


        public void setStats()
        {

            switch (name)
            {
                case "George":
                    this.personality = "Furious";
                    this.natioanality = "English";
                    this.Class = "Tank";

                    inventory.Add(new CreditItem());
                    inventory.Add(new CreditItem());
                    inventory.Add(new CreditItem());
                    inventory.Add(new CreditItem());

                    inventory.Add(new CreditItem());
                    inventory.Add(new CreditItem());
                    inventory.Add(new CreditItem());


                    break;
                case "Toni":
                    this.personality = "Stable";
                    this.natioanality = "Croat";
                    this.Class = "Sniper";

                    inventory.Add(new CreditItem());
                    inventory.Add(new CreditItem());
                    inventory.Add(new CreditItem());
                    inventory.Add(new CreditItem());

                    inventory.Add(new CreditItem());
                    inventory.Add(new CreditItem());
                    inventory.Add(new CreditItem());


                    break;
                case "Orest":
                    this.personality = "Mechanic";
                    this.natioanality = "Ukrainian";
                    this.Class = "Mechanic";

                    inventory.Add(new CreditItem());
                    inventory.Add(new CreditItem());
                    inventory.Add(new CreditItem());
                    inventory.Add(new CreditItem());

                    inventory.Add(new CreditItem());
                    inventory.Add(new CreditItem());
                    inventory.Add(new CreditItem());


                    break;
                case "John":
                    this.personality = "Mechanic";
                    this.natioanality = "Ukrainian";
                    this.Class = "Mechanic";
                    

                    inventory.Add(new CreditItem());
                    inventory.Add(new CreditItem());
                    inventory.Add(new CreditItem());
                    inventory.Add(new CreditItem());

                    inventory.Add(new CreditItem());
                    inventory.Add(new CreditItem());
                    inventory.Add(new CreditItem());

                    armor = 100;
                    break;
            }


            weapons[0].owner = this;
            weapons[1].owner = this;
            currentWeapon = weapons[0];
        }



        public new void Update(Game1 game)
        {

            if(!isPlayer)
            {
                handleAiBehaviour(game);
                for (int i = 0; i < game.entities.Count; i++)
                {
                    game.statsManager.HandleGroupMemberTouch(this, game.entities[i]);
                }
            }
            else
            {   
                FlatMouse mouse = FlatMouse.Instance;
                FlatVector mouseWorldPosition = FlatConverter.ToFlatVector(mouse.GetMouseWorldPosition(game, game.screen, game.camera));
                FlatVector direc = mouseWorldPosition - Body.Position;
                direction = FlatMath.Normalize(direc);
            }



            base.Update(game);

        }



        public void handleAiBehaviour(Game1 game)
        {


            if(game.gameState != game.GAMEMENUSTATE)
            {
                if (this.Class == "Tank")
                {

                    game.aiManager.FollowIfFar(this, game.player, 9f);
                    game.aiManager.SetDirectionToClosestInRadiusAndShootTillDead(this, typeof(Mob), 25f);
                }


                if (this.Class == "Sniper")
                {

                    game.aiManager.FollowIfFar(this, game.player, 5.5f);
                    game.aiManager.SetDirectionToClosestInRadiusAndShootTillDead(this, typeof(Mob), 25f);
                }


                if (this.Class == "Mechanic")
                {

                    game.aiManager.FollowIfFar(this, game.player, 2.5f);
                    game.aiManager.SetDirectionToClosestInRadiusAndShootTillDead(this, typeof(Mob), 25f);

                }
            }
            else
            {
                game.aiManager.AdjustGroupMemberPosition(this, 5f);
            }
            
        }

    }


}
