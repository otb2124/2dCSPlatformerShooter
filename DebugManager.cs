using Microsoft.Xna.Framework;
using Platformer.Entities.Types;
using Platformer.Entities.group;
using Platformer.Graphics.Menu;
using Platformer.Physics;
using Platformer.Utils;

namespace Platformer
{
    public class DebugManager
    {

        private ConsoleTextField console;
        private Game1 game;

        public DebugManager(Game1 game)
        {
            this.game = game;


        }






        public void start()
        {
            game.gameMode = game.DEBUGMODE;
            game.uiManager.setDebugConsoleWindow();
            console = getConsole();
        }


        public void stop()
        {
            game.gameMode = game.PLAYMODE;
            game.uiManager.clerDebugConsoleWindow();
        }

        public void Execute(string command)
        {



            string[] parts = command.Split(' ');
            parts[0] = parts[0].Substring(1);

            switch (parts[0])
            {

                case "player":
                    if (parts.Length >= 2)
                    {





                        if (parts[1] == "set")
                        {

                            if (parts[2] == "pos")
                            {
                                float x, y;
                                if (float.TryParse(parts[3], out x) && float.TryParse(parts[4], out y))
                                {
                                    game.player.Body.MoveTo(new FlatVector(x, y));
                                    console.commandHistory.Add(command + " - Player set pos to " + x + ", " + y);
                                }
                                else
                                {
                                    console.commandHistory.Add(command + " - Insufficient coordinates provided for " + parts[2]);
                                }
                            }
                            else if (parts[2] == "chp")
                            {
                                float x;
                                if (float.TryParse(parts[3], out x))
                                {
                                    game.player.currentHP = x;
                                    console.commandHistory.Add(command + " - Player set current hp to " + x);
                                }
                                else
                                {
                                    console.commandHistory.Add(command + " - Insufficient number provided for " + parts[2]);
                                }
                            }
                            else if (parts[2] == "mhp")
                            {
                                float x;
                                if (float.TryParse(parts[3], out x))
                                {
                                    game.player.maxHP = x;
                                    console.commandHistory.Add(command + " - Player set max hp to " + x);
                                }
                                else
                                {
                                    console.commandHistory.Add(command + " - Insufficient number provided for " + parts[2]);
                                }
                            }
                            else
                            {
                                console.commandHistory.Add(command + " - Insufficient parameter provided for " + parts[1]);
                            }
                        }
                        else if (parts[1] == "get")
                        {
                            if (parts[2] == "pos")
                            {
                                console.commandHistory.Add(command + " - Player X is " + game.player.Body.Position.X + ", Y is " + game.player.Body.Position.Y);
                            }
                            else if (parts[2] == "hp")
                            {
                                console.commandHistory.Add(command + " - Player HP is " + game.player.currentHP.ToString() + "/" + game.player.maxHP.ToString());
                            }
                            else
                            {
                                console.commandHistory.Add(command + " - Insufficient parameter provided for " + parts[1]);
                            }
                        }
                        else
                        {
                            console.commandHistory.Add(command + " - Insufficient command provided for " + parts[0]);
                        }

                    }
                    else
                    {
                        console.commandHistory.Add(command + " - Insufficient parameters provided for " + parts[0]);
                    }
                    break;




                case "add":
                    if (parts.Length >= 5)
                    {



                        float x, y;

                        if (float.TryParse(parts[2], out x) && float.TryParse(parts[3], out y))
                        {
                            if (parts[1] == "box")
                            {
                                AddObject(0, new FlatVector(x, y));
                                console.commandHistory.Add(command + " - Box added at X: " + x + ", Y: " + y);
                            }
                            if (parts[1] == "circle")
                            {
                                AddObject(1, new FlatVector(x, y));
                                console.commandHistory.Add(command + " - Circle added at X: " + x + ", Y: " + y);
                            }

                        }
                        else
                        {
                            console.commandHistory.Add(command + " - Invalid coordinates provided for " + parts[0] + " " + parts[1]);
                        }



                        if (float.TryParse(parts[3], out x) && float.TryParse(parts[4], out y))
                        {
                            if (parts[1] == "mob")
                            {

                                if (parts[2] == "w")
                                {
                                    AddObject(2, new FlatVector(x, y));
                                    console.commandHistory.Add(command + " - Mob added at X: " + x + ", Y: " + y);
                                }
                                if (parts[2] == "j")
                                {
                                    AddObject(3, new FlatVector(x, y));
                                    console.commandHistory.Add(command + " - Mob added at X: " + x + ", Y: " + y);
                                }
                                else
                                {
                                    console.commandHistory.Add(command + " - Wrong Mob type");
                                }


                            }
                        }
                        else
                        {
                            console.commandHistory.Add(command + " - Invalid coordinates provided for " + parts[0] + " " + parts[1]);
                        }
                        
                        
                    }
                    else
                    {
                        console.commandHistory.Add(command + " - Insufficient parameters provided for " + parts[0]);
                    }
                    break;



                case "closest":

                    if (parts.Length >= 2)
                    {

                        FlatEntity entity = GetClosest();

                        if (parts[1] == "d")
                        {


                                DestroyableEntity entity1 = (DestroyableEntity)entity;

                                if (entity != null)
                                {
                                    if (parts[2] == "hp")
                                    {
                                        console.commandHistory.Add(command + " - Closest entity currHP is " + entity1.currentHP.ToString());
                                    }
                                    if (parts[2] == "maxhp")
                                    {
                                        console.commandHistory.Add(command + " - Closest entity maxHP is " + entity1.maxHP.ToString());
                                    }

                                    if (parts[2] == "color")
                                    {
                                        console.commandHistory.Add(command + " - Closest entity color is " + entity1.Color.ToString());
                                    }
                                }
                                else
                                {
                                    console.commandHistory.Add(command + " - No clothest destroyable");
                                }
                            
                            

                            
                        }
                        else
                        {
                            console.commandHistory.Add(command + " - Insufficient class identificator provided for " + parts[0]);
                        }


                        if (parts[1] == "class")
                        {

                                console.commandHistory.Add(command + " - Closest entity class is " + entity.GetType().ToString());
                            
                            
                        }




                    }
                    else
                    {
                        console.commandHistory.Add(command + " - Insufficient parameters provided for " + parts[0]);
                    }
                    break;



                case "setmap":

                    if (parts.Length >= 2)
                    {
                        if (parts[1] == "next")
                        {
                            game.eventHandler.MoveToNextMap();
                        }
                        if (parts[1] == "prev")
                        {
                            game.eventHandler.MoveToPrevMap();
                        }
                    }
                    else
                    {
                        console.commandHistory.Add(command + " - Insufficient parameters provided for " + parts[0]);
                    }

                    break;
                default:

                    console.commandHistory.Add(command + " - unknown command");
                    break;
            }
        }




