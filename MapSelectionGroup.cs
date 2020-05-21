// Decompiled with JetBrains decompiler
// Type: TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.MapSelectionGroup
// Assembly: TaleWorlds.MountAndBlade.CustomBattle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BE5DA056-FF60-485A-AA32-12EF43DB1898
// Assembly location: G:\steam\steamapps\common\Mount & Blade II Bannerlord\Modules\CustomBattle\bin\Win64_Shipping_Client\TaleWorlds.MountAndBlade.CustomBattle.dll

using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.MapSelection;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle
{
  public class MapSelectionGroup : ViewModel
  {
    private MapSelectionGroup.SearchMode _currentSearchMode;
    private readonly MapSelectionGroup.MapComparer _mapComparer;
    private bool _isCurrentMapSiege;
    private bool _isSallyOutSelected;
    private string _searchText;
    private SelectorVM<SelectorItemVM> _sceneLevelSelection;
    private SelectorVM<SelectorItemVM> _wallHitpointSelection;
    private SelectorVM<SelectorItemVM> _seasonSelection;
    private MBBindingList<MapSelectionResultItemVM> _mapSearchResults;

    public MapSelectionElement SelectedMap { get; private set; }

    public List<MapSelectionElement> ElementList { get; private set; }

    private IEnumerable<MapSelectionElement> _currentAllRelevantMaps
    {
      get
      {
        if (this._currentSearchMode == MapSelectionGroup.SearchMode.Battle)
          return this.ElementList.Where<MapSelectionElement>((Func<MapSelectionElement, bool>) (m => !m.IsSiegeMap && !m.IsVillageMap));
        if (this._currentSearchMode == MapSelectionGroup.SearchMode.Siege)
          return this.ElementList.Where<MapSelectionElement>((Func<MapSelectionElement, bool>) (m => m.IsSiegeMap && !m.IsVillageMap));
        return this._currentSearchMode == MapSelectionGroup.SearchMode.Village ? this.ElementList.Where<MapSelectionElement>((Func<MapSelectionElement, bool>) (m => !m.IsSiegeMap && m.IsVillageMap)) : (IEnumerable<MapSelectionElement>) this.ElementList;
      }
    }

    public MapSelectionGroup(string name, List<MapSelectionElement> elementList = null)
    {
      this.MapSearchResults = new MBBindingList<MapSelectionResultItemVM>();
      this._mapComparer = new MapSelectionGroup.MapComparer();
      this.ElementList = elementList;
      this.IsCurrentMapSiege = false;
      this.WallHitpointSelection = new SelectorVM<SelectorItemVM>((IEnumerable<string>) new List<string>()
      {
        "0",
        "50",
        "100"
      }, 0, (Action<SelectorVM<SelectorItemVM>>) null);
      this.SceneLevelSelection = new SelectorVM<SelectorItemVM>((IEnumerable<string>) new List<string>()
      {
        "1",
        "2",
        "3"
      }, 0, (Action<SelectorVM<SelectorItemVM>>) null);
      this.SeasonSelection = new SelectorVM<SelectorItemVM>((IEnumerable<string>) new List<string>()
      {
        "Summer",
        "Fall",
        "Winter",
        "Spring"
      }, 0, (Action<SelectorVM<SelectorItemVM>>) null);
      this.SetSearchMode(MapSelectionGroup.SearchMode.Battle);
    }

    public void ExecuteSallyOutChange()
    {
      this.IsSallyOutSelected = !this.IsSallyOutSelected;
    }

    public void SetSearchMode(MapSelectionGroup.SearchMode mode)
    {
      if (this._currentSearchMode == mode)
        return;
      this.MapSearchResults.Clear();
      this._currentSearchMode = mode;
      foreach (MapSelectionElement currentAllRelevantMap in this._currentAllRelevantMaps)
        this.MapSearchResults.Add(new MapSelectionResultItemVM(currentAllRelevantMap, "", new Action<MapSelectionResultItemVM>(this.OnMapSelection)));
      this.IsCurrentMapSiege = mode == MapSelectionGroup.SearchMode.Siege;
      this.SearchText = "";
      this.SelectedMap = (MapSelectionElement) null;
      this.MapSearchResults.Sort((IComparer<MapSelectionResultItemVM>) this._mapComparer);
    }

    public void ExecuteSelectRandomMap()
    {
      MBBindingList<MapSelectionResultItemVM> mapSearchResults = this.MapSearchResults;
      // ISSUE: explicit non-virtual call
      if ((mapSearchResults != null ? (__nonvirtual (mapSearchResults.Count) > 0 ? 1 : 0) : 0) == 0)
        return;
      this.SearchText = "";
      this.MapSearchResults[MBRandom.RandomInt(this.MapSearchResults.Count)].ExecuteSelection();
    }

    public void RandomizeAll()
    {
      this.ExecuteSelectRandomMap();
      this.SceneLevelSelection.ExecuteRandomize();
      this.SeasonSelection.ExecuteRandomize();
      this.WallHitpointSelection.ExecuteRandomize();
    }

    private void RefreshSearch(bool isAppending)
    {
      if (!isAppending)
      {
        this.MapSearchResults.Clear();
        foreach (MapSelectionElement currentAllRelevantMap in this._currentAllRelevantMaps)
        {
          MapSelectionElement map = currentAllRelevantMap;
          if (map.MapName.IndexOf(this._searchText, StringComparison.OrdinalIgnoreCase) >= 0 && !this.MapSearchResults.Any<MapSelectionResultItemVM>((Func<MapSelectionResultItemVM, bool>) (m => m.OrgNameText == map.MapName)))
            this.MapSearchResults.Add(new MapSelectionResultItemVM(map, this.SearchText, new Action<MapSelectionResultItemVM>(this.OnMapSelection)));
        }
      }
      else if (isAppending)
      {
        foreach (MapSelectionResultItemVM selectionResultItemVm in this.MapSearchResults.ToList<MapSelectionResultItemVM>())
        {
          if (selectionResultItemVm.OrgNameText.IndexOf(this._searchText, StringComparison.OrdinalIgnoreCase) < 0)
            this.MapSearchResults.Remove(selectionResultItemVm);
          else
            selectionResultItemVm.UpdateSearchedText(this._searchText);
        }
      }
      this.MapSearchResults.Sort((IComparer<MapSelectionResultItemVM>) this._mapComparer);
    }

    private void OnMapSelection(MapSelectionResultItemVM item)
    {
      this.SearchText = item.OrgNameText;
      this.SelectedMap = item.Source;
    }

    public MapSelectionElement GetMapWithName(string mapId)
    {
      return this.ElementList.Find((Predicate<MapSelectionElement>) (m => m.MapName == mapId));
    }

    [DataSourceProperty]
    public MBBindingList<MapSelectionResultItemVM> MapSearchResults
    {
      get
      {
        return this._mapSearchResults;
      }
      set
      {
        if (value == this._mapSearchResults)
          return;
        this._mapSearchResults = value;
        this.OnPropertyChanged(nameof (MapSearchResults));
      }
    }

    [DataSourceProperty]
    public SelectorVM<SelectorItemVM> SceneLevelSelection
    {
      get
      {
        return this._sceneLevelSelection;
      }
      set
      {
        if (value == this._sceneLevelSelection)
          return;
        this._sceneLevelSelection = value;
        this.OnPropertyChanged(nameof (SceneLevelSelection));
      }
    }

    [DataSourceProperty]
    public SelectorVM<SelectorItemVM> WallHitpointSelection
    {
      get
      {
        return this._wallHitpointSelection;
      }
      set
      {
        if (value == this._wallHitpointSelection)
          return;
        this._wallHitpointSelection = value;
        this.OnPropertyChanged(nameof (WallHitpointSelection));
      }
    }

    [DataSourceProperty]
    public SelectorVM<SelectorItemVM> SeasonSelection
    {
      get
      {
        return this._seasonSelection;
      }
      set
      {
        if (value == this._seasonSelection)
          return;
        this._seasonSelection = value;
        this.OnPropertyChanged(nameof (SeasonSelection));
      }
    }

    [DataSourceProperty]
    public bool IsCurrentMapSiege
    {
      get
      {
        return this._isCurrentMapSiege;
      }
      set
      {
        if (value == this._isCurrentMapSiege)
          return;
        this._isCurrentMapSiege = value;
        this.OnPropertyChanged(nameof (IsCurrentMapSiege));
      }
    }

    [DataSourceProperty]
    public bool IsSallyOutSelected
    {
      get
      {
        return this._isSallyOutSelected;
      }
      set
      {
        if (value == this._isSallyOutSelected)
          return;
        this._isSallyOutSelected = value;
        this.OnPropertyChanged(nameof (IsSallyOutSelected));
      }
    }

    [DataSourceProperty]
    public string SearchText
    {
      get
      {
        return this._searchText;
      }
      set
      {
        if (!(value != this._searchText))
          return;
        bool isAppending = true;
        if (this._searchText != null && this._searchText != string.Empty)
          isAppending = value.ToLower().Contains(this._searchText);
        this._searchText = value.ToLower();
        this.RefreshSearch(isAppending);
        this.OnPropertyChanged(nameof (SearchText));
      }
    }

    public enum SearchMode
    {
      None,
      Battle,
      Village,
      Siege,
    }

    private class MapComparer : IComparer<MapSelectionResultItemVM>
    {
      public int Compare(MapSelectionResultItemVM x, MapSelectionResultItemVM y)
      {
        return -x.OrgNameText.CompareTo(y.OrgNameText);
      }
    }
  }
}
