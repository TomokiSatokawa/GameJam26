//AI
using System;
using System.Collections.Generic;
using System.Threading;
using Common.Sound;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace InGame.Order
{
    /// <summary>
    /// 文字列を単語ごとのVoice Clipに分解し、順番に再生してコールを読み上げる
    /// </summary>
    public class CallVoice : MonoBehaviour
    {

        [SerializeField] private float _clipInterval = 0.05f;

        private readonly Dictionary<string, AudioClip> _clipMap = new();
        private CancellationTokenSource _cts;
        private int _maxWordLength;
        private VoiceContainer _currentVoice;

        private void Awake()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            CancelCurrent();
        }

        private void Initialize()
        {
            foreach (var entry in _currentVoice.VoiceContainers)
            {
                if (string.IsNullOrEmpty(entry.Word) || entry.Clip == null)
                {
                    Debug.LogError($"[CallVoice] Voice設定が不正です。Word: '{entry.Word}', Clip: {entry.Clip}");
                    continue;
                }

                _clipMap[entry.Word] = entry.Clip;
                _maxWordLength = Mathf.Max(_maxWordLength, entry.Word.Length);
            }
        }

        /// <summary>
        /// コールを読み上げる。再生中の場合は中断して最初から再生する
        /// </summary>
        public void Call(string text)
        {
            CancelCurrent();
            _cts = new CancellationTokenSource();
            CallAsync(text, _cts.Token).Forget();
        }

        /// <summary>
        /// コールを読み上げ、再生完了まで待機する
        /// </summary>
        public async UniTask CallAsync(string text, CancellationToken token)
        {
            var clips = ParseClips(text);

            foreach (var clip in clips)
            {
                SoundManager.SE.PlayOneShot(clip,1f);
                await UniTask.Delay(TimeSpan.FromSeconds(clip.length + _clipInterval), cancellationToken: token);
            }
        }

        /// <summary>
        /// 再生中のコールを中断する
        /// </summary>
        public void CancelCurrent()
        {
            if (_cts == null)
            {
                return;
            }

            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }

        private List<AudioClip> ParseClips(string text)
        {
            var clips = new List<AudioClip>();
            if (string.IsNullOrEmpty(text))
            {
                return clips;
            }

            var index = 0;
            while (index < text.Length)
            {
                if (TryMatchWord(text, index, out var word, out var clip))
                {
                    clips.Add(clip);
                    index += word.Length;
                    continue;
                }

                if (!char.IsWhiteSpace(text[index]))
                {
                    Debug.LogError($"[CallVoice] 対応するVoiceがありません。文字: '{text[index]}', text: '{text}'");
                }

                index++;
            }

            return clips;
        }

        // 長い単語を優先してマッチさせる(「マシ」より「マシマシ」を登録した場合に対応するため)
        private bool TryMatchWord(string text, int index, out string word, out AudioClip clip)
        {
            var maxLength = Mathf.Min(_maxWordLength, text.Length - index);
            for (var length = maxLength; length >= 1; length--)
            {
                word = text.Substring(index, length);
                if (_clipMap.TryGetValue(word, out clip))
                {
                    return true;
                }
            }

            word = null;
            clip = null;
            return false;
        }
    }
}