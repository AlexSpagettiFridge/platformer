using Godot;
using System;

namespace Game
{
    public class PauseMenu : Node
    {

        public override void _Ready()
        {
            GetTree().Paused = true;
            GetNode<Button>("Control/Panel/VBoxContainer/ButtonContinue").Connect("pressed", this, nameof(OnContinue));
            GetNode<Button>("Control/Panel/VBoxContainer/ButtonConfig").Connect("pressed", this, nameof(OnConfig));
            GetNode<Button>("Control/Panel/VBoxContainer/ButtonToMenu").Connect("pressed", this, nameof(OnToMenu));
            GetNode<Button>("Control/Panel/VBoxContainer/ButtonExit").Connect("pressed", this, nameof(OnExit));
        }


        public override void _Process(float delta)
        {
            if (Input.IsActionJustPressed("gm_pause"))
            {
                OnContinue();
            }
        }

        private void OnContinue()
        {
            QueueFree();
            GetTree().Paused = false;
        }

        private void OnConfig()
        {
            ConfigMenu nConfigMenu = (ConfigMenu)ResourceLoader.Load<PackedScene>("res://scenes/ConfigMenu.tscn").Instance();
            AddChild(nConfigMenu);
        }

        private void OnToMenu()
        {
            MainMenu nMainMenu = (MainMenu)ResourceLoader.Load<PackedScene>("res://scenes/MainMenu.tscn").Instance();
            Util.GetGameController(this).QueueFree();
            GetNode("/root/").AddChild(nMainMenu);
        }

        private void OnExit()
        {
            GetTree().Quit();
        }
    }
}