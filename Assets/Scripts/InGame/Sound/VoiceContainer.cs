using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VoiceContainer", menuName = "Scriptable Objects/VoiceContainer")]
public class VoiceContainer : ScriptableObject
{
    [Serializable]
    public class VoiceEntry
    {
        public string Word;
        public AudioClip Clip;
    }

    [SerializeField] private List<VoiceEntry> _voiceEntries = new();
    public IReadOnlyList<VoiceEntry> VoiceContainers => _voiceEntries;
}
