using System.Collections.Generic;
using System.Drawing;





namespace Platformer.Entities.Types
{

    public class GameElement
    {

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Class { get; set; }
        public Color Color { get; set; }
        public int RotationAngle { get; set; }
        public bool IsStatic { get; set; }
        public bool IsCollidableWithLive { get; set; }


        public int Level { get; set; }
        public string MobType { get; set; }
        public int AgroRange { get; set; }
        public int UnAgroRange { get; set; }
        public List<SaveWeapon> Weapons { get; set; }
        public List<SaveInventoryItem> Inventory { get; set; }
        public float MaxHP { get; set; }
        public float CurrentHP { get; set; }
        public float Armor { get; set; }
        public float Speed { get; set; }
        public float JumpForce { get; set; }
        public float FlyForce { get; set; }
        public float BodyDamage { get; set; }
        public float KnockbackStrength { get; set; }


        public int EventID { get; set; }


        public string Name { get; set; }
        public string GroupMemberClass { get; set; }
        public bool IsPlayer { get; set; }


        public bool isRound { get; set; }

    }


    public class SaveData
    {
        public string saveDate { get; set; }
        public string inGameTime { get; set; }
        public GameProgress gameProgress { get; set; }
        public MapInfo mapInfo { get; set; }
    }

    public class GameProgress
    {
        public float percentage { get; set; }
    }

    public class MapInfo
    {
        public int currentMap { get; set; }
        public List<MapData> maps { get; set; }
    }

    public class MapData
    {
        public string name { get; set; }
        public List<GameElement> updatableData { get; set; }
        public List<GameElement> setData { get; set; }
    }

    public class SaveWeapon
    {
        public string Name { get; set; }
        public float Damage { get; set; }
        public float BulletSpeed { get; set; }
        public float AmmotAmount { get; set; }
        public float CurrentAmmoAmoint { get; set; }
        public bool IsReloading { get; set; }
        public float BulletDelay { get; set; }
        public float BulletLifeCount { get; set; }
        public int ReloadSpeed { get; set; }
        public float Spray { get; set; }
    }


    public class SaveInventoryItem
    {

        public float Value { get; set; }
        public bool IsStackable { get; set; }
        public int Amount { get; set; }
        public string Name { get; set; }
    }



}