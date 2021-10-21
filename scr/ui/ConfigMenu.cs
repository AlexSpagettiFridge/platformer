using Godot;

namespace Game
{
    public class ConfigMenu : Control
    {
        private static Vector2[] availableResolutions;
        private Configurations configurations;

        public ConfigMenu()
        {
            configurations = Configurations.Default;
            availableResolutions = new Vector2[3];
            availableResolutions[0] = new Vector2(1600, 900);
            availableResolutions[1] = new Vector2(1024, 600);
            availableResolutions[2] = new Vector2(640, 480);
        }
        public override void _Ready()
        {
            OptionButton nResolutionButton = GetNode<OptionButton>("TabContainer/Display/LeftColumn/ResolutionContainer/ResolutionButton");
            Godot.Collections.Array resolutionItems = new Godot.Collections.Array();
            for (int i = 0; i < availableResolutions.Length; i++)
            {
                Vector2 v = availableResolutions[i];
                nResolutionButton.AddItem(v.x + "x" + v.y, i);
                if (v == configurations.screenResolution)
                {
                    nResolutionButton.Selected = i;
                }
            }
            nResolutionButton.GrabFocus();

            Button nScreenmodeButton = GetNode<Button>("TabContainer/Display/LeftColumn/ScreenmodeButton");
            nScreenmodeButton.Pressed = configurations.screenFullscreen;
        }

        private void OnAcceptButtonPressed()
        {
            configurations.Apply();
            GetNode<Button>("Panel/HBoxContainer/AcceptButton").Disabled = true;
        }

        private void OnSettingChanged(){
            GetNode<Button>("Panel/HBoxContainer/AcceptButton").Disabled = false;
        }

        private void OnBackButtonPressed()
        {
            QueueFree();
        }

        private void OnResolutionButtonItemSelected(int index)
        {
            configurations.screenResolution = availableResolutions[index];
            OnSettingChanged();
        }

        private void OnScreenmodeButtonToggled(bool pressed)
        {
            configurations.screenFullscreen = pressed;
            OnSettingChanged();
        }

        private void OnPixelsnapButtonToggled(bool pressed)
        {
            configurations.screenPixelsnap = pressed;
            OnSettingChanged();
        }

        private void OnMasterSliderValueChanged(float volume){
            configurations.sfxMaster = volume;
            OnSettingChanged();
        }

        private void OnMusicSliderValueChanged(float volume){
            configurations.sfxMusic = volume;
            OnSettingChanged();
        }

        private void OnSoundSliderValueChanged(float volume){
            configurations.sfxSound = volume;
            OnSettingChanged();
        }
    }
}