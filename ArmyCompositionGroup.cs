// Decompiled with JetBrains decompiler
// Type: TaleWorlds.MountAndBlade.CustomBattle.ArmyCompositionGroup
// Assembly: TaleWorlds.MountAndBlade.CustomBattle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BE5DA056-FF60-485A-AA32-12EF43DB1898
// Assembly location: G:\steam\steamapps\common\Mount & Blade II Bannerlord\Modules\CustomBattle\bin\Win64_Shipping_Client\TaleWorlds.MountAndBlade.CustomBattle.dll

using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.CustomBattle
{
  public class ArmyCompositionGroup : ViewModel
  {
    private float _maxNumOfMen = (float) Math.Round((double) BannerlordConfig.BattleSize / 2.0);
    private List<BasicCharacterObject> _allCharacterObjects = new List<BasicCharacterObject>();
    private List<BasicCharacterObject> _meleeInfantryTypes = new List<BasicCharacterObject>();
    private List<BasicCharacterObject> _rangedInfantryTypes = new List<BasicCharacterObject>();
    private List<BasicCharacterObject> _meleeCavalryTypes = new List<BasicCharacterObject>();
    private List<BasicCharacterObject> _rangedCavalryTypes = new List<BasicCharacterObject>();
    private readonly bool _isPlayerSide;
    private float _minNumOfMen;
    private bool updatingSliders;
    private BasicCultureObject _selectedCulture;
    private float[] _values;
    private int _armySize;
    private float _armySizeSliderValue;
    private string _armySizeText;
    private string _armyComposition1Text;
    private string _armyComposition2Text;
    private string _armyComposition3Text;
    private string _armyComposition4Text;
    private bool _isArmyComposition1Enabled;
    private bool _isArmyComposition2Enabled;
    private bool _isArmyComposition3Enabled;
    private bool _isArmyComposition4Enabled;
    private string _name;
    private string _armySizeTitle;

    public List<BasicCharacterObject> SelectedMeleeInfantryTypes { get; private set; }

    public List<BasicCharacterObject> SelectedRangedInfantryTypes { get; private set; }

    public List<BasicCharacterObject> SelectedMeleeCavalryTypes { get; private set; }

    public List<BasicCharacterObject> SelectedRangedCavalryTypes { get; private set; }

    public ArmyCompositionGroup(string name, bool isPlayerSide)
    {
      this._isPlayerSide = isPlayerSide;
      this._minNumOfMen = 1f;
      this._armySizeText = this._minNumOfMen.ToString() + " men";
      Game.Current.ObjectManager.GetAllInstancesOfObjectType<BasicCharacterObject>(ref this._allCharacterObjects);
      this._allCharacterObjects = this._allCharacterObjects.Where<BasicCharacterObject>((Func<BasicCharacterObject, bool>) (c => c.IsSoldier)).ToList<BasicCharacterObject>();
      this.SelectedMeleeInfantryTypes = new List<BasicCharacterObject>();
      this.SelectedRangedInfantryTypes = new List<BasicCharacterObject>();
      this.SelectedMeleeCavalryTypes = new List<BasicCharacterObject>();
      this.SelectedRangedCavalryTypes = new List<BasicCharacterObject>();
      this._armySize = 1;
      this._name = name;
      this._values = new float[4];
      this._values[0] = 25f;
      this._values[1] = 25f;
      this._values[2] = 25f;
      this._values[3] = 25f;
      this.ArmyComposition1Text = "%" + this._values[0].ToString("0.0");
      this.ArmyComposition2Text = "%" + this._values[1].ToString("0.0");
      this.ArmyComposition3Text = "%" + this._values[2].ToString("0.0");
      this.ArmyComposition4Text = "%" + this._values[3].ToString("0.0");
      this.RefreshValues();
    }

    public override void RefreshValues()
    {
      base.RefreshValues();
      this.ArmySizeTitle = GameTexts.FindText("str_army_size", (string) null).ToString();
    }

    private static float SumOfValues(float[] array, bool[] enabledArray, int excludedIndex = -1)
    {
      float num = 0.0f;
      for (int index = 0; index < array.Length; ++index)
      {
        if (enabledArray[index] && excludedIndex != index)
          num += array[index];
      }
      return num;
    }

    private void ExecuteMeleeInfantryTypeSelection()
    {
      if (!Game.Current.IsDevelopmentMode)
        return;
      List<InquiryElement> inquiryElements = new List<InquiryElement>();
      foreach (BasicCharacterObject meleeInfantryType in this._meleeInfantryTypes)
      {
        ImageIdentifier imageIdentifier = new ImageIdentifier(CharacterCode.CreateFrom(meleeInfantryType));
        inquiryElements.Add(new InquiryElement((object) meleeInfantryType, meleeInfantryType.Name.ToString(), imageIdentifier));
      }
      InformationManager.ShowMultiSelectionInquiry(new MultiSelectionInquiryData("Melee Infantry Troop Types", string.Empty, inquiryElements, true, false, "Done", "", new Action<List<InquiryElement>>(this.OnMeleeInfantryTypeSelectionOver), new Action<List<InquiryElement>>(this.OnMeleeInfantryTypeSelectionOver), ""), false);
    }

    private void OnMeleeInfantryTypeSelectionOver(List<InquiryElement> selectedElements)
    {
      this.SelectedMeleeInfantryTypes.Clear();
      foreach (InquiryElement selectedElement in selectedElements)
        this.SelectedMeleeInfantryTypes.Add(selectedElement.Identifier as BasicCharacterObject);
    }

    private void ExecuteRangedInfantryTypeSelection()
    {
      if (!Game.Current.IsDevelopmentMode)
        return;
      List<InquiryElement> inquiryElements = new List<InquiryElement>();
      foreach (BasicCharacterObject rangedInfantryType in this._rangedInfantryTypes)
      {
        ImageIdentifier imageIdentifier = new ImageIdentifier(CharacterCode.CreateFrom(rangedInfantryType));
        inquiryElements.Add(new InquiryElement((object) rangedInfantryType, rangedInfantryType.Name.ToString(), imageIdentifier));
      }
      InformationManager.ShowMultiSelectionInquiry(new MultiSelectionInquiryData("Ranged Infantry Troop Types", string.Empty, inquiryElements, true, false, "Done", "", new Action<List<InquiryElement>>(this.OnRangedInfantryTypeSelectionOver), new Action<List<InquiryElement>>(this.OnRangedInfantryTypeSelectionOver), ""), false);
    }

    private void OnRangedInfantryTypeSelectionOver(List<InquiryElement> selectedElements)
    {
      this.SelectedRangedInfantryTypes.Clear();
      foreach (InquiryElement selectedElement in selectedElements)
        this.SelectedRangedInfantryTypes.Add(selectedElement.Identifier as BasicCharacterObject);
    }

    private void ExecuteMeleeCavalryTypeSelection()
    {
      if (!Game.Current.IsDevelopmentMode)
        return;
      List<InquiryElement> inquiryElements = new List<InquiryElement>();
      foreach (BasicCharacterObject meleeCavalryType in this._meleeCavalryTypes)
      {
        ImageIdentifier imageIdentifier = new ImageIdentifier(CharacterCode.CreateFrom(meleeCavalryType));
        inquiryElements.Add(new InquiryElement((object) meleeCavalryType, meleeCavalryType.Name.ToString(), imageIdentifier));
      }
      InformationManager.ShowMultiSelectionInquiry(new MultiSelectionInquiryData("Melee Cavalry Troop Types", string.Empty, inquiryElements, true, false, "Done", "", new Action<List<InquiryElement>>(this.OnMeleeCavalryTypeSelectionOver), new Action<List<InquiryElement>>(this.OnMeleeCavalryTypeSelectionOver), ""), false);
    }

    private void OnMeleeCavalryTypeSelectionOver(List<InquiryElement> selectedElements)
    {
      this.SelectedMeleeCavalryTypes.Clear();
      foreach (InquiryElement selectedElement in selectedElements)
        this.SelectedMeleeCavalryTypes.Add(selectedElement.Identifier as BasicCharacterObject);
    }

    private void ExecuteRangedCavalryTypeSelection()
    {
      if (!Game.Current.IsDevelopmentMode)
        return;
      List<InquiryElement> inquiryElements = new List<InquiryElement>();
      foreach (BasicCharacterObject rangedCavalryType in this._rangedCavalryTypes)
      {
        ImageIdentifier imageIdentifier = new ImageIdentifier(CharacterCode.CreateFrom(rangedCavalryType));
        inquiryElements.Add(new InquiryElement((object) rangedCavalryType, rangedCavalryType.Name.ToString(), imageIdentifier));
      }
      InformationManager.ShowMultiSelectionInquiry(new MultiSelectionInquiryData("Ranged Cavalry Troop Types", string.Empty, inquiryElements, true, false, "Done", "", new Action<List<InquiryElement>>(this.OnRangedCavalryTypeSelectionOver), new Action<List<InquiryElement>>(this.OnRangedCavalryTypeSelectionOver), ""), false);
    }

    private void OnRangedCavalryTypeSelectionOver(List<InquiryElement> selectedElements)
    {
      this.SelectedRangedCavalryTypes.Clear();
      foreach (InquiryElement selectedElement in selectedElements)
        this.SelectedRangedCavalryTypes.Add(selectedElement.Identifier as BasicCharacterObject);
    }

    public void SetCurrentSelectedCulture(BasicCultureObject selectedCulture)
    {
      if (this._selectedCulture == selectedCulture)
        return;
      this.PopulateTroopTypeList(ArmyCompositionGroup.TroopType.MeleeInfantry, selectedCulture);
      this.PopulateTroopTypeList(ArmyCompositionGroup.TroopType.RangedInfantry, selectedCulture);
      this.PopulateTroopTypeList(ArmyCompositionGroup.TroopType.MeleeCavalry, selectedCulture);
      this.PopulateTroopTypeList(ArmyCompositionGroup.TroopType.RangedCavalry, selectedCulture);
      this._selectedCulture = selectedCulture;
    }

    private void PopulateTroopTypeList(
      ArmyCompositionGroup.TroopType troopType,
      BasicCultureObject cultureOfTroops)
    {
      if (troopType == ArmyCompositionGroup.TroopType.MeleeInfantry)
      {
        this._meleeInfantryTypes.Clear();
        this._meleeInfantryTypes.AddRange(this._allCharacterObjects.Where<BasicCharacterObject>((Func<BasicCharacterObject, bool>) (o => this.IsValidUnitItem(o, cultureOfTroops, troopType))));
        this.SelectedMeleeInfantryTypes.Clear();
        this.SelectedMeleeInfantryTypes.Add(CustomBattleState.Helper.GetDefaultTroopOfFormationForFaction(cultureOfTroops, FormationClass.Infantry));
      }
      else if (troopType == ArmyCompositionGroup.TroopType.RangedInfantry)
      {
        this._rangedInfantryTypes.Clear();
        this._rangedInfantryTypes.AddRange(this._allCharacterObjects.Where<BasicCharacterObject>((Func<BasicCharacterObject, bool>) (o => this.IsValidUnitItem(o, cultureOfTroops, troopType))));
        this.SelectedRangedInfantryTypes.Clear();
        this.SelectedRangedInfantryTypes.Add(CustomBattleState.Helper.GetDefaultTroopOfFormationForFaction(cultureOfTroops, FormationClass.Ranged));
      }
      else if (troopType == ArmyCompositionGroup.TroopType.MeleeCavalry)
      {
        this._meleeCavalryTypes.Clear();
        this._meleeCavalryTypes.AddRange(this._allCharacterObjects.Where<BasicCharacterObject>((Func<BasicCharacterObject, bool>) (o => this.IsValidUnitItem(o, cultureOfTroops, troopType))));
        this.SelectedMeleeCavalryTypes.Clear();
        this.SelectedMeleeCavalryTypes.Add(CustomBattleState.Helper.GetDefaultTroopOfFormationForFaction(cultureOfTroops, FormationClass.Cavalry));
      }
      else
      {
        if (troopType != ArmyCompositionGroup.TroopType.RangedCavalry)
          return;
        this._rangedCavalryTypes.Clear();
        this._rangedCavalryTypes.AddRange(this._allCharacterObjects.Where<BasicCharacterObject>((Func<BasicCharacterObject, bool>) (o => this.IsValidUnitItem(o, cultureOfTroops, troopType))));
        this.SelectedRangedCavalryTypes.Clear();
        this.SelectedRangedCavalryTypes.Add(CustomBattleState.Helper.GetDefaultTroopOfFormationForFaction(cultureOfTroops, FormationClass.HorseArcher));
      }
    }

    private bool IsValidUnitItem(
      BasicCharacterObject o,
      BasicCultureObject culture,
      ArmyCompositionGroup.TroopType troopType)
    {
      if (o == null || culture != o.Culture)
        return false;
      switch (troopType)
      {
        case ArmyCompositionGroup.TroopType.MeleeInfantry:
          return o.CurrentFormationClass == FormationClass.Infantry || o.CurrentFormationClass == FormationClass.HeavyInfantry;
        case ArmyCompositionGroup.TroopType.RangedInfantry:
          return o.CurrentFormationClass == FormationClass.Ranged;
        case ArmyCompositionGroup.TroopType.MeleeCavalry:
          return o.CurrentFormationClass == FormationClass.Cavalry || o.CurrentFormationClass == FormationClass.HeavyCavalry || o.CurrentFormationClass == FormationClass.LightCavalry;
        case ArmyCompositionGroup.TroopType.RangedCavalry:
          return o.CurrentFormationClass == FormationClass.HorseArcher;
        default:
          return false;
      }
    }

    private static float SumOfValues(float[] array)
    {
      float num = 0.0f;
      for (int index = 0; index < array.Length; ++index)
        num += array[index];
      return num;
    }

    private void UpdateSliders(float value, int changedSliderIndex)
    {
      this.updatingSliders = true;
      bool[] enabledArray = new bool[4]
      {
        !this.IsArmyComposition1Enabled,
        !this.IsArmyComposition2Enabled,
        !this.IsArmyComposition3Enabled,
        !this.IsArmyComposition4Enabled
      };
      float[] array = new float[4]
      {
        this._values[0],
        this._values[1],
        this._values[2],
        this._values[3]
      };
      float[] numArray = new float[4]
      {
        this._values[0],
        this._values[1],
        this._values[2],
        this._values[3]
      };
      int num1 = 0;
      for (int index = 0; index < enabledArray.Length; ++index)
      {
        if (enabledArray[index] && index != changedSliderIndex)
          ++num1;
      }
      if (num1 > 0)
      {
        float num2 = ArmyCompositionGroup.SumOfValues(array, enabledArray, -1);
        if ((double) value >= (double) num2)
          value = num2;
        float num3 = value - array[changedSliderIndex];
        if ((double) num3 != 0.0)
        {
          float num4 = ArmyCompositionGroup.SumOfValues(array, enabledArray, changedSliderIndex);
          float num5 = num4 - num3;
          if ((double) num5 > 0.0)
          {
            float num6 = 0.0f;
            numArray[changedSliderIndex] = value;
            for (int index = 0; index < enabledArray.Length; ++index)
            {
              if (changedSliderIndex != index && enabledArray[index] && (double) array[index] != 0.0)
              {
                float num7 = array[index] / num4 * num5;
                num6 += num7;
                numArray[index] = num7;
              }
            }
            float num8 = num5 - num6;
            if ((double) num8 != 0.0)
            {
              int num7 = 0;
              for (int index = 0; index < enabledArray.Length; ++index)
              {
                if (enabledArray[index] && index != changedSliderIndex && (0.0 < (double) array[index] + (double) num8 && 100.0 > (double) array[index] + (double) num8))
                  ++num7;
              }
              for (int index = 0; index < enabledArray.Length; ++index)
              {
                if (enabledArray[index] && index != changedSliderIndex && (0.0 < (double) array[index] + (double) num8 && 100.0 > (double) array[index] + (double) num8))
                  numArray[index] = numArray[index] + num8 / (float) num7;
              }
            }
          }
          else
          {
            numArray[changedSliderIndex] = value;
            for (int index = 0; index < enabledArray.Length; ++index)
            {
              if (changedSliderIndex != index && enabledArray[index])
                numArray[index] = 0.0f;
            }
          }
        }
      }
      this.SetArmyCompositionValue(0, numArray[0]);
      this.SetArmyCompositionValue(1, numArray[1]);
      this.SetArmyCompositionValue(2, numArray[2]);
      this.SetArmyCompositionValue(3, numArray[3]);
      this.updatingSliders = false;
    }

    private int CalculateNumOfAvailableSliders(
      bool isAvailableToIncrease,
      int indexOfSliderToExclude)
    {
      int num = 0;
      if (isAvailableToIncrease)
      {
        if ((double) this.ArmyComposition1Value < 100.0 && indexOfSliderToExclude != 1)
          ++num;
        if ((double) this.ArmyComposition2Value < 100.0 && indexOfSliderToExclude != 2)
          ++num;
        if ((double) this.ArmyComposition3Value < 100.0 && indexOfSliderToExclude != 3)
          ++num;
        if ((double) this.ArmyComposition4Value < 100.0 && indexOfSliderToExclude != 4)
          ++num;
      }
      else
      {
        if ((double) this.ArmyComposition1Value > 0.0 && indexOfSliderToExclude != 1)
          ++num;
        if ((double) this.ArmyComposition2Value > 0.0 && indexOfSliderToExclude != 2)
          ++num;
        if ((double) this.ArmyComposition3Value > 0.0 && indexOfSliderToExclude != 3)
          ++num;
        if ((double) this.ArmyComposition4Value > 0.0 && indexOfSliderToExclude != 4)
          ++num;
      }
      return num;
    }

    public void RandomizeArmySize()
    {
      this.ArmySizeSliderValue = MBRandom.RandomFloat * 100f;
    }

    internal void OnPlayerTypeChange(bool isCommander)
    {
      this._minNumOfMen = isCommander ? 1f : 2f;
      this.OnArmySizePercentChange(this.ArmySizeSliderValue);
    }

    private void OnArmySizePercentChange(float value)
    {
      this.ArmySize = (int) Math.Round((double) value / 100.0 * ((double) this._maxNumOfMen - (double) this._minNumOfMen) + (double) this._minNumOfMen);
      TextObject textObject = new TextObject("{=mxbq3InD}{ARMY_SIZE} men", (Dictionary<string, TextObject>) null);
      textObject.SetTextVariable("ARMY_SIZE", this.ArmySize);
      this.ArmySizeText = textObject.ToString();
    }

    [DataSourceProperty]
    public string ArmySizeTitle
    {
      get
      {
        return this._armySizeTitle;
      }
      set
      {
        if (!(value != this._armySizeTitle))
          return;
        this._armySizeTitle = value;
        this.OnPropertyChanged(nameof (ArmySizeTitle));
      }
    }

    public int ArmySize
    {
      get
      {
        return this._armySize;
      }
      set
      {
        if (this._armySize == value)
          return;
        this._armySize = value;
        this.OnPropertyChanged(nameof (ArmySize));
      }
    }

    public float ArmySizeSliderValue
    {
      get
      {
        return this._armySizeSliderValue;
      }
      set
      {
        if ((double) this._armySizeSliderValue == (double) value)
          return;
        this._armySizeSliderValue = value;
        this.OnPropertyChanged(nameof (ArmySizeSliderValue));
        this.OnArmySizePercentChange(value);
      }
    }

    public string ArmySizeText
    {
      get
      {
        return this._armySizeText;
      }
      set
      {
        if (!(this._armySizeText != value))
          return;
        this._armySizeText = value;
        this.OnPropertyChanged(nameof (ArmySizeText));
      }
    }

    private void CheckAndSet(float value, int index)
    {
      if ((double) this._values[index] == (double) value || this.updatingSliders)
        return;
      this.UpdateSliders(value, index);
    }

    public float ArmyComposition1Value
    {
      get
      {
        return this._values[0];
      }
      set
      {
        this.CheckAndSet(value, 0);
      }
    }

    public float ArmyComposition2Value
    {
      get
      {
        return this._values[1];
      }
      set
      {
        this.CheckAndSet(value, 1);
      }
    }

    public float ArmyComposition3Value
    {
      get
      {
        return this._values[2];
      }
      set
      {
        this.CheckAndSet(value, 2);
      }
    }

    public float ArmyComposition4Value
    {
      get
      {
        return this._values[3];
      }
      set
      {
        this.CheckAndSet(value, 3);
      }
    }

    private void SetArmyCompositionValue(int index, float value)
    {
      switch (index)
      {
        case 0:
          this._values[0] = value;
          this.OnPropertyChanged("ArmyComposition1Value");
          this.ArmyComposition1Text = "%" + value.ToString("0.0");
          break;
        case 1:
          this._values[1] = value;
          this.OnPropertyChanged("ArmyComposition2Value");
          this.ArmyComposition2Text = "%" + value.ToString("0.0");
          break;
        case 2:
          this._values[2] = value;
          this.OnPropertyChanged("ArmyComposition3Value");
          this.ArmyComposition3Text = "%" + value.ToString("0.0");
          break;
        case 3:
          this._values[3] = value;
          this.OnPropertyChanged("ArmyComposition4Value");
          this.ArmyComposition4Text = "%" + value.ToString("0.0");
          break;
      }
    }

    public bool IsArmyComposition1Enabled
    {
      get
      {
        return this._isArmyComposition1Enabled;
      }
      set
      {
        if (this._isArmyComposition1Enabled == value)
          return;
        this._isArmyComposition1Enabled = value;
        this.OnPropertyChanged(nameof (IsArmyComposition1Enabled));
      }
    }

    public bool IsArmyComposition2Enabled
    {
      get
      {
        return this._isArmyComposition2Enabled;
      }
      set
      {
        if (this._isArmyComposition2Enabled == value)
          return;
        this._isArmyComposition2Enabled = value;
        this.OnPropertyChanged(nameof (IsArmyComposition2Enabled));
      }
    }

    public bool IsArmyComposition3Enabled
    {
      get
      {
        return this._isArmyComposition3Enabled;
      }
      set
      {
        if (this._isArmyComposition3Enabled == value)
          return;
        this._isArmyComposition3Enabled = value;
        this.OnPropertyChanged(nameof (IsArmyComposition3Enabled));
      }
    }

    public bool IsArmyComposition4Enabled
    {
      get
      {
        return this._isArmyComposition4Enabled;
      }
      set
      {
        if (this._isArmyComposition4Enabled == value)
          return;
        this._isArmyComposition4Enabled = value;
        this.OnPropertyChanged(nameof (IsArmyComposition4Enabled));
      }
    }

    [DataSourceProperty]
    public string ArmyComposition1Text
    {
      get
      {
        return this._armyComposition1Text;
      }
      set
      {
        if (!(this._armyComposition1Text != value))
          return;
        this._armyComposition1Text = value;
        this.OnPropertyChanged(nameof (ArmyComposition1Text));
      }
    }

    [DataSourceProperty]
    public string ArmyComposition2Text
    {
      get
      {
        return this._armyComposition2Text;
      }
      set
      {
        if (!(this._armyComposition2Text != value))
          return;
        this._armyComposition2Text = value;
        this.OnPropertyChanged(nameof (ArmyComposition2Text));
      }
    }

    [DataSourceProperty]
    public string ArmyComposition3Text
    {
      get
      {
        return this._armyComposition3Text;
      }
      set
      {
        if (!(this._armyComposition3Text != value))
          return;
        this._armyComposition3Text = value;
        this.OnPropertyChanged(nameof (ArmyComposition3Text));
      }
    }

    [DataSourceProperty]
    public string ArmyComposition4Text
    {
      get
      {
        return this._armyComposition4Text;
      }
      set
      {
        if (!(this._armyComposition4Text != value))
          return;
        this._armyComposition4Text = value;
        this.OnPropertyChanged(nameof (ArmyComposition4Text));
      }
    }

    private enum TroopType
    {
      MeleeInfantry,
      RangedInfantry,
      MeleeCavalry,
      RangedCavalry,
    }
  }
}
