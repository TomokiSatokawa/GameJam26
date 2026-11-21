using CoreKit.Sound;
using UnityEngine;

namespace Common.Sound
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundSection _bgm;
        [SerializeField] private SoundSection _se;

        public static IReadOnlySoundSection BGM;
        public static IReadOnlySoundSection SE;

        private void Awake()
        {
            if (BGM != null)
            {
                Destroy(this.gameObject);
                return;
            }

            BGM = _bgm;
            SE = _se;

            DontDestroyOnLoad(this.gameObject);
        }
    }

}
