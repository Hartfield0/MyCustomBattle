// Decompiled with JetBrains decompiler
// Type: TaleWorlds.MountAndBlade.CustomBattle.SelectionGroup
// Assembly: TaleWorlds.MountAndBlade.CustomBattle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BE5DA056-FF60-485A-AA32-12EF43DB1898
// Assembly location: G:\steam\steamapps\common\Mount & Blade II Bannerlord\Modules\CustomBattle\bin\Win64_Shipping_Client\TaleWorlds.MountAndBlade.CustomBattle.dll

using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.CustomBattle
{
  public class SelectionGroup : ViewModel
  {
    protected List<string> _textList = new List<string>();
    private int _index;
    private string _name;
    private string _text;

    public SelectionGroup(string name, List<string> textList = null)
    {
      this._name = name;
      if (textList != null)
        this._textList = textList;
      this.Text = this._textList.Count > 0 ? this._textList[0] : "";
    }

    protected virtual void ClickSelectionLeft()
    {
      --this._index;
      if (this._index < 0)
        this._index = this._textList.Count - 1;
      this.Text = this._textList.Count > 0 ? this._textList[this._index] : "";
    }

    protected virtual void ClickSelectionRight()
    {
      ++this._index;
      this._index %= this._textList.Count;
      this.Text = this._textList.Count > 0 ? this._textList[this._index] : "";
    }

    public string Text
    {
      get
      {
        return this._text;
      }
      set
      {
        if (!(value != this._text))
          return;
        this._text = value;
        this.OnPropertyChanged(nameof (Text));
      }
    }

    public List<string> TextList
    {
      get
      {
        return this._textList;
      }
      set
      {
        if (value == this._textList)
          return;
        this._textList = value;
        this.Text = this._textList.Count > 0 ? this._textList[this._index] : "";
      }
    }

    public int Index
    {
      get
      {
        return this._index;
      }
      private set
      {
        value = this._index;
      }
    }
  }
}
