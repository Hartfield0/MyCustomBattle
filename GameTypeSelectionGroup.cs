// Decompiled with JetBrains decompiler
// Type: TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.GameTypeSelectionGroup
// Assembly: TaleWorlds.MountAndBlade.CustomBattle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BE5DA056-FF60-485A-AA32-12EF43DB1898
// Assembly location: G:\steam\steamapps\common\Mount & Blade II Bannerlord\Modules\CustomBattle\bin\Win64_Shipping_Client\TaleWorlds.MountAndBlade.CustomBattle.dll

using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle
{
  public class GameTypeSelectionGroup : ViewModel
  {
    private MapSelectionGroup _mapSelectionGroup;
    private readonly Action<bool> _onPlayerTypeChange;
    private SelectorVM<SelectorItemVM> _gameTypeSelection;
    private SelectorVM<SelectorItemVM> _playerTypeSelection;
    private SelectorVM<SelectorItemVM> _playerSideSelection;

    public MapSelectionElement SelectedMap { get; private set; }

    public List<MapSelectionElement> ElementList { get; private set; }

    public GameTypeSelectionGroup(
      string name,
      MapSelectionGroup mapSelectionGroup,
      Action<bool> onPlayerTypeChange)
    {
      this._mapSelectionGroup = mapSelectionGroup;
      this._onPlayerTypeChange = onPlayerTypeChange;
      this.GameTypeSelection = new SelectorVM<SelectorItemVM>((IEnumerable<string>) new List<string>()
      {
        "Battle",
        "Village",
        "Siege"
      }, 0, new Action<SelectorVM<SelectorItemVM>>(this.OnGameTypeSelection));
      this.PlayerTypeSelection = new SelectorVM<SelectorItemVM>((IEnumerable<string>) new List<string>()
      {
        "Commander",
        "Sergeant"
      }, 0, new Action<SelectorVM<SelectorItemVM>>(this.OnPlayerTypeSelectionChange));
      this.PlayerSideSelection = new SelectorVM<SelectorItemVM>((IEnumerable<string>) new List<string>()
      {
        "Attacker",
        "Defender"
      }, 0, (Action<SelectorVM<SelectorItemVM>>) null);
    }

    public void RandomizeAll()
    {
      this.GameTypeSelection.ExecuteRandomize();
      this.PlayerTypeSelection.ExecuteRandomize();
      this.PlayerSideSelection.ExecuteRandomize();
    }

    private void OnGameTypeSelection(SelectorVM<SelectorItemVM> selector)
    {
      int num = selector.get_ItemList()[selector.get_SelectedIndex()].get_StringItem().IndexOf("siege", StringComparison.OrdinalIgnoreCase) >= 0 ? 1 : 0;
      bool flag = selector.get_ItemList()[selector.get_SelectedIndex()].get_StringItem().IndexOf("vill", StringComparison.OrdinalIgnoreCase) >= 0;
      this._mapSelectionGroup.SetSearchMode(num == 0 ? (!flag ? MapSelectionGroup.SearchMode.Battle : MapSelectionGroup.SearchMode.Village) : MapSelectionGroup.SearchMode.Siege);
    }

    private void OnPlayerTypeSelectionChange(SelectorVM<SelectorItemVM> obj)
    {
      this._onPlayerTypeChange(obj.get_SelectedIndex() == 0);
    }

    public BattleSideEnum GetCurrentPlayerSide()
    {
      return this.PlayerSideSelection.get_SelectedIndex() != 1 ? BattleSideEnum.Attacker : BattleSideEnum.Defender;
    }

    public GameTypeSelectionGroup.PlayerType GetCurrentPlayerType()
    {
      return this.PlayerTypeSelection.get_SelectedIndex() != 0 ? GameTypeSelectionGroup.PlayerType.Sergeant : GameTypeSelectionGroup.PlayerType.Commander;
    }

    public GameTypeSelectionGroup.GameType GetCurrentGameType()
    {
      return this.GameTypeSelection.get_SelectedIndex() != 0 ? GameTypeSelectionGroup.GameType.Siege : GameTypeSelectionGroup.GameType.Battle;
    }

    [DataSourceProperty]
    public SelectorVM<SelectorItemVM> GameTypeSelection
    {
      get
      {
        return this._gameTypeSelection;
      }
      set
      {
        if (value == this._gameTypeSelection)
          return;
        this._gameTypeSelection = value;
        this.OnPropertyChanged(nameof (GameTypeSelection));
      }
    }

    [DataSourceProperty]
    public SelectorVM<SelectorItemVM> PlayerTypeSelection
    {
      get
      {
        return this._playerTypeSelection;
      }
      set
      {
        if (value == this._playerTypeSelection)
          return;
        this._playerTypeSelection = value;
        this.OnPropertyChanged(nameof (PlayerTypeSelection));
      }
    }

    [DataSourceProperty]
    public SelectorVM<SelectorItemVM> PlayerSideSelection
    {
      get
      {
        return this._playerSideSelection;
      }
      set
      {
        if (value == this._playerSideSelection)
          return;
        this._playerSideSelection = value;
        this.OnPropertyChanged(nameof (PlayerSideSelection));
      }
    }

    public enum PlayerType
    {
      None,
      Commander,
      Sergeant,
    }

    public enum GameType
    {
      Battle,
      Village,
      Siege,
    }
  }
}
