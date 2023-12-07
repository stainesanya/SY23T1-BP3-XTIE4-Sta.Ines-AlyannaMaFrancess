using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class AudioManager : Singleton<AudioManager>
    {
        public new static AudioManager instance;

        [SerializeField]
        private List<AudioClip> sounds = new List<AudioClip>();

        [SerializeField]
        private AudioSource audioSource;

        public enum PlayerSFX
        {
            Laser,
            Blip,
            LaserMini,
            Shotgun,
            Explosion
        }

        public override void Awake()
        {
            instance = this;
        }

        void Start()
        {
            foreach (string _a in System.Enum.GetNames(typeof(PlayerSFX)))
            {
                sounds.Add(Resources.Load<AudioClip>("sounds/" + _a));
            }
        }

        public void PlaySound(PlayerSFX _laser)
        {
            audioSource.PlayOneShot(sounds[(int)_laser]);
        }
    }
}
