/*
 * Copyright 2005 dotlucene.net
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using DesktopSearch1.Icons;
using DesktopSearch1.Parsing;
using ZetaLongPaths;

namespace DesktopSearch1
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class Form1 : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button buttonSearch;

        private ColumnHeader columnHeaderFolder;
        private ColumnHeader columnHeaderIcon;
        private ColumnHeader columnHeaderName;
        private ColumnHeader columnHeaderScore;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private FolderBrowserDialog folderBrowserDialog1;
        private SystemImageList imageListDocuments;
        private ListView listViewResults;
        private string pathIndex;
        private string[] patterns = { "*.doc", "*.xls", "*.ppt", "*.htm", "*.txt" };
        private IndexSearcher searcher = null;
        private Label labelStatus;
        private TextBox textBoxQuery;

        public Form1()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            imageListDocuments = new SystemImageList(SystemImageListSize.SmallIcons);
            SystemImageListHelper.SetListViewImageList(listViewResults, imageListDocuments, false);
            ResizeListViewColumns(listViewResults);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.DoEvents();

            Application.Run(new Form1());
        }

        private void buttonSearch_Click(object sender, System.EventArgs e)
        {
            search();
            //			this.dataGridResults.DataSource = this.results;
        }

        private void checkIndex()
        {
            try
            {
                searcher = new IndexSearcher(this.pathIndex);
                searcher.Close();
            }
            catch (IOException)
            {
                status("The index doesn't exist or is damaged. Please rebuild it.", true);
                return;
            }

            string msg = String.Format("Index is ready. It contains {0} documents.", searcher.MaxDoc());
            status(msg);
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            this.pathIndex = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DesktopSearch");
            checkIndex();
        }

        private void listViewResults_DoubleClick(object sender, System.EventArgs e)
        {
            if (this.listViewResults.SelectedItems.Count != 1)
                return;
            string path = (string)this.listViewResults.SelectedItems[0].Tag;
            Process.Start(path);
        }

        private void ResizeListViewColumns(ListView lv)
        {
            foreach (ColumnHeader column in lv.Columns)
            {
                column.Width = -2;
            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxQuery = new System.Windows.Forms.TextBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.listViewResults = new System.Windows.Forms.ListView();
            this.columnHeaderIcon = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderScore = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderFolder = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.labelStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // textBoxQuery
            //
            this.textBoxQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxQuery.Location = new System.Drawing.Point(16, 176);
            this.textBoxQuery.Name = "textBoxQuery";
            this.textBoxQuery.Size = new System.Drawing.Size(648, 20);
            this.textBoxQuery.TabIndex = 5;
            this.textBoxQuery.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxQuery_KeyDown);
            //
            // buttonSearch
            //
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonSearch.Location = new System.Drawing.Point(672, 176);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(80, 21);
            this.buttonSearch.TabIndex = 6;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            //
            // listViewResults
            //
            this.listViewResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderIcon,
            this.columnHeaderName,
            this.columnHeaderScore,
            this.columnHeaderFolder});
            this.listViewResults.FullRowSelect = true;
            this.listViewResults.HideSelection = false;
            this.listViewResults.Location = new System.Drawing.Point(16, 208);
            this.listViewResults.Name = "listViewResults";
            this.listViewResults.Size = new System.Drawing.Size(736, 352);
            this.listViewResults.TabIndex = 7;
            this.listViewResults.UseCompatibleStateImageBehavior = false;
            this.listViewResults.View = System.Windows.Forms.View.Details;
            this.listViewResults.DoubleClick += new System.EventHandler(this.listViewResults_DoubleClick);
            //
            // columnHeaderIcon
            //
            this.columnHeaderIcon.Text = "";
            this.columnHeaderIcon.Width = 22;
            //
            // columnHeaderName
            //
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 243;
            //
            // columnHeaderScore
            //
            this.columnHeaderScore.Text = "Score";
            //
            // columnHeaderFolder
            //
            this.columnHeaderFolder.Text = "Folder";
            this.columnHeaderFolder.Width = 500;
            //
            // folderBrowserDialog1
            //
            this.folderBrowserDialog1.ShowNewFolderButton = false;
            //
            // labelStatus
            //
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(91, 82);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(35, 13);
            this.labelStatus.TabIndex = 8;
            this.labelStatus.Text = "label1";
            //
            // Form1
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(768, 574);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.listViewResults);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.textBoxQuery);
            this.Name = "Form1";
            this.Text = "Desktop Search";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion Windows Form Designer generated code

        private void search()
        {
            DateTime start = DateTime.Now;

            try
            {
                searcher = new IndexSearcher(this.pathIndex);
            }
            catch (IOException ex)
            {
                MessageBox.Show("The index doesn't exist or is damaged. Please rebuild the index.\r\n\r\nDetails:\r\n" + ex.Message);
                return;
            }

            this.listViewResults.Items.Clear();

            // Parse the query, "text" is the default field to search
            if (this.textBoxQuery.Text.Trim(new char[] { ' ' }) == String.Empty)
                return;

            Query query = QueryParser.Parse(this.textBoxQuery.Text, "text", new StandardAnalyzer());

            // Search
            Hits hits = searcher.Search(query);

            //			Optionally limit the result count
            //			int resultsCount = smallerOf(20, hits.Length());

            listViewResults.BeginUpdate();
            for (int i = 0; i < hits.Length(); i++)
            {
                // get the document from index
                Document doc = hits.Doc(i);

                // create a new row with the result data
                string filename = doc.Get("title");
                string path = doc.Get("path");

                string folder = Path.GetDirectoryName(path);
                Debug.WriteLine($"path: {path} - folder: {folder}");

                DirectoryInfo di = new DirectoryInfo(folder);

                ListViewItem item = new ListViewItem(new string[] { null, filename, hits.Score(i).ToString(), di.FullName });
                item.Tag = path;
                item.ImageIndex = imageListDocuments.IconIndex(filename);
                this.listViewResults.Items.Add(item);
                Application.DoEvents();
            }
            listViewResults.EndUpdate();
            ResizeListViewColumns(listViewResults);
            searcher.Close();

            string searchReport = String.Format("Search took {0}. Found {1} items.", (DateTime.Now - start), hits.Length());
            status(searchReport);
        }

        private int smallerOf(int first, int second)
        {
            return first < second ? first : second;
        }

        /// <summary>
        /// Updates the status label.
        /// </summary>
        /// <param name="msg"></param>
        private void status(string msg)
        {
            status(msg, false);
        }

        private void status(string msg, bool error)
        {
            this.labelStatus.Text = msg;

            if (error)
                this.labelStatus.ForeColor = Color.Red;
            else
                this.labelStatus.ForeColor = DefaultForeColor;

            Application.DoEvents();
        }

        private void textBoxQuery_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                search();
        }
    }
}