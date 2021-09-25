using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SharpScripter.Models
{
    public class ScripterItem
    {
        public enum ScriptTypes
        {
            Click,
            FindAndMove,
            RGBSniper,
            MouseCoords,
            SelectAll,
            Copy,
            Paste,
            Sleep,
            Write,
            Move,
            Drag,
            Screenshot,
            SelectArea,
            PressKey,
            StartProgram,
            KillProgram,
            KillScripter,
            LoadAtRuntime,
            Shutdown,
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public ScriptTypes Script { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public Dictionary<string, Keys> KeyParams { get; set; }
    }
}
