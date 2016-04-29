using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryViewer
{
    public partial class LMain : Form
    {
        private readonly Dictionary<int, int> _indexList = new Dictionary<int, int>();
        private List<Bitmap> _LImageList = new List<Bitmap>();
        private List<string> _NameList = new List<string>();
        private MLibraryV2 _library;
        private string folderName = "";
        private bool showFrontSide = false;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        public LMain()
        {
            InitializeComponent();
            SendMessage(PreviewListView.Handle, 4149, 0, 5242946); //80 x 66 
        }

        private void ClearInterface()
        {
            ImageBox.Image = null;

            WidthLabel.Text = "<No Image>";
            HeightLabel.Text = "<No Image>";
            LibNameLabel.Text = "<No Selection>";
            LibCountLabel.Text = ImageList.Images.Count.ToString();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = LibraryFolderDialog.ShowDialog();
            if (result != DialogResult.OK) return;

            ClearInterface();
            ImageList.Images.Clear();
            _indexList.Clear();
            _LImageList.Clear();
            PreviewListView.VirtualListSize = 0;
            PreviewListView.Items.Clear();

            DebugBox.Clear();

            folderName = LibraryFolderDialog.SelectedPath;

            LoadSettings form = new LoadSettings();
            form.ShowDialog();

            showFrontSide = form.GetFrontSide();

            string Prefix = "";
            if (form.GetManualPrefix())
                Prefix = form.GetPrefix();
            else
                foreach (string file in Directory.EnumerateFiles(folderName, "*.lib"))
                {
                    Prefix = Path.GetFileNameWithoutExtension(file);
                    break;
                }

            //DebugBox.Text += "Showfront: "+showFrontSide.ToString() + "\r\n";
            //DebugBox.Text += "Prefix: "+Prefix + "\r\n";
            Program.LoadFailed = false;

            MessageBox.Show("This can take a while.\n Press 'OK' to Start.");

            
            Stopwatch sw = Stopwatch.StartNew();//Timing
            int folderLength = Directory.GetFiles(folderName, "*.lib").Length;
            for (int i = 0; i < folderLength; i++)
            {
                string PathName = folderName+Path.DirectorySeparatorChar;
                string flName = i.ToString(Prefix) + ".lib";
                string fullname = PathName + flName;

                if (File.Exists(fullname))
                {
                    //DebugBox.Text += fullname + "\r\n";

                    if (_library != null) _library.Close();
                    _library = new MLibraryV2(fullname);
                    if (Program.LoadFailed)
                    {
                        break;
                    }

                    _NameList.Add(Path.GetFileName(fullname));

                    PreviewListView.VirtualListSize = ImageList.Images.Count + 1;
                }
            }
            sw.Stop();//Timing

            LibCountLabel.Text = ImageList.Images.Count.ToString();


            if (ImageList.Images.Count < 1)
                MessageBox.Show("No images seem to be found.\nMake sure you choose the right prefix!");
            else
                MessageBox.Show("Folder processing finally finished.\nTime Taken: " + sw.Elapsed.TotalMilliseconds + "ms");
        }

        private void PreviewListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PreviewListView.SelectedIndices.Count == 0)
            {
                ClearInterface();
                return;
            }
            ImageBox.Image = _LImageList[PreviewListView.SelectedIndices[0]];

            WidthLabel.Text = ImageBox.Image.Width.ToString();
            HeightLabel.Text = ImageBox.Image.Height.ToString();
            LibNameLabel.Text = _NameList[PreviewListView.SelectedIndices[0]];
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

            string dirName = new DirectoryInfo(folderName).Name;
            int getImageIndex = showFrontSide ? 16 : 0;
            switch (dirName)
            {
                case "Monster":
                    switch (e.ItemIndex)
                    {
                        case 79:
                            if (showFrontSide) getImageIndex = 49;
                            else getImageIndex = 9;
                            break;
                        default:
                            if (showFrontSide) getImageIndex = 16;
                            else getImageIndex = 0;
                            break;
                    }
                    break;
                case "AArmour":
                case "AHair":
                case "AHumEffect":
                case "ARArmour":
                case "ARHair":
                case "ARHumEffect":
                case "ARWeapon":
                case "AWeapon":
                case "CArmour":
                case "CHair":
                case "CHumEffect":
                case "CWeapon":
                case "Fishing":
                    switch (e.ItemIndex)
                    {
                        default:
                            if (showFrontSide) getImageIndex = 16;
                            else getImageIndex = 0;
                            break;
                    }
                    break;
                case "Mount":
                    switch (e.ItemIndex)
                    {
                        default:
                            if (showFrontSide) getImageIndex = 16;
                            else getImageIndex = 0;
                            break;
                    }
                    break;
                case "NPC":
                    switch (e.ItemIndex)
                    {
                        default:
                            if (showFrontSide) getImageIndex = 16;
                            else getImageIndex = 0;
                            break;
                    }
                    break;
                case "Pet":
                    switch (e.ItemIndex)
                    {
                        case 1:
                        case 3:
                        case 5:
                            if (showFrontSide) getImageIndex = 16;
                            else getImageIndex = 0;
                            break;
                        default:
                            if (showFrontSide) getImageIndex = 24;
                            else getImageIndex = 0;
                            break;
                    }
                    break;
            }
            if ((_library.Images.Count - 1) < getImageIndex) getImageIndex = 0;//<-- Prevents NullReferenceException

            _LImageList.Add(_library.GetMImage(getImageIndex).Image);
            ImageList.Images.Add(_library.GetPreview(getImageIndex));
            if (_library != null) _library.Close();

            e.Item = new ListViewItem { ImageIndex = index, Text = e.ItemIndex.ToString() };
        }

        private void checkBackground_CheckedChanged(object sender, System.EventArgs e)
        {
            if (checkBackground.Checked)
                ImageBox.BackColor = Color.White;
            else
                ImageBox.BackColor = Color.Black;
        }

        private void checkCenter_CheckedChanged(object sender, System.EventArgs e)
        {
            if (checkCenter.Checked)
                ImageBox.SizeMode = PictureBoxSizeMode.CenterImage;
            else
                ImageBox.SizeMode = PictureBoxSizeMode.Normal;
        }

        private void ExportImagesButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _LImageList.Count; i++)
            {
                Bitmap image = _LImageList[i];
                string filename = _NameList[i];

                string _newName = filename.Replace(".lib", ".bmp");
                string _folder = Application.StartupPath + "\\Exported\\";

                Bitmap blank = new Bitmap(1, 1);

                // Create the folder if it doesn't exist.
                (new FileInfo(_folder)).Directory.Create();

                if (image == null)
                {
                    blank.Save(_folder + _newName, ImageFormat.Bmp);
                }
                else
                {
                    image.Save(_folder + _newName, ImageFormat.Bmp);
                }

            }

            MessageBox.Show("Image export complete.", "Image export", MessageBoxButtons.OK);
        }
    }

    public class SemiNumericPathComparer : IComparer<string>
    {
        public int Compare(string s1, string s2)
        {
            string f1 = Path.GetFileName(s1);
            string f2 = Path.GetFileName(s2);

            if (IsNumeric(f1) && IsNumeric(f2))
            {
                if (Convert.ToInt32(f1) > Convert.ToInt32(f2)) return 1;//higher
                if (Convert.ToInt32(f1) < Convert.ToInt32(f2)) return -1;//lower
                if (Convert.ToInt32(f1) == Convert.ToInt32(f2)) return 0;//equal
            }

            //numeric before string
            if (IsNumeric(f1) && !IsNumeric(f2)) return -1;
            if (!IsNumeric(f1) && IsNumeric(f2)) return 1;

            return string.Compare(f1, f2, true);
        }

        public static bool IsNumeric(object value)
        {
            try
            {
                int i = Convert.ToInt32(value.ToString());
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
