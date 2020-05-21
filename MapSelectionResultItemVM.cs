// Decompiled with JetBrains decompiler
// Type: TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.MapSelection.MapSelectionResultItemVM
// Assembly: TaleWorlds.MountAndBlade.CustomBattle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BE5DA056-FF60-485A-AA32-12EF43DB1898
// Assembly location: G:\steam\steamapps\common\Mount & Blade II Bannerlord\Modules\CustomBattle\bin\Win64_Shipping_Client\TaleWorlds.MountAndBlade.CustomBattle.dll

using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.MapSelection
{
  public class MapSelectionResultItemVM : ViewModel
  {
    private string _searchedText;
    private Action<MapSelectionResultItemVM> _onSelect;
    public string _nameText;

    public string OrgNameText { get; private set; }

    public MapSelectionElement Source { get; private set; }

    public MapSelectionResultItemVM(
      MapSelectionElement source,
      string searchedText,
      Action<MapSelectionResultItemVM> onSelection)
    {
      this.Source = source;
      this.OrgNameText = source.MapName;
      this._nameText = source.MapName;
      this._onSelect = onSelection;
      this.UpdateSearchedText(searchedText);
    }

    public void UpdateSearchedText(string searchedText)
    {
      this._searchedText = searchedText;
      string oldValue = this.OrgNameText.Substring(this.OrgNameText.ToLower().IndexOf(this._searchedText.ToLower()), this._searchedText.Length);
      if (string.IsNullOrEmpty(oldValue))
        return;
      this.NameText = this.OrgNameText.Replace(oldValue, "<a>" + oldValue + "</a>");
    }

    public void ExecuteSelection()
    {
      this._onSelect(this);
    }

    [DataSourceProperty]
    public string NameText
    {
      get
      {
        return this._nameText;
      }
      set
      {
        if (!(this._nameText != value))
          return;
        this._nameText = value;
        this.OnPropertyChanged(nameof (NameText));
      }
    }
  }
}
