using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManagedSquish;

namespace LibraryEditor
{
    public partial class LMain : Form
    {
        private readonly Dictionary<int, int> _indexList = new Dictionary<int, int>();
        private MLibrary _library;
        private MLibrary.MImage _selectedImage;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        public LMain()
        {
            InitializeComponent();

            SendMessage(PreviewListView.Handle, 4149, 0, 5242946); //80 x 66 
        }

        private void ClearInterface()
        {
            _selectedImage = null;
            ImageBox.Image = null;

            WidthLabel.Text = "<No Image>";
            HeightLabel.Text = "<No Image>";
            OffSetXTextBox.Text = string.Empty;
            OffSetYTextBox.Text = string.Empty;
            OffSetXTextBox.BackColor = SystemColors.Window;
            OffSetYTextBox.BackColor = SystemColors.Window;
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
            OffSetXTextBox.Text = _selectedImage.X.ToString();
            OffSetYTextBox.Text = _selectedImage.Y.ToString();
            ImageBox.Image = _selectedImage.Image;
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
            if (ImportImageDialog.ShowDialog() != DialogResult.OK)
                return;

            List<string> fileNames = new List<string>(ImportImageDialog.FileNames);

            fileNames.Sort();


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
            }

            PreviewListView.VirtualListSize = _library.Images.Count;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveLibraryDialog.ShowDialog() != DialogResult.OK) return;

