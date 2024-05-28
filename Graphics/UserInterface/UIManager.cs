using Microsoft.Xna.Framework;
using Platformer.Entities.group;
using Platformer.Entities.Types;
using Platformer.Graphics.Generators;
using Platformer.Graphics.Menu;
using Platformer.Utils;
using SharpDX.Direct3D9;
using SharpDX.MediaFoundation;
using SharpDX.Multimedia;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Numerics;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;


namespace Platformer.Graphics.UserInterface
{
    public class UIManager
    {
        private FontManager fontManager;
        private MenuHandler menuHandler;
        private Game1 game;

        private float[] extends = new float[4];

        public List<UIComponent> uiList;

        public bool HUDisOn = false;
        public bool DialogShown = false;
        public bool interractionShown = false;

        public UIManager(Game1 game)
        {
            uiList = new List<UIComponent>();

            fontManager = new FontManager(game);
            menuHandler = new MenuHandler(game);
            this.game = game;


            setPressAnyKeyMenu();

        }




        public void setPressAnyKeyMenu()
        {
            uiList.Clear();
            AnyKeyCheker anyKeyChecker = new AnyKeyCheker("Press Any Key", fontManager.fonts[1], new Vector2(-7, 0), 0.15f, Color.White, menuHandler);
            uiList.Add(anyKeyChecker);
        }




        public void setMainMenu()
        {
            uiList.Clear();



            float x = -30;
            float y = 1;
            float yDif = 3;
            float scale = 0.15f;
            Color color = Color.Gray;

            Button btn = new Button("Continue", fontManager.fonts[1], new Vector2(x, (0 + y) * yDif), scale, color, menuHandler);
            uiList.Add(btn);
            btn = new Button("New Game", fontManager.fonts[1], new Vector2(x, (-1 + y) * yDif), scale, color, menuHandler);
            uiList.Add(btn);
            btn = new Button("Load Game", fontManager.fonts[1], new Vector2(x, (-2 + y) * yDif), scale, color, menuHandler);
            uiList.Add(btn);
            btn = new Button("Options", fontManager.fonts[1], new Vector2(x, (-3 + y) * yDif), scale, color, menuHandler);
            uiList.Add(btn);
            btn = new Button("Exit", fontManager.fonts[1], new Vector2(x, (-5 + y) * yDif), scale, color, menuHandler);
            uiList.Add(btn);
        }


        public void setLoadMenu()
        {
            uiList.Clear();

            Color color = Color.Gray;

            float panelWidth = 30f; // Width of the title panel
            float panelHeight = 35f;


            ScrollPane tpan = new ScrollPane("Load Game", fontManager.fonts[1], panelWidth, panelHeight);

            float starty = 5;
            float dif = -12;


            for (int i = 0; i < game.aSetter.mapReader.GetSavesAmount(); i++)
            {
                SaveSlot slot1 = new SaveSlot(i+1, game.aSetter.mapReader.mapNames[game.aSetter.mapReader.currentMaps[i]], game.aSetter.mapReader.progresses[i].ToString(), game.aSetter.mapReader.inGameTimes[i], game.aSetter.mapReader.saveDates[i], fontManager.fonts[1], new Vector2(0, starty + dif), 0.1f, Color.Gray, menuHandler);
                tpan.add(slot1);

                dif += -12;
            }

            uiList.Add(tpan);

            //back bttn
            Button btn = new Button("< Back", fontManager.fonts[1], new Vector2(-32, 15), 0.1f, color, menuHandler);
            uiList.Add(btn);


        }




