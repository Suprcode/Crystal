using Map_Editor.Properties;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Windows.Forms;

namespace Map_Editor
{
  public class Main : Form
  {
    private const int CellWidth = 48;
    private const int CellHeight = 32;
    private const int Mir2BigTileBlock = 50;
    private const int Mir3BigTileBlock = 30;
    private const int smTileBlock = 60;
    public static System.Drawing.Font font = new System.Drawing.Font("Tahoma", 10f, FontStyle.Bold);
    private static int zoomMIN;
    private static readonly int zoomMAX = Main.zoomMIN = 20;
    public static readonly Stopwatch Timer = Stopwatch.StartNew();
    public static readonly DateTime StartTime = DateTime.Now;
    public static long Time;
    public static long OldTime;
    private static long _fpsTime;
    private static int _fps;
    public static int FPS;
    public static long MoveTime;
    private static readonly Vector2[] vector2S = new Vector2[5];
    private static readonly Vector2[] line = new Vector2[2];
    private static readonly Random random = new Random();
    private static int AutoTileRange;
    private static int AutoTileChanges;
    private readonly Editor _editor = new Editor();
    private readonly Dictionary<int, int> _shandaMir2IndexList = new Dictionary<int, int>();
    private readonly Dictionary<int, int> _shandaMir3IndexList = new Dictionary<int, int>();
    private readonly Dictionary<int, int> _tilesIndexList = new Dictionary<int, int>();
    private readonly Dictionary<int, int> _wemadeMir2IndexList = new Dictionary<int, int>();
    private readonly Dictionary<int, int> _wemadeMir3IndexList = new Dictionary<int, int>();
    private readonly List<CellInfoData> bigTilePoints = new List<CellInfoData>();
    private readonly CellInfoControl cellInfoControl = new CellInfoControl();
    private readonly int[] Mir2BigTilesPreviewIndex = new int[9]
    {
      5,
      15,
      6,
      20,
      0,
      21,
      7,
      17,
      8
    };
    private readonly int[] Mir3BigTilesPreviewIndex1 = new int[9]
    {
      10,
      20,
      11,
      25,
      0,
      26,
      12,
      22,
      13
    };
    private readonly int[] Mir3BigTilesPreviewIndex2 = new int[9]
    {
      18,
      22,
      17,
      26,
      5,
      27,
      16,
      20,
      15
    };
    private readonly List<CellInfoData> smTilePoints = new List<CellInfoData>();
    private readonly int[] smTilesPreviewIndex = new int[9]
    {
      39,
      11,
      15,
      35,
      0,
      19,
      31,
      25,
      23
    };
    public int AnimationCount;
    private CellInfoData[] cellInfoDatas;
    private int cellX;
    private int cellY;
    private int drawY;
    private int drawX;
    private int libIndex;
    private int index;
    private Graphics graphics;
    private bool Grasping;
    private bool keyDown;
    private Main.Layer layer = Main.Layer.None;
    public CellInfo[,] M2CellInfo;
    private MapReader map;
    private string mapFileName;
    private Point mapPoint;
    private int mapWidth;
    private int mapHeight;
    public CellInfo[,] NewCellInfo;
    private CellInfoData[] objectDatas;
    private int OffSetX;
    private int OffSetY;
    private Point p1;
    private Point p2;
    private int selectImageIndex;
    private MLibrary.MImage selectLibMImage;
    private ListItem selectListItem;
    private int selectTilesIndex = -1;
    private ListItem shangdaMir2ListItem;
    private ListItem shangdaMir3ListItem;
    private CellInfoData[] unTemp;
    private CellInfoData[] reTemp;
    private ListItem wemadeMir2ListItem;
    private ListItem wemadeMir3ListItem;
    private bool grid = true;
    public static Point MPoint;
    private Bitmap cellHighlight = new Bitmap(48, 32);
    public int CellSizeX;
    public int CellSizeY;
    public int[,] SelectedCells;
    private MLibrary _library;
    public Bitmap _mainImage;
    private bool pictureBox_loaded = false;
    private IContainer components = (IContainer) null;
    private ToolStrip toolStrip1;
    private ToolStripButton btnOpen;
    private TabControl tabControl1;
    private TabPage tabMap;
    private ToolStripDropDownButton toolStripDropDownButton2;
    private ToolStripMenuItem chkBack;
    private ToolStripMenuItem chkMidd;
    private ToolStripMenuItem chkFront;
    private ToolStripMenuItem chkBackMask;
    private ToolStripMenuItem chkFrontMask;
    private ToolStripMenuItem chkDoor;
    private ToolStripMenuItem chkDoorSign;
    private ToolStripMenuItem chkFrontAnimationTag;
    private ToolStripMenuItem chkLightTag;
    private ImageList ShandaMir2ImageList;
    private TabPage tabShandaMir2;
    private ListBox ShandaMir2LibListBox;
    private ListView ShandaMir2LibListView;
    private TabPage tabWemadeMir2;
    private ListBox WemadeMir2LibListBox;
    private ListView WemadeMir2LibListView;
    private ImageList WemadeMir2ImageList;
    private ImageList ShandaMir3ImageList;
    private ImageList WemadeMir3ImageList;
    private TabPage tabWemadeMir3;
    private ListBox WemadeMir3LibListBox;
    private ListView WemadeMir3LibListView;
    private TabPage tabShandaMir3;
    private ListBox ShandaMir3LibListBox;
    private ListView ShandaMir3LibListView;
    private ToolStripMenuItem chkMiddleAnimationTag;
    private ToolStripLabel toolStripLabel1;
    private ToolStripComboBox cmbEditorLayer;
    private PictureBox picWemdeMir2;
    private Label labWemadeMir2OffSetY;
    private Label labeWemadeMir2OffSetX;
    private Label LabWemadeMir2Height;
    private Label LabWemadeMir2Width;
    private Label labshandaMir2OffSetY;
    private Label labShandaMir2OffSetX;
    private Label labShandaMir2Height;
    private Label labShandaMir2Width;
    private PictureBox picShandaMir2;
    private Label labWemadeMir3OffSetY;
    private Label labeWemadeMir3OffSetX;
    private Label LabWemadeMir3Height;
    private Label LabWemadeMir3Width;
    private PictureBox picWemdeMir3;
    private Label labshandaMir3OffSetY;
    private Label labShandaMir3OffSetX;
    private Label labShandaMir3Height;
    private Label labShandaMir3Width;
    private PictureBox picShandaMir3;
    private ToolStripMenuItem chkDrawGrids;
    private TabPage tabObjects;
    private PictureBox picObjects;
    private ImageList ObjectsimageList;
    private ToolStripMenuItem chkFrontTag;
    private Button btnDeleteObjects;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem 撤销ToolStripMenuItem;
    private ToolStripMenuItem 返回ToolStripMenuItem;
    private ToolStripMenuItem btnSetDoor;
    private ToolStripMenuItem btnSetAnimation;
    private ToolStripMenuItem btnSetLight;
    private TabPage tabTiles;
    private Panel MapPanel;
    private ListBox ObjectslistBox;
    private ToolStripMenuItem chkMiddleTag;
    private ToolStripMenuItem chkShowCellInfo;
    private PictureBox picTile;
    private ListView TileslistView;
    private ImageList TilesImageList;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem toolStripMenuItem1;
    private ToolStripMenuItem menuNew;
    private ToolStripMenuItem menuOpen;
    private ToolStripMenuItem menuSave;
    private ToolStripMenuItem toolStripMenuItem2;
    private ToolStripMenuItem menuClearMap;
    private ToolStripMenuItem menuUndo;
    private ToolStripMenuItem menuRedo;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem toolStripMenuItem9;
    private ToolStripMenuItem menuZoomIn;
    private ToolStripMenuItem menuZoomOut;
    private TableLayoutPanel tableLayoutPanel1;
    private VScrollBar vScrollBar;
    private HScrollBar hScrollBar;
    private ToolStripMenuItem menu_DeleteSelectedCellData;
    private ToolStripMenuItem menu_SaveObject;
    private SplitContainer splitContainer1;
    private SplitContainer splitContainer2;
    private ToolStripMenuItem toolsToolStripMenuItem;
    private ToolStripMenuItem menuFreeMemory;
    private ToolStripMenuItem menuJump;
    private ToolStripButton btnNew;
    private ToolStripMenuItem menuInvertMir3Layer;
    private ToolStripMenuItem helpToolStripMenuItem;
    private ToolStripMenuItem menuAbout;
    private ToolStripButton btnMiniMap;
    private ToolStripButton btnJump;
    private ToolStripButton btnFreeMemory;
    private ToolStripButton btnSave;
    private TabPage tabTileCutter;
    private SplitContainer splitContainer3;
    private Button btn_grid;
    private Button btn_vCut;
    private Button btn_load;
    private Label label1;
    private Button btn_up;
    private ComboBox comboBox_cellSize;
    private Button btn_left;
    private Button btn_down;
    private Button btn_right;
    private PictureBox pictureBox_Highlight;
    private PictureBox pictureBox_Grid;
    private PictureBox pictureBox_Image;
    private OpenFileDialog loadImageDialog;
    private SaveFileDialog SaveLibraryDialog;
    private Button btnRefreshList;
    private ToolStripSeparator toolStripSeparator2;
    private ContextMenuStrip contextMenuTileCutter;
    private ToolStripMenuItem menuSelectAllCells;
    private ToolStripMenuItem menuDeselectAllCells;

    public Main()
    {
      this.InitializeComponent();
      this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
      this.SetStyle(ControlStyles.UserPaint, true);
      this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
      Application.Idle += new EventHandler(this.Application_Idle);
      this.pictureBox_Grid.Parent = (Control) this.pictureBox_Image;
      this.pictureBox_Highlight.Parent = (Control) this.pictureBox_Grid;
    }