            if (_library != null) _library.Close();
            _library = new MLibrary(SaveLibraryDialog.FileName);
            PreviewListView.VirtualListSize = 0;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenLibraryDialog.ShowDialog() != DialogResult.OK)
                return;

            ClearInterface();
            ImageList.Images.Clear();
            PreviewListView.Items.Clear();
            _indexList.Clear();

            if (_library != null) _library.Close();
            _library = new MLibrary(OpenLibraryDialog.FileName);
            PreviewListView.VirtualListSize = _library.Images.Count;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_library == null)
                return;
            _library.Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_library == null)
                return;
            if (SaveLibraryDialog.ShowDialog() != DialogResult.OK) return;

            _library.FileName = SaveLibraryDialog.FileName;
            _library.Save();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _library.Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the selected Image?", "Delete Selected.", MessageBoxButtons.YesNoCancel) != DialogResult.Yes) return;

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

        private void convertToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (OpenWeMadeDialog.ShowDialog() != DialogResult.OK) return;


            ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = 8 };

            Parallel.For(0, OpenWeMadeDialog.FileNames.Length, options, i =>
            {
                if (Path.GetExtension(OpenWeMadeDialog.FileNames[i]) == ".wtl")
                {
                    WTLLibrary WTLlib = new WTLLibrary(OpenWeMadeDialog.FileNames[i]);
                    WTLlib.ToMLibrary();
                }
                else
                {
                    WeMadeLibrary WILlib = new WeMadeLibrary(OpenWeMadeDialog.FileNames[i]);
                    WILlib.ToMLibrary();
                }
            });

        }

        private void copyToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PreviewListView.SelectedIndices.Count == 0) return;
            if (SaveLibraryDialog.ShowDialog() != DialogResult.OK) return;

            MLibrary tempLibrary = new MLibrary(SaveLibraryDialog.FileName);

            List<int> copyList = new List<int>();


            for (int i = 0; i < PreviewListView.SelectedIndices.Count; i++)
                copyList.Add(PreviewListView.SelectedIndices[i]);

            copyList.Sort();
            for (int i = 0; i < copyList.Count; i++)
            {
                MLibrary.MImage image = _library.GetMImage(copyList[i]);
                tempLibrary.AddImage(image.Image, image.X, image.Y);
            }

            tempLibrary.Save();
        }

        private void removeBlanksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove the blank images?", "Remove Blanks", MessageBoxButtons.YesNo) != DialogResult.Yes)  return;

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

            MLibrary.Load = false;
            int count = 0;
            for (int i = 0; i < OpenLibraryDialog.FileNames.Length; i++)
            {

                MLibrary library = new MLibrary(OpenLibraryDialog.FileNames[i]);

                for (int x = 0; x < library.Count; x++)
                {
                    if (library.Images[x].Length <= 8)
                        count++;
                }
                library.Close();
            }
            MLibrary.Load = true;

            
            MessageBox.Show(count.ToString());
        }

        private void OffSetXTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox control = sender as TextBox;

            if (control == null || !control.Focused) return;

            short temp;

            if (!short.TryParse(control.Text, out temp))
            {
                control.BackColor = Color.Red;
                return;
            }
            control.BackColor = SystemColors.Window;

            for (int i = 0; i < PreviewListView.SelectedIndices.Count; i++)
            {
                MLibrary.MImage image = _library.GetMImage(PreviewListView.SelectedIndices[i]);
                image.X = temp;
            }
        }

        private void OffSetYTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox control = sender as TextBox;

            if (control == null || !control.Focused) return;

            short temp;

            if (!short.TryParse(control.Text, out temp))
            {
                control.BackColor = Color.Red;
                return;
            }
            control.BackColor = SystemColors.Window;

            for (int i = 0; i < PreviewListView.SelectedIndices.Count; i++)
            {
                MLibrary.MImage image = _library.GetMImage(PreviewListView.SelectedIndices[i]);
                image.Y = temp;
            }
        }

        private void InsertImageButton_Click(object sender, EventArgs e)
        {
            if (PreviewListView.SelectedIndices.Count == 0) return;
            if (ImportImageDialog.ShowDialog() != DialogResult.OK)
                return;

            List<string> fileNames = new List<string>(ImportImageDialog.FileNames);

            fileNames.Sort();
            int index = PreviewListView.SelectedIndices[0];

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

                _library.InsertImage(index, image, x, y);
            }

            ImageList.Images.Clear();
            _indexList.Clear();
            PreviewListView.VirtualListSize = _library.Images.Count;
        }

        private void safeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove the blank images?", "Remove Blanks", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            _library.RemoveBlanks(true);
            ImageList.Images.Clear();
            _indexList.Clear();
            PreviewListView.VirtualListSize = _library.Count;
        }

        private void convertlibsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to convert every .lib file in the folder to version 1?\nThis will break any .lib file that is not version 0!", "Convert lib folder", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            MessageBox.Show("Select any .lib file you want.\nThe code will convert all .lib files in this folder + subfolders.\nThis will take a while.\nYou will get a message when its finished!\nRemember to backup first!");
            if (OpenLibraryDialog.ShowDialog() != DialogResult.OK) return;
            string MainFolder = Path.GetDirectoryName(OpenLibraryDialog.FileName);
            string NewFolder = MainFolder + "\\Converted\\";
            ProcessDir(MainFolder, 0, NewFolder);
            MessageBox.Show("Folder processing finally finished.\n location: " + NewFolder);
        }
        const int HowDeepToScan = 6; 
        public static void ProcessDir(string sourceDir, int recursionLvl, string outputDir)
        {
            if (recursionLvl <= HowDeepToScan)
            {
                // Process the list of files found in the directory. 
                string[] fileEntries = Directory.GetFiles(sourceDir);
                foreach (string fileName in fileEntries)
                {
                    if (Path.GetExtension(fileName) != ".lib") continue;

                    if (Directory.Exists(outputDir) != true) Directory.CreateDirectory(outputDir);
                    MLibraryv0 OldLibrary = new MLibraryv0(fileName);
                    MLibrary NewLibrary = new MLibrary(outputDir + Path.GetFileName(fileName)) { Images = new List<MLibrary.MImage>(), IndexList = new List<int>(), Count = OldLibrary.Images.Count }; ;
                    for (int i = 0; i < OldLibrary.Images.Count; i++)
                        NewLibrary.Images.Add(null);
                    for (int j = 0; j < OldLibrary.Images.Count; j++)
                    {
                        MLibraryv0.MImage oldimage = OldLibrary.GetMImage(j);
                        NewLibrary.Images[j] = new MLibrary.MImage(oldimage.FBytes,oldimage.Width, oldimage.Height) { X = oldimage.X, Y = oldimage.Y };
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
                    // Do not iterate through reparse points
                    if (Path.GetFileName(Path.GetFullPath(subdir).TrimEnd(Path.DirectorySeparatorChar)) == Path.GetFileName(Path.GetFullPath(outputDir).TrimEnd(Path.DirectorySeparatorChar))) continue;
                    if ((File.GetAttributes(subdir) &
                         FileAttributes.ReparsePoint) !=
                             FileAttributes.ReparsePoint)
                    ProcessDir(subdir, recursionLvl + 1, outputDir + " \\" + Path.GetFileName(Path.GetFullPath(subdir).TrimEnd(Path.DirectorySeparatorChar)) + "\\");
                }
            }
        }
    }
}