        public void setOptionsMenu(int id)
        {

            uiList.Clear();



            Color color = Color.Gray;



            //back bttn
            Button btn = new Button("< Back", fontManager.fonts[1], new Vector2(-32, 15), 0.1f, color, menuHandler);
            uiList.Add(btn);

            //topbar
            float x = -24;
            float y = 14.75f;
            float xDif = 10;
            float scale = 0.125f;

            btn = new Button("General", fontManager.fonts[1], new Vector2(x + 0 * xDif, y), scale, color, menuHandler);
            uiList.Add(btn);
            btn = new Button("Graphics", fontManager.fonts[1], new Vector2(x + 1 * xDif, y), scale, color, menuHandler);
            uiList.Add(btn);
            btn = new Button("Video", fontManager.fonts[1], new Vector2(x + 1 + 2 * xDif, y), scale, color, menuHandler);
            uiList.Add(btn);
            btn = new Button("Sound", fontManager.fonts[1], new Vector2(x + 0.5f + 3 * xDif, y), scale, color, menuHandler);
            uiList.Add(btn);
            btn = new Button("Controls", fontManager.fonts[1], new Vector2(x + 0.5f + 4 * xDif, y), scale, color, menuHandler);
            uiList.Add(btn);


            //main
            x = -10;
            y = 0;
            float yDif = 3;
            scale = 0.1f;
            color = Color.White;


            switch (id)
            {


                //General
                case 0:

                    //left
                    Label label = new Label("Attribute 1", fontManager.fonts[1], new Vector2(x, y + 0 * yDif), scale, color);
                    uiList.Add(label);
                    label = new Label("Attribute 2", fontManager.fonts[1], new Vector2(x, y + 1 * yDif), scale, color);
                    uiList.Add(label);
                    label = new Label("Attribute 3", fontManager.fonts[1], new Vector2(x, y + 2 * yDif), scale, color);
                    uiList.Add(label);
                    label = new Label("Attribute 4", fontManager.fonts[1], new Vector2(x, y + 3 * yDif), scale, color);
                    uiList.Add(label);



                    break;




                //Graphics
                case 1:

                    //left
                    Label label1 = new Label("Attribute 1", fontManager.fonts[1], new Vector2(x, y + 0 * yDif), scale, color);
                    uiList.Add(label1);
                    label1 = new Label("Attribute 3", fontManager.fonts[1], new Vector2(x, y + 2 * yDif), scale, color);
                    uiList.Add(label1);
                    label1 = new Label("Attribute 4", fontManager.fonts[1], new Vector2(x, y + 3 * yDif), scale, color);
                    uiList.Add(label1);


                    break;

                case 2:

                    //left
                    Label label2 = new Label("Attribute 1", fontManager.fonts[1], new Vector2(x, y + 0 * yDif), scale, color);
                    uiList.Add(label2);
                    label2 = new Label("Attribute 2", fontManager.fonts[1], new Vector2(x, y + 1 * yDif), scale, color);
                    uiList.Add(label2);
                    label2 = new Label("Attribute 3", fontManager.fonts[1], new Vector2(x, y + 2 * yDif), scale, color);
                    uiList.Add(label2);





                    break;

                case 3:
                    break;

                case 4:
                    break;

                case 5:
                    break;




            }


        }



        public void setGameMenuBar()
        {
            float x = 0;
            float y = 18;
            float width = 30f;
            float height = 3f;

            GameMenuBar gbar = new GameMenuBar(new Vector2(x, y), width, height, Color.Gray);
            uiList.Add(gbar);

        }




        public void hideGameMenuAndGameMenuBar()
        {
            for (int i = 0; i < uiList.Count; i++)
            {

                if (uiList[i] is GameMenuBar)
                {
                    uiList.Remove(uiList[i]);
                }

            }

            hideGameMenu();
        }

        public void hideGameMenu()
        {
            for (int i = 0; i < uiList.Count; i++)
            {
                if (uiList[i] is TitlePanel)
                {
                    uiList.Remove(uiList[i]);
                }
            }
        }


