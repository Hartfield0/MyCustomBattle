// Decompiled with JetBrains decompiler
// Type: TaleWorlds.MountAndBlade.CustomBattle.CustomBattleMenuVM
// Assembly: TaleWorlds.MountAndBlade.CustomBattle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BE5DA056-FF60-485A-AA32-12EF43DB1898
// Assembly location: G:\steam\steamapps\common\Mount & Blade II Bannerlord\Modules\CustomBattle\bin\Win64_Shipping_Client\TaleWorlds.MountAndBlade.CustomBattle.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle;

namespace TaleWorlds.MountAndBlade.CustomBattle
{
  public class CustomBattleMenuVM : ViewModel
  {
    private List<BasicCultureObject> _factionList;
    private List<CustomBattleSceneData> _customBattleScenes;
    private List<BasicCharacterObject> _charactersList;
    private CustomBattleState _customBattleState;
    private CustomBattleMenuSideVM _enemySide;
    private CustomBattleMenuSideVM _playerSide;
    private bool _isAttackerCustomMachineSelectionEnabled;
    private bool _isDefenderCustomMachineSelectionEnabled;
    private GameTypeSelectionGroup _gameTypeSelectionGroup;
    private MapSelectionGroup _mapSelectionGroup;
    private string _randomizeButtonText;
    private string _backButtonText;
    private string _startButtonText;
    private string _titleText;
    private MBBindingList<CustomBattleSiegeMachineVM> _attackerMeleeMachines;
    private MBBindingList<CustomBattleSiegeMachineVM> _attackerRangedMachines;
    private MBBindingList<CustomBattleSiegeMachineVM> _defenderMachines;

    public CustomBattleMenuVM(CustomBattleState battleState)
    {
      this._customBattleState = battleState;
      this._factionList = new List<BasicCultureObject>()
      {
        Game.Current.ObjectManager.GetObject<BasicCultureObject>("empire"),
        Game.Current.ObjectManager.GetObject<BasicCultureObject>("sturgia"),
        Game.Current.ObjectManager.GetObject<BasicCultureObject>("aserai"),
        Game.Current.ObjectManager.GetObject<BasicCultureObject>("vlandia"),
        Game.Current.ObjectManager.GetObject<BasicCultureObject>("battania"),
        Game.Current.ObjectManager.GetObject<BasicCultureObject>("khuzait")
      };
      this._customBattleScenes = new List<CustomBattleSceneData>();
      this._charactersList = new List<BasicCharacterObject>()
      {
        Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_1"),
        Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_2"),
        Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_3"),
        Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_4"),
        Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_5"),
        Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_6"),
        Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_7")
      };
      this.EnemySide = new CustomBattleMenuSideVM(new TextObject("{=35IHscBa}ENEMY", (Dictionary<string, TextObject>) null), false);
      this.PlayerSide = new CustomBattleMenuSideVM(new TextObject("{=BC7n6qxk}PLAYER", (Dictionary<string, TextObject>) null), true);
      List<string> stringList = new List<string>();
      List<string> list = this._factionList.Select<BasicCultureObject, string>((Func<BasicCultureObject, string>) (f => f.Name.ToString())).ToList<string>();
      this.EnemySide.FactionSelectionGroup.Refresh((IEnumerable<string>) list, 0, new Action<SelectorVM<SelectorItemVM>>(this.OnEnemyFactionSelection));
      this.PlayerSide.FactionSelectionGroup.Refresh((IEnumerable<string>) list, 0, new Action<SelectorVM<SelectorItemVM>>(this.OnPlayerFactionSelection));
      this.IsAttackerCustomMachineSelectionEnabled = false;
      List<MapSelectionElement> elementList = new List<MapSelectionElement>();
      this._customBattleScenes = CustomGame.Current.CustomBattleScenes.ToList<CustomBattleSceneData>();
      foreach (CustomBattleSceneData customBattleScene in this._customBattleScenes)
        elementList.Add(new MapSelectionElement(customBattleScene.Name.ToString(), customBattleScene.IsSiegeMap, customBattleScene.IsVillageMap));
      this.MapSelectionGroup = new MapSelectionGroup(new TextObject("{=fXhblsky}MAP", (Dictionary<string, TextObject>) null).ToString(), elementList);
      this.PlayerSide.CharacterSelectionGroup.Refresh((IEnumerable<string>) this._charactersList.Select<BasicCharacterObject, string>((Func<BasicCharacterObject, string>) (p => p.Name.ToString())).ToList<string>(), 0, new Action<SelectorVM<SelectorItemVM>>(this.OnPlayerCharacterSelection));
      this.EnemySide.CharacterSelectionGroup.Refresh((IEnumerable<string>) this._charactersList.Select<BasicCharacterObject, string>((Func<BasicCharacterObject, string>) (p => p.Name.ToString())).ToList<string>(), 1, new Action<SelectorVM<SelectorItemVM>>(this.OnEnemyCharacterSelection));
      this.OnPlayerCharacterSelection(this.PlayerSide.CharacterSelectionGroup);
      this.OnEnemyCharacterSelection(this.EnemySide.CharacterSelectionGroup);
      this.AttackerMeleeMachines = new MBBindingList<CustomBattleSiegeMachineVM>();
      for (int index = 0; index < 3; ++index)
        this.AttackerMeleeMachines.Add(new CustomBattleSiegeMachineVM((SiegeEngineType) null, new Action<CustomBattleSiegeMachineVM>(this.OnMeleeMachineSelection)));
      this.AttackerRangedMachines = new MBBindingList<CustomBattleSiegeMachineVM>();
      for (int index = 0; index < 4; ++index)
        this.AttackerRangedMachines.Add(new CustomBattleSiegeMachineVM((SiegeEngineType) null, new Action<CustomBattleSiegeMachineVM>(this.OnAttackerRangedMachineSelection)));
      this.DefenderMachines = new MBBindingList<CustomBattleSiegeMachineVM>();
      for (int index = 0; index < 4; ++index)
        this.DefenderMachines.Add(new CustomBattleSiegeMachineVM((SiegeEngineType) null, new Action<CustomBattleSiegeMachineVM>(this.OnDefenderRangedMachineSelection)));
      this.GameTypeSelectionGroup = new GameTypeSelectionGroup("Game Type", this.MapSelectionGroup, new Action<bool>(this.OnPlayerTypeChange));
      this.RefreshValues();
    }

