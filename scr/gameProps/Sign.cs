using Godot;
using System;

namespace Game
{
    public class Sign : Area2D
    {
        public override void _Ready()
        {
            Connect("body_entered",this,nameof(OnEnter));
            Connect("body_exited",this,nameof(OnLeave));
        }

        public void SetText(String text)
        {
            GetNode<Label>("CanvasLayer/Label").Text = text;
            GetNode<CanvasLayer>("CanvasLayer").Offset = Position;
        }

        public void SetStyle(int styleId){
            GetNode<Sprite>("Sprite").Frame = styleId;
        }

        public void OnEnter(Node body)
        {
            if (body is Hero){
                GetNode<Label>("CanvasLayer/Label").Visible = true;
            }
        }

        public void OnLeave(Node body)
        {
            if (body is Hero){
                GetNode<Label>("CanvasLayer/Label").Visible = false;
            }
        }
    }
}