        public void setGameMenu(int id)
        {
            float xOffset = game.camera.Position.X;
            float yOffset = game.camera.Position.Y - 10;
            float width = 30f;
            float height = 35f;

            TitlePanel tpanel = null;

            switch (id)
            {
                case 1:

                    float panelWidth = 30f; // Width of the title panel
                    float panelHeight = 35f;

                    // Create the title panel
                    tpanel = new TitlePanel("Inventory", fontManager.fonts[1], panelWidth, panelHeight, new Color(0.7f, 0.7f, 0.7f, 0.1f));


                    float inventoryWidth = 10f;
                    float inventoryHeight = 10; // Height of the inventory panel (adjust as needed)


                    foreach (GroupMember member in game.group.CurrentGroup)
                    {
                        float inventoryX = member.Body.Position.X;
                        float inventoryY = member.Body.Position.Y;

                        Inventory inventory = new Inventory(member, fontManager.fonts[1], new Vector2(inventoryX-2f, inventoryY), inventoryWidth, inventoryHeight, Color.Gray);
                        tpanel.add(inventory);
                    }


                    break;
                case 2:
                    tpanel = new TitlePanel("Group", fontManager.fonts[1], width, height, new Color(0.7f, 0.7f, 0.7f, 0.1f));
                    break;
                case 3:
                    tpanel = new TitlePanel("Statistics", fontManager.fonts[1], width, height, new Color(0.7f, 0.7f, 0.7f, 0.1f));
                    break;
                case 4:
                    tpanel = new TitlePanel("QuestBook", fontManager.fonts[1], width, height, new Color(0.7f, 0.7f, 0.7f, 0.1f));
                    break;
                case 5:
                    tpanel = new TitlePanel("Settings", fontManager.fonts[1], width, height, new Color(0.7f, 0.7f, 0.7f, 0.1f));
                    break;
                case 6:
                    tpanel = new TitlePanel("Exit", fontManager.fonts[1], width, height, new Color(0.7f, 0.7f, 0.7f, 0.1f));
                    break;


                case 0:
                    break;
            }

            if (tpanel != null)
            {
                hideGameMenu();
                uiList.Add(tpanel);
            }
        }



        public void setHUD()
        {
            setPlayerBars();
            setLiveEntitiesBars();
            HUDisOn = true;
        }


        public void refresh()
        {
            //hideHUD();
            uiList.Clear();
            HUDisOn = false;
            interractionShown = false;
        }

        public void setPlayerBars()
        {
            float x = -20;
            float y = 18;

            Bar healthbar = new Bar(new Vector2(x, y), 10f, 1f, 0, Color.Red, true, game.player);

            x = 20;

            Bar ammobar = new Bar(new Vector2(x, y), 10f, 1f, 1, Color.Yellow, true, game.player);

            uiList.Add(healthbar);
            uiList.Add(ammobar);

        }


        public void setLiveEntitiesBars()
        {

            float x = 0;
            float y = 0;


            for (int i = 0; i < game.entities.Count; i++)
            {
                if (game.entities[i] is LiveEntity lent)
                {
                    if(lent is GroupMember member && member.isPlayer)
                    {
                        continue;
                    }
                    x = lent.Body.Position.X;
                    y = lent.Body.Position.Y + lent.Body.Height;
                    Bar healthbar = new Bar(new Vector2(x, y), 2f, 0.2f, 0, Color.Red, false, lent);
                    uiList.Add(healthbar);
                }


            }
        }


        public void hideHUD()
        {
            for (int i = 0; i < uiList.Count; i++)
            {
                if (uiList[i] is Bar)
                {
                    uiList.Remove(uiList[i]);
                }
            }
        }



        public void setDialogues()
        {


            float width = extends[1] - extends[0];
            float height = (extends[3] - extends[2]) / 3;
            float padding = width * 0.1f;
            width -= padding;
            height -= padding;


            DialogueBox dialogueBox = new DialogueBox(new Vector2(0, 0), width, height, fontManager.fonts[1], (NPC)game.player.isInterracting, game.aSetter);


            float x = 18f;
            float y = -15f;


            Button next = new Button("Next", fontManager.fonts[1], new Vector2(x, y), 0.075f, Color.White, menuHandler);

            // Add the Next Button to the DialogueBox
            dialogueBox.add(next);

            // Add the DialogueBox to the UI list
            uiList.Add(dialogueBox);
            DialogShown = true;
        }