        public void AddObject(int id, FlatVector pos)
        {

            switch (id)
            {
                case 0:

                    float width = RandomHelper.RandomSingle(2f, 3f);
                    float height = RandomHelper.RandomSingle(2f, 3f);

                    DestroyableEntity box = new DestroyableEntity(width, height, false, 50, 50, pos, 0);

                    game.aSetter.addEntity(box);


                    break;
                case 1:

                    float radius = RandomHelper.RandomSingle(1.25f, 1.5f);

                    DestroyableEntity circle = new DestroyableEntity(radius, false, 50, 50, pos, 0);

                    game.aSetter.addEntity(circle);
                    break;

                case 2:
                    float width1 = RandomHelper.RandomSingle(2f, 3f);
                    float height1 = RandomHelper.RandomSingle(2f, 3f);

                    Mob mob = new Mob(width1, height1, 100, 100, 5, false, Mob.mobType.WalkingMob, pos);

                    game.aSetter.addEntity(mob);

                    break;

                case 3:
                    float width2 = RandomHelper.RandomSingle(2f, 3f);
                    float height2 = RandomHelper.RandomSingle(2f, 3f);

                    Mob mob2 = new Mob(width2, height2, 50, 50, 10, false, Mob.mobType.JumpingMob, pos);

                    game.aSetter.addEntity(mob2);

                    break;
            }








        }

        public FlatEntity GetClosest()
        {
            FlatEntity closestEntity = null;
            float closestDistanceSquared = float.MaxValue;

            foreach (var entity in game.entities)
            {

                if (entity is GroupMember || entity is StaticEntity)
                    continue;
                // Calculate the squared distance between the player and the entity
                float distanceSquared = Vector2.DistanceSquared(FlatConverter.ToVector2(game.player.Body.Position), FlatConverter.ToVector2(entity.Body.Position));

                // If the distance is closer than the current closest, update the closest entity and distance
                if (distanceSquared < closestDistanceSquared)
                {
                    closestEntity = entity;
                    closestDistanceSquared = distanceSquared;
                }
            }

            return closestEntity;
        }




        public ConsoleTextField getConsole()
        {
            for (int i = 0; i < game.uiManager.uiList.Count; i++)
            {
                if (game.uiManager.uiList[i] is ConsoleTextField)
                {
                    return (ConsoleTextField)game.uiManager.uiList[i];
                }
            }

            return null;
        }
    }
}