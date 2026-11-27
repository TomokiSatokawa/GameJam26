using CoreKit.Sound;
using UnityEngine;

namespace Common.Sound
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundSection _bgm;
        [SerializeField] private SoundSection _se;
        [SerializeField] private SoundSection _voice;

        public static IReadOnlySoundSection BGM;
        public static IReadOnlySoundSection SE;
        public static IReadOnlySoundSection Voice;

        private void Awake()
        {
            if (BGM != null)
            {
                Destroy(this.gameObject);
                return;
            }

            BGM = _bgm;
            SE = _se;
            Voice = _voice;

            DontDestroyOnLoad(this.gameObject);
        }

        private void Update()
        {
             _bgm.Update();
            _se.Update();
            _voice.Update();
        }
    }

}