    internal void SetActiveState(bool isActive)
    {
      if (isActive)
      {
        this.EnemySide.CurrentSelectedCharacter = new CharacterViewModel((CharacterViewModel.StanceTypes) 1);
        this.EnemySide.CurrentSelectedCharacter.FillFrom(this._charactersList[this.EnemySide.CharacterSelectionGroup.get_SelectedIndex()], -1);
        this.PlayerSide.CurrentSelectedCharacter = new CharacterViewModel((CharacterViewModel.StanceTypes) 1);
        this.PlayerSide.CurrentSelectedCharacter.FillFrom(this._charactersList[this.PlayerSide.CharacterSelectionGroup.get_SelectedIndex()], -1);
      }
      else
      {
        this.EnemySide.CurrentSelectedCharacter = (CharacterViewModel) null;
        this.PlayerSide.CurrentSelectedCharacter = (CharacterViewModel) null;
      }
    }

    private void OnPlayerTypeChange(bool isCommander)
    {
      this.PlayerSide.OnPlayerTypeChange(isCommander);
    }

    public override void RefreshValues()
    {
      base.RefreshValues();
      this.RandomizeButtonText = GameTexts.FindText("str_randomize", (string) null).ToString();
      this.StartButtonText = GameTexts.FindText("str_start", (string) null).ToString();
      this.BackButtonText = GameTexts.FindText("str_back", (string) null).ToString();
      this.TitleText = GameTexts.FindText("str_custom_battle", (string) null).ToString();
      this.EnemySide.RefreshValues();
      this.PlayerSide.RefreshValues();
      this.AttackerMeleeMachines.ApplyActionOnAllItems((Action<CustomBattleSiegeMachineVM>) (x => x.RefreshValues()));
      this.AttackerRangedMachines.ApplyActionOnAllItems((Action<CustomBattleSiegeMachineVM>) (x => x.RefreshValues()));
      this.DefenderMachines.ApplyActionOnAllItems((Action<CustomBattleSiegeMachineVM>) (x => x.RefreshValues()));
      this.MapSelectionGroup.RefreshValues();
    }

