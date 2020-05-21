// Decompiled with JetBrains decompiler
// Type: TaleWorlds.MountAndBlade.CustomBattle.CustomBattleMenuSideVM
// Assembly: TaleWorlds.MountAndBlade.CustomBattle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BE5DA056-FF60-485A-AA32-12EF43DB1898
// Assembly location: G:\steam\steamapps\common\Mount & Blade II Bannerlord\Modules\CustomBattle\bin\Win64_Shipping_Client\TaleWorlds.MountAndBlade.CustomBattle.dll

using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.CustomBattle
{
  public class CustomBattleMenuSideVM : ViewModel
  {
    private readonly TextObject _sideName;
    private readonly bool _isPlayerSide;
    private ArmyCompositionGroup _compositionGroup;
    private SelectorVM<SelectorItemVM> _factionSelectionGroup;
    private SelectorVM<SelectorItemVM> _characterSelectionGroup;
    private CharacterViewModel _currentSelectedCharacter;
    private string _currentSelectedCultureID;
    private string _name;
    private string _factionText;

    public CustomBattleMenuSideVM(TextObject sideName, bool isPlayerSide)
    {
      this._sideName = sideName;
      this._isPlayerSide = isPlayerSide;
      this.CompositionGroup = new ArmyCompositionGroup(sideName.ToString() + "_COMPOSITION", this._isPlayerSide);
      this.FactionSelectionGroup = new SelectorVM<SelectorItemVM>((IEnumerable<string>) new List<string>(), 0, (Action<SelectorVM<SelectorItemVM>>) null);
      this.CharacterSelectionGroup = new SelectorVM<SelectorItemVM>((IEnumerable<string>) new List<string>(), 0, (Action<SelectorVM<SelectorItemVM>>) null);
      this.RefreshValues();
    }

    public void OnPlayerTypeChange(bool isCommander)
    {
      this.CompositionGroup.OnPlayerTypeChange(isCommander);
    }

    public override void RefreshValues()
    {
      base.RefreshValues();
      this.Name = this._sideName.ToString();
      this.FactionText = GameTexts.FindText("str_faction", (string) null).ToString();
      this.CompositionGroup.RefreshValues();
      ((ViewModel) this.CharacterSelectionGroup).RefreshValues();
      ((ViewModel) this.FactionSelectionGroup).RefreshValues();
    }

    public void Randomize()
    {
      this.FactionSelectionGroup.ExecuteRandomize();
      this.CharacterSelectionGroup.ExecuteRandomize();
      this.CompositionGroup.RandomizeArmySize();
      float randomFloat1 = MBRandom.RandomFloat;
      float randomFloat2 = MBRandom.RandomFloat;
      float randomFloat3 = MBRandom.RandomFloat;
      float randomFloat4 = MBRandom.RandomFloat;
      float num1 = randomFloat1 + randomFloat2 + randomFloat3 + randomFloat4;
      float num2 = (float) Math.Round(100.0 * ((double) randomFloat1 / (double) num1));
      float num3 = (float) Math.Round(100.0 * ((double) randomFloat2 / (double) num1));
      float num4 = (float) Math.Round(100.0 * ((double) randomFloat3 / (double) num1));
      float num5 = (float) (100.0 - ((double) num2 + (double) num3 + (double) num4));
      this.CompositionGroup.IsArmyComposition1Enabled = false;
      this.CompositionGroup.IsArmyComposition2Enabled = false;
      this.CompositionGroup.IsArmyComposition3Enabled = false;
      this.CompositionGroup.IsArmyComposition4Enabled = false;
      this.CompositionGroup.ArmyComposition1Value = num2;
      this.CompositionGroup.ArmyComposition2Value = num3;
      this.CompositionGroup.ArmyComposition3Value = num4;
      this.CompositionGroup.ArmyComposition4Value = num5;
    }

    [DataSourceProperty]
    public CharacterViewModel CurrentSelectedCharacter
    {
      get
      {
        return this._currentSelectedCharacter;
      }
      set
      {
        if (value == this._currentSelectedCharacter)
          return;
        this._currentSelectedCharacter = value;
        this.OnPropertyChanged(nameof (CurrentSelectedCharacter));
      }
    }

    [DataSourceProperty]
    public string CurrentSelectedCultureID
    {
      get
      {
        return this._currentSelectedCultureID;
      }
      set
      {
        if (!(value != this._currentSelectedCultureID))
          return;
        this._currentSelectedCultureID = value;
        this.OnPropertyChanged(nameof (CurrentSelectedCultureID));
      }
    }

    [DataSourceProperty]
    public string FactionText
    {
      get
      {
        return this._factionText;
      }
      set
      {
        if (!(value != this._factionText))
          return;
        this._factionText = value;
        this.OnPropertyChanged(nameof (FactionText));
      }
    }

    [DataSourceProperty]
    public string Name
    {
      get
      {
        return this._name;
      }
      set
      {
        if (!(value != this._name))
          return;
        this._name = value;
        this.OnPropertyChanged(nameof (Name));
      }
    }

    [DataSourceProperty]
    public SelectorVM<SelectorItemVM> CharacterSelectionGroup
    {
      get
      {
        return this._characterSelectionGroup;
      }
      set
      {
        if (value == this._characterSelectionGroup)
          return;
        this._characterSelectionGroup = value;
        this.OnPropertyChanged(nameof (CharacterSelectionGroup));
      }
    }

    [DataSourceProperty]
    public ArmyCompositionGroup CompositionGroup
    {
      get
      {
        return this._compositionGroup;
      }
      set
      {
        if (value == this._compositionGroup)
          return;
        this._compositionGroup = value;
        this.OnPropertyChanged(nameof (CompositionGroup));
      }
    }

    [DataSourceProperty]
    public SelectorVM<SelectorItemVM> FactionSelectionGroup
    {
      get
      {
        return this._factionSelectionGroup;
      }
      set
      {
        if (value == this._factionSelectionGroup)
          return;
        this._factionSelectionGroup = value;
        this.OnPropertyChanged(nameof (FactionSelectionGroup));
      }
    }
  }
}
