using System.Collections.Generic;

namespace Map_Editor
{
  public class Editor
  {
    private Stack<CellInfoData[]> _unDo;
    private Stack<CellInfoData[]> _reDo;

    public Editor()
    {
      this._unDo = new Stack<CellInfoData[]>();
      this._reDo = new Stack<CellInfoData[]>();
    }

    public CellInfoData[] UnDo
    {
      get => this._unDo.Count > 0 ? this._unDo.Pop() : (CellInfoData[]) null;
      set => this._unDo.Push((CellInfoData[]) value.Clone());
    }

    public CellInfoData[] ReDo
    {
      get => this._reDo.Count > 0 ? this._reDo.Pop() : (CellInfoData[]) null;
      set => this._reDo.Push((CellInfoData[]) value.Clone());
    }

    public int UnDoCount() => this._unDo.Count;

    public int ReDoCount() => this._reDo.Count;

    public void UndoClear() => this._unDo.Clear();

    public void ReDoClear() => this._reDo.Clear();
  }
}