    private void OnMeleeMachineSelection(CustomBattleSiegeMachineVM selectedSlot)
    {
      List<InquiryElement> inquiryElements = new List<InquiryElement>();
      inquiryElements.Add(new InquiryElement((object) null, "Empty", (ImageIdentifier) null));
      foreach (SiegeEngineType attackerMeleeMachine in this.GetAllAttackerMeleeMachines())
        inquiryElements.Add(new InquiryElement((object) attackerMeleeMachine, attackerMeleeMachine.Name.ToString(), (ImageIdentifier) null));
      InformationManager.ShowMultiSelectionInquiry(new MultiSelectionInquiryData(new TextObject("{=MVOWsP48}Select a Melee Machine", (Dictionary<string, TextObject>) null).ToString(), string.Empty, inquiryElements, false, true, GameTexts.FindText("str_done", (string) null).ToString(), "", (Action<List<InquiryElement>>) (selectedElements => selectedSlot.SetMachineType(selectedElements.First<InquiryElement>().Identifier as SiegeEngineType)), (Action<List<InquiryElement>>) null, ""), false);
    }

    private void OnAttackerRangedMachineSelection(CustomBattleSiegeMachineVM selectedSlot)
    {
      List<InquiryElement> inquiryElements = new List<InquiryElement>();
      inquiryElements.Add(new InquiryElement((object) null, "Empty", (ImageIdentifier) null));
      foreach (SiegeEngineType attackerRangedMachine in this.GetAllAttackerRangedMachines())
        inquiryElements.Add(new InquiryElement((object) attackerRangedMachine, attackerRangedMachine.Name.ToString(), (ImageIdentifier) null));
      InformationManager.ShowMultiSelectionInquiry(new MultiSelectionInquiryData(new TextObject("{=SLZzfNPr}Select a Ranged Machine", (Dictionary<string, TextObject>) null).ToString(), string.Empty, inquiryElements, false, true, GameTexts.FindText("str_done", (string) null).ToString(), "", (Action<List<InquiryElement>>) (selectedElements => selectedSlot.SetMachineType(selectedElements[0].Identifier as SiegeEngineType)), (Action<List<InquiryElement>>) null, ""), false);
    }

    private void OnDefenderRangedMachineSelection(CustomBattleSiegeMachineVM selectedSlot)
    {
      List<InquiryElement> inquiryElements = new List<InquiryElement>();
      inquiryElements.Add(new InquiryElement((object) null, "Empty", (ImageIdentifier) null));
      foreach (SiegeEngineType defenderRangedMachine in this.GetAllDefenderRangedMachines())
        inquiryElements.Add(new InquiryElement((object) defenderRangedMachine, defenderRangedMachine.Name.ToString(), (ImageIdentifier) null));
      InformationManager.ShowMultiSelectionInquiry(new MultiSelectionInquiryData(new TextObject("{=SLZzfNPr}Select a Ranged Machine", (Dictionary<string, TextObject>) null).ToString(), string.Empty, inquiryElements, false, true, GameTexts.FindText("str_done", (string) null).ToString(), "", (Action<List<InquiryElement>>) (selectedElements => selectedSlot.SetMachineType(selectedElements[0].Identifier as SiegeEngineType)), (Action<List<InquiryElement>>) null, ""), false);
    }

    private void OnPlayerCharacterSelection(SelectorVM<SelectorItemVM> selector)
    {
      this.PlayerSide.CurrentSelectedCharacter = new CharacterViewModel((CharacterViewModel.StanceTypes) 1);
      this.PlayerSide.CurrentSelectedCharacter.FillFrom(this._charactersList[selector.get_SelectedIndex()], -1);
      ((IEnumerable<SelectorItemVM>) this.EnemySide.CharacterSelectionGroup.get_ItemList()).ToList<SelectorItemVM>().ForEach((Action<SelectorItemVM>) (i => i.set_CanBeSelected(true)));
      if (this.EnemySide.CharacterSelectionGroup.get_ItemList().Count <= selector.get_SelectedIndex())
        return;
      this.EnemySide.CharacterSelectionGroup.get_ItemList()[selector.get_SelectedIndex()].set_CanBeSelected(false);
    }

