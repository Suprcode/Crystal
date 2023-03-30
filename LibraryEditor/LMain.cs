using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared.Extensions;

namespace LibraryEditor
{
    public partial class LMain : Form
    {
        private readonly Dictionary<int, int> _indexList = new Dictionary<int, int>();
        private MLibraryV2 _library, _referenceLibrary, _shadowLibrary;
        private MLibraryV2.MImage _selectedImage, _exportImage;
        private Image _originalImage;
        public Bitmap _referenceImage;

        protected bool ImageTabActive = true;
        protected bool MaskTabActive = false;
        protected bool FrameTabActive = false;

        public bool ApplyOffsets => checkBox1.Checked;

        protected string ViewMode = "Image";

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        public LMain()
        {
            InitializeComponent();

            this.FrameAction.ValueType = typeof(MirAction);
            this.FrameAction.DataSource = Enum.GetValues(typeof(MirAction));


            SendMessage(PreviewListView.Handle, 4149, 0, 5242946); //80 x 66

            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);

            if (Program.openFileWith.Length > 0 && File.Exists(Program.openFileWith))
            {
                OpenLibrary(Program.openFileWith);
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (Path.GetExtension(files[0]).ToUpper() == ".WIL" ||
                Path.GetExtension(files[0]).ToUpper() == ".WZL" ||
                Path.GetExtension(files[0]).ToUpper() == ".MIZ")
            {
                toolStripProgressBar.Maximum = files.Length;
                toolStripProgressBar.Value = 0;

                new Action(() =>
                {
                    try
                    {
                        ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = 8 };
                        Parallel.For(0, files.Length, options, i =>
                        {
                            if (Path.GetExtension(files[i]) == ".wtl")
                            {
                                WTLLibrary WTLlib = new WTLLibrary(files[i]);
                                WTLlib.ToMLibrary();
                            }
                            else
                            {
                                WeMadeLibrary WILlib = new WeMadeLibrary(files[i]);
                                WILlib.ToMLibrary();
                            }

                            Invoke(new Action(() =>
                            {
                                toolStripProgressBar.Value++;
                            }));

                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }

                    Invoke(new Action(() =>
                    {
                        toolStripProgressBar.Value = 0;
                    }));

                    MessageBox.Show(
                        string.Format("Successfully converted {0} {1}",
                            (files.Length).ToString(),
                            (files.Length > 1) ? "libraries" : "library"));
                }).BeginInvoke(null, null);
            }
            else if (Path.GetExtension(files[0]).ToUpper() == ".LIB")
            {
                ClearInterface();
                ImageList.Images.Clear();
                PreviewListView.Items.Clear();
                _indexList.Clear();

                if (_library != null) _library.Close();
                _library = new MLibraryV2(files[0]);
                PreviewListView.VirtualListSize = _library.Images.Count;
                PreviewListView.RedrawItems(0, PreviewListView.Items.Count - 1, true);

                // Show .Lib path in application title.
                this.Text = files[0].ToString();
            }
            else
            {
                return;
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void ClearInterface()
        {
            _selectedImage = null;
            ImageBox.Image = null;
            ZoomTrackBar.Value = 1;

            WidthLabel.Text = "<No Image>";
            HeightLabel.Text = "<No Image>";
            numericUpDownX.Value = 0;
            numericUpDownY.Value = 0;
        }

        public static Bitmap AddPaddingToBitmap(Bitmap originalBitmap, int padding)
        {
            int newWidth = originalBitmap.Width + 2 * padding;
            int newHeight = originalBitmap.Height + 2 * padding;

            Bitmap paddedBitmap = new Bitmap(newWidth, newHeight);

            using (Graphics g = Graphics.FromImage(paddedBitmap))
            {
                g.Clear(Color.Transparent);

                int x = (paddedBitmap.Width - originalBitmap.Width) / 2;
                int y = (paddedBitmap.Height - originalBitmap.Height) / 2;

                g.DrawImage(originalBitmap, x, y);
            }

            return paddedBitmap;
        }

        private void PreviewListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PreviewListView.SelectedIndices.Count == 0)
            {
                ClearInterface();
                return;
            }

            _selectedImage = _library.GetMImage(PreviewListView.SelectedIndices[0]);

            if (_selectedImage == null)
            {
                ClearInterface();
                return;
            }

            WidthLabel.Text = _selectedImage.Width.ToString();
            HeightLabel.Text = _selectedImage.Height.ToString();

            numericUpDownX.Value = _selectedImage.X;
            numericUpDownY.Value = _selectedImage.Y;

            Bitmap referenceImage = null;
            MLibraryV2.MImage referenceMImage = null;
            if (_referenceLibrary != null)
            {
                referenceMImage = _referenceLibrary.GetMImage(PreviewListView.SelectedIndices[0]);
                if (referenceMImage != null)
                {
                    referenceImage = referenceMImage.Image;
                }
            }

            Bitmap image = null;
            if (ViewMode == "Image")
                image = _selectedImage.Image;
            else
                image = _selectedImage.MaskImage;

            if (image == null)
            {
                ImageBox.Image = null;
                return;
            }

            Bitmap newImage = null;
            if (!ApplyOffsets)
            {
                newImage = new Bitmap(Math.Max(_referenceImage?.Width ?? 0, Math.Max(image.Width, referenceImage?.Width ?? 0)), Math.Max(_referenceImage?.Height ?? 0, Math.Max(image.Height, referenceImage?.Height ?? 0)));
                using (var g = Graphics.FromImage(newImage))
                {
                    if (_referenceImage != null)
                        g.DrawImage(_referenceImage, Point.Empty);
                    if (referenceImage != null)
                        g.DrawImage(referenceImage, Point.Empty);
                    g.DrawImage(image, Point.Empty);
                }
            }
            else
            {
                var maxWidth = Math.Max(image.Width, referenceImage?.Width ?? 0);
                var maxHeight = Math.Max(image.Height, referenceImage?.Height ?? 0);

                int offsetX = 0;
                int offsetY = 0;
                if (referenceImage != null)
                {
                    offsetX = -_selectedImage.X + referenceMImage.X;
                    offsetY = -_selectedImage.Y + referenceMImage.Y;
                }
                maxWidth += Math.Abs(offsetX);
                maxHeight += Math.Abs(offsetY);

                newImage = new Bitmap(maxWidth, maxHeight);
                using (var g = Graphics.FromImage(newImage))
                {
                    if (referenceImage != null)
                        g.DrawImage(referenceImage, new Point(offsetX > 0 ? offsetX : 0, offsetY > 0 ? offsetY : 0));
                    g.DrawImage(image, new Point(offsetX < 0 ? Math.Abs(offsetX) : 0, offsetY < 0 ? Math.Abs(offsetY) : 0));
                }

                if (_referenceImage != null)
                {
                    var newMaxWidth = Math.Max(_referenceImage.Width, newImage.Width + Math.Abs(_selectedImage.X));
                    var newMaxHeight = Math.Max(_referenceImage.Height, newImage.Height + Math.Abs(_selectedImage.Y));

                    var anotherNewBitmap = new Bitmap(newMaxWidth, newMaxHeight);
                    using (var g = Graphics.FromImage(anotherNewBitmap))
                    {
                        g.DrawImage(_referenceImage, new Point(_selectedImage.X < 0 ? Math.Abs(_selectedImage.X) : 0, _selectedImage.Y < 0 ? Math.Abs(_selectedImage.Y) : 0));
                        g.DrawImage(image, new Point(_selectedImage.X > 0 ? _selectedImage.X : 0, _selectedImage.Y > 0 ? _selectedImage.Y : 0));
                    }
                    newImage = anotherNewBitmap;
                }
            }

            ImageBox.Image = newImage;
            int globalOffsetX = _referenceImage != null ? 0 : referenceMImage?.X ?? _selectedImage.X;
            int globalOffsetY = _referenceImage != null ? 0 : referenceMImage?.Y ?? _selectedImage.Y;

            ImageBox.Location = ApplyOffsets ? new Point(100 + globalOffsetX, 100 + globalOffsetY) : Point.Empty;

            // Keep track of what image/s are selected.
            if (PreviewListView.SelectedIndices.Count > 1)
            {
                toolStripStatusLabel.ForeColor = Color.Red;
                toolStripStatusLabel.Text = "Multiple images selected.";
            }
            else
            {
                toolStripStatusLabel.ForeColor = SystemColors.ControlText;
                toolStripStatusLabel.Text = "Selected Image: " + string.Format("{0} / {1}",
                PreviewListView.SelectedIndices[0].ToString(),
                (PreviewListView.Items.Count - 1).ToString());
            }

            nudJump.Value = PreviewListView.SelectedIndices[0];
        }

        private void PreviewListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            int index;

            if (_indexList.TryGetValue(e.ItemIndex, out index))
            {
                e.Item = new ListViewItem { ImageIndex = index, Text = e.ItemIndex.ToString() };
                return;
            }

            _indexList.Add(e.ItemIndex, ImageList.Images.Count);
            ImageList.Images.Add(_library.GetPreview(e.ItemIndex));
            e.Item = new ListViewItem { ImageIndex = index, Text = e.ItemIndex.ToString() };
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (_library == null) return;
            if (_library.FileName == null) return;

            if (ImportImageDialog.ShowDialog() != DialogResult.OK) return;

            List<string> fileNames = new List<string>(ImportImageDialog.FileNames);

            //fileNames.Sort();
            toolStripProgressBar.Value = 0;
            toolStripProgressBar.Maximum = fileNames.Count;

            for (int i = 0; i < fileNames.Count; i++)
            {
                string fileName = fileNames[i];
                Bitmap image;

                try
                {
                    image = new Bitmap(fileName);
                }
                catch
                {
                    continue;
                }

                fileName = Path.Combine(Path.GetDirectoryName(fileName), "Placements", Path.GetFileNameWithoutExtension(fileName));
                fileName = Path.ChangeExtension(fileName, ".txt");

                short x = 0;
                short y = 0;

                if (File.Exists(fileName))
                {
                    string[] placements = File.ReadAllLines(fileName);

                    if (placements.Length > 0)
                        short.TryParse(placements[0], out x);
                    if (placements.Length > 1)
                        short.TryParse(placements[1], out y);
                }

                _library.AddImage(image, x, y);
                toolStripProgressBar.Value++;
                //image.Dispose();
            }

            PreviewListView.VirtualListSize = _library.Images.Count;
            toolStripProgressBar.Value = 0;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveLibraryDialog.ShowDialog() != DialogResult.OK) return;

