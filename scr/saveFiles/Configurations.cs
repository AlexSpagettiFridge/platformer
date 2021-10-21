using Godot;

namespace Game
{
    public struct Configurations
    {
        public Vector2 screenResolution;
        public bool screenFullscreen, screenPixelsnap;
        public float sfxMaster, sfxMusic, sfxSound;

        public static Configurations Default
        {
            get
            {
                Configurations defaultConfig = new Configurations();
                defaultConfig.screenResolution = new Vector2(1024, 600);
                defaultConfig.screenFullscreen = false;
                defaultConfig.screenPixelsnap = true;
                defaultConfig.sfxMaster = 0.5f;
                defaultConfig.sfxMusic = 0.5f;
                defaultConfig.sfxSound = 0.5f;
                return defaultConfig;
            }
        }

        public void Apply()
        {
            OS.WindowSize = screenResolution;
            OS.WindowFullscreen = screenFullscreen;
            ProjectSettings.SetSetting("rendering/quality/2d/use_pixel_snap", screenPixelsnap);
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"),sfxMaster);
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), sfxMusic);
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Sound"), sfxSound);
        }
    }
}