    private void OnEnemyCharacterSelection(SelectorVM<SelectorItemVM> selector)
    {
      this.EnemySide.CurrentSelectedCharacter = new CharacterViewModel((CharacterViewModel.StanceTypes) 1);
      this.EnemySide.CurrentSelectedCharacter.FillFrom(this._charactersList[selector.get_SelectedIndex()], -1);
      ((IEnumerable<SelectorItemVM>) this.PlayerSide.CharacterSelectionGroup.get_ItemList()).ToList<SelectorItemVM>().ForEach((Action<SelectorItemVM>) (i => i.set_CanBeSelected(true)));
      if (this.PlayerSide.CharacterSelectionGroup.get_ItemList().Count <= selector.get_SelectedIndex())
        return;
      this.PlayerSide.CharacterSelectionGroup.get_ItemList()[selector.get_SelectedIndex()].set_CanBeSelected(false);
    }

    private void OnEnemyFactionSelection(SelectorVM<SelectorItemVM> selector)
    {
      BasicCultureObject faction = this._factionList[this.EnemySide.FactionSelectionGroup.get_SelectedIndex()];
      this.EnemySide.CompositionGroup.SetCurrentSelectedCulture(this._factionList[this.EnemySide.FactionSelectionGroup.get_SelectedIndex()]);
      this.EnemySide.CurrentSelectedCultureID = faction.StringId;
    }

    private void OnPlayerFactionSelection(SelectorVM<SelectorItemVM> selector)
    {
      BasicCultureObject faction = this._factionList[this.PlayerSide.FactionSelectionGroup.get_SelectedIndex()];
      this.PlayerSide.CompositionGroup.SetCurrentSelectedCulture(faction);
      this.PlayerSide.CurrentSelectedCultureID = faction.StringId;
    }

    private void ExecuteRandomizeAttackerSiegeEngines()
    {
      List<SiegeEngineType> e = new List<SiegeEngineType>();
      e.AddRange(this.GetAllAttackerMeleeMachines());
      e.Add((SiegeEngineType) null);
      foreach (CustomBattleSiegeMachineVM attackerMeleeMachine in (Collection<CustomBattleSiegeMachineVM>) this._attackerMeleeMachines)
        attackerMeleeMachine.SetMachineType(e.GetRandomElement<SiegeEngineType>());
      e.Clear();
      e.AddRange(this.GetAllAttackerRangedMachines());
      e.Add((SiegeEngineType) null);
      foreach (CustomBattleSiegeMachineVM attackerRangedMachine in (Collection<CustomBattleSiegeMachineVM>) this._attackerRangedMachines)
        attackerRangedMachine.SetMachineType(e.GetRandomElement<SiegeEngineType>());
    }

    private void ExecuteRandomizeDefenderSiegeEngines()
    {
      List<SiegeEngineType> e = new List<SiegeEngineType>();
      e.AddRange(this.GetAllDefenderRangedMachines());
      e.Add((SiegeEngineType) null);
      foreach (CustomBattleSiegeMachineVM defenderMachine in (Collection<CustomBattleSiegeMachineVM>) this._defenderMachines)
        defenderMachine.SetMachineType(e.GetRandomElement<SiegeEngineType>());
    }

    private void ExecuteBack()
    {
      Debug.Print("EXECUTE BACK - PRESSED", 0, Debug.DebugColor.Green, 17592186044416UL);
      Game.Current.GameStateManager.PopState(0);
    }

    private void ExecuteStart()
    {
      int armySize1 = this.PlayerSide.CompositionGroup.ArmySize;
      int armySize2 = this.EnemySide.CompositionGroup.ArmySize;
      bool isPlayerAttacker = this.GameTypeSelectionGroup.GetCurrentPlayerSide() == BattleSideEnum.Attacker;
      bool isPlayerGeneral = this.GameTypeSelectionGroup.GetCurrentPlayerType() == GameTypeSelectionGroup.PlayerType.Commander;
      BasicCharacterObject playerSideGeneralCharacter = (BasicCharacterObject) null;
      if (!isPlayerGeneral)
      {
        List<BasicCharacterObject> list = this._charactersList.ToList<BasicCharacterObject>();
        list.Remove(this._charactersList[this.PlayerSide.CharacterSelectionGroup.get_SelectedIndex()]);
        list.Remove(this._charactersList[this.EnemySide.CharacterSelectionGroup.get_SelectedIndex()]);
        playerSideGeneralCharacter = list.GetRandomElement<BasicCharacterObject>();
        --armySize1;
      }
      int num1 = armySize1 - 1;
      int num2 = (int) Math.Round((double) this.PlayerSide.CompositionGroup.ArmyComposition2Value / 100.0 * (double) num1);
      int num3 = (int) Math.Round((double) this.PlayerSide.CompositionGroup.ArmyComposition3Value / 100.0 * (double) num1);
      int num4 = (int) Math.Round((double) this.PlayerSide.CompositionGroup.ArmyComposition4Value / 100.0 * (double) num1);
      int num5 = num1 - (num2 + num3 + num4);
      int num6 = armySize2 - 1;
      int num7 = (int) Math.Round((double) this.EnemySide.CompositionGroup.ArmyComposition2Value / 100.0 * (double) num6);
      int num8 = (int) Math.Round((double) this.EnemySide.CompositionGroup.ArmyComposition3Value / 100.0 * (double) num6);
      int num9 = (int) Math.Round((double) this.EnemySide.CompositionGroup.ArmyComposition4Value / 100.0 * (double) num6);
      int num10 = num6 - (num7 + num8 + num9);
      CustomBattleCombatant[] customBattleParties = this._customBattleState.GetCustomBattleParties(this._charactersList[this.PlayerSide.CharacterSelectionGroup.get_SelectedIndex()], playerSideGeneralCharacter, this._charactersList[this.EnemySide.CharacterSelectionGroup.get_SelectedIndex()], this._factionList[this.PlayerSide.FactionSelectionGroup.get_SelectedIndex()], new int[4]
      {
        num5,
        num2,
        num3,
        num4
      }, new List<BasicCharacterObject>[4]
      {
        this.PlayerSide.CompositionGroup.SelectedMeleeInfantryTypes,
        this.PlayerSide.CompositionGroup.SelectedRangedInfantryTypes,
        this.PlayerSide.CompositionGroup.SelectedMeleeCavalryTypes,
        this.PlayerSide.CompositionGroup.SelectedRangedCavalryTypes
      }, this._factionList[this.EnemySide.FactionSelectionGroup.get_SelectedIndex()], new int[4]
      {
        num10,
        num7,
        num8,
        num9
      }, new List<BasicCharacterObject>[4]
      {
        this.EnemySide.CompositionGroup.SelectedMeleeInfantryTypes,
        this.EnemySide.CompositionGroup.SelectedRangedInfantryTypes,
        this.EnemySide.CompositionGroup.SelectedMeleeCavalryTypes,
        this.EnemySide.CompositionGroup.SelectedRangedCavalryTypes
      }, (isPlayerAttacker ? 1 : 0) != 0);
      Game.Current.PlayerTroop = this._charactersList[this.PlayerSide.CharacterSelectionGroup.get_SelectedIndex()];
      bool isSiege = this.GameTypeSelectionGroup.GetCurrentGameType() == GameTypeSelectionGroup.GameType.Siege;
      MapSelectionElement selectedMap = this.MapSelectionGroup.SelectedMap;
      MapSelectionElement mapWithName = this.MapSelectionGroup.GetMapWithName(this.MapSelectionGroup.SearchText);
      if (mapWithName != null && mapWithName != selectedMap)
        selectedMap = mapWithName;
      CustomBattleSceneData customBattleSceneData = selectedMap != null ? this._customBattleScenes.Single<CustomBattleSceneData>((Func<CustomBattleSceneData, bool>) (s => s.Name.ToString() == selectedMap.MapName)) : this._customBattleScenes.First<CustomBattleSceneData>((Func<CustomBattleSceneData, bool>) (cbs => cbs.IsSiegeMap == isSiege));
      float timeOfDay = 6f;
      if (this.MapSelectionGroup.IsCurrentMapSiege)
      {
        Dictionary<SiegeEngineType, int> siegeWeaponsCountOfAttackers = new Dictionary<SiegeEngineType, int>();
        foreach (CustomBattleSiegeMachineVM attackerMeleeMachine in (Collection<CustomBattleSiegeMachineVM>) this._attackerMeleeMachines)
        {
          if (attackerMeleeMachine.SiegeEngineType != null)
          {
            SiegeEngineType siegeWeaponType = CustomBattleMenuVM.GetSiegeWeaponType(attackerMeleeMachine.SiegeEngineType);
            if (!siegeWeaponsCountOfAttackers.ContainsKey(siegeWeaponType))
              siegeWeaponsCountOfAttackers.Add(siegeWeaponType, 0);
            siegeWeaponsCountOfAttackers[siegeWeaponType]++;
          }
        }
        foreach (CustomBattleSiegeMachineVM attackerRangedMachine in (Collection<CustomBattleSiegeMachineVM>) this._attackerRangedMachines)
        {
          if (attackerRangedMachine.SiegeEngineType != null)
          {
            SiegeEngineType siegeWeaponType = CustomBattleMenuVM.GetSiegeWeaponType(attackerRangedMachine.SiegeEngineType);
            if (!siegeWeaponsCountOfAttackers.ContainsKey(siegeWeaponType))
              siegeWeaponsCountOfAttackers.Add(siegeWeaponType, 0);
            siegeWeaponsCountOfAttackers[siegeWeaponType]++;
          }
        }
        Dictionary<SiegeEngineType, int> siegeWeaponsCountOfDefenders = new Dictionary<SiegeEngineType, int>();
        foreach (CustomBattleSiegeMachineVM defenderMachine in (Collection<CustomBattleSiegeMachineVM>) this._defenderMachines)
        {
          if (defenderMachine.SiegeEngineType != null)
          {
            SiegeEngineType siegeWeaponType = CustomBattleMenuVM.GetSiegeWeaponType(defenderMachine.SiegeEngineType);
            if (!siegeWeaponsCountOfDefenders.ContainsKey(siegeWeaponType))
              siegeWeaponsCountOfDefenders.Add(siegeWeaponType, 0);
            siegeWeaponsCountOfDefenders[siegeWeaponType]++;
          }
        }
        int num11;
        float num12 = (float) (num11 = int.Parse(this.MapSelectionGroup.WallHitpointSelection.get_ItemList()[this.MapSelectionGroup.WallHitpointSelection.get_SelectedIndex()].get_StringItem())) / 100f;
        float[] wallHitPointPercentages = new float[2];
        if (num11 == 50)
        {
          int index = MBRandom.RandomInt(2);
          wallHitPointPercentages[index] = 0.0f;
          wallHitPointPercentages[1 - index] = 1f;
        }
        else
        {
          wallHitPointPercentages[0] = num12;
          wallHitPointPercentages[1] = num12;
        }
        BannerlordMissions.OpenSiegeMissionWithDeployment(customBattleSceneData.SceneID, this._charactersList[this.PlayerSide.CharacterSelectionGroup.get_SelectedIndex()], customBattleParties[0], customBattleParties[1], this.GameTypeSelectionGroup.GetCurrentPlayerType() == GameTypeSelectionGroup.PlayerType.Commander, wallHitPointPercentages, this._attackerMeleeMachines.Any<CustomBattleSiegeMachineVM>((Func<CustomBattleSiegeMachineVM, bool>) (mm => mm.SiegeEngineType == DefaultSiegeEngineTypes.SiegeTower)), siegeWeaponsCountOfAttackers, siegeWeaponsCountOfDefenders, isPlayerAttacker, int.Parse(this.MapSelectionGroup.SceneLevelSelection.GetCurrentItem().get_StringItem()), this.MapSelectionGroup.SeasonSelection.GetCurrentItem().get_StringItem().ToLower(), this.MapSelectionGroup.IsSallyOutSelected, false, timeOfDay);
      }
      else
        BannerlordMissions.OpenCustomBattleMission(customBattleSceneData.SceneID, this._charactersList[this.PlayerSide.CharacterSelectionGroup.get_SelectedIndex()], customBattleParties[0], customBattleParties[1], isPlayerGeneral, playerSideGeneralCharacter, "", this.MapSelectionGroup.SeasonSelection.GetCurrentItem().get_StringItem().ToLower(), timeOfDay);
      Debug.Print("P-Ranged: " + (object) num2 + " P-Mounted: " + (object) num3 + " P-HorseArcher: " + (object) num4 + " P-Infantry: " + (object) num5, 0, Debug.DebugColor.Blue, 17592186044416UL);
      Debug.Print("E-Ranged: " + (object) num7 + " E-Mounted: " + (object) num8 + " E-HorseArcher: " + (object) num9 + " E-Infantry: " + (object) num10, 0, Debug.DebugColor.Blue, 17592186044416UL);
      Debug.Print("EXECUTE START - PRESSED", 0, Debug.DebugColor.Green, 17592186044416UL);
    }

    private void ExecuteRandomize()
    {
      this.GameTypeSelectionGroup.RandomizeAll();
      this.MapSelectionGroup.RandomizeAll();
      this.PlayerSide.Randomize();
      this.EnemySide.Randomize();
      if (this.MapSelectionGroup.IsCurrentMapSiege)
      {
        this.ExecuteRandomizeAttackerSiegeEngines();
        this.ExecuteRandomizeDefenderSiegeEngines();
      }
      Debug.Print("EXECUTE RANDOMIZE - PRESSED", 0, Debug.DebugColor.Green, 17592186044416UL);
    }

    private void ExecuteDoneDefenderCustomMachineSelection()
    {
      this.IsDefenderCustomMachineSelectionEnabled = false;
    }

    private void ExecuteDoneAttackerCustomMachineSelection()
    {
      this.IsAttackerCustomMachineSelectionEnabled = false;
    }

    [DataSourceProperty]
    public bool IsAttackerCustomMachineSelectionEnabled
    {
      get
      {
        return this._isAttackerCustomMachineSelectionEnabled;
      }
      set
      {
        if (value == this._isAttackerCustomMachineSelectionEnabled)
          return;
        this._isAttackerCustomMachineSelectionEnabled = value;
        this.OnPropertyChanged(nameof (IsAttackerCustomMachineSelectionEnabled));
      }
    }

    [DataSourceProperty]
    public bool IsDefenderCustomMachineSelectionEnabled
    {
      get
      {
        return this._isDefenderCustomMachineSelectionEnabled;
      }
      set
      {
        if (value == this._isDefenderCustomMachineSelectionEnabled)
          return;
        this._isDefenderCustomMachineSelectionEnabled = value;
        this.OnPropertyChanged(nameof (IsDefenderCustomMachineSelectionEnabled));
      }
    }

    [DataSourceProperty]
    public string RandomizeButtonText
    {
      get
      {
        return this._randomizeButtonText;
      }
      set
      {
        if (!(value != this._randomizeButtonText))
          return;
        this._randomizeButtonText = value;
        this.OnPropertyChanged(nameof (RandomizeButtonText));
      }
    }

    [DataSourceProperty]
    public string TitleText
    {
      get
      {
        return this._titleText;
      }
      set
      {
        if (!(value != this._titleText))
          return;
        this._titleText = value;
        this.OnPropertyChanged(nameof (TitleText));
      }
    }

    [DataSourceProperty]
    public string BackButtonText
    {
      get
      {
        return this._backButtonText;
      }
      set
      {
        if (!(value != this._backButtonText))
          return;
        this._backButtonText = value;
        this.OnPropertyChanged(nameof (BackButtonText));
      }
    }

    [DataSourceProperty]
    public string StartButtonText
    {
      get
      {
        return this._startButtonText;
      }
      set
      {
        if (!(value != this._startButtonText))
          return;
        this._startButtonText = value;
        this.OnPropertyChanged(nameof (StartButtonText));
      }
    }

    [DataSourceProperty]
    public CustomBattleMenuSideVM EnemySide
    {
      get
      {
        return this._enemySide;
      }
      set
      {
        if (value == this._enemySide)
          return;
        this._enemySide = value;
        this.OnPropertyChanged(nameof (EnemySide));
      }
    }

    [DataSourceProperty]
    public CustomBattleMenuSideVM PlayerSide
    {
      get
      {
        return this._playerSide;
      }
      set
      {
        if (value == this._playerSide)
          return;
        this._playerSide = value;
        this.OnPropertyChanged(nameof (PlayerSide));
      }
    }

    [DataSourceProperty]
    public GameTypeSelectionGroup GameTypeSelectionGroup
    {
      get
      {
        return this._gameTypeSelectionGroup;
      }
      set
      {
        if (value == this._gameTypeSelectionGroup)
          return;
        this._gameTypeSelectionGroup = value;
        this.OnPropertyChanged(nameof (GameTypeSelectionGroup));
      }
    }

