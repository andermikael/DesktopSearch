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

namespace DesktopSearch1
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private string pathIndex;
		private IndexWriter indexWriter;
		private string[] patterns = {"*.doc", "*.xls", "*.ppt", "*.htm", "*.txt"};
		private SystemImageList imageListDocuments;
		
		private IndexSearcher searcher = null;

		// statistics
		private long bytesTotal = 0;
		private int countTotal = 0;
		private int countSkipped = 0;

		private System.Windows.Forms.TextBox textBoxPath;
		private System.Windows.Forms.Button buttonBrowse;
		private System.Windows.Forms.Button buttonIndex;
		private System.Windows.Forms.Label labelStatus;
		private System.Windows.Forms.TextBox textBoxQuery;
		private System.Windows.Forms.Button buttonSearch;
		private System.Windows.Forms.ListView listViewResults;
		private System.Windows.Forms.ColumnHeader columnHeaderIcon;
		private System.Windows.Forms.ColumnHeader columnHeaderName;
		private System.Windows.Forms.ColumnHeader columnHeaderFolder;
		private System.Windows.Forms.ColumnHeader columnHeaderScore;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Button buttonClean;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			imageListDocuments = new SystemImageList(SystemImageListSize.SmallIcons);
			SystemImageListHelper.SetListViewImageList(listViewResults, imageListDocuments, false);

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.textBoxPath = new System.Windows.Forms.TextBox();
			this.buttonBrowse = new System.Windows.Forms.Button();
			this.buttonIndex = new System.Windows.Forms.Button();
			this.labelStatus = new System.Windows.Forms.Label();
			this.textBoxQuery = new System.Windows.Forms.TextBox();
			this.buttonSearch = new System.Windows.Forms.Button();
			this.listViewResults = new System.Windows.Forms.ListView();
			this.columnHeaderIcon = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderFolder = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderScore = new System.Windows.Forms.ColumnHeader();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.buttonClean = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// textBoxPath
			// 
			this.textBoxPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxPath.Location = new System.Drawing.Point(16, 48);
			this.textBoxPath.Name = "textBoxPath";
			this.textBoxPath.Size = new System.Drawing.Size(616, 20);
			this.textBoxPath.TabIndex = 1;
			this.textBoxPath.Text = "";
			// 
			// buttonBrowse
			// 
			this.buttonBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonBrowse.Location = new System.Drawing.Point(648, 48);
			this.buttonBrowse.Name = "buttonBrowse";
			this.buttonBrowse.TabIndex = 2;
			this.buttonBrowse.Text = "Browse...";
			this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
			// 
			// buttonIndex
			// 
			this.buttonIndex.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonIndex.Location = new System.Drawing.Point(16, 80);
			this.buttonIndex.Name = "buttonIndex";
			this.buttonIndex.Size = new System.Drawing.Size(112, 23);
			this.buttonIndex.TabIndex = 3;
			this.buttonIndex.Text = "Rebuild the index";
			this.buttonIndex.Click += new System.EventHandler(this.buttonIndex_Click);
			// 
			// labelStatus
			// 
			this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.labelStatus.Location = new System.Drawing.Point(16, 128);
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.Size = new System.Drawing.Size(736, 40);
			this.labelStatus.TabIndex = 4;
			// 
			// textBoxQuery
			// 
			this.textBoxQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxQuery.Location = new System.Drawing.Point(16, 176);
			this.textBoxQuery.Name = "textBoxQuery";
			this.textBoxQuery.Size = new System.Drawing.Size(648, 20);
			this.textBoxQuery.TabIndex = 5;
			this.textBoxQuery.Text = "";
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
																							  this.columnHeaderFolder,
																							  this.columnHeaderScore});
			this.listViewResults.FullRowSelect = true;
			this.listViewResults.Location = new System.Drawing.Point(16, 208);
			this.listViewResults.Name = "listViewResults";
			this.listViewResults.Size = new System.Drawing.Size(736, 352);
			this.listViewResults.TabIndex = 7;
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
			// columnHeaderFolder
			// 
			this.columnHeaderFolder.Text = "Folder";
			this.columnHeaderFolder.Width = 120;
			// 
			// columnHeaderScore
			// 
			this.columnHeaderScore.Text = "Score";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.buttonClean);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.buttonBrowse);
			this.groupBox1.Controls.Add(this.textBoxPath);
			this.groupBox1.Controls.Add(this.buttonIndex);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(16, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(736, 112);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Index";
			// 
			// buttonClean
			// 
			this.buttonClean.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonClean.Location = new System.Drawing.Point(136, 80);
			this.buttonClean.Name = "buttonClean";
			this.buttonClean.TabIndex = 5;
			this.buttonClean.Text = "Clean up";
			this.buttonClean.Click += new System.EventHandler(this.buttonClean_Click);
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(344, 23);
			this.label1.TabIndex = 4;
			this.label1.Text = "Index and search the following folder:";
			// 
			// folderBrowserDialog1
			// 
			this.folderBrowserDialog1.ShowNewFolderButton = false;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(768, 574);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.listViewResults);
			this.Controls.Add(this.buttonSearch);
			this.Controls.Add(this.textBoxQuery);
			this.Controls.Add(this.labelStatus);
			this.Name = "Form1";
			this.Text = "Desktop Search";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.EnableVisualStyles(); 
			Application.DoEvents(); 

			Application.Run(new Form1());
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			this.textBoxPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			this.pathIndex = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DesktopSearch");
			checkIndex();
		}

		private void buttonIndex_Click(object sender, System.EventArgs e)
		{
			indexWriter = new IndexWriter(this.pathIndex, new StandardAnalyzer(), true);

			bytesTotal = 0;
			countTotal = 0;
			countSkipped = 0;

			enableControls(false);

			DirectoryInfo di = new DirectoryInfo(this.textBoxPath.Text);
			
			DateTime start = DateTime.Now;
			
			addFolder(di);

			string summary = String.Format("Done. Indexed {0} files ({1} bytes). Skipped {2} files.", countTotal, bytesTotal, countSkipped);
			summary += String.Format(" Took {0}", (DateTime.Now - start));
			status(summary);
			enableControls(true);

			indexWriter.Optimize();
			indexWriter.Close();
		}

		/// <summary>
		/// Turns the controls on or off.
		/// </summary>
		/// <param name="enable"></param>
		private void enableControls(bool enable)
		{
			this.textBoxPath.Enabled = enable;
			this.buttonIndex.Enabled = enable;
			this.buttonBrowse.Enabled = enable;
			this.buttonClean.Enabled = enable;
		}

		
		/// <summary>
		/// Indexes a folder.
		/// </summary>
		/// <param name="directory"></param>
		private void addFolder(DirectoryInfo directory)
		{
			// find all matching files
			foreach (string pattern in patterns)
			{
				foreach (FileInfo fi in directory.GetFiles(pattern))
				{
					// skip temporary office files
					if (fi.Name.StartsWith("~"))
						continue;

					try 
					{
						addOfficeDocument(fi.FullName);

						// update statistics
						this.countTotal++;
						this.bytesTotal += fi.Length;

						// show added file
						status(fi.FullName);
					}
					catch (Exception)
					{
						// parsing and indexing wasn't successful, skipping that file
						this.countSkipped++;
						status("Skipped: " + fi.FullName);
					}
				}
			}

			// add subfolders
			foreach (DirectoryInfo di in directory.GetDirectories())
			{
				addFolder(di);
			}
		}

		/// <summary>
		/// Parses and indexes an IFilter parseable file.
		/// </summary>
		/// <param name="path"></param>
		private void addOfficeDocument(string path)
		{
			Document doc = new Document();
			string filename = Path.GetFileName(path);
	
			doc.Add(Field.UnStored("text", Parser.Parse(path)));
			doc.Add(Field.Keyword("path", path));
			doc.Add(Field.Text("title", filename));
			indexWriter.AddDocument(doc);
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
			if (this.textBoxQuery.Text.Trim(new char[] {' '}) == String.Empty)
				return;

			Query query = QueryParser.Parse(this.textBoxQuery.Text, "text", new StandardAnalyzer()); 

			// Search
			Hits hits = searcher.Search(query);

//			Optionally limit the result count
//			int resultsCount = smallerOf(20, hits.Length());

			for (int i = 0; i < hits.Length(); i++) 
			{
				// get the document from index
				Document doc = hits.Doc(i);

				// create a new row with the result data
				string filename = doc.Get("title");
				string path = doc.Get("path");
				string folder = Path.GetDirectoryName(path);
				DirectoryInfo di = new DirectoryInfo(folder);

				ListViewItem item = new ListViewItem(new string[] {null, filename, di.Name, hits.Score(i).ToString()});
				item.Tag = path;
				item.ImageIndex = imageListDocuments.IconIndex(filename);
				this.listViewResults.Items.Add(item);
				Application.DoEvents();
			} 
			searcher.Close();

			string searchReport = String.Format("Search took {0}. Found {1} items.", (DateTime.Now - start), hits.Length());
			status(searchReport);
		}

		private int smallerOf(int first, int second)
		{
			return first < second ? first : second;
		}

		private void listViewResults_DoubleClick(object sender, System.EventArgs e)
		{
			if (this.listViewResults.SelectedItems.Count != 1)
				return;
			string path = (string) this.listViewResults.SelectedItems[0].Tag;
			Process.Start(path);
		}

		private void textBoxQuery_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				search();

		}

		private void buttonBrowse_Click(object sender, System.EventArgs e)
		{
			this.folderBrowserDialog1.SelectedPath = this.textBoxPath.Text;
			if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				this.textBoxPath.Text = this.folderBrowserDialog1.SelectedPath;
			}
		}

		private void buttonClean_Click(object sender, System.EventArgs e)
		{
			Directory.Delete(this.pathIndex, true);
			checkIndex();
		}


	}
}
