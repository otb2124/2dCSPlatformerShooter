using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.Physics;
using Flat.Input;
using Platformer.Entities.group;
using Platformer.Graphics.UserInterface;
using Platformer.Entities.Types;
using Platformer.Graphics.Generators;
using Platformer.Entities.Weapons;



namespace Platformer
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        public Screen screen;
        public int ResolutionX = 1280, ResolutionY = 768;
        public Sprites sprites;
        private Shapes shapes;
        public Camera camera;

        public FlatKeyboard keyboard;
        public FlatMouse mouse;


        public FlatWorld world;
        public List<FlatEntity> entities;
        public List<FlatEntity> backEntities;
        public List<FlatEntity> entitiesToRemoval;
        public Stopwatch watch;

        private double totalWorldStepTIme = 0d;
        private int totalBodyCount = 0;
        private int totalSampleCount = 0;
        private Stopwatch sampleTimer = new Stopwatch();

        private string worldStepTimeString = string.Empty;
        private string bodyCountString = string.Empty;

        private KeyHandler kh;
        public AssetSetter aSetter;
        public UIManager uiManager;
        public DebugManager dmanager;
        public StatsManager statsManager;
        public AIManager aiManager;
        public EventManager eventHandler;

        public GroupMember player;
        public Group group;

        public int gameState;
        public readonly int PLAYSTATE = 0, MENUSTATE = 1, GAMEMENUSTATE = 2, DIALOGUESTATE = 3, SILENTSTATE = 4;

        public int gameMode;
        public readonly int PLAYMODE = 0, DEBUGMODE = 1;


        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.graphics.SynchronizeWithVerticalRetrace = true;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = true;

            const double UpdatesPerSecond = 60d;
            this.TargetElapsedTime = TimeSpan.FromTicks((long)Math.Round((double)TimeSpan.TicksPerSecond / UpdatesPerSecond));
        }

        protected override void Initialize()
        {
            gameState = MENUSTATE;

            this.Window.Position = new Point(10, 40);

            FlatUtil.SetRelativeBackBufferSize(this.graphics, 0.85f);

            this.screen = new Screen(this, 1280, 768);
            this.sprites = new Sprites(this);
            this.shapes = new Shapes(this);


            this.entities = new List<FlatEntity>();
            this.backEntities = new List<FlatEntity>();
            this.entitiesToRemoval = new List<FlatEntity>();
            this.world = new FlatWorld();


            //my classes
            this.kh = new KeyHandler(this);
            this.aSetter = new AssetSetter(this);
            this.uiManager = new UIManager(this);
            this.dmanager = new DebugManager(this);
            this.statsManager = new StatsManager(this);
            this.aiManager = new AIManager(this);
            this.eventHandler = new EventManager(this);



            //set assets
            aSetter.setAll();

            




            //camera
            this.camera = new Camera(this.screen, this);
            this.camera.Zoom = 20;




            this.watch = new Stopwatch();
            this.sampleTimer.Start();

            //FlatUtil.ToggleFullScreen(graphics);


            base.Initialize();
        }


        protected override void Update(GameTime gameTime)
        {

            //updates
            keyboard = FlatKeyboard.Instance;
            mouse = FlatMouse.Instance;
            keyboard.Update();
            mouse.Update();

            kh.Update();



            //world step
            FlatWorld.TransformCount = 0;
            FlatWorld.NoTransformCount = 0;

            this.watch.Restart();

            //physics
            if (gameState == PLAYSTATE || gameState == DIALOGUESTATE || gameState == GAMEMENUSTATE || gameState == SILENTSTATE)
            {
                this.world.Step(FlatUtil.GetElapsedTimeInSeconds(gameTime), 20);
            }
            this.watch.Stop();




            this.totalWorldStepTIme += this.watch.Elapsed.TotalMilliseconds;

            this.totalBodyCount += this.world.BodyCount;

            this.totalSampleCount++;


            this.entitiesToRemoval.Clear();












            if (gameState == MENUSTATE)
            {

            }
            else
            {

                this.group.Update(this);
                this.player.Update(this);

                this.camera.Update();

                this.eventHandler.Update(this);
                

                for (int i = 0; i < entities.Count; i++)
                {




                    if (entities[i] is LiveEntity)
                    {
                        if (entities[i] is Mob mob)
                        {
                            mob.Update(this);
                        }

                        if (entities[i] is NPC npc)
                        {
                            npc.Update(this);
                        }



                    }
                    else if (entities[i] is Projectile proj)
                    {
                        proj.Update(this);
                        statsManager.HandleProjectileLogic(proj);
                    }
                    else if (entities[i] is InterractiveItem interractiveItem)
                    {
                        interractiveItem.Update(this);
                    }
                    else if (entities[i] is DestroyableEntity destroyableEntity)
                    {
                        destroyableEntity.Update(this);
                    }
                    else if (entities[i] is LadderEntity ladderEntity)
                    {
                        ladderEntity.Update(this);
                    }
                    else if (entities[i] is PlatformEntity platformEntity)
                    {
                        platformEntity.Update(this);
                    }


                    statsManager.Update(entities[i]);



                    if (entities[i] is DynamicEntity)
                    {
                        //fall limit
                        if (entities[i].Body.Position.Y < -30)
                        {
                            entities.RemoveAt(i);
                            i--; // Decrement i to avoid skipping the next entity after removal
                        }
                    }



                    if (entities[i] != null && !backEntities.Contains(entities[i]))
                    {
                        if (entities[i] is DecorationEntity || entities[i] is InterractiveItem || entities[i] is LadderEntity)
                        {
                            this.backEntities.Add(entities[i]);
                        }
                    }
                    

                }




                



                //removal
                for (int i = 0; i < this.entitiesToRemoval.Count; i++)
                {
                    FlatEntity entity = this.entitiesToRemoval[i];
                    this.world.RemoveBody(entity.Body);
                    this.entities.Remove(entity);
                    this.backEntities.Remove(entity);
                }
            }


            uiManager.Update();

            base.Update(gameTime);
        }









        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.White);



            this.screen.Set();
            this.shapes.Begin(this.camera);

            if (gameState == PLAYSTATE || gameState == GAMEMENUSTATE || gameState == DIALOGUESTATE || gameState == SILENTSTATE)
            {


                for (int i = 0; i < backEntities.Count; i++)
                {
                    if (backEntities[i] != null)
                    {

                        this.backEntities[i].Draw(this.shapes);



                    }
                }

                for (int i = 0; i < this.entities.Count; i++)
                {
                    if (entities[i] != null)
                    {

                        if (!backEntities.Contains(entities[i]))
                        {
                            this.entities[i].Draw(this.shapes);
                        }
                        
                        

                    }

                }

            }


            this.sprites.Begin(this.camera);


            this.uiManager.Draw(this.sprites, this.shapes);


            this.shapes.End();
            this.sprites.End();



            this.screen.Unset();
            this.screen.Present(this.sprites);

            base.Draw(gameTime);
        }





    }
}