        public void setNextDialogue()
        {
            for (int i = 0; i < uiList.Count; i++)
            {

                if (uiList[i] is DialogueBox box)
                {
                    if (game.player.isInterracting != null)
                    {
                        if (game.player.isInterracting is NPC npc)
                        {

                            if (npc.currentDialogueId < npc.dialogueIds.Length - 1)
                            {
                                box.npc.currentDialogueId++;
                                game.uiManager.clearDialogues();
                                game.uiManager.setDialogues();
                                break;
                            }
                            else
                            {
                                game.uiManager.clearDialogues();
                                game.gameState = game.PLAYSTATE;
                            }

                        }
                    }



                }


            }
        }




        public void clearDialogues()
        {
            for (int i = 0; i < uiList.Count; i++)
            {

                if (uiList[i] is DialogueBox)
                {
                    uiList.Remove(uiList[i]);
                    DialogShown = false;
                    break;
                }


            }
        }

        public void clearInterraction()
        {
            for (int i = 0; i < uiList.Count; i++)
            {

                if (uiList[i] is InterractionBox)
                {
                    uiList.Remove(uiList[i]);
                    interractionShown = false;
                    break;
                }


            }
        }



        public void setInterraction()
        {
            float x = 0;
            float y = 0;

            string text = "";

            if(game.player.isInterracting is NPC npc)
            {
                text = npc.interractions[npc.interractionID];
            }
            else if(game.player.isInterracting is InterractiveItem ii)
            {

                if (ii.eventID == 0)
                {
                    text = game.aSetter.mapReader.mapNames[game.aSetter.currentMap + 1];
                }
                if (ii.eventID == 1)
                {
                    text = game.aSetter.mapReader.mapNames[game.aSetter.currentMap - 1];
                }
                if (ii.eventID == 2)
                {
                    text = "Save";
                }

            }

            InterractionBox ibox = new InterractionBox(text, fontManager.fonts[1], Color.White);
            uiList.Add(ibox);
            interractionShown = true;
        }




        public void setDebugConsoleWindow()
        {
            float x = 0;
            float y = 0;
            float yDif = 1.5f;
            float scale = 0.05f;
            Color color = Color.White;

            ConsoleTextField console = new ConsoleTextField(fontManager.fonts[1], new Vector2(x, y), scale, color, menuHandler);
            uiList.Add(console);
        }



        public void clerDebugConsoleWindow()
        {
            for (int i = 0; i < uiList.Count; i++)
            {

                if (uiList[i] is ConsoleTextField)
                {
                    uiList.Remove(uiList[i]);
                    break;
                }


            }
        }






        public void Update()
        {

            game.camera.GetExtents(out float left, out float right, out float bottom, out float top);

            extends[0] = left;
            extends[1] = right;
            extends[2] = bottom;
            extends[3] = top;

            if (game.gameState != game.MENUSTATE)
            {
                if (!HUDisOn)
                {
                    setHUD();
                }


                if (!interractionShown)
                {
                    if (game.player.isInterracting != null)
                    {
                        setInterraction();
                    }
                }
                else
                {
                    if (game.player.isInterracting == null)
                    {
                        clearInterraction();
                    }
                }


            }

            if (game.gameState == game.DIALOGUESTATE && !DialogShown)
            {
                setDialogues();
            }



            for (int i = 0; i < uiList.Count; i++)
            {



                uiList[i].position = new Vector2(game.camera.Position.X + uiList[i].offset.X, game.camera.Position.Y + uiList[i].offset.Y);



                if (!(uiList[i] is InterractionBox) || !(uiList[i] is Bar bar && bar.isPlayer))
                {
                    uiList[i].scaledHeight = uiList[i].height * (20.0f / game.camera.Zoom);
                    uiList[i].scaledwidth = uiList[i].width * (20.0f / game.camera.Zoom);
                    uiList[i].scalePosition = new Vector2(uiList[i].position.X, uiList[i].position.Y);
                }


                uiList[i].Update(game);

            }



        }



        public void Draw(Sprites sprites, Shapes shapes)
        {

            for (int i = 0; i < uiList.Count; i++)
            {
                uiList[i].Draw(sprites, shapes, game.camera);
            }
        }
    }
}
