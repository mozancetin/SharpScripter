using SharpScripter.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SharpScripter.Helpers
{
    public partial class AppHelper : Form
    {
        public List<ListViewItem> allItems = new List<ListViewItem>();
        public App selectedApp;
        public bool cancelled = true;
        private Dictionary<string, string> parameters = null;
        private bool start;
        public bool maximized;
        public AppHelper(Dictionary<string, string> GivenParameters = null)
        {
            InitializeComponent();
            AppBox.View = View.Details;
            AppBox.GridLines = true;
            AppBox.CheckBoxes = false;
            AppBox.FullRowSelect = true;
            AppBox.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            AppBox.Refresh();

            if (GivenParameters != null) { this.parameters = GivenParameters; }
        }

        private void SelectWithGivenParameters(Dictionary<string, string> GivenParameters)
        {
            if (GivenParameters.ContainsKey("path")) { start = true; }
            else { start = false; }

            if (start)
            {
                string path = GivenParameters["path"];
                int index = 0;
                foreach (ListViewItem item in AppBox.Items)
                {
                    if (((App)item.Tag).ExePath == path)
                    {
                        index = item.Index;
                    }
                }
                if (index != 0)
                {
                    AppBox.Items[index].Selected = true;
                    AppBox.Select();
                }
            }
            else
            {
                string name = GivenParameters["name"];
                int index = 0;
                foreach (ListViewItem item in AppBox.Items)
                {
                    if (((App)item.Tag).AppName == name)
                    {
                        index = item.Index;
                    }
                }
                if (index != 0)
                {
                    AppBox.Items[index].Selected = true;
                    AppBox.Select();
                }
            }
        }

        private void AppHelper_Load(object sender, EventArgs e)
        {
            int len = 0;
            ImageList mediumImageList = new ImageList();
            foreach (App item in Utils.GetAllApps())
            {
                CreateListViewItem(AppBox, item);
                mediumImageList.Images.Add(item.Icon);
                if (len < item.AppName.Length) { len = item.AppName.Length; }
            }
            mediumImageList.ImageSize = new Size(32, 32);
            AppBox.SmallImageList = mediumImageList;
            int i = 0;
            foreach (ListViewItem item in AppBox.Items)
            {
                item.ImageIndex = i;
                i++;
            }

            AppBox.Columns[0].Width = len * 12;
            foreach (ListViewItem viewItem in AppBox.Items)
            {
                allItems.Add(viewItem);
            }
            AppBox.Refresh();

            if (parameters != null) { SelectWithGivenParameters(parameters); }
        }

        private void CreateListViewItem(ListView view, App obj)
        {
            ListViewItem item = new ListViewItem(" " + obj.AppName);
            item.Tag = obj;
            view.Items.Add(item);
        }

        private void ReloadList(string like = null)
        {
            AppBox.Items.Clear();
            if (!string.IsNullOrWhiteSpace(like))
            {
                allItems.ForEach(i =>
                {
                    if (i.Text.ToLower().Contains(like.ToLower())) { AppBox.Items.Add(i); }
                });
            }
            else
            {
                allItems.ForEach(i => { AppBox.Items.Add(i); });
            }

            AppBox.Refresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ReloadList(textBox1.Text);
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (AppBox.SelectedItems.Count > 0)
            {
                selectedApp = (App)AppBox.SelectedItems[0].Tag;
                cancelled = false;
            }
            this.Close();
        }

        private void AppBox_ItemActivate(object sender, EventArgs e)
        {
            if (AppBox.SelectedItems.Count > 0)
            {
                selectedApp = (App)AppBox.SelectedItems[0].Tag;
                cancelled = false;
            }
            this.Close();
        }
    }
}