            if (_library != null) _library.Close();
            _library = new MLibraryV2(SaveLibraryDialog.FileName);
            PreviewListView.VirtualListSize = 0;
            _library.Save();

            UpdateFrameGridView();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenLibraryDialog.ShowDialog() != DialogResult.OK) return;

            _referenceLibrary = null;
            _referenceImage = null;
            OpenLibrary(OpenLibraryDialog.FileName);
        }

        private void OpenLibrary(string filename)
        {
            ClearInterface();
            ImageList.Images.Clear();
            PreviewListView.Items.Clear();
            _indexList.Clear();

            if (_library != null) _library.Close();
            _library = new MLibraryV2(filename);
            PreviewListView.VirtualListSize = _library.Images.Count;

            // Show .Lib path in application title.
            this.Text = filename;

            PreviewListView.SelectedIndices.Clear();

            if (PreviewListView.Items.Count > 0)
                PreviewListView.Items[0].Selected = true;

            UpdateFrameGridView();
        }

        private void OpenReferenceLibrary(string filename)
        {
            if (_referenceLibrary != null) _referenceLibrary.Close();
            _referenceLibrary = new MLibraryV2(filename);
        }

        private void OpenShadowLibraryAndImport(string filename)
        {
            if (_library == null) return;

            if (_shadowLibrary != null) _shadowLibrary.Close();
            _shadowLibrary = new MLibraryV2(filename);

            ImageList.Images.Clear();
            _indexList.Clear();

            for (int i = 0; i < _library.Images.Count; i++)
            {
                var mImage = _library.GetMImage(i);
                if (mImage == null || mImage.Image == null) continue;

                var shadowImage = _shadowLibrary.GetMImage(i);
                if (shadowImage == null || shadowImage.Image == null) continue;

                var offSetX = -mImage.X + shadowImage.X;
                var offSetY = -mImage.Y + shadowImage.Y;

                var maxWidth = Math.Max(mImage.Width, shadowImage.Width + Math.Abs(offSetX));
                var maxHeight = Math.Max(mImage.Height, shadowImage.Height + Math.Abs(offSetY));

                var newBitmap = new Bitmap(maxWidth, maxHeight);
                using (var g = Graphics.FromImage(newBitmap))
                {
                    g.DrawImage(mImage.Image, new Point(offSetX < 0 ? Math.Abs(offSetX) : 0, offSetY < 0 ? Math.Abs(offSetY) : 0));
                    g.DrawImage(shadowImage.Image, new Point(offSetX > 0 ? offSetX : 0, offSetY > 0 ? offSetY : 0));
                }

                _library.ReplaceImage(i, newBitmap, mImage.X, mImage.Y);
            }

            PreviewListView.VirtualListSize = _library.Images.Count;

            try
            {
                PreviewListView.RedrawItems(0, PreviewListView.Items.Count - 1, true);

                if (ViewMode == "Image")
                {
                    ImageBox.Image = _library.Images[PreviewListView.SelectedIndices[0]].Image;
                }
                else
                {
                    ImageBox.Image = _library.Images[PreviewListView.SelectedIndices[0]].MaskImage;
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_library == null) return;

            UpdateFrameGridData();

            _library.Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_library == null) return;
            if (SaveLibraryDialog.ShowDialog() != DialogResult.OK) return;

            UpdateFrameGridData();

            _library.FileName = SaveLibraryDialog.FileName;
            _library.Save();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (_library == null) return;
            if (_library.FileName == null) return;
            if (PreviewListView.SelectedIndices.Count == 0) return;

            if (MessageBox.Show("Are you sure you want to delete the selected Image?",
                "Delete Selected.",
                MessageBoxButtons.YesNoCancel) != DialogResult.Yes) return;

            List<int> removeList = new List<int>();

            for (int i = 0; i < PreviewListView.SelectedIndices.Count; i++)
                removeList.Add(PreviewListView.SelectedIndices[i]);

            removeList.Sort();

            for (int i = removeList.Count - 1; i >= 0; i--)
                _library.RemoveImage(removeList[i]);

            ImageList.Images.Clear();
            _indexList.Clear();
            PreviewListView.VirtualListSize -= removeList.Count;
        }

        private void convertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenWeMadeDialog.ShowDialog() != DialogResult.OK) return;

            toolStripProgressBar.Maximum = OpenWeMadeDialog.FileNames.Length;
            toolStripProgressBar.Value = 0;

            try
            {
                ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = 8 };
                Parallel.For(0, OpenWeMadeDialog.FileNames.Length, options, i =>
                            {
                                if (Path.GetExtension(OpenWeMadeDialog.FileNames[i]) == ".wtl")
                                {
                                    WTLLibrary WTLlib = new WTLLibrary(OpenWeMadeDialog.FileNames[i]);
                                    WTLlib.ToMLibrary();
                                }
                                else if (Path.GetExtension(OpenWeMadeDialog.FileNames[i]) == ".Lib")
                                {
                                    MLibraryV1 v1Lib = new MLibraryV1(OpenWeMadeDialog.FileNames[i]);
                                    v1Lib.ToMLibrary();
                                }
                                else
                                {
                                    WeMadeLibrary WILlib = new WeMadeLibrary(OpenWeMadeDialog.FileNames[i]);
                                    WILlib.ToMLibrary();
                                }
                                toolStripProgressBar.Value++;
                            });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            toolStripProgressBar.Value = 0;

            MessageBox.Show(string.Format("Successfully converted {0} {1}",
                (OpenWeMadeDialog.FileNames.Length).ToString(),
                (OpenWeMadeDialog.FileNames.Length > 1) ? "libraries" : "library"));
        }

        private void copyToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PreviewListView.SelectedIndices.Count == 0) return;
            if (SaveLibraryDialog.ShowDialog() != DialogResult.OK) return;

            MLibraryV2 tempLibrary = new MLibraryV2(SaveLibraryDialog.FileName);

            List<int> copyList = new List<int>();

            for (int i = 0; i < PreviewListView.SelectedIndices.Count; i++)
                copyList.Add(PreviewListView.SelectedIndices[i]);

            copyList.Sort();

            for (int i = 0; i < copyList.Count; i++)
            {
                MLibraryV2.MImage image = _library.GetMImage(copyList[i]);
                tempLibrary.AddImage(image.Image, image.MaskImage, image.X, image.Y);
            }

            tempLibrary.Save();
        }

        private void removeBlanksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove the blank images?",
                "Remove Blanks",
                MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            _library.RemoveBlanks();
            ImageList.Images.Clear();
            _indexList.Clear();
            PreviewListView.VirtualListSize = _library.Count;
        }

        private void countBlanksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenLibraryDialog.Multiselect = true;

            if (OpenLibraryDialog.ShowDialog() != DialogResult.OK)
            {
                OpenLibraryDialog.Multiselect = false;
                return;
            }

            OpenLibraryDialog.Multiselect = false;

            MLibraryV2.Load = false;

            int count = 0;

            for (int i = 0; i < OpenLibraryDialog.FileNames.Length; i++)
            {
                MLibraryV2 library = new MLibraryV2(OpenLibraryDialog.FileNames[i]);

                for (int x = 0; x < library.Count; x++)
                {
                    if (library.Images[x].Length <= 8)
                        count++;
                }

                library.Close();
            }

            MLibraryV2.Load = true;
            MessageBox.Show(count.ToString());
        }

        private void InsertImageButton_Click(object sender, EventArgs e)
        {
            if (_library == null) return;
            if (_library.FileName == null) return;
            if (PreviewListView.SelectedIndices.Count == 0) return;
            if (ImportImageDialog.ShowDialog() != DialogResult.OK) return;

            List<string> fileNames = new List<string>(ImportImageDialog.FileNames);

            //fileNames.Sort();

            int index = PreviewListView.SelectedIndices[0];

            toolStripProgressBar.Value = 0;
            toolStripProgressBar.Maximum = fileNames.Count;

            for (int i = fileNames.Count - 1; i >= 0; i--)
            {
                string fileName = fileNames[i];

                Bitmap image;

                try
                {
                    image = new Bitmap(fileName);
                }
                catch
                {
                    continue;
                }

                fileName = Path.Combine(Path.GetDirectoryName(fileName), "Placements", Path.GetFileNameWithoutExtension(fileName));
                fileName = Path.ChangeExtension(fileName, ".txt");

                short x = 0;
                short y = 0;

                if (File.Exists(fileName))
                {
                    string[] placements = File.ReadAllLines(fileName);

                    if (placements.Length > 0)
                        short.TryParse(placements[0], out x);
                    if (placements.Length > 1)
                        short.TryParse(placements[1], out y);
                }

                _library.InsertImage(index, image, x, y, checkboxRemoveBlackOnImport.Checked);

                toolStripProgressBar.Value++;
            }

            ImageList.Images.Clear();
            _indexList.Clear();
            PreviewListView.VirtualListSize = _library.Images.Count;
            toolStripProgressBar.Value = 0;
            _library.Save();
        }

        private void safeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove the blank images?",
                "Remove Blanks", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            _library.RemoveBlanks(true);
            ImageList.Images.Clear();
            _indexList.Clear();
            PreviewListView.VirtualListSize = _library.Count;
        }

        private const int HowDeepToScan = 6;

        public static void ProcessDir(string sourceDir, int recursionLvl, string outputDir)
        {
            if (recursionLvl <= HowDeepToScan)
            {
                // Process the list of files found in the directory.
                string[] fileEntries = Directory.GetFiles(sourceDir);
                foreach (string fileName in fileEntries)
                {
                    if (Directory.Exists(outputDir) != true) Directory.CreateDirectory(outputDir);
                    MLibraryV0 OldLibrary = new MLibraryV0(fileName);
                    MLibraryV2 NewLibrary = new MLibraryV2(outputDir + Path.GetFileName(fileName)) { Images = new List<MLibraryV2.MImage>(), IndexList = new List<int>(), Count = OldLibrary.Images.Count }; ;
                    for (int i = 0; i < OldLibrary.Images.Count; i++)
                        NewLibrary.Images.Add(null);
                    for (int j = 0; j < OldLibrary.Images.Count; j++)
                    {
                        MLibraryV0.MImage oldimage = OldLibrary.GetMImage(j);
                        NewLibrary.Images[j] = new MLibraryV2.MImage(oldimage.FBytes, oldimage.Width, oldimage.Height) { X = oldimage.X, Y = oldimage.Y };
                    }
                    NewLibrary.Save();
                    for (int i = 0; i < NewLibrary.Images.Count; i++)
                    {
                        if (NewLibrary.Images[i].Preview != null)
                            NewLibrary.Images[i].Preview.Dispose();
                        if (NewLibrary.Images[i].Image != null)
                            NewLibrary.Images[i].Image.Dispose();
                        if (NewLibrary.Images[i].MaskImage != null)
                            NewLibrary.Images[i].MaskImage.Dispose();
                    }
                    for (int i = 0; i < OldLibrary.Images.Count; i++)
                    {
                        if (OldLibrary.Images[i].Preview != null)
                            OldLibrary.Images[i].Preview.Dispose();
                        if (OldLibrary.Images[i].Image != null)
                            OldLibrary.Images[i].Image.Dispose();
                    }
                    NewLibrary.Images.Clear();
                    NewLibrary.IndexList.Clear();
                    OldLibrary.Images.Clear();
                    OldLibrary.IndexList.Clear();
                    NewLibrary.Close();
                    OldLibrary.Close();
                    NewLibrary = null;
                    OldLibrary = null;
                }

                // Recurse into subdirectories of this directory.
                string[] subdirEntries = Directory.GetDirectories(sourceDir);
                foreach (string subdir in subdirEntries)
                {
                    // Do not iterate through re-parse points.
                    if (Path.GetFileName(Path.GetFullPath(subdir).TrimEnd(Path.DirectorySeparatorChar)) == Path.GetFileName(Path.GetFullPath(outputDir).TrimEnd(Path.DirectorySeparatorChar))) continue;
                    if ((File.GetAttributes(subdir) &
                         FileAttributes.ReparsePoint) !=
                             FileAttributes.ReparsePoint)
                        ProcessDir(subdir, recursionLvl + 1, outputDir + " \\" + Path.GetFileName(Path.GetFullPath(subdir).TrimEnd(Path.DirectorySeparatorChar)) + "\\");
                }
            }
        }

        // Export a single image.
        private void ExportButton_Click(object sender, EventArgs e)
        {
            if (_library == null) return;
            if (_library.FileName == null) return;
            if (PreviewListView.SelectedIndices.Count == 0) return;

            string _fileName = Path.GetFileName(OpenLibraryDialog.FileName);
            string _newName = _fileName.Remove(_fileName.IndexOf('.'));
            string _folder = Application.StartupPath + "\\Exported\\" + _newName + "\\";

            Bitmap blank = new Bitmap(1, 1);

            // Create the folder if it doesn't exist.
            (new FileInfo(_folder)).Directory.Create();

            ListView.SelectedIndexCollection _col = PreviewListView.SelectedIndices;

            toolStripProgressBar.Value = 0;
            toolStripProgressBar.Maximum = _col.Count;

            for (int i = _col[0]; i < (_col[0] + _col.Count); i++)
            {
                _exportImage = _library.GetMImage(i);
                if (_exportImage.Image == null)
                {
                    blank.Save(_folder + i.ToString() + ".bmp", ImageFormat.Bmp);
                }
                else
                {
                    _exportImage.Image.Save(_folder + i.ToString() + ".bmp", ImageFormat.Bmp);
                }

                toolStripProgressBar.Value++;

                if (!Directory.Exists(_folder + "/Placements/"))
                    Directory.CreateDirectory(_folder + "/Placements/");

                File.WriteAllLines(_folder + "/Placements/" + i.ToString() + ".txt", new string[] { _exportImage.X.ToString(), _exportImage.Y.ToString() });
            }

            toolStripProgressBar.Value = 0;
            MessageBox.Show("Saving to " + _folder + "...", "Image Saved", MessageBoxButtons.OK);
        }

        // Don't let the splitter go out of sight on resizing.
        private void LMain_Resize(object sender, EventArgs e)
        {
            if (splitContainer1.SplitterDistance <= this.Height - 150) return;
            if (this.Height - 150 > 0)
            {
                splitContainer1.SplitterDistance = this.Height - 150;
            }
        }

        // Resize the image(Zoom).
        private Image ImageBoxZoom(Image image, Size size)
        {
            _originalImage = _selectedImage.Image;
            Bitmap _bmp = new Bitmap(_originalImage, Convert.ToInt32(_originalImage.Width * size.Width), Convert.ToInt32(_originalImage.Height * size.Height));
            Graphics _gfx = Graphics.FromImage(_bmp);
            return _bmp;
        }

        // Zoom in and out.
        private void ZoomTrackBar_Scroll(object sender, EventArgs e)
        {
            if (ImageBox.Image == null)
            {
                ZoomTrackBar.Value = 1;
            }
            if (ZoomTrackBar.Value > 0)
            {
                try
                {
                    PreviewListView.Items[(int)nudJump.Value].EnsureVisible();

                    Bitmap _newBMP = new Bitmap(_selectedImage.Width * ZoomTrackBar.Value, _selectedImage.Height * ZoomTrackBar.Value);
                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(_newBMP))
                    {
                        if (checkBoxPreventAntiAliasing.Checked == true)
                        {
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.CompositingMode = CompositingMode.SourceCopy;
                        }

                        if (checkBoxQuality.Checked == true)
                        {
                            g.InterpolationMode = InterpolationMode.NearestNeighbor;
                        }

                        g.DrawImage(_selectedImage.Image, new Rectangle(0, 0, _newBMP.Width, _newBMP.Height));
                    }
                    ImageBox.Image = _newBMP;

                    toolStripStatusLabel.ForeColor = SystemColors.ControlText;
                    toolStripStatusLabel.Text = "Selected Image: " + string.Format("{0} / {1}",
                        PreviewListView.SelectedIndices[0].ToString(),
                        (PreviewListView.Items.Count - 1).ToString());
                }
                catch
                {
                    return;
                }
            }
        }

        // Swap the image panel background colour Black/White.
        private void pictureBox_Click(object sender, EventArgs e)
        {
            if (panel.BackColor == Color.Black)
            {
                panel.BackColor = Color.GhostWhite;
            }
            else
            {
                panel.BackColor = Color.Black;
            }
        }

        private void PreviewListView_VirtualItemsSelectionRangeChanged(object sender, ListViewVirtualItemsSelectionRangeChangedEventArgs e)
        {
            // Keep track of what image/s are selected.
            ListView.SelectedIndexCollection _col = PreviewListView.SelectedIndices;

            if (_col.Count > 1)
            {
                toolStripStatusLabel.ForeColor = Color.Red;
                toolStripStatusLabel.Text = "Multiple images selected.";
            }
        }

        private void buttonReplace_Click(object sender, EventArgs e)
        {
            if (_library == null) return;
            if (_library.FileName == null) return;
            if (PreviewListView.SelectedIndices.Count == 0) return;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();

            if (ofd.FileName == "") return;

            Bitmap newBmp = new Bitmap(ofd.FileName);

            ImageList.Images.Clear();
            _indexList.Clear();
            _library.ReplaceImage(PreviewListView.SelectedIndices[0], newBmp, 0, 0);
            PreviewListView.VirtualListSize = _library.Images.Count;

            try
            {
                PreviewListView.RedrawItems(0, PreviewListView.Items.Count - 1, true);

                if (ViewMode == "Image")
                {
                    ImageBox.Image = _library.Images[PreviewListView.SelectedIndices[0]].Image;
                }
                else
                {
                    ImageBox.Image = _library.Images[PreviewListView.SelectedIndices[0]].MaskImage;
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private void previousImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (PreviewListView.Visible && PreviewListView.Items.Count > 0)
                {
                    int index = PreviewListView.SelectedIndices[0];
                    index = index - 1;
                    PreviewListView.SelectedIndices.Clear();
                    this.PreviewListView.Items[index].Selected = true;
                    PreviewListView.Items[index].EnsureVisible();

                    if (_selectedImage.Height == 1 && _selectedImage.Width == 1 && PreviewListView.SelectedIndices[0] != 0)
                    {
                        previousImageToolStripMenuItem_Click(null, null);
                    }
                }
            }
            catch (Exception)
            {
                PreviewListView.SelectedIndices.Clear();
                this.PreviewListView.Items[PreviewListView.Items.Count - 1].Selected = true;
            }
        }

        private void nextImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (PreviewListView.Visible && PreviewListView.Items.Count > 0)
                {
                    int index = PreviewListView.SelectedIndices[0];
                    index = index + 1;
                    PreviewListView.SelectedIndices.Clear();
                    this.PreviewListView.Items[index].Selected = true;
                    PreviewListView.Items[index].EnsureVisible();

                    if (_selectedImage.Height == 1 && _selectedImage.Width == 1 && PreviewListView.SelectedIndices[0] != 0)
                    {
                        nextImageToolStripMenuItem_Click(null, null);
                    }
                }
            }
            catch (Exception)
            {
                PreviewListView.SelectedIndices.Clear();
                this.PreviewListView.Items[0].Selected = true;
            }
        }

        // Move Left and Right through images.
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!ImageTabActive) return false;

            if (keyData == Keys.Left)
            {
                previousImageToolStripMenuItem_Click(null, null);
                return true;
            }

            if (keyData == Keys.Right)
            {
                nextImageToolStripMenuItem_Click(null, null);
                return true;
            }

            if (keyData == Keys.Up) //Not 100% accurate but works for now.
            {
                double d = Math.Floor((double)(PreviewListView.Width / 67));
                int index = PreviewListView.SelectedIndices[0] - (int)d;

                PreviewListView.SelectedIndices.Clear();
                if (index < 0)
                    index = 0;

                this.PreviewListView.Items[index].Selected = true;

                return true;
            }

            if (keyData == Keys.Down) //Not 100% accurate but works for now.
            {
                double d = Math.Floor((double)(PreviewListView.Width / 67));
                int index = PreviewListView.SelectedIndices[0] + (int)d;

                PreviewListView.SelectedIndices.Clear();
                if (index > PreviewListView.Items.Count - 1)
                    index = PreviewListView.Items.Count - 1;

                this.PreviewListView.Items[index].Selected = true;

                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void buttonSkipNext_Click(object sender, EventArgs e)
        {
            nextImageToolStripMenuItem_Click(null, null);
        }

        private void buttonSkipPrevious_Click(object sender, EventArgs e)
        {
            previousImageToolStripMenuItem_Click(null, null);
        }

        private void checkBoxQuality_CheckedChanged(object sender, EventArgs e)
        {
            ZoomTrackBar_Scroll(null, null);
        }

        private void checkBoxPreventAntiAliasing_CheckedChanged(object sender, EventArgs e)
        {
            ZoomTrackBar_Scroll(null, null);
        }

        private void nudJump_ValueChanged(object sender, EventArgs e)
        {
            if (PreviewListView.Items.Count - 1 >= nudJump.Value)
            {
                PreviewListView.SelectedIndices.Clear();
                PreviewListView.Items[(int)nudJump.Value].Selected = true;
                PreviewListView.Items[(int)nudJump.Value].EnsureVisible();
            }
        }

        private void nudJump_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //Enter key is down.
                if (PreviewListView.Items.Count - 1 >= nudJump.Value)
                {
                    PreviewListView.SelectedIndices.Clear();
                    PreviewListView.Items[(int)nudJump.Value].Selected = true;
                    PreviewListView.Items[(int)nudJump.Value].EnsureVisible();
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        #region Frames

        private void defaultMonsterFramesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _library.Frames.Clear();
            _library.Frames = new FrameSet(FrameSet.DefaultMonsterFrameSet);

            UpdateFrameGridView();
        }

        private void defaultNPCFramesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _library.Frames.Clear();
            _library.Frames = new FrameSet(FrameSet.DefaultNPCFrameSet);

            UpdateFrameGridView();
        }

        private void defaultPlayerFramesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl.SelectedIndex)
            {
                case 0: //Images
                    ImageTabActive = true;
                    MaskTabActive = false;
                    FrameTabActive = false;
                    ImageBox.Location = new Point(0, 0);
                    FrameAnimTimer.Stop();
                    break;
                case 1: //Masks
                    ImageTabActive = false;
                    MaskTabActive = true;
                    FrameTabActive = false;
                    ImageBox.Location = new Point(0, 0);
                    FrameAnimTimer.Stop();
                    break;
                case 2: //Frames
                    ImageTabActive = false;
                    MaskTabActive = false;
                    FrameTabActive = true;
                    break;
            }
        }

        private void autofillNpcFramesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FolderLibraryDialog.ShowDialog() != DialogResult.OK) return;

            var path = FolderLibraryDialog.SelectedPath;

            var files = Directory.GetFiles(path, "*.Lib");

            if (MessageBox.Show($"Are you sure you want to populate {files.Count()} Libs with their matching FrameSet?",
                "Autofill Libs.",
                MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            foreach (var file in files)
            {
                if (_library != null) _library.Close();
                _library = new MLibraryV2(file);

                // Show .Lib path in application title.
                this.Text = file;

                var name = Path.GetFileNameWithoutExtension(file);

                if (!int.TryParse(name, out int imageNumber)) continue;

                _library.Frames = GetFrameSetByImage((Monster)imageNumber);
                _library.Save();
            }
        }

        private void frameGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            frameGridView.Rows[e.RowIndex].ErrorText = "";

            if (frameGridView.Rows[e.RowIndex].IsNewRow) { return; }

            if (e.ColumnIndex >= 1 && e.ColumnIndex <= 8)
            {
                if (!int.TryParse(e.FormattedValue.ToString(), out _))
                {
                    e.Cancel = true;
                    frameGridView.Rows[e.RowIndex].ErrorText = "the value must be an integer";
                }
            }
        }

        private void frameGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["FrameStart"].Value = 0;
            e.Row.Cells["FrameCount"].Value = 0;
            e.Row.Cells["FrameSkip"].Value = 0;
            e.Row.Cells["FrameInterval"].Value = 0;
            e.Row.Cells["FrameEffectStart"].Value = 0;
            e.Row.Cells["FrameEffectCount"].Value = 0;
            e.Row.Cells["FrameEffectSkip"].Value = 0;
            e.Row.Cells["FrameEffectInterval"].Value = 0;
            e.Row.Cells["FrameReverse"].Value = false;
            e.Row.Cells["FrameBlend"].Value = false;
        }


        private void UpdateFrameGridView()
        {
            frameGridView.Rows.Clear();

            foreach (var action in _library.Frames.Keys)
            {
                var frame = _library.Frames[action];

                int rowIndex = frameGridView.Rows.Add();

                var row = frameGridView.Rows[rowIndex];

                row.Cells["FrameAction"].Value = action;
                row.Cells["FrameStart"].Value = frame.Start;
                row.Cells["FrameCount"].Value = frame.Count;
                row.Cells["FrameSkip"].Value = frame.Skip;
                row.Cells["FrameInterval"].Value = frame.Interval;
                row.Cells["FrameEffectStart"].Value = frame.EffectStart;
                row.Cells["FrameEffectCount"].Value = frame.EffectCount;
                row.Cells["FrameEffectSkip"].Value = frame.EffectSkip;
                row.Cells["FrameEffectInterval"].Value = frame.EffectInterval;
                row.Cells["FrameReverse"].Value = frame.Reverse;
                row.Cells["FrameBlend"].Value = frame.Blend;
            }
        }

        private void UpdateFrameGridData()
        {
            if (_library == null) return;

            _library.Frames.Clear();

            foreach (DataGridViewRow row in frameGridView.Rows)
            {
                var cells = row.Cells;

                if (cells["FrameAction"].Value == null) continue;

                var action = (MirAction)row.Cells["FrameAction"].Value;

                if (_library.Frames.ContainsKey(action))
                {
                    MessageBox.Show(string.Format($"The action '{action}' exists more than once so will not be saved."));
                    continue;
                }

                var frame = new Frame(cells["FrameStart"].Value.ValueOrDefault<int>(),
                                        cells["FrameCount"].Value.ValueOrDefault<int>(),
                                        cells["FrameSkip"].Value.ValueOrDefault<int>(),
                                        cells["FrameInterval"].Value.ValueOrDefault<int>(),
                                        cells["FrameEffectStart"].Value.ValueOrDefault<int>(),
                                        cells["FrameEffectCount"].Value.ValueOrDefault<int>(),
                                        cells["FrameEffectSkip"].Value.ValueOrDefault<int>(),
                                        cells["FrameEffectInterval"].Value.ValueOrDefault<int>())
                {
                    Reverse = cells["FrameReverse"].Value.ValueOrDefault<bool>(),
                    Blend = cells["FrameBlend"].Value.ValueOrDefault<bool>()
                };

                _library.Frames.Add(action, frame);
            }
        }

        private void frameGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            var row = frameGridView.Rows[e.RowIndex];

            if (row == null) return;

            var cells = row.Cells;

            if (cells["FrameAction"].Value == null) return;

            var frame = new Frame(cells["FrameStart"].Value.ValueOrDefault<int>(),
                                        cells["FrameCount"].Value.ValueOrDefault<int>(),
                                        cells["FrameSkip"].Value.ValueOrDefault<int>(),
                                        cells["FrameInterval"].Value.ValueOrDefault<int>(),
                                        cells["FrameEffectStart"].Value.ValueOrDefault<int>(),
                                        cells["FrameEffectCount"].Value.ValueOrDefault<int>(),
                                        cells["FrameEffectSkip"].Value.ValueOrDefault<int>(),
                                        cells["FrameEffectInterval"].Value.ValueOrDefault<int>())
            {
                Reverse = cells["FrameReverse"].Value.ValueOrDefault<bool>(),
                Blend = cells["FrameBlend"].Value.ValueOrDefault<bool>()
            };

            if (frame.Interval == 0) return;

            _drawFrame = frame;

            FrameAnimTimer.Interval = frame.Interval;
            FrameAnimTimer.Start();
        }

        private Frame _drawFrame;
        private int _currentFrame;
        private MirDirection _currentDirection;

        private void FrameAnimTimer_Tick(object sender, EventArgs e)
        {
            if (_drawFrame == null) return;

            try
            {
                if (_currentFrame >= _drawFrame.Count - 1)
                {
                    _currentFrame = 0;
                    MirDirection[] arr = (MirDirection[])Enum.GetValues(typeof(MirDirection));
                    int j = Array.IndexOf<MirDirection>(arr, _currentDirection) + 1;
                    _currentDirection = (arr.Length == j) ? arr[0] : arr[j];
                }

                var drawFrame = _drawFrame.Start + (_drawFrame.OffSet * (byte)_currentDirection) + _currentFrame;

                _selectedImage = _library.GetMImage(drawFrame);

                if (ViewMode == "Image")
                {
                    ImageBox.Location = new Point(250 + _selectedImage.X, 250 + _selectedImage.Y);
                    ImageBox.Image = _selectedImage.Image;
                }
                else
                {
                    ImageBox.Location = new Point(250 + _selectedImage.X, 250 + _selectedImage.Y);
                    ImageBox.Image = _selectedImage.MaskImage;
                }

                _currentFrame++;
            }
            catch { }
        }

        private void PreviewListViewMask_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void RButtonViewMode_CheckedChanged(object sender, EventArgs e)
        {
            if (RButtonImage.Checked)
            {
                ViewMode = "Image";
            }
            else if (RButtonOverlay.Checked)
            {
                ViewMode = "Overlay";
            }

            if (_selectedImage != null)
            {
                if (ViewMode == "Image")
                {
                    ImageBox.Image = _selectedImage.Image;
                }
                else
                {
                    ImageBox.Image = _selectedImage.MaskImage;
                }
            }
        }

        /// <summary>
        /// List of monsters and matching frames
        /// Method MUST be edited before use. The existing code is only here as an example.
        /// READ THE COMMENTS WITHIN THIS METHOD BEFORE USE
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private FrameSet GetFrameSetByImage(Monster image)
        {
            //REMOVE THE BELOW EXCEPTION ONCE THE DESIRED CODE HAS BEEN ADDED          
            throw new NotImplementedException("The method 'GetFrameSetByImage' must be updated before this function can be used");

            //UNCOMMENT THE CODE BELOW, IT SERVES AS AN EXAMPLE OF HOW TO MATCH IMAGES UP TO THE CORRECT FRAMES
            //List<FrameSet> FrameList = new List<FrameSet>();
            //FrameSet frame;

            ////ADD LIST OF FRAMES (CAN BE COPIED FROM THE CLIENTS FRAME.CS)
            //FrameList.Add(frame = new FrameSet());
            //frame.Add(MirAction.Standing, new Frame(0, 4, 0, 450));
            //frame.Add(MirAction.Harvest, new Frame(12, 10, 0, 200));

            ////ADD SWITCH OF IMAGE TO CORRECT FRAME (CAN BE COPIED FROM THE MONSTEROBJECT.CS FRAME LIST)
            //FrameSet matchingFrame = new FrameSet();
            //switch (image)
            //{
            //    case Monster.Hen:
            //        matchingFrame = FrameList[0];
            //        break;
            //}

            //return matchingFrame;
        }
        #endregion

        private void openReferenceFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenLibraryDialog.ShowDialog() != DialogResult.OK) return;

            OpenReferenceLibrary(OpenLibraryDialog.FileName);
            PreviewListView.Invoke(new EventHandler(PreviewListView_SelectedIndexChanged), EventArgs.Empty);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            PreviewListView.Invoke(new EventHandler(PreviewListView_SelectedIndexChanged), EventArgs.Empty);
        }

        private void importShadowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenLibraryDialog.ShowDialog() != DialogResult.OK) return;
            OpenShadowLibraryAndImport(OpenLibraryDialog.FileName);
        }

        private void openReferenceImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_library == null) return;
            if (_library.FileName == null) return;

            if (ImportImageDialog.ShowDialog() != DialogResult.OK) return;

            string fileName = ImportImageDialog.FileNames[0];
            _referenceImage = new Bitmap(fileName);
        }

        private void BulkButton_Click(object sender, EventArgs e)
        {
            // Create an instance of the InputDialog class
            InputDialog dlg = new InputDialog();

            // Show the dialog as a modal dialog
            DialogResult result = dlg.ShowDialog();

            // If the user clicked the Ok button, retrieve the values entered by the user
            if (result == DialogResult.OK)
            {
                for (int i = 0; i < PreviewListView.SelectedIndices.Count; i++)
                {
                    MLibraryV2.MImage image = _library.GetMImage(PreviewListView.SelectedIndices[i]);
                    if (image == null || image.Image == null) continue;
                    image.X += (short)dlg.Value1;
                    image.Y += (short)dlg.Value2;
                }
            }
        }

        private void numericUpDownX_ValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < PreviewListView.SelectedIndices.Count; i++)
            {
                MLibraryV2.MImage image = _library.GetMImage(PreviewListView.SelectedIndices[i]);
                image.X = (short)numericUpDownX.Value;
            }
            PreviewListView.Invoke(new EventHandler(PreviewListView_SelectedIndexChanged), EventArgs.Empty);
        }

        private void numericUpDownY_ValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < PreviewListView.SelectedIndices.Count; i++)
            {
                MLibraryV2.MImage image = _library.GetMImage(PreviewListView.SelectedIndices[i]);
                image.Y = (short)numericUpDownY.Value;
            }
            PreviewListView.Invoke(new EventHandler(PreviewListView_SelectedIndexChanged), EventArgs.Empty);
        }
    }
}