using Godot;

namespace Game
{
    public class InfoWindow : CanvasLayer
    {
        public override void _Process(float delta)
        {
            if (Input.IsActionJustPressed("gm_jump"))
            {
                QueueFree();
            }
        }

        public void SetText(string text)
        {
            GetNode<RichTextLabel>("Control/Panel/RichTextLabel").Text = text;
        }

        public void MakeLastWindow()
        {
            Connect("tree_exiting", this, nameof(FreePlayer));
        }

        private void FreePlayer()
        {
            GetNode<GameController>("/root/GameController").MainPlayer.ResumeNormalState();
        }
    }
}