using Godot;

namespace Game
{
    public class MainMenu : Control
    {

        public override void _Ready()
        {
            Button nButtonContinue = GetNode<Button>("Panel/VBoxContainer/ButtonContinue");
            nButtonContinue.Connect("pressed", this, nameof(OnContinueButtonPressed));

            Button nButtonPlay = GetNode<Button>("Panel/VBoxContainer/ButtonPlay");
            nButtonPlay.Connect("pressed", this, nameof(OnPlayButtonPressed));

            Button nButtonConfig = GetNode<Button>("Panel/VBoxContainer/ButtonConfig");
            nButtonConfig.Connect("pressed", this, nameof(OnConfigButtonPressed));

            Button nButtonExit = GetNode<Button>("Panel/VBoxContainer/ButtonExit");
            nButtonExit.Connect("pressed", this, nameof(OnExitButtonPressed));

            nButtonPlay.GrabFocus();
            
            nButtonContinue.FocusNeighbourTop = nButtonExit.GetPath();
            nButtonContinue.FocusNeighbourBottom = nButtonPlay.GetPath();
            nButtonPlay.FocusNeighbourTop = nButtonContinue.GetPath();
            nButtonPlay.FocusNeighbourBottom = nButtonConfig.GetPath();
            nButtonConfig.FocusNeighbourTop = nButtonPlay.GetPath();
            nButtonConfig.FocusNeighbourBottom = nButtonExit.GetPath();
            nButtonExit.FocusNeighbourTop = nButtonConfig.GetPath();
            nButtonExit.FocusNeighbourBottom = nButtonContinue.GetPath();
        }

        private void OnContinueButtonPressed()
        {
            GeneralSaveGame saveGame = GeneralSaveGame.Load(GeneralSaveGame.LastLoadedSave);
            StartGame(saveGame);
        }

        private void OnPlayButtonPressed()
        {
            Control savegameScene = (Control)ResourceLoader.Load<PackedScene>("res://scenes/SavegameScene.tscn").Instance();
            AddChild(savegameScene);
        }

        private void OnConfigButtonPressed()
        {
            ConfigMenu configMenu = (ConfigMenu)ResourceLoader.Load<PackedScene>("res://scenes/ConfigMenu.tscn").Instance();
            AddChild(configMenu);
        }

        private void OnExitButtonPressed()
        {
            GetTree().Quit();
        }

        public void StartGame(GeneralSaveGame saveGame){
            GameController nGameScene = (GameController)ResourceLoader.Load<PackedScene>("res://scenes/GameScene.tscn").Instance();
            nGameScene.SaveGame = saveGame;
            GetNode("/root/").AddChild(nGameScene);
            QueueFree();
        }

    }
}
