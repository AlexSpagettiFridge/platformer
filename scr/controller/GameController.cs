using Godot;
using System;
using System.Collections.Generic;

namespace Game
{
    public partial class GameController : Node2D
    {
        [Export]
        private int currentSaveId = 0;
        private static readonly PackedScene propInfoWindow = ResourceLoader.Load<PackedScene>("res://props/ui/InfoWindow.tscn");
        private int berries = 0, localTotalBerries = 0, scrolls = 0, localTotalScrolls = 0;
        private Camera2D nCamera = null;
        private bool isCameraPosOverwritten = false;
        private Vector2 cameraPosOverwrite = new Vector2();
        private Hero nHero = null;
        private GuiController guiController;
        private GeneralSaveGame generalSaveGame;
        private LocalSaveGame localSaveGame;
        private Dictionary<int, AudioStreamPlayer2D> loseAudioPlayerList = new Dictionary<int, AudioStreamPlayer2D>();
        private int loseAudioPlayerListNewId = 0;
        private float screenShakeTimer = 0, screenShakeStrength = 0;

        public GeneralSaveGame SaveGame{
            set => generalSaveGame = value;
        }

        public override void _Ready()
        {
            guiController = GetNode<GuiController>("Gui");
            nCamera = GetNode<Camera2D>("Camera");
            LoadTiledMap(generalSaveGame.currentMap);
        }

        public override void _Process(float delta)
        {
            if (!isCameraPosOverwritten)
            {
                if (nHero != null)
                {
                    nCamera.Position = nHero.Position;
                }
            }
            else
            {
                nCamera.Position = cameraPosOverwrite;
            }
            if (screenShakeTimer > 0)
            {
                screenShakeTimer -= delta;
                float shake = screenShakeStrength * Math.Min(screenShakeStrength * screenShakeTimer * 10, screenShakeStrength);
                Random rr = new Random();
                nCamera.Position += new Vector2((float)rr.NextDouble() * shake, (float)rr.NextDouble() * shake);
                nCamera.Rotation = Mathf.Deg2Rad((float)rr.NextDouble() * shake/30);
            }
            else
            {
                nCamera.Rotation = 0;
            }
            if (Input.IsActionJustPressed("gm_pause"))
            {
                AddChild(ResourceLoader.Load<PackedScene>("res://props/ui/PauseMenu.tscn").Instance());
            }
        }

        public void MarkEventAsPassed(String eventName)
        {
            generalSaveGame.activatedEvents.Add(eventName);
        }

        public bool DidEventPass(String eventName)
        {
            return generalSaveGame.activatedEvents.Contains(eventName);
        }

        public void SaveCurrentGame(int saveSpotId, bool isMarmalade = false)
        {
            generalSaveGame.saveSpotId = saveSpotId;
            generalSaveGame.isAtMarmalade = isMarmalade;
            generalSaveGame.Save();
            localSaveGame.Save();
        }

        public int Berries
        {
            get { return berries; }
        }

        public void CollectBerry(int id)
        {
            localSaveGame.berriesCollected.Add(id);
            generalSaveGame.BerryCounter++;
            berries++;
            guiController.UpdateBerryCounter();
        }

        public void CollectScroll(int id)
        {
            localSaveGame.scrollsCollected.Add(id);
            generalSaveGame.ScrollCounter++;
            scrolls++;
        }

        public int LocalTotalBerries
        {
            get { return localTotalBerries; }
        }

        public Hero MainPlayer
        {
            get { return nHero; }
        }

        public void LoseGame()
        {
            Timer nGameOverTimer = GetNode<Timer>("GameOverTimer");
            nGameOverTimer.Start();
            nGameOverTimer.Connect("timeout", this, nameof(Restart));
        }

        public void DetachHero()
        {
            nHero = null;
        }

        public void Restart()
        {
            GetTree().ReloadCurrentScene();
        }

        public void ForceCameraToPosition(Vector2 position)
        {
            cameraPosOverwrite = position;
            isCameraPosOverwritten = true;
        }

        public void UnforceCameraPosition()
        {
            isCameraPosOverwritten = false;
        }

        public void SaveCurrentProgress()
        {
            generalSaveGame.Save();
            localSaveGame.Save();
        }

        public InfoWindow ShowInfoWindow(string text, bool lastMessage = false)
        {
            InfoWindow nn = (InfoWindow)propInfoWindow.Instance();
            nn.SetText(text);
            AddChild(nn);
            if (lastMessage)
            {
                nn.MakeLastWindow();
            }
            return nn;
        }

        public void ScreenShake(float time, float strength)
        {
            screenShakeTimer = time;
            screenShakeStrength = strength;
        }

        public AudioStreamPlayer2D CreateLoseAudioSteamPlayerAt(AudioStream sound, Vector2 position)
        {
            AudioStreamPlayer2D nAudioPlayer = new AudioStreamPlayer2D();
            nAudioPlayer.Name = "LoseAudioPlayer";
            nAudioPlayer.Stream = sound;
            nAudioPlayer.Position = position;
            nAudioPlayer.Bus = "Sound";
            nAudioPlayer.Play();
            AddChild(nAudioPlayer);
            loseAudioPlayerList.Add(loseAudioPlayerListNewId, nAudioPlayer);
            Godot.Collections.Array arguments = new Godot.Collections.Array { loseAudioPlayerListNewId };
            loseAudioPlayerListNewId++;
            nAudioPlayer.Connect("finished", this, nameof(OnLoseAudioPlayerFinished), arguments);
            return nAudioPlayer;
        }

        private void OnLoseAudioPlayerFinished(int playerIndex)
        {
            loseAudioPlayerList[playerIndex].QueueFree();
            loseAudioPlayerList.Remove(playerIndex);
        }

        public Camera2D GameCamera{
            get => nCamera;
        }
    }
}