    [DataSourceProperty]
    public MapSelectionGroup MapSelectionGroup
    {
      get
      {
        return this._mapSelectionGroup;
      }
      set
      {
        if (value == this._mapSelectionGroup)
          return;
        this._mapSelectionGroup = value;
        this.OnPropertyChanged(nameof (MapSelectionGroup));
      }
    }

    [DataSourceProperty]
    public MBBindingList<CustomBattleSiegeMachineVM> AttackerMeleeMachines
    {
      get
      {
        return this._attackerMeleeMachines;
      }
      set
      {
        if (value == this._attackerMeleeMachines)
          return;
        this._attackerMeleeMachines = value;
        this.OnPropertyChanged(nameof (AttackerMeleeMachines));
      }
    }

    [DataSourceProperty]
    public MBBindingList<CustomBattleSiegeMachineVM> AttackerRangedMachines
    {
      get
      {
        return this._attackerRangedMachines;
      }
      set
      {
        if (value == this._attackerRangedMachines)
          return;
        this._attackerRangedMachines = value;
        this.OnPropertyChanged(nameof (AttackerRangedMachines));
      }
    }

    [DataSourceProperty]
    public MBBindingList<CustomBattleSiegeMachineVM> DefenderMachines
    {
      get
      {
        return this._defenderMachines;
      }
      set
      {
        if (value == this._defenderMachines)
          return;
        this._defenderMachines = value;
        this.OnPropertyChanged(nameof (DefenderMachines));
      }
    }

    private IEnumerable<SiegeEngineType> GetAllDefenderRangedMachines()
    {
      yield return DefaultSiegeEngineTypes.Ballista;
      yield return DefaultSiegeEngineTypes.FireBallista;
      yield return DefaultSiegeEngineTypes.Catapult;
      yield return DefaultSiegeEngineTypes.FireCatapult;
    }

    private IEnumerable<SiegeEngineType> GetAllAttackerRangedMachines()
    {
      yield return DefaultSiegeEngineTypes.Ballista;
      yield return DefaultSiegeEngineTypes.FireBallista;
      yield return DefaultSiegeEngineTypes.Onager;
      yield return DefaultSiegeEngineTypes.FireOnager;
      yield return DefaultSiegeEngineTypes.Trebuchet;
    }

    private IEnumerable<SiegeEngineType> GetAllAttackerMeleeMachines()
    {
      yield return DefaultSiegeEngineTypes.Ram;
      yield return DefaultSiegeEngineTypes.SiegeTower;
    }

    private static SiegeEngineType GetSiegeWeaponType(SiegeEngineType siegeWeaponType)
    {
      if (siegeWeaponType == DefaultSiegeEngineTypes.Ladder)
        return DefaultSiegeEngineTypes.Ladder;
      if (siegeWeaponType == DefaultSiegeEngineTypes.Ballista)
        return DefaultSiegeEngineTypes.Ballista;
      if (siegeWeaponType == DefaultSiegeEngineTypes.FireBallista)
        return DefaultSiegeEngineTypes.FireBallista;
      if (siegeWeaponType == DefaultSiegeEngineTypes.Ram || siegeWeaponType == DefaultSiegeEngineTypes.ImprovedRam)
        return DefaultSiegeEngineTypes.Ram;
      if (siegeWeaponType == DefaultSiegeEngineTypes.SiegeTower)
        return DefaultSiegeEngineTypes.SiegeTower;
      if (siegeWeaponType == DefaultSiegeEngineTypes.Onager || siegeWeaponType == DefaultSiegeEngineTypes.Catapult)
        return DefaultSiegeEngineTypes.Onager;
      if (siegeWeaponType == DefaultSiegeEngineTypes.FireOnager || siegeWeaponType == DefaultSiegeEngineTypes.FireCatapult)
        return DefaultSiegeEngineTypes.FireOnager;
      return siegeWeaponType == DefaultSiegeEngineTypes.Trebuchet || siegeWeaponType == DefaultSiegeEngineTypes.Bricole ? DefaultSiegeEngineTypes.Trebuchet : siegeWeaponType;
    }
  }
}
