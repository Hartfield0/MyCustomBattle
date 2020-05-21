// Decompiled with JetBrains decompiler
// Type: TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.MapSelectionElement
// Assembly: TaleWorlds.MountAndBlade.CustomBattle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BE5DA056-FF60-485A-AA32-12EF43DB1898
// Assembly location: G:\steam\steamapps\common\Mount & Blade II Bannerlord\Modules\CustomBattle\bin\Win64_Shipping_Client\TaleWorlds.MountAndBlade.CustomBattle.dll

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle
{
  public class MapSelectionElement
  {
    public MapSelectionElement(string mapName, bool isSiegeMap = false, bool isVillageMap = false)
    {
      this.MapName = mapName;
      this.IsSiegeMap = isSiegeMap;
      this.IsVillageMap = isVillageMap;
    }

    public string MapName { get; private set; }

    public bool IsSiegeMap { get; private set; }

    public bool IsVillageMap { get; private set; }
  }
}
