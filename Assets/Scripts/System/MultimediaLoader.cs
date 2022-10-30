using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class MultimediaLoader
{
    public static AudioClip GetClipFromDisk(string path)
    {
        if(!File.Exists(path))
        {
            throw new FileNotFoundException($"The file at \"{path}\" does not exist");
        }

        path = Path.GetFullPath(path);

        string fileExtension = Path.GetExtension(path).ToLower();

        AudioType audioType = fileExtension switch
        {
            ".wav" => AudioType.WAV,
            ".ogg" => AudioType.OGGVORBIS,
            _ => throw new NotSupportedException(),
        };

        using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + path, audioType);
        var operation = www.SendWebRequest();
        while (!www.isDone)
            Thread.Sleep(1); // Yes, this stops the main thread, but it's localhost so essentially just a sync file read

        if (www.result != UnityWebRequest.Result.Success)
            throw new FileLoadException();

        return DownloadHandlerAudioClip.GetContent(www);
    }

    public static Texture2D GetTextureFromDisk(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"The file at \"{path}\" does not exist");
        }

        path = Path.GetFullPath(path);

        string fileExtension = Path.GetExtension(path).ToLower();

        if (fileExtension != ".png" && fileExtension != ".jpg")
            throw new NotSupportedException();

        Texture2D tex = new(2, 2);
        tex.LoadImage(File.ReadAllBytes(path), true);

        return tex;
    }
}
