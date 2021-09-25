using SharpScripter.Helpers;
using SharpScripter.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading;
using System.Windows.Forms;
using static SharpScripter.Models.ScripterItem;

namespace SharpScripter
{
    public partial class Scripter : Form
    {
        public List<ScripterItem> scriptList = new List<ScripterItem>();
        public int ID = 0;
        public JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true, Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };
        int step = 0;
        bool stop = false;
        bool loop = false;
        bool loadAtRuntime = false;
        string loadPath = null;

        public Scripter()
        {
            InitializeComponent();
            ScriptsBox.View = View.Details;
            ScriptsBox.GridLines = false;
            ScriptsBox.CheckBoxes = false;
            ScriptsBox.FullRowSelect = true;
            ScriptsBox.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            ScriptsBox.Columns[0].Width = Column1.Width;
            ScriptsBox.Refresh();
        }

        private void Scripter_Load(object sender, EventArgs e)
        {
            List<ScripterItem> functions = new List<ScripterItem>();
            functions.Add(new ScripterItem { Name = "Click", Script = ScriptTypes.Click });
            functions.Add(new ScripterItem { Name = "Find And Move Cursor", Script = ScriptTypes.FindAndMove });
            functions.Add(new ScripterItem { Name = "Press Key", Script = ScriptTypes.PressKey });
            functions.Add(new ScripterItem { Name = "Write", Script = ScriptTypes.Write });
            functions.Add(new ScripterItem { Name = "Select All (CTRL + A)", Script = ScriptTypes.SelectAll });
            functions.Add(new ScripterItem { Name = "Copy (CTRL + C)", Script = ScriptTypes.Copy });
            functions.Add(new ScripterItem { Name = "Paste (CTRL + V)", Script = ScriptTypes.Paste });
            functions.Add(new ScripterItem { Name = "Sleep", Script = ScriptTypes.Sleep });
            functions.Add(new ScripterItem { Name = "Move Cursor", Script = ScriptTypes.Move });
            functions.Add(new ScripterItem { Name = "Drag Cursor", Script = ScriptTypes.Drag });
            functions.Add(new ScripterItem { Name = "Get RGB Values (Copy)", Script = ScriptTypes.RGBSniper });
            functions.Add(new ScripterItem { Name = "Get Cursor Pos (Copy)", Script = ScriptTypes.MouseCoords });
            functions.Add(new ScripterItem { Name = "Screenshot", Script = ScriptTypes.Screenshot });
            functions.Add(new ScripterItem { Name = "Start Specific Program", Script = ScriptTypes.StartProgram });
            functions.Add(new ScripterItem { Name = "Close Specific Program", Script = ScriptTypes.KillProgram });
            functions.Add(new ScripterItem { Name = "Close the Scripter", Script = ScriptTypes.KillScripter });
            functions.Add(new ScripterItem { Name = "Shutdown the PC", Script = ScriptTypes.Shutdown });
            functions.Add(new ScripterItem { Name = "Load At Runtime", Script = ScriptTypes.LoadAtRuntime });

            FunctionsBox.DataSource = functions;
            FunctionsBox.ValueMember = "Name";
            FunctionsBox.DisplayMember = "Name";
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown1.Enabled = radioButton5.Checked;
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            Add();
        }

        private void Add()
        {
            #region ADD ITEMS
            if (ScriptsBox.Items.Count > 0)
            {
                var item = ScriptsBox.Items[ScriptsBox.Items.Count - 1];
                switch (((ScripterItem)item.Tag).Script)
                {
                    case ScriptTypes.LoadAtRuntime:
                    case ScriptTypes.KillScripter:
                    case ScriptTypes.Shutdown:
                        ScriptsBox.Items[ScriptsBox.Items.Count - 1].BackColor = Color.Red;
                        MessageBox.Show("If you want to add new items, delete the last item in your script.");
                        ScriptsBox.Items[ScriptsBox.Items.Count - 1].BackColor = Color.White;
                        return;
                }
            }

            if (FunctionsBox.SelectedItem != null)
            {
                switch (((ScripterItem)FunctionsBox.SelectedItem).Script)
                {
                    case ScriptTypes.Shutdown:

                    case ScriptTypes.KillScripter:

                    case ScriptTypes.MouseCoords:

                    case ScriptTypes.SelectAll:

                    case ScriptTypes.Copy:

                    case ScriptTypes.Paste:

                        ScripterItem selected = (ScripterItem)FunctionsBox.SelectedItem;
                        ScripterItem item = new ScripterItem()
                        {
                            ID = ID,
                            Name = selected.Name,
                            Script = selected.Script,
                        };
                        ID++;
                        scriptList.Add(item);
                        ReloadList();
                        break;

                    case ScriptTypes.Click:
                        ClickHelper clickHelper = new ClickHelper() { TopMost = true };
                        this.Hide();
                        clickHelper.ShowDialog();
                        this.Show();
                        Dictionary<string, string> clickParameters = clickHelper.Parameters;
                        if (!clickParameters.ContainsKey("FromCursor")) { return; }
                        if (clickParameters["FromCursor"] == "1")
                            scriptList.Add(new ScripterItem() { ID = ID, Name = "Click (From Cursor)", Script = ScriptTypes.Click, Parameters = clickParameters });
                        else
                            scriptList.Add(new ScripterItem() { ID = ID, Name = "Click (" + clickParameters["x"] + ", " + clickParameters["y"] + ")", Script = ScriptTypes.Click, Parameters = clickParameters });
                        ID++;
                        ReloadList();
                        break;

                    case ScriptTypes.FindAndMove:
                        string photoPath = "";
                        using (OpenFileDialog openFileDialog = new OpenFileDialog())
                        {
                            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            openFileDialog.Filter = "PNG File | *.png";
                            openFileDialog.FilterIndex = 1;
                            openFileDialog.RestoreDirectory = true;

                            if (openFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                photoPath = openFileDialog.FileName;
                            }
                        }
                        if (string.IsNullOrWhiteSpace(photoPath)) { return; }
                        Dictionary<string, string> findParams = new Dictionary<string, string>();
                        findParams.Add("path", photoPath);
                        Array pathArr = photoPath.Split('\\');
                        string photoName = pathArr.GetValue(pathArr.Length - 1).ToString();
                        scriptList.Add(new ScripterItem() { ID = ID, Name = "Find And Move Cursor (" + photoName + ")", Script = ScriptTypes.FindAndMove, Parameters = findParams });
                        ID++;
                        ReloadList();
                        break;

                    case ScriptTypes.PressKey:
                        KeyHelper keyHelper = new KeyHelper() { TopMost = true };
                        keyHelper.ShowDialog();
                        Keys key = keyHelper.key;
                        if (key == Keys.None) { return; }
                        string keyText = keyHelper.keyText;
                        bool hold = keyHelper.hold;
                        int sec = keyHelper.secs;

                        Dictionary<string, Keys> keyParameters = new Dictionary<string, Keys>();
                        Dictionary<string, string> normalParameters = new Dictionary<string, string>();
                        keyParameters.Add("key", key);
                        normalParameters.Add("hold", hold.ToString());
                        if (hold) { normalParameters.Add("sec", sec.ToString()); }

                        scriptList.Add(new ScripterItem() { ID = ID, Name = "Key Press (" + keyText + ")", Script = ScriptTypes.PressKey, KeyParams = keyParameters, Parameters = normalParameters });
                        ID++;
                        ReloadList();
                        break;

                    case ScriptTypes.Write:
                        WriteHelper writeHelper = new WriteHelper() { TopMost = true };
                        writeHelper.ShowDialog();
                        string text = writeHelper.text;
                        if (string.IsNullOrEmpty(text)) { return; }

                        Dictionary<string, string> writeParams = new Dictionary<string, string>();
                        writeParams.Add("text", text);

                        scriptList.Add(new ScripterItem() { ID = ID, Name = "Write (" + text.Replace("\n", " ") + ")", Script = ScriptTypes.Write, Parameters = writeParams });
                        ID++;
                        ReloadList();
                        break;

                    case ScriptTypes.Sleep:
                        SleepHelper sleepHelper = new SleepHelper() { TopMost = true };
                        sleepHelper.ShowDialog();
                        string seconds = sleepHelper.seconds;
                        if (string.IsNullOrWhiteSpace(seconds)) { return; }

                        Dictionary<string, string> sleepParams = new Dictionary<string, string>();
                        sleepParams.Add("sec", seconds);

                        scriptList.Add(new ScripterItem() { ID = ID, Name = "Sleep (" + seconds + " sec)", Script = ScriptTypes.Sleep, Parameters = sleepParams });
                        ID++;
                        ReloadList();
                        break;

                    case ScriptTypes.Screenshot:
                        ScreenshotHelper ssHelper = new ScreenshotHelper() { TopMost = true };
                        this.Hide();
                        ssHelper.ShowDialog();
                        this.Show();
                        if (ssHelper.cancelled) { return; }
                        (Point start, Point end) = (ssHelper.startPoint, ssHelper.endPoint);

                        Dictionary<string, string> ssParams = new Dictionary<string, string>();
                        ssParams.Add("x1", start.X.ToString());
                        ssParams.Add("y1", start.Y.ToString());
                        ssParams.Add("x2", end.X.ToString());
                        ssParams.Add("y2", end.Y.ToString());

                        scriptList.Add(new ScripterItem() { ID = ID, Name = "Screenshot (" + start.ToString() + ", " + end.ToString() + ")", Script = ScriptTypes.Screenshot, Parameters = ssParams });
                        ID++;
                        ReloadList();
                        break;

                    case ScriptTypes.Move:
                        MoveCursorHelper mcHelper = new MoveCursorHelper() { TopMost = true };
                        this.Hide();
                        mcHelper.ShowDialog();
                        this.Show();
                        if (mcHelper.cancelled) { return; }
                        Point coords = mcHelper.coords;

                        Dictionary<string, string> mcParams = new Dictionary<string, string>();
                        mcParams.Add("x", coords.X.ToString());
                        mcParams.Add("y", coords.Y.ToString());

                        scriptList.Add(new ScripterItem() { ID = ID, Name = "Move Cursor (" + coords.X.ToString() + ", " + coords.Y.ToString() + ")", Script = ScriptTypes.Move, Parameters = mcParams });
                        ID++;
                        ReloadList();
                        break;

                    case ScriptTypes.Drag:
                        DragCursorHelper dcHelper = new DragCursorHelper() { TopMost = true };
                        this.Hide();
                        dcHelper.ShowDialog();
                        this.Show();
                        if (dcHelper.cancelled) { return; }
                        (Point startP, Point endP) = (dcHelper.startPoint, dcHelper.endPoint);

                        Dictionary<string, string> dcParams = new Dictionary<string, string>();
                        dcParams.Add("x1", startP.X.ToString());
                        dcParams.Add("y1", startP.Y.ToString());
                        dcParams.Add("x2", endP.X.ToString());
                        dcParams.Add("y2", endP.Y.ToString());

                        scriptList.Add(new ScripterItem() { ID = ID, Name = "Drag Cursor (" + startP.ToString() + ", " + endP.ToString() + ")", Script = ScriptTypes.Drag, Parameters = dcParams });
                        ID++;
                        ReloadList();
                        break;

                    case ScriptTypes.RGBSniper:
                        RGBHelper rgbHelper = new RGBHelper() { TopMost = true };
                        this.Hide();
                        rgbHelper.ShowDialog();
                        this.Show();

                        if (rgbHelper.cancelled) { return; }
                        Point rgbCoords = rgbHelper.coords;
                        bool fromCursor = rgbHelper.FromCursor;

                        Dictionary<string, string> rgbParams = new Dictionary<string, string>();
                        rgbParams.Add("x", rgbCoords.X.ToString());
                        rgbParams.Add("y", rgbCoords.Y.ToString());
                        rgbParams.Add("FromCursor", fromCursor.ToString());

                        if (rgbHelper.FromCursor)
                        {
                            scriptList.Add(new ScripterItem() { ID = ID, Name = "Get RGB (From Cursor)", Script = ScriptTypes.RGBSniper, Parameters = rgbParams });
                        }
                        else
                        {
                            scriptList.Add(new ScripterItem() { ID = ID, Name = "Get RGB (" + rgbCoords.X.ToString() + ", " + rgbCoords.Y.ToString() + ")", Script = ScriptTypes.RGBSniper, Parameters = rgbParams });
                        }

                        ID++;
                        ReloadList();
                        break;

                    case ScriptTypes.StartProgram:
                        AppHelper appHelper = new AppHelper() { TopMost = true };
                        this.Hide();
                        appHelper.ShowDialog();
                        this.Show();
                        if (appHelper.cancelled) { return; }
                        App app = appHelper.selectedApp;

                        Dictionary<string, string> appParams = new Dictionary<string, string>();
                        appParams.Add("path", app.ExePath);
                        scriptList.Add(new ScripterItem() { ID = ID, Name = "Start Specific Program (" + app.AppName + ")", Script = ScriptTypes.StartProgram, Parameters = appParams });

                        ID++;
                        ReloadList();
                        break;

                    case ScriptTypes.KillProgram:
                        AppHelper killHelper = new AppHelper() { TopMost = true };
                        this.Hide();
                        killHelper.ShowDialog();
                        this.Show();
                        if (killHelper.cancelled) { return; }
                        App appToKill = killHelper.selectedApp;

                        Dictionary<string, string> killParams = new Dictionary<string, string>();
                        killParams.Add("name", appToKill.AppName);
                        scriptList.Add(new ScripterItem() { ID = ID, Name = "Close Specific Program (" + appToKill.AppName + ")", Script = ScriptTypes.KillProgram, Parameters = killParams });

                        ID++;
                        ReloadList();
                        break;

                    case ScriptTypes.LoadAtRuntime:
                        string filePath = null;
                        using (OpenFileDialog openFileDialog = new OpenFileDialog())
                        {
                            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            openFileDialog.Filter = "JSON File (*.json)|*.json";
                            openFileDialog.FilterIndex = 1;
                            openFileDialog.RestoreDirectory = true;

                            if (openFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                filePath = openFileDialog.FileName;
                            }
                        }
                        if (string.IsNullOrWhiteSpace(filePath)) { return; }
                        Dictionary<string, string> loadParams = new Dictionary<string, string>();
                        loadParams.Add("path", filePath);
                        Array arr = filePath.Split('\\');
                        string programText = arr.GetValue(arr.Length - 1).ToString();
                        scriptList.Add(new ScripterItem() { ID = ID, Name = "Load At Runtime (" + programText + ")", Script = ScriptTypes.LoadAtRuntime, Parameters = loadParams });

                        ID++;
                        ReloadList();
                        break;

                    default:
                        MessageBox.Show("Bir hata oluştu!");
                        return;
                }
            }
            #endregion
        }

        private int checkForLength(ListView view)
        {
            int length = 0;
            foreach (ListViewItem item in view.Items)
            {
                int nameLength = ((ScripterItem)item.Tag).Name.Length;
                if (length < nameLength) { length = nameLength; }
            }
            return length;
        }

        private void CreateListViewItem(ListView view, ScripterItem obj)
        {
            ListViewItem item = new ListViewItem(obj.Name);
            item.Tag = obj;

            view.Items.Add(item);
        }

        private void ReloadList()
        {
            ScriptsBox.Items.Clear();
            scriptList.ForEach(scriptObject => { CreateListViewItem(ScriptsBox, scriptObject); });
            ScriptsBox.Columns[0].Width = 12 * checkForLength(ScriptsBox);
            ScriptsBox.Refresh();
        }

        private void UpBtn_Click(object sender, EventArgs e)
        {
            if (ScriptsBox.SelectedItems.Count > 0)
            {
                ScripterItem selected = (ScripterItem)ScriptsBox.SelectedItems[0].Tag;
                int index = scriptList.IndexOf(selected);
                if (index <= 0)
                {
                    MessageBox.Show("Selected item is already at the top.");
                    return;
                }
                scriptList[index] = scriptList[index - 1];
                scriptList[index - 1] = selected;
                ReloadList();
                ScriptsBox.Items[index - 1].Selected = true;
                ScriptsBox.Select();
            }
            else { return; }
        }

