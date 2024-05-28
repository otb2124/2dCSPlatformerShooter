using Platformer.Physics;
using Platformer.Utils;
using SharpDX.Direct3D9;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;

namespace Platformer.Entities.Types
{
    public class Event
    {

        public bool hasStarted = false;
        public bool hasEnded = false;
        public RectangleF startRect;
        public int startRectMap;

        public int battleAmountOfEnemies;
        public int currentBattleCount = 0;

        public GameEventType type;
        public enum GameEventType { 
        
            battle,
            bossBattle
        
        };




        public Event(GameEventType type, PointF pos, SizeF size, int startRectMap, int amount)
        {
            this.type = type;
            this.startRect = new RectangleF(pos-size/2, size);
            this.startRectMap = startRectMap;
            this.battleAmountOfEnemies = amount;
        }



        public void Update(Game1 game)
        {

            if(!hasStarted && !hasEnded)
            {
                if (game.aSetter.currentMap == startRectMap)
                {
                    

                    if (startRect.Contains(game.player.Body.Position.X, game.player.Body.Position.Y))
                    {
                        if (this.type == GameEventType.bossBattle)
                        {
                            Boss boss = new Boss("orest", new FlatVector(game.player.Body.Position.X + 30, game.player.Body.Position.Y + 5));
                            for (int i = 0; i < boss.bodyparts.Count; i++)
                            {
                                game.aSetter.addEntity(boss.bodyparts[i]);
                            }
                            
                        }
                        hasStarted = true;
                    }
                }
            }


            if (hasStarted)
            {
                if(this.type == GameEventType.battle)
                {

                    float spawnChance = RandomHelper.RandomSingle(0, 1);

                    if (spawnChance < 0.01f)
                    {
                        float randomX = RandomHelper.RandomSingle(0, 1);
                        

                        if (randomX > 0.5f)
                        {
                            randomX = 1;
                        }
                        else
                        {
                            randomX = -1;
                        }

                        Mob mob = new Mob(2, 1, 50, 50, 5, false, Mob.mobType.WalkingMob, new FlatVector(game.player.Body.Position.X + (30 * randomX), game.player.Body.Position.Y + 5));
                        mob.AgroRange = 100;

                        game.aSetter.addEntity(mob);
                        currentBattleCount++;
                    }
                    
                    if(currentBattleCount > battleAmountOfEnemies)
                    {
                        hasEnded = true;
                        hasStarted = false;
                    }

                }
                else if(this.type == GameEventType.bossBattle)
                {

                }
            }


            
        }

    }
}
