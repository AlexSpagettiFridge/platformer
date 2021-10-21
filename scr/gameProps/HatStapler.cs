using Godot;
using System;
using System.Collections.Generic;
namespace Game
{
    public class HatStapler : Area2D
    {
        private List<int> hatIds = new List<int>();
        public int hatCapacity = 2, selectedIndex = 0, hatsRemaining = 0;
        private bool playerIsPresent = false, hatDispenceMode = false;
        private GameController gameController;
        private CanvasItem nGuiControl, nActionHint, nHatMenu;
        private AnimationTree nSelectorAnimation;
        public override void _Ready()
        {
            gameController = GetNode<GameController>("/root/GameController");
            nGuiControl = GetNode<CanvasItem>("StaplerGui/GuiControl");
            nActionHint = GetNode<CanvasItem>("StaplerGui/GuiControl/MainPanel/InsertInfo");
            nHatMenu = GetNode<CanvasItem>("StaplerGui/GuiControl/MainPanel/HatChooseMenu");
            nSelectorAnimation = GetNode<AnimationTree>("StaplerGui/GuiControl/MainPanel/HatChooseMenu/ViewportContainer/Viewport/AnimationTree");
            Connect("body_entered", this, nameof(OnBodyEntered));
            Connect("body_exited", this, nameof(OnBodyExited));
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!playerIsPresent)
            {
                return;
            }
            Hero nHero = gameController.MainPlayer;
            if (!HatDispenceMode)
            {
                if (@event.IsActionPressed("gm_up"))
                {
                    if (nHero.CurrentHat != null)
                    {
                        if (nHero.HatCount<2){
                            ChangeActionHint(false);
                        }
                        HeroHat hat = nHero.CurrentHat;
                        if (!hatIds.Contains(hat.GetHatId()))
                        {
                            hatIds.Add(hat.GetHatId());
                        }
                        hat.QueueFree();

                        
                    }
                    else
                    {
                        if(hatIds.Count != 0){
                            hatsRemaining = hatCapacity;
                            HatDispenceMode = true;
                            nHero.ChangeState(null);
                        }
                    }
                }
            }
            else
            {
                if (@event.IsActionPressed("gm_up"))
                {
                    ChangeActionHint(nHero.CurrentHat != null);
                    HatDispenceMode = false;
                    nHero.ResumeNormalState();
                }
                if (Input.IsActionJustPressed("gm_left")) { MoveSelector(-1); }
                if (Input.IsActionJustPressed("gm_right")) { MoveSelector(1); }
                if (Input.IsActionJustPressed("gm_hataction"))
                {
                    nHero.AddHat(hatIds[selectedIndex]);
                    hatsRemaining--;
                    if (hatsRemaining <= 0)
                    {
                        HatDispenceMode = false;
                        nHero.ResumeNormalState();
                        ChangeActionHint(true);
                    }
                }
            }
        }

        private void OnBodyEntered(Node body)
        {
            if (body is Hero)
            {
                playerIsPresent = true;
                nGuiControl.Visible = true;
                HatDispenceMode = false;
                GetNode<CanvasLayer>("StaplerGui").Offset = Position;
                Hero hero = (Hero)body;
                ChangeActionHint(hero.CurrentHat != null);
            }
        }

        private void OnBodyExited(Node body)
        {
            if (body is Hero)
            {
                playerIsPresent = false;
                nGuiControl.Visible = false;
                Hero nHero = (Hero)body;
                if (nHero.CurrentState == null)
                {
                    nHero.ResumeNormalState();
                }
            }
        }

        private bool HatDispenceMode
        {
            get { return hatDispenceMode; }
            set
            {
                nActionHint.Visible = !value;
                hatDispenceMode = value;
                nHatMenu.Visible = value;
                if (value){
                    MoveSelector(0);
                }
            }
        }

        private void ChangeActionHint(bool insertMode)
        {
            Label nActionHintLabel = (Label)nActionHint;
            if (insertMode)
            {
                nActionHintLabel.Text = "insert Hat";
            }
            else
            {
                nActionHintLabel.Text = "activate";
                if(hatIds.Count == 0){
                    nActionHintLabel.Text = "empty";
                }
            }
        }

        private void MoveSelector(int change)
        {
            if (change>0){
                GetNode<AnimationPlayer>("StaplerGui/GuiControl/MainPanel/HatChooseMenu/ViewportContainer/Viewport/AnimationPlayer").Play("shiftLeft");
            }
            selectedIndex = GetSelectorNeighbour(change);
            Node nSpriteContainer = GetNode<Node2D>("StaplerGui/GuiControl/MainPanel/HatChooseMenu/ViewportContainer/Viewport/SpriteContainer");
            int offset = -2;
            foreach (AnimatedSprite nHatSprite in nSpriteContainer.GetChildren())
            {
                nHatSprite.Play(hatIds[GetSelectorNeighbour(offset)] + "idle");
                offset++;
            }
        }

        private int GetSelectorNeighbour(int change)
        {
            int neighbour = selectedIndex + change;
            for (;neighbour < 0;)
            {
                neighbour += hatIds.Count;
            }
            for (;neighbour >= hatIds.Count;)
            {
                neighbour -= hatIds.Count;
            }
            return neighbour;
        }

        public void SetHatCapacity(int hatCapacity){
            this.hatCapacity = hatCapacity;
        }
    }
}
