using Godot;
using System;

namespace Game
{
    public class HatRespawnPoint : Node2D
    {
        private static readonly PackedScene propHatPickup = (PackedScene)ResourceLoader.Load("res://props/misc/HatPickup.tscn");
        private Timer nTimer;
        private int hatIndex = 0;
        public override void _Ready()
        {
            nTimer = GetNode<Timer>("Timer");
            nTimer.Connect("timeout", this, nameof(CreateHat));
            CreateHat();
        }

        private void CreateHat()
        {
            HatPickup nHatPickup = (HatPickup)propHatPickup.Instance();
            nHatPickup.HatIndex = hatIndex;
            AddChild(nHatPickup);
            nHatPickup.Connect("tree_exited", this, nameof(OnHatExitedTree));
        }

        private void OnHatExitedTree()
        {
            nTimer.Start();
        }

        public int HatIndex
        {
            get { return hatIndex; }
            set
            {
                if (value != hatIndex)
                {
                    hatIndex = value;
                    HatPickup nHat = GetNode<HatPickup>("HatPickup");
                    if (nHat!=null){
                        nHat.HatIndex = value;
                    }
                }
            }
        }
    }
}