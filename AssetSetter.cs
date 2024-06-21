using Microsoft.Xna.Framework;
using Platformer.Entities;
using Platformer.Entities.group;
using Platformer.Entities.Types;
using Platformer.Graphics.Menu;
using Platformer.Utils;
using System;
using System.Collections.Generic;

namespace Platformer
{
    public class AssetSetter
    {
        private Game1 game1;
        public MapReader mapReader;
        public bool groupSet = false;
        public int currentMap;
        public List<Dialogue> dialogues;

        public bool isClean = false, isFilled = true;

        public AssetSetter(Game1 game1)
        {
            this.game1 = game1;
            this.mapReader = new MapReader(game1);
            this.dialogues = new List<Dialogue>();


        }

        public void setAll()
        {
            setDialogues();
            setEvents();
            mapReader.ExtractSaveInfoFromAllFiles();
        }


        public void loadSave(int index)
        {
            string path = "save";

            if (index < 10)
            {
                path += "0";
            }
            path += index.ToString();

            mapReader.ParseSaveData(path);
        }

        public void loadMap(int index)
        {
            if(game1.group != null)
            {
                mapReader.UpdateCurrentMapData();
            }
           
            clearMap(out List<GroupMember> groupMembers);
            loadEntitiesFromMap(groupMembers, index);
        }


        public void clearMap(out List<GroupMember> groupMemberList)
        {
            List<GroupMember> groupMembers = new List<GroupMember>();
            foreach (var entity in game1.entities)
            {
                if (entity is GroupMember groupMember)
                {
                    groupMembers.Add(groupMember);
                }


                game1.world.RemoveBody(entity.Body);

            }

            game1.entities.Clear();
            game1.backEntities.Clear();
            groupMemberList = groupMembers;
        }


        public void loadEntitiesFromMap(List<GroupMember> groupMembers, int index)
        {
            List<FlatEntity> mapEntities = mapReader.ParseMapElements(index);

            game1.entities.AddRange(groupMembers);
            foreach (var groupMember in groupMembers)
            {
                game1.world.AddBody(groupMember.Body);
            }

            int groupMembersAddedCount = 0;

            foreach (var entity in mapEntities)
            {
                if (entity is GroupMember && !groupSet)
                {
                    game1.entities.Add(entity);
                    game1.world.AddBody(entity.Body);
                    groupMembersAddedCount++;
                    if (groupMembersAddedCount >= 4)
                    {
                        groupSet = true;
                    }
                }
                else if (!(entity is GroupMember)) // Add non-GroupMember entities directly
                {
                    game1.entities.Add(entity);
                    game1.world.AddBody(entity.Body);
                }
            }



            currentMap = index;
            if (game1.player != null)
            {
                game1.player.isInterracting = null;
            }
            game1.uiManager.refresh();
        }


        public void InitializePlayState()
        {
            game1.group = new Group();
            for (int i = 0; i < game1.entities.Count; i++)
            {
                if (game1.entities[i] is GroupMember member)
                {
                    game1.group.CurrentGroup.Add(member);

                    if (member.isPlayer)
                    {
                        game1.player = member;
                    }
                }
            }
            game1.uiManager.uiList.Clear();
            game1.gameState = game1.PLAYSTATE;
            groupSet = true;
        }



        public void setDialogues()
        {
            dialogues.Add(new Dialogue("Bobby", "Jamie", Color.Aqua, Color.Silver, "Hy, Jamie. its me, bobby. How is it going?\nYou good?", 1));
            dialogues.Add(new Dialogue("Bobby", "Jamie", Color.Aqua, Color.Silver, "Yeah Im good. See ya, mate.", 2));
        }

        public void setEvents()
        {
            game1.eventHandler.eventList.Add(new Event(Event.GameEventType.battle, new System.Drawing.PointF(80, 12), new System.Drawing.SizeF(5, 5), 4, 50));
        }

        public void addEntity(FlatEntity entity)
        {
            game1.entities.Add(entity);
            game1.world.AddBody(entity.Body);
        }

        public void removeEntity(FlatEntity entity)
        {
            game1.entitiesToRemoval.Add(entity);
        }






    }
}