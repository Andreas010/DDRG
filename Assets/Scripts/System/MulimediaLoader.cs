using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class MulimediaLoader
{
    public static AudioClip GetClipFromDisk(string path)
    {
        using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.WAV);
        return null;
    }
}
