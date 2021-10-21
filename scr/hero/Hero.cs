using Godot;
using Game.HeroStates;
using System.Collections.Generic;

namespace Game
{
    public class Hero : PlatformerEntity
    {
        private Sprite nPupilSprite, nEyeSprite;
        private CollisionShape2D nEyeColShape;
        private float coyoteTime = 0, pastPathTime = 0, hatRotation = 0;
        private bool isDucking = false;
        private Vector2 moveDir, lookDirection;
        private List<HeroHat> nHats = new List<HeroHat>();
        private HeroState currentState;
        public int facing = 1;
        private AudioStreamPlayer2D nAudioPlayer;
        public override void _Ready()
        {
            Connect("tree_exiting", this, nameof(OnExitingTree));
            maxSpeed = 225;
            friction = 1050;
            nPupilSprite = GetNode<Sprite>("PupilSprite");
            nEyeSprite = GetNode<Sprite>("EyeSprite");
            nEyeColShape = GetNode<CollisionShape2D>("EyeColShape");
            nAudioPlayer = GetNode<AudioStreamPlayer2D>("AudioPlayer");
            ResumeNormalState();
        }
        public override void _Process(float delta)
        {
            if (Input.IsActionPressed("gm_left")) { facing = -1; }
            if (Input.IsActionPressed("gm_right")) { facing = 1; }
            lookDirection = (moveSpeed / maxSpeed * 2).Clamped(2);
            hatRotation = Mathf.Max(0, Mathf.Abs(hatRotation) - delta * 30) * Mathf.Sign(hatRotation);
            foreach (HeroHat hat in nHats)
            {
                hat.Wiggle(hatRotation);
            }
            nPupilSprite.Position = lookDirection;
        }

        public HeroHat CurrentHat
        {
            get
            {
                if (nHats.Count == 0)
                {
                    return null;
                }
                return nHats[nHats.Count - 1];
            }
        }

        public int HatCount
        {
            get { return nHats.Count; }
        }

        public void AddHat(int hatId)
        {
            CallDeferred(nameof(AddHatDeferred), hatId);
        }
        private void AddHatDeferred(int hatId)
        {
            HeroHat nHat = HeroHat.CreateHatById(hatId);
            nHat.SetOwner(this);
            if (CurrentHat == null)
            {
                AddChild(nHat);
            }
            else
            {
                CurrentHat.AddChild(nHat);
            }
            nHats.Add(nHat);
        }

        public void RemoveHat(HeroHat heroHat)
        {
            nHats.Remove(heroHat);
        }

        public HeroState CurrentState
        {
            get { return currentState; }
        }

        private void OnExitingTree()
        {
            if (Util.GetGameController(this).MainPlayer == this)
            {
                Util.GetGameController(this).DetachHero();
            }
        }

        public void Kill()
        {
            Util.GetGameController(this).LoseGame();
            QueueFree();
        }

        public void ResumeNormalState()
        {
            ChangeState(new StandardState());
        }

        public void ChangeState(HeroState newState)
        {
            if (currentState != null)
            {
                currentState.EndState();
                currentState.QueueFree();
            }
            currentState = newState;
            if (currentState != null)
            {
                newState.AddToHero(this);
            }
        }

        public void EnableEyes(bool enabled)
        {
            nEyeSprite.Visible = enabled;
            nPupilSprite.Visible = enabled;
            nEyeColShape.Disabled = !enabled;
        }

        public void PlaySound(AudioStream sound){
            nAudioPlayer.Stream = sound;
            nAudioPlayer.Play();
        }
    }
}