// Decompiled with JetBrains decompiler
// Type: TaleWorlds.MountAndBlade.CustomBattle.CustomBattleScreen
// Assembly: TaleWorlds.MountAndBlade.CustomBattle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BE5DA056-FF60-485A-AA32-12EF43DB1898
// Assembly location: G:\steam\steamapps\common\Mount & Blade II Bannerlord\Modules\CustomBattle\bin\Win64_Shipping_Client\TaleWorlds.MountAndBlade.CustomBattle.dll

using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Screen;

namespace TaleWorlds.MountAndBlade.CustomBattle
{
  [GameStateScreen(typeof (CustomBattleState))]
  public class CustomBattleScreen : ScreenBase, IGameStateListener
  {
    private CustomBattleState _customBattleState;
    private GauntletLayer _gauntletLayer;
    private GauntletMovie _gauntletMovie;
    private CustomBattleMenuVM _dataSource;
    private bool _isMovieLoaded;

    public CustomBattleScreen(CustomBattleState customBattleState)
    {
      this._customBattleState = customBattleState;
    }

    void IGameStateListener.OnActivate()
    {
    }

    void IGameStateListener.OnDeactivate()
    {
    }

    void IGameStateListener.OnInitialize()
    {
    }

    void IGameStateListener.OnFinalize()
    {
    }

    protected override void OnInitialize()
    {
      base.OnInitialize();
      this._dataSource = new CustomBattleMenuVM(this._customBattleState);
      this._gauntletLayer = new GauntletLayer(1, "GauntletLayer");
      this.LoadMovie();
      ((ScreenLayer) this._gauntletLayer).InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
      this._dataSource.SetActiveState(true);
      this.AddLayer((ScreenLayer) this._gauntletLayer);
    }

    protected override void OnFinalize()
    {
      this.UnloadMovie();
      this.RemoveLayer((ScreenLayer) this._gauntletLayer);
      this._dataSource = (CustomBattleMenuVM) null;
      this._gauntletLayer = (GauntletLayer) null;
      base.OnFinalize();
    }

    protected override void OnActivate()
    {
      this.LoadMovie();
      this._dataSource?.SetActiveState(true);
      LoadingWindow.DisableGlobalLoadingWindow();
      base.OnActivate();
    }

    protected override void OnDeactivate()
    {
      base.OnDeactivate();
      this.UnloadMovie();
      this._dataSource?.SetActiveState(false);
    }

    private void LoadMovie()
    {
      if (this._isMovieLoaded)
        return;
      this._gauntletMovie = this._gauntletLayer.LoadMovie(nameof (CustomBattleScreen), (ViewModel) this._dataSource);
      this._isMovieLoaded = true;
    }

    private void UnloadMovie()
    {
      if (!this._isMovieLoaded)
        return;
      this._gauntletLayer.ReleaseMovie(this._gauntletMovie);
      this._gauntletMovie = (GauntletMovie) null;
      this._isMovieLoaded = false;
    }
  }
}
