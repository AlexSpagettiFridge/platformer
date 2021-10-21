using Godot;
using System;

namespace Game
{
    public class SavegameSelect : Control
    {
        [Export]
        private int slotId;
        private GeneralSaveGame generalSave;
        private LocalSaveGame localSave;
        private Label nBerryLabel, nScrollLabel;
        public override void _Ready()
        {
            nBerryLabel = GetNode<Label>("HSplitContainer/LeftControl/HBoxContainer/LeftControl/VBoxContainer/berryBox/BerryLabel");
            nScrollLabel = GetNode<Label>("HSplitContainer/LeftControl/HBoxContainer/LeftControl/VBoxContainer/HBoxContainer2/ScrollLabel2");
            UpdateInformation();
        }

        private void UpdateInformation(){
            generalSave = GeneralSaveGame.Load(slotId);
            localSave = generalSave.GetCurrentMapLocalSaveGame();
            GetNode<Label>("HSplitContainer/LeftControl/SlotName").Text = "Slot "+slotId;
            nBerryLabel.Text = generalSave.totalBerries+"/IDK";
            nScrollLabel.Text = generalSave.totalScrolls+"/IDK";
        }

        private void OnLoadButtonPressed(){
            GeneralSaveGame.LastLoadedSave = slotId;
            GetNode<MainMenu>("/root/MainMenu").StartGame(generalSave);
        }

        private void OnDeleteButtonPressed(){
            generalSave.Delete();
            UpdateInformation();
        }
    }
}