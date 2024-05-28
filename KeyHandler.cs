using Flat.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Platformer.Physics;
using Platformer.Utils;
using System.Diagnostics;
using Platformer.Entities.Types;

namespace Platformer
{
    internal class KeyHandler
    {


        Game1 game;
        public int SilenceCounter;


        public KeyHandler(Game1 game) {

            this.game = game;
        }



        public void Update()
        {


            if (game.gameState == game.PLAYSTATE)
            {


                //shooting
                if (game.mouse.IsLeftMouseButtonDown())
                {
                    game.player.isShooting = true;
                    game.player.SecondpWeapon = false;
                }
                else if (game.mouse.IsRightMouseButtonDown())
                {
                    game.player.isShooting = true;
                    game.player.SecondpWeapon = true;
                }
                else
                {
                    game.player.isShooting = false;
                }



                //crouch
                if (game.keyboard.IsKeyDown(Keys.LeftControl))
                {
                    game.player.isCrouching = true;
                }
                else
                {
                    game.player.isCrouching = false;
                }

                //sprint
                if (game.keyboard.IsKeyDown(Keys.LeftShift))
                {
                    game.player.isSprinting = true;
                }
                else
                {
                    game.player.isSprinting = false;
                }



                //silent
                if (isAnyPressed())
                {
                    SilenceCounter = 0;
                }

                if (SilenceCounter > 60 * 10)
                {
                    game.gameState = game.SILENTSTATE;
                }




                //ladder
                if (game.player.isOnLadder)
                {

                    if (game.keyboard.IsKeyDown(Keys.W))
                    {
                        game.player.Body.Move(new FlatVector(0, 0.1f));
                        game.player.Body.IsStatic = true;
                    }
                    else if (game.keyboard.IsKeyDown(Keys.S))
                    {
                        game.player.Body.Move(new FlatVector(0, -0.1f));
                        game.player.Body.IsStatic = true;
                    }
                }
                else
                {
                    game.player.Body.IsStatic = false;
                }


                //platform descend
                if (game.player.isOnPlatform)
                {

                    for (int i = 0; i < game.entities.Count; i++)
                    {

                        if (game.entities[i] is PlatformEntity platform)
                        {
                            if (platform.Interraction)
                            {

                                if (game.keyboard.IsKeyDown(Keys.S))
                                {
                                    platform.isCollidable = false;
                                }

                            }
                        }
                    }


                }







                if (game.keyboard.IsKeyAvailable)
                {



                    if (game.keyboard.IsKeyDown(Keys.A))
                    {
                        // Move the player left
                        game.player.Body.Move(new FlatVector(-0.1f * game.player.currentSpeed, 0f));
                    }
                    if (game.keyboard.IsKeyDown(Keys.D))
                    {
                        // Move the player right
                        game.player.Body.Move(new FlatVector(0.1f * game.player.currentSpeed, 0f));
                    }
                    if (game.keyboard.IsKeyClicked(Keys.Space))
                    {
                        game.player.Body.Jump(game.player.currentJumpForce);
                    }





                    if (game.player.isInterracting != null)
                    {

                        if (game.keyboard.IsKeyClicked(Keys.Enter))
                        {
                            if (game.player.isInterracting is NPC npc)
                            {
                                if (npc.interractions[npc.interractionID] == "Talk")
                                {
                                    game.gameState = game.DIALOGUESTATE;
                                }
                                
                            }
                            else if (game.player.isInterracting is InterractiveItem ii)
                            {
                                game.eventHandler.handleEvent(ii.eventID);
                            }

                        }
                        else if (game.keyboard.IsKeyClicked(Keys.Right))
                        {
                            if (game.player.isInterracting is NPC npc)
                            {
                                npc.interractionID++;
                                if (npc.interractionID >= npc.interractions.Length)
                                {
                                    npc.interractionID = 0;
                                }
                            }
                            else if (game.player.isInterracting is InterractiveItem ii)
                            {
                                ii.interractionID++;
                                if (ii.interractionID >= ii.interractions.Length)
                                {
                                    ii.interractionID = 0;
                                }
                            }
                            game.uiManager.clearInterraction();
                            game.uiManager.interractionShown = false;
                        }
                        else if (game.keyboard.IsKeyClicked(Keys.Left))
                        {
                            if (game.player.isInterracting is NPC npc)
                            {
                                npc.interractionID--;
                                if (npc.interractionID < 0)
                                {
                                    npc.interractionID = npc.interractions.Length - 1;
                                }
                            }
                            else if (game.player.isInterracting is InterractiveItem ii)
                            {
                                ii.interractionID--;
                                if (ii.interractionID < 0)
                                {
                                    ii.interractionID = ii.interractions.Length - 1;
                                }
                            }
                            game.uiManager.clearInterraction();
                            game.uiManager.interractionShown = false;
                        }
                    
                


                }


                    






                    if (game.keyboard.IsKeyDown(Keys.OemPlus))
                    {
                       game.camera.IncZoom();
                        Debug.WriteLine(game.camera.Zoom);
                    }
                    if (game.keyboard.IsKeyDown(Keys.OemMinus))
                    {
                       game.camera.DecZoom();
                        Debug.WriteLine(game.camera.Zoom);
                    }


                    if (game.keyboard.IsKeyDown(Keys.Right))
                    {
                        game.camera.MoveRight(1f);
                    }

                    if (game.keyboard.IsKeyDown(Keys.Left))
                    {
                        game.camera.MoveLeft(1f);
                    }

                    if (game.keyboard.IsKeyDown(Keys.Up))
                    {
                        game.camera.MoveUp(1f);
                    }

                    if (game.keyboard.IsKeyDown(Keys.Down))
                    {
                        game.camera.MoveDown(1f);
                    }



                    if (game.keyboard.IsKeyClicked(Keys.OemTilde))
                    {

                        if (game.gameMode == game.DEBUGMODE)
                        {
                            game.dmanager.stop();
                        }
                        else
                        {
                            game.dmanager.start();
                        }

                    }

                    if (game.keyboard.IsKeyClicked(Keys.Escape))
                    {
                        game.uiManager.setGameMenuBar();
                        game.gameState = game.GAMEMENUSTATE;
                    }


                }




                SilenceCounter++;
            }
            else if(game.gameState == game.GAMEMENUSTATE)
            {


                if (game.keyboard.IsKeyClicked(Keys.Escape))
                {
                    game.uiManager.hideGameMenuAndGameMenuBar();
                    game.gameState = game.PLAYSTATE;
                }
            }
            else if (game.gameState == game.DIALOGUESTATE)
            {
                if (game.keyboard.IsKeyAvailable)
                {
                    if (game.keyboard.IsKeyClicked(Keys.Escape))
                    {
                        game.uiManager.clearDialogues();
                        game.gameState = game.PLAYSTATE;
                    }
                }
            }
            else if(game.gameState == game.SILENTSTATE)
            {
                game.uiManager.hideHUD();
                if (isAnyPressed() || game.mouse.IsMouseMoved())
                {
                    game.uiManager.HUDisOn = false;
                    game.gameState = game.PLAYSTATE;
                }
            }



            

        }


        private bool isAnyPressed()
        {
            List<Keys> pressedKeys = game.keyboard.GetPressedKeys();
            List<MouseButtons> pressedButtons = game.mouse.GetPressedButtons();

            if ((pressedKeys.Count + pressedButtons.Count) > 0)
            {
                return true;
            }


            return false;
        }
    }
}