    private void Application_Idle(object sender, EventArgs e)
    {
      try
      {
        while (Main.AppStillIdle)
        {
          this.UpdateTime();
          this.UpdateEnviroment();
          this.RenderEnviroment();
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void UpdateTime() => Main.Time = Main.Timer.ElapsedMilliseconds;

    private void UpdateEnviroment()
    {
      if (Main.Time >= Main._fpsTime)
      {
        Main._fpsTime = Main.Time + 1000L;
        Main.FPS = Main._fps;
        Main._fps = 0;
      }
      else
        ++Main._fps;
      if (Main.Time >= Main.MoveTime)
      {
        Main.MoveTime += 100L;
        ++this.AnimationCount;
      }
      this.Text = string.Format("FPS: {0}---Map:W {1}:H {2} ----W,S,A,D，观察地图<{3}>", (object) Main.FPS, (object) this.mapWidth, (object) this.mapHeight, (object) this.mapFileName);
    }

    private void RenderEnviroment()
    {
      try
      {
        if (DXManager.DeviceLost)
        {
          DXManager.AttemptReset();
          Thread.Sleep(1);
        }
        else
        {
          if (this.M2CellInfo == null)
            return;
          DXManager.Device.Clear(ClearFlags.Target, Color.White, 0.0f, 0);
          DXManager.Device.BeginScene();
          DXManager.Sprite.Begin(SpriteFlags.AlphaBlend);
          DXManager.TextSprite.Begin(SpriteFlags.AlphaBlend);
          this.OffSetX = this.MapPanel.Width / (48 * Main.zoomMIN / Main.zoomMAX);
          this.OffSetY = this.MapPanel.Height / (32 * Main.zoomMIN / Main.zoomMAX);
          this.DrawBack(this.chkBack.Checked);
          this.DrawMidd(this.chkMidd.Checked);
          this.DrawFront(this.chkFront.Checked);
          this.DrawObject(this.objectDatas);
          this.DrawSelectTextureImage();
          this.DrawLimit();
          this.DrawDoorTag(this.chkDoorSign.Checked);
          this.DrawFrontAnimationTag(this.chkFrontAnimationTag.Checked);
          this.DrawMiddleAnimationTag(this.chkMiddleAnimationTag.Checked);
          this.DrawLightTag(this.chkLightTag.Checked);
          this.DrawBackLimit(this.chkBackMask.Checked);
          this.DrawFrontMask(this.chkFrontMask.Checked);
          this.DrawFrontTag(this.chkFrontTag.Checked);
          this.DrawMiddleTag(this.chkMiddleTag.Checked);
          DXManager.Sprite.End();
          DXManager.TextSprite.End();
          this.DrawGrids(this.chkDrawGrids.Checked);
          this.GraspingRectangle();
          DXManager.Device.EndScene();
          DXManager.Device.Present();
        }
      }
      catch (DeviceLostException ex)
      {
      }
      catch (Exception ex)
      {
        DXManager.AttemptRecovery();
      }
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
    private static extern int SetProcessWorkingSetSize(
      IntPtr process,
      int minimumWorkingSetSize,
      int maximumWorkingSetSize);

    public new void Dispose()
    {
      GC.Collect();
      GC.SuppressFinalize((object) this);
      if (Environment.OSVersion.Platform != PlatformID.Win32NT)
        return;
      Main.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
    }

    public void SetMapSize(int w, int h)
    {
      this.mapWidth = w;
      this.mapHeight = h;
      this.graphics = Graphics.FromHwnd(this.MapPanel.Handle);
    }

    private void Main_Load(object sender, EventArgs e)
    {
      Libraries.LoadGameLibraries();
      this.ReadShandaMir2LibToListBox();
      this.ReadWemadeMir2LibToListBox();
      this.ReadWemadeMir3LibToListBox();
      this.ReadShandaMir3LibToListBox();
      this.ReadObjectsToListBox();
      DXManager.Create((Control) this.MapPanel);
      this.comboBox_cellSize.SelectedIndex = 0;
      this.gridUpdate(false);
      Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Map_Editor.Resources.Square.png");
      using (Graphics graphics = Graphics.FromImage((Image) this.cellHighlight))
        graphics.DrawImage(Image.FromStream(manifestResourceStream), new Point(0, 0));
    }

    private void Draw(int libIndex, int index, int drawX, int drawY)
    {
      Libraries.MapLibs[libIndex].CheckImage(index);
      MLibrary.MImage image = Libraries.MapLibs[libIndex].Images[index];
      if (image.Image == null || image.ImageTexture == (Texture) null)
        return;
      int width = (int) image.Width;
      int height = (int) image.Height;
      DXManager.Sprite.Draw2D(image.ImageTexture, Rectangle.Empty, new SizeF((float) (width * Main.zoomMIN / Main.zoomMAX), (float) (height * Main.zoomMIN / Main.zoomMAX)), new PointF((float) drawX, (float) drawY), Color.White);
    }

    public void DrawBlend(
      int libindex,
      int index,
      Point point,
      Color colour,
      bool offSet = false,
      float rate = 1f)
    {
      Libraries.MapLibs[this.libIndex].CheckImage(index);
      MLibrary.MImage image = Libraries.MapLibs[this.libIndex].Images[index];
      if (image.Image == null || image.ImageTexture == (Texture) null)
        return;
      int width = (int) image.Width;
      int height = (int) image.Height;
      if (offSet)
        point.Offset((int) image.X * Main.zoomMIN / Main.zoomMAX, (int) image.Y * Main.zoomMIN / Main.zoomMAX);
      bool blending = DXManager.Blending;
      DXManager.SetBlend(true, rate);
      DXManager.Sprite.Draw2D(image.ImageTexture, Rectangle.Empty, new SizeF((float) (width * Main.zoomMIN / Main.zoomMAX), (float) (height * Main.zoomMIN / Main.zoomMAX)), (PointF) point, Color.White);
      DXManager.SetBlend(blending);
    }

    private string GetLibName(int index)
    {
      if (index < 0 || index >= Libraries.ListItems.Length)
        return string.Empty;
      try
      {
        return Libraries.ListItems[index].Text;
      }
      catch (Exception ex)
      {
        return string.Empty;
      }
    }

    private void MapPanel_MouseMove(object sender, MouseEventArgs e)
    {
      if (this.M2CellInfo == null)
        return;
      Point client = this.MapPanel.PointToClient(Control.MousePosition);
      this.cellX = client.X / (48 * Main.zoomMIN / Main.zoomMAX) + this.mapPoint.X;
      this.cellY = client.Y / (32 * Main.zoomMIN / Main.zoomMAX) + this.mapPoint.Y;
      if (this.cellX >= this.mapWidth || this.cellY >= this.mapHeight || this.cellX < 0 || this.cellY < 0)
        return;
      this.ShowCellInfo(this.chkShowCellInfo.Checked);
      if (this.keyDown)
        this.MapPanel_MouseClick(sender, e);
      if (this.M2CellInfo == null)
        return;
      switch (this.layer)
      {
        case Main.Layer.GraspingMir2Front:
          if (this.Grasping && !this.p1.IsEmpty)
          {
            this.p2 = new Point(this.cellX, this.cellY);
            break;
          }
          break;
        case Main.Layer.GraspingInvertMir3FrontMiddle:
          if (this.Grasping && !this.p1.IsEmpty)
          {
            this.p2 = new Point(this.cellX, this.cellY);
            break;
          }
          break;
      }
    }

    private void DrawLimit()
    {
      int drawX = (this.cellX - this.mapPoint.X) * (48 * Main.zoomMIN / Main.zoomMAX);
      int drawY = (this.cellY - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX);
      switch (this.layer)
      {
        case Main.Layer.BackLimit:
          this.Draw(1, 58, drawX, drawY);
          break;
        case Main.Layer.FrontLimit:
          this.Draw(1, 59, drawX, drawY);
          break;
        case Main.Layer.BackFrontLimit:
          this.Draw(1, 58, drawX, drawY);
          this.Draw(1, 59, drawX, drawY);
          break;
      }
    }

    private void DrawSelectTextureImage()
    {
      if (this.selectLibMImage == null || this.selectLibMImage.ImageTexture == (Texture) null || this.selectListItem == null)
        return;
      if (this.layer == Main.Layer.MiddleImage || this.layer == Main.Layer.FrontImage)
      {
        int libIndex = this.selectListItem.Value;
        int selectImageIndex = this.selectImageIndex;
        int drawX = (this.cellX - this.mapPoint.X) * (48 * Main.zoomMIN / Main.zoomMAX);
        int num1 = (this.cellY - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX);
        Size size = Libraries.MapLibs[libIndex].GetSize(selectImageIndex);
        if ((size.Width != 48 || size.Height != 32) && (size.Width != 96 || size.Height != 64))
        {
          int num2 = (this.cellY - this.mapPoint.Y + 1) * (32 * Main.zoomMIN / Main.zoomMAX);
          this.Draw(libIndex, selectImageIndex, drawX, num2 - size.Height * Main.zoomMIN / Main.zoomMAX);
        }
        else
        {
          int drawY = (this.cellY - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX);
          this.Draw(libIndex, selectImageIndex, drawX, drawY);
        }
      }
      else
      {
        if (this.layer != Main.Layer.BackImage)
          return;
        Point point = this.CheckPointIsEven(new Point(this.cellX, this.cellY));
        this.cellX = point.X;
        this.cellY = point.Y;
        int libIndex = this.selectListItem.Value;
        int selectImageIndex = this.selectImageIndex;
        int drawX = (this.cellX - this.mapPoint.X) * (48 * Main.zoomMIN / Main.zoomMAX);
        int num3 = (this.cellY - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX);
        Size size = Libraries.MapLibs[libIndex].GetSize(selectImageIndex);
        if ((size.Width != 48 || size.Height != 32) && (size.Width != 96 || size.Height != 64))
        {
          int num4 = (this.cellY - this.mapPoint.Y + 1) * (32 * Main.zoomMIN / Main.zoomMAX);
          this.Draw(libIndex, selectImageIndex, drawX, num4 - size.Height * Main.zoomMIN / Main.zoomMAX);
        }
        else
        {
          int drawY = (this.cellY - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX);
          this.Draw(libIndex, selectImageIndex, drawX, drawY);
        }
      }
    }

    private void DrawObject(CellInfoData[] datas)
    {
      if (datas == null || this.layer != Main.Layer.PlaceObjects)
        return;
      for (int index = 0; index < datas.Length; ++index)
      {
        if ((uint) (datas[index].X % 2) <= 0U && (uint) (datas[index].Y % 2) <= 0U && datas[index].X + this.cellX < this.mapWidth && datas[index].Y + this.cellY < this.mapWidth)
        {
          this.drawX = (datas[index].X + this.cellX - this.mapPoint.X) * (48 * Main.zoomMIN / Main.zoomMAX);
          this.drawY = (datas[index].Y + this.cellY - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX);
          this.index = (datas[index].CellInfo.BackImage & 536870911) - 1;
          this.libIndex = (int) datas[index].CellInfo.BackIndex;
          if (this.libIndex >= 0 && this.libIndex < Libraries.MapLibs.Length && this.index >= 0 && this.index < Libraries.MapLibs[this.libIndex].Images.Count)
            this.Draw(this.libIndex, this.index, this.drawX, this.drawY);
        }
      }
      for (int index = 0; index < datas.Length; ++index)
      {
        if (datas[index].X + this.cellX < this.mapWidth && datas[index].Y + this.cellY < this.mapWidth)
        {
          this.drawX = (datas[index].X + this.cellX - this.mapPoint.X) * (48 * Main.zoomMIN / Main.zoomMAX);
          this.index = (int) datas[index].CellInfo.MiddleImage - 1;
          this.libIndex = (int) datas[index].CellInfo.MiddleIndex;
          if (this.libIndex >= 0 && this.libIndex < Libraries.MapLibs.Length && this.index >= 0 && this.index < Libraries.MapLibs[this.libIndex].Images.Count)
          {
            byte middleAnimationFrame = datas[index].CellInfo.MiddleAnimationFrame;
            bool flag = false;
            if (middleAnimationFrame > (byte) 0 && middleAnimationFrame < byte.MaxValue)
            {
              if (((int) middleAnimationFrame & 15) > 0)
              {
                flag = true;
                middleAnimationFrame &= (byte) 15;
              }
              if (middleAnimationFrame > (byte) 0)
              {
                byte middleAnimationTick = datas[index].CellInfo.MiddleAnimationTick;
                this.index += this.AnimationCount % ((int) middleAnimationFrame + (int) middleAnimationFrame * (int) middleAnimationTick) / (1 + (int) middleAnimationTick);
              }
            }
            Size size = Libraries.MapLibs[this.libIndex].GetSize(this.index);
            if ((size.Width != 48 || size.Height != 32) && (size.Width != 96 || size.Height != 64))
            {
              this.drawY = (datas[index].Y + this.cellY - this.mapPoint.Y + 1) * (32 * Main.zoomMIN / Main.zoomMAX);
              this.Draw(this.libIndex, this.index, this.drawX, this.drawY - size.Height * Main.zoomMIN / Main.zoomMAX);
            }
            else
            {
              this.drawY = (datas[index].Y + this.cellY - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX);
              this.Draw(this.libIndex, this.index, this.drawX, this.drawY);
            }
            if (((int) datas[index].CellInfo.MiddleImage & (int) short.MaxValue) - 1 >= 0)
            {
              this.drawY = (datas[index].Y + this.cellY - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX);
              this.Draw(1, 56, this.drawX, this.drawY);
            }
          }
        }
      }
      for (int index = 0; index < datas.Length; ++index)
      {
        if (datas[index].X + this.cellX < this.mapWidth && datas[index].Y + this.cellY < this.mapWidth)
        {
          this.drawX = (datas[index].X + this.cellX - this.mapPoint.X) * (48 * Main.zoomMIN / Main.zoomMAX);
          this.drawY = (datas[index].Y + this.cellY - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX);
          this.index = ((int) datas[index].CellInfo.FrontImage & (int) short.MaxValue) - 1;
          this.libIndex = (int) datas[index].CellInfo.FrontIndex;
          if (this.libIndex >= 0 && this.libIndex < Libraries.MapLibs.Length && this.index >= 0 && this.index < Libraries.MapLibs[this.libIndex].Images.Count)
          {
            Size size = Libraries.MapLibs[this.libIndex].GetSize(this.index);
            byte frontAnimationFrame = datas[index].CellInfo.FrontAnimationFrame;
            bool flag;
            if (((int) frontAnimationFrame & 128) > 0)
            {
              flag = true;
              frontAnimationFrame &= (byte) 127;
            }
            else
              flag = false;
            if (frontAnimationFrame > (byte) 0)
            {
              byte frontAnimationTick = datas[index].CellInfo.FrontAnimationTick;
              this.index += this.AnimationCount % ((int) frontAnimationFrame + (int) frontAnimationFrame * (int) frontAnimationTick) / (1 + (int) frontAnimationTick);
            }
            if ((size.Width != 48 || size.Height != 32) && (size.Width != 96 || size.Height != 64))
            {
              this.drawY = (datas[index].Y + this.cellY - this.mapPoint.Y + 1) * (32 * Main.zoomMIN / Main.zoomMAX);
              if (frontAnimationFrame > (byte) 0)
              {
                if (flag)
                {
                  if (this.libIndex > 99 & this.libIndex < 199)
                    this.DrawBlend(this.libIndex, this.index, new Point(this.drawX, this.drawY - 96 * Main.zoomMIN / Main.zoomMAX), Color.White, true);
                  else
                    this.DrawBlend(this.libIndex, this.index, new Point(this.drawX, this.drawY - size.Height * Main.zoomMIN / Main.zoomMAX), Color.White, this.index >= 2723 && this.index <= 2732);
                }
                else
                  this.Draw(this.libIndex, this.index, this.drawX, this.drawY - size.Height * Main.zoomMIN / Main.zoomMAX);
              }
              else
                this.Draw(this.libIndex, this.index, this.drawX, this.drawY - size.Height * Main.zoomMIN / Main.zoomMAX);
            }
            else
            {
              this.drawY = (datas[index].Y + this.cellY - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX);
              this.Draw(this.libIndex, this.index, this.drawX, this.drawY);
            }
            if (((int) datas[index].CellInfo.FrontImage & (int) short.MaxValue) - 1 >= 0)
            {
              this.drawY = (datas[index].Y + this.cellY - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX);
              this.Draw(1, 56, this.drawX, this.drawY);
            }
          }
        }
      }
    }

    private void DrawGrids(bool blGrids)
    {
      if (!blGrids || Main.FPS < 25)
        return;
      for (int y = this.mapPoint.Y; y <= this.mapPoint.Y + this.OffSetY + 2; ++y)
      {
        if (y < this.mapHeight && y >= 0)
        {
          this.drawY = (y - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX);
          for (int x = this.mapPoint.X; x <= this.mapPoint.X + this.OffSetX + 2; ++x)
          {
            if (x < this.mapWidth && x >= 0)
            {
              this.drawX = (x - this.mapPoint.X) * (48 * Main.zoomMIN / Main.zoomMAX);
              this.DrawCube(this.drawX, this.drawY);
            }
          }
        }
      }
    }

    private void DrawGrids2(bool blGrids)
    {
      if (!blGrids || Main.FPS < 25)
        return;
      for (int x = 0; x <= this.OffSetX; ++x)
      {
        if (x < this.mapHeight && x >= 0)
        {
          for (int y = 0; y <= this.OffSetY; ++y)
          {
            if (y < this.mapWidth && y >= 0)
            {
              this.DrawHorizontalLine(x, y);
              this.DrawVerticalLine(x, y);
            }
          }
        }
      }
    }

    private void DrawCube(int x, int y)
    {
      Main.vector2S[0] = new Vector2((float) x, (float) y);
      Main.vector2S[1] = new Vector2((float) (48 * Main.zoomMIN / Main.zoomMAX), (float) y);
      Main.vector2S[2] = new Vector2((float) (48 * Main.zoomMIN / Main.zoomMAX), (float) (32 * Main.zoomMIN / Main.zoomMAX));
      Main.vector2S[3] = new Vector2((float) x, (float) (32 * Main.zoomMIN / Main.zoomMAX));
      Main.vector2S[4] = new Vector2((float) x, (float) y);
      DXManager.Line.Width = 0.5f;
      DXManager.Line.Draw(Main.vector2S, Color.Magenta);
    }

    private void DrawHorizontalLine(int x, int y)
    {
      Main.line[0] = new Vector2((float) (x * 48 * Main.zoomMIN / Main.zoomMAX), (float) (y * 32 * Main.zoomMIN / Main.zoomMAX));
      Main.line[1] = new Vector2((float) this.MapPanel.Width, (float) (y * 32 * Main.zoomMIN / Main.zoomMAX));
      DXManager.Line.Width = 0.5f;
      DXManager.Line.Draw(Main.line, Color.Magenta);
    }

    private void DrawVerticalLine(int x, int y)
    {
      Main.line[0] = new Vector2((float) (x * 48 * Main.zoomMIN / Main.zoomMAX), (float) (y * 32 * Main.zoomMIN / Main.zoomMAX));
      Main.line[1] = new Vector2((float) (x * 48 * Main.zoomMIN / Main.zoomMAX), (float) this.MapPanel.Height);
      DXManager.Line.Width = 0.5f;
      DXManager.Line.Draw(Main.line, Color.Magenta);
    }

    private void DrawCube(Point p1, Point p2)
    {
      Vector2[] vertexList = new Vector2[5]
      {
        new Vector2((float) ((p1.X - this.mapPoint.X) * (48 * Main.zoomMIN / Main.zoomMAX)), (float) ((p1.Y - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX))),
        new Vector2((float) ((p2.X - this.mapPoint.X + 1) * (48 * Main.zoomMIN / Main.zoomMAX)), (float) ((p1.Y - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX))),
        new Vector2((float) ((p2.X - this.mapPoint.X + 1) * (48 * Main.zoomMIN / Main.zoomMAX)), (float) ((p2.Y - this.mapPoint.Y + 1) * (32 * Main.zoomMIN / Main.zoomMAX))),
        new Vector2((float) ((p1.X - this.mapPoint.X) * (48 * Main.zoomMIN / Main.zoomMAX)), (float) ((p2.Y - this.mapPoint.Y + 1) * (32 * Main.zoomMIN / Main.zoomMAX))),
        new Vector2((float) ((p1.X - this.mapPoint.X) * (48 * Main.zoomMIN / Main.zoomMAX)), (float) ((p1.Y - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX)))
      };
      DXManager.Line.Width = 2f;
      DXManager.Line.Draw(vertexList, Color.Red);
    }

    private void GraspingRectangle()
    {
      if (this.p1.IsEmpty || this.p2.IsEmpty || this.layer != Main.Layer.GraspingMir2Front && this.layer != Main.Layer.GraspingInvertMir3FrontMiddle)
        return;
      this.DrawCube(this.p1, this.p2);
    }

    private void DrawFrontMask(bool blFrontMask)
    {
      if (!blFrontMask)
        return;
      for (int index = this.mapPoint.Y - 1; index <= this.mapPoint.Y + this.OffSetY + 35; ++index)
      {
        if (index < this.mapHeight && index >= 0)
        {
          this.drawY = (index - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX);
          for (int x = this.mapPoint.X; x <= this.mapPoint.X + this.OffSetX + 35; ++x)
          {
            if (x < this.mapWidth && x >= 0)
            {
              this.drawX = (x - this.mapPoint.X) * (48 * Main.zoomMIN / Main.zoomMAX);
              if (Convert.ToBoolean((int) this.M2CellInfo[x, index].FrontImage & 32768))
                this.Draw(1, 59, this.drawX, this.drawY);
            }
          }
        }
      }
    }

    private void DrawBackLimit(bool blBackMask)
    {
      if (!blBackMask)
        return;
      for (int index = this.mapPoint.Y - 1; index <= this.mapPoint.Y + this.OffSetY + 35; ++index)
      {
        if (index < this.mapHeight && index >= 0)
        {
          this.drawY = (index - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX);
          for (int x = this.mapPoint.X; x <= this.mapPoint.X + this.OffSetX + 35; ++x)
          {
            if (x < this.mapWidth && x >= 0)
            {
              this.drawX = (x - this.mapPoint.X) * (48 * Main.zoomMIN / Main.zoomMAX);
              if (Convert.ToBoolean(this.M2CellInfo[x, index].BackImage & 536870912))
                this.Draw(1, 58, this.drawX, this.drawY);
            }
          }
        }
      }
    }

    private void DrawFront(bool blFront)
    {
      if (!blFront)
        return;
      for (int index = this.mapPoint.Y - 1; index <= this.mapPoint.Y + this.OffSetY + 35; ++index)
      {
        if (index < this.mapHeight && index >= 0)
        {
          for (int x = this.mapPoint.X; x <= this.mapPoint.X + this.OffSetX + 35; ++x)
          {
            if (x < this.mapWidth && x >= 0)
            {
              this.drawX = (x - this.mapPoint.X) * (48 * Main.zoomMIN / Main.zoomMAX);
              this.index = ((int) this.M2CellInfo[x, index].FrontImage & (int) short.MaxValue) - 1;
              this.libIndex = (int) this.M2CellInfo[x, index].FrontIndex;
              if (this.libIndex >= 0 && this.libIndex < Libraries.MapLibs.Length && this.index >= 0 && this.index < Libraries.MapLibs[this.libIndex].Images.Count)
              {
                byte frontAnimationFrame = this.M2CellInfo[x, index].FrontAnimationFrame;
                bool flag;
                if (((int) frontAnimationFrame & 128) > 0)
                {
                  flag = true;
                  frontAnimationFrame &= (byte) 127;
                }
                else
                  flag = false;
                if (frontAnimationFrame > (byte) 0)
                {
                  byte frontAnimationTick = this.M2CellInfo[x, index].FrontAnimationTick;
                  this.index += this.AnimationCount % ((int) frontAnimationFrame + (int) frontAnimationFrame * (int) frontAnimationTick) / (1 + (int) frontAnimationTick);
                }
                byte doorOffset = this.M2CellInfo[x, index].DoorOffset;
                Size size = Libraries.MapLibs[this.libIndex].GetSize(this.index);
                if ((size.Width != 48 || size.Height != 32) && (size.Width != 96 || size.Height != 64))
                {
                  this.drawY = (index - this.mapPoint.Y + 1) * (32 * Main.zoomMIN / Main.zoomMAX);
                  if (frontAnimationFrame > (byte) 0)
                  {
                    if (flag)
                    {
                      if (this.libIndex > 99 & this.libIndex < 199)
                        this.DrawBlend(this.libIndex, this.index, new Point(this.drawX, this.drawY - 96 * Main.zoomMIN / Main.zoomMAX), Color.White, true);
                      else
                        this.DrawBlend(this.libIndex, this.index, new Point(this.drawX, this.drawY - size.Height * Main.zoomMIN / Main.zoomMAX), Color.White, this.index >= 2723 && this.index <= 2732);
                    }
                    else
                      this.Draw(this.libIndex, this.index, this.drawX, this.drawY - size.Height * Main.zoomMIN / Main.zoomMAX);
                  }
                  else
                    this.Draw(this.libIndex, this.index, this.drawX, this.drawY - size.Height * Main.zoomMIN / Main.zoomMAX);
                }
                else
                {
                  this.drawY = (index - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX);
                  this.Draw(this.libIndex, this.index, this.drawX, this.drawY);
                }
                if (this.chkDoor.Checked && doorOffset > (byte) 0)
                {
                  this.drawY = (index - this.mapPoint.Y + 1) * (32 * Main.zoomMIN / Main.zoomMAX);
                  this.Draw(this.libIndex, this.index + (int) doorOffset, this.drawX, this.drawY - size.Height * Main.zoomMIN / Main.zoomMAX);
                }
              }
            }
          }
        }
      }
    }

    private void DrawFrontTag(bool blFront)
    {
      if (!blFront)
        return;
      for (int index = this.mapPoint.Y - 1; index <= this.mapPoint.Y + this.OffSetY + 35; ++index)
      {
        if (index < this.mapHeight && index >= 0)
        {
          this.drawY = (index - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX);
          for (int x = this.mapPoint.X; x <= this.mapPoint.X + this.OffSetX + 35; ++x)
          {
            if (x < this.mapWidth && x >= 0)
            {
              this.drawX = (x - this.mapPoint.X) * (48 * Main.zoomMIN / Main.zoomMAX);
              if (((int) this.M2CellInfo[x, index].FrontImage & (int) short.MaxValue) - 1 >= 0)
                this.Draw(1, 56, this.drawX, this.drawY);
            }
          }
        }
      }
    }

    private void DrawMiddleTag(bool blMiddle)
    {
      if (!blMiddle)
        return;
      for (int index = this.mapPoint.Y - 1; index <= this.mapPoint.Y + this.OffSetY + 35; ++index)
      {
        if (index < this.mapHeight && index >= 0)
        {
          this.drawY = (index - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX);
          for (int x = this.mapPoint.X; x <= this.mapPoint.X + this.OffSetX + 35; ++x)
          {
            if (x < this.mapWidth && x >= 0)
            {
              this.drawX = (x - this.mapPoint.X) * (48 * Main.zoomMIN / Main.zoomMAX);
              if (((int) this.M2CellInfo[x, index].MiddleImage & (int) short.MaxValue) - 1 >= 0)
                this.Draw(1, 56, this.drawX, this.drawY);
            }
          }
        }
      }
    }

    private void DrawMidd(bool blMidd)
    {
      if (!blMidd)
        return;
      for (int index1 = this.mapPoint.Y - 1; index1 <= this.mapPoint.Y + this.OffSetY + 35; ++index1)
      {
        if (index1 < this.mapHeight && index1 >= 0)
        {
          for (int index2 = this.mapPoint.X - 1; index2 <= this.mapPoint.X + this.OffSetX + 35; ++index2)
          {
            if (index2 < this.mapWidth && index2 >= 0)
            {
              this.drawX = (index2 - this.mapPoint.X) * (48 * Main.zoomMIN / Main.zoomMAX);
              this.index = (int) this.M2CellInfo[index2, index1].MiddleImage - 1;
              this.libIndex = (int) this.M2CellInfo[index2, index1].MiddleIndex;
              if (this.libIndex >= 0 && this.libIndex < Libraries.MapLibs.Length && this.index >= 0 && this.index < Libraries.MapLibs[this.libIndex].Images.Count)
              {
                byte middleAnimationFrame = this.M2CellInfo[index2, index1].MiddleAnimationFrame;
                bool flag = false;
                if (middleAnimationFrame > (byte) 0 && middleAnimationFrame < byte.MaxValue)
                {
                  if (((int) middleAnimationFrame & 15) > 0)
                  {
                    flag = true;
                    middleAnimationFrame &= (byte) 15;
                  }
                  if (middleAnimationFrame > (byte) 0)
                  {
                    byte middleAnimationTick = this.M2CellInfo[index2, index1].MiddleAnimationTick;
                    this.index += this.AnimationCount % ((int) middleAnimationFrame + (int) middleAnimationFrame * (int) middleAnimationTick) / (1 + (int) middleAnimationTick);
                  }
                }
                Size size = Libraries.MapLibs[this.libIndex].GetSize(this.index);
                if ((size.Width != 48 || size.Height != 32) && (size.Width != 96 || size.Height != 64))
                {
                  this.drawY = (index1 - this.mapPoint.Y + 1) * (32 * Main.zoomMIN / Main.zoomMAX);
                  this.Draw(this.libIndex, this.index, this.drawX, this.drawY - size.Height * Main.zoomMIN / Main.zoomMAX);
                }
                else
                {
                  this.drawY = (index1 - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX);
                  this.Draw(this.libIndex, this.index, this.drawX, this.drawY);
                }
              }
            }
          }
        }
      }
    }

    private void DrawBack(bool blBack)
    {
      if (!blBack)
        return;
      for (int index1 = this.mapPoint.Y - 1; index1 <= this.mapPoint.Y + this.OffSetY; ++index1)
      {
        if ((uint) (index1 % 2) <= 0U && index1 < this.mapHeight)
        {
          this.drawY = (index1 - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX);
          for (int index2 = this.mapPoint.X - 1; index2 <= this.mapPoint.X + this.OffSetX; ++index2)
          {
            if ((uint) (index2 % 2) <= 0U && index2 < this.mapWidth)
            {
              this.drawX = (index2 - this.mapPoint.X) * (48 * Main.zoomMIN / Main.zoomMAX);
              this.index = (this.M2CellInfo[index2, index1].BackImage & 536870911) - 1;
              this.libIndex = (int) this.M2CellInfo[index2, index1].BackIndex;
              if (this.libIndex >= 0 && this.libIndex < Libraries.MapLibs.Length && this.index >= 0 && this.index < Libraries.MapLibs[this.libIndex].Images.Count)
                this.Draw(this.libIndex, this.index, this.drawX, this.drawY);
            }
          }
        }
      }
    }

    private void ReadWemadeMir2LibToListBox()
    {
      for (int index = 0; index < 100; ++index)
      {
        if (Libraries.ListItems[index] != null)
          this.WemadeMir2LibListBox.Items.Add((object) Libraries.ListItems[index]);
      }
    }

    private void ReadShandaMir2LibToListBox()
    {
      for (int index = 100; index < 200; ++index)
      {
        if (Libraries.ListItems[index] != null)
          this.ShandaMir2LibListBox.Items.Add((object) Libraries.ListItems[index]);
      }
    }

    private void ReadWemadeMir3LibToListBox()
    {
      for (int index = 200; index < 300; ++index)
      {
        if (Libraries.ListItems[index] != null)
          this.WemadeMir3LibListBox.Items.Add((object) Libraries.ListItems[index]);
      }
    }

    private void ReadShandaMir3LibToListBox()
    {
      for (int index = 300; index < 400; ++index)
      {
        if (Libraries.ListItems[index] != null)
          this.ShandaMir3LibListBox.Items.Add((object) Libraries.ListItems[index]);
      }
    }

    private void Clear()
    {
      this._shandaMir2IndexList.Clear();
      this.ShandaMir2ImageList.Images.Clear();
      this._wemadeMir2IndexList.Clear();
      this.WemadeMir2ImageList.Images.Clear();
      this._wemadeMir3IndexList.Clear();
      this.WemadeMir3ImageList.Images.Clear();
      this._shandaMir3IndexList.Clear();
      this.ShandaMir3ImageList.Images.Clear();
      this.TilesImageList.Images.Clear();
      this._tilesIndexList.Clear();
    }

    private void ClearImage()
    {
      for (int index1 = 0; index1 < Libraries.MapLibs.Length; ++index1)
      {
        for (int index2 = 0; index2 < Libraries.MapLibs[index1].Images.Count; ++index2)
          Libraries.MapLibs[index1].Images[index2] = (MLibrary.MImage) null;
      }
    }

    public void EnlargeZoom()
    {
      this.graphics.Clear(Color.Black);
      Main.zoomMIN += 3;
      if (Main.zoomMIN < Main.zoomMAX)
        return;
      Main.zoomMIN = Main.zoomMAX;
    }

    public void NarrowZoom()
    {
      this.graphics.Clear(Color.Black);
      Main.zoomMIN -= 3;
      if (Main.zoomMIN > 0)
        return;
      Main.zoomMIN = 1;
    }

    private void btnDispose_Click(object sender, EventArgs e) => this.Dispose();

    private void chkBack_Click(object sender, EventArgs e) => this.chkBack.Checked = !this.chkBack.Checked;

    private void chkMidd_Click(object sender, EventArgs e) => this.chkMidd.Checked = !this.chkMidd.Checked;

    private void chkFront_Click(object sender, EventArgs e) => this.chkFront.Checked = !this.chkFront.Checked;

    private void chkBackMask_Click(object sender, EventArgs e) => this.chkBackMask.Checked = !this.chkBackMask.Checked;

    private void chkFrontMask_Click(object sender, EventArgs e) => this.chkFrontMask.Checked = !this.chkFrontMask.Checked;

    private void btnToImage_Click(object sender, EventArgs e)
    {
    }

    private void chkDoor_Click(object sender, EventArgs e) => this.chkDoor.Checked = !this.chkDoor.Checked;

    private void DrawDoorTag(bool blDoorTag)
    {
      if (!blDoorTag)
        return;
      System.Drawing.Font font1 = new System.Drawing.Font("Comic Sans MS", 10f, FontStyle.Bold);
      Microsoft.DirectX.Direct3D.Font font2 = new Microsoft.DirectX.Direct3D.Font(DXManager.Device, font1);
      for (int y = this.mapPoint.Y; y <= this.mapPoint.Y + this.OffSetY; ++y)
      {
        if (y < this.mapHeight && y >= 0)
        {
          this.drawY = (y - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX);
          for (int x = this.mapPoint.X; x <= this.mapPoint.X + this.OffSetX + 35; ++x)
          {
            if (x < this.mapWidth && x >= 0)
            {
              this.drawX = (x - this.mapPoint.X) * (48 * Main.zoomMIN / Main.zoomMAX);
              int num = (int) this.M2CellInfo[x, y].DoorIndex & (int) sbyte.MaxValue;
              byte doorOffset = this.M2CellInfo[x, y].DoorOffset;
              bool boolean = Convert.ToBoolean((int) this.M2CellInfo[x, y].DoorIndex & 128);
              if (num > 0)
              {
                string text = !boolean ? string.Format("D{0}/{1}", (object) num, (object) doorOffset) : string.Format("Dx{0}/{1}", (object) num, (object) doorOffset);
                font2.DrawText(DXManager.TextSprite, text, this.drawX, this.drawY, Color.AliceBlue);
              }
            }
          }
        }
      }
      font1.Dispose();
      font2.Dispose();
    }

    private void chkDoorSign_Click(object sender, EventArgs e) => this.chkDoorSign.Checked = !this.chkDoorSign.Checked;

    private void DrawFrontAnimationTag(bool blFrontAnimationTag)
    {
      if (!blFrontAnimationTag)
        return;
      System.Drawing.Font font1 = new System.Drawing.Font("Comic Sans MS", 10f, FontStyle.Bold);
      Microsoft.DirectX.Direct3D.Font font2 = new Microsoft.DirectX.Direct3D.Font(DXManager.Device, font1);
      for (int y = this.mapPoint.Y; y <= this.mapPoint.Y + this.OffSetY; ++y)
      {
        if (y < this.mapHeight && y >= 0)
        {
          this.drawY = (y - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX);
          for (int x = this.mapPoint.X; x <= this.mapPoint.X + this.OffSetX + 35; ++x)
          {
            if (x < this.mapWidth && x >= 0)
            {
              this.drawX = (x - this.mapPoint.X) * (48 * Main.zoomMIN / Main.zoomMAX);
              if (this.M2CellInfo[x, y].FrontAnimationFrame != byte.MaxValue)
              {
                int num = (int) this.M2CellInfo[x, y].FrontAnimationFrame & (int) sbyte.MaxValue;
                byte frontAnimationTick = this.M2CellInfo[x, y].FrontAnimationTick;
                bool boolean = Convert.ToBoolean((int) this.M2CellInfo[x, y].FrontAnimationFrame & 128);
                if (num > 0)
                {
                  string text = !boolean ? string.Format("FA{0}/{1}", (object) num, (object) frontAnimationTick) : string.Format("FAb{0}/{1}", (object) num, (object) frontAnimationTick);
                  font2.DrawText(DXManager.TextSprite, text, this.drawX, this.drawY, Color.AliceBlue);
                }
              }
            }
          }
        }
      }
      font1.Dispose();
      font2.Dispose();
    }

    private void DrawMiddleAnimationTag(bool blMiddleAnimationTag)
    {
      if (!blMiddleAnimationTag)
        return;
      System.Drawing.Font font1 = new System.Drawing.Font("Comic Sans MS", 10f, FontStyle.Bold);
      Microsoft.DirectX.Direct3D.Font font2 = new Microsoft.DirectX.Direct3D.Font(DXManager.Device, font1);
      for (int y = this.mapPoint.Y; y <= this.mapPoint.Y + this.OffSetY; ++y)
      {
        if (y < this.mapHeight && y >= 0)
        {
          this.drawY = (y - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX);
          for (int x = this.mapPoint.X; x <= this.mapPoint.X + this.OffSetX + 35; ++x)
          {
            if (x < this.mapWidth && x >= 0)
            {
              this.drawX = (x - this.mapPoint.X) * (48 * Main.zoomMIN / Main.zoomMAX);
              if (this.M2CellInfo[x, y].MiddleAnimationFrame != byte.MaxValue)
              {
                int num = (int) this.M2CellInfo[x, y].MiddleAnimationFrame & 15;
                byte middleAnimationTick = this.M2CellInfo[x, y].MiddleAnimationTick;
                bool boolean = Convert.ToBoolean((int) this.M2CellInfo[x, y].MiddleAnimationFrame & 8);
                if (num > 0)
                {
                  string text = !boolean ? string.Format("MA{0}/{1}", (object) num, (object) middleAnimationTick) : string.Format("MAb{0}/{1}", (object) num, (object) middleAnimationTick);
                  font2.DrawText(DXManager.TextSprite, text, this.drawX, this.drawY, Color.Black);
                }
              }
            }
          }
        }
      }
      font1.Dispose();
      font2.Dispose();
    }

    private void chkFrontAnimationTag_Click(object sender, EventArgs e) => this.chkFrontAnimationTag.Checked = !this.chkFrontAnimationTag.Checked;

    private void DrawLightTag(bool blLightTag)
    {
      if (!blLightTag)
        return;
      System.Drawing.Font font1 = new System.Drawing.Font("Comic Sans MS", 10f, FontStyle.Bold);
      Microsoft.DirectX.Direct3D.Font font2 = new Microsoft.DirectX.Direct3D.Font(DXManager.Device, font1);
      for (int index = this.mapPoint.Y - 1; index <= this.mapPoint.Y + this.OffSetY + 35; ++index)
      {
        if (index < this.mapHeight && index >= 0)
        {
          this.drawY = (index - this.mapPoint.Y) * (32 * Main.zoomMIN / Main.zoomMAX);
          for (int x = this.mapPoint.X; x <= this.mapPoint.X + this.OffSetX + 35; ++x)
          {
            if (x < this.mapWidth && x >= 0)
            {
              this.drawX = (x - this.mapPoint.X) * (48 * Main.zoomMIN / Main.zoomMAX);
              int light = (int) this.M2CellInfo[x, index].Light;
              if (light > 0)
              {
                this.Draw(1, 57, this.drawX, this.drawY);
                string text = string.Format("L{0}", (object) light);
                font2.DrawText(DXManager.TextSprite, text, this.drawX + 32 * Main.zoomMIN / Main.zoomMAX, this.drawY, Color.AliceBlue);
              }
            }
          }
        }
      }
      font1.Dispose();
      font2.Dispose();
    }

    private void chkLightTag_Click(object sender, EventArgs e) => this.chkLightTag.Checked = !this.chkLightTag.Checked;

    private void Save()
    {
      if (this.M2CellInfo == null)
        return;
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "Custom  Map (*.Map)|*.Map";
      saveFileDialog.FileName = "Custom  Map";
      if (saveFileDialog.ShowDialog() == DialogResult.OK)
      {
        BinaryWriter binaryWriter = new BinaryWriter((Stream) new FileStream(saveFileDialog.FileName, FileMode.Create));
        short num1 = 1;
        char[] chars = new char[2]{ 'C', '#' };
        binaryWriter.Write(num1);
        binaryWriter.Write(chars);
        binaryWriter.Write(Convert.ToInt16(this.mapWidth));
        binaryWriter.Write(Convert.ToInt16(this.mapHeight));
        for (int index1 = 0; index1 < this.mapWidth; ++index1)
        {
          for (int index2 = 0; index2 < this.mapHeight; ++index2)
          {
            binaryWriter.Write(this.M2CellInfo[index1, index2].BackIndex);
            binaryWriter.Write(this.M2CellInfo[index1, index2].BackImage);
            binaryWriter.Write(this.M2CellInfo[index1, index2].MiddleIndex);
            binaryWriter.Write(this.M2CellInfo[index1, index2].MiddleImage);
            binaryWriter.Write(this.M2CellInfo[index1, index2].FrontIndex);
            binaryWriter.Write(this.M2CellInfo[index1, index2].FrontImage);
            binaryWriter.Write(this.M2CellInfo[index1, index2].DoorIndex);
            binaryWriter.Write(this.M2CellInfo[index1, index2].DoorOffset);
            binaryWriter.Write(this.M2CellInfo[index1, index2].FrontAnimationFrame);
            binaryWriter.Write(this.M2CellInfo[index1, index2].FrontAnimationTick);
            binaryWriter.Write(this.M2CellInfo[index1, index2].MiddleAnimationFrame);
            binaryWriter.Write(this.M2CellInfo[index1, index2].MiddleAnimationTick);
            binaryWriter.Write(this.M2CellInfo[index1, index2].TileAnimationImage);
            binaryWriter.Write(this.M2CellInfo[index1, index2].TileAnimationOffset);
            binaryWriter.Write(this.M2CellInfo[index1, index2].TileAnimationFrames);
            binaryWriter.Write(this.M2CellInfo[index1, index2].Light);
          }
        }
        binaryWriter.Flush();
        binaryWriter.Dispose();
        int num2 = (int) MessageBox.Show("Map Saved");
      }
    }

    private void InvertMir3Layer()
    {
      if (this.M2CellInfo == null)
        return;
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "Custom  Map (*.Mir3)|*.Mir3";
      saveFileDialog.FileName = "Invert Mir3 Layer";
      if (saveFileDialog.ShowDialog() == DialogResult.OK)
      {
        FileStream output = new FileStream(saveFileDialog.FileName, FileMode.Create);
        BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
        short num1 = 1;
        char[] chars = new char[2]{ 'C', '#' };
        binaryWriter.Write(num1);
        binaryWriter.Write(chars);
        binaryWriter.Write(Convert.ToInt16(this.mapWidth));
        binaryWriter.Write(Convert.ToInt16(this.mapHeight));
        for (int index1 = 0; index1 < this.mapWidth; ++index1)
        {
          for (int index2 = 0; index2 < this.mapHeight; ++index2)
          {
            binaryWriter.Write(this.M2CellInfo[index1, index2].BackIndex);
            binaryWriter.Write(this.M2CellInfo[index1, index2].BackImage);
            if (this.M2CellInfo[index1, index2].MiddleImage != (short) 0 && ((int) this.M2CellInfo[index1, index2].FrontImage & (int) short.MaxValue) == 0)
            {
              if (this.GetLibName((int) this.M2CellInfo[index1, index2].MiddleIndex).IndexOf("SmTiles", StringComparison.Ordinal) <= -1)
              {
                if (this.M2CellInfo[index1, index2].MiddleAnimationFrame != (byte) 0 && this.M2CellInfo[index1, index2].MiddleAnimationFrame != byte.MaxValue && this.M2CellInfo[index1, index2].FrontAnimationFrame == (byte) 0)
                {
                  this.M2CellInfo[index1, index2].FrontAnimationFrame = (byte) ((uint) this.M2CellInfo[index1, index2].MiddleAnimationFrame & 15U);
                  this.M2CellInfo[index1, index2].FrontAnimationTick = this.M2CellInfo[index1, index2].MiddleAnimationTick;
                  this.M2CellInfo[index1, index2].MiddleAnimationFrame = (byte) 0;
                  this.M2CellInfo[index1, index2].MiddleAnimationTick = (byte) 0;
                }
                this.M2CellInfo[index1, index2].FrontImage = this.M2CellInfo[index1, index2].MiddleImage;
                this.M2CellInfo[index1, index2].FrontIndex = this.M2CellInfo[index1, index2].MiddleIndex;
                this.M2CellInfo[index1, index2].MiddleImage = (short) 0;
                this.M2CellInfo[index1, index2].MiddleIndex = (short) 0;
              }
            }
            else if (this.M2CellInfo[index1, index2].MiddleImage != (short) 0 && ((uint) this.M2CellInfo[index1, index2].FrontImage & (uint) short.MaxValue) > 0U && this.GetLibName((int) this.M2CellInfo[index1, index2].MiddleIndex).IndexOf("SmTiles", StringComparison.Ordinal) <= -1 && (this.M2CellInfo[index1, index2].MiddleAnimationFrame == byte.MaxValue || this.M2CellInfo[index1, index2].MiddleAnimationFrame == (byte) 0) && this.M2CellInfo[index1, index2].FrontAnimationFrame == (byte) 0)
            {
              short middleImage = this.M2CellInfo[index1, index2].MiddleImage;
              this.M2CellInfo[index1, index2].MiddleImage = (short) ((int) this.M2CellInfo[index1, index2].FrontImage & (int) short.MaxValue);
              this.M2CellInfo[index1, index2].FrontImage = middleImage;
              short middleIndex = this.M2CellInfo[index1, index2].MiddleIndex;
              this.M2CellInfo[index1, index2].MiddleIndex = this.M2CellInfo[index1, index2].FrontIndex;
              this.M2CellInfo[index1, index2].FrontIndex = middleIndex;
            }
            binaryWriter.Write(this.M2CellInfo[index1, index2].MiddleIndex);
            binaryWriter.Write(this.M2CellInfo[index1, index2].MiddleImage);
            binaryWriter.Write(this.M2CellInfo[index1, index2].FrontIndex);
            binaryWriter.Write(this.M2CellInfo[index1, index2].FrontImage);
            binaryWriter.Write(this.M2CellInfo[index1, index2].DoorIndex);
            binaryWriter.Write(this.M2CellInfo[index1, index2].DoorOffset);
            binaryWriter.Write(this.M2CellInfo[index1, index2].FrontAnimationFrame);
            binaryWriter.Write(this.M2CellInfo[index1, index2].FrontAnimationTick);
            binaryWriter.Write(this.M2CellInfo[index1, index2].MiddleAnimationFrame);
            binaryWriter.Write(this.M2CellInfo[index1, index2].MiddleAnimationTick);
            binaryWriter.Write(this.M2CellInfo[index1, index2].TileAnimationImage);
            binaryWriter.Write(this.M2CellInfo[index1, index2].TileAnimationOffset);
            binaryWriter.Write(this.M2CellInfo[index1, index2].TileAnimationFrames);
            binaryWriter.Write(this.M2CellInfo[index1, index2].Light);
          }
        }
        binaryWriter.Flush();
        binaryWriter.Dispose();
        output.Dispose();
        int num2 = (int) MessageBox.Show("保存成功");
      }
    }

    private void ShandaMir2LibListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.Clear();
      this.shangdaMir2ListItem = (ListItem) this.ShandaMir2LibListBox.SelectedItem;
      this.selectListItem = this.shangdaMir2ListItem;
      this.selectListItem.Version = (byte) 2;
      this.ShandaMir2LibListView.VirtualListSize = Libraries.MapLibs[this.shangdaMir2ListItem.Value].Images.Count;
      this.TileslistView.VirtualListSize = Libraries.MapLibs[this.selectListItem.Value].Images.Count / 50;
    }

    private void ShandaMir2LiblistView_RetrieveVirtualItem(
      object sender,
      RetrieveVirtualItemEventArgs e)
    {
      int num;
      if (this._shandaMir2IndexList.TryGetValue(e.ItemIndex, out num))
      {
        e.Item = new ListViewItem()
        {
          ImageIndex = num,
          Text = e.ItemIndex.ToString()
        };
      }
      else
      {
        this._shandaMir2IndexList.Add(e.ItemIndex, this.ShandaMir2ImageList.Images.Count);
        Libraries.MapLibs[this.shangdaMir2ListItem.Value].CheckImage(e.ItemIndex);
        this.ShandaMir2ImageList.Images.Add((Image) Libraries.MapLibs[this.shangdaMir2ListItem.Value].GetPreview(e.ItemIndex));
        e.Item = new ListViewItem()
        {
          ImageIndex = num,
          Text = e.ItemIndex.ToString()
        };
        Libraries.MapLibs[this.shangdaMir2ListItem.Value].Images[e.ItemIndex] = (MLibrary.MImage) null;
      }
    }

    private void WemadeMir2LibListView_RetrieveVirtualItem(
      object sender,
      RetrieveVirtualItemEventArgs e)
    {
      int num;
      if (this._wemadeMir2IndexList.TryGetValue(e.ItemIndex, out num))
      {
        e.Item = new ListViewItem()
        {
          ImageIndex = num,
          Text = e.ItemIndex.ToString()
        };
      }
      else
      {
        this._wemadeMir2IndexList.Add(e.ItemIndex, this.WemadeMir2ImageList.Images.Count);
        Libraries.MapLibs[this.wemadeMir2ListItem.Value].CheckImage(e.ItemIndex);
        this.WemadeMir2ImageList.Images.Add((Image) Libraries.MapLibs[this.wemadeMir2ListItem.Value].GetPreview(e.ItemIndex));
        e.Item = new ListViewItem()
        {
          ImageIndex = num,
          Text = e.ItemIndex.ToString()
        };
        Libraries.MapLibs[this.wemadeMir2ListItem.Value].Images[e.ItemIndex] = (MLibrary.MImage) null;
      }
    }

    private void WemadeMir2LibListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.Clear();
      this.wemadeMir2ListItem = (ListItem) this.WemadeMir2LibListBox.SelectedItem;
      this.selectListItem = this.wemadeMir2ListItem;
      this.selectListItem.Version = (byte) 1;
      this.WemadeMir2LibListView.VirtualListSize = Libraries.MapLibs[this.wemadeMir2ListItem.Value].Images.Count;
      this.TileslistView.VirtualListSize = Libraries.MapLibs[this.wemadeMir2ListItem.Value].Images.Count / 50;
    }

    private void WemadeMir3LibListView_RetrieveVirtualItem(
      object sender,
      RetrieveVirtualItemEventArgs e)
    {
      int num;
      if (this._wemadeMir3IndexList.TryGetValue(e.ItemIndex, out num))
      {
        e.Item = new ListViewItem()
        {
          ImageIndex = num,
          Text = e.ItemIndex.ToString()
        };
      }
      else
      {
        this._wemadeMir3IndexList.Add(e.ItemIndex, this.WemadeMir3ImageList.Images.Count);
        Libraries.MapLibs[this.wemadeMir3ListItem.Value].CheckImage(e.ItemIndex);
        this.WemadeMir3ImageList.Images.Add((Image) Libraries.MapLibs[this.wemadeMir3ListItem.Value].GetPreview(e.ItemIndex));
        e.Item = new ListViewItem()
        {
          ImageIndex = num,
          Text = e.ItemIndex.ToString()
        };
        Libraries.MapLibs[this.wemadeMir3ListItem.Value].Images[e.ItemIndex] = (MLibrary.MImage) null;
      }
    }

    private void WemadeMir3LibListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.Clear();
      this.wemadeMir3ListItem = (ListItem) this.WemadeMir3LibListBox.SelectedItem;
      this.selectListItem = this.wemadeMir3ListItem;
      this.selectListItem.Version = (byte) 3;
      this.WemadeMir3LibListView.VirtualListSize = Libraries.MapLibs[this.wemadeMir3ListItem.Value].Images.Count;
      if (this.selectListItem.Text.IndexOf("Tiles30", StringComparison.Ordinal) > -1)
      {
        if ((uint) (Libraries.MapLibs[this.selectListItem.Value].Images.Count % 10) > 0U)
          this.TileslistView.VirtualListSize = (Libraries.MapLibs[this.wemadeMir3ListItem.Value].Images.Count + 1) / 30 * 2;
        else
          this.TileslistView.VirtualListSize = Libraries.MapLibs[this.wemadeMir3ListItem.Value].Images.Count / 30 * 2;
      }
      else
      {
        if (this.selectListItem.Text.IndexOf("smtiles", StringComparison.Ordinal) <= -1)
          return;
        this.TileslistView.VirtualListSize = Libraries.MapLibs[this.wemadeMir3ListItem.Value].Images.Count / 30;
      }
    }

    private void ShandaMir3LibListView_RetrieveVirtualItem(
      object sender,
      RetrieveVirtualItemEventArgs e)
    {
      int num;
      if (this._shandaMir3IndexList.TryGetValue(e.ItemIndex, out num))
      {
        e.Item = new ListViewItem()
        {
          ImageIndex = num,
          Text = e.ItemIndex.ToString()
        };
      }
      else
      {
        this._shandaMir3IndexList.Add(e.ItemIndex, this.ShandaMir3ImageList.Images.Count);
        Libraries.MapLibs[this.shangdaMir3ListItem.Value].CheckImage(e.ItemIndex);
        this.ShandaMir3ImageList.Images.Add((Image) Libraries.MapLibs[this.shangdaMir3ListItem.Value].GetPreview(e.ItemIndex));
        e.Item = new ListViewItem()
        {
          ImageIndex = num,
          Text = e.ItemIndex.ToString()
        };
        Libraries.MapLibs[this.shangdaMir3ListItem.Value].Images[e.ItemIndex] = (MLibrary.MImage) null;
      }
    }

    private void ShandaMir3LibListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.Clear();
      this.shangdaMir3ListItem = (ListItem) this.ShandaMir3LibListBox.SelectedItem;
      this.selectListItem = this.shangdaMir3ListItem;
      this.selectListItem.Version = (byte) 4;
      this.ShandaMir3LibListView.VirtualListSize = Libraries.MapLibs[this.shangdaMir3ListItem.Value].Images.Count;
      if (this.selectListItem.Text.IndexOf("Tiles30", StringComparison.Ordinal) > -1)
      {
        if ((uint) (Libraries.MapLibs[this.selectListItem.Value].Images.Count % 10) > 0U)
          this.TileslistView.VirtualListSize = (Libraries.MapLibs[this.shangdaMir3ListItem.Value].Images.Count + 1) / 30 * 2;
        else
          this.TileslistView.VirtualListSize = Libraries.MapLibs[this.shangdaMir3ListItem.Value].Images.Count / 30 * 2;
      }
      else
      {
        if (this.selectListItem.Text.IndexOf("smtiles", StringComparison.Ordinal) <= -1)
          return;
        this.TileslistView.VirtualListSize = Libraries.MapLibs[this.shangdaMir3ListItem.Value].Images.Count / 30;
      }
    }

    private void chkMiddleAnimationTag_Click(object sender, EventArgs e) => this.chkMiddleAnimationTag.Checked = !this.chkMiddleAnimationTag.Checked;

    private void cmbEditorLayer_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (this.cmbEditorLayer.SelectedIndex)
      {
        case 0:
          this.layer = Main.Layer.None;
          break;
        case 1:
          this.layer = Main.Layer.BackImage;
          break;
        case 2:
          this.layer = Main.Layer.MiddleImage;
          break;
        case 3:
          this.layer = Main.Layer.FrontImage;
          break;
        case 4:
          this.layer = Main.Layer.BackLimit;
          break;
        case 5:
          this.layer = Main.Layer.FrontLimit;
          break;
        case 6:
          this.layer = Main.Layer.BackFrontLimit;
          break;
        case 7:
          this.layer = Main.Layer.GraspingMir2Front;
          break;
        case 8:
          this.layer = Main.Layer.GraspingInvertMir3FrontMiddle;
          break;
        case 9:
          this.layer = Main.Layer.PlaceObjects;
          break;
        case 10:
          this.layer = Main.Layer.ClearAll;
          break;
        case 11:
          this.layer = Main.Layer.ClearBack;
          break;
        case 12:
          this.layer = Main.Layer.ClearMidd;
          break;
        case 13:
          this.layer = Main.Layer.ClearFront;
          break;
        case 14:
          this.layer = Main.Layer.ClearBackFrontLimit;
          break;
        case 15:
          this.layer = Main.Layer.ClearBackLimit;
          break;
        case 16:
          this.layer = Main.Layer.ClearFrontLimit;
          break;
        case 17:
          this.layer = Main.Layer.BrushMir2BigTiles;
          break;
        case 18:
          this.layer = Main.Layer.BrushSmTiles;
          break;
        case 19:
          this.layer = Main.Layer.BrushMir3BigTiles;
          break;
      }
    }

