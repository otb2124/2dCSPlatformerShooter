using Microsoft.Xna.Framework.Input;

namespace Platformer.Graphics.Menu
{
    public static class KeyboardHelper
    {
        public static char GetCharFromKey(Keys key, bool shiftPressed)
        {
            switch (key)
            {
                // Alphabet keys
                case Keys.A: return shiftPressed ? 'A' : 'a';
                case Keys.B: return shiftPressed ? 'B' : 'b';
                case Keys.C: return shiftPressed ? 'C' : 'c';
                case Keys.D: return shiftPressed ? 'D' : 'd';
                case Keys.E: return shiftPressed ? 'E' : 'e';
                case Keys.F: return shiftPressed ? 'F' : 'f';
                case Keys.G: return shiftPressed ? 'G' : 'g';
                case Keys.H: return shiftPressed ? 'H' : 'h';
                case Keys.I: return shiftPressed ? 'I' : 'i';
                case Keys.J: return shiftPressed ? 'J' : 'j';
                case Keys.K: return shiftPressed ? 'K' : 'k';
                case Keys.L: return shiftPressed ? 'L' : 'l';
                case Keys.M: return shiftPressed ? 'M' : 'm';
                case Keys.N: return shiftPressed ? 'N' : 'n';
                case Keys.O: return shiftPressed ? 'O' : 'o';
                case Keys.P: return shiftPressed ? 'P' : 'p';
                case Keys.Q: return shiftPressed ? 'Q' : 'q';
                case Keys.R: return shiftPressed ? 'R' : 'r';
                case Keys.S: return shiftPressed ? 'S' : 's';
                case Keys.T: return shiftPressed ? 'T' : 't';
                case Keys.U: return shiftPressed ? 'U' : 'u';
                case Keys.V: return shiftPressed ? 'V' : 'v';
                case Keys.W: return shiftPressed ? 'W' : 'w';
                case Keys.X: return shiftPressed ? 'X' : 'x';
                case Keys.Y: return shiftPressed ? 'Y' : 'y';
                case Keys.Z: return shiftPressed ? 'Z' : 'z';

                // Number keys
                case Keys.D0: return shiftPressed ? ')' : '0';
                case Keys.D1: return shiftPressed ? '!' : '1';
                case Keys.D2: return shiftPressed ? '@' : '2';
                case Keys.D3: return shiftPressed ? '#' : '3';
                case Keys.D4: return shiftPressed ? '$' : '4';
                case Keys.D5: return shiftPressed ? '%' : '5';
                case Keys.D6: return shiftPressed ? '^' : '6';
                case Keys.D7: return shiftPressed ? '&' : '7';
                case Keys.D8: return shiftPressed ? '*' : '8';
                case Keys.D9: return shiftPressed ? '(' : '9';

                // Symbol keys
                case Keys.OemTilde: return shiftPressed ? '~' : '`';
                case Keys.OemSemicolon: return shiftPressed ? ':' : ';';
                case Keys.OemQuotes: return shiftPressed ? '"' : '\'';
                case Keys.OemOpenBrackets: return shiftPressed ? '{' : '[';
                case Keys.OemCloseBrackets: return shiftPressed ? '}' : ']';
                case Keys.OemMinus: return shiftPressed ? '_' : '-';
                case Keys.OemComma: return shiftPressed ? '<' : ',';
                case Keys.Space: return ' ';
                case Keys.Enter: return '\n';
                case Keys.Tab: return '\t';
                case Keys.Back: return (char)8; // Backspace
                case Keys.Delete: return (char)127; // Delete

                // Ignore other keys
                default: return '\0';
            }
        }
    }
}
