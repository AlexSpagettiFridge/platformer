using Godot;
using System;

namespace Game.MindSpace
{
    public class MindController : Node2D
    {
        private PackedScene propMindText = ResourceLoader.Load<PackedScene>("res://props/unique/MindText.tscn");
        private MindTextList mindTextList;
        public int currentDoorId = -1;

        public override void _Ready()
        {
            GetNode<Timer>("MindTextTimer").Connect("timeout", this, nameof(CreateRandomizedMindText));
            mindTextList = new MindTextList();
            for (float i = 10; i > 0; i -= 0.5f)
            {
                MindText nn = CreateRandomizedMindText();
                nn.RemoveLiveTime(i);
            }
        }

        private MindText CreateRandomizedMindText()
        {
            Random rr = new Random();
            MindText nn = (MindText)propMindText.Instance();
            nn.SetValues(mindTextList.GetRandomMindText(), new Vector2(-30 + (float)rr.NextDouble() * 1054, -30 + (float)rr.NextDouble() * 630));
            GetNode<CanvasLayer>("CanvasLayer").AddChild(nn);
            return nn;
        }
    }
}