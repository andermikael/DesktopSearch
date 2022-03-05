using System;
using System.Windows.Forms;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using DesktopSearch1.Icons;
using DesktopSearch1.Parsing;
using ZetaLongPaths;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace DesktopSearch1
{
    public partial class IndexForm : Form
    {
        private string pathIndex;
        private IndexWriter indexWriter;
        private string[] patterns = { "*.doc", "*.xls", "*.ppt", "*.htm", "*.txt" };
        private IndexSearcher searcher = null;

        // statistics
        private long bytesTotal = 0;

        private int countTotal = 0;
        private int countSkipped = 0;

        private List<string> RootDirectories;

        /// <summary>
        /// Constructor
        /// </summary>
        public IndexForm()
        {
            InitializeComponent();
            folderBrowserDialog1 = new FolderBrowserDialog();
            RootDirectories = Properties.Settings.Default.RootDirectories;
            PopulateRootDirectories(RootDirectories);
        }

        private void PopulateRootDirectories(List<string> rootDirectories)
        {
            if (rootDirectories == null)
                return;

            RootDirectoriesCombobox.Items.Clear();

            foreach (var r in rootDirectories)
            {
                RootDirectoriesCombobox.Items.Add(r);
            }
        }

        /// <summary>
        /// Event handler for click ontbuttonIndex
        /// </summary>
        /// <param name="sender">Not Used</param>
        /// <param name="e">Not Used</param>
        private void buttonIndex_Click(object sender, System.EventArgs e)
        {
            BuildIndex(true);
        }

        /// <summary>
        /// Build index
        /// </summary>
        /// <param name="create">true if we should create a new index, false if we should add to the current</param>
        private void BuildIndex(bool create)
        {
            bool brc = AddRootDirectory(pathIndex);

            indexWriter = new IndexWriter(pathIndex, new StandardAnalyzer(), create);

            bytesTotal = 0;
            countTotal = 0;
            countSkipped = 0;

            enableControls(false);

            ZlpDirectoryInfo di = new ZlpDirectoryInfo(textBoxPath.Text);

            DateTime start = DateTime.Now;

            addFolder(di);

            string summary = String.Format("Done. Indexed {0} files ({1} bytes). Skipped {2} files.", countTotal, bytesTotal, countSkipped);
            summary += String.Format(" Took {0}", (DateTime.Now - start));
            status(summary);
            enableControls(true);

            indexWriter.Optimize();
            indexWriter.Close();
        }

        private bool AddRootDirectory(string pathIndex)
        {
            bool result = true;

            if (RootDirectories == null)
            {
                RootDirectories = new List<string>();
            }

            if (RootDirectories.Any(e => e.Equals(pathIndex)))
            {
                // directory alredy scanned
                return false;
            }
            RootDirectories.Add(pathIndex);

            // Save it to Settings
            Properties.Settings.Default.RootDirectories = RootDirectories;
            Properties.Settings.Default.Save();

            // update Combobox
            PopulateRootDirectories(RootDirectories);

            return result;
        }

        /// <summary>
        /// Turns the controls on or off.
        /// </summary>
        /// <param name="enable"></param>
        private void enableControls(bool enable)
        {
            textBoxPath.Enabled = enable;
            buttonIndex.Enabled = enable;
            buttonBrowse.Enabled = enable;
            buttonClean.Enabled = enable;
        }

        private void IndexForm_Load(object sender, EventArgs e)
        {
            textBoxPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            pathIndex = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DesktopSearch");
            checkIndex();
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
            labelStatus.Text = msg;

            if (error)
                labelStatus.ForeColor = Color.Red;
            else
                labelStatus.ForeColor = DefaultForeColor;

            Application.DoEvents();
        }

        private void checkIndex()
        {
            try
            {
                searcher = new IndexSearcher(pathIndex);
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

        // <summary>
        /// Indexes a folder.
        /// </summary>
        /// <param name="directory"></param>
        private void addFolder(ZlpDirectoryInfo directory)
        {
            // find all matching files
            foreach (string pattern in patterns)
            {
                foreach (ZlpFileInfo fi in directory.GetFiles(pattern))
                {
                    // skip temporary files
                    if (fi.Name.StartsWith("~"))
                        continue;

                    try
                    {
                        addDocument(fi.FullName);

                        // update statistics
                        countTotal++;
                        bytesTotal += fi.Length;

                        // show added file
                        status(fi.FullName);
                    }
                    catch (Exception)
                    {
                        // parsing and indexing wasn't successful, skipping that file
                        countSkipped++;
                        status("Skipped: " + fi.FullName);
                    }
                }
            }

            // add subfolders
            foreach (ZlpDirectoryInfo di in directory.GetDirectories())
            {
                addFolder(di);
            }
        }

        /// <summary>
        /// Parses and indexes an IFilter parseable file.
        /// </summary>
        /// <param name="path"></param>
        private void addDocument(string path)
        {
            Document doc = new Document();
            string filename = Path.GetFileName(path);
            string extention = Path.GetExtension(path);

            doc.Add(Field.UnStored("text", Parser.Parse(path)));
            doc.Add(Field.Keyword("path", path));
            doc.Add(Field.Text("title", filename));
            doc.Add(Field.Text("ext", extention));
            indexWriter.AddDocument(doc);
        }

        private void buttonBrowse_Click(object sender, System.EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = textBoxPath.Text;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void buttonClean_Click(object sender, System.EventArgs e)
        {
            Directory.Delete(pathIndex, true);
            checkIndex();
        }

        private void AddToIndexButton_Click(object sender, EventArgs e)
        {
            BuildIndex(false);
        }
    }
}