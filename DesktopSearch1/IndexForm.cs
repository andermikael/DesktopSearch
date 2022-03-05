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

        /// <summary>
        /// Constructor
        /// </summary>
        public IndexForm()
        {
            InitializeComponent();
            folderBrowserDialog1 = new FolderBrowserDialog();
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
            indexWriter = new IndexWriter(this.pathIndex, new StandardAnalyzer(), create);

            bytesTotal = 0;
            countTotal = 0;
            countSkipped = 0;

            enableControls(false);

            ZlpDirectoryInfo di = new ZlpDirectoryInfo(this.textBoxPath.Text);

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

        private void IndexForm_Load(object sender, EventArgs e)
        {
            this.textBoxPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            this.pathIndex = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DesktopSearch");
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
            this.labelStatus.Text = msg;

            if (error)
                this.labelStatus.ForeColor = Color.Red;
            else
                this.labelStatus.ForeColor = DefaultForeColor;

            Application.DoEvents();
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

            doc.Add(Field.UnStored("text", Parser.Parse(path)));
            doc.Add(Field.Keyword("path", path));
            doc.Add(Field.Text("title", filename));
            indexWriter.AddDocument(doc);
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

        private void AddToIndexButton_Click(object sender, EventArgs e)
        {
            BuildIndex(false);
        }
    }
}