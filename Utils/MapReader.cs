using Newtonsoft.Json.Linq;
using Platformer.Entities.Types;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using Platformer.Entities;
using Platformer.Entities.group;
using Platformer.Physics;
using Microsoft.Xna.Framework;

namespace Platformer.Utils
{
    public class MapReader
    {
        public Game1 game;
        public SaveData saveData;

        public List<string> saveDates = new List<string>();
        public List<string> inGameTimes = new List<string>();
        public List<float> progresses = new List<float>();
        public List<int> currentMaps = new List<int>();
        public List<string> mapNames = new List<string>();

        public MapReader(Game1 game) { this.game = game; }


        public void ParseSaveData(string saveName)
        {
            try
            {
                string json = File.ReadAllText(GetSaveFilePath(saveName));
                saveData = JsonConvert.DeserializeObject<SaveData>(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error parsing save file: {ex.Message}");
            }

        }

        public List<List<GameElement>> GetMaps()
        {
            List<List<GameElement>> mapsData = new List<List<GameElement>>();

            foreach (var mapData in saveData.mapInfo.maps)
            {
                List<GameElement> mapEntities = new List<GameElement>();
                mapEntities.AddRange(mapData.setData);
                mapEntities.AddRange(mapData.updatableData);
                mapsData.Add(mapEntities);
            }

            return mapsData;
        }

        public List<GameElement> GetMap(int id)
        {
            return GetMaps()[id];
        }

        public List<FlatEntity> ParseMapElements(int id)
        {
            List<FlatEntity> entities = new List<FlatEntity> ();

            foreach (var element in GetMap(id))
            {
                FlatEntity entity = null;

                
                
                float width = element.Width / 10;
                float height = element.Height / 10;
                float radius = (width + height) / 4;
                float x = element.X / 10 + width/2;
                float y = element.Y / 10 + height/2;
                FlatVector pos = new FlatVector(x, y);

                float angle = 0;
                if (element.RotationAngle > 0)
                {
                    angle = -MathHelper.TwoPi / element.RotationAngle;
                }
               

                switch (element.Class)
                {
                    case "Static Rectangle":
                        entity = new StaticEntity(width, height, pos, angle);
                        break;
                    case "Static Circle":
                        entity = new StaticEntity(radius, pos, angle);
                        break;
                    case "Dynamic Rectangle":
                        entity = new DynamicEntity(width, height, pos, angle);
                        break;
                    case "Dynamic Circle":
                        entity = new DynamicEntity(radius, pos, angle);
                        break;



                    case "Destroyable Static Rectangle":
                        entity = new DestroyableEntity(width, height, element.IsStatic, element.MaxHP, element.CurrentHP, pos, angle);
                        break;
                    case "Destroyable Static Circle":
                        entity = new DestroyableEntity(radius, element.IsStatic, element.MaxHP, element.CurrentHP, pos, angle);
                        break;
                    case "Destroyable Dynamic Rectangle":
                        entity = new DestroyableEntity(width, height, element.IsStatic, element.MaxHP, element.CurrentHP, pos, angle);
                        break;
                    case "Destroyable Dynamic Circle":
                        entity = new DestroyableEntity(radius, element.IsStatic, element.MaxHP, element.CurrentHP, pos, angle);
                        break;



                    case "Mob":
                        if(element.MobType != "WheelMob")
                        {
                            entity = new Mob(width, height, element.MaxHP, element.CurrentHP, 10, false, Mob.ConvertToMobType(element.MobType), pos);
                        }
                        else
                        {
                            entity = new Mob(radius, element.MaxHP, element.CurrentHP, 10, false, Mob.ConvertToMobType(element.MobType), pos);
                        }
                        
                        break;
                    case "NPC":
                        entity = new NPC(width, height, element.MaxHP, element.CurrentHP, pos);
                        break;
                    case "GroupMember":
                        Weapon[] weapons = new Weapon[2];
                        weapons[0] = new Weapon(element.Weapons[0].Damage, element.Weapons[0].BulletSpeed, element.Weapons[0].BulletDelay, element.Weapons[0].BulletLifeCount, element.Weapons[0].AmmotAmount, element.Weapons[0].ReloadSpeed, element.Weapons[0].Spray, Entities.Weapons.Projectile.ProjectileType.bullet, (LiveEntity)entity);
                        weapons[1] = new Weapon(element.Weapons[1].Damage, element.Weapons[1].BulletSpeed, element.Weapons[1].BulletDelay, element.Weapons[1].BulletLifeCount, element.Weapons[1].AmmotAmount, element.Weapons[1].ReloadSpeed, element.Weapons[1].Spray, Entities.Weapons.Projectile.ProjectileType.bullet, (LiveEntity)entity);
                        entity = new GroupMember(width, height, element.MaxHP, element.CurrentHP, element.Name, element.IsPlayer, pos, weapons);
                        break;
                    case "Interractive Item":
                        entity = new InterractiveItem(width, height, element.IsStatic, element.EventID, pos);
                        break;
                    case "Ladder":
                        entity = new LadderEntity(width, height, pos, angle);
                        break;
                    case "Platform":
                        entity = new PlatformEntity(width, height, pos, angle);
                        break;
                    case "Decoration":
                        entity = new DecorationEntity(width, height, pos, angle);
                        break;
                    default:
                        Debug.WriteLine("Unknown Entity class. Skip loading");
                        break;
                }

                if(entity != null)
                {
                    entities.Add(entity);
                }
                
            }

            return entities;
        }


        public void UpdateCurrentMapData()
        {
            // Initialize a new MapData object
            var currentMapIndex = game.eventHandler.prevMapIndex;

            var existingMapData = saveData.mapInfo.maps[currentMapIndex];

            var mapData = new MapData
            {
                setData = existingMapData.setData,
                updatableData = new List<GameElement>(),
            };

            // Iterate through the game entities
            foreach (var entity in game.entities)
            {
                if (entity is Mob || entity is NPC || entity is InterractiveItem || entity is DestroyableEntity)
                {

                    var gameElement = new GameElement();

                    int width = (int)entity.Body.Width * 10;
                    int height = (int)entity.Body.Height * 10;
                    int radius = (int)entity.Body.Radius * 10;
                    int x = (int)entity.Body.Position.X * 10 - width / 2;
                    int y = (int)entity.Body.Position.Y * 10 - height / 2;
                    

                    float angle = 0;
                    if (entity.Body.Angle > 0)
                    {
                        angle = -MathHelper.TwoPi * entity.Body.Angle;
                    }



                    if (entity is Mob mob && mob.Body.Radius <= 0)
                    {
                        gameElement = new GameElement
                        {
                            Class = "Mob",
                            Width = width,
                            Height = height,
                            X = x,
                            Y = y,
                            RotationAngle = (int)angle,
                            MaxHP = ((Mob)entity).maxHP,
                            CurrentHP = ((Mob)entity).currentHP,
                            MobType = ((Mob)entity).thisMobtype.ToString()
                            //Weapons = 
                        };
                    }
                    else if (entity is Mob mob1 && mob1.Body.Radius > 0)
                    {
                        gameElement = new GameElement
                        {
                            Class = "Mob",
                            Width = radius*2,
                            Height = radius*2,
                            X = x,
                            Y = y,
                            RotationAngle = (int)angle,
                            MaxHP = ((Mob)entity).maxHP,
                            CurrentHP = ((Mob)entity).currentHP,
                            MobType = ((Mob)entity).thisMobtype.ToString()
                            //Weapons = 
                        };
                    }
                    else if (entity is NPC)
                    {
                        gameElement = new GameElement
                        {
                            Class = "NPC",
                            Width = width,
                            Height = height,
                            X = x,
                            Y = y,
                            RotationAngle = (int)angle,
                            MaxHP = ((NPC)entity).maxHP,
                            CurrentHP = ((NPC)entity).currentHP
                            //Weapons = 
                        };
                    }
                    else if (entity is DestroyableEntity)
                    {
                        gameElement = new GameElement
                        {
                            Class = "Destroyable Dynamic Circle",
                            Width = width,
                            Height = height,
                            X = x,
                            Y = y,
                            RotationAngle = (int)angle,
                            MaxHP = ((DestroyableEntity)entity).maxHP,
                            CurrentHP = ((DestroyableEntity)entity).currentHP
                            //Weapons = 
                        };
                    }
                    else if (entity is InterractiveItem)
                    {
                        gameElement = new GameElement
                        {
                            Class = "Interractive Item",
                            Width = width,
                            Height = height,
                            X = x,
                            Y = y,
                            RotationAngle = (int)angle,
                            EventID = ((InterractiveItem)entity).eventID,
                            //Weapons = 
                        };
                    }
                    





                    mapData.updatableData.Add(gameElement);
                }

            }

            // Update the save data
            saveData.mapInfo.maps[currentMapIndex] = mapData;
        }




        public void ExtractSaveInfoFromAllFiles()
        {
            string folderPath = GetFolderRelativePath();

            if (Directory.Exists(folderPath))
            {
                string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");

                foreach (string jsonFile in jsonFiles)
                {
                    try
                    {
                        JObject json = JObject.Parse(File.ReadAllText(jsonFile));
                        string saveDate = (string)json["saveDate"];
                        string inGameTime = (string)json["inGameTime"];
                        float gameProgress = (float)json["gameProgress"]["percentage"];
                        int currentMap = (int)json["mapInfo"]["currentMap"];

                        foreach (var map in json["mapInfo"]["maps"])
                        {
                            string mapName = (string)map["name"];
                            mapNames.Add(mapName);
                        }


                        saveDates.Add(saveDate);
                        inGameTimes.Add(inGameTime);
                        progresses.Add(gameProgress);
                        currentMaps.Add(currentMap);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error reading file {jsonFile}: {ex.Message}");
                    }
                }
            }
            else
            {
                Debug.WriteLine("Folder does not exist.");
            }
        }



        public int GetSavesAmount()
        {
            string folderPath = GetFolderRelativePath();
            if (Directory.Exists(folderPath))
            {
                return Directory.GetFiles(folderPath).Length;
            }
            else
            {
                Debug.WriteLine("Folder does not exist.");
                return 0;
            }
        }



        private string GetFolderRelativePath()
        {
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string relativePath = Path.Combine(projectDirectory, "res", "saves");
            return relativePath;
        }

        private string GetSaveFilePath(string saveName)
        {
            string folderPath = GetFolderRelativePath();
            string saveFilePath = Path.Combine(folderPath, saveName + ".json");
            return saveFilePath;
        }











        public void SaveCurrentDataToFile(string saveName)
        {
            try
            {
                string saveFilePath = GetSaveFilePath(saveName);
                string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
                File.WriteAllText(saveFilePath, json);
                Debug.WriteLine("Save data successfully written to file.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error writing save data to file: {ex.Message}");
            }
        }




    }
}
