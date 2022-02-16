namespace Map_Editor
{
  public class CellInfoData
  {
    public int X;
    public int Y;
    public CellInfo CellInfo;

    public CellInfoData()
    {
    }

    public CellInfoData(int x, int y, CellInfo cellInfo)
    {
      this.X = x;
      this.Y = y;
      this.CellInfo = new CellInfo()
      {
        FrontImage = cellInfo.FrontImage,
        BackImage = cellInfo.BackImage,
        BackIndex = cellInfo.BackIndex,
        DoorIndex = cellInfo.DoorIndex,
        DoorOffset = cellInfo.DoorOffset,
        FishingCell = cellInfo.FishingCell,
        FrontAnimationFrame = cellInfo.FrontAnimationFrame,
        FrontAnimationTick = cellInfo.FrontAnimationTick,
        FrontIndex = cellInfo.FrontIndex,
        Light = cellInfo.Light,
        MiddleAnimationFrame = cellInfo.MiddleAnimationFrame,
        MiddleAnimationTick = cellInfo.MiddleAnimationTick,
        MiddleImage = cellInfo.MiddleImage,
        MiddleIndex = cellInfo.MiddleIndex,
        TileAnimationFrames = cellInfo.TileAnimationFrames,
        TileAnimationImage = cellInfo.TileAnimationImage,
        TileAnimationOffset = cellInfo.TileAnimationOffset,
        Unknown = cellInfo.Unknown
      };
    }
  }
}
