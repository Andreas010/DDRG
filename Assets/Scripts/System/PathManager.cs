using System;
using System.IO;
using UnityEngine;

public static class PathManager
{
    public static string DEFAULT_PATH;

    public static void InitialiseDirectories()
    {
#if UNITY_WEBGL
        return;
#endif

        DEFAULT_PATH = Path.GetFullPath(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.ddrg/");

        if (!Directory.Exists(DEFAULT_PATH))
        {
            DirectoryInfo defaultPathInfo = Directory.CreateDirectory(DEFAULT_PATH);
            defaultPathInfo.Attributes |= FileAttributes.Hidden;
        }

        if (!Directory.Exists(GetBeatmapsFolder()))
            Directory.CreateDirectory(GetBeatmapsFolder());
    }

    public static string GetBeatmapsFolder() => DEFAULT_PATH + "Beatmaps/";
}

/*
 * FOLDER STRUCTURE FOR DDRG
 * -------------------------
 * .ddrg
 * |- Beatmaps
 * |  |- ef67abf0-NAME
 * |  |  |- meta.ddrb (name, author, difficulties)
 * |  |  |- easy.ddrc
 * |  |  |- hard.ddrc
 * |  |  |- impossible.ddrc
 * |  |  |- bg.png
 * |  |  |- splash.png
 * |  |  |- music.ogg
 * 
 * 
 * .ddrg -> Zip-Archive for a full beatmap
 * .ddrb -> Info of a ddrg beatmap (0xDDb7)
 * .ddrc -> Chart of a ddrg map (0xDDf7)
 */