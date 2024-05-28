using Platformer.Entities.Types;
using Platformer.Physics;
using System;
using System.Collections.Generic;


namespace Platformer
{
    public class EventManager
    {

        Game1 game;
        public int prevMapIndex;
        public List<Event> eventList;

        public EventManager(Game1 game)
        {
            this.game = game;
            eventList = new List<Event>();
        }



        public void handleEvent(int id)
        {
            switch (id)
            {
                case 0:
                    MoveToNextMap();
                    break;
                case 1:
                    MoveToPrevMap();
                    break;
                case 2:
                    //writeSave
                    break;

            }
        }



        public void MoveToNextMap()
        {

            prevMapIndex = game.aSetter.currentMap;
            game.aSetter.currentMap++;
            game.aSetter.loadMap(game.aSetter.currentMap);


            switch (game.aSetter.currentMap)
            {
                case 1:
                    TelePortGroupTo(new FlatVector(23, 14));
                    break;
            }



        }

        public void MoveToPrevMap()
        {
            prevMapIndex = game.aSetter.currentMap;
            game.aSetter.currentMap--;
            game.aSetter.loadMap(game.aSetter.currentMap);



            switch (game.aSetter.currentMap)
            {
                case 0:
                    TelePortGroupTo(new FlatVector(20, 14));
                    break;
            }

        }




        public void TelePortGroupTo(FlatVector cords)
        {

            for (int i = 0; i < game.group.CurrentGroup.Count; i++)
            {
                game.group.CurrentGroup[i].Body.MoveTo(cords);
                game.group.CurrentGroup[i].isInterracting = null;
            }
        }




        public void Update(Game1 game)
        {
            for (int i = 0; i < eventList.Count; i++)
            {
                eventList[i].Update(game);
            }

        }
    }

}
