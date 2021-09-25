using System.Windows.Forms;

namespace SharpScripter.Helpers
{
    public static class KeyConvert
    {
        public static string Convert(Keys key)
        {
            string text;
            switch (key)
            {
                #region CONTROL KEYS
                case Keys.OemQuestion:
                    text = "Ö";
                    break;

                case Keys.Oem7:
                    text = "İ";
                    break;

                case Keys.Oem6:
                    text = "Ü";
                    break;

                case Keys.Oem5:
                    text = "Ç";
                    break;

                case Keys.Oem1:
                    text = "Ş";
                    break;

                case Keys.OemOpenBrackets:
                    text = "Ğ";
                    break;

                case Keys.Enter:
                    text = "{ENTER}";
                    break;

                case Keys.Back:
                    text = "{BACKSPACE}";
                    break;

                case Keys.Down:
                case Keys.Up:
                case Keys.Left:
                case Keys.Right:
                    text = "{" + $"{key.ToString().ToUpper()}" + "}";
                    break;

                case Keys.Control:
                    text = "^";
                    break;

                case Keys.Shift:
                    text = "+";
                    break;

                case Keys.Alt:
                    text = "%";
                    break;

                case Keys.Escape:
                    text = "{ESC}";
                    break;

                case Keys.Tab:
                    text = "{TAB}";
                    break;

                case Keys.F1:
                case Keys.F2:
                case Keys.F3:
                case Keys.F4:
                case Keys.F5:
                case Keys.F6:
                case Keys.F7:
                case Keys.F8:
                case Keys.F9:
                case Keys.F10:
                case Keys.F11:
                case Keys.F12:
                case Keys.F13:
                case Keys.F14:
                case Keys.F15:
                case Keys.F16:
                    text = "{" + $"{key.ToString().ToUpper()}" + "}";
                    break;

                default:
                    text = $"{key.ToString().ToLower()}";
                    break;
                    #endregion
            }
            return text;
        }
    }
}
