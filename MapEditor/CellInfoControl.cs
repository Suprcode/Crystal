using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Map_Editor
{
  public class CellInfoControl : UserControl
  {
    private IContainer components = (IContainer) null;
    private Label label1;
    private Label LabX;
    private Label labY;
    private Label label4;
    private Label labBackImageIndex;
    private Label label6;
    private Label labMiddleImageIndex;
    private Label label8;
    private Label labFrontImageIndex;
    private Label label10;
    private Label labBVersion;
    private Label labBLibName;
    private Label labBLibIndex;
    private Label labMLibIndex;
    private Label labMLibName;
    private Label labMVersion;
    private Label labFLibIndex;
    private Label labFLibName;
    private Label labFVersion;
    private Label label2;
    private Label label3;
    private Label label5;
    private Label label7;
    private Label label9;
    private Label LabBackLimit;
    private Label labFrontLimit;
    private Label label12;
    private Label label11;
    private Label label13;
    private Label labFFrame;
    private Label labFTick;
    private Label label15;
    private Label label14;
    private Label labFBlend;
    private Label labMBlend;
    private Label label17;
    private Label labMTick;
    private Label label19;
    private Label labMFrame;
    private Label label21;
    private Label label22;
    private Label label16;
    private Label label18;
    private Label labDoorOffSet;
    private Label label20;
    private Label labDoorIndex;
    private Label label23;
    private Label labEntityDoor;
    private Label label24;
    private Label labLight;
    private Label label25;
    private Label labfishing;

    public CellInfoControl() => this.InitializeComponent();

    public void SetText(
      int x,
      int y,
      int backImageIndex,
      int middleImageIndex,
      int frontImageIndex,
      int bLibIndex,
      int mLibIndex,
      int fLibIndex,
      string bLibName,
      string mLibName,
      string fLibName,
      int backLimit,
      int frontLimit,
      byte fFrame,
      byte ftick,
      bool fblend,
      byte mFrame,
      byte mTick,
      bool mBlend,
      byte doorOffSet,
      byte doorIndex,
      bool entityDoor,
      byte light,
      bool fishing)
    {
      this.LabX.Text = x.ToString();
      this.labY.Text = y.ToString();
      this.labBackImageIndex.Text = backImageIndex.ToString();
      this.labMiddleImageIndex.Text = middleImageIndex.ToString();
      this.labFrontImageIndex.Text = frontImageIndex.ToString();
      if (bLibIndex >= 0 && bLibIndex <= 99)
        this.labBVersion.Text = "WemadeMir2";
      else if (bLibIndex >= 100 && bLibIndex <= 199)
        this.labBVersion.Text = "ShandaMir2";
      else if (bLibIndex >= 200 && bLibIndex <= 299)
        this.labBVersion.Text = "WemadeMir3";
      else if (bLibIndex >= 300 && bLibIndex <= 399)
        this.labBVersion.Text = "ShandaMir3";
      else
        this.labBVersion.Text = "";
      if (mLibIndex >= 0 && mLibIndex <= 99)
        this.labMVersion.Text = "WemadeMir2";
      else if (mLibIndex >= 100 && mLibIndex <= 199)
        this.labMVersion.Text = "ShandaMir2";
      else if (mLibIndex >= 200 && mLibIndex <= 299)
        this.labMVersion.Text = "WemadeMir3";
      else if (mLibIndex >= 300 && mLibIndex <= 399)
        this.labMVersion.Text = "ShandaMir3";
      else
        this.labMVersion.Text = "";
      if (fLibIndex >= 0 && fLibIndex <= 99)
        this.labFVersion.Text = "WemadeMir2";
      else if (fLibIndex >= 100 && fLibIndex <= 199)
        this.labFVersion.Text = "ShandaMir2";
      else if (fLibIndex >= 200 && fLibIndex <= 299)
        this.labFVersion.Text = "WemadeMir3";
      else if (fLibIndex >= 300 && fLibIndex <= 399)
        this.labFVersion.Text = "ShandaMir3";
      else
        this.labFVersion.Text = "";
      this.labBLibIndex.Text = bLibIndex.ToString();
      this.labMLibIndex.Text = mLibIndex.ToString();
      this.labFLibIndex.Text = fLibIndex.ToString();
      this.labBLibName.Text = bLibName;
      this.labMLibName.Text = mLibName;
      this.labFLibName.Text = fLibName;
      if ((uint) backLimit > 0U)
        this.LabBackLimit.Text = "True";
      else
        this.LabBackLimit.Text = "False";
      if ((uint) frontLimit > 0U)
        this.labFrontLimit.Text = "True";
      else
        this.labFrontLimit.Text = "False";
      if (fFrame > (byte) 0)
      {
        this.labFFrame.Text = fFrame.ToString();
        this.labFTick.Text = ftick.ToString();
        this.labFBlend.Text = fblend.ToString();
      }
      else
      {
        this.labFFrame.Text = string.Empty;
        this.labFTick.Text = string.Empty;
        this.labFBlend.Text = string.Empty;
      }
      if (mFrame > (byte) 0 && mFrame < byte.MaxValue)
      {
        this.labMFrame.Text = ((int) mFrame & 15).ToString();
        this.labMTick.Text = mTick.ToString();
        this.labMBlend.Text = Convert.ToBoolean((int) mFrame & 15).ToString();
      }
      else
      {
        this.labMFrame.Text = string.Empty;
        this.labMTick.Text = string.Empty;
        this.labMBlend.Text = string.Empty;
      }
      this.labDoorOffSet.Text = doorOffSet.ToString();
      this.labDoorIndex.Text = doorIndex.ToString();
      this.labEntityDoor.Text = entityDoor.ToString();
      this.labLight.Text = light.ToString();
      this.labfishing.Text = fishing.ToString();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.LabX = new Label();
      this.labY = new Label();
      this.label4 = new Label();
      this.labBackImageIndex = new Label();
      this.label6 = new Label();
      this.labMiddleImageIndex = new Label();
      this.label8 = new Label();
      this.labFrontImageIndex = new Label();
      this.label10 = new Label();
      this.labBVersion = new Label();
      this.labBLibName = new Label();
      this.labBLibIndex = new Label();
      this.labMLibIndex = new Label();
      this.labMLibName = new Label();
      this.labMVersion = new Label();
      this.labFLibIndex = new Label();
      this.labFLibName = new Label();
      this.labFVersion = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label5 = new Label();
      this.label7 = new Label();
      this.label9 = new Label();
      this.LabBackLimit = new Label();
      this.labFrontLimit = new Label();
      this.label12 = new Label();
      this.label11 = new Label();
      this.label13 = new Label();
      this.labFFrame = new Label();
      this.labFTick = new Label();
      this.label15 = new Label();
      this.label14 = new Label();
      this.labFBlend = new Label();
      this.labMBlend = new Label();
      this.label17 = new Label();
      this.labMTick = new Label();
      this.label19 = new Label();
      this.labMFrame = new Label();
      this.label21 = new Label();
      this.label22 = new Label();
      this.label16 = new Label();
      this.label18 = new Label();
      this.labDoorOffSet = new Label();
      this.label20 = new Label();
      this.labDoorIndex = new Label();
      this.label23 = new Label();
      this.labEntityDoor = new Label();
      this.label24 = new Label();
      this.labLight = new Label();
      this.label25 = new Label();
      this.labfishing = new Label();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(3, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(21, 17);
      this.label1.TabIndex = 0;
      this.label1.Text = "X:";
      this.LabX.AutoSize = true;
      this.LabX.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.LabX.Location = new Point(26, 10);
      this.LabX.Name = "LabX";
      this.LabX.Size = new Size(15, 17);
      this.LabX.TabIndex = 1;
      this.LabX.Text = "0";
      this.labY.AutoSize = true;
      this.labY.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labY.Location = new Point(82, 10);
      this.labY.Name = "labY";
      this.labY.Size = new Size(15, 17);
      this.labY.TabIndex = 3;
      this.labY.Text = "0";
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(59, 10);
      this.label4.Name = "label4";
      this.label4.Size = new Size(20, 17);
      this.label4.TabIndex = 2;
      this.label4.Text = "Y:";
      this.labBackImageIndex.AutoSize = true;
      this.labBackImageIndex.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labBackImageIndex.Location = new Point(82, 29);
      this.labBackImageIndex.Name = "labBackImageIndex";
      this.labBackImageIndex.Size = new Size(15, 17);
      this.labBackImageIndex.TabIndex = 5;
      this.labBackImageIndex.Text = "0";
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(3, 29);
      this.label6.Name = "label6";
      this.label6.Size = new Size(71, 17);
      this.label6.TabIndex = 4;
      this.label6.Text = "BackImage:";
      this.labMiddleImageIndex.AutoSize = true;
      this.labMiddleImageIndex.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labMiddleImageIndex.Location = new Point(82, 48);
      this.labMiddleImageIndex.Name = "labMiddleImageIndex";
      this.labMiddleImageIndex.Size = new Size(15, 17);
      this.labMiddleImageIndex.TabIndex = 7;
      this.labMiddleImageIndex.Text = "0";
      this.label8.AutoSize = true;
      this.label8.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label8.Location = new Point(3, 48);
      this.label8.Name = "label8";
      this.label8.Size = new Size(81, 17);
      this.label8.TabIndex = 6;
      this.label8.Text = "MiddleImage:";
      this.labFrontImageIndex.AutoSize = true;
      this.labFrontImageIndex.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labFrontImageIndex.Location = new Point(82, 67);
      this.labFrontImageIndex.Name = "labFrontImageIndex";
      this.labFrontImageIndex.Size = new Size(15, 17);
      this.labFrontImageIndex.TabIndex = 9;
      this.labFrontImageIndex.Text = "0";
      this.label10.AutoSize = true;
      this.label10.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label10.Location = new Point(3, 67);
      this.label10.Name = "label10";
      this.label10.Size = new Size(76, 17);
      this.label10.TabIndex = 8;
      this.label10.Text = "FrontImage:";
      this.labBVersion.AutoSize = true;
      this.labBVersion.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labBVersion.Location = new Point(120, 29);
      this.labBVersion.Name = "labBVersion";
      this.labBVersion.Size = new Size(58, 17);
      this.labBVersion.TabIndex = 10;
      this.labBVersion.Text = "BVersion";
      this.labBLibName.AutoSize = true;
      this.labBLibName.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labBLibName.Location = new Point(200, 29);
      this.labBLibName.Name = "labBLibName";
      this.labBLibName.Size = new Size(49, 17);
      this.labBLibName.TabIndex = 11;
      this.labBLibName.Text = "objects";
      this.labBLibIndex.AutoSize = true;
      this.labBLibIndex.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labBLibIndex.Location = new Point(288, 29);
      this.labBLibIndex.Name = "labBLibIndex";
      this.labBLibIndex.Size = new Size(29, 17);
      this.labBLibIndex.TabIndex = 12;
      this.labBLibIndex.Text = "399";
      this.labMLibIndex.AutoSize = true;
      this.labMLibIndex.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labMLibIndex.Location = new Point(288, 46);
      this.labMLibIndex.Name = "labMLibIndex";
      this.labMLibIndex.Size = new Size(29, 17);
      this.labMLibIndex.TabIndex = 15;
      this.labMLibIndex.Text = "399";
      this.labMLibName.AutoSize = true;
      this.labMLibName.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labMLibName.Location = new Point(200, 46);
      this.labMLibName.Name = "labMLibName";
      this.labMLibName.Size = new Size(49, 17);
      this.labMLibName.TabIndex = 14;
      this.labMLibName.Text = "objects";
      this.labMVersion.AutoSize = true;
      this.labMVersion.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labMVersion.Location = new Point(120, 46);
      this.labMVersion.Name = "labMVersion";
      this.labMVersion.Size = new Size(58, 17);
      this.labMVersion.TabIndex = 13;
      this.labMVersion.Text = "BVersion";
      this.labFLibIndex.AutoSize = true;
      this.labFLibIndex.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labFLibIndex.Location = new Point(288, 63);
      this.labFLibIndex.Name = "labFLibIndex";
      this.labFLibIndex.Size = new Size(29, 17);
      this.labFLibIndex.TabIndex = 18;
      this.labFLibIndex.Text = "399";
      this.labFLibName.AutoSize = true;
      this.labFLibName.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labFLibName.Location = new Point(200, 63);
      this.labFLibName.Name = "labFLibName";
      this.labFLibName.Size = new Size(49, 17);
      this.labFLibName.TabIndex = 17;
      this.labFLibName.Text = "objects";
      this.labFVersion.AutoSize = true;
      this.labFVersion.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labFVersion.Location = new Point(120, 63);
      this.labFVersion.Name = "labFVersion";
      this.labFVersion.Size = new Size(58, 17);
      this.labFVersion.TabIndex = 16;
      this.labFVersion.Text = "BVersion";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(124, 10);
      this.label2.Name = "label2";
      this.label2.Size = new Size(50, 17);
      this.label2.TabIndex = 19;
      this.label2.Text = "Version";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(198, 10);
      this.label3.Name = "label3";
      this.label3.Size = new Size(55, 17);
      this.label3.TabIndex = 20;
      this.label3.Text = "LibName";
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(262, 10);
      this.label5.Name = "label5";
      this.label5.Size = new Size(57, 17);
      this.label5.TabIndex = 21;
      this.label5.Text = "LibIndex";
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(3, 84);
      this.label7.Name = "label7";
      this.label7.Size = new Size(41, 17);
      this.label7.TabIndex = 22;
      this.label7.Text = "Limit:";
      this.label9.AutoSize = true;
      this.label9.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label9.Location = new Point(64, 84);
      this.label9.Name = "label9";
      this.label9.Size = new Size(34, 17);
      this.label9.TabIndex = 23;
      this.label9.Text = "Back";
      this.LabBackLimit.AutoSize = true;
      this.LabBackLimit.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.LabBackLimit.Location = new Point(100, 84);
      this.LabBackLimit.Name = "LabBackLimit";
      this.LabBackLimit.Size = new Size(40, 17);
      this.LabBackLimit.TabIndex = 24;
      this.LabBackLimit.Text = "Back=";
      this.labFrontLimit.AutoSize = true;
      this.labFrontLimit.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labFrontLimit.Location = new Point(253, 84);
      this.labFrontLimit.Name = "labFrontLimit";
      this.labFrontLimit.Size = new Size(40, 17);
      this.labFrontLimit.TabIndex = 26;
      this.labFrontLimit.Text = "Back=";
      this.label12.AutoSize = true;
      this.label12.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label12.Location = new Point(213, 84);
      this.label12.Name = "label12";
      this.label12.Size = new Size(39, 17);
      this.label12.TabIndex = 25;
      this.label12.Text = "Front";
      this.label11.AutoSize = true;
      this.label11.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label11.Location = new Point(3, 101);
      this.label11.Name = "label11";
      this.label11.Size = new Size(66, 17);
      this.label11.TabIndex = 27;
      this.label11.Text = "Animation:";
      this.label13.AutoSize = true;
      this.label13.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label13.Location = new Point(65, 101);
      this.label13.Name = "label13";
      this.label13.Size = new Size(56, 17);
      this.label13.TabIndex = 28;
      this.label13.Text = "F_Frame";
      this.labFFrame.AutoSize = true;
      this.labFFrame.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labFFrame.Location = new Point(120, 101);
      this.labFFrame.Name = "labFFrame";
      this.labFFrame.Size = new Size(20, 17);
      this.labFFrame.TabIndex = 29;
      this.labFFrame.Text = "10";
      this.labFTick.AutoSize = true;
      this.labFTick.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labFTick.Location = new Point(187, 101);
      this.labFTick.Name = "labFTick";
      this.labFTick.Size = new Size(13, 17);
      this.labFTick.TabIndex = 31;
      this.labFTick.Text = "1";
      this.label15.AutoSize = true;
      this.label15.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label15.Location = new Point(141, 101);
      this.label15.Name = "label15";
      this.label15.Size = new Size(47, 17);
      this.label15.TabIndex = 30;
      this.label15.Text = "F_Tick";
      this.label14.AutoSize = true;
      this.label14.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label14.Location = new Point(213, 101);
      this.label14.Name = "label14";
      this.label14.Size = new Size(53, 17);
      this.label14.TabIndex = 32;
      this.label14.Text = "F_Blend";
      this.labFBlend.AutoSize = true;
      this.labFBlend.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labFBlend.Location = new Point(264, 101);
      this.labFBlend.Name = "labFBlend";
      this.labFBlend.Size = new Size(32, 17);
      this.labFBlend.TabIndex = 33;
      this.labFBlend.Text = "true";
      this.labMBlend.AutoSize = true;
      this.labMBlend.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labMBlend.Location = new Point(264, 118);
      this.labMBlend.Name = "labMBlend";
      this.labMBlend.Size = new Size(32, 17);
      this.labMBlend.TabIndex = 40;
      this.labMBlend.Text = "true";
      this.label17.AutoSize = true;
      this.label17.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label17.Location = new Point(213, 118);
      this.label17.Name = "label17";
      this.label17.Size = new Size(56, 17);
      this.label17.TabIndex = 39;
      this.label17.Text = "M_Blend";
      this.labMTick.AutoSize = true;
      this.labMTick.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labMTick.Location = new Point(187, 118);
      this.labMTick.Name = "labMTick";
      this.labMTick.Size = new Size(13, 17);
      this.labMTick.TabIndex = 38;
      this.labMTick.Text = "1";
      this.label19.AutoSize = true;
      this.label19.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label19.Location = new Point(141, 118);
      this.label19.Name = "label19";
      this.label19.Size = new Size(50, 17);
      this.label19.TabIndex = 37;
      this.label19.Text = "M_Tick";
      this.labMFrame.AutoSize = true;
      this.labMFrame.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labMFrame.Location = new Point(120, 118);
      this.labMFrame.Name = "labMFrame";
      this.labMFrame.Size = new Size(20, 17);
      this.labMFrame.TabIndex = 36;
      this.labMFrame.Text = "10";
      this.label21.AutoSize = true;
      this.label21.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label21.Location = new Point(65, 118);
      this.label21.Name = "label21";
      this.label21.Size = new Size(59, 17);
      this.label21.TabIndex = 35;
      this.label21.Text = "M_Frame";
      this.label22.AutoSize = true;
      this.label22.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label22.Location = new Point(3, 118);
      this.label22.Name = "label22";
      this.label22.Size = new Size(66, 17);
      this.label22.TabIndex = 34;
      this.label22.Text = "Animation:";
      this.label16.AutoSize = true;
      this.label16.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label16.Location = new Point(3, 135);
      this.label16.Name = "label16";
      this.label16.Size = new Size(39, 17);
      this.label16.TabIndex = 41;
      this.label16.Text = "Door:";
      this.label18.AutoSize = true;
      this.label18.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label18.Location = new Point(65, 135);
      this.label18.Name = "label18";
      this.label18.Size = new Size(50, 17);
      this.label18.TabIndex = 42;
      this.label18.Text = "OffSet";
      this.labDoorOffSet.AutoSize = true;
      this.labDoorOffSet.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labDoorOffSet.Location = new Point(121, 135);
      this.labDoorOffSet.Name = "labDoorOffSet";
      this.labDoorOffSet.Size = new Size(15, 17);
      this.labDoorOffSet.TabIndex = 43;
      this.labDoorOffSet.Text = "5";
      this.label20.AutoSize = true;
      this.label20.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label20.Location = new Point(141, 135);
      this.label20.Name = "label20";
      this.label20.Size = new Size(40, 17);
      this.label20.TabIndex = 44;
      this.label20.Text = "Index";
      this.labDoorIndex.AutoSize = true;
      this.labDoorIndex.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labDoorIndex.Location = new Point(184, 135);
      this.labDoorIndex.Name = "labDoorIndex";
      this.labDoorIndex.Size = new Size(15, 17);
      this.labDoorIndex.TabIndex = 45;
      this.labDoorIndex.Text = "5";
      this.label23.AutoSize = true;
      this.label23.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label23.Location = new Point(213, 135);
      this.label23.Name = "label23";
      this.label23.Size = new Size(44, 17);
      this.label23.TabIndex = 46;
      this.label23.Text = "Entity";
      this.labEntityDoor.AutoSize = true;
      this.labEntityDoor.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labEntityDoor.Location = new Point(263, 135);
      this.labEntityDoor.Name = "labEntityDoor";
      this.labEntityDoor.Size = new Size(32, 17);
      this.labEntityDoor.TabIndex = 47;
      this.labEntityDoor.Text = "true";
      this.label24.AutoSize = true;
      this.label24.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label24.Location = new Point(3, 152);
      this.label24.Name = "label24";
      this.label24.Size = new Size(41, 17);
      this.label24.TabIndex = 48;
      this.label24.Text = "Light:";
      this.labLight.AutoSize = true;
      this.labLight.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labLight.Location = new Point(48, 152);
      this.labLight.Name = "labLight";
      this.labLight.Size = new Size(15, 17);
      this.labLight.TabIndex = 49;
      this.labLight.Text = "5";
      this.label25.AutoSize = true;
      this.label25.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label25.Location = new Point(124, 152);
      this.label25.Name = "label25";
      this.label25.Size = new Size(51, 17);
      this.label25.TabIndex = 50;
      this.label25.Text = "Fishing:";
      this.labfishing.AutoSize = true;
      this.labfishing.Font = new Font("Comic Sans MS", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labfishing.Location = new Point(181, 152);
      this.labfishing.Name = "labfishing";
      this.labfishing.Size = new Size(32, 17);
      this.labfishing.TabIndex = 51;
      this.labfishing.Text = "true";
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.Control;
      this.Controls.Add((Control) this.labfishing);
      this.Controls.Add((Control) this.label25);
      this.Controls.Add((Control) this.labLight);
      this.Controls.Add((Control) this.label24);
      this.Controls.Add((Control) this.labEntityDoor);
      this.Controls.Add((Control) this.label23);
      this.Controls.Add((Control) this.labDoorIndex);
      this.Controls.Add((Control) this.label20);
      this.Controls.Add((Control) this.labDoorOffSet);
      this.Controls.Add((Control) this.label18);
      this.Controls.Add((Control) this.label16);
      this.Controls.Add((Control) this.labMBlend);
      this.Controls.Add((Control) this.label17);
      this.Controls.Add((Control) this.labMTick);
      this.Controls.Add((Control) this.label19);
      this.Controls.Add((Control) this.labMFrame);
      this.Controls.Add((Control) this.label21);
      this.Controls.Add((Control) this.label22);
      this.Controls.Add((Control) this.labFBlend);
      this.Controls.Add((Control) this.label14);
      this.Controls.Add((Control) this.labFTick);
      this.Controls.Add((Control) this.label15);
      this.Controls.Add((Control) this.labFFrame);
      this.Controls.Add((Control) this.label13);
      this.Controls.Add((Control) this.label11);
      this.Controls.Add((Control) this.labFrontLimit);
      this.Controls.Add((Control) this.label12);
      this.Controls.Add((Control) this.LabBackLimit);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.labFLibIndex);
      this.Controls.Add((Control) this.labFLibName);
      this.Controls.Add((Control) this.labFVersion);
      this.Controls.Add((Control) this.labMLibIndex);
      this.Controls.Add((Control) this.labMLibName);
      this.Controls.Add((Control) this.labMVersion);
      this.Controls.Add((Control) this.labBLibIndex);
      this.Controls.Add((Control) this.labBLibName);
      this.Controls.Add((Control) this.labBVersion);
      this.Controls.Add((Control) this.labFrontImageIndex);
      this.Controls.Add((Control) this.label10);
      this.Controls.Add((Control) this.labMiddleImageIndex);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.labBackImageIndex);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.labY);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.LabX);
      this.Controls.Add((Control) this.label1);
      this.Name = nameof (CellInfoControl);
      this.Size = new Size(316, 175);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