        private void DownBtn_Click(object sender, EventArgs e)
        {
            if (ScriptsBox.SelectedItems.Count > 0)
            {
                ScripterItem selected = (ScripterItem)ScriptsBox.SelectedItems[0].Tag;
                int index = scriptList.IndexOf(selected);
                if (index >= scriptList.Count - 1)
                {
                    MessageBox.Show("Selected item is already at the bottom.");
                    return;
                }

                scriptList[index] = scriptList[index + 1];
                scriptList[index + 1] = selected;
                ReloadList();
                ScriptsBox.Items[index + 1].Selected = true;
                ScriptsBox.Select();
            }
            else { return; }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (ScriptsBox.SelectedItems.Count > 0)
            {
                ScripterItem selected = (ScripterItem)ScriptsBox.SelectedItems[0].Tag;
                scriptList.Remove(selected);
                ReloadList();
            }
            else { return; }
        }

        private void FunctionsBox_DoubleClick(object sender, EventArgs e)
        {
            Add();
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JSON File (*.json)|*.json";
            saveFileDialog1.Title = "JSON Olarak Kaydet";
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                string path = saveFileDialog1.FileName;
                Dictionary<string, string> exportDict = new Dictionary<string, string>();
                exportDict.Add("loop", checkBox1.Checked.ToString());
                exportDict.Add("minimize", checkBox2.Checked.ToString());
                if (radioButton1.Checked) { exportDict.Add("1", "5"); }
                else if (radioButton2.Checked) { exportDict.Add("2", "10"); }
                else if (radioButton3.Checked) { exportDict.Add("3", "15"); }
                else if (radioButton4.Checked) { exportDict.Add("4", "30"); }
                else if (radioButton5.Checked) { exportDict.Add("5", numericUpDown1.Value.ToString()); }

                IEData ExportData = new IEData()
                {
                    ID = ID,
                    Items = scriptList,
                    Parameters = exportDict
                };

                string jsonString = JsonSerializer.Serialize(ExportData, options);
                File.WriteAllText(path, jsonString);
                MessageBox.Show("Script saved successfully!");
            }
        }

        private void LoadBtn_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                openFileDialog.Filter = "JSON File (*.json)|*.json";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                }
            }

            if (filePath != string.Empty)
            {
                string jsonString = File.ReadAllText(filePath);
                IEData ImportData = JsonSerializer.Deserialize<IEData>(jsonString, options);
                this.ID = ImportData.ID;
                checkBox1.Checked = Convert.ToBoolean(ImportData.Parameters["loop"]);
                checkBox2.Checked = Convert.ToBoolean(ImportData.Parameters["minimize"]);
                if (ImportData.Parameters.ContainsKey("1")) { radioButton1.Checked = true; }
                else if (ImportData.Parameters.ContainsKey("2")) { radioButton2.Checked = true; }
                else if (ImportData.Parameters.ContainsKey("3")) { radioButton3.Checked = true; }
                else if (ImportData.Parameters.ContainsKey("4")) { radioButton4.Checked = true; }
                else if (ImportData.Parameters.ContainsKey("5"))
                {
                    radioButton5.Checked = true;
                    numericUpDown1.Value = Convert.ToInt32(ImportData.Parameters["5"]);
                }
                else
                {
                    radioButton5.Checked = true;
                    numericUpDown1.Value = 0;
                }

                scriptList = ImportData.Items;
                ReloadList();
            }
        }

        private void LoadAtRuntime(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                string jsonString = File.ReadAllText(path);
                IEData ImportData = JsonSerializer.Deserialize<IEData>(jsonString, options);
                this.ID = ImportData.ID;
                checkBox1.Checked = Convert.ToBoolean(ImportData.Parameters["loop"]);
                checkBox2.Checked = Convert.ToBoolean(ImportData.Parameters["minimize"]);
                if (ImportData.Parameters.ContainsKey("1")) { radioButton1.Checked = true; }
                else if (ImportData.Parameters.ContainsKey("2")) { radioButton2.Checked = true; }
                else if (ImportData.Parameters.ContainsKey("3")) { radioButton3.Checked = true; }
                else if (ImportData.Parameters.ContainsKey("4")) { radioButton4.Checked = true; }
                else if (ImportData.Parameters.ContainsKey("5"))
                {
                    radioButton5.Checked = true;
                    numericUpDown1.Value = Convert.ToInt32(ImportData.Parameters["5"]);
                }
                else
                {
                    radioButton5.Checked = true;
                    numericUpDown1.Value = 0;
                }

                scriptList = ImportData.Items;
                ReloadList();
            }
        }

        private void worker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            int sleepBeforeStart = 0;
            if (radioButton1.Checked) { sleepBeforeStart = 5; }
            else if (radioButton2.Checked) { sleepBeforeStart = 10; }
            else if (radioButton3.Checked) { sleepBeforeStart = 15; }
            else if (radioButton4.Checked) { sleepBeforeStart = 30; }
            else if (radioButton5.Checked) { sleepBeforeStart = Convert.ToInt32(numericUpDown1.Value); }

            if (sleepBeforeStart != 0) { Thread.Sleep(sleepBeforeStart * 1000); }

            while (true)
            {
                #region MAIN LOOP
                foreach (ScripterItem item in scriptList)
                {
                    if (stop) { return; }
                    switch (item.Script)
                    {
                        case ScriptTypes.Click:
                            if (item.Parameters["FromCursor"] == "1")
                            {
                                Point cursor = Utils.GetCursorPos();
                                if (item.Parameters["doubleclick"] == "0") { Utils.Click(cursor.X, cursor.Y); }
                                else if (item.Parameters["doubleclick"] == "1") { Utils.Click(cursor.X, cursor.Y, true); }
                            }
                            else if (item.Parameters["FromCursor"] == "0")
                            {
                                Point newPoint = new Point(Convert.ToInt32(item.Parameters["x"]), Convert.ToInt32(item.Parameters["y"]));
                                if (item.Parameters["doubleclick"] == "0") { Utils.Click(newPoint.X, newPoint.Y, true); }
                                else if (item.Parameters["doubleclick"] == "1") { Utils.Click(newPoint.X, newPoint.Y, true); }
                            }
                            break;

                        case ScriptTypes.FindAndMove:
                            Bitmap Pic = new Bitmap(item.Parameters["path"]);
                            Bitmap screenCapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                            Graphics g = Graphics.FromImage(screenCapture);

                            g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                             Screen.PrimaryScreen.Bounds.Y,
                                             0, 0,
                                             screenCapture.Size,
                                             CopyPixelOperation.SourceCopy);

                            Rectangle rect = Utils.FindImageOnScreen(Pic, screenCapture, false);
                            if (rect != Rectangle.Empty)
                            {
                                Utils.SetCursorPos(rect.X + (rect.Width / 2), rect.Y + (rect.Height / 2));
                            }
                            break;

                        case ScriptTypes.SelectAll:
                            this.Invoke((MethodInvoker)delegate
                            {
                                SendKeys.Send("^a");
                            });
                            break;

                        case ScriptTypes.Copy:
                            this.Invoke((MethodInvoker)delegate
                            {
                                SendKeys.Send("^c");
                            });
                            break;

                        case ScriptTypes.Paste:
                            this.Invoke((MethodInvoker)delegate
                            {
                                SendKeys.Send("^v");
                            });
                            break;

                        case ScriptTypes.Move:
                            Point toMove = new Point(Convert.ToInt32(item.Parameters["x"]), Convert.ToInt32(item.Parameters["y"]));
                            Utils.SetCursorPos(toMove.X, toMove.Y);
                            break;

                        case ScriptTypes.Drag:
                            Point startPoint = new Point(Convert.ToInt32(item.Parameters["x1"]), Convert.ToInt32(item.Parameters["y1"]));
                            Point endPoint = new Point(Convert.ToInt32(item.Parameters["x2"]), Convert.ToInt32(item.Parameters["y2"]));
                            Point currentPos = Utils.GetCursorPos();

                            if (startPoint == currentPos) { Utils.SetCursorDrag(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y); }
                            else
                            {
                                Utils.SetCursorPos(startPoint.X, startPoint.Y);
                                Thread.Sleep(300);
                                Utils.SetCursorDrag(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);
                            }
                            break;

                        case ScriptTypes.MouseCoords:
                            Point coords = MousePosition;
                            Clipboard.SetText("x = " + coords.X.ToString() + ", y = " + coords.Y.ToString());
                            break;

                        case ScriptTypes.PressKey:
                            Keys key = item.KeyParams["key"];
                            int secs = 0;
                            bool hold = Convert.ToBoolean(item.Parameters["hold"]);
                            if (hold) { secs = Convert.ToInt32(item.Parameters["sec"]); }
                            string text = KeyConvert.Convert(key);
                            this.Invoke((MethodInvoker)delegate
                            {
                                if (hold)
                                {
                                    DateTime finishPress = DateTime.Now.AddSeconds(secs);
                                    while (DateTime.Now <= finishPress)
                                    {
                                        SendKeys.Send(text);
                                    }
                                }
                                else
                                {
                                    SendKeys.Send(text);
                                }
                            });
                            break;

                        case ScriptTypes.Screenshot:
                            Point start = new Point(Convert.ToInt32(item.Parameters["x1"]), Convert.ToInt32(item.Parameters["y1"]));
                            Point end = new Point(Convert.ToInt32(item.Parameters["x2"]), Convert.ToInt32(item.Parameters["y2"]));
                            Utils.GetScreenshot(start.X, start.Y, end.X - start.X, end.Y - start.Y);
                            break;

                        case ScriptTypes.Write:
                            this.Invoke((MethodInvoker)delegate
                            {
                                string textToWrite = item.Parameters["text"];
                                string oldText = Clipboard.GetText();
                                Clipboard.SetText(textToWrite);
                                SendKeys.Send("^v");
                                Clipboard.SetText(oldText);
                            });

                            break;

                        case ScriptTypes.RGBSniper:
                            if (Convert.ToBoolean(item.Parameters["FromCursor"]))
                            {
                                Color color = Utils.GetPixelColor(Utils.GetCursorPos());
                                this.Invoke((MethodInvoker)delegate
                                {
                                    Clipboard.SetText($"R: {color.R}, G: {color.G}, B: {color.B}");
                                });
                            }
                            else
                            {
                                Point rgbCoords = new Point(Convert.ToInt32(item.Parameters["x"]), Convert.ToInt32(item.Parameters["y"]));
                                Color color = Utils.GetPixelColor(rgbCoords);
                                this.Invoke((MethodInvoker)delegate
                                {
                                    Clipboard.SetText($"R: {color.R}, G: {color.G}, B: {color.B}");
                                });
                            }
                            break;

                        case ScriptTypes.Sleep:
                            int sec = Convert.ToInt32(item.Parameters["sec"]);
                            for (int i = 0; i < sec * 10; i++)
                            {
                                if (stop) { return; }
                                Thread.Sleep(100);
                            }
                            break;

                        case ScriptTypes.StartProgram:
                            string path = item.Parameters["path"];
                            Utils.StartProcess(path);
                            break;

                        case ScriptTypes.KillProgram:
                            string name = item.Parameters["name"];
                            Utils.KillProcess(name);
                            break;

                        case ScriptTypes.KillScripter:
                            Environment.Exit(0);
                            break;

                        case ScriptTypes.Shutdown:
                            Process.Start("shutdown", "/s /t 0");
                            Environment.Exit(0);
                            break;

                        case ScriptTypes.LoadAtRuntime:
                            loadPath = item.Parameters["path"];
                            loadAtRuntime = true;
                            break;
                    }
                    worker.ReportProgress(0);
                }

                if (!loop || stop)
                {
                    break;
                }
                else
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        step = 0;
                        for (int i = 0; i < ScriptsBox.Items.Count; i++)
                        {
                            ScriptsBox.Items[i].BackColor = Color.FromArgb(255, 172, 0);
                        }
                    });
                }
                #endregion
            }
        }

        private void worker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            ScriptsBox.Items[step].BackColor = Color.LawnGreen;
            step++;
        }

        private void worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            radioButton1.Enabled = true;
            radioButton3.Enabled = true;
            radioButton4.Enabled = true;
            radioButton2.Enabled = true;
            radioButton5.Enabled = true;
            numericUpDown1.Enabled = true;
            AddBtn.Enabled = true;
            EditBtn.Enabled = true;
            DeleteBtn.Enabled = true;
            UpBtn.Enabled = true;
            DownBtn.Enabled = true;
            RunBtn.Enabled = true;
            SaveBtn.Enabled = true;
            LoadBtn.Enabled = true;
            StopBtn.Enabled = false;
            stop = false;
            step = 0;
            if (checkBox2.Checked) { this.WindowState = FormWindowState.Normal; }
            this.TopMost = true;
            if (!loadAtRuntime) { MessageBox.Show(new Form() { TopMost = true }, "Script is done."); }
            this.TopMost = false;
            for (int i = 0; i < ScriptsBox.Items.Count; i++)
            {
                ScriptsBox.Items[i].BackColor = Color.White;
            }

            if (loadAtRuntime)
            {
                LoadAtRuntime(loadPath);
                RunBtn_Click(this, new EventArgs());
                loadAtRuntime = false;
            }
        }

        private void RunBtn_Click(object sender, EventArgs e)
        {
            loop = checkBox1.Checked;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            radioButton3.Enabled = false;
            radioButton4.Enabled = false;
            radioButton5.Enabled = false;
            numericUpDown1.Enabled = false;
            AddBtn.Enabled = false;
            EditBtn.Enabled = false;
            DeleteBtn.Enabled = false;
            UpBtn.Enabled = false;
            DownBtn.Enabled = false;
            RunBtn.Enabled = false;
            SaveBtn.Enabled = false;
            LoadBtn.Enabled = false;
            StopBtn.Enabled = true;
            stop = false;

            for (int i = 0; i < ScriptsBox.Items.Count; i++)
            {
                ScriptsBox.Items[i].BackColor = Color.FromArgb(255, 172, 0);
            }

            if (checkBox2.Checked) { this.WindowState = FormWindowState.Minimized; }
            worker.RunWorkerAsync();
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            stop = true;
            worker.CancelAsync();
        }

        private void Edit()
        {
            #region EDIT
            if (ScriptsBox.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = ScriptsBox.SelectedItems[0];
                ScripterItem item = (ScripterItem)selectedItem.Tag;
                switch (item.Script)
                {
                    case ScriptTypes.Shutdown:
                    case ScriptTypes.KillScripter:
                    case ScriptTypes.SelectAll:
                    case ScriptTypes.Copy:
                    case ScriptTypes.Paste:
                    case ScriptTypes.MouseCoords:
                        MessageBox.Show("This property cannot be edited");
                        break;

                    case ScriptTypes.Click:
                        ClickHelper clickHelper = new ClickHelper(item.Parameters) { TopMost = true };
                        clickHelper.ShowDialog();

                        Dictionary<string, string> clickParameters = clickHelper.Parameters;
                        if (!clickParameters.ContainsKey("FromCursor")) { return; }
                        if (clickParameters["FromCursor"] == "1")
                            scriptList.Insert(scriptList.IndexOf(item), new ScripterItem() { ID = item.ID, Name = "Click (From Cursor)", Script = ScriptTypes.Click, Parameters = clickParameters });
                        else
                            scriptList.Insert(scriptList.IndexOf(item), new ScripterItem() { ID = item.ID, Name = "Click (" + clickParameters["x"] + ", " + clickParameters["y"] + ")", Script = ScriptTypes.Click, Parameters = clickParameters });

                        scriptList.Remove(item);
                        ReloadList();
                        break;

                    case ScriptTypes.FindAndMove:
                        string photoPath = "";
                        using (OpenFileDialog openFileDialog = new OpenFileDialog())
                        {
                            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            openFileDialog.Filter = "PNG File | *.png";
                            openFileDialog.FilterIndex = 1;
                            openFileDialog.RestoreDirectory = true;

                            if (openFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                photoPath = openFileDialog.FileName;
                            }
                        }
                        if (string.IsNullOrWhiteSpace(photoPath)) { return; }
                        Dictionary<string, string> findParams = new Dictionary<string, string>();
                        findParams.Add("path", photoPath);
                        Array pathArr = photoPath.Split('\\');
                        string photoName = pathArr.GetValue(pathArr.Length - 1).ToString();
                        scriptList.Insert(scriptList.IndexOf(item), new ScripterItem() { ID = item.ID, Name = "Find And Move Cursor (" + photoName + ")", Script = ScriptTypes.FindAndMove, Parameters = findParams });
                        scriptList.Remove(item);
                        ReloadList();
                        break;

                    case ScriptTypes.PressKey:
                        KeyHelper keyHelper = new KeyHelper(item.KeyParams, item.Parameters) { TopMost = true };
                        keyHelper.ShowDialog();
                        Keys key = keyHelper.key;
                        bool hold = keyHelper.hold;
                        int secs = keyHelper.secs;
                        if (key == Keys.None) { return; }
                        string keyText = keyHelper.keyText;
                        Dictionary<string, Keys> keyParameters = new Dictionary<string, Keys>();
                        Dictionary<string, string> normalParameters = new Dictionary<string, string>();
                        keyParameters.Add("key", key);
                        normalParameters.Add("hold", hold.ToString());
                        if (hold) { normalParameters.Add("sec", secs.ToString()); }

                        scriptList.Insert(scriptList.IndexOf(item), new ScripterItem() { ID = item.ID, Name = "Key Press (" + keyText + ")", Script = ScriptTypes.PressKey, KeyParams = keyParameters, Parameters = normalParameters });

                        scriptList.Remove(item);
                        ReloadList();
                        break;

                    case ScriptTypes.Write:
                        WriteHelper writeHelper = new WriteHelper(item.Parameters) { TopMost = true };
                        writeHelper.ShowDialog();
                        string text = writeHelper.text;
                        if (string.IsNullOrEmpty(text)) { return; }

                        Dictionary<string, string> writeParams = new Dictionary<string, string>();
                        writeParams.Add("text", text);

                        scriptList.Insert(scriptList.IndexOf(item), new ScripterItem() { ID = item.ID, Name = "Write (" + text.Replace("\n", " ") + ")", Script = ScriptTypes.Write, Parameters = writeParams });
                        scriptList.Remove(item);
                        ReloadList();
                        break;

                    case ScriptTypes.Sleep:
                        SleepHelper sleepHelper = new SleepHelper(item.Parameters) { TopMost = true };
                        sleepHelper.ShowDialog();
                        string seconds = sleepHelper.seconds;
                        if (string.IsNullOrWhiteSpace(seconds)) { return; }

                        Dictionary<string, string> sleepParams = new Dictionary<string, string>();
                        sleepParams.Add("sec", seconds);

                        scriptList.Insert(scriptList.IndexOf(item), new ScripterItem() { ID = item.ID, Name = "Sleep (" + seconds + " sec)", Script = ScriptTypes.Sleep, Parameters = sleepParams });
                        scriptList.Remove(item);
                        ReloadList();
                        break;

                    case ScriptTypes.Screenshot:
                        ScreenshotHelper ssHelper = new ScreenshotHelper(item.Parameters) { TopMost = true };
                        this.Hide();
                        ssHelper.ShowDialog();
                        this.Show();
                        if (ssHelper.cancelled) { return; }
                        (Point start, Point end) = (ssHelper.startPoint, ssHelper.endPoint);

                        Dictionary<string, string> ssParams = new Dictionary<string, string>();
                        ssParams.Add("x1", start.X.ToString());
                        ssParams.Add("y1", start.Y.ToString());
                        ssParams.Add("x2", end.X.ToString());
                        ssParams.Add("y2", end.Y.ToString());

                        scriptList.Insert(scriptList.IndexOf(item), new ScripterItem() { ID = item.ID, Name = "Screenshot (" + start.ToString() + ", " + end.ToString() + ")", Script = ScriptTypes.Screenshot, Parameters = ssParams });
                        scriptList.Remove(item);
                        ReloadList();
                        break;

                    case ScriptTypes.Move:
                        MoveCursorHelper mcHelper = new MoveCursorHelper(item.Parameters) { TopMost = true };
                        this.Hide();
                        mcHelper.ShowDialog();
                        this.Show();
                        if (mcHelper.cancelled) { return; }
                        Point coords = mcHelper.coords;

                        Dictionary<string, string> mcParams = new Dictionary<string, string>();
                        mcParams.Add("x", coords.X.ToString());
                        mcParams.Add("y", coords.Y.ToString());

                        scriptList.Insert(scriptList.IndexOf(item), new ScripterItem() { ID = item.ID, Name = "Move Cursor (" + coords.X.ToString() + ", " + coords.Y.ToString() + ")", Script = ScriptTypes.Move, Parameters = mcParams });
                        scriptList.Remove(item);
                        ReloadList();
                        break;

                    case ScriptTypes.Drag:
                        DragCursorHelper dcHelper = new DragCursorHelper(item.Parameters) { TopMost = true };
                        this.Hide();
                        dcHelper.ShowDialog();
                        this.Show();
                        if (dcHelper.cancelled) { return; }
                        (Point startP, Point endP) = (dcHelper.startPoint, dcHelper.endPoint);

                        Dictionary<string, string> dcParams = new Dictionary<string, string>();
                        dcParams.Add("x1", startP.X.ToString());
                        dcParams.Add("y1", startP.Y.ToString());
                        dcParams.Add("x2", endP.X.ToString());
                        dcParams.Add("y2", endP.Y.ToString());

                        scriptList.Insert(scriptList.IndexOf(item), new ScripterItem() { ID = item.ID, Name = "Drag Cursor (" + startP.ToString() + ", " + endP.ToString() + ")", Script = ScriptTypes.Drag, Parameters = dcParams });
                        scriptList.Remove(item);
                        ReloadList();
                        break;

                    case ScriptTypes.RGBSniper:
                        RGBHelper rgbHelper = new RGBHelper(item.Parameters) { TopMost = true };
                        this.Hide();
                        rgbHelper.ShowDialog();
                        this.Show();

                        if (rgbHelper.cancelled) { return; }
                        Point rgbCoords = rgbHelper.coords;
                        bool fromCursor = rgbHelper.FromCursor;

                        Dictionary<string, string> rgbParams = new Dictionary<string, string>();
                        rgbParams.Add("x", rgbCoords.X.ToString());
                        rgbParams.Add("y", rgbCoords.Y.ToString());
                        rgbParams.Add("FromCursor", fromCursor.ToString());

                        if (rgbHelper.FromCursor)
                        {
                            scriptList.Insert(scriptList.IndexOf(item), new ScripterItem() { ID = item.ID, Name = "Get RGB (From Cursor)", Script = ScriptTypes.RGBSniper, Parameters = rgbParams });
                        }
                        else
                        {
                            scriptList.Insert(scriptList.IndexOf(item), new ScripterItem() { ID = item.ID, Name = "Get RGB (" + rgbCoords.X.ToString() + ", " + rgbCoords.Y.ToString() + ")", Script = ScriptTypes.RGBSniper, Parameters = rgbParams });
                        }

                        scriptList.Remove(item);
                        ReloadList();
                        break;

                    case ScriptTypes.StartProgram:
                        AppHelper appHelper = new AppHelper(item.Parameters) { TopMost = true };
                        this.Hide();
                        appHelper.ShowDialog();
                        this.Show();
                        if (appHelper.cancelled) { return; }
                        App app = appHelper.selectedApp;

                        Dictionary<string, string> appParams = new Dictionary<string, string>();
                        appParams.Add("path", app.ExePath);
                        scriptList.Insert(scriptList.IndexOf(item), new ScripterItem() { ID = item.ID, Name = "Start Specific Program (" + app.AppName + ")", Script = ScriptTypes.StartProgram, Parameters = appParams });

                        scriptList.Remove(item);
                        ReloadList();
                        break;

                    case ScriptTypes.KillProgram:
                        AppHelper killHelper = new AppHelper(item.Parameters) { TopMost = true };
                        this.Hide();
                        killHelper.ShowDialog();
                        this.Show();
                        if (killHelper.cancelled) { return; }
                        App appToKill = killHelper.selectedApp;

                        Dictionary<string, string> killParams = new Dictionary<string, string>();
                        killParams.Add("name", appToKill.AppName);
                        scriptList.Insert(scriptList.IndexOf(item), new ScripterItem() { ID = item.ID, Name = "Close Specific Program (" + appToKill.AppName + ")", Script = ScriptTypes.KillProgram, Parameters = killParams });

                        scriptList.Remove(item);
                        ReloadList();
                        break;

                    case ScriptTypes.LoadAtRuntime:
                        string filePath = null;
                        using (OpenFileDialog openFileDialog = new OpenFileDialog())
                        {
                            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            openFileDialog.Filter = "JSON File (*.json)|*.json";
                            openFileDialog.FilterIndex = 1;
                            openFileDialog.RestoreDirectory = true;

                            if (openFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                filePath = openFileDialog.FileName;
                            }
                        }
                        if (string.IsNullOrWhiteSpace(filePath)) { return; }
                        Dictionary<string, string> loadParams = new Dictionary<string, string>();
                        loadParams.Add("path", filePath);
                        Array arr = filePath.Split('\\');
                        string programText = arr.GetValue(arr.Length - 1).ToString();
                        scriptList.Insert(scriptList.IndexOf(item), new ScripterItem() { ID = item.ID, Name = "Load At Runtime (" + programText + ")", Script = ScriptTypes.LoadAtRuntime, Parameters = loadParams });

                        scriptList.Remove(item);
                        ReloadList();
                        break;

                    default:
                        MessageBox.Show("An error occurred!");
                        return;
                }
            }
            #endregion
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            Edit();
        }

        private void ScriptsBox_ItemActivate(object sender, EventArgs e)
        {
            Edit();
        }
    }
}