    private void WemadeMir2LibListView_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.selectListItem = this.wemadeMir2ListItem;
      if (this.WemadeMir2LibListView.SelectedIndices.Count <= 0)
        return;
      this.selectLibMImage = Libraries.MapLibs[this.wemadeMir2ListItem.Value].GetMImage(this.WemadeMir2LibListView.SelectedIndices[0]);
      if (this.selectLibMImage != null)
      {
        this.selectImageIndex = this.WemadeMir2LibListView.SelectedIndices[0];
        this.picWemdeMir2.Image = (Image) this.selectLibMImage.Image;
        this.LabWemadeMir2Width.Text = "Width : " + (object) this.selectLibMImage.Width;
        this.LabWemadeMir2Height.Text = "Height : " + (object) this.selectLibMImage.Height;
        this.labeWemadeMir2OffSetX.Text = "OffSetX : " + (object) this.selectLibMImage.X;
        this.labWemadeMir2OffSetY.Text = "OffSetY : " + (object) this.selectLibMImage.Y;
        Libraries.MapLibs[this.wemadeMir2ListItem.Value].Images[this.WemadeMir2LibListView.SelectedIndices[0]] = (MLibrary.MImage) null;
      }
    }

    private bool CheckImageSizeIsBigTile(int libIndex, int index)
    {
      Size size = Libraries.MapLibs[libIndex].GetSize(index);
      return size.Width == 96 || size.Height == 64;
    }

    private Point CheckPointIsEven(Point point)
    {
      if (point.X % 2 == 0 && point.Y % 2 == 0)
        return point;
      if ((uint) (point.X % 2) > 0U)
        --point.X;
      if ((uint) (point.Y % 2) > 0U)
        --point.Y;
      return point;
    }

    private void MapPanel_MouseClick(object sender, MouseEventArgs e)
    {
      if (this.M2CellInfo == null)
        return;
      switch (this.layer)
      {
        case Main.Layer.BackImage:
          if (this.selectListItem == null || !this.CheckImageSizeIsBigTile(this.selectListItem.Value, this.selectImageIndex))
            break;
          Point point1 = this.CheckPointIsEven(new Point(this.cellX, this.cellY));
          this.cellX = point1.X;
          this.cellY = point1.Y;
          this.AddCellInfoPoints(new Point[1]
          {
            new Point(this.cellX, this.cellY)
          });
          this.M2CellInfo[this.cellX, this.cellY].BackIndex = Convert.ToInt16(this.selectListItem.Value);
          this.M2CellInfo[this.cellX, this.cellY].BackImage = this.selectImageIndex + 1;
          break;
        case Main.Layer.MiddleImage:
          if (this.selectListItem == null)
            break;
          this.AddCellInfoPoints(new Point[1]
          {
            new Point(this.cellX, this.cellY)
          });
          this.M2CellInfo[this.cellX, this.cellY].MiddleIndex = Convert.ToInt16(this.selectListItem.Value);
          this.M2CellInfo[this.cellX, this.cellY].MiddleImage = Convert.ToInt16(this.selectImageIndex + 1);
          break;
        case Main.Layer.FrontImage:
          if (this.selectListItem == null)
            break;
          this.AddCellInfoPoints(new Point[1]
          {
            new Point(this.cellX, this.cellY)
          });
          this.M2CellInfo[this.cellX, this.cellY].FrontIndex = Convert.ToInt16(this.selectListItem.Value);
          this.M2CellInfo[this.cellX, this.cellY].FrontImage = Convert.ToInt16(this.selectImageIndex + 1);
          break;
        case Main.Layer.BackLimit:
          this.AddCellInfoPoints(new Point[1]
          {
            new Point(this.cellX, this.cellY)
          });
          this.SetBackLimit();
          break;
        case Main.Layer.FrontLimit:
          this.AddCellInfoPoints(new Point[1]
          {
            new Point(this.cellX, this.cellY)
          });
          this.SetFrontLimit();
          break;
        case Main.Layer.BackFrontLimit:
          this.AddCellInfoPoints(new Point[1]
          {
            new Point(this.cellX, this.cellY)
          });
          this.SetBackLimit();
          this.SetFrontLimit();
          break;
        case Main.Layer.PlaceObjects:
          if (!this.AddCellInfoPoints(this.GetObjectDatasPoints(this.objectDatas)) || this.objectDatas == null)
            break;
          this.ModifyM2CellInfo(this.objectDatas);
          break;
        case Main.Layer.ClearAll:
          this.AddCellInfoPoints(new Point[1]
          {
            new Point(this.cellX, this.cellY)
          });
          this.ClearAll();
          break;
        case Main.Layer.ClearBack:
          this.AddCellInfoPoints(new Point[1]
          {
            new Point(this.cellX, this.cellY)
          });
          this.ClearBack();
          break;
        case Main.Layer.ClearMidd:
          this.AddCellInfoPoints(new Point[1]
          {
            new Point(this.cellX, this.cellY)
          });
          this.ClearMidd();
          break;
        case Main.Layer.ClearFront:
          this.AddCellInfoPoints(new Point[1]
          {
            new Point(this.cellX, this.cellY)
          });
          this.ClearFront();
          break;
        case Main.Layer.ClearBackFrontLimit:
          this.AddCellInfoPoints(new Point[1]
          {
            new Point(this.cellX, this.cellY)
          });
          this.ClearBackLimit();
          this.ClearFrontLimit();
          break;
        case Main.Layer.ClearBackLimit:
          this.AddCellInfoPoints(new Point[1]
          {
            new Point(this.cellX, this.cellY)
          });
          this.ClearBackLimit();
          break;
        case Main.Layer.ClearFrontLimit:
          this.AddCellInfoPoints(new Point[1]
          {
            new Point(this.cellX, this.cellY)
          });
          this.ClearFrontLimit();
          break;
        case Main.Layer.BrushMir2BigTiles:
          Point point2 = this.CheckPointIsEven(new Point(this.cellX, this.cellY));
          this.cellX = point2.X;
          this.cellY = point2.Y;
          this.CreateMir2BigTiles();
          this.AddCellInfoPoints(this.bigTilePoints.ToArray());
          break;
        case Main.Layer.BrushSmTiles:
          this.CreateSmTiles();
          this.AddCellInfoPoints(this.smTilePoints.ToArray());
          break;
        case Main.Layer.BrushMir3BigTiles:
          Point point3 = this.CheckPointIsEven(new Point(this.cellX, this.cellY));
          this.cellX = point3.X;
          this.cellY = point3.Y;
          this.CreateMir3BigTiles();
          this.AddCellInfoPoints(this.bigTilePoints.ToArray());
          break;
      }
    }

    private Point[] GetObjectDatasPoints(CellInfoData[] datas)
    {
      if (datas == null)
        return (Point[]) null;
      List<Point> pointList = new List<Point>();
      for (int index = 0; index < datas.Length; ++index)
        pointList.Add(new Point(datas[index].X + this.cellX, datas[index].Y + this.cellY));
      return pointList.ToArray();
    }

    private void ModifyM2CellInfo(CellInfoData[] datas)
    {
      for (int index = 0; index < this.objectDatas.Length; ++index)
      {
        if ((uint) (datas[index].CellInfo.BackImage & 536870912) > 0U)
        {
          if ((datas[index].CellInfo.BackImage & 131071) - 1 == -1 || datas[index].CellInfo.BackIndex == (short) -1)
          {
            this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].BackImage |= 536870912;
          }
          else
          {
            this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].BackImage = datas[index].CellInfo.BackImage;
            this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].BackIndex = datas[index].CellInfo.BackIndex;
          }
        }
        else if ((datas[index].CellInfo.BackImage & 131071) - 1 != -1 && datas[index].CellInfo.BackIndex != (short) -1)
        {
          this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].BackImage = datas[index].CellInfo.BackImage;
          this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].BackIndex = datas[index].CellInfo.BackIndex;
        }
        if ((uint) this.objectDatas[index].CellInfo.MiddleImage > 0U)
        {
          if (((uint) this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].MiddleAnimationFrame & (uint) sbyte.MaxValue) > 0U)
          {
            this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].MiddleAnimationFrame = (byte) 0;
            this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].MiddleAnimationTick = (byte) 0;
          }
          this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].MiddleImage = datas[index].CellInfo.MiddleImage;
          this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].MiddleIndex = datas[index].CellInfo.MiddleIndex;
        }
        if ((uint) this.objectDatas[index].CellInfo.MiddleIndex > 0U)
          this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].MiddleIndex = datas[index].CellInfo.MiddleIndex;
        if ((uint) this.objectDatas[index].CellInfo.FrontImage > 0U)
        {
          if (((uint) this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].FrontAnimationFrame & (uint) sbyte.MaxValue) > 0U)
          {
            this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].FrontAnimationFrame = (byte) 0;
            this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].FrontAnimationTick = (byte) 0;
          }
          this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].FrontImage = datas[index].CellInfo.FrontImage;
          this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].FrontIndex = datas[index].CellInfo.FrontIndex;
        }
        if ((uint) this.objectDatas[index].CellInfo.FrontIndex > 0U)
          this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].FrontIndex = datas[index].CellInfo.FrontIndex;
        if (datas[index].CellInfo.DoorIndex > (byte) 0)
          this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].DoorIndex = datas[index].CellInfo.DoorIndex;
        if (datas[index].CellInfo.DoorOffset > (byte) 0)
          this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].DoorOffset = datas[index].CellInfo.DoorOffset;
        if (datas[index].CellInfo.MiddleAnimationFrame > (byte) 0)
          this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].MiddleAnimationFrame = datas[index].CellInfo.MiddleAnimationFrame;
        if (datas[index].CellInfo.MiddleAnimationTick > (byte) 0)
          this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].MiddleAnimationTick = datas[index].CellInfo.MiddleAnimationTick;
        if (datas[index].CellInfo.FrontAnimationFrame > (byte) 0)
          this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].FrontAnimationFrame = datas[index].CellInfo.FrontAnimationFrame;
        if (datas[index].CellInfo.FrontAnimationTick > (byte) 0)
          this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].FrontAnimationTick = datas[index].CellInfo.FrontAnimationTick;
        if (datas[index].CellInfo.Light > (byte) 0)
          this.M2CellInfo[this.cellX + datas[index].X, this.cellY + datas[index].Y].Light = datas[index].CellInfo.Light;
      }
    }

    private bool CheckCellInfoIsZero(CellInfo cellInfo) => cellInfo.BackImage == 0 && cellInfo.BackIndex == (short) 0 && cellInfo.MiddleImage == (short) 0 && cellInfo.MiddleIndex == (short) 0 && cellInfo.FrontImage == (short) 0 && cellInfo.FrontIndex == (short) 0 && cellInfo.DoorIndex == (byte) 0 && cellInfo.DoorOffset == (byte) 0 && cellInfo.FrontAnimationFrame == (byte) 0 && cellInfo.FrontAnimationTick == (byte) 0 && cellInfo.MiddleAnimationFrame == (byte) 0 && cellInfo.MiddleAnimationTick == (byte) 0 && cellInfo.TileAnimationImage == (short) 0 && cellInfo.TileAnimationOffset == (short) 0 && cellInfo.TileAnimationFrames == (byte) 0 && cellInfo.Light == (byte) 0;

    private void ShandaMir2LibListView_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.selectListItem = this.shangdaMir2ListItem;
      if (this.ShandaMir2LibListView.SelectedIndices.Count <= 0)
        return;
      this.selectLibMImage = Libraries.MapLibs[this.shangdaMir2ListItem.Value].GetMImage(this.ShandaMir2LibListView.SelectedIndices[0]);
      if (this.selectLibMImage != null)
      {
        this.selectImageIndex = this.ShandaMir2LibListView.SelectedIndices[0];
        this.picShandaMir2.Image = (Image) this.selectLibMImage.Image;
        this.labShandaMir2Width.Text = "Width : " + (object) this.selectLibMImage.Width;
        this.labShandaMir2Height.Text = "Height : " + (object) this.selectLibMImage.Height;
        this.labShandaMir2OffSetX.Text = "OffSetX : " + (object) this.selectLibMImage.X;
        this.labshandaMir2OffSetY.Text = "OffSetY : " + (object) this.selectLibMImage.Y;
        Libraries.MapLibs[this.shangdaMir2ListItem.Value].Images[this.ShandaMir2LibListView.SelectedIndices[0]] = (MLibrary.MImage) null;
      }
    }

    private void WemadeMir3LibListView_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.selectListItem = this.wemadeMir3ListItem;
      if (this.WemadeMir3LibListView.SelectedIndices.Count <= 0)
        return;
      this.selectLibMImage = Libraries.MapLibs[this.wemadeMir3ListItem.Value].GetMImage(this.WemadeMir3LibListView.SelectedIndices[0]);
      if (this.selectLibMImage != null)
      {
        this.selectImageIndex = this.WemadeMir3LibListView.SelectedIndices[0];
        this.picWemdeMir3.Image = (Image) this.selectLibMImage.Image;
        this.LabWemadeMir3Width.Text = "Width : " + (object) this.selectLibMImage.Width;
        this.LabWemadeMir3Height.Text = "Height : " + (object) this.selectLibMImage.Height;
        this.labeWemadeMir3OffSetX.Text = "OffSetX : " + (object) this.selectLibMImage.X;
        this.labWemadeMir3OffSetY.Text = "OffSetY : " + (object) this.selectLibMImage.Y;
        Libraries.MapLibs[this.wemadeMir3ListItem.Value].Images[this.WemadeMir3LibListView.SelectedIndices[0]] = (MLibrary.MImage) null;
      }
    }

    private void ShandaMir3LibListView_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.selectListItem = this.shangdaMir3ListItem;
      if (this.ShandaMir3LibListView.SelectedIndices.Count <= 0)
        return;
      this.selectLibMImage = Libraries.MapLibs[this.shangdaMir3ListItem.Value].GetMImage(this.ShandaMir3LibListView.SelectedIndices[0]);
      if (this.selectLibMImage != null)
      {
        this.selectImageIndex = this.ShandaMir3LibListView.SelectedIndices[0];
        this.picShandaMir3.Image = (Image) this.selectLibMImage.Image;
        this.labShandaMir3Width.Text = "Width : " + (object) this.selectLibMImage.Width;
        this.labShandaMir3Height.Text = "Height : " + (object) this.selectLibMImage.Height;
        this.labShandaMir3OffSetX.Text = "OffSetX : " + (object) this.selectLibMImage.X;
        this.labshandaMir3OffSetY.Text = "OffSetY : " + (object) this.selectLibMImage.Y;
        Libraries.MapLibs[this.shangdaMir3ListItem.Value].Images[this.ShandaMir3LibListView.SelectedIndices[0]] = (MLibrary.MImage) null;
      }
    }

    private void chkDrawGrids_Click(object sender, EventArgs e) => this.chkDrawGrids.Checked = !this.chkDrawGrids.Checked;

    private bool AddCellInfoPoints(Point[] points)
    {
      if (points == null || this.M2CellInfo == null)
        return false;
      this.cellInfoDatas = new CellInfoData[points.Length];
      for (int index = 0; index < this.cellInfoDatas.Length; ++index)
      {
        if (points[index].X >= this.mapWidth || points[index].Y >= this.mapHeight || points[index].X < 0 || points[index].Y < 0)
        {
          int num = (int) MessageBox.Show("放置位置不对，或地图过小！");
          return false;
        }
        this.cellInfoDatas[index] = new CellInfoData(points[index].X, points[index].Y, this.M2CellInfo[points[index].X, points[index].Y]);
      }
      this._editor.UnDo = this.cellInfoDatas;
      return true;
    }

    private bool AddCellInfoPoints(CellInfoData[] datas)
    {
      if (datas.Length == 0)
        return false;
      this._editor.UnDo = datas;
      return true;
    }

    private void UnDo()
    {
      this.unTemp = this._editor.UnDo;
      if (this.unTemp == null)
        return;
      this.reTemp = new CellInfoData[this.unTemp.Length];
      for (int index = 0; index < this.unTemp.Length; ++index)
      {
        int x = this.unTemp[index].X;
        int y = this.unTemp[index].Y;
        this.reTemp[index] = new CellInfoData(this.unTemp[index].X, this.unTemp[index].Y, this.M2CellInfo[this.unTemp[index].X, this.unTemp[index].Y]);
        this.M2CellInfo[x, y] = this.unTemp[index].CellInfo;
      }
      this._editor.ReDo = this.reTemp;
    }

    private void ReDo()
    {
      this.reTemp = this._editor.ReDo;
      if (this.reTemp == null)
        return;
      this.unTemp = new CellInfoData[this.reTemp.Length];
      for (int index = 0; index < this.reTemp.Length; ++index)
      {
        int x = this.reTemp[index].X;
        int y = this.reTemp[index].Y;
        this.unTemp[index] = new CellInfoData(this.reTemp[index].X, this.reTemp[index].Y, this.M2CellInfo[this.reTemp[index].X, this.reTemp[index].Y]);
        this.M2CellInfo[x, y] = this.reTemp[index].CellInfo;
      }
      this._editor.UnDo = this.unTemp;
    }

    private void SaveObjectsFile()
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "Object (*.X)|*.X";
      saveFileDialog.FileName = "Object";
      if (saveFileDialog.ShowDialog() != DialogResult.OK)
        return;
      FileStream output = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.ReadWrite);
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
      bool flag = false;
      int num1 = 0;
      int num2 = 0;
      List<CellInfoData> cellInfoDataList = new List<CellInfoData>();
      for (int y = this.p1.Y; y <= this.p2.Y; ++y)
      {
        for (int x = this.p1.X; x <= this.p2.X; ++x)
        {
          if (!flag)
          {
            if (!this.CheckCellInfoIsZero(this.M2CellInfo[x, y]))
            {
              flag = true;
              num1 = x;
              num2 = y;
              cellInfoDataList.Add(new CellInfoData(x - num1, y - num2, this.M2CellInfo[x, y]));
            }
          }
          else if (!this.CheckCellInfoIsZero(this.M2CellInfo[x, y]))
            cellInfoDataList.Add(new CellInfoData(x - num1, y - num2, this.M2CellInfo[x, y]));
        }
      }
      binaryWriter.Write(cellInfoDataList.Count);
      for (int index = 0; index < cellInfoDataList.Count; ++index)
      {
        binaryWriter.Write(cellInfoDataList[index].X);
        binaryWriter.Write(cellInfoDataList[index].Y);
        binaryWriter.Write(cellInfoDataList[index].CellInfo.BackIndex);
        binaryWriter.Write(cellInfoDataList[index].CellInfo.BackImage);
        binaryWriter.Write(cellInfoDataList[index].CellInfo.MiddleIndex);
        binaryWriter.Write(cellInfoDataList[index].CellInfo.MiddleImage);
        binaryWriter.Write(cellInfoDataList[index].CellInfo.FrontIndex);
        binaryWriter.Write(cellInfoDataList[index].CellInfo.FrontImage);
        binaryWriter.Write(cellInfoDataList[index].CellInfo.DoorIndex);
        binaryWriter.Write(cellInfoDataList[index].CellInfo.DoorOffset);
        binaryWriter.Write(cellInfoDataList[index].CellInfo.FrontAnimationFrame);
        binaryWriter.Write(cellInfoDataList[index].CellInfo.FrontAnimationTick);
        binaryWriter.Write(cellInfoDataList[index].CellInfo.MiddleAnimationFrame);
        binaryWriter.Write(cellInfoDataList[index].CellInfo.MiddleAnimationTick);
        binaryWriter.Write(cellInfoDataList[index].CellInfo.TileAnimationImage);
        binaryWriter.Write(cellInfoDataList[index].CellInfo.TileAnimationOffset);
        binaryWriter.Write(cellInfoDataList[index].CellInfo.TileAnimationFrames);
        binaryWriter.Write(cellInfoDataList[index].CellInfo.Light);
      }
      binaryWriter.Flush();
      binaryWriter.Dispose();
      output.Dispose();
    }

    private void MapPanel_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right || this.M2CellInfo == null)
        return;
      switch (this.layer)
      {
        case Main.Layer.GraspingMir2Front:
          this.p1 = new Point(this.cellX, this.cellY);
          this.p2 = this.p1;
          this.Grasping = true;
          break;
        case Main.Layer.GraspingInvertMir3FrontMiddle:
          this.p1 = new Point(this.cellX, this.cellY);
          this.p2 = this.p1;
          this.Grasping = true;
          break;
      }
    }

    private void MapPanel_MouseUp(object sender, MouseEventArgs e)
    {
      if (this.M2CellInfo == null)
        return;
      switch (this.layer)
      {
        case Main.Layer.GraspingMir2Front:
          this.GraspingData();
          this.Grasping = false;
          break;
        case Main.Layer.GraspingInvertMir3FrontMiddle:
          this.GraspingData();
          this.Grasping = false;
          break;
      }
    }

    private void GraspingData()
    {
      if (this.p1.IsEmpty || this.p2.IsEmpty || this.layer != Main.Layer.GraspingMir2Front && this.layer != Main.Layer.GraspingInvertMir3FrontMiddle || this.M2CellInfo == null)
        return;
      this.objectDatas = new CellInfoData[Math.Abs(this.p2.X - this.p1.X + 1) * Math.Abs(this.p2.Y - this.p1.Y + 1)];
      int index = 0;
      int x = this.p1.X;
      int num1 = 0;
      while (x <= this.p2.X)
      {
        int y = this.p1.Y;
        int num2 = 0;
        while (y <= this.p2.Y)
        {
          this.objectDatas[index] = new CellInfoData();
          this.objectDatas[index].CellInfo = new CellInfo();
          this.objectDatas[index].X = x - this.p1.X - 1;
          this.objectDatas[index].Y = y - this.p2.Y - 1;
          this.objectDatas[index].CellInfo.BackImage = this.M2CellInfo[x, y].BackImage & 536870912;
          this.objectDatas[index].CellInfo.BackIndex = (short) 0;
          if (this.layer == Main.Layer.GraspingMir2Front)
          {
            this.objectDatas[index].CellInfo.MiddleImage = (short) 0;
            this.objectDatas[index].CellInfo.MiddleIndex = (short) 0;
          }
          else if (this.layer == Main.Layer.GraspingInvertMir3FrontMiddle)
          {
            this.objectDatas[index].CellInfo.MiddleImage = this.M2CellInfo[x, y].MiddleImage;
            this.objectDatas[index].CellInfo.MiddleIndex = this.M2CellInfo[x, y].MiddleIndex;
          }
          this.objectDatas[index].CellInfo.MiddleAnimationFrame = (byte) 0;
          this.objectDatas[index].CellInfo.MiddleAnimationTick = (byte) 0;
          this.objectDatas[index].CellInfo.FrontImage = this.M2CellInfo[x, y].FrontImage;
          this.objectDatas[index].CellInfo.FrontIndex = this.M2CellInfo[x, y].FrontIndex;
          this.objectDatas[index].CellInfo.FrontAnimationFrame = this.M2CellInfo[x, y].FrontAnimationFrame;
          this.objectDatas[index].CellInfo.FrontAnimationTick = this.M2CellInfo[x, y].FrontAnimationTick;
          this.objectDatas[index].CellInfo.DoorIndex = this.M2CellInfo[x, y].DoorIndex;
          this.objectDatas[index].CellInfo.DoorOffset = this.M2CellInfo[x, y].DoorOffset;
          this.objectDatas[index].CellInfo.TileAnimationImage = this.M2CellInfo[x, y].TileAnimationImage;
          this.objectDatas[index].CellInfo.TileAnimationFrames = this.M2CellInfo[x, y].TileAnimationFrames;
          this.objectDatas[index].CellInfo.TileAnimationOffset = this.M2CellInfo[x, y].TileAnimationOffset;
          this.objectDatas[index].CellInfo.Light = this.M2CellInfo[x, y].Light;
          this.objectDatas[index].CellInfo.FishingCell = this.M2CellInfo[x, y].FishingCell;
          ++index;
          ++y;
          ++num2;
        }
        ++x;
        ++num1;
      }
    }

    private void ReadObjectsToListBox()
    {
      if (!Directory.Exists(".\\Data\\Objects\\"))
        Directory.CreateDirectory(".\\Data\\Objects\\");
      string[] array = Directory.EnumerateFileSystemEntries(".\\Data\\Objects\\", "*.X", SearchOption.AllDirectories).OrderBy<string, string>((Func<string, string>) (x => x)).ToArray<string>();
      Array.Sort((Array) array, (IComparer) new AlphanumComparatorFast());
      foreach (string str in array)
        this.ObjectslistBox.Items.Add((object) str.Replace(".\\Data\\Objects\\", "").Replace(".X", ""));
    }

    private CellInfoData[] ReadObjectsFile(string objectFile)
    {
      int startIndex1 = 0;
      CellInfoData[] cellInfoDataArray = (CellInfoData[]) null;
      if (File.Exists(objectFile))
      {
        byte[] numArray1 = File.ReadAllBytes(objectFile);
        int int32 = BitConverter.ToInt32(numArray1, startIndex1);
        int startIndex2 = startIndex1 + 4;
        cellInfoDataArray = new CellInfoData[int32];
        for (int index1 = 0; index1 < int32; ++index1)
        {
          cellInfoDataArray[index1] = new CellInfoData();
          cellInfoDataArray[index1].CellInfo = new CellInfo();
          cellInfoDataArray[index1].X = BitConverter.ToInt32(numArray1, startIndex2);
          int startIndex3 = startIndex2 + 4;
          cellInfoDataArray[index1].Y = BitConverter.ToInt32(numArray1, startIndex3);
          int startIndex4 = startIndex3 + 4;
          cellInfoDataArray[index1].CellInfo.BackIndex = BitConverter.ToInt16(numArray1, startIndex4);
          int startIndex5 = startIndex4 + 2;
          cellInfoDataArray[index1].CellInfo.BackImage = BitConverter.ToInt32(numArray1, startIndex5);
          int startIndex6 = startIndex5 + 4;
          cellInfoDataArray[index1].CellInfo.MiddleIndex = BitConverter.ToInt16(numArray1, startIndex6);
          int startIndex7 = startIndex6 + 2;
          cellInfoDataArray[index1].CellInfo.MiddleImage = BitConverter.ToInt16(numArray1, startIndex7);
          int startIndex8 = startIndex7 + 2;
          cellInfoDataArray[index1].CellInfo.FrontIndex = BitConverter.ToInt16(numArray1, startIndex8);
          int startIndex9 = startIndex8 + 2;
          cellInfoDataArray[index1].CellInfo.FrontImage = BitConverter.ToInt16(numArray1, startIndex9);
          int num1 = startIndex9 + 2;
          CellInfo cellInfo1 = cellInfoDataArray[index1].CellInfo;
          byte[] numArray2 = numArray1;
          int index2 = num1;
          int num2 = index2 + 1;
          int num3 = (int) numArray2[index2];
          cellInfo1.DoorIndex = (byte) num3;
          CellInfo cellInfo2 = cellInfoDataArray[index1].CellInfo;
          byte[] numArray3 = numArray1;
          int index3 = num2;
          int num4 = index3 + 1;
          int num5 = (int) numArray3[index3];
          cellInfo2.DoorOffset = (byte) num5;
          CellInfo cellInfo3 = cellInfoDataArray[index1].CellInfo;
          byte[] numArray4 = numArray1;
          int index4 = num4;
          int num6 = index4 + 1;
          int num7 = (int) numArray4[index4];
          cellInfo3.FrontAnimationFrame = (byte) num7;
          CellInfo cellInfo4 = cellInfoDataArray[index1].CellInfo;
          byte[] numArray5 = numArray1;
          int index5 = num6;
          int num8 = index5 + 1;
          int num9 = (int) numArray5[index5];
          cellInfo4.FrontAnimationTick = (byte) num9;
          CellInfo cellInfo5 = cellInfoDataArray[index1].CellInfo;
          byte[] numArray6 = numArray1;
          int index6 = num8;
          int num10 = index6 + 1;
          int num11 = (int) numArray6[index6];
          cellInfo5.MiddleAnimationFrame = (byte) num11;
          CellInfo cellInfo6 = cellInfoDataArray[index1].CellInfo;
          byte[] numArray7 = numArray1;
          int index7 = num10;
          int startIndex10 = index7 + 1;
          int num12 = (int) numArray7[index7];
          cellInfo6.MiddleAnimationTick = (byte) num12;
          cellInfoDataArray[index1].CellInfo.TileAnimationImage = BitConverter.ToInt16(numArray1, startIndex10);
          int startIndex11 = startIndex10 + 2;
          cellInfoDataArray[index1].CellInfo.TileAnimationOffset = BitConverter.ToInt16(numArray1, startIndex11);
          int num13 = startIndex11 + 2;
          CellInfo cellInfo7 = cellInfoDataArray[index1].CellInfo;
          byte[] numArray8 = numArray1;
          int index8 = num13;
          int num14 = index8 + 1;
          int num15 = (int) numArray8[index8];
          cellInfo7.TileAnimationFrames = (byte) num15;
          CellInfo cellInfo8 = cellInfoDataArray[index1].CellInfo;
          byte[] numArray9 = numArray1;
          int index9 = num14;
          startIndex2 = index9 + 1;
          int num16 = (int) numArray9[index9];
          cellInfo8.Light = (byte) num16;
          if (cellInfoDataArray[index1].CellInfo.Light == (byte) 100 || cellInfoDataArray[index1].CellInfo.Light == (byte) 101)
            cellInfoDataArray[index1].CellInfo.FishingCell = true;
        }
      }
      return cellInfoDataArray;
    }

    private Bitmap GetObjectPreview(int w, int h, CellInfoData[] datas)
    {
      if (datas == null)
        return (Bitmap) null;
      Bitmap objectPreview = new Bitmap(w * 48, h * 32);
      Graphics graphics = Graphics.FromImage((Image) objectPreview);
      graphics.InterpolationMode = InterpolationMode.Low;
      for (int index = 0; index < datas.Length; ++index)
      {
        if ((uint) (datas[index].Y % 2) <= 0U)
        {
          this.drawY = (datas[index].Y + h / 4) * 32;
          if ((uint) (datas[index].X % 2) <= 0U)
          {
            this.drawX = (datas[index].X + w / 4) * 48;
            this.index = (datas[index].CellInfo.BackImage & 536870911) - 1;
            this.libIndex = (int) datas[index].CellInfo.BackIndex;
            if (this.libIndex >= 0 && this.libIndex < Libraries.MapLibs.Length && this.index >= 0 && this.index < Libraries.MapLibs[this.libIndex].Images.Count)
            {
              Libraries.MapLibs[this.libIndex].CheckImage(this.index);
              MLibrary.MImage image = Libraries.MapLibs[this.libIndex].Images[this.index];
              if (image.Image != null)
              {
                Rectangle destRect = new Rectangle(this.drawX, this.drawY, (int) image.Width, (int) image.Height);
                Rectangle srcRect = new Rectangle(0, 0, (int) image.Width, (int) image.Height);
                graphics.DrawImage((Image) image.Image, destRect, srcRect, GraphicsUnit.Pixel);
                Libraries.MapLibs[this.libIndex].Images[this.index] = (MLibrary.MImage) null;
              }
            }
          }
        }
      }
      for (int index = 0; index < datas.Length; ++index)
      {
        this.drawX = (datas[index].X + w / 4) * 48;
        this.index = (int) datas[index].CellInfo.MiddleImage - 1;
        this.libIndex = (int) datas[index].CellInfo.MiddleIndex;
        if (this.libIndex >= 0 && this.libIndex < Libraries.MapLibs.Length && this.index >= 0 && this.index < Libraries.MapLibs[this.libIndex].Images.Count)
        {
          Size size = Libraries.MapLibs[this.libIndex].GetSize(this.index);
          this.drawY = size.Width == 48 && size.Height == 32 || size.Width == 96 && size.Height == 64 ? (datas[index].Y + h / 4) * 32 : (datas[index].Y + 1 + h / 4) * 32 - size.Height;
          Libraries.MapLibs[this.libIndex].CheckImage(this.index);
          MLibrary.MImage image = Libraries.MapLibs[this.libIndex].Images[this.index];
          if (image.Image != null)
          {
            Rectangle destRect = new Rectangle(this.drawX, this.drawY, (int) image.Width, (int) image.Height);
            Rectangle srcRect = new Rectangle(0, 0, (int) image.Width, (int) image.Height);
            graphics.DrawImage((Image) image.Image, destRect, srcRect, GraphicsUnit.Pixel);
            Libraries.MapLibs[this.libIndex].Images[this.index] = (MLibrary.MImage) null;
          }
        }
      }
      for (int index = 0; index < datas.Length; ++index)
      {
        this.drawX = (datas[index].X + w / 4) * 48;
        this.index = ((int) datas[index].CellInfo.FrontImage & (int) short.MaxValue) - 1;
        this.libIndex = (int) datas[index].CellInfo.FrontIndex;
        if (this.libIndex >= 0 && this.libIndex < Libraries.MapLibs.Length && this.index >= 0 && this.index < Libraries.MapLibs[this.libIndex].Images.Count)
        {
          Size size = Libraries.MapLibs[this.libIndex].GetSize(this.index);
          this.drawY = size.Width == 48 && size.Height == 32 || size.Width == 96 && size.Height == 64 ? (datas[index].Y + h / 4) * 32 : (datas[index].Y + 1 + h / 4) * 32 - size.Height;
          Libraries.MapLibs[this.libIndex].CheckImage(this.index);
          MLibrary.MImage image = Libraries.MapLibs[this.libIndex].Images[this.index];
          if (image.Image != null)
          {
            Rectangle destRect = new Rectangle(this.drawX, this.drawY, (int) image.Width, (int) image.Height);
            Rectangle srcRect = new Rectangle(0, 0, (int) image.Width, (int) image.Height);
            graphics.DrawImage((Image) image.Image, destRect, srcRect, GraphicsUnit.Pixel);
            Libraries.MapLibs[this.libIndex].Images[this.index] = (MLibrary.MImage) null;
          }
        }
      }
      graphics.Save();
      graphics.Dispose();
      return objectPreview;
    }

    private void chkFrontTag_Click(object sender, EventArgs e) => this.chkFrontTag.Checked = !this.chkFrontTag.Checked;

    private void btnDeleteObjects_Click(object sender, EventArgs e)
    {
      string path = Application.StartupPath + "\\Data\\Objects\\" + (this.ObjectslistBox.SelectedItem.ToString() + ".X");
      if (!File.Exists(path))
        return;
      File.Delete(path);
      this.ObjectslistBox.Items.Clear();
      this.ReadObjectsToListBox();
    }

    private void ClearAll() => this.M2CellInfo[this.cellX, this.cellY] = new CellInfo();

    private void ClearBack()
    {
      this.M2CellInfo[this.cellX, this.cellY].BackIndex = (short) 0;
      this.M2CellInfo[this.cellX, this.cellY].BackImage = 0;
    }

    private void ClearMidd()
    {
      this.M2CellInfo[this.cellX, this.cellY].MiddleImage = (short) 0;
      this.M2CellInfo[this.cellX, this.cellY].MiddleIndex = (short) 0;
      this.M2CellInfo[this.cellX, this.cellY].MiddleAnimationFrame = (byte) 0;
      this.M2CellInfo[this.cellX, this.cellY].MiddleAnimationTick = (byte) 0;
    }

    private void ClearFront()
    {
      this.M2CellInfo[this.cellX, this.cellY].FrontImage = (short) 0;
      this.M2CellInfo[this.cellX, this.cellY].FrontIndex = (short) 0;
      this.M2CellInfo[this.cellX, this.cellY].FrontAnimationFrame = (byte) 0;
      this.M2CellInfo[this.cellX, this.cellY].FrontAnimationTick = (byte) 0;
    }

    private void ClearBackLimit() => this.M2CellInfo[this.cellX, this.cellY].BackImage &= 536870911;

    private void ClearFrontLimit() => this.M2CellInfo[this.cellX, this.cellY].FrontImage &= short.MaxValue;

    private void 撤销ToolStripMenuItem_Click(object sender, EventArgs e) => this.UnDo();

    private void 返回ToolStripMenuItem_Click(object sender, EventArgs e) => this.ReDo();

    private void btnSetDoor_Click(object sender, EventArgs e)
    {
      if (this.M2CellInfo == null || new FrmSetDoor(new Main.DelSetDoorProperty(this.SetDoorProperty)).ShowDialog() != DialogResult.OK)
        ;
    }

    private void SetDoorProperty(bool blCoreDoor, byte index, byte offSet)
    {
      this.AddCellInfoPoints(new Point[1]
      {
        new Point(this.cellX, this.cellY)
      });
      this.M2CellInfo[this.cellX, this.cellY].DoorIndex |= index;
      this.M2CellInfo[this.cellX, this.cellY].DoorOffset = offSet;
      if (blCoreDoor)
        this.M2CellInfo[this.cellX, this.cellY].DoorIndex |= (byte) 128;
      else
        this.M2CellInfo[this.cellX, this.cellY].DoorIndex &= (byte) 127;
    }

    private void btnSetAnimation_Click(object sender, EventArgs e)
    {
      if (this.M2CellInfo == null || new FrmSetAnimation(new Main.DelSetAnimationProperty(this.SetAnimationProperty)).ShowDialog() != DialogResult.OK)
        ;
    }

    private void SetAnimationProperty(bool blend, byte frame, byte tick)
    {
      this.AddCellInfoPoints(new Point[1]
      {
        new Point(this.cellX, this.cellY)
      });
      this.M2CellInfo[this.cellX, this.cellY].FrontAnimationFrame = frame;
      this.M2CellInfo[this.cellX, this.cellY].FrontAnimationTick = tick;
      if (blend)
        this.M2CellInfo[this.cellX, this.cellY].FrontAnimationFrame |= (byte) 128;
      else
        this.M2CellInfo[this.cellX, this.cellY].FrontAnimationFrame &= (byte) 127;
    }

    private void btnSetLight_Click(object sender, EventArgs e)
    {
      if (this.M2CellInfo == null || new FrmSetLight(new Main.DelSetLightProperty(this.SetLightProperty)).ShowDialog() != DialogResult.OK)
        ;
    }

    private void SetLightProperty(byte light)
    {
      this.AddCellInfoPoints(new Point[1]
      {
        new Point(this.cellX, this.cellY)
      });
      this.M2CellInfo[this.cellX, this.cellY].Light = light;
    }

    private void SetBackLimit()
    {
      if (this.M2CellInfo == null)
        return;
      this.M2CellInfo[this.cellX, this.cellY].BackImage |= 536870912;
    }

    private void SetFrontLimit()
    {
      if (this.M2CellInfo == null)
        return;
      this.M2CellInfo[this.cellX, this.cellY].FrontImage |= short.MinValue;
    }

    private void btnJump_Click(object sender, EventArgs e)
    {
      if (this.M2CellInfo == null || new FrmJump(new Main.DelJump(this.Jump)).ShowDialog() != DialogResult.OK)
        ;
    }

    private void Jump(int x, int y)
    {
      this.mapPoint.X = x;
      this.mapPoint.Y = y;
      if (this.mapPoint.X + this.OffSetX >= this.mapWidth)
        this.mapPoint.X = this.mapWidth - this.OffSetX - 1;
      if (this.mapPoint.X < 0)
        this.mapPoint.X = 0;
      if (this.mapPoint.Y < 0)
        this.mapPoint.Y = 0;
      if (this.mapPoint.Y + this.OffSetY >= this.mapHeight)
        this.mapPoint.Y = this.mapHeight - this.OffSetY - 1;
      this.setScrollBar();
    }

    private void Main_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.KeyCode)
      {
        case Keys.Escape:
          this.layer = Main.Layer.None;
          this.cmbEditorLayer.SelectedIndex = 0;
          break;
        case Keys.D0:
          this.layer = Main.Layer.BrushMir2BigTiles;
          this.cmbEditorLayer.SelectedIndex = 17;
          break;
        case Keys.D2:
          this.layer = Main.Layer.FrontImage;
          this.cmbEditorLayer.SelectedIndex = 3;
          break;
        case Keys.D3:
          this.layer = Main.Layer.MiddleImage;
          this.cmbEditorLayer.SelectedIndex = 2;
          break;
        case Keys.D4:
          this.layer = Main.Layer.BackImage;
          this.cmbEditorLayer.SelectedIndex = 1;
          break;
        case Keys.D5:
          this.layer = Main.Layer.PlaceObjects;
          this.cmbEditorLayer.SelectedIndex = 9;
          break;
        case Keys.D7:
          this.layer = Main.Layer.ClearFront;
          this.cmbEditorLayer.SelectedIndex = 13;
          break;
        case Keys.D8:
          this.layer = Main.Layer.ClearMidd;
          this.cmbEditorLayer.SelectedIndex = 12;
          break;
        case Keys.D9:
          this.layer = Main.Layer.ClearBack;
          this.cmbEditorLayer.SelectedIndex = 11;
          break;
        case Keys.A:
          this.Jump(this.mapPoint.X - 1, this.mapPoint.Y);
          break;
        case Keys.D:
          this.Jump(this.mapPoint.X + 1, this.mapPoint.Y);
          break;
        case Keys.G:
          this.chkDrawGrids.Checked = !this.chkDrawGrids.Checked;
          break;
        case Keys.J:
          if (this.M2CellInfo == null || new FrmJump(new Main.DelJump(this.Jump)).ShowDialog() != DialogResult.OK)
            break;
          break;
        case Keys.N:
          this.NewMap();
          break;
        case Keys.O:
          this.OpenMap();
          break;
        case Keys.R:
          this.tabControl1.SelectedTab = this.tabObjects;
          break;
        case Keys.S:
          this.Jump(this.mapPoint.X, this.mapPoint.Y + 1);
          break;
        case Keys.T:
          this.tabControl1.SelectedTab = this.tabTiles;
          break;
        case Keys.W:
          this.Jump(this.mapPoint.X, this.mapPoint.Y - 1);
          break;
        case Keys.Z:
          if (e.Control)
          {
            this.ReDo();
            break;
          }
          this.UnDo();
          break;
        case Keys.Add:
          if (e.Shift)
            Main.zoomMIN = Main.zoomMAX;
          this.ZoomIn();
          break;
        case Keys.Subtract:
          if (e.Shift)
            Main.zoomMIN = 1;
          this.ZoomOut();
          break;
        case Keys.F1:
          e.SuppressKeyPress = true;
          this.tabControl1.SelectedTab = this.tabWemadeMir2;
          break;
        case Keys.F2:
          e.SuppressKeyPress = true;
          this.tabControl1.SelectedTab = this.tabShandaMir2;
          break;
        case Keys.F3:
          e.SuppressKeyPress = true;
          this.tabControl1.SelectedTab = this.tabWemadeMir3;
          break;
        case Keys.F4:
          e.SuppressKeyPress = true;
          this.tabControl1.SelectedTab = this.tabShandaMir3;
          break;
        case Keys.F5:
          e.SuppressKeyPress = true;
          this.tabControl1.SelectedTab = this.tabMap;
          break;
        case Keys.F6:
          e.SuppressKeyPress = true;
          this.chkFront.Checked = !this.chkFront.Checked;
          break;
        case Keys.F7:
          e.SuppressKeyPress = true;
          this.chkMidd.Checked = !this.chkMidd.Checked;
          break;
        case Keys.F8:
          e.SuppressKeyPress = true;
          this.chkBack.Checked = !this.chkBack.Checked;
          break;
        case Keys.F9:
          e.SuppressKeyPress = true;
          this.chkShowCellInfo.Checked = !this.chkShowCellInfo.Checked;
          this.cellInfoControl.Visible = this.chkShowCellInfo.Checked;
          break;
        case Keys.F10:
          e.SuppressKeyPress = true;
          this.chkFrontTag.Checked = !this.chkFrontTag.Checked;
          break;
        case Keys.F11:
          e.SuppressKeyPress = true;
          this.chkMiddleTag.Checked = !this.chkMiddleTag.Checked;
          break;
        case Keys.F12:
          e.SuppressKeyPress = true;
          break;
        case Keys.Oemplus:
          this.layer = Main.Layer.BrushSmTiles;
          this.cmbEditorLayer.SelectedIndex = 18;
          break;
        case Keys.Oemcomma:
          --this.selectImageIndex;
          break;
        case Keys.OemMinus:
          this.layer = Main.Layer.BrushMir3BigTiles;
          this.cmbEditorLayer.SelectedIndex = 19;
          break;
        case Keys.OemPeriod:
          ++this.selectImageIndex;
          break;
        case Keys.Oemtilde:
          this.layer = Main.Layer.None;
          this.cmbEditorLayer.SelectedIndex = 0;
          break;
        case Keys.Oem8:
          this.layer = Main.Layer.None;
          this.cmbEditorLayer.SelectedIndex = 0;
          break;
      }
      if (this.M2CellInfo == null)
        return;
      if (this.layer == Main.Layer.BackImage && e.Control)
        this.keyDown = true;
      if ((this.layer == Main.Layer.BrushMir2BigTiles || this.layer == Main.Layer.BrushSmTiles || this.layer == Main.Layer.BrushMir3BigTiles) && e.Control)
        this.keyDown = true;
    }

    private void Main_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyData != Keys.ControlKey)
        return;
      this.keyDown = false;
    }

    private void ObjectslistBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.objectDatas = this.ReadObjectsFile(Application.StartupPath + "\\Data\\Objects\\" + (this.ObjectslistBox.SelectedItem.ToString() + ".X"));
      this.picObjects.Image = (Image) this.GetObjectPreview(26, 24, this.objectDatas);
    }

    private void chkMiddleTag_Click(object sender, EventArgs e) => this.chkMiddleTag.Checked = !this.chkMiddleTag.Checked;

    private void Main_FormClosing(object sender, FormClosingEventArgs e) => this.Dispose();

    private void chkShowCellInfo_Click(object sender, EventArgs e)
    {
      this.chkShowCellInfo.Checked = !this.chkShowCellInfo.Checked;
      this.cellInfoControl.Visible = this.chkShowCellInfo.Checked;
    }

    private void ShowCellInfo(bool blShow)
    {
      if (!blShow)
        return;
      this.cellInfoControl.SetText(this.cellX, this.cellY, (this.M2CellInfo[this.cellX, this.cellY].BackImage & 131071) - 1, (int) this.M2CellInfo[this.cellX, this.cellY].MiddleImage - 1, ((int) this.M2CellInfo[this.cellX, this.cellY].FrontImage & (int) short.MaxValue) - 1, (int) this.M2CellInfo[this.cellX, this.cellY].BackIndex, (int) this.M2CellInfo[this.cellX, this.cellY].MiddleIndex, (int) this.M2CellInfo[this.cellX, this.cellY].FrontIndex, this.GetLibName((int) this.M2CellInfo[this.cellX, this.cellY].BackIndex), this.GetLibName((int) this.M2CellInfo[this.cellX, this.cellY].MiddleIndex), this.GetLibName((int) this.M2CellInfo[this.cellX, this.cellY].FrontIndex), this.M2CellInfo[this.cellX, this.cellY].BackImage & 536870912, (int) this.M2CellInfo[this.cellX, this.cellY].FrontImage & 32768, (byte) ((uint) this.M2CellInfo[this.cellX, this.cellY].FrontAnimationFrame & (uint) sbyte.MaxValue), this.M2CellInfo[this.cellX, this.cellY].FrontAnimationTick, Convert.ToBoolean((int) this.M2CellInfo[this.cellX, this.cellY].FrontAnimationFrame & 128), this.M2CellInfo[this.cellX, this.cellY].MiddleAnimationFrame, this.M2CellInfo[this.cellX, this.cellY].MiddleAnimationTick, Convert.ToBoolean(this.M2CellInfo[this.cellX, this.cellY].MiddleAnimationFrame), this.M2CellInfo[this.cellX, this.cellY].DoorOffset, (byte) ((uint) this.M2CellInfo[this.cellX, this.cellY].DoorIndex & (uint) sbyte.MaxValue), Convert.ToBoolean((int) this.M2CellInfo[this.cellX, this.cellY].DoorIndex & 128), this.M2CellInfo[this.cellX, this.cellY].Light, this.M2CellInfo[this.cellX, this.cellY].FishingCell);
      this.drawX = (this.cellX - this.mapPoint.X + 1) * (48 * Main.zoomMIN / Main.zoomMAX);
      this.drawY = (this.cellY - this.mapPoint.Y + 1) * (32 * Main.zoomMIN / Main.zoomMAX);
      if (this.drawX + this.cellInfoControl.Width >= this.MapPanel.Width)
        this.drawX = this.drawX - this.cellInfoControl.Width - 1;
      this.cellInfoControl.Location = new Point(this.drawX, this.drawY);
      if (!this.MapPanel.Controls.Contains((Control) this.cellInfoControl))
        this.MapPanel.Controls.Add((Control) this.cellInfoControl);
    }

    private void btnMir3ToMir2_Click(object sender, EventArgs e)
    {
    }

    private void WemadeMir2LibListView_Click(object sender, EventArgs e)
    {
      this.selectListItem = this.wemadeMir2ListItem;
      if (this.WemadeMir2LibListView.SelectedIndices.Count <= 0)
        return;
      this.selectLibMImage = Libraries.MapLibs[this.wemadeMir2ListItem.Value].GetMImage(this.WemadeMir2LibListView.SelectedIndices[0]);
      if (this.selectLibMImage != null)
      {
        this.selectImageIndex = this.WemadeMir2LibListView.SelectedIndices[0];
        this.picWemdeMir2.Image = (Image) this.selectLibMImage.Image;
        this.LabWemadeMir2Width.Text = "Width : " + (object) this.selectLibMImage.Width;
        this.LabWemadeMir2Height.Text = "Height : " + (object) this.selectLibMImage.Height;
        this.labeWemadeMir2OffSetX.Text = "OffSetX : " + (object) this.selectLibMImage.X;
        this.labWemadeMir2OffSetY.Text = "OffSetY : " + (object) this.selectLibMImage.Y;
        Libraries.MapLibs[this.wemadeMir2ListItem.Value].Images[this.WemadeMir2LibListView.SelectedIndices[0]] = (MLibrary.MImage) null;
      }
    }

    private void ShandaMir2LibListView_Click(object sender, EventArgs e)
    {
      this.selectListItem = this.shangdaMir2ListItem;
      if (this.ShandaMir2LibListView.SelectedIndices.Count <= 0)
        return;
      this.selectLibMImage = Libraries.MapLibs[this.shangdaMir2ListItem.Value].GetMImage(this.ShandaMir2LibListView.SelectedIndices[0]);
      if (this.selectLibMImage != null)
      {
        this.selectImageIndex = this.ShandaMir2LibListView.SelectedIndices[0];
        this.picShandaMir2.Image = (Image) this.selectLibMImage.Image;
        this.labShandaMir2Width.Text = "Width : " + (object) this.selectLibMImage.Width;
        this.labShandaMir2Height.Text = "Height : " + (object) this.selectLibMImage.Height;
        this.labShandaMir2OffSetX.Text = "OffSetX : " + (object) this.selectLibMImage.X;
        this.labshandaMir2OffSetY.Text = "OffSetY : " + (object) this.selectLibMImage.Y;
        Libraries.MapLibs[this.shangdaMir2ListItem.Value].Images[this.ShandaMir2LibListView.SelectedIndices[0]] = (MLibrary.MImage) null;
      }
    }

    private void WemadeMir3LibListView_Click(object sender, EventArgs e)
    {
      this.selectListItem = this.wemadeMir3ListItem;
      if (this.WemadeMir3LibListView.SelectedIndices.Count <= 0)
        return;
      this.selectLibMImage = Libraries.MapLibs[this.wemadeMir3ListItem.Value].GetMImage(this.WemadeMir3LibListView.SelectedIndices[0]);
      if (this.selectLibMImage != null)
      {
        this.selectImageIndex = this.WemadeMir3LibListView.SelectedIndices[0];
        this.picWemdeMir3.Image = (Image) this.selectLibMImage.Image;
        this.LabWemadeMir3Width.Text = "Width : " + (object) this.selectLibMImage.Width;
        this.LabWemadeMir3Height.Text = "Height : " + (object) this.selectLibMImage.Height;
        this.labeWemadeMir3OffSetX.Text = "OffSetX : " + (object) this.selectLibMImage.X;
        this.labWemadeMir3OffSetY.Text = "OffSetY : " + (object) this.selectLibMImage.Y;
        Libraries.MapLibs[this.wemadeMir3ListItem.Value].Images[this.WemadeMir3LibListView.SelectedIndices[0]] = (MLibrary.MImage) null;
      }
    }

    private void ShandaMir3LibListView_Click(object sender, EventArgs e)
    {
      this.selectListItem = this.shangdaMir3ListItem;
      if (this.ShandaMir3LibListView.SelectedIndices.Count <= 0)
        return;
      this.selectLibMImage = Libraries.MapLibs[this.shangdaMir3ListItem.Value].GetMImage(this.ShandaMir3LibListView.SelectedIndices[0]);
      if (this.selectLibMImage != null)
      {
        this.selectImageIndex = this.ShandaMir3LibListView.SelectedIndices[0];
        this.picShandaMir3.Image = (Image) this.selectLibMImage.Image;
        this.labShandaMir3Width.Text = "Width : " + (object) this.selectLibMImage.Width;
        this.labShandaMir3Height.Text = "Height : " + (object) this.selectLibMImage.Height;
        this.labShandaMir3OffSetX.Text = "OffSetX : " + (object) this.selectLibMImage.X;
        this.labshandaMir3OffSetY.Text = "OffSetY : " + (object) this.selectLibMImage.Y;
        Libraries.MapLibs[this.shangdaMir3ListItem.Value].Images[this.ShandaMir3LibListView.SelectedIndices[0]] = (MLibrary.MImage) null;
      }
    }

    private void CreateMir2BigTiles()
    {
      if (this.selectListItem == null || this.selectTilesIndex == -1)
        return;
      this.bigTilePoints.Clear();
      this.PutAutoTile(this.cellX, this.cellY, this.RandomAutoMir2Tile(Main.TileType.Center));
      this.DrawAutoMir2TileSide(this.cellX, this.cellY);
      Main.AutoTileRange = 2;
      for (int index = 0; index < 30; ++index)
      {
        Main.AutoTileChanges = 0;
        this.DrawAutoMir2TilePattern(this.cellX, this.cellY);
        if (Main.AutoTileChanges == 0)
          break;
        Main.AutoTileRange += 2;
      }
    }

    private void CreateMir3BigTiles()
    {
      if (this.selectListItem == null || this.selectTilesIndex == -1)
        return;
      this.bigTilePoints.Clear();
      this.PutAutoTile(this.cellX, this.cellY, this.RandomAutoMir3Tile(Main.TileType.Center));
      this.DrawAutoMir3TileSide(this.cellX, this.cellY);
      Main.AutoTileRange = 2;
      for (int index = 0; index < 30; ++index)
      {
        Main.AutoTileChanges = 0;
        this.DrawAutoMir3TilePattern(this.cellX, this.cellY);
        if (Main.AutoTileChanges == 0)
          break;
        Main.AutoTileRange += 2;
      }
    }

    private void CreateSmTiles()
    {
      if (this.selectListItem == null || this.selectTilesIndex == -1)
        return;
      this.smTilePoints.Clear();
      this.PutAutoSmTile(this.cellX, this.cellY, this.RandomAutoSmTile(Main.TileType.Center));
      this.DrawAutoSmTileSide(this.cellX, this.cellY);
      Main.AutoTileRange = 2;
      for (int index = 0; index < 30; ++index)
      {
        Main.AutoTileChanges = 0;
        this.DrawAutoSmTilePattern(this.cellX, this.cellY);
        if (Main.AutoTileChanges == 0)
          break;
        Main.AutoTileRange += 2;
      }
    }

    private int GetTile(int x, int y) => x < 0 || y < 0 || x >= this.mapWidth || y >= this.mapHeight ? -1 : (this.M2CellInfo[x, y].BackImage & 536870911) - 1;

    private int GetSmTile(int x, int y) => x < 0 || y < 0 || x >= this.mapWidth || y >= this.mapHeight ? -1 : (int) this.M2CellInfo[x, y].MiddleImage - 1;

    private Main.TileType GetAutoMir2TileType(int x, int y)
    {
      int tile = this.GetTile(x, y);
      if (tile / 50 != this.selectTilesIndex)
        return Main.TileType.None;
      switch (tile % 50)
      {
        case 0:
        case 1:
        case 2:
        case 3:
        case 4:
          return Main.TileType.Center;
        case 5:
          return Main.TileType.UpLeft;
        case 6:
          return Main.TileType.UpRight;
        case 7:
          return Main.TileType.DownLeft;
        case 8:
          return Main.TileType.DownRight;
        case 10:
          return Main.TileType.InUpLeft;
        case 11:
          return Main.TileType.InUpRight;
        case 12:
          return Main.TileType.InDownLeft;
        case 13:
          return Main.TileType.InDownRight;
        case 15:
        case 16:
          return Main.TileType.Up;
        case 17:
        case 18:
          return Main.TileType.Down;
        case 20:
        case 22:
          return Main.TileType.Left;
        case 21:
        case 23:
          return Main.TileType.Right;
        default:
          return Main.TileType.None;
      }
    }

    private Main.TileType GetAutoMir3TileType(int x, int y)
    {
      int tile = this.GetTile(x, y);
      int num = (uint) (Libraries.MapLibs[this.selectListItem.Value].Images.Count % 10) <= 0U ? Libraries.MapLibs[this.selectListItem.Value].Images.Count / 30 : (Libraries.MapLibs[this.selectListItem.Value].Images.Count + 1) / 30;
      if (tile / 30 != this.selectTilesIndex && tile / 30 != this.selectTilesIndex - num)
        return Main.TileType.None;
      if (this.selectTilesIndex < num)
      {
        switch (tile % 30)
        {
          case 0:
          case 1:
          case 2:
          case 3:
          case 4:
            return Main.TileType.Center;
          case 10:
            return Main.TileType.UpLeft;
          case 11:
            return Main.TileType.UpRight;
          case 12:
            return Main.TileType.DownLeft;
          case 13:
            return Main.TileType.DownRight;
          case 15:
            return Main.TileType.InUpLeft;
          case 16:
            return Main.TileType.InUpRight;
          case 17:
            return Main.TileType.InDownLeft;
          case 18:
            return Main.TileType.InDownRight;
          case 20:
          case 21:
            return Main.TileType.Up;
          case 22:
          case 23:
            return Main.TileType.Down;
          case 25:
          case 27:
            return Main.TileType.Left;
          case 26:
          case 28:
            return Main.TileType.Right;
        }
      }
      else
      {
        switch (tile % 30)
        {
          case 5:
          case 6:
          case 7:
          case 8:
          case 9:
            return Main.TileType.Center;
          case 10:
            return Main.TileType.InDownRight;
          case 11:
            return Main.TileType.InDownLeft;
          case 12:
            return Main.TileType.InUpRight;
          case 13:
            return Main.TileType.InUpLeft;
          case 15:
            return Main.TileType.DownRight;
          case 16:
            return Main.TileType.DownLeft;
          case 17:
            return Main.TileType.UpRight;
          case 18:
            return Main.TileType.UpLeft;
          case 20:
          case 21:
            return Main.TileType.Down;
          case 22:
          case 23:
            return Main.TileType.Up;
          case 25:
          case 27:
            return Main.TileType.Right;
          case 26:
          case 28:
            return Main.TileType.Left;
        }
      }
      return Main.TileType.None;
    }

    private Main.TileType GetAutoSmTileType(int iX, int iY)
    {
      int smTile = this.GetSmTile(iX, iY);
      if (smTile < this.selectTilesIndex * 60 || smTile >= (this.selectTilesIndex + 1) * 60)
        return Main.TileType.None;
      int num = smTile - this.selectTilesIndex * 60;
      return num < 8 ? Main.TileType.Center : (Main.TileType) ((num - 8) / 4 + 1);
    }

    private void PutAutoTile(int x, int y, int imageIndex)
    {
      if (x < 0 || y < 0 || x >= this.mapWidth || y >= this.mapHeight)
        return;
      ++Main.AutoTileChanges;
      for (int index = 0; index < this.bigTilePoints.Count; ++index)
      {
        if (this.bigTilePoints[index].X == x && this.bigTilePoints[index].Y == y)
        {
          this.M2CellInfo[x, y].BackImage = imageIndex + 1;
          this.M2CellInfo[x, y].BackIndex = (short) this.selectListItem.Value;
          return;
        }
      }
      this.bigTilePoints.Add(new CellInfoData(x, y, this.M2CellInfo[x, y]));
      this.M2CellInfo[x, y].BackImage = imageIndex + 1;
      this.M2CellInfo[x, y].BackIndex = (short) this.selectListItem.Value;
    }

    private void PutAutoSmTile(int x, int y, int imageIndex)
    {
      if (x < 0 || y < 0 || x >= this.mapWidth || y >= this.mapHeight)
        return;
      ++Main.AutoTileChanges;
      for (int index = 0; index < this.smTilePoints.Count; ++index)
      {
        if (this.smTilePoints[index].X == x && this.smTilePoints[index].Y == y)
        {
          this.M2CellInfo[x, y].MiddleImage = (short) (imageIndex + 1);
          this.M2CellInfo[x, y].MiddleIndex = (short) this.selectListItem.Value;
          return;
        }
      }
      this.smTilePoints.Add(new CellInfoData(x, y, this.M2CellInfo[x, y]));
      this.M2CellInfo[x, y].MiddleImage = (short) (imageIndex + 1);
      this.M2CellInfo[x, y].MiddleIndex = (short) this.selectListItem.Value;
    }

    private void DrawAutoMir2TileSide(int iX, int iY)
    {
      if (this.GetAutoMir2TileType(iX, iY - 2) < Main.TileType.Center)
        this.PutAutoTile(iX, iY - 2, this.RandomAutoMir2Tile(Main.TileType.Up));
      if (this.GetAutoMir2TileType(iX + 2, iY - 2) < Main.TileType.Center)
        this.PutAutoTile(iX + 2, iY - 2, this.RandomAutoMir2Tile(Main.TileType.UpRight));
      if (this.GetAutoMir2TileType(iX + 2, iY) < Main.TileType.Center)
        this.PutAutoTile(iX + 2, iY, this.RandomAutoMir2Tile(Main.TileType.Right));
      if (this.GetAutoMir2TileType(iX + 2, iY + 2) < Main.TileType.Center)
        this.PutAutoTile(iX + 2, iY + 2, this.RandomAutoMir2Tile(Main.TileType.DownRight));
      if (this.GetAutoMir2TileType(iX, iY + 2) < Main.TileType.Center)
        this.PutAutoTile(iX, iY + 2, this.RandomAutoMir2Tile(Main.TileType.Down));
      if (this.GetAutoMir2TileType(iX - 2, iY + 2) < Main.TileType.Center)
        this.PutAutoTile(iX - 2, iY + 2, this.RandomAutoMir2Tile(Main.TileType.DownLeft));
      if (this.GetAutoMir2TileType(iX - 2, iY) < Main.TileType.Center)
        this.PutAutoTile(iX - 2, iY, this.RandomAutoMir2Tile(Main.TileType.Left));
      if (this.GetAutoMir2TileType(iX - 2, iY - 2) >= Main.TileType.Center)
        return;
      this.PutAutoTile(iX - 2, iY - 2, this.RandomAutoMir2Tile(Main.TileType.UpLeft));
    }

    private void DrawAutoMir3TileSide(int iX, int iY)
    {
      if (this.GetAutoMir3TileType(iX, iY - 2) < Main.TileType.Center)
        this.PutAutoTile(iX, iY - 2, this.RandomAutoMir3Tile(Main.TileType.Up));
      if (this.GetAutoMir3TileType(iX + 2, iY - 2) < Main.TileType.Center)
        this.PutAutoTile(iX + 2, iY - 2, this.RandomAutoMir3Tile(Main.TileType.UpRight));
      if (this.GetAutoMir3TileType(iX + 2, iY) < Main.TileType.Center)
        this.PutAutoTile(iX + 2, iY, this.RandomAutoMir3Tile(Main.TileType.Right));
      if (this.GetAutoMir3TileType(iX + 2, iY + 2) < Main.TileType.Center)
        this.PutAutoTile(iX + 2, iY + 2, this.RandomAutoMir3Tile(Main.TileType.DownRight));
      if (this.GetAutoMir3TileType(iX, iY + 2) < Main.TileType.Center)
        this.PutAutoTile(iX, iY + 2, this.RandomAutoMir3Tile(Main.TileType.Down));
      if (this.GetAutoMir3TileType(iX - 2, iY + 2) < Main.TileType.Center)
        this.PutAutoTile(iX - 2, iY + 2, this.RandomAutoMir3Tile(Main.TileType.DownLeft));
      if (this.GetAutoMir3TileType(iX - 2, iY) < Main.TileType.Center)
        this.PutAutoTile(iX - 2, iY, this.RandomAutoMir3Tile(Main.TileType.Left));
      if (this.GetAutoMir3TileType(iX - 2, iY - 2) >= Main.TileType.Center)
        return;
      this.PutAutoTile(iX - 2, iY - 2, this.RandomAutoMir3Tile(Main.TileType.UpLeft));
    }

    private void DrawAutoSmTileSide(int iX, int iY)
    {
      if (this.GetAutoSmTileType(iX, iY - 1) < Main.TileType.Center)
        this.PutAutoSmTile(iX, iY - 1, this.RandomAutoSmTile(Main.TileType.Up));
      if (this.GetAutoSmTileType(iX + 1, iY - 1) < Main.TileType.Center)
        this.PutAutoSmTile(iX + 1, iY - 1, this.RandomAutoSmTile(Main.TileType.UpRight));
      if (this.GetAutoSmTileType(iX + 1, iY) < Main.TileType.Center)
        this.PutAutoSmTile(iX + 1, iY, this.RandomAutoSmTile(Main.TileType.Right));
      if (this.GetAutoSmTileType(iX + 1, iY + 1) < Main.TileType.Center)
        this.PutAutoSmTile(iX + 1, iY + 1, this.RandomAutoSmTile(Main.TileType.DownRight));
      if (this.GetAutoSmTileType(iX, iY + 1) < Main.TileType.Center)
        this.PutAutoSmTile(iX, iY + 1, this.RandomAutoSmTile(Main.TileType.Down));
      if (this.GetAutoSmTileType(iX - 1, iY + 1) < Main.TileType.Center)
        this.PutAutoSmTile(iX - 1, iY + 1, this.RandomAutoSmTile(Main.TileType.DownLeft));
      if (this.GetAutoSmTileType(iX - 1, iY) < Main.TileType.Center)
        this.PutAutoSmTile(iX - 1, iY, this.RandomAutoSmTile(Main.TileType.Left));
      if (this.GetAutoSmTileType(iX - 1, iY - 1) >= Main.TileType.Center)
        return;
      this.PutAutoSmTile(iX - 1, iY - 1, this.RandomAutoSmTile(Main.TileType.UpLeft));
    }

    private void DrawAutoMir2TilePattern(int iX, int iY)
    {
      for (int index1 = iY - Main.AutoTileRange; index1 <= iY + Main.AutoTileRange; index1 += 2)
      {
        for (int index2 = iX - Main.AutoTileRange; index2 <= iX + Main.AutoTileRange; index2 += 2)
        {
          if (index2 > 1 && index1 > 1 && this.GetAutoMir2TileType(index2, index1) > Main.TileType.Center)
          {
            if ((uint) this.GetAutoMir2TileType(index2, index1) > 0U)
            {
              int num = 0;
              if (this.GetAutoMir2TileType(index2, index1 - 2) >= Main.TileType.Center)
                ++num;
              if (this.GetAutoMir2TileType(index2 + 2, index1 - 2) >= Main.TileType.Center)
                ++num;
              if (this.GetAutoMir2TileType(index2 + 2, index1) >= Main.TileType.Center)
                ++num;
              if (this.GetAutoMir2TileType(index2 + 2, index1 + 2) >= Main.TileType.Center)
                ++num;
              if (this.GetAutoMir2TileType(index2, index1 + 2) >= Main.TileType.Center)
                ++num;
              if (this.GetAutoMir2TileType(index2 - 2, index1 + 2) >= Main.TileType.Center)
                ++num;
              if (this.GetAutoMir2TileType(index2 - 2, index1) >= Main.TileType.Center)
                ++num;
              if (this.GetAutoMir2TileType(index2 - 2, index1 - 2) >= Main.TileType.Center)
                ++num;
              if (num >= 8)
                this.PutAutoTile(index2, index1, this.RandomAutoMir2Tile(Main.TileType.Center));
            }
            if (this.GetAutoMir2TileType(index2, index1) != Main.TileType.Up)
            {
              Main.TileType autoMir2TileType1 = this.GetAutoMir2TileType(index2 - 2, index1);
              Main.TileType autoMir2TileType2 = this.GetAutoMir2TileType(index2 + 2, index1);
              if ((autoMir2TileType1 == Main.TileType.Up || autoMir2TileType1 == Main.TileType.UpLeft || autoMir2TileType1 == Main.TileType.InDownLeft) && (autoMir2TileType2 == Main.TileType.Up || autoMir2TileType2 == Main.TileType.UpRight || autoMir2TileType2 == Main.TileType.InDownRight) && this.GetAutoMir2TileType(index2, index1 - 2) < Main.TileType.Center)
                this.PutAutoTile(index2, index1, this.RandomAutoMir2Tile(Main.TileType.Up));
              if (this.GetAutoMir2TileType(index2, index1) == Main.TileType.UpRight && (this.GetAutoMir2TileType(index2, index1 + 2) == Main.TileType.Center && this.GetAutoMir2TileType(index2 + 2, index1 + 2) == Main.TileType.Center || this.GetAutoMir2TileType(index2 + 2, index1) == Main.TileType.UpLeft || this.GetAutoMir2TileType(index2 + 2, index1) == Main.TileType.Left))
                this.PutAutoTile(index2, index1, this.RandomAutoMir2Tile(Main.TileType.Up));
              if (this.GetAutoMir2TileType(index2, index1) == Main.TileType.UpLeft && (this.GetAutoMir2TileType(index2, index1 + 2) == Main.TileType.Center && this.GetAutoMir2TileType(index2 - 2, index1 + 2) == Main.TileType.Center || this.GetAutoMir2TileType(index2 - 2, index1) == Main.TileType.UpRight || this.GetAutoMir2TileType(index2 - 2, index1) == Main.TileType.Right))
                this.PutAutoTile(index2, index1, this.RandomAutoMir2Tile(Main.TileType.Up));
            }
            if (this.GetAutoMir2TileType(index2, index1) != Main.TileType.Right)
            {
              Main.TileType autoMir2TileType3 = this.GetAutoMir2TileType(index2, index1 - 2);
              Main.TileType autoMir2TileType4 = this.GetAutoMir2TileType(index2, index1 + 2);
              if ((autoMir2TileType3 == Main.TileType.Right || autoMir2TileType3 == Main.TileType.UpRight || autoMir2TileType3 == Main.TileType.InUpLeft) && (autoMir2TileType4 == Main.TileType.Right || autoMir2TileType4 == Main.TileType.DownRight || autoMir2TileType4 == Main.TileType.InDownLeft) && this.GetAutoMir2TileType(index2 + 2, index1) < Main.TileType.Center)
                this.PutAutoTile(index2, index1, this.RandomAutoMir2Tile(Main.TileType.Right));
              if (this.GetAutoMir2TileType(index2, index1) == Main.TileType.DownRight && (this.GetAutoMir2TileType(index2 - 2, index1) == Main.TileType.Center && this.GetAutoMir2TileType(index2 - 2, index1 + 2) == Main.TileType.Center || this.GetAutoMir2TileType(index2, index1 + 2) == Main.TileType.UpRight || this.GetAutoMir2TileType(index2, index1 + 2) == Main.TileType.Up))
                this.PutAutoTile(index2, index1, this.RandomAutoMir2Tile(Main.TileType.Right));
              if (this.GetAutoMir2TileType(index2, index1) == Main.TileType.UpRight && (this.GetAutoMir2TileType(index2 - 2, index1) == Main.TileType.Center && this.GetAutoMir2TileType(index2 - 2, index1 - 2) == Main.TileType.Center || this.GetAutoMir2TileType(index2, index1 - 2) == Main.TileType.DownRight || this.GetAutoMir2TileType(index2, index1 - 2) == Main.TileType.Down))
                this.PutAutoTile(index2, index1, this.RandomAutoMir2Tile(Main.TileType.Right));
            }
            if (this.GetAutoMir2TileType(index2, index1) != Main.TileType.Down)
            {
              Main.TileType autoMir2TileType5 = this.GetAutoMir2TileType(index2 - 2, index1);
              Main.TileType autoMir2TileType6 = this.GetAutoMir2TileType(index2 + 2, index1);
              if ((autoMir2TileType5 == Main.TileType.Down || autoMir2TileType5 == Main.TileType.DownLeft || autoMir2TileType5 == Main.TileType.InUpLeft) && (autoMir2TileType6 == Main.TileType.Down || autoMir2TileType6 == Main.TileType.DownRight || autoMir2TileType6 == Main.TileType.InUpRight) && this.GetAutoMir2TileType(index2, index1 + 2) < Main.TileType.Center)
                this.PutAutoTile(index2, index1, this.RandomAutoMir2Tile(Main.TileType.Down));
              if (this.GetAutoMir2TileType(index2, index1) == Main.TileType.DownRight && (this.GetAutoMir2TileType(index2, index1 - 2) == Main.TileType.Center && this.GetAutoMir2TileType(index2 + 2, index1 - 2) == Main.TileType.Center || this.GetAutoMir2TileType(index2 + 2, index1) == Main.TileType.DownLeft || this.GetAutoMir2TileType(index2 + 2, index1) == Main.TileType.Left))
                this.PutAutoTile(index2, index1, this.RandomAutoMir2Tile(Main.TileType.Down));
              if (this.GetAutoMir2TileType(index2, index1) == Main.TileType.DownLeft && (this.GetAutoMir2TileType(index2 - 2, index1 - 2) == Main.TileType.Center && this.GetAutoMir2TileType(index2, index1 - 2) == Main.TileType.Center || this.GetAutoMir2TileType(index2 - 2, index1) == Main.TileType.DownRight || this.GetAutoMir2TileType(index2 - 2, index1) == Main.TileType.Right))
                this.PutAutoTile(index2, index1, this.RandomAutoMir2Tile(Main.TileType.Down));
            }
            if (this.GetAutoMir2TileType(index2, index1) != Main.TileType.Left)
            {
              Main.TileType autoMir2TileType7 = this.GetAutoMir2TileType(index2, index1 - 2);
              Main.TileType autoMir2TileType8 = this.GetAutoMir2TileType(index2, index1 + 2);
              if ((autoMir2TileType7 == Main.TileType.Left || autoMir2TileType7 == Main.TileType.UpLeft || autoMir2TileType7 == Main.TileType.InUpRight) && (autoMir2TileType8 == Main.TileType.Left || autoMir2TileType8 == Main.TileType.DownLeft || autoMir2TileType8 == Main.TileType.InDownRight) && this.GetAutoMir2TileType(index2 - 2, index1) < Main.TileType.Center)
                this.PutAutoTile(index2, index1, this.RandomAutoMir2Tile(Main.TileType.Left));
              if (this.GetAutoMir2TileType(index2, index1) == Main.TileType.DownLeft && (this.GetAutoMir2TileType(index2 + 2, index1) == Main.TileType.Center && this.GetAutoMir2TileType(index2 + 2, index1 + 2) == Main.TileType.Center || this.GetAutoMir2TileType(index2, index1 + 2) == Main.TileType.UpLeft || this.GetAutoMir2TileType(index2, index1 + 2) == Main.TileType.Up))
                this.PutAutoTile(index2, index1, this.RandomAutoMir2Tile(Main.TileType.Left));
              if (this.GetAutoMir2TileType(index2, index1) == Main.TileType.UpLeft && (this.GetAutoMir2TileType(index2 + 2, index1) == Main.TileType.Center && this.GetAutoMir2TileType(index2 + 2, index1 + 2) == Main.TileType.Center || this.GetAutoMir2TileType(index2, index1 - 2) == Main.TileType.DownLeft || this.GetAutoMir2TileType(index2, index1 - 2) == Main.TileType.Down))
                this.PutAutoTile(index2, index1, this.RandomAutoMir2Tile(Main.TileType.Left));
            }
            Main.TileType autoMir2TileType9 = this.GetAutoMir2TileType(index2 - 2, index1);
            Main.TileType autoMir2TileType10 = this.GetAutoMir2TileType(index2, index1 + 2);
            if (this.GetAutoMir2TileType(index2, index1) != Main.TileType.InDownLeft && (autoMir2TileType9 == Main.TileType.DownLeft || autoMir2TileType9 == Main.TileType.Down || autoMir2TileType9 == Main.TileType.InUpLeft) && (autoMir2TileType10 == Main.TileType.DownLeft || autoMir2TileType10 == Main.TileType.Left || autoMir2TileType10 == Main.TileType.InDownRight))
              this.PutAutoTile(index2, index1, this.RandomAutoMir2Tile(Main.TileType.InDownLeft));
            Main.TileType autoMir2TileType11 = this.GetAutoMir2TileType(index2, index1 - 2);
            Main.TileType autoMir2TileType12 = this.GetAutoMir2TileType(index2 - 2, index1);
            if (this.GetAutoMir2TileType(index2, index1) != Main.TileType.InUpLeft && (autoMir2TileType11 == Main.TileType.UpLeft || autoMir2TileType11 == Main.TileType.Left || autoMir2TileType11 == Main.TileType.InUpRight) && (autoMir2TileType12 == Main.TileType.UpLeft || autoMir2TileType12 == Main.TileType.Up || autoMir2TileType12 == Main.TileType.InDownLeft))
              this.PutAutoTile(index2, index1, this.RandomAutoMir2Tile(Main.TileType.InUpLeft));
            Main.TileType autoMir2TileType13 = this.GetAutoMir2TileType(index2, index1 - 2);
            Main.TileType autoMir2TileType14 = this.GetAutoMir2TileType(index2 + 2, index1);
            if (this.GetAutoMir2TileType(index2, index1) != Main.TileType.InUpRight && (autoMir2TileType13 == Main.TileType.UpRight || autoMir2TileType13 == Main.TileType.Right || autoMir2TileType13 == Main.TileType.InUpLeft) && (autoMir2TileType14 == Main.TileType.UpRight || autoMir2TileType14 == Main.TileType.Up || autoMir2TileType14 == Main.TileType.InDownRight))
              this.PutAutoTile(index2, index1, this.RandomAutoMir2Tile(Main.TileType.InUpRight));
            Main.TileType autoMir2TileType15 = this.GetAutoMir2TileType(index2 + 2, index1);
            Main.TileType autoMir2TileType16 = this.GetAutoMir2TileType(index2, index1 + 2);
            if (this.GetAutoMir2TileType(index2, index1) != Main.TileType.InDownRight && (autoMir2TileType15 == Main.TileType.DownRight || autoMir2TileType15 == Main.TileType.Down || autoMir2TileType15 == Main.TileType.InUpRight) && (autoMir2TileType16 == Main.TileType.DownRight || autoMir2TileType16 == Main.TileType.Right || autoMir2TileType16 == Main.TileType.InDownLeft))
              this.PutAutoTile(index2, index1, this.RandomAutoMir2Tile(Main.TileType.InDownRight));
            if (this.GetAutoMir2TileType(index2 - 2, index1) == Main.TileType.Down && this.GetAutoMir2TileType(index2, index1 - 2) == Main.TileType.Right && this.GetAutoMir2TileType(index2 + 2, index1) == Main.TileType.Up && this.GetAutoMir2TileType(index2, index1 + 2) == Main.TileType.Left || this.GetAutoMir2TileType(index2 - 2, index1) == Main.TileType.Up && this.GetAutoMir2TileType(index2, index1 + 2) == Main.TileType.Right && this.GetAutoMir2TileType(index2, index1 - 2) == Main.TileType.Left && this.GetAutoMir2TileType(index2 + 2, index1) == Main.TileType.Down)
            {
              this.PutAutoTile(index2, index1, this.RandomAutoMir2Tile(Main.TileType.Center));
              this.DrawAutoMir2TileSide(index2, index1);
            }
          }
        }
      }
    }

    private void DrawAutoMir3TilePattern(int iX, int iY)
    {
      for (int index1 = iY - Main.AutoTileRange; index1 <= iY + Main.AutoTileRange; index1 += 2)
      {
        for (int index2 = iX - Main.AutoTileRange; index2 <= iX + Main.AutoTileRange; index2 += 2)
        {
          if (index2 > 1 && index1 > 1 && this.GetAutoMir3TileType(index2, index1) > Main.TileType.Center)
          {
            if ((uint) this.GetAutoMir3TileType(index2, index1) > 0U)
            {
              int num = 0;
              if (this.GetAutoMir3TileType(index2, index1 - 2) >= Main.TileType.Center)
                ++num;
              if (this.GetAutoMir3TileType(index2 + 2, index1 - 2) >= Main.TileType.Center)
                ++num;
              if (this.GetAutoMir3TileType(index2 + 2, index1) >= Main.TileType.Center)
                ++num;
              if (this.GetAutoMir3TileType(index2 + 2, index1 + 2) >= Main.TileType.Center)
                ++num;
              if (this.GetAutoMir3TileType(index2, index1 + 2) >= Main.TileType.Center)
                ++num;
              if (this.GetAutoMir3TileType(index2 - 2, index1 + 2) >= Main.TileType.Center)
                ++num;
              if (this.GetAutoMir3TileType(index2 - 2, index1) >= Main.TileType.Center)
                ++num;
              if (this.GetAutoMir3TileType(index2 - 2, index1 - 2) >= Main.TileType.Center)
                ++num;
              if (num >= 8)
                this.PutAutoTile(index2, index1, this.RandomAutoMir3Tile(Main.TileType.Center));
            }
            if (this.GetAutoMir3TileType(index2, index1) != Main.TileType.Up)
            {
              Main.TileType autoMir3TileType1 = this.GetAutoMir3TileType(index2 - 2, index1);
              Main.TileType autoMir3TileType2 = this.GetAutoMir3TileType(index2 + 2, index1);
              if ((autoMir3TileType1 == Main.TileType.Up || autoMir3TileType1 == Main.TileType.UpLeft || autoMir3TileType1 == Main.TileType.InDownLeft) && (autoMir3TileType2 == Main.TileType.Up || autoMir3TileType2 == Main.TileType.UpRight || autoMir3TileType2 == Main.TileType.InDownRight) && this.GetAutoMir3TileType(index2, index1 - 2) < Main.TileType.Center)
                this.PutAutoTile(index2, index1, this.RandomAutoMir3Tile(Main.TileType.Up));
              if (this.GetAutoMir3TileType(index2, index1) == Main.TileType.UpRight && (this.GetAutoMir3TileType(index2, index1 + 2) == Main.TileType.Center && this.GetAutoMir3TileType(index2 + 2, index1 + 2) == Main.TileType.Center || this.GetAutoMir3TileType(index2 + 2, index1) == Main.TileType.UpLeft || this.GetAutoMir3TileType(index2 + 2, index1) == Main.TileType.Left))
                this.PutAutoTile(index2, index1, this.RandomAutoMir3Tile(Main.TileType.Up));
              if (this.GetAutoMir3TileType(index2, index1) == Main.TileType.UpLeft && (this.GetAutoMir3TileType(index2, index1 + 2) == Main.TileType.Center && this.GetAutoMir3TileType(index2 - 2, index1 + 2) == Main.TileType.Center || this.GetAutoMir3TileType(index2 - 2, index1) == Main.TileType.UpRight || this.GetAutoMir3TileType(index2 - 2, index1) == Main.TileType.Right))
                this.PutAutoTile(index2, index1, this.RandomAutoMir3Tile(Main.TileType.Up));
            }
            if (this.GetAutoMir3TileType(index2, index1) != Main.TileType.Right)
            {
              Main.TileType autoMir3TileType3 = this.GetAutoMir3TileType(index2, index1 - 2);
              Main.TileType autoMir3TileType4 = this.GetAutoMir3TileType(index2, index1 + 2);
              if ((autoMir3TileType3 == Main.TileType.Right || autoMir3TileType3 == Main.TileType.UpRight || autoMir3TileType3 == Main.TileType.InUpLeft) && (autoMir3TileType4 == Main.TileType.Right || autoMir3TileType4 == Main.TileType.DownRight || autoMir3TileType4 == Main.TileType.InDownLeft) && this.GetAutoMir3TileType(index2 + 2, index1) < Main.TileType.Center)
                this.PutAutoTile(index2, index1, this.RandomAutoMir3Tile(Main.TileType.Right));
              if (this.GetAutoMir3TileType(index2, index1) == Main.TileType.DownRight && (this.GetAutoMir3TileType(index2 - 2, index1) == Main.TileType.Center && this.GetAutoMir3TileType(index2 - 2, index1 + 2) == Main.TileType.Center || this.GetAutoMir3TileType(index2, index1 + 2) == Main.TileType.UpRight || this.GetAutoMir3TileType(index2, index1 + 2) == Main.TileType.Up))
                this.PutAutoTile(index2, index1, this.RandomAutoMir3Tile(Main.TileType.Right));
              if (this.GetAutoMir3TileType(index2, index1) == Main.TileType.UpRight && (this.GetAutoMir3TileType(index2 - 2, index1) == Main.TileType.Center && this.GetAutoMir3TileType(index2 - 2, index1 - 2) == Main.TileType.Center || this.GetAutoMir3TileType(index2, index1 - 2) == Main.TileType.DownRight || this.GetAutoMir3TileType(index2, index1 - 2) == Main.TileType.Down))
                this.PutAutoTile(index2, index1, this.RandomAutoMir3Tile(Main.TileType.Right));
            }
            if (this.GetAutoMir3TileType(index2, index1) != Main.TileType.Down)
            {
              Main.TileType autoMir3TileType5 = this.GetAutoMir3TileType(index2 - 2, index1);
              Main.TileType autoMir3TileType6 = this.GetAutoMir3TileType(index2 + 2, index1);
              if ((autoMir3TileType5 == Main.TileType.Down || autoMir3TileType5 == Main.TileType.DownLeft || autoMir3TileType5 == Main.TileType.InUpLeft) && (autoMir3TileType6 == Main.TileType.Down || autoMir3TileType6 == Main.TileType.DownRight || autoMir3TileType6 == Main.TileType.InUpRight) && this.GetAutoMir3TileType(index2, index1 + 2) < Main.TileType.Center)
                this.PutAutoTile(index2, index1, this.RandomAutoMir3Tile(Main.TileType.Down));
              if (this.GetAutoMir3TileType(index2, index1) == Main.TileType.DownRight && (this.GetAutoMir3TileType(index2, index1 - 2) == Main.TileType.Center && this.GetAutoMir3TileType(index2 + 2, index1 - 2) == Main.TileType.Center || this.GetAutoMir3TileType(index2 + 2, index1) == Main.TileType.DownLeft || this.GetAutoMir3TileType(index2 + 2, index1) == Main.TileType.Left))
                this.PutAutoTile(index2, index1, this.RandomAutoMir3Tile(Main.TileType.Down));
              if (this.GetAutoMir3TileType(index2, index1) == Main.TileType.DownLeft && (this.GetAutoMir3TileType(index2 - 2, index1 - 2) == Main.TileType.Center && this.GetAutoMir3TileType(index2, index1 - 2) == Main.TileType.Center || this.GetAutoMir3TileType(index2 - 2, index1) == Main.TileType.DownRight || this.GetAutoMir3TileType(index2 - 2, index1) == Main.TileType.Right))
                this.PutAutoTile(index2, index1, this.RandomAutoMir3Tile(Main.TileType.Down));
            }
            if (this.GetAutoMir3TileType(index2, index1) != Main.TileType.Left)
            {
              Main.TileType autoMir3TileType7 = this.GetAutoMir3TileType(index2, index1 - 2);
              Main.TileType autoMir3TileType8 = this.GetAutoMir3TileType(index2, index1 + 2);
              if ((autoMir3TileType7 == Main.TileType.Left || autoMir3TileType7 == Main.TileType.UpLeft || autoMir3TileType7 == Main.TileType.InUpRight) && (autoMir3TileType8 == Main.TileType.Left || autoMir3TileType8 == Main.TileType.DownLeft || autoMir3TileType8 == Main.TileType.InDownRight) && this.GetAutoMir3TileType(index2 - 2, index1) < Main.TileType.Center)
                this.PutAutoTile(index2, index1, this.RandomAutoMir3Tile(Main.TileType.Left));
              if (this.GetAutoMir3TileType(index2, index1) == Main.TileType.DownLeft && (this.GetAutoMir3TileType(index2 + 2, index1) == Main.TileType.Center && this.GetAutoMir3TileType(index2 + 2, index1 + 2) == Main.TileType.Center || this.GetAutoMir3TileType(index2, index1 + 2) == Main.TileType.UpLeft || this.GetAutoMir3TileType(index2, index1 + 2) == Main.TileType.Up))
                this.PutAutoTile(index2, index1, this.RandomAutoMir3Tile(Main.TileType.Left));
              if (this.GetAutoMir3TileType(index2, index1) == Main.TileType.UpLeft && (this.GetAutoMir3TileType(index2 + 2, index1) == Main.TileType.Center && this.GetAutoMir3TileType(index2 + 2, index1 + 2) == Main.TileType.Center || this.GetAutoMir3TileType(index2, index1 - 2) == Main.TileType.DownLeft || this.GetAutoMir3TileType(index2, index1 - 2) == Main.TileType.Down))
                this.PutAutoTile(index2, index1, this.RandomAutoMir3Tile(Main.TileType.Left));
            }
            Main.TileType autoMir3TileType9 = this.GetAutoMir3TileType(index2 - 2, index1);
            Main.TileType autoMir3TileType10 = this.GetAutoMir3TileType(index2, index1 + 2);
            if (this.GetAutoMir3TileType(index2, index1) != Main.TileType.InDownLeft && (autoMir3TileType9 == Main.TileType.DownLeft || autoMir3TileType9 == Main.TileType.Down || autoMir3TileType9 == Main.TileType.InUpLeft) && (autoMir3TileType10 == Main.TileType.DownLeft || autoMir3TileType10 == Main.TileType.Left || autoMir3TileType10 == Main.TileType.InDownRight))
              this.PutAutoTile(index2, index1, this.RandomAutoMir3Tile(Main.TileType.InDownLeft));
            Main.TileType autoMir3TileType11 = this.GetAutoMir3TileType(index2, index1 - 2);
            Main.TileType autoMir3TileType12 = this.GetAutoMir3TileType(index2 - 2, index1);
            if (this.GetAutoMir3TileType(index2, index1) != Main.TileType.InUpLeft && (autoMir3TileType11 == Main.TileType.UpLeft || autoMir3TileType11 == Main.TileType.Left || autoMir3TileType11 == Main.TileType.InUpRight) && (autoMir3TileType12 == Main.TileType.UpLeft || autoMir3TileType12 == Main.TileType.Up || autoMir3TileType12 == Main.TileType.InDownLeft))
              this.PutAutoTile(index2, index1, this.RandomAutoMir3Tile(Main.TileType.InUpLeft));
            Main.TileType autoMir3TileType13 = this.GetAutoMir3TileType(index2, index1 - 2);
            Main.TileType autoMir3TileType14 = this.GetAutoMir3TileType(index2 + 2, index1);
            if (this.GetAutoMir3TileType(index2, index1) != Main.TileType.InUpRight && (autoMir3TileType13 == Main.TileType.UpRight || autoMir3TileType13 == Main.TileType.Right || autoMir3TileType13 == Main.TileType.InUpLeft) && (autoMir3TileType14 == Main.TileType.UpRight || autoMir3TileType14 == Main.TileType.Up || autoMir3TileType14 == Main.TileType.InDownRight))
              this.PutAutoTile(index2, index1, this.RandomAutoMir3Tile(Main.TileType.InUpRight));
            Main.TileType autoMir3TileType15 = this.GetAutoMir3TileType(index2 + 2, index1);
            Main.TileType autoMir3TileType16 = this.GetAutoMir3TileType(index2, index1 + 2);
            if (this.GetAutoMir3TileType(index2, index1) != Main.TileType.InDownRight && (autoMir3TileType15 == Main.TileType.DownRight || autoMir3TileType15 == Main.TileType.Down || autoMir3TileType15 == Main.TileType.InUpRight) && (autoMir3TileType16 == Main.TileType.DownRight || autoMir3TileType16 == Main.TileType.Right || autoMir3TileType16 == Main.TileType.InDownLeft))
              this.PutAutoTile(index2, index1, this.RandomAutoMir3Tile(Main.TileType.InDownRight));
            if (this.GetAutoMir3TileType(index2 - 2, index1) == Main.TileType.Down && this.GetAutoMir3TileType(index2, index1 - 2) == Main.TileType.Right && this.GetAutoMir3TileType(index2 + 2, index1) == Main.TileType.Up && this.GetAutoMir3TileType(index2, index1 + 2) == Main.TileType.Left || this.GetAutoMir3TileType(index2 - 2, index1) == Main.TileType.Up && this.GetAutoMir3TileType(index2, index1 + 2) == Main.TileType.Right && this.GetAutoMir3TileType(index2, index1 - 2) == Main.TileType.Left && this.GetAutoMir3TileType(index2 + 2, index1) == Main.TileType.Down)
            {
              this.PutAutoTile(index2, index1, this.RandomAutoMir3Tile(Main.TileType.Center));
              this.DrawAutoMir3TileSide(index2, index1);
            }
          }
        }
      }
    }

    private void DrawAutoSmTilePattern(int iX, int iY)
    {
      for (int index1 = iY - Main.AutoTileRange; index1 <= iY + Main.AutoTileRange; ++index1)
      {
        for (int index2 = iX - Main.AutoTileRange; index2 <= iX + Main.AutoTileRange; ++index2)
        {
          if (index2 > 0 && index1 > 0 && this.GetAutoSmTileType(index2, index1) > Main.TileType.Center)
          {
            if ((uint) this.GetAutoSmTileType(index2, index1) > 0U)
            {
              int num = 0;
              if (this.GetAutoSmTileType(index2, index1 - 1) >= Main.TileType.Center)
                ++num;
              if (this.GetAutoSmTileType(index2 + 1, index1 - 1) >= Main.TileType.Center)
                ++num;
              if (this.GetAutoSmTileType(index2 + 1, index1) >= Main.TileType.Center)
                ++num;
              if (this.GetAutoSmTileType(index2 + 1, index1 + 1) >= Main.TileType.Center)
                ++num;
              if (this.GetAutoSmTileType(index2, index1 + 1) >= Main.TileType.Center)
                ++num;
              if (this.GetAutoSmTileType(index2 - 1, index1 + 1) >= Main.TileType.Center)
                ++num;
              if (this.GetAutoSmTileType(index2 - 1, index1) >= Main.TileType.Center)
                ++num;
              if (this.GetAutoSmTileType(index2 - 1, index1 - 1) >= Main.TileType.Center)
                ++num;
              if (num >= 8)
                this.PutAutoSmTile(index2, index1, this.RandomAutoSmTile(Main.TileType.Center));
            }
            if (this.GetAutoSmTileType(index2, index1) != Main.TileType.Up)
            {
              Main.TileType autoSmTileType1 = this.GetAutoSmTileType(index2 - 1, index1);
              Main.TileType autoSmTileType2 = this.GetAutoSmTileType(index2 + 1, index1);
              if ((autoSmTileType1 == Main.TileType.Up || autoSmTileType1 == Main.TileType.UpLeft || autoSmTileType1 == Main.TileType.InDownLeft) && (autoSmTileType2 == Main.TileType.Up || autoSmTileType2 == Main.TileType.UpRight || autoSmTileType2 == Main.TileType.InDownRight) && this.GetAutoSmTileType(index2, index1 - 1) < Main.TileType.Center)
                this.PutAutoSmTile(index2, index1, this.RandomAutoSmTile(Main.TileType.Up));
              if (this.GetAutoSmTileType(index2, index1) == Main.TileType.UpRight && (this.GetAutoSmTileType(index2, index1 + 1) == Main.TileType.Center && this.GetAutoSmTileType(index2 + 1, index1 + 1) == Main.TileType.Center || this.GetAutoSmTileType(index2 + 1, index1) == Main.TileType.UpLeft || this.GetAutoSmTileType(index2 + 1, index1) == Main.TileType.Left))
                this.PutAutoSmTile(index2, index1, this.RandomAutoSmTile(Main.TileType.Up));
              if (this.GetAutoSmTileType(index2, index1) == Main.TileType.UpLeft && (this.GetAutoSmTileType(index2, index1 + 1) == Main.TileType.Center && this.GetAutoSmTileType(index2 - 1, index1 + 1) == Main.TileType.Center || this.GetAutoSmTileType(index2 - 1, index1) == Main.TileType.UpRight || this.GetAutoSmTileType(index2 - 1, index1) == Main.TileType.Right))
                this.PutAutoSmTile(index2, index1, this.RandomAutoSmTile(Main.TileType.Up));
            }
            if (this.GetAutoSmTileType(index2, index1) != Main.TileType.Right)
            {
              Main.TileType autoSmTileType3 = this.GetAutoSmTileType(index2, index1 - 1);
              Main.TileType autoSmTileType4 = this.GetAutoSmTileType(index2, index1 + 1);
              if ((autoSmTileType3 == Main.TileType.Right || autoSmTileType3 == Main.TileType.UpRight || autoSmTileType3 == Main.TileType.InUpLeft) && (autoSmTileType4 == Main.TileType.Right || autoSmTileType4 == Main.TileType.DownRight || autoSmTileType4 == Main.TileType.InDownLeft) && this.GetAutoSmTileType(index2 + 1, index1) < Main.TileType.Center)
                this.PutAutoSmTile(index2, index1, this.RandomAutoSmTile(Main.TileType.Right));
              if (this.GetAutoSmTileType(index2, index1) == Main.TileType.DownRight && (this.GetAutoSmTileType(index2 - 1, index1) == Main.TileType.Center && this.GetAutoSmTileType(index2 - 1, index1 + 1) == Main.TileType.Center || this.GetAutoSmTileType(index2, index1 + 1) == Main.TileType.UpRight || this.GetAutoSmTileType(index2, index1 + 1) == Main.TileType.Up))
                this.PutAutoSmTile(index2, index1, this.RandomAutoSmTile(Main.TileType.Right));
              if (this.GetAutoSmTileType(index2, index1) == Main.TileType.UpRight && (this.GetAutoSmTileType(index2 - 1, index1) == Main.TileType.Center && this.GetAutoSmTileType(index2 - 1, index1 - 1) == Main.TileType.Center || this.GetAutoSmTileType(index2, index1 - 1) == Main.TileType.DownRight || this.GetAutoSmTileType(index2, index1 - 1) == Main.TileType.Down))
                this.PutAutoSmTile(index2, index1, this.RandomAutoSmTile(Main.TileType.Right));
            }
            if (this.GetAutoSmTileType(index2, index1) != Main.TileType.Down)
            {
              Main.TileType autoSmTileType5 = this.GetAutoSmTileType(index2 - 1, index1);
              Main.TileType autoSmTileType6 = this.GetAutoSmTileType(index2 + 1, index1);
              if ((autoSmTileType5 == Main.TileType.Down || autoSmTileType5 == Main.TileType.DownLeft || autoSmTileType5 == Main.TileType.InUpLeft) && (autoSmTileType6 == Main.TileType.Down || autoSmTileType6 == Main.TileType.DownRight || autoSmTileType6 == Main.TileType.InUpRight) && this.GetAutoSmTileType(index2, index1 + 1) < Main.TileType.Center)
                this.PutAutoSmTile(index2, index1, this.RandomAutoSmTile(Main.TileType.Down));
              if (this.GetAutoSmTileType(index2, index1) == Main.TileType.DownRight && (this.GetAutoSmTileType(index2, index1 - 1) == Main.TileType.Center && this.GetAutoSmTileType(index2 + 1, index1 - 1) == Main.TileType.Center || this.GetAutoSmTileType(index2 + 1, index1) == Main.TileType.DownLeft || this.GetAutoSmTileType(index2 + 1, index1) == Main.TileType.Left))
                this.PutAutoSmTile(index2, index1, this.RandomAutoSmTile(Main.TileType.Down));
              if (this.GetAutoSmTileType(index2, index1) == Main.TileType.DownLeft && (this.GetAutoSmTileType(index2 - 1, index1 - 1) == Main.TileType.Center && this.GetAutoSmTileType(index2, index1 - 1) == Main.TileType.Center || this.GetAutoSmTileType(index2 - 1, index1) == Main.TileType.DownRight || this.GetAutoSmTileType(index2 - 1, index1) == Main.TileType.Right))
                this.PutAutoSmTile(index2, index1, this.RandomAutoSmTile(Main.TileType.Down));
            }
            if (this.GetAutoSmTileType(index2, index1) != Main.TileType.Left)
            {
              Main.TileType autoSmTileType7 = this.GetAutoSmTileType(index2, index1 - 1);
              Main.TileType autoSmTileType8 = this.GetAutoSmTileType(index2, index1 + 1);
              if ((autoSmTileType7 == Main.TileType.Left || autoSmTileType7 == Main.TileType.UpLeft || autoSmTileType7 == Main.TileType.InUpRight) && (autoSmTileType8 == Main.TileType.Left || autoSmTileType8 == Main.TileType.DownLeft || autoSmTileType8 == Main.TileType.InDownRight) && this.GetAutoSmTileType(index2 - 1, index1) < Main.TileType.Center)
                this.PutAutoSmTile(index2, index1, this.RandomAutoSmTile(Main.TileType.Left));
              if (this.GetAutoSmTileType(index2, index1) == Main.TileType.DownLeft && (this.GetAutoSmTileType(index2 + 1, index1) == Main.TileType.Center && this.GetAutoSmTileType(index2 + 1, index1 + 1) == Main.TileType.Center || this.GetAutoSmTileType(index2, index1 + 1) == Main.TileType.UpLeft || this.GetAutoSmTileType(index2, index1 + 1) == Main.TileType.Up))
                this.PutAutoSmTile(index2, index1, this.RandomAutoSmTile(Main.TileType.Left));
              if (this.GetAutoSmTileType(index2, index1) == Main.TileType.UpLeft && (this.GetAutoSmTileType(index2 + 1, index1) == Main.TileType.Center && this.GetAutoSmTileType(index2 + 1, index1 + 1) == Main.TileType.Center || this.GetAutoSmTileType(index2, index1 - 1) == Main.TileType.DownLeft || this.GetAutoSmTileType(index2, index1 - 1) == Main.TileType.Down))
                this.PutAutoSmTile(index2, index1, this.RandomAutoSmTile(Main.TileType.Left));
            }
            if (this.GetAutoSmTileType(index2, index1) != Main.TileType.InUpRight)
            {
              Main.TileType autoSmTileType9 = this.GetAutoSmTileType(index2 - 1, index1);
              Main.TileType autoSmTileType10 = this.GetAutoSmTileType(index2, index1 + 1);
              if ((autoSmTileType9 == Main.TileType.DownLeft || autoSmTileType9 == Main.TileType.Down || autoSmTileType9 == Main.TileType.InUpLeft) && (autoSmTileType10 == Main.TileType.DownLeft || autoSmTileType10 == Main.TileType.Left || autoSmTileType10 == Main.TileType.InDownRight))
                this.PutAutoSmTile(index2, index1, this.RandomAutoSmTile(Main.TileType.InUpRight));
            }
            if (this.GetAutoSmTileType(index2, index1) != Main.TileType.InDownRight)
            {
              Main.TileType autoSmTileType11 = this.GetAutoSmTileType(index2, index1 - 1);
              Main.TileType autoSmTileType12 = this.GetAutoSmTileType(index2 - 1, index1);
              if ((autoSmTileType11 == Main.TileType.UpLeft || autoSmTileType11 == Main.TileType.Left || autoSmTileType11 == Main.TileType.InUpRight) && (autoSmTileType12 == Main.TileType.UpLeft || autoSmTileType12 == Main.TileType.Up || autoSmTileType12 == Main.TileType.InDownLeft))
                this.PutAutoSmTile(index2, index1, this.RandomAutoSmTile(Main.TileType.InDownRight));
            }
            if (this.GetAutoSmTileType(index2, index1) != Main.TileType.InDownLeft)
            {
              Main.TileType autoSmTileType13 = this.GetAutoSmTileType(index2, index1 - 1);
              Main.TileType autoSmTileType14 = this.GetAutoSmTileType(index2 + 1, index1);
              if ((autoSmTileType13 == Main.TileType.UpRight || autoSmTileType13 == Main.TileType.Right || autoSmTileType13 == Main.TileType.InUpLeft) && (autoSmTileType14 == Main.TileType.UpRight || autoSmTileType14 == Main.TileType.Up || autoSmTileType14 == Main.TileType.InDownRight))
                this.PutAutoSmTile(index2, index1, this.RandomAutoSmTile(Main.TileType.InDownLeft));
            }
            if (this.GetAutoSmTileType(index2, index1) != Main.TileType.InUpLeft)
            {
              Main.TileType autoSmTileType15 = this.GetAutoSmTileType(index2 + 1, index1);
              Main.TileType autoSmTileType16 = this.GetAutoSmTileType(index2, index1 + 1);
              if ((autoSmTileType15 == Main.TileType.DownRight || autoSmTileType15 == Main.TileType.Down || autoSmTileType15 == Main.TileType.InUpRight) && (autoSmTileType16 == Main.TileType.DownRight || autoSmTileType16 == Main.TileType.Right || autoSmTileType16 == Main.TileType.InDownLeft))
                this.PutAutoSmTile(index2, index1, this.RandomAutoSmTile(Main.TileType.InUpLeft));
            }
            if (this.GetAutoSmTileType(index2 - 1, index1) == Main.TileType.Down && this.GetAutoSmTileType(index2, index1 - 1) == Main.TileType.Right && this.GetAutoSmTileType(index2 + 1, index1) == Main.TileType.Up && this.GetAutoSmTileType(index2, index1 + 1) == Main.TileType.Left || this.GetAutoSmTileType(index2 - 1, index1) == Main.TileType.Up && this.GetAutoSmTileType(index2, index1 + 1) == Main.TileType.Right && this.GetAutoSmTileType(index2, index1 - 1) == Main.TileType.Left && this.GetAutoSmTileType(index2 + 1, index1) == Main.TileType.Down)
            {
              this.PutAutoSmTile(index2, index1, this.RandomAutoSmTile(Main.TileType.Center));
              this.DrawAutoSmTileSide(index2, index1);
            }
          }
        }
      }
    }

    private Bitmap GetTilesPreview(ListItem selectListView, int index)
    {
      Bitmap tilesPreview = new Bitmap(288, 192);
      Graphics.FromImage((Image) tilesPreview).InterpolationMode = InterpolationMode.Low;
      switch (this.selectListItem.Version)
      {
        case 1:
        case 2:
          if (selectListView.Text.IndexOf("SmTiles", StringComparison.Ordinal) > -1 || selectListView.Text.IndexOf("Smtiles", StringComparison.Ordinal) > -1)
          {
            int index1 = 0;
            tilesPreview = new Bitmap(144, 96);
            Graphics graphics = Graphics.FromImage((Image) tilesPreview);
            graphics.InterpolationMode = InterpolationMode.Low;
            for (int index2 = 0; index2 < 3; ++index2)
            {
              this.drawY = index2 * 32;
              for (int index3 = 0; index3 < 3; ++index3)
              {
                this.drawX = index3 * 48;
                if (index * 60 + this.smTilesPreviewIndex[index1] < Libraries.MapLibs[selectListView.Value].Images.Count)
                {
                  Libraries.MapLibs[selectListView.Value].CheckImage(index * 60 + this.smTilesPreviewIndex[index1]);
                  MLibrary.MImage image = Libraries.MapLibs[selectListView.Value].Images[index * 60 + this.smTilesPreviewIndex[index1]];
                  if (image.Image != null)
                  {
                    Rectangle destRect = new Rectangle(this.drawX, this.drawY, (int) image.Width, (int) image.Height);
                    Rectangle srcRect = new Rectangle(0, 0, (int) image.Width, (int) image.Height);
                    graphics.DrawImage((Image) image.Image, destRect, srcRect, GraphicsUnit.Pixel);
                    Libraries.MapLibs[this.libIndex].Images[index] = (MLibrary.MImage) null;
                    ++index1;
                  }
                }
              }
            }
            break;
          }
          if (selectListView.Text.IndexOf("Tiles", StringComparison.Ordinal) > -1)
          {
            int index4 = 0;
            tilesPreview = new Bitmap(288, 192);
            Graphics graphics = Graphics.FromImage((Image) tilesPreview);
            graphics.InterpolationMode = InterpolationMode.Low;
            for (int index5 = 0; index5 < 3; ++index5)
            {
              this.drawY = index5 * 2 * 32;
              for (int index6 = 0; index6 < 3; ++index6)
              {
                this.drawX = index6 * 2 * 48;
                if (index * 50 + this.Mir2BigTilesPreviewIndex[index4] < Libraries.MapLibs[selectListView.Value].Images.Count)
                {
                  Libraries.MapLibs[selectListView.Value].CheckImage(index * 50 + this.Mir2BigTilesPreviewIndex[index4]);
                  MLibrary.MImage image = Libraries.MapLibs[selectListView.Value].Images[index * 50 + this.Mir2BigTilesPreviewIndex[index4]];
                  if (image.Image != null)
                  {
                    Rectangle destRect = new Rectangle(this.drawX, this.drawY, (int) image.Width, (int) image.Height);
                    Rectangle srcRect = new Rectangle(0, 0, (int) image.Width, (int) image.Height);
                    graphics.DrawImage((Image) image.Image, destRect, srcRect, GraphicsUnit.Pixel);
                    Libraries.MapLibs[this.libIndex].Images[index] = (MLibrary.MImage) null;
                    ++index4;
                  }
                }
              }
            }
            break;
          }
          break;
        case 3:
        case 4:
          if (selectListView.Text.IndexOf("SmTiles", StringComparison.Ordinal) > -1 || selectListView.Text.IndexOf("Smtiles", StringComparison.Ordinal) > -1)
          {
            int index7 = 0;
            tilesPreview = new Bitmap(144, 96);
            Graphics graphics = Graphics.FromImage((Image) tilesPreview);
            graphics.InterpolationMode = InterpolationMode.Low;
            for (int index8 = 0; index8 < 3; ++index8)
            {
              this.drawY = index8 * 32;
              for (int index9 = 0; index9 < 3; ++index9)
              {
                this.drawX = index9 * 48;
                if (index * 60 + this.smTilesPreviewIndex[index7] < Libraries.MapLibs[selectListView.Value].Images.Count)
                {
                  Libraries.MapLibs[selectListView.Value].CheckImage(index * 60 + this.smTilesPreviewIndex[index7]);
                  MLibrary.MImage image = Libraries.MapLibs[selectListView.Value].Images[index * 60 + this.smTilesPreviewIndex[index7]];
                  if (image.Image != null)
                  {
                    Rectangle destRect = new Rectangle(this.drawX, this.drawY, (int) image.Width, (int) image.Height);
                    Rectangle srcRect = new Rectangle(0, 0, (int) image.Width, (int) image.Height);
                    graphics.DrawImage((Image) image.Image, destRect, srcRect, GraphicsUnit.Pixel);
                    Libraries.MapLibs[this.libIndex].Images[index] = (MLibrary.MImage) null;
                    ++index7;
                  }
                }
              }
            }
            break;
          }
          if (selectListView.Text.IndexOf("Tiles30", StringComparison.Ordinal) > -1)
          {
            int index10 = 0;
            int num = (uint) (Libraries.MapLibs[selectListView.Value].Images.Count % 10) <= 0U ? Libraries.MapLibs[selectListView.Value].Images.Count / 30 : (Libraries.MapLibs[selectListView.Value].Images.Count + 1) / 30;
            tilesPreview = new Bitmap(288, 192);
            Graphics graphics = Graphics.FromImage((Image) tilesPreview);
            graphics.InterpolationMode = InterpolationMode.Low;
            int[] numArray = index >= num ? this.Mir3BigTilesPreviewIndex2 : this.Mir3BigTilesPreviewIndex1;
            int index11 = index >= num ? index - num : index;
            for (int index12 = 0; index12 < 3; ++index12)
            {
              this.drawY = index12 * 2 * 32;
              for (int index13 = 0; index13 < 3; ++index13)
              {
                this.drawX = index13 * 2 * 48;
                if (index11 * 30 + numArray[index10] < Libraries.MapLibs[selectListView.Value].Images.Count)
                {
                  Libraries.MapLibs[selectListView.Value].CheckImage(index11 * 30 + numArray[index10]);
                  MLibrary.MImage image = Libraries.MapLibs[selectListView.Value].Images[index11 * 30 + numArray[index10]];
                  if (image.Image != null)
                  {
                    Rectangle destRect = new Rectangle(this.drawX, this.drawY, (int) image.Width, (int) image.Height);
                    Rectangle srcRect = new Rectangle(0, 0, (int) image.Width, (int) image.Height);
                    graphics.DrawImage((Image) image.Image, destRect, srcRect, GraphicsUnit.Pixel);
                    Libraries.MapLibs[this.libIndex].Images[index11] = (MLibrary.MImage) null;
                    ++index10;
                  }
                }
              }
            }
            break;
          }
          break;
      }
      return tilesPreview;
    }

    private void TileslistView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
    {
      int num;
      if (this._tilesIndexList.TryGetValue(e.ItemIndex, out num))
      {
        e.Item = new ListViewItem()
        {
          ImageIndex = num,
          Text = e.ItemIndex.ToString()
        };
      }
      else
      {
        this._tilesIndexList.Add(e.ItemIndex, this.TilesImageList.Images.Count);
        this.TilesImageList.Images.Add((Image) this.GetTilesPreview(this.selectListItem, e.ItemIndex));
        e.Item = new ListViewItem()
        {
          ImageIndex = num,
          Text = e.ItemIndex.ToString()
        };
        Libraries.MapLibs[this.selectListItem.Value].Images[e.ItemIndex] = (MLibrary.MImage) null;
      }
    }

    private void TileslistView_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.TileslistView.SelectedIndices.Count <= 0)
        return;
      this.picTile.Image = (Image) this.GetTilesPreview(this.selectListItem, this.TileslistView.SelectedIndices[0]);
      this.selectTilesIndex = this.TileslistView.SelectedIndices[0];
    }

    private int RandomAutoMir2Tile(Main.TileType tileType)
    {
      if (this.selectTilesIndex < 0)
        return -1;
      switch (tileType)
      {
        case Main.TileType.Center:
          return this.selectTilesIndex * 50 + Main.random.Next(5);
        case Main.TileType.Up:
          return this.selectTilesIndex * 50 + Main.random.Next(15, 17);
        case Main.TileType.UpRight:
          return this.selectTilesIndex * 50 + 6;
        case Main.TileType.Right:
          return Main.random.Next(2) == 0 ? this.selectTilesIndex * 50 + 21 : this.selectTilesIndex * 50 + 23;
        case Main.TileType.DownRight:
          return this.selectTilesIndex * 50 + 8;
        case Main.TileType.Down:
          return this.selectTilesIndex * 50 + Main.random.Next(17, 19);
        case Main.TileType.DownLeft:
          return this.selectTilesIndex * 50 + 7;
        case Main.TileType.Left:
          return Main.random.Next(2) == 0 ? this.selectTilesIndex * 50 + 20 : this.selectTilesIndex * 50 + 22;
        case Main.TileType.UpLeft:
          return this.selectTilesIndex * 50 + 5;
        case Main.TileType.InUpRight:
          return this.selectTilesIndex * 50 + 11;
        case Main.TileType.InDownRight:
          return this.selectTilesIndex * 50 + 13;
        case Main.TileType.InDownLeft:
          return this.selectTilesIndex * 50 + 12;
        case Main.TileType.InUpLeft:
          return this.selectTilesIndex * 50 + 10;
        default:
          return -1;
      }
    }

    private void menuClearMap_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Are You sure you want to clear map tiles?", "Clear Map", MessageBoxButtons.OKCancel) != DialogResult.OK || this.M2CellInfo == null)
        return;
      for (int index1 = 0; index1 < this.mapWidth; ++index1)
      {
        for (int index2 = 0; index2 < this.mapHeight; ++index2)
          this.M2CellInfo[index1, index2] = new CellInfo();
      }
    }

    private void setScrollBar()
    {
      this.OffSetX = this.MapPanel.Width / (48 * Main.zoomMIN / Main.zoomMAX);
      this.OffSetY = this.MapPanel.Height / (32 * Main.zoomMIN / Main.zoomMAX);
      if (this.mapWidth - this.OffSetX >= 0)
        this.hScrollBar.Maximum = this.mapWidth - this.OffSetX;
      else
        this.hScrollBar.Maximum = 0;
      if (this.mapHeight - this.OffSetY >= 0)
        this.vScrollBar.Maximum = this.mapHeight - this.OffSetY;
      else
        this.vScrollBar.Maximum = 0;
      if (this.mapPoint.X >= 0)
      {
        if (this.mapPoint.X <= this.mapWidth)
          this.hScrollBar.Value = this.mapPoint.X;
        else
          this.hScrollBar.Value = this.mapWidth - 1;
      }
      else
        this.hScrollBar.Value = 0;
      if (this.mapPoint.Y >= 0)
      {
        if (this.mapPoint.Y <= this.mapHeight)
          this.vScrollBar.Value = this.mapPoint.Y;
        else
          this.vScrollBar.Value = this.mapHeight - 1;
      }
      else
        this.vScrollBar.Value = 0;
    }

    private void menuNew_Click(object sender, EventArgs e) => this.NewMap();

    private void NewMap()
    {
      if (new NewFileFrm(new Main.DelSetMapSize(this.SetMapSize)).ShowDialog() != DialogResult.OK)
        return;
      this.M2CellInfo = new CellInfo[this.mapWidth, this.mapHeight];
      for (int index1 = 0; index1 < this.mapWidth; ++index1)
      {
        for (int index2 = 0; index2 < this.mapHeight; ++index2)
          this.M2CellInfo[index1, index2] = new CellInfo();
      }
      this.mapPoint = new Point(0, 0);
      this.setScrollBar();
    }

    private void menuOpen_Click(object sender, EventArgs e) => this.OpenMap();

    private void OpenMap()
    {
      this.ClearImage();
      OpenFileDialog openFileDialog = new OpenFileDialog();
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      this.map = new MapReader(openFileDialog.FileName);
      this.M2CellInfo = this.map.MapCells;
      this.mapPoint = new Point(0, 0);
      this.SetMapSize(this.map.Width, this.map.Height);
      this.mapFileName = openFileDialog.FileName;
      this.setScrollBar();
    }

    private void menuSave_Click(object sender, EventArgs e) => this.Save();

    private void menuUndo_Click(object sender, EventArgs e) => this.UnDo();

    private void menuRedo_Click(object sender, EventArgs e) => this.ReDo();

    private void menuZoomIn_Click(object sender, EventArgs e) => this.ZoomIn();

    private void ZoomIn()
    {
      if (this.map == null)
        return;
      this.EnlargeZoom();
      this.SetMapSize(this.mapWidth, this.mapHeight);
    }

    private void menuZoomOut_Click(object sender, EventArgs e) => this.ZoomOut();

    private void ZoomOut()
    {
      if (this.map == null)
        return;
      this.NarrowZoom();
      this.SetMapSize(this.mapWidth, this.mapHeight);
    }

    private void hScrollBar_Scroll(object sender, ScrollEventArgs e) => this.Jump(this.hScrollBar.Value, this.mapPoint.Y);

    private void vScrollBar_Scroll(object sender, ScrollEventArgs e) => this.Jump(this.mapPoint.X, this.vScrollBar.Value);

    private void menu_DeleteSelectedCellData_Click(object sender, EventArgs e)
    {
      if (this.M2CellInfo == null || MessageBox.Show("Are You sure you want to delete selected Cell Data?", "Delete", MessageBoxButtons.OKCancel) != DialogResult.OK)
        return;
      if (this.p1.X > this.p2.X)
      {
        this.p1.X += this.p2.X;
        this.p2.X = this.p1.X - this.p2.X - 1;
        this.p1.X -= this.p2.X;
      }
      if (this.p1.Y > this.p2.Y)
      {
        this.p1.Y += this.p2.Y;
        this.p2.Y = this.p1.Y - this.p2.Y - 1;
        this.p1.Y -= this.p2.Y;
      }
      for (int x = this.p1.X; x <= this.p2.X; ++x)
      {
        for (int y = this.p1.Y; y <= this.p2.Y; ++y)
          this.M2CellInfo[x, y] = new CellInfo();
      }
    }

    private void menu_SaveObject_Click(object sender, EventArgs e)
    {
      if (this.M2CellInfo == null)
        return;
      if (this.p1.X > this.p2.X)
      {
        this.p1.X += this.p2.X;
        this.p2.X = this.p1.X - this.p2.X - 1;
        this.p1.X -= this.p2.X;
      }
      if (this.p1.Y > this.p2.Y)
      {
        this.p1.Y += this.p2.Y;
        this.p2.Y = this.p1.Y - this.p2.Y - 1;
        this.p1.Y -= this.p2.Y;
      }
      this.SaveObjectsFile();
      this.ObjectslistBox.Items.Clear();
      this.ReadObjectsToListBox();
    }

    private void menuFreeMemory_Click(object sender, EventArgs e) => this.Dispose();

    private void menuJump_Click(object sender, EventArgs e)
    {
      if (this.M2CellInfo == null || new FrmJump(new Main.DelJump(this.Jump)).ShowDialog() != DialogResult.OK)
        ;
    }

    private void menuInvertMir3Layer_Click(object sender, EventArgs e) => this.InvertMir3Layer();

    private void menuAbout_Click(object sender, EventArgs e)
    {
      int num = (int) new FrmAbout().ShowDialog();
    }

    private void btnOpen_Click(object sender, EventArgs e) => this.OpenMap();

    private void btnNew_Click(object sender, EventArgs e) => this.NewMap();

    private int RandomAutoMir3Tile(Main.TileType tileType)
    {
      if (this.selectTilesIndex < 0)
        return -1;
      int num = (uint) (Libraries.MapLibs[this.selectListItem.Value].Images.Count % 10) <= 0U ? Libraries.MapLibs[this.selectListItem.Value].Images.Count / 30 : (Libraries.MapLibs[this.selectListItem.Value].Images.Count + 1) / 30;
      if (this.selectTilesIndex < num)
      {
        switch (tileType)
        {
          case Main.TileType.Center:
            return this.selectTilesIndex * 30 + Main.random.Next(5);
          case Main.TileType.Up:
            return this.selectTilesIndex * 30 + Main.random.Next(20, 22);
          case Main.TileType.UpRight:
            return this.selectTilesIndex * 30 + 11;
          case Main.TileType.Right:
            return Main.random.Next(2) == 0 ? this.selectTilesIndex * 30 + 26 : this.selectTilesIndex * 30 + 28;
          case Main.TileType.DownRight:
            return this.selectTilesIndex * 30 + 13;
          case Main.TileType.Down:
            return this.selectTilesIndex * 30 + Main.random.Next(22, 24);
          case Main.TileType.DownLeft:
            return this.selectTilesIndex * 30 + 12;
          case Main.TileType.Left:
            return Main.random.Next(2) == 0 ? this.selectTilesIndex * 30 + 25 : this.selectTilesIndex * 30 + 27;
          case Main.TileType.UpLeft:
            return this.selectTilesIndex * 30 + 10;
          case Main.TileType.InUpRight:
            return this.selectTilesIndex * 30 + 16;
          case Main.TileType.InDownRight:
            return this.selectTilesIndex * 30 + 18;
          case Main.TileType.InDownLeft:
            return this.selectTilesIndex * 30 + 17;
          case Main.TileType.InUpLeft:
            return this.selectTilesIndex * 30 + 15;
        }
      }
      else
      {
        switch (tileType)
        {
          case Main.TileType.Center:
            return (this.selectTilesIndex - num) * 30 + Main.random.Next(5, 10);
          case Main.TileType.Up:
            return (this.selectTilesIndex - num) * 30 + Main.random.Next(22, 24);
          case Main.TileType.UpRight:
            return (this.selectTilesIndex - num) * 30 + 17;
          case Main.TileType.Right:
            return Main.random.Next(2) == 0 ? (this.selectTilesIndex - num) * 30 + 25 : (this.selectTilesIndex - num) * 30 + 27;
          case Main.TileType.DownRight:
            return (this.selectTilesIndex - num) * 30 + 15;
          case Main.TileType.Down:
            return (this.selectTilesIndex - num) * 30 + Main.random.Next(20, 22);
          case Main.TileType.DownLeft:
            return (this.selectTilesIndex - num) * 30 + 16;
          case Main.TileType.Left:
            return Main.random.Next(2) == 0 ? (this.selectTilesIndex - num) * 30 + 26 : (this.selectTilesIndex - num) * 30 + 28;
          case Main.TileType.UpLeft:
            return (this.selectTilesIndex - num) * 30 + 18;
          case Main.TileType.InUpRight:
            return (this.selectTilesIndex - num) * 30 + 12;
          case Main.TileType.InDownRight:
            return (this.selectTilesIndex - num) * 30 + 10;
          case Main.TileType.InDownLeft:
            return (this.selectTilesIndex - num) * 30 + 11;
          case Main.TileType.InUpLeft:
            return (this.selectTilesIndex - num) * 30 + 13;
        }
      }
      return -1;
    }

    private void btnMiniMap_Click(object sender, EventArgs e) => this.createMiniMap();

    private void btnFreeMemory_Click(object sender, EventArgs e) => this.Dispose();

    private void btnSave_Click(object sender, EventArgs e) => this.Save();

    private void btn_load_Click(object sender, EventArgs e)
    {
      if (this.loadImageDialog.ShowDialog() != DialogResult.OK)
        return;
      string fileName = this.loadImageDialog.FileName;
      try
      {
        this._mainImage = new Bitmap(fileName);
        this.pictureBox_Image.Image = (Image) this._mainImage;
        this.pictureBox_Image.Width = this._mainImage.Width;
        this.pictureBox_Image.Height = this._mainImage.Height;
        this.pictureBox_Grid.Width = this._mainImage.Width;
        this.pictureBox_Grid.Height = this._mainImage.Height;
        this.pictureBox_Highlight.Width = this._mainImage.Width;
        this.pictureBox_Highlight.Height = this._mainImage.Height;
        this.pictureBox_loaded = true;
      }
      catch
      {
      }
      this.CellSizeX = (this.comboBox_cellSize.SelectedIndex + 1) * 48;
      this.CellSizeY = (this.comboBox_cellSize.SelectedIndex + 1) * 32;
      this.SelectedCells = new int[this.pictureBox_Image.Image.Width / this.CellSizeX + 2, this.pictureBox_Image.Image.Height / this.CellSizeY + 2];
      this.gridUpdate(false);
    }

    private void gridUpdate(bool toggle)
    {
      if (this.pictureBox_Image.Image == null)
        return;
      if (toggle)
        this.grid = !this.grid;
      if (this.grid)
      {
        Bitmap bitmap = new Bitmap(this.pictureBox_Grid.Width, this.pictureBox_Grid.Height);
        this.CellSizeX = (this.comboBox_cellSize.SelectedIndex + 1) * 48;
        this.CellSizeY = (this.comboBox_cellSize.SelectedIndex + 1) * 32;
        for (int y = 0; y < (this.pictureBox_Image.Image.Height / this.CellSizeY + 2) * this.CellSizeY + 1; y = y + (this.CellSizeY - 1) + 1)
        {
          for (int x = 0; x < (this.pictureBox_Image.Image.Width / this.CellSizeX + 2) * this.CellSizeX; ++x)
          {
            if (x < this.pictureBox_Grid.Width && y < this.pictureBox_Grid.Height)
              bitmap.SetPixel(x, y, Color.HotPink);
          }
        }
        for (int x = 0; x < (this.pictureBox_Image.Image.Width / this.CellSizeX + 2) * this.CellSizeX + 1; x = x + (this.CellSizeX - 1) + 1)
        {
          for (int y = 0; y < (this.pictureBox_Image.Image.Height / this.CellSizeY + 2) * this.CellSizeY; ++y)
          {
            if (x < this.pictureBox_Grid.Width && y < this.pictureBox_Grid.Height)
              bitmap.SetPixel(x, y, Color.HotPink);
          }
        }
        this.pictureBox_Grid.Image = (Image) bitmap;
      }
      else
        this.pictureBox_Grid.Image = (Image) new Bitmap(1, 1);
    }

    private void btn_up_Click(object sender, EventArgs e)
    {
      PictureBox pictureBoxImage = this.pictureBox_Image;
      Padding padding1 = this.pictureBox_Image.Padding;
      int left = padding1.Left;
      padding1 = this.pictureBox_Image.Padding;
      int top = padding1.Top - 1;
      Padding padding2 = new Padding(left, top, 0, 0);
      pictureBoxImage.Padding = padding2;
      this.pictureBox_Image.Image = this.pictureBox_Image.Image;
    }

    private void btn_down_Click(object sender, EventArgs e)
    {
      PictureBox pictureBoxImage = this.pictureBox_Image;
      Padding padding1 = this.pictureBox_Image.Padding;
      int left = padding1.Left;
      padding1 = this.pictureBox_Image.Padding;
      int top = padding1.Top + 1;
      Padding padding2 = new Padding(left, top, 0, 0);
      pictureBoxImage.Padding = padding2;
      this.pictureBox_Image.Image = this.pictureBox_Image.Image;
    }

    private void btn_left_Click(object sender, EventArgs e)
    {
      PictureBox pictureBoxImage = this.pictureBox_Image;
      Padding padding1 = this.pictureBox_Image.Padding;
      int left = padding1.Left - 1;
      padding1 = this.pictureBox_Image.Padding;
      int top = padding1.Top;
      Padding padding2 = new Padding(left, top, 0, 0);
      pictureBoxImage.Padding = padding2;
      this.pictureBox_Image.Image = this.pictureBox_Image.Image;
    }

    private void btn_right_Click(object sender, EventArgs e)
    {
      PictureBox pictureBoxImage = this.pictureBox_Image;
      Padding padding1 = this.pictureBox_Image.Padding;
      int left = padding1.Left + 1;
      padding1 = this.pictureBox_Image.Padding;
      int top = padding1.Top;
      Padding padding2 = new Padding(left, top, 0, 0);
      pictureBoxImage.Padding = padding2;
      this.pictureBox_Image.Image = this.pictureBox_Image.Image;
    }

    private void btn_grid_Click(object sender, EventArgs e) => this.gridUpdate(true);

    private void pictureBox_Highlight_Click(object sender, EventArgs e)
    {
      if (!this.pictureBox_loaded)
        return;
      Main.MPoint = this.pictureBox_Grid.PointToClient(Cursor.Position);
      if (Main.MPoint.Y >= 0)
      {
        Main.MPoint.Y /= this.CellSizeY;
        if (Main.MPoint.X >= 0)
          Main.MPoint.X /= this.CellSizeX;
        else
          Main.MPoint.X = -1;
      }
      else
        Main.MPoint.Y = -1;
      if (Main.MPoint.X < 0 || Main.MPoint.Y < 0 || Main.MPoint.X > this.pictureBox_Image.Image.Width / this.CellSizeX + 1 || Main.MPoint.Y > this.pictureBox_Image.Image.Height / this.CellSizeY + 1)
        return;
      if (this.grid)
      {
        Bitmap bitmap = new Bitmap(this.pictureBox_Grid.Width, this.pictureBox_Grid.Height);
        this.SelectedCells[Main.MPoint.X, Main.MPoint.Y] = this.SelectedCells[Main.MPoint.X, Main.MPoint.Y] == 1 ? 0 : 1;
        for (int index1 = 0; index1 <= this.pictureBox_Image.Image.Height / this.CellSizeY + 1; ++index1)
        {
          for (int index2 = 0; index2 <= this.pictureBox_Image.Image.Width / this.CellSizeX + 1; ++index2)
          {
            if (this.SelectedCells[index2, index1] == 1)
            {
              using (Graphics graphics = Graphics.FromImage((Image) bitmap))
                graphics.DrawImage((Image) this.cellHighlight, new Point(index2 * this.CellSizeX, index1 * this.CellSizeY));
            }
          }
        }
        this.pictureBox_Highlight.Image = (Image) bitmap;
      }
      else
        this.pictureBox_Highlight.Image = (Image) new Bitmap(1, 1);
    }

    private void comboBox_cellSize_SelectedIndexChanged(object sender, EventArgs e) => this.gridUpdate(false);

    private void Main_ResizeEnd(object sender, EventArgs e) => this.gridUpdate(false);

    private void btn_vCut_Click(object sender, EventArgs e)
    {
      Bitmap bitmap = new Bitmap(this._mainImage.Width + this.pictureBox_Image.Padding.Left, this._mainImage.Height + this.pictureBox_Image.Padding.Top);
      using (Graphics graphics = Graphics.FromImage((Image) bitmap))
        graphics.DrawImage((Image) this._mainImage, this.pictureBox_Image.Padding.Left, this.pictureBox_Image.Padding.Top, this._mainImage.Width, this._mainImage.Height);
      if (this.SaveLibraryDialog.ShowDialog() != DialogResult.OK)
        return;
      if (this._library != null)
        this._library.Close();
      this._library = new MLibrary(this.SaveLibraryDialog.FileName);
      this._library.AddImage((Bitmap) null, (short) 0, (short) 0);
      int num = 0;
      for (int index1 = 0; index1 < this.pictureBox_Image.Image.Width / this.CellSizeX + 2; ++index1)
      {
        for (int index2 = 0; index2 < this.pictureBox_Image.Image.Height / this.CellSizeY + 2; ++index2)
        {
          bool flag = true;
          if (num == 0)
          {
            for (int index3 = 0; index3 < this.CellSizeY; ++index3)
            {
              for (int index4 = 0; index4 < this.CellSizeX; ++index4)
              {
                if (index4 * index1 <= bitmap.Width && index3 * index2 <= bitmap.Height && bitmap.GetPixel(index4 * index1, index3 * index2).A > (byte) 0)
                {
                  flag = false;
                  break;
                }
              }
              if (!flag)
                break;
            }
            if (flag)
              continue;
          }
          if (this.SelectedCells[index1, index2] == 1)
          {
            Bitmap image = new Bitmap(this.CellSizeX, this.CellSizeY * (num + 1), PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage((Image) image))
              graphics.DrawImage((Image) bitmap, new Rectangle(0, 0, image.Width, image.Height), new Rectangle(index1 * this.CellSizeX, (index2 - num) * this.CellSizeY, image.Width, image.Height), GraphicsUnit.Pixel);
            for (int y = 0; y < image.Height; ++y)
            {
              for (int x = 0; x < image.Width; ++x)
              {
                if (image.GetPixel(x, y).A > (byte) 0)
                  flag = false;
              }
            }
            if (!flag)
              this._library.AddImage(image, (short) 0, (short) 0);
            num = 0;
          }
          else
            ++num;
        }
        if (num > 0)
        {
          Bitmap image = new Bitmap(this.CellSizeX, this.CellSizeY * num, PixelFormat.Format32bppArgb);
          using (Graphics graphics = Graphics.FromImage((Image) image))
            graphics.DrawImage((Image) bitmap, new Rectangle(0, 0, image.Width, image.Height), new Rectangle(index1 * this.CellSizeX, (this.pictureBox_Image.Image.Height / this.CellSizeY + 2 - num) * this.CellSizeY, image.Width, image.Height), GraphicsUnit.Pixel);
          bool flag = true;
          for (int y = 0; y < image.Height; ++y)
          {
            for (int x = 0; x < image.Width; ++x)
            {
              if (image.GetPixel(x, y).A > (byte) 0)
                flag = false;
            }
          }
          if (!flag)
            this._library.AddImage(image, (short) 0, (short) 0);
          num = 0;
        }
      }
      this._library.Save();
    }

    private void btnRefreshList_Click(object sender, EventArgs e)
    {
      this.ObjectslistBox.Items.Clear();
      this.ReadObjectsToListBox();
    }

    private void menuSelectAllCells_Click(object sender, EventArgs e)
    {
      Bitmap bitmap = new Bitmap(this.pictureBox_Grid.Width, this.pictureBox_Grid.Height);
      for (int index1 = 0; index1 <= this.pictureBox_Image.Image.Height / this.CellSizeY + 1; ++index1)
      {
        for (int index2 = 0; index2 <= this.pictureBox_Image.Image.Width / this.CellSizeX + 1; ++index2)
        {
          this.SelectedCells[index2, index1] = 1;
          using (Graphics graphics = Graphics.FromImage((Image) bitmap))
            graphics.DrawImage((Image) this.cellHighlight, new Point(index2 * this.CellSizeX, index1 * this.CellSizeY));
        }
      }
      this.pictureBox_Highlight.Image = (Image) bitmap;
    }

    private void menuDeselectAllCells_Click(object sender, EventArgs e)
    {
      for (int index1 = 0; index1 <= this.pictureBox_Image.Image.Height / this.CellSizeY + 1; ++index1)
      {
        for (int index2 = 0; index2 <= this.pictureBox_Image.Image.Width / this.CellSizeX + 1; ++index2)
          this.SelectedCells[index2, index1] = 0;
      }
      this.pictureBox_Highlight.Image = (Image) new Bitmap(this.pictureBox_Grid.Width, this.pictureBox_Grid.Height);
    }

    private int RandomAutoSmTile(Main.TileType iTileType) => iTileType >= Main.TileType.Up ? this.selectTilesIndex * 60 + 8 + (int) (iTileType - 1) * 4 + Main.random.Next(4) : this.selectTilesIndex * 60 + Main.random.Next(8);

    public void createMiniMap()
    {
      Bitmap original = new Bitmap(this.mapWidth * 12, this.mapHeight * 8, PixelFormat.Format32bppArgb);
      for (int index1 = 0; index1 <= this.mapHeight - 1; index1 = index1 + 1 + 1)
      {
        for (int index2 = 0; index2 <= this.mapWidth - 1; index2 = index2 + 1 + 1)
        {
          if ((uint) (this.M2CellInfo[index2, index1].BackImage & 536870911) > 0U)
          {
            try
            {
              Libraries.MapLibs[(int) this.M2CellInfo[index2, index1].BackIndex].CheckImage((this.M2CellInfo[index2, index1].BackImage & 536870911) - 1);
              MLibrary.MImage image = Libraries.MapLibs[(int) this.M2CellInfo[index2, index1].BackIndex].Images[(this.M2CellInfo[index2, index1].BackImage & 536870911) - 1];
              if (image.Image != null || image.ImageTexture != (Texture) null)
              {
                using (Graphics graphics = Graphics.FromImage((Image) original))
                {
                  Rectangle rect = new Rectangle(index2 * 12, index1 * 8, 24, 16);
                  graphics.DrawImage((Image) image.Image, rect);
                }
              }
            }
            catch
            {
            }
          }
        }
      }
      for (int index3 = 0; index3 <= this.mapHeight - 1; ++index3)
      {
        for (int index4 = 0; index4 <= this.mapWidth - 1; ++index4)
        {
          if ((uint) this.M2CellInfo[index4, index3].MiddleImage > 0U)
          {
            try
            {
              Libraries.MapLibs[(int) this.M2CellInfo[index4, index3].MiddleIndex].CheckImage((int) this.M2CellInfo[index4, index3].MiddleImage - 1);
              MLibrary.MImage image = Libraries.MapLibs[(int) this.M2CellInfo[index4, index3].MiddleIndex].Images[(int) this.M2CellInfo[index4, index3].MiddleImage - 1];
              if (image.Image != null || image.ImageTexture != (Texture) null)
              {
                using (Graphics graphics = Graphics.FromImage((Image) original))
                {
                  Rectangle rect = new Rectangle(index4 * 12, index3 * 8 - image.Image.Height / 4 + 8, image.Image.Width / 4, image.Image.Height / 4);
                  graphics.DrawImage((Image) image.Image, rect);
                }
              }
            }
            catch
            {
            }
          }
        }
      }
      for (int index5 = 0; index5 <= this.mapHeight - 1; ++index5)
      {
        for (int index6 = 0; index6 <= this.mapWidth - 1; ++index6)
        {
          if (((int) this.M2CellInfo[index6, index5].FrontImage & (int) short.MaxValue) != 0 && ((int) this.M2CellInfo[index6, index5].FrontAnimationFrame & 128) < 1)
          {
            try
            {
              Libraries.MapLibs[(int) this.M2CellInfo[index6, index5].FrontIndex].CheckImage(((int) this.M2CellInfo[index6, index5].FrontImage & (int) short.MaxValue) - 1);
              MLibrary.MImage image = Libraries.MapLibs[(int) this.M2CellInfo[index6, index5].FrontIndex].Images[((int) this.M2CellInfo[index6, index5].FrontImage & (int) short.MaxValue) - 1];
              if (image.Image != null || image.ImageTexture != (Texture) null)
              {
                using (Graphics graphics = Graphics.FromImage((Image) original))
                {
                  Rectangle rect = new Rectangle(index6 * 12, index5 * 8 - image.Image.Height / 4 + 8, image.Image.Width / 4, image.Image.Height / 4);
                  graphics.DrawImage((Image) image.Image, rect);
                }
              }
            }
            catch
            {
            }
          }
        }
      }
      Bitmap bitmap = new Bitmap((Image) original, original.Width / 8, original.Height / 8);
      string str = Path.GetDirectoryName(this.mapFileName) + "\\" + Path.GetFileNameWithoutExtension(this.mapFileName);
      bitmap.Save(str + "_MiniMap.png", ImageFormat.Png);
      int num = (int) MessageBox.Show("Saved... " + str + "_MiniMap.png");
    }

    public static bool AppStillIdle => !Main.PeekMessage(out Main.PeekMsg _, IntPtr.Zero, 0U, 0U, 0U);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    private static extern bool PeekMessage(
      out Main.PeekMsg msg,
      IntPtr hWnd,
      uint messageFilterMin,
      uint messageFilterMax,
      uint flags);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Main));
      this.toolStrip1 = new ToolStrip();
      this.btnNew = new ToolStripButton();
      this.btnOpen = new ToolStripButton();
      this.btnSave = new ToolStripButton();
      this.btnJump = new ToolStripButton();
      this.btnFreeMemory = new ToolStripButton();
      this.toolStripDropDownButton2 = new ToolStripDropDownButton();
      this.chkBack = new ToolStripMenuItem();
      this.chkMidd = new ToolStripMenuItem();
      this.chkFront = new ToolStripMenuItem();
      this.chkShowCellInfo = new ToolStripMenuItem();
      this.chkDoor = new ToolStripMenuItem();
      this.chkDoorSign = new ToolStripMenuItem();
      this.chkFrontAnimationTag = new ToolStripMenuItem();
      this.chkMiddleAnimationTag = new ToolStripMenuItem();
      this.chkLightTag = new ToolStripMenuItem();
      this.chkBackMask = new ToolStripMenuItem();
      this.chkFrontMask = new ToolStripMenuItem();
      this.chkDrawGrids = new ToolStripMenuItem();
      this.chkFrontTag = new ToolStripMenuItem();
      this.chkMiddleTag = new ToolStripMenuItem();
      this.toolStripLabel1 = new ToolStripLabel();
      this.cmbEditorLayer = new ToolStripComboBox();
      this.btnMiniMap = new ToolStripButton();
      this.tabControl1 = new TabControl();
      this.tabWemadeMir2 = new TabPage();
      this.labWemadeMir2OffSetY = new Label();
      this.labeWemadeMir2OffSetX = new Label();
      this.LabWemadeMir2Height = new Label();
      this.LabWemadeMir2Width = new Label();
      this.picWemdeMir2 = new PictureBox();
      this.WemadeMir2LibListBox = new ListBox();
      this.WemadeMir2LibListView = new ListView();
      this.WemadeMir2ImageList = new ImageList(this.components);
      this.tabShandaMir2 = new TabPage();
      this.labshandaMir2OffSetY = new Label();
      this.labShandaMir2OffSetX = new Label();
      this.labShandaMir2Height = new Label();
      this.labShandaMir2Width = new Label();
      this.picShandaMir2 = new PictureBox();
      this.ShandaMir2LibListBox = new ListBox();
      this.ShandaMir2LibListView = new ListView();
      this.ShandaMir2ImageList = new ImageList(this.components);
      this.tabWemadeMir3 = new TabPage();
      this.labWemadeMir3OffSetY = new Label();
      this.labeWemadeMir3OffSetX = new Label();
      this.LabWemadeMir3Height = new Label();
      this.LabWemadeMir3Width = new Label();
      this.picWemdeMir3 = new PictureBox();
      this.WemadeMir3LibListBox = new ListBox();
      this.WemadeMir3LibListView = new ListView();
      this.WemadeMir3ImageList = new ImageList(this.components);
      this.tabShandaMir3 = new TabPage();
      this.labshandaMir3OffSetY = new Label();
      this.labShandaMir3OffSetX = new Label();
      this.labShandaMir3Height = new Label();
      this.labShandaMir3Width = new Label();
      this.picShandaMir3 = new PictureBox();
      this.ShandaMir3LibListBox = new ListBox();
      this.ShandaMir3LibListView = new ListView();
      this.ShandaMir3ImageList = new ImageList(this.components);
      this.tabObjects = new TabPage();
      this.splitContainer1 = new SplitContainer();
      this.ObjectslistBox = new ListBox();
      this.splitContainer2 = new SplitContainer();
      this.btnRefreshList = new Button();
      this.btnDeleteObjects = new Button();
      this.picObjects = new PictureBox();
      this.tabTiles = new TabPage();
      this.picTile = new PictureBox();
      this.TileslistView = new ListView();
      this.TilesImageList = new ImageList(this.components);
      this.tabMap = new TabPage();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.vScrollBar = new VScrollBar();
      this.MapPanel = new Panel();
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.撤销ToolStripMenuItem = new ToolStripMenuItem();
      this.返回ToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator2 = new ToolStripSeparator();
      this.menu_SaveObject = new ToolStripMenuItem();
      this.btnSetDoor = new ToolStripMenuItem();
      this.btnSetAnimation = new ToolStripMenuItem();
      this.btnSetLight = new ToolStripMenuItem();
      this.menu_DeleteSelectedCellData = new ToolStripMenuItem();
      this.hScrollBar = new HScrollBar();
      this.tabTileCutter = new TabPage();
      this.splitContainer3 = new SplitContainer();
      this.btn_grid = new Button();
      this.btn_vCut = new Button();
      this.btn_load = new Button();
      this.label1 = new Label();
      this.btn_up = new Button();
      this.comboBox_cellSize = new ComboBox();
      this.btn_left = new Button();
      this.btn_down = new Button();
      this.btn_right = new Button();
      this.pictureBox_Highlight = new PictureBox();
      this.pictureBox_Grid = new PictureBox();
      this.pictureBox_Image = new PictureBox();
      this.ObjectsimageList = new ImageList(this.components);
      this.menuStrip1 = new MenuStrip();
      this.toolStripMenuItem1 = new ToolStripMenuItem();
      this.menuNew = new ToolStripMenuItem();
      this.menuOpen = new ToolStripMenuItem();
      this.menuSave = new ToolStripMenuItem();
      this.toolStripMenuItem2 = new ToolStripMenuItem();
      this.menuUndo = new ToolStripMenuItem();
      this.menuRedo = new ToolStripMenuItem();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.menuClearMap = new ToolStripMenuItem();
      this.toolStripMenuItem9 = new ToolStripMenuItem();
      this.menuZoomIn = new ToolStripMenuItem();
      this.menuZoomOut = new ToolStripMenuItem();
      this.toolsToolStripMenuItem = new ToolStripMenuItem();
      this.menuFreeMemory = new ToolStripMenuItem();
      this.menuJump = new ToolStripMenuItem();
      this.menuInvertMir3Layer = new ToolStripMenuItem();
      this.helpToolStripMenuItem = new ToolStripMenuItem();
      this.menuAbout = new ToolStripMenuItem();
      this.loadImageDialog = new OpenFileDialog();
      this.SaveLibraryDialog = new SaveFileDialog();
      this.contextMenuTileCutter = new ContextMenuStrip(this.components);
      this.menuSelectAllCells = new ToolStripMenuItem();
      this.menuDeselectAllCells = new ToolStripMenuItem();
      this.toolStrip1.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabWemadeMir2.SuspendLayout();
      ((ISupportInitialize) this.picWemdeMir2).BeginInit();
      this.tabShandaMir2.SuspendLayout();
      ((ISupportInitialize) this.picShandaMir2).BeginInit();
      this.tabWemadeMir3.SuspendLayout();
      ((ISupportInitialize) this.picWemdeMir3).BeginInit();
      this.tabShandaMir3.SuspendLayout();
      ((ISupportInitialize) this.picShandaMir3).BeginInit();
      this.tabObjects.SuspendLayout();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.splitContainer2.BeginInit();
      this.splitContainer2.Panel1.SuspendLayout();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      ((ISupportInitialize) this.picObjects).BeginInit();
      this.tabTiles.SuspendLayout();
      ((ISupportInitialize) this.picTile).BeginInit();
      this.tabMap.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      this.contextMenuStrip1.SuspendLayout();
      this.tabTileCutter.SuspendLayout();
      this.splitContainer3.BeginInit();
      this.splitContainer3.Panel1.SuspendLayout();
      this.splitContainer3.Panel2.SuspendLayout();
      this.splitContainer3.SuspendLayout();
      ((ISupportInitialize) this.pictureBox_Highlight).BeginInit();
      ((ISupportInitialize) this.pictureBox_Grid).BeginInit();
      ((ISupportInitialize) this.pictureBox_Image).BeginInit();
      this.menuStrip1.SuspendLayout();
      this.contextMenuTileCutter.SuspendLayout();
      this.SuspendLayout();
      this.toolStrip1.ImageScalingSize = new Size(20, 20);
      this.toolStrip1.Items.AddRange(new ToolStripItem[9]
      {
        (ToolStripItem) this.btnNew,
        (ToolStripItem) this.btnOpen,
        (ToolStripItem) this.btnSave,
        (ToolStripItem) this.btnJump,
        (ToolStripItem) this.btnFreeMemory,
        (ToolStripItem) this.toolStripDropDownButton2,
        (ToolStripItem) this.toolStripLabel1,
        (ToolStripItem) this.cmbEditorLayer,
        (ToolStripItem) this.btnMiniMap
      });
      this.toolStrip1.Location = new Point(0, 28);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new Size(1982, 59);
      this.toolStrip1.TabIndex = 3;
      this.toolStrip1.Text = "toolStrip1";
      this.btnNew.Font = new System.Drawing.Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnNew.Image = (Image) Resources._100;
      this.btnNew.ImageScaling = ToolStripItemImageScaling.None;
      this.btnNew.ImageTransparentColor = Color.Magenta;
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(44, 56);
      this.btnNew.Text = "New";
      this.btnNew.TextImageRelation = TextImageRelation.ImageAboveText;
      this.btnNew.ToolTipText = "Create New Map";
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.btnOpen.Font = new System.Drawing.Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnOpen.Image = (Image) Resources._110;
      this.btnOpen.ImageScaling = ToolStripItemImageScaling.None;
      this.btnOpen.ImageTransparentColor = Color.Magenta;
      this.btnOpen.Name = "btnOpen";
      this.btnOpen.Size = new Size(49, 56);
      this.btnOpen.Text = "Open";
      this.btnOpen.TextImageRelation = TextImageRelation.ImageAboveText;
      this.btnOpen.ToolTipText = "Open Map";
      this.btnOpen.Click += new EventHandler(this.btnOpen_Click);
      this.btnSave.Font = new System.Drawing.Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnSave.Image = (Image) Resources._120;
      this.btnSave.ImageScaling = ToolStripItemImageScaling.None;
      this.btnSave.ImageTransparentColor = Color.Magenta;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(46, 56);
      this.btnSave.Text = "Save";
      this.btnSave.TextImageRelation = TextImageRelation.ImageAboveText;
      this.btnSave.ToolTipText = "Save Map";
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnJump.Font = new System.Drawing.Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnJump.Image = (Image) Resources.Navigator;
      this.btnJump.ImageScaling = ToolStripItemImageScaling.None;
      this.btnJump.ImageTransparentColor = Color.Magenta;
      this.btnJump.Name = "btnJump";
      this.btnJump.Size = new Size(50, 56);
      this.btnJump.Text = "Jump";
      this.btnJump.TextImageRelation = TextImageRelation.ImageAboveText;
      this.btnJump.ToolTipText = "Teleport to Location";
      this.btnJump.Click += new EventHandler(this.btnJump_Click);
      this.btnFreeMemory.Font = new System.Drawing.Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnFreeMemory.Image = (Image) Resources.System_folder;
      this.btnFreeMemory.ImageScaling = ToolStripItemImageScaling.None;
      this.btnFreeMemory.ImageTransparentColor = Color.Magenta;
      this.btnFreeMemory.Name = "btnFreeMemory";
      this.btnFreeMemory.Size = new Size(100, 56);
      this.btnFreeMemory.Text = "FreeMemory";
      this.btnFreeMemory.TextImageRelation = TextImageRelation.ImageAboveText;
      this.btnFreeMemory.ToolTipText = "Free Memory";
      this.btnFreeMemory.Click += new EventHandler(this.btnFreeMemory_Click);
      this.toolStripDropDownButton2.DropDownItems.AddRange(new ToolStripItem[14]
      {
        (ToolStripItem) this.chkBack,
        (ToolStripItem) this.chkMidd,
        (ToolStripItem) this.chkFront,
        (ToolStripItem) this.chkShowCellInfo,
        (ToolStripItem) this.chkDoor,
        (ToolStripItem) this.chkDoorSign,
        (ToolStripItem) this.chkFrontAnimationTag,
        (ToolStripItem) this.chkMiddleAnimationTag,
        (ToolStripItem) this.chkLightTag,
        (ToolStripItem) this.chkBackMask,
        (ToolStripItem) this.chkFrontMask,
        (ToolStripItem) this.chkDrawGrids,
        (ToolStripItem) this.chkFrontTag,
        (ToolStripItem) this.chkMiddleTag
      });
      this.toolStripDropDownButton2.Font = new System.Drawing.Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.toolStripDropDownButton2.Image = (Image) Resources._190;
      this.toolStripDropDownButton2.ImageScaling = ToolStripItemImageScaling.None;
      this.toolStripDropDownButton2.ImageTransparentColor = Color.Magenta;
      this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
      this.toolStripDropDownButton2.Size = new Size(56, 56);
      this.toolStripDropDownButton2.Text = "View";
      this.toolStripDropDownButton2.TextImageRelation = TextImageRelation.ImageAboveText;
      this.chkBack.Checked = true;
      this.chkBack.CheckState = CheckState.Checked;
      this.chkBack.Name = "chkBack";
      this.chkBack.Size = new Size(400, 26);
      this.chkBack.Text = "显示背景图片 show background image ";
      this.chkBack.Click += new EventHandler(this.chkBack_Click);
      this.chkMidd.Checked = true;
      this.chkMidd.CheckState = CheckState.Checked;
      this.chkMidd.Name = "chkMidd";
      this.chkMidd.Size = new Size(400, 26);
      this.chkMidd.Text = "显示中间图片 Show middle image ";
      this.chkMidd.Click += new EventHandler(this.chkMidd_Click);
      this.chkFront.Checked = true;
      this.chkFront.CheckState = CheckState.Checked;
      this.chkFront.Name = "chkFront";
      this.chkFront.Size = new Size(400, 26);
      this.chkFront.Text = "显示前景图片 show front image ";
      this.chkFront.Click += new EventHandler(this.chkFront_Click);
      this.chkShowCellInfo.Name = "chkShowCellInfo";
      this.chkShowCellInfo.Size = new Size(400, 26);
      this.chkShowCellInfo.Text = "显示cell信息 show Cell Info";
      this.chkShowCellInfo.Click += new EventHandler(this.chkShowCellInfo_Click);
      this.chkDoor.Name = "chkDoor";
      this.chkDoor.Size = new Size(400, 26);
      this.chkDoor.Text = "显示门打开 show door open";
      this.chkDoor.Click += new EventHandler(this.chkDoor_Click);
      this.chkDoorSign.Name = "chkDoorSign";
      this.chkDoorSign.Size = new Size(400, 26);
      this.chkDoorSign.Text = "显示门标记 show door tag";
      this.chkDoorSign.Click += new EventHandler(this.chkDoorSign_Click);
      this.chkFrontAnimationTag.Name = "chkFrontAnimationTag";
      this.chkFrontAnimationTag.Size = new Size(400, 26);
      this.chkFrontAnimationTag.Text = "显示前景动画标记 show front animation  tag";
      this.chkFrontAnimationTag.Click += new EventHandler(this.chkFrontAnimationTag_Click);
      this.chkMiddleAnimationTag.Name = "chkMiddleAnimationTag";
      this.chkMiddleAnimationTag.Size = new Size(400, 26);
      this.chkMiddleAnimationTag.Text = "显示中间动画标记 show middle animation  tag";
      this.chkMiddleAnimationTag.Click += new EventHandler(this.chkMiddleAnimationTag_Click);
      this.chkLightTag.Name = "chkLightTag";
      this.chkLightTag.Size = new Size(400, 26);
      this.chkLightTag.Text = "显示常亮点标记 show Light tag";
      this.chkLightTag.Click += new EventHandler(this.chkLightTag_Click);
      this.chkBackMask.Name = "chkBackMask";
      this.chkBackMask.Size = new Size(400, 26);
      this.chkBackMask.Text = "显示背景移动标记 show Back move limit tag";
      this.chkBackMask.Click += new EventHandler(this.chkBackMask_Click);
      this.chkFrontMask.Name = "chkFrontMask";
      this.chkFrontMask.Size = new Size(400, 26);
      this.chkFrontMask.Text = "显示前景移动标记 show front move limit tag";
      this.chkFrontMask.Click += new EventHandler(this.chkFrontMask_Click);
      this.chkDrawGrids.Name = "chkDrawGrids";
      this.chkDrawGrids.Size = new Size(400, 26);
      this.chkDrawGrids.Text = "显示网格 show Grids";
      this.chkDrawGrids.Click += new EventHandler(this.chkDrawGrids_Click);
      this.chkFrontTag.Name = "chkFrontTag";
      this.chkFrontTag.Size = new Size(400, 26);
      this.chkFrontTag.Text = "显示前景标记 show front tag";
      this.chkFrontTag.Click += new EventHandler(this.chkFrontTag_Click);
      this.chkMiddleTag.Name = "chkMiddleTag";
      this.chkMiddleTag.Size = new Size(400, 26);
      this.chkMiddleTag.Text = "显示中间标记 show middle tag";
      this.chkMiddleTag.Click += new EventHandler(this.chkMiddleTag_Click);
      this.toolStripLabel1.Font = new System.Drawing.Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.toolStripLabel1.Name = "toolStripLabel1";
      this.toolStripLabel1.Size = new Size(92, 56);
      this.toolStripLabel1.Text = "EditorLayer";
      this.cmbEditorLayer.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbEditorLayer.DropDownWidth = 210;
      this.cmbEditorLayer.FlatStyle = FlatStyle.Flat;
      this.cmbEditorLayer.Font = new System.Drawing.Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cmbEditorLayer.Items.AddRange(new object[20]
      {
        (object) "None",
        (object) "Back Image",
        (object) "Middle Image",
        (object) "Front Image",
        (object) "Back Limit",
        (object) "Front Limit",
        (object) "Back&Front Limit",
        (object) "Grasping Mir2 Front",
        (object) "Grasping Invert Mir3 Front&Middle",
        (object) "Place Objects",
        (object) "Clear All",
        (object) "Clear Back",
        (object) "Clear Midd",
        (object) "Clear Front",
        (object) "Clear Back&Front Limit",
        (object) "Clear Back Limit",
        (object) "Clear Front Limit",
        (object) "Brush Mir2 BigTiles",
        (object) "Brush SmTiles",
        (object) "Brush Mir3 BigTiles"
      });
      this.cmbEditorLayer.Name = "cmbEditorLayer";
      this.cmbEditorLayer.Size = new Size(185, 59);
      this.cmbEditorLayer.SelectedIndexChanged += new EventHandler(this.cmbEditorLayer_SelectedIndexChanged);
      this.btnMiniMap.Font = new System.Drawing.Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnMiniMap.Image = (Image) Resources.Camera;
      this.btnMiniMap.ImageScaling = ToolStripItemImageScaling.None;
      this.btnMiniMap.ImageTransparentColor = Color.Magenta;
      this.btnMiniMap.Name = "btnMiniMap";
      this.btnMiniMap.Size = new Size(71, 56);
      this.btnMiniMap.Text = "MiniMap";
      this.btnMiniMap.TextImageRelation = TextImageRelation.ImageAboveText;
      this.btnMiniMap.ToolTipText = "Create MiniMap";
      this.btnMiniMap.Click += new EventHandler(this.btnMiniMap_Click);
      this.tabControl1.Controls.Add((Control) this.tabWemadeMir2);
      this.tabControl1.Controls.Add((Control) this.tabShandaMir2);
      this.tabControl1.Controls.Add((Control) this.tabWemadeMir3);
      this.tabControl1.Controls.Add((Control) this.tabShandaMir3);
      this.tabControl1.Controls.Add((Control) this.tabObjects);
      this.tabControl1.Controls.Add((Control) this.tabTiles);
      this.tabControl1.Controls.Add((Control) this.tabMap);
      this.tabControl1.Controls.Add((Control) this.tabTileCutter);
      this.tabControl1.Dock = DockStyle.Fill;
      this.tabControl1.Font = new System.Drawing.Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tabControl1.Location = new Point(0, 87);
      this.tabControl1.Margin = new Padding(4, 5, 4, 5);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new Size(1982, 1068);
      this.tabControl1.TabIndex = 4;
      this.tabWemadeMir2.Controls.Add((Control) this.labWemadeMir2OffSetY);
      this.tabWemadeMir2.Controls.Add((Control) this.labeWemadeMir2OffSetX);
      this.tabWemadeMir2.Controls.Add((Control) this.LabWemadeMir2Height);
      this.tabWemadeMir2.Controls.Add((Control) this.LabWemadeMir2Width);
      this.tabWemadeMir2.Controls.Add((Control) this.picWemdeMir2);
      this.tabWemadeMir2.Controls.Add((Control) this.WemadeMir2LibListBox);
      this.tabWemadeMir2.Controls.Add((Control) this.WemadeMir2LibListView);
      this.tabWemadeMir2.Location = new Point(4, 29);
      this.tabWemadeMir2.Margin = new Padding(4, 5, 4, 5);
      this.tabWemadeMir2.Name = "tabWemadeMir2";
      this.tabWemadeMir2.Size = new Size(1974, 1035);
      this.tabWemadeMir2.TabIndex = 5;
      this.tabWemadeMir2.Text = "WemadeMir2";
      this.tabWemadeMir2.UseVisualStyleBackColor = true;
      this.labWemadeMir2OffSetY.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.labWemadeMir2OffSetY.AutoSize = true;
      this.labWemadeMir2OffSetY.Font = new System.Drawing.Font("Microsoft YaHei", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.labWemadeMir2OffSetY.Location = new Point(1740, 73);
      this.labWemadeMir2OffSetY.Margin = new Padding(4, 0, 4, 0);
      this.labWemadeMir2OffSetY.Name = "labWemadeMir2OffSetY";
      this.labWemadeMir2OffSetY.Size = new Size(75, 24);
      this.labWemadeMir2OffSetY.TabIndex = 15;
      this.labWemadeMir2OffSetY.Text = "OffSetY";
      this.labeWemadeMir2OffSetX.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.labeWemadeMir2OffSetX.AutoSize = true;
      this.labeWemadeMir2OffSetX.Font = new System.Drawing.Font("Microsoft YaHei", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.labeWemadeMir2OffSetX.Location = new Point(1740, 25);
      this.labeWemadeMir2OffSetX.Margin = new Padding(4, 0, 4, 0);
      this.labeWemadeMir2OffSetX.Name = "labeWemadeMir2OffSetX";
      this.labeWemadeMir2OffSetX.Size = new Size(76, 24);
      this.labeWemadeMir2OffSetX.TabIndex = 14;
      this.labeWemadeMir2OffSetX.Text = "OffSetX";
      this.LabWemadeMir2Height.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.LabWemadeMir2Height.AutoSize = true;
      this.LabWemadeMir2Height.Font = new System.Drawing.Font("Microsoft YaHei", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.LabWemadeMir2Height.Location = new Point(1625, 73);
      this.LabWemadeMir2Height.Margin = new Padding(4, 0, 4, 0);
      this.LabWemadeMir2Height.Name = "LabWemadeMir2Height";
      this.LabWemadeMir2Height.Size = new Size(69, 24);
      this.LabWemadeMir2Height.TabIndex = 13;
      this.LabWemadeMir2Height.Text = "Height";
      this.LabWemadeMir2Width.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.LabWemadeMir2Width.AutoSize = true;
      this.LabWemadeMir2Width.Font = new System.Drawing.Font("Microsoft YaHei", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.LabWemadeMir2Width.Location = new Point(1625, 25);
      this.LabWemadeMir2Width.Margin = new Padding(4, 0, 4, 0);
      this.LabWemadeMir2Width.Name = "LabWemadeMir2Width";
      this.LabWemadeMir2Width.Size = new Size(63, 24);
      this.LabWemadeMir2Width.TabIndex = 12;
      this.LabWemadeMir2Width.Text = "Width";
      this.picWemdeMir2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picWemdeMir2.BackColor = Color.Black;
      this.picWemdeMir2.Location = new Point(1630, 135);
      this.picWemdeMir2.Margin = new Padding(4, 5, 4, 5);
      this.picWemdeMir2.Name = "picWemdeMir2";
      this.picWemdeMir2.Size = new Size(100, 50);
      this.picWemdeMir2.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picWemdeMir2.TabIndex = 11;
      this.picWemdeMir2.TabStop = false;
      this.WemadeMir2LibListBox.Dock = DockStyle.Left;
      this.WemadeMir2LibListBox.Font = new System.Drawing.Font("Comic Sans MS", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.WemadeMir2LibListBox.FormattingEnabled = true;
      this.WemadeMir2LibListBox.ItemHeight = 24;
      this.WemadeMir2LibListBox.Location = new Point(0, 0);
      this.WemadeMir2LibListBox.Margin = new Padding(4, 5, 4, 5);
      this.WemadeMir2LibListBox.Name = "WemadeMir2LibListBox";
      this.WemadeMir2LibListBox.Size = new Size(200, 1035);
      this.WemadeMir2LibListBox.TabIndex = 10;
      this.WemadeMir2LibListBox.SelectedIndexChanged += new EventHandler(this.WemadeMir2LibListBox_SelectedIndexChanged);
      this.WemadeMir2LibListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.WemadeMir2LibListView.HideSelection = false;
      this.WemadeMir2LibListView.LargeImageList = this.WemadeMir2ImageList;
      this.WemadeMir2LibListView.Location = new Point(188, 5);
      this.WemadeMir2LibListView.Margin = new Padding(4, 5, 4, 5);
      this.WemadeMir2LibListView.Name = "WemadeMir2LibListView";
      this.WemadeMir2LibListView.Size = new Size(1331, 1036);
      this.WemadeMir2LibListView.TabIndex = 9;
      this.WemadeMir2LibListView.UseCompatibleStateImageBehavior = false;
      this.WemadeMir2LibListView.VirtualMode = true;
      this.WemadeMir2LibListView.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(this.WemadeMir2LibListView_RetrieveVirtualItem);
      this.WemadeMir2LibListView.SelectedIndexChanged += new EventHandler(this.WemadeMir2LibListView_SelectedIndexChanged);
      this.WemadeMir2LibListView.Click += new EventHandler(this.WemadeMir2LibListView_Click);
      this.WemadeMir2ImageList.ColorDepth = ColorDepth.Depth32Bit;
      this.WemadeMir2ImageList.ImageSize = new Size(64, 64);
      this.WemadeMir2ImageList.TransparentColor = Color.Transparent;
      this.tabShandaMir2.Controls.Add((Control) this.labshandaMir2OffSetY);
      this.tabShandaMir2.Controls.Add((Control) this.labShandaMir2OffSetX);
      this.tabShandaMir2.Controls.Add((Control) this.labShandaMir2Height);
      this.tabShandaMir2.Controls.Add((Control) this.labShandaMir2Width);
      this.tabShandaMir2.Controls.Add((Control) this.picShandaMir2);
      this.tabShandaMir2.Controls.Add((Control) this.ShandaMir2LibListBox);
      this.tabShandaMir2.Controls.Add((Control) this.ShandaMir2LibListView);
      this.tabShandaMir2.Location = new Point(4, 29);
      this.tabShandaMir2.Margin = new Padding(4, 5, 4, 5);
      this.tabShandaMir2.Name = "tabShandaMir2";
      this.tabShandaMir2.Size = new Size(1974, 1035);
      this.tabShandaMir2.TabIndex = 4;
      this.tabShandaMir2.Text = "ShandaMir2";
      this.tabShandaMir2.UseVisualStyleBackColor = true;
      this.labshandaMir2OffSetY.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.labshandaMir2OffSetY.AutoSize = true;
      this.labshandaMir2OffSetY.Font = new System.Drawing.Font("Microsoft YaHei", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.labshandaMir2OffSetY.Location = new Point(1732, 70);
      this.labshandaMir2OffSetY.Margin = new Padding(4, 0, 4, 0);
      this.labshandaMir2OffSetY.Name = "labshandaMir2OffSetY";
      this.labshandaMir2OffSetY.Size = new Size(75, 24);
      this.labshandaMir2OffSetY.TabIndex = 20;
      this.labshandaMir2OffSetY.Text = "OffSetY";
      this.labShandaMir2OffSetX.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.labShandaMir2OffSetX.AutoSize = true;
      this.labShandaMir2OffSetX.Font = new System.Drawing.Font("Microsoft YaHei", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.labShandaMir2OffSetX.Location = new Point(1732, 22);
      this.labShandaMir2OffSetX.Margin = new Padding(4, 0, 4, 0);
      this.labShandaMir2OffSetX.Name = "labShandaMir2OffSetX";
      this.labShandaMir2OffSetX.Size = new Size(76, 24);
      this.labShandaMir2OffSetX.TabIndex = 19;
      this.labShandaMir2OffSetX.Text = "OffSetX";
      this.labShandaMir2Height.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.labShandaMir2Height.AutoSize = true;
      this.labShandaMir2Height.Font = new System.Drawing.Font("Microsoft YaHei", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.labShandaMir2Height.Location = new Point(1617, 70);
      this.labShandaMir2Height.Margin = new Padding(4, 0, 4, 0);
      this.labShandaMir2Height.Name = "labShandaMir2Height";
      this.labShandaMir2Height.Size = new Size(69, 24);
      this.labShandaMir2Height.TabIndex = 18;
      this.labShandaMir2Height.Text = "Height";
      this.labShandaMir2Width.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.labShandaMir2Width.AutoSize = true;
      this.labShandaMir2Width.Font = new System.Drawing.Font("Microsoft YaHei", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.labShandaMir2Width.Location = new Point(1617, 22);
      this.labShandaMir2Width.Margin = new Padding(4, 0, 4, 0);
      this.labShandaMir2Width.Name = "labShandaMir2Width";
      this.labShandaMir2Width.Size = new Size(63, 24);
      this.labShandaMir2Width.TabIndex = 17;
      this.labShandaMir2Width.Text = "Width";
      this.picShandaMir2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picShandaMir2.BackColor = Color.Black;
      this.picShandaMir2.Location = new Point(1622, 125);
      this.picShandaMir2.Margin = new Padding(4, 5, 4, 5);
      this.picShandaMir2.Name = "picShandaMir2";
      this.picShandaMir2.Size = new Size(100, 50);
      this.picShandaMir2.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picShandaMir2.TabIndex = 16;
      this.picShandaMir2.TabStop = false;
      this.ShandaMir2LibListBox.Dock = DockStyle.Left;
      this.ShandaMir2LibListBox.Font = new System.Drawing.Font("Comic Sans MS", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ShandaMir2LibListBox.FormattingEnabled = true;
      this.ShandaMir2LibListBox.ItemHeight = 24;
      this.ShandaMir2LibListBox.Location = new Point(0, 0);
      this.ShandaMir2LibListBox.Margin = new Padding(4, 5, 4, 5);
      this.ShandaMir2LibListBox.Name = "ShandaMir2LibListBox";
      this.ShandaMir2LibListBox.Size = new Size(175, 1035);
      this.ShandaMir2LibListBox.TabIndex = 8;
      this.ShandaMir2LibListBox.SelectedIndexChanged += new EventHandler(this.ShandaMir2LibListBox_SelectedIndexChanged);
      this.ShandaMir2LibListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.ShandaMir2LibListView.HideSelection = false;
      this.ShandaMir2LibListView.LargeImageList = this.ShandaMir2ImageList;
      this.ShandaMir2LibListView.Location = new Point(176, 2);
      this.ShandaMir2LibListView.Margin = new Padding(4, 5, 4, 5);
      this.ShandaMir2LibListView.Name = "ShandaMir2LibListView";
      this.ShandaMir2LibListView.Size = new Size(1432, 1039);
      this.ShandaMir2LibListView.TabIndex = 7;
      this.ShandaMir2LibListView.UseCompatibleStateImageBehavior = false;
      this.ShandaMir2LibListView.VirtualMode = true;
      this.ShandaMir2LibListView.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(this.ShandaMir2LiblistView_RetrieveVirtualItem);
      this.ShandaMir2LibListView.SelectedIndexChanged += new EventHandler(this.ShandaMir2LibListView_SelectedIndexChanged);
      this.ShandaMir2LibListView.Click += new EventHandler(this.ShandaMir2LibListView_Click);
      this.ShandaMir2ImageList.ColorDepth = ColorDepth.Depth32Bit;
      this.ShandaMir2ImageList.ImageSize = new Size(64, 64);
      this.ShandaMir2ImageList.TransparentColor = Color.Transparent;
      this.tabWemadeMir3.Controls.Add((Control) this.labWemadeMir3OffSetY);
      this.tabWemadeMir3.Controls.Add((Control) this.labeWemadeMir3OffSetX);
      this.tabWemadeMir3.Controls.Add((Control) this.LabWemadeMir3Height);
      this.tabWemadeMir3.Controls.Add((Control) this.LabWemadeMir3Width);
      this.tabWemadeMir3.Controls.Add((Control) this.picWemdeMir3);
      this.tabWemadeMir3.Controls.Add((Control) this.WemadeMir3LibListBox);
      this.tabWemadeMir3.Controls.Add((Control) this.WemadeMir3LibListView);
      this.tabWemadeMir3.Location = new Point(4, 29);
      this.tabWemadeMir3.Margin = new Padding(4, 5, 4, 5);
      this.tabWemadeMir3.Name = "tabWemadeMir3";
      this.tabWemadeMir3.Size = new Size(1974, 1035);
      this.tabWemadeMir3.TabIndex = 6;
      this.tabWemadeMir3.Text = "WemadeMir3";
      this.tabWemadeMir3.UseVisualStyleBackColor = true;
      this.labWemadeMir3OffSetY.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.labWemadeMir3OffSetY.AutoSize = true;
      this.labWemadeMir3OffSetY.Font = new System.Drawing.Font("Microsoft YaHei", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.labWemadeMir3OffSetY.Location = new Point(1733, 76);
      this.labWemadeMir3OffSetY.Margin = new Padding(4, 0, 4, 0);
      this.labWemadeMir3OffSetY.Name = "labWemadeMir3OffSetY";
      this.labWemadeMir3OffSetY.Size = new Size(75, 24);
      this.labWemadeMir3OffSetY.TabIndex = 20;
      this.labWemadeMir3OffSetY.Text = "OffSetY";
      this.labeWemadeMir3OffSetX.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.labeWemadeMir3OffSetX.AutoSize = true;
      this.labeWemadeMir3OffSetX.Font = new System.Drawing.Font("Microsoft YaHei", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.labeWemadeMir3OffSetX.Location = new Point(1733, 29);
      this.labeWemadeMir3OffSetX.Margin = new Padding(4, 0, 4, 0);
      this.labeWemadeMir3OffSetX.Name = "labeWemadeMir3OffSetX";
      this.labeWemadeMir3OffSetX.Size = new Size(76, 24);
      this.labeWemadeMir3OffSetX.TabIndex = 19;
      this.labeWemadeMir3OffSetX.Text = "OffSetX";
      this.LabWemadeMir3Height.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.LabWemadeMir3Height.AutoSize = true;
      this.LabWemadeMir3Height.Font = new System.Drawing.Font("Microsoft YaHei", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.LabWemadeMir3Height.Location = new Point(1618, 76);
      this.LabWemadeMir3Height.Margin = new Padding(4, 0, 4, 0);
      this.LabWemadeMir3Height.Name = "LabWemadeMir3Height";
      this.LabWemadeMir3Height.Size = new Size(69, 24);
      this.LabWemadeMir3Height.TabIndex = 18;
      this.LabWemadeMir3Height.Text = "Height";
      this.LabWemadeMir3Width.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.LabWemadeMir3Width.AutoSize = true;
      this.LabWemadeMir3Width.Font = new System.Drawing.Font("Microsoft YaHei", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.LabWemadeMir3Width.Location = new Point(1618, 29);
      this.LabWemadeMir3Width.Margin = new Padding(4, 0, 4, 0);
      this.LabWemadeMir3Width.Name = "LabWemadeMir3Width";
      this.LabWemadeMir3Width.Size = new Size(63, 24);
      this.LabWemadeMir3Width.TabIndex = 17;
      this.LabWemadeMir3Width.Text = "Width";
      this.picWemdeMir3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picWemdeMir3.BackColor = Color.Black;
      this.picWemdeMir3.Location = new Point(1624, 131);
      this.picWemdeMir3.Margin = new Padding(4, 5, 4, 5);
      this.picWemdeMir3.Name = "picWemdeMir3";
      this.picWemdeMir3.Size = new Size(100, 50);
      this.picWemdeMir3.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picWemdeMir3.TabIndex = 16;
      this.picWemdeMir3.TabStop = false;
      this.WemadeMir3LibListBox.Dock = DockStyle.Left;
      this.WemadeMir3LibListBox.Font = new System.Drawing.Font("Comic Sans MS", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.WemadeMir3LibListBox.FormattingEnabled = true;
      this.WemadeMir3LibListBox.ItemHeight = 24;
      this.WemadeMir3LibListBox.Location = new Point(0, 0);
      this.WemadeMir3LibListBox.Margin = new Padding(4, 5, 4, 5);
      this.WemadeMir3LibListBox.Name = "WemadeMir3LibListBox";
      this.WemadeMir3LibListBox.Size = new Size(224, 1035);
      this.WemadeMir3LibListBox.TabIndex = 10;
      this.WemadeMir3LibListBox.SelectedIndexChanged += new EventHandler(this.WemadeMir3LibListBox_SelectedIndexChanged);
      this.WemadeMir3LibListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.WemadeMir3LibListView.HideSelection = false;
      this.WemadeMir3LibListView.LargeImageList = this.WemadeMir3ImageList;
      this.WemadeMir3LibListView.Location = new Point(224, 2);
      this.WemadeMir3LibListView.Margin = new Padding(4, 5, 4, 5);
      this.WemadeMir3LibListView.Name = "WemadeMir3LibListView";
      this.WemadeMir3LibListView.Size = new Size(1375, 1039);
      this.WemadeMir3LibListView.TabIndex = 9;
      this.WemadeMir3LibListView.UseCompatibleStateImageBehavior = false;
      this.WemadeMir3LibListView.VirtualMode = true;
      this.WemadeMir3LibListView.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(this.WemadeMir3LibListView_RetrieveVirtualItem);
      this.WemadeMir3LibListView.SelectedIndexChanged += new EventHandler(this.WemadeMir3LibListView_SelectedIndexChanged);
      this.WemadeMir3LibListView.Click += new EventHandler(this.WemadeMir3LibListView_Click);
      this.WemadeMir3ImageList.ColorDepth = ColorDepth.Depth32Bit;
      this.WemadeMir3ImageList.ImageSize = new Size(64, 64);
      this.WemadeMir3ImageList.TransparentColor = Color.Transparent;
      this.tabShandaMir3.Controls.Add((Control) this.labshandaMir3OffSetY);
      this.tabShandaMir3.Controls.Add((Control) this.labShandaMir3OffSetX);
      this.tabShandaMir3.Controls.Add((Control) this.labShandaMir3Height);
      this.tabShandaMir3.Controls.Add((Control) this.labShandaMir3Width);
      this.tabShandaMir3.Controls.Add((Control) this.picShandaMir3);
      this.tabShandaMir3.Controls.Add((Control) this.ShandaMir3LibListBox);
      this.tabShandaMir3.Controls.Add((Control) this.ShandaMir3LibListView);
      this.tabShandaMir3.Location = new Point(4, 29);
      this.tabShandaMir3.Margin = new Padding(4, 5, 4, 5);
      this.tabShandaMir3.Name = "tabShandaMir3";
      this.tabShandaMir3.Size = new Size(1974, 1035);
      this.tabShandaMir3.TabIndex = 7;
      this.tabShandaMir3.Text = "ShandaMir3";
      this.tabShandaMir3.UseVisualStyleBackColor = true;
      this.labshandaMir3OffSetY.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.labshandaMir3OffSetY.AutoSize = true;
      this.labshandaMir3OffSetY.Font = new System.Drawing.Font("Microsoft YaHei", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.labshandaMir3OffSetY.Location = new Point(1745, 73);
      this.labshandaMir3OffSetY.Margin = new Padding(4, 0, 4, 0);
      this.labshandaMir3OffSetY.Name = "labshandaMir3OffSetY";
      this.labshandaMir3OffSetY.Size = new Size(75, 24);
      this.labshandaMir3OffSetY.TabIndex = 20;
      this.labshandaMir3OffSetY.Text = "OffSetY";
      this.labShandaMir3OffSetX.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.labShandaMir3OffSetX.AutoSize = true;
      this.labShandaMir3OffSetX.Font = new System.Drawing.Font("Microsoft YaHei", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.labShandaMir3OffSetX.Location = new Point(1745, 25);
      this.labShandaMir3OffSetX.Margin = new Padding(4, 0, 4, 0);
      this.labShandaMir3OffSetX.Name = "labShandaMir3OffSetX";
      this.labShandaMir3OffSetX.Size = new Size(76, 24);
      this.labShandaMir3OffSetX.TabIndex = 19;
      this.labShandaMir3OffSetX.Text = "OffSetX";
      this.labShandaMir3Height.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.labShandaMir3Height.AutoSize = true;
      this.labShandaMir3Height.Font = new System.Drawing.Font("Microsoft YaHei", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.labShandaMir3Height.Location = new Point(1630, 73);
      this.labShandaMir3Height.Margin = new Padding(4, 0, 4, 0);
      this.labShandaMir3Height.Name = "labShandaMir3Height";
      this.labShandaMir3Height.Size = new Size(69, 24);
      this.labShandaMir3Height.TabIndex = 18;
      this.labShandaMir3Height.Text = "Height";
      this.labShandaMir3Width.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.labShandaMir3Width.AutoSize = true;
      this.labShandaMir3Width.Font = new System.Drawing.Font("Microsoft YaHei", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.labShandaMir3Width.Location = new Point(1630, 25);
      this.labShandaMir3Width.Margin = new Padding(4, 0, 4, 0);
      this.labShandaMir3Width.Name = "labShandaMir3Width";
      this.labShandaMir3Width.Size = new Size(63, 24);
      this.labShandaMir3Width.TabIndex = 17;
      this.labShandaMir3Width.Text = "Width";
      this.picShandaMir3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picShandaMir3.BackColor = Color.Black;
      this.picShandaMir3.Location = new Point(1636, 130);
      this.picShandaMir3.Margin = new Padding(4, 5, 4, 5);
      this.picShandaMir3.Name = "picShandaMir3";
      this.picShandaMir3.Size = new Size(100, 50);
      this.picShandaMir3.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picShandaMir3.TabIndex = 16;
      this.picShandaMir3.TabStop = false;
      this.ShandaMir3LibListBox.Dock = DockStyle.Left;
      this.ShandaMir3LibListBox.Font = new System.Drawing.Font("Comic Sans MS", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ShandaMir3LibListBox.FormattingEnabled = true;
      this.ShandaMir3LibListBox.ItemHeight = 24;
      this.ShandaMir3LibListBox.Location = new Point(0, 0);
      this.ShandaMir3LibListBox.Margin = new Padding(4, 5, 4, 5);
      this.ShandaMir3LibListBox.Name = "ShandaMir3LibListBox";
      this.ShandaMir3LibListBox.Size = new Size(220, 1035);
      this.ShandaMir3LibListBox.TabIndex = 10;
      this.ShandaMir3LibListBox.SelectedIndexChanged += new EventHandler(this.ShandaMir3LibListBox_SelectedIndexChanged);
      this.ShandaMir3LibListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.ShandaMir3LibListView.HideSelection = false;
      this.ShandaMir3LibListView.LargeImageList = this.ShandaMir3ImageList;
      this.ShandaMir3LibListView.Location = new Point(219, 0);
      this.ShandaMir3LibListView.Margin = new Padding(4, 5, 4, 5);
      this.ShandaMir3LibListView.Name = "ShandaMir3LibListView";
      this.ShandaMir3LibListView.Size = new Size(1377, 1041);
      this.ShandaMir3LibListView.TabIndex = 9;
      this.ShandaMir3LibListView.UseCompatibleStateImageBehavior = false;
      this.ShandaMir3LibListView.VirtualMode = true;
      this.ShandaMir3LibListView.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(this.ShandaMir3LibListView_RetrieveVirtualItem);
      this.ShandaMir3LibListView.SelectedIndexChanged += new EventHandler(this.ShandaMir3LibListView_SelectedIndexChanged);
      this.ShandaMir3LibListView.Click += new EventHandler(this.ShandaMir3LibListView_Click);
      this.ShandaMir3ImageList.ColorDepth = ColorDepth.Depth32Bit;
      this.ShandaMir3ImageList.ImageSize = new Size(64, 64);
      this.ShandaMir3ImageList.TransparentColor = Color.Transparent;
      this.tabObjects.Controls.Add((Control) this.splitContainer1);
      this.tabObjects.Location = new Point(4, 29);
      this.tabObjects.Margin = new Padding(4, 5, 4, 5);
      this.tabObjects.Name = "tabObjects";
      this.tabObjects.Size = new Size(1974, 1035);
      this.tabObjects.TabIndex = 8;
      this.tabObjects.Text = "Objects";
      this.tabObjects.UseVisualStyleBackColor = true;
      this.splitContainer1.Dock = DockStyle.Fill;
      this.splitContainer1.Location = new Point(0, 0);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.ObjectslistBox);
      this.splitContainer1.Panel2.Controls.Add((Control) this.splitContainer2);
      this.splitContainer1.Size = new Size(1974, 1035);
      this.splitContainer1.SplitterDistance = 200;
      this.splitContainer1.TabIndex = 8;
      this.ObjectslistBox.Dock = DockStyle.Fill;
      this.ObjectslistBox.Font = new System.Drawing.Font("Comic Sans MS", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ObjectslistBox.FormattingEnabled = true;
      this.ObjectslistBox.ItemHeight = 24;
      this.ObjectslistBox.Location = new Point(0, 0);
      this.ObjectslistBox.Margin = new Padding(4, 5, 4, 5);
      this.ObjectslistBox.Name = "ObjectslistBox";
      this.ObjectslistBox.Size = new Size(200, 1035);
      this.ObjectslistBox.TabIndex = 6;
      this.ObjectslistBox.SelectedIndexChanged += new EventHandler(this.ObjectslistBox_SelectedIndexChanged);
      this.splitContainer2.Dock = DockStyle.Fill;
      this.splitContainer2.Location = new Point(0, 0);
      this.splitContainer2.Name = "splitContainer2";
      this.splitContainer2.Orientation = Orientation.Horizontal;
      this.splitContainer2.Panel1.Controls.Add((Control) this.btnRefreshList);
      this.splitContainer2.Panel1.Controls.Add((Control) this.btnDeleteObjects);
      this.splitContainer2.Panel2.Controls.Add((Control) this.picObjects);
      this.splitContainer2.Size = new Size(1770, 1035);
      this.splitContainer2.SplitterDistance = 46;
      this.splitContainer2.TabIndex = 3;
      this.btnRefreshList.Location = new Point(111, 5);
      this.btnRefreshList.Name = "btnRefreshList";
      this.btnRefreshList.Size = new Size(100, 36);
      this.btnRefreshList.TabIndex = 3;
      this.btnRefreshList.Text = "Refresh";
      this.btnRefreshList.UseVisualStyleBackColor = true;
      this.btnRefreshList.Click += new EventHandler(this.btnRefreshList_Click);
      this.btnDeleteObjects.Location = new Point(4, 5);
      this.btnDeleteObjects.Margin = new Padding(4, 5, 4, 5);
      this.btnDeleteObjects.Name = "btnDeleteObjects";
      this.btnDeleteObjects.Size = new Size(100, 36);
      this.btnDeleteObjects.TabIndex = 2;
      this.btnDeleteObjects.Text = "Delete";
      this.btnDeleteObjects.UseVisualStyleBackColor = true;
      this.btnDeleteObjects.Click += new EventHandler(this.btnDeleteObjects_Click);
      this.picObjects.BackColor = Color.Transparent;
      this.picObjects.BorderStyle = BorderStyle.FixedSingle;
      this.picObjects.Dock = DockStyle.Fill;
      this.picObjects.Location = new Point(0, 0);
      this.picObjects.Margin = new Padding(4, 5, 4, 5);
      this.picObjects.Name = "picObjects";
      this.picObjects.Size = new Size(1770, 985);
      this.picObjects.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picObjects.TabIndex = 0;
      this.picObjects.TabStop = false;
      this.tabTiles.Controls.Add((Control) this.picTile);
      this.tabTiles.Controls.Add((Control) this.TileslistView);
      this.tabTiles.Location = new Point(4, 29);
      this.tabTiles.Margin = new Padding(4, 5, 4, 5);
      this.tabTiles.Name = "tabTiles";
      this.tabTiles.Size = new Size(1974, 1035);
      this.tabTiles.TabIndex = 9;
      this.tabTiles.Text = "Tiles";
      this.tabTiles.UseVisualStyleBackColor = true;
      this.picTile.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.picTile.BackColor = Color.Black;
      this.picTile.BorderStyle = BorderStyle.FixedSingle;
      this.picTile.Location = new Point(1563, 24);
      this.picTile.Margin = new Padding(4, 5, 4, 5);
      this.picTile.Name = "picTile";
      this.picTile.Size = new Size(889, 684);
      this.picTile.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picTile.TabIndex = 11;
      this.picTile.TabStop = false;
      this.TileslistView.Dock = DockStyle.Left;
      this.TileslistView.HideSelection = false;
      this.TileslistView.LargeImageList = this.TilesImageList;
      this.TileslistView.Location = new Point(0, 0);
      this.TileslistView.Margin = new Padding(4, 5, 4, 5);
      this.TileslistView.Name = "TileslistView";
      this.TileslistView.Size = new Size(1539, 1035);
      this.TileslistView.TabIndex = 10;
      this.TileslistView.UseCompatibleStateImageBehavior = false;
      this.TileslistView.VirtualMode = true;
      this.TileslistView.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(this.TileslistView_RetrieveVirtualItem);
      this.TileslistView.SelectedIndexChanged += new EventHandler(this.TileslistView_SelectedIndexChanged);
      this.TilesImageList.ColorDepth = ColorDepth.Depth32Bit;
      this.TilesImageList.ImageSize = new Size(144, 96);
      this.TilesImageList.TransparentColor = Color.Transparent;
      this.tabMap.BackColor = Color.Black;
      this.tabMap.Controls.Add((Control) this.tableLayoutPanel1);
      this.tabMap.Location = new Point(4, 29);
      this.tabMap.Margin = new Padding(4, 5, 4, 5);
      this.tabMap.Name = "tabMap";
      this.tabMap.Size = new Size(1974, 1035);
      this.tabMap.TabIndex = 2;
      this.tabMap.Text = "Map";
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
      this.tableLayoutPanel1.Controls.Add((Control) this.vScrollBar, 1, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.MapPanel, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.hScrollBar, 0, 1);
      this.tableLayoutPanel1.Dock = DockStyle.Fill;
      this.tableLayoutPanel1.Location = new Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 2;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
      this.tableLayoutPanel1.Size = new Size(1974, 1035);
      this.tableLayoutPanel1.TabIndex = 10;
      this.vScrollBar.Dock = DockStyle.Fill;
      this.vScrollBar.LargeChange = 1;
      this.vScrollBar.Location = new Point(1954, 0);
      this.vScrollBar.Name = "vScrollBar";
      this.vScrollBar.Size = new Size(20, 1015);
      this.vScrollBar.TabIndex = 1;
      this.vScrollBar.Scroll += new ScrollEventHandler(this.vScrollBar_Scroll);
      this.MapPanel.AutoScroll = true;
      this.MapPanel.BackColor = Color.Transparent;
      this.MapPanel.ContextMenuStrip = this.contextMenuStrip1;
      this.MapPanel.Dock = DockStyle.Fill;
      this.MapPanel.Location = new Point(4, 5);
      this.MapPanel.Margin = new Padding(4, 5, 4, 5);
      this.MapPanel.Name = "MapPanel";
      this.MapPanel.Size = new Size(1946, 1005);
      this.MapPanel.TabIndex = 9;
      this.MapPanel.MouseClick += new MouseEventHandler(this.MapPanel_MouseClick);
      this.MapPanel.MouseDown += new MouseEventHandler(this.MapPanel_MouseDown);
      this.MapPanel.MouseMove += new MouseEventHandler(this.MapPanel_MouseMove);
      this.MapPanel.MouseUp += new MouseEventHandler(this.MapPanel_MouseUp);
      this.contextMenuStrip1.ImageScalingSize = new Size(20, 20);
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[8]
      {
        (ToolStripItem) this.撤销ToolStripMenuItem,
        (ToolStripItem) this.返回ToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator2,
        (ToolStripItem) this.menu_SaveObject,
        (ToolStripItem) this.btnSetDoor,
        (ToolStripItem) this.btnSetAnimation,
        (ToolStripItem) this.btnSetLight,
        (ToolStripItem) this.menu_DeleteSelectedCellData
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new Size(556, 178);
      this.撤销ToolStripMenuItem.Name = "撤销ToolStripMenuItem";
      this.撤销ToolStripMenuItem.Size = new Size(555, 24);
      this.撤销ToolStripMenuItem.Text = "撤销 undo";
      this.撤销ToolStripMenuItem.Click += new EventHandler(this.撤销ToolStripMenuItem_Click);
      this.返回ToolStripMenuItem.Name = "返回ToolStripMenuItem";
      this.返回ToolStripMenuItem.Size = new Size(555, 24);
      this.返回ToolStripMenuItem.Text = "返回 return";
      this.返回ToolStripMenuItem.Click += new EventHandler(this.返回ToolStripMenuItem_Click);
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new Size(552, 6);
      this.menu_SaveObject.Name = "menu_SaveObject";
      this.menu_SaveObject.Size = new Size(555, 24);
      this.menu_SaveObject.Text = "Save Selection As Object (.X)";
      this.menu_SaveObject.Click += new EventHandler(this.menu_SaveObject_Click);
      this.btnSetDoor.Name = "btnSetDoor";
      this.btnSetDoor.Size = new Size(555, 24);
      this.btnSetDoor.Text = "设置当前坐标门属性 Set the current coordinates door properties";
      this.btnSetDoor.Click += new EventHandler(this.btnSetDoor_Click);
      this.btnSetAnimation.Name = "btnSetAnimation";
      this.btnSetAnimation.Size = new Size(555, 24);
      this.btnSetAnimation.Text = "设置当前坐标动画属性 Set the current coordinates animation attributes ";
      this.btnSetAnimation.Click += new EventHandler(this.btnSetAnimation_Click);
      this.btnSetLight.Name = "btnSetLight";
      this.btnSetLight.Size = new Size(555, 24);
      this.btnSetLight.Text = "设置当前坐标亮度属性 Set the current coordinates brightness property ";
      this.btnSetLight.Click += new EventHandler(this.btnSetLight_Click);
      this.menu_DeleteSelectedCellData.Name = "menu_DeleteSelectedCellData";
      this.menu_DeleteSelectedCellData.Size = new Size(555, 24);
      this.menu_DeleteSelectedCellData.Text = "Delete Selected Cell Data";
      this.menu_DeleteSelectedCellData.Click += new EventHandler(this.menu_DeleteSelectedCellData_Click);
      this.hScrollBar.Dock = DockStyle.Fill;
      this.hScrollBar.LargeChange = 1;
      this.hScrollBar.Location = new Point(0, 1015);
      this.hScrollBar.Name = "hScrollBar";
      this.hScrollBar.Size = new Size(1954, 20);
      this.hScrollBar.TabIndex = 0;
      this.hScrollBar.Scroll += new ScrollEventHandler(this.hScrollBar_Scroll);
      this.tabTileCutter.Controls.Add((Control) this.splitContainer3);
      this.tabTileCutter.Location = new Point(4, 29);
      this.tabTileCutter.Name = "tabTileCutter";
      this.tabTileCutter.Padding = new Padding(3);
      this.tabTileCutter.Size = new Size(1974, 1035);
      this.tabTileCutter.TabIndex = 10;
      this.tabTileCutter.Text = "TileCutter";
      this.tabTileCutter.UseVisualStyleBackColor = true;
      this.splitContainer3.Dock = DockStyle.Fill;
      this.splitContainer3.Location = new Point(3, 3);
      this.splitContainer3.Name = "splitContainer3";
      this.splitContainer3.Panel1.Controls.Add((Control) this.btn_grid);
      this.splitContainer3.Panel1.Controls.Add((Control) this.btn_vCut);
      this.splitContainer3.Panel1.Controls.Add((Control) this.btn_load);
      this.splitContainer3.Panel1.Controls.Add((Control) this.label1);
      this.splitContainer3.Panel1.Controls.Add((Control) this.btn_up);
      this.splitContainer3.Panel1.Controls.Add((Control) this.comboBox_cellSize);
      this.splitContainer3.Panel1.Controls.Add((Control) this.btn_left);
      this.splitContainer3.Panel1.Controls.Add((Control) this.btn_down);
      this.splitContainer3.Panel1.Controls.Add((Control) this.btn_right);
      this.splitContainer3.Panel1MinSize = 170;
      this.splitContainer3.Panel2.AutoScroll = true;
      this.splitContainer3.Panel2.Controls.Add((Control) this.pictureBox_Highlight);
      this.splitContainer3.Panel2.Controls.Add((Control) this.pictureBox_Grid);
      this.splitContainer3.Panel2.Controls.Add((Control) this.pictureBox_Image);
      this.splitContainer3.Panel2MinSize = 50;
      this.splitContainer3.Size = new Size(1968, 1029);
      this.splitContainer3.SplitterDistance = 175;
      this.splitContainer3.TabIndex = 0;
      this.btn_grid.Location = new Point(125, 19);
      this.btn_grid.Margin = new Padding(0);
      this.btn_grid.Name = "btn_grid";
      this.btn_grid.Size = new Size(27, 25);
      this.btn_grid.TabIndex = 17;
      this.btn_grid.Text = "#";
      this.btn_grid.UseVisualStyleBackColor = true;
      this.btn_grid.Click += new EventHandler(this.btn_grid_Click);
      this.btn_vCut.Location = new Point(21, 212);
      this.btn_vCut.Margin = new Padding(4);
      this.btn_vCut.Name = "btn_vCut";
      this.btn_vCut.Size = new Size(100, 28);
      this.btn_vCut.TabIndex = 16;
      this.btn_vCut.Text = "Save Lib";
      this.btn_vCut.UseVisualStyleBackColor = true;
      this.btn_vCut.Click += new EventHandler(this.btn_vCut_Click);
      this.btn_load.Location = new Point(21, 17);
      this.btn_load.Margin = new Padding(4);
      this.btn_load.Name = "btn_load";
      this.btn_load.Size = new Size(100, 28);
      this.btn_load.TabIndex = 9;
      this.btn_load.Text = "Load Image";
      this.btn_load.UseVisualStyleBackColor = true;
      this.btn_load.Click += new EventHandler(this.btn_load_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(21, 141);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(69, 20);
      this.label1.TabIndex = 15;
      this.label1.Text = "Tile Size";
      this.btn_up.Location = new Point(58, 54);
      this.btn_up.Margin = new Padding(0);
      this.btn_up.Name = "btn_up";
      this.btn_up.Size = new Size(27, 25);
      this.btn_up.TabIndex = 10;
      this.btn_up.UseVisualStyleBackColor = true;
      this.btn_up.Click += new EventHandler(this.btn_up_Click);
      this.comboBox_cellSize.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox_cellSize.FormattingEnabled = true;
      this.comboBox_cellSize.Items.AddRange(new object[2]
      {
        (object) "48 x 32",
        (object) "96 x 64"
      });
      this.comboBox_cellSize.Location = new Point(21, 161);
      this.comboBox_cellSize.Margin = new Padding(4);
      this.comboBox_cellSize.Name = "comboBox_cellSize";
      this.comboBox_cellSize.Size = new Size(99, 28);
      this.comboBox_cellSize.TabIndex = 14;
      this.comboBox_cellSize.SelectedIndexChanged += new EventHandler(this.comboBox_cellSize_SelectedIndexChanged);
      this.btn_left.Location = new Point(32, 78);
      this.btn_left.Margin = new Padding(0);
      this.btn_left.Name = "btn_left";
      this.btn_left.Size = new Size(27, 25);
      this.btn_left.TabIndex = 11;
      this.btn_left.UseVisualStyleBackColor = true;
      this.btn_left.Click += new EventHandler(this.btn_left_Click);
      this.btn_down.Location = new Point(58, 103);
      this.btn_down.Margin = new Padding(0);
      this.btn_down.Name = "btn_down";
      this.btn_down.Size = new Size(27, 25);
      this.btn_down.TabIndex = 13;
      this.btn_down.UseVisualStyleBackColor = true;
      this.btn_down.Click += new EventHandler(this.btn_down_Click);
      this.btn_right.Location = new Point(85, 78);
      this.btn_right.Margin = new Padding(0);
      this.btn_right.Name = "btn_right";
      this.btn_right.Size = new Size(27, 25);
      this.btn_right.TabIndex = 12;
      this.btn_right.UseVisualStyleBackColor = true;
      this.btn_right.Click += new EventHandler(this.btn_right_Click);
      this.pictureBox_Highlight.BackColor = Color.Transparent;
      this.pictureBox_Highlight.ContextMenuStrip = this.contextMenuTileCutter;
      this.pictureBox_Highlight.Location = new Point(0, 0);
      this.pictureBox_Highlight.Margin = new Padding(4);
      this.pictureBox_Highlight.Name = "pictureBox_Highlight";
      this.pictureBox_Highlight.Size = new Size(1500, 900);
      this.pictureBox_Highlight.TabIndex = 5;
      this.pictureBox_Highlight.TabStop = false;
      this.pictureBox_Highlight.Click += new EventHandler(this.pictureBox_Highlight_Click);
      this.pictureBox_Grid.BackColor = Color.Transparent;
      this.pictureBox_Grid.Location = new Point(0, 0);
      this.pictureBox_Grid.Margin = new Padding(0);
      this.pictureBox_Grid.Name = "pictureBox_Grid";
      this.pictureBox_Grid.Size = new Size(1550, 950);
      this.pictureBox_Grid.TabIndex = 4;
      this.pictureBox_Grid.TabStop = false;
      this.pictureBox_Image.Location = new Point(0, 0);
      this.pictureBox_Image.Margin = new Padding(4);
      this.pictureBox_Image.Name = "pictureBox_Image";
      this.pictureBox_Image.Size = new Size(1600, 1000);
      this.pictureBox_Image.TabIndex = 3;
      this.pictureBox_Image.TabStop = false;
      this.ObjectsimageList.ColorDepth = ColorDepth.Depth32Bit;
      this.ObjectsimageList.ImageSize = new Size(96, 96);
      this.ObjectsimageList.TransparentColor = Color.Transparent;
      this.menuStrip1.ImageScalingSize = new Size(20, 20);
      this.menuStrip1.Items.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.toolStripMenuItem1,
        (ToolStripItem) this.toolStripMenuItem2,
        (ToolStripItem) this.toolStripMenuItem9,
        (ToolStripItem) this.toolsToolStripMenuItem,
        (ToolStripItem) this.helpToolStripMenuItem
      });
      this.menuStrip1.Location = new Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new Size(1982, 28);
      this.menuStrip1.TabIndex = 5;
      this.menuStrip1.Text = "menuStrip1";
      this.toolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.menuNew,
        (ToolStripItem) this.menuOpen,
        (ToolStripItem) this.menuSave
      });
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new Size(44, 24);
      this.toolStripMenuItem1.Text = "File";
      this.menuNew.Name = "menuNew";
      this.menuNew.Size = new Size(120, 26);
      this.menuNew.Text = "New";
      this.menuNew.Click += new EventHandler(this.menuNew_Click);
      this.menuOpen.Name = "menuOpen";
      this.menuOpen.Size = new Size(120, 26);
      this.menuOpen.Text = "Open";
      this.menuOpen.Click += new EventHandler(this.menuOpen_Click);
      this.menuSave.Name = "menuSave";
      this.menuSave.Size = new Size(120, 26);
      this.menuSave.Text = "Save";
      this.menuSave.Click += new EventHandler(this.menuSave_Click);
      this.toolStripMenuItem2.DropDownItems.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.menuUndo,
        (ToolStripItem) this.menuRedo,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.menuClearMap
      });
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      this.toolStripMenuItem2.Size = new Size(47, 24);
      this.toolStripMenuItem2.Text = "Edit";
      this.menuUndo.Name = "menuUndo";
      this.menuUndo.Size = new Size(152, 26);
      this.menuUndo.Text = "Undo";
      this.menuUndo.Click += new EventHandler(this.menuUndo_Click);
      this.menuRedo.Name = "menuRedo";
      this.menuRedo.Size = new Size(152, 26);
      this.menuRedo.Text = "Redo";
      this.menuRedo.Click += new EventHandler(this.menuRedo_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(149, 6);
      this.menuClearMap.Name = "menuClearMap";
      this.menuClearMap.Size = new Size(152, 26);
      this.menuClearMap.Text = "Clear Map";
      this.menuClearMap.Click += new EventHandler(this.menuClearMap_Click);
      this.toolStripMenuItem9.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.menuZoomIn,
        (ToolStripItem) this.menuZoomOut
      });
      this.toolStripMenuItem9.Name = "toolStripMenuItem9";
      this.toolStripMenuItem9.Size = new Size(53, 24);
      this.toolStripMenuItem9.Text = "View";
      this.menuZoomIn.Name = "menuZoomIn";
      this.menuZoomIn.Size = new Size(172, 26);
      this.menuZoomIn.Text = "Zoom In (+)";
      this.menuZoomIn.Click += new EventHandler(this.menuZoomIn_Click);
      this.menuZoomOut.Name = "menuZoomOut";
      this.menuZoomOut.Size = new Size(172, 26);
      this.menuZoomOut.Text = "Zoom Out (-)";
      this.menuZoomOut.Click += new EventHandler(this.menuZoomOut_Click);
      this.toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.menuFreeMemory,
        (ToolStripItem) this.menuJump,
        (ToolStripItem) this.menuInvertMir3Layer
      });
      this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
      this.toolsToolStripMenuItem.Size = new Size(56, 24);
      this.toolsToolStripMenuItem.Text = "Tools";
      this.menuFreeMemory.Name = "menuFreeMemory";
      this.menuFreeMemory.Size = new Size(216, 26);
      this.menuFreeMemory.Text = "Free Memory";
      this.menuFreeMemory.Click += new EventHandler(this.menuFreeMemory_Click);
      this.menuJump.Name = "menuJump";
      this.menuJump.Size = new Size(216, 26);
      this.menuJump.Text = "Jump";
      this.menuJump.Click += new EventHandler(this.menuJump_Click);
      this.menuInvertMir3Layer.Name = "menuInvertMir3Layer";
      this.menuInvertMir3Layer.Size = new Size(216, 26);
      this.menuInvertMir3Layer.Text = "Invert Mir 3 Layer";
      this.menuInvertMir3Layer.Click += new EventHandler(this.menuInvertMir3Layer_Click);
      this.helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.menuAbout
      });
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.Size = new Size(53, 24);
      this.helpToolStripMenuItem.Text = "Help";
      this.menuAbout.Name = "menuAbout";
      this.menuAbout.Size = new Size(125, 26);
      this.menuAbout.Text = "About";
      this.menuAbout.Click += new EventHandler(this.menuAbout_Click);
      this.SaveLibraryDialog.Filter = "Lib|*.lib";
      this.contextMenuTileCutter.ImageScalingSize = new Size(20, 20);
      this.contextMenuTileCutter.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.menuSelectAllCells,
        (ToolStripItem) this.menuDeselectAllCells
      });
      this.contextMenuTileCutter.Name = "contextMenuStrip1";
      this.contextMenuTileCutter.Size = new Size(211, 80);
      this.menuSelectAllCells.Name = "menuSelectAllCells";
      this.menuSelectAllCells.Size = new Size(210, 24);
      this.menuSelectAllCells.Text = "SelectAll";
      this.menuSelectAllCells.Click += new EventHandler(this.menuSelectAllCells_Click);
      this.menuDeselectAllCells.Name = "menuDeselectAllCells";
      this.menuDeselectAllCells.Size = new Size(210, 24);
      this.menuDeselectAllCells.Text = "DeselectAll";
      this.menuDeselectAllCells.Click += new EventHandler(this.menuDeselectAllCells_Click);
      this.AutoScaleDimensions = new SizeF(10f, 24f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1982, 1155);
      this.Controls.Add((Control) this.tabControl1);
      this.Controls.Add((Control) this.toolStrip1);
      this.Controls.Add((Control) this.menuStrip1);
      this.Font = new System.Drawing.Font("Comic Sans MS", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.KeyPreview = true;
      this.MainMenuStrip = this.menuStrip1;
      this.Margin = new Padding(4, 5, 4, 5);
      this.Name = nameof (Main);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Map Editor";
      this.FormClosing += new FormClosingEventHandler(this.Main_FormClosing);
      this.Load += new EventHandler(this.Main_Load);
      this.ResizeEnd += new EventHandler(this.Main_ResizeEnd);
      this.KeyDown += new KeyEventHandler(this.Main_KeyDown);
      this.KeyUp += new KeyEventHandler(this.Main_KeyUp);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.tabControl1.ResumeLayout(false);
      this.tabWemadeMir2.ResumeLayout(false);
      this.tabWemadeMir2.PerformLayout();
      ((ISupportInitialize) this.picWemdeMir2).EndInit();
      this.tabShandaMir2.ResumeLayout(false);
      this.tabShandaMir2.PerformLayout();
      ((ISupportInitialize) this.picShandaMir2).EndInit();
      this.tabWemadeMir3.ResumeLayout(false);
      this.tabWemadeMir3.PerformLayout();
      ((ISupportInitialize) this.picWemdeMir3).EndInit();
      this.tabShandaMir3.ResumeLayout(false);
      this.tabShandaMir3.PerformLayout();
      ((ISupportInitialize) this.picShandaMir3).EndInit();
      this.tabObjects.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.splitContainer2.Panel1.ResumeLayout(false);
      this.splitContainer2.Panel2.ResumeLayout(false);
      this.splitContainer2.Panel2.PerformLayout();
      this.splitContainer2.EndInit();
      this.splitContainer2.ResumeLayout(false);
      ((ISupportInitialize) this.picObjects).EndInit();
      this.tabTiles.ResumeLayout(false);
      this.tabTiles.PerformLayout();
      ((ISupportInitialize) this.picTile).EndInit();
      this.tabMap.ResumeLayout(false);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.contextMenuStrip1.ResumeLayout(false);
      this.tabTileCutter.ResumeLayout(false);
      this.splitContainer3.Panel1.ResumeLayout(false);
      this.splitContainer3.Panel1.PerformLayout();
      this.splitContainer3.Panel2.ResumeLayout(false);
      this.splitContainer3.EndInit();
      this.splitContainer3.ResumeLayout(false);
      ((ISupportInitialize) this.pictureBox_Highlight).EndInit();
      ((ISupportInitialize) this.pictureBox_Grid).EndInit();
      ((ISupportInitialize) this.pictureBox_Image).EndInit();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.contextMenuTileCutter.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public delegate void DelJump(int x, int y);

    public delegate void DelSetAnimationProperty(bool blend, byte frame, byte tick);

    public delegate void DelSetDoorProperty(bool door, byte index, byte offSet);

    public delegate void DelSetLightProperty(byte light);

    public delegate void DelSetMapSize(int w, int h);

    private enum Layer
    {
      None,
      BackImage,
      MiddleImage,
      FrontImage,
      BackLimit,
      FrontLimit,
      BackFrontLimit,
      GraspingMir2Front,
      GraspingInvertMir3FrontMiddle,
      PlaceObjects,
      ClearAll,
      ClearBack,
      ClearMidd,
      ClearFront,
      ClearBackFrontLimit,
      ClearBackLimit,
      ClearFrontLimit,
      BrushMir2BigTiles,
      BrushSmTiles,
      BrushMir3BigTiles,
    }

    private enum MirVerSion : byte
    {
      None,
      WemadeMir2,
      ShandaMir2,
      WemadeMir3,
      ShandaMir3,
    }

    private enum TileType
    {
      None = -1, // 0xFFFFFFFF
      Center = 0,
      Up = 1,
      UpRight = 2,
      Right = 3,
      DownRight = 4,
      Down = 5,
      DownLeft = 6,
      Left = 7,
      UpLeft = 8,
      InUpRight = 9,
      InDownRight = 10, // 0x0000000A
      InDownLeft = 11, // 0x0000000B
      InUpLeft = 12, // 0x0000000C
    }

    private struct PeekMsg
    {
      private readonly IntPtr hWnd;
      private readonly Message msg;
      private readonly IntPtr wParam;
      private readonly IntPtr lParam;
      private readonly uint time;
      private readonly Point p;
    }
  }
}
