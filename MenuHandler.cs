using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Platformer.Entities.group;
using Platformer.Graphics.Menu;
using Platformer.Utils;
using SharpDX.Direct3D11;
using static System.Net.Mime.MediaTypeNames;

namespace Platformer
{
    public class MenuHandler
    {

        private Game1 game1;

        public MenuHandler(Game1 game1) { this.game1 = game1; }



        public void handleSaveSlotClick(SaveSlot slot)
        {
            game1.aSetter.loadSave(slot.id-1);
            game1.aSetter.loadMap(game1.aSetter.mapReader.saveData.mapInfo.currentMap);
            game1.aSetter.InitializePlayState();
        }



        public void handleSaveSlotOnHover(SaveSlot slot)
        {

            slot.color = Color.LightGray;
            
        }

        public void handleSaveSlotUnHover(SaveSlot slot)
        {
            slot.color = slot.oldColor;
        }



        public void handleButtonClick(Button button)
        {
            switch (button.text)
            {
                case "Continue":

                    break;

                case "Load Game":
                    game1.uiManager.setLoadMenu();
                    break;


                case "Options":
                    game1.uiManager.setOptionsMenu(0);
                    break;
                case "General":
                    game1.uiManager.setOptionsMenu(0);
                    break;
                case "Graphics":
                    game1.uiManager.setOptionsMenu(1);
                    break;
                case "Video":
                    game1.uiManager.setOptionsMenu(2);
                    break;
                case "Sound":
                    game1.uiManager.setOptionsMenu(3);
                    break;
                case "Controls":
                    game1.uiManager.setOptionsMenu(4);
                    break;
                case "< Back":
                    game1.uiManager.setMainMenu();
                    break;
                case "Next":
                    game1.uiManager.setNextDialogue();
                    break;
                case "Exit":
                    game1.Exit();
                    break;




                default:
                    break;
            }
        }


        public void handleButtonHover(Button button)
        {
            button.color = Color.White;
        }


        public void handleButtonUnhover(Button button)
        {
            button.color = Color.Gray;
        }





        public void handleTextFieldSelected(ConsoleTextField consoleTextField)
        {
            List<Keys> pressedKeys = game1.keyboard.GetPressedKeys();

            foreach (Keys key in pressedKeys)
            {
                if (game1.keyboard.IsKeyClicked(key))
                {
                    if (key == Keys.Enter)
                    {


                        game1.dmanager.Execute(consoleTextField.text.ToString());
                        consoleTextField.text.Clear();
                        consoleTextField.text.Append(">");
                    }
                    else if (key == Keys.Back)
                    {
                        if (consoleTextField.text.Length > 1)
                        {
                            consoleTextField.text.Remove(consoleTextField.text.Length - 1, 1);
                        }
                    }


                    else if (key == Keys.Up)
                    {
                        if (consoleTextField.commandHistory.Count > 0)
                        {
                            if (consoleTextField.commandIndex == -1)
                                consoleTextField.commandIndex = consoleTextField.commandHistory.Count - 1;
                            else if (consoleTextField.commandIndex > 0)
                                consoleTextField.commandIndex--;

                            consoleTextField.text.Clear();
                            consoleTextField.text.Append(">");

                            if (consoleTextField.commandIndex >= 0)
                            {
                                string currentCommand = consoleTextField.commandHistory[consoleTextField.commandIndex];
                                int dashIndex = currentCommand.IndexOf("-");

                                if (dashIndex != -1)
                                {
                                    string substring = currentCommand.Substring(1, dashIndex - 2);
                                    consoleTextField.text.Append(substring);
                                }
                            }
                        }
                    }
                    else if (key == Keys.Down)
                    {
                        if (consoleTextField.commandHistory.Count > 0)
                        {
                            if (consoleTextField.commandIndex == consoleTextField.commandHistory.Count - 1)
                                consoleTextField.commandIndex = -1;
                            else if (consoleTextField.commandIndex < consoleTextField.commandHistory.Count - 1)
                                consoleTextField.commandIndex++;

                            consoleTextField.text.Clear();
                            consoleTextField.text.Append(">");

                            if (consoleTextField.commandIndex >= 0)
                            {
                                string currentCommand = consoleTextField.commandHistory[consoleTextField.commandIndex];
                                int dashIndex = currentCommand.IndexOf("-");

                                if (dashIndex != -1)
                                {
                                    string substring = currentCommand.Substring(1, dashIndex - 2);
                                    consoleTextField.text.Append(substring);
                                }
                            }
                        }
                    }


                    // Handle specific keys
                    else if (key == Keys.OemQuestion) // Handle '/'
                    {
                        consoleTextField.text.Append('/');
                    }
                    else if (key == Keys.Escape)
                    {
                        consoleTextField.isSelected = false;
                    }

                    else
                    {
                        char character = KeyboardHelper.GetCharFromKey(key, game1.keyboard.IsKeyDown(Keys.LeftShift) || game1.keyboard.IsKeyDown(Keys.RightShift));
                        if (character != '\0')
                        {
                            consoleTextField.text.Append(character);
                        }
                    }
                }
            }
        }




        public void handleAnyClick()
        {
            game1.uiManager.setMainMenu();
        }



    }


}
