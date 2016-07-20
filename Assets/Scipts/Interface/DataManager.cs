﻿using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager {

    public static void SaveBool(bool val)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = new FileStream(Application.persistentDataPath + "/level.dat", FileMode.OpenOrCreate);

        bf.Serialize(file, val);
        file.Close();
    }

    public static bool LoadBool()
    {
        if (File.Exists(Application.persistentDataPath + "/level.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/level.dat", FileMode.Open);
            bool val = (bool)bf.Deserialize(file);
            file.Close();
            return val;
        }
        else
        {
            return false;
        }
    }


    public static void SaveVars(float in_musicLevel, float in_effectsLevel, float in_sensetive, int in_qualityLevel, int in_crosshair, string nickname)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = new FileStream(Application.persistentDataPath + "/settings.dat", FileMode.OpenOrCreate);

        Settings settingsStruct = new Settings();
        settingsStruct.qualityLevel = in_qualityLevel;
        settingsStruct.musicLevel = in_musicLevel;
        settingsStruct.effectsLevel = in_effectsLevel;
        settingsStruct.nickname = nickname;
        settingsStruct.crosshair = in_crosshair;
        settingsStruct.mouseSensetive = in_sensetive;

        bf.Serialize(file, settingsStruct);
        file.Close();
    }

    public static Settings LoadVars()
    {
        if (File.Exists(Application.persistentDataPath + "/settings.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/settings.dat", FileMode.Open);
            Settings settingsStruct = (Settings)bf.Deserialize(file);
            file.Close();
            return settingsStruct;
        }
        else
        {
            Settings settingsStruct = new Settings();
            settingsStruct.effectsLevel = 1;
            settingsStruct.musicLevel = 1;
            settingsStruct.mouseSensetive = 1;
            settingsStruct.qualityLevel = 1;
            settingsStruct.nickname = "Player";
            settingsStruct.crosshair = 0;
            return settingsStruct;
        }
    }
}

[Serializable]
public class Settings
{
    public int qualityLevel;
    public int crosshair;
    public float musicLevel;
    public float effectsLevel;
    public float mouseSensetive;
    public string nickname;
}
