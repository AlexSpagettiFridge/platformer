using Godot;
using System;

namespace Game
{
    public class GuiController : CanvasLayer
    {
        private GameController gameController;
        private Label nBerryCounter;
        public override void _Ready()
        {
            gameController = GetParent<GameController>();
            nBerryCounter = GetNode<Label>("Control/BerryCounter/BerryCounterText");
            UpdateBerryCounter();
        }

        public void UpdateBerryCounter()
        {
            nBerryCounter.Text = gameController.Berries + "/" + gameController.LocalTotalBerries;
        }
    }
}
