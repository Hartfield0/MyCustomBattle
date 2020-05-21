// Decompiled with JetBrains decompiler
// Type: TaleWorlds.MountAndBlade.CustomBattle.CustomBattleSubModule
// Assembly: TaleWorlds.MountAndBlade.CustomBattle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BE5DA056-FF60-485A-AA32-12EF43DB1898
// Assembly location: G:\steam\steamapps\common\Mount & Blade II Bannerlord\Modules\CustomBattle\bin\Win64_Shipping_Client\TaleWorlds.MountAndBlade.CustomBattle.dll

using System;
using System.Collections.Generic;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.CustomBattle
{
  public class CustomBattleSubModule : MBSubModuleBase
  {
    protected override void OnSubModuleLoad()
    {
      base.OnSubModuleLoad();
      Module.CurrentModule.AddInitialStateOption(new InitialStateOption("CustomBattle", new TextObject("{=4gOGGbeQ}Custom Battle", (Dictionary<string, TextObject>) null), 5000, (Action) (() => MBGameManager.StartNewGame((MBGameManager) new CustomGameManager())), false));
    }
  }
}
