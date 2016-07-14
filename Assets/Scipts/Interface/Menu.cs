﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Menu : MonoBehaviour {

    public Texture background;
    public Texture menuBackground;

    NetworkManager networkManager;

    enum MenuStatus { General, SinglePlayer, Server, Client, MultiPlayer, Options, Sound, Graphics, Gameplay }
    MenuStatus menuStatus = MenuStatus.General;

    Rect[] buttons = new Rect[12];
    Rect backgroundRect;
    Rect menuBackgroundRect;

    int difficult = 1;
    int quality = 1;
    int crosshair = 0;
    int language = 0;
    float musicLevel = 1;
    float effectsLevel = 1;
    float sensetive = 1;
    int l_quality = 1;
    int l_crosshair = 0;
    int l_language = 0;
    float l_musicLevel = 1;
    float l_effectsLevel = 1;
    float l_sensetive = 1;
    string l_nickname;
    string nickname = "Player";
    string[] difficultSelection;
    string[] qualitySelection;
    string[] colorSelection;
    string[] langSelection;
    string host = "localhost";
    bool isDarkness;

    List<string> wordsList = new List<string>();

    public GUIStyle buttonStyle;

    void Start () {
        backgroundRect = new Rect(0, 0, Screen.width, Screen.height);

        float pixel = Screen.height / 10;

        menuBackgroundRect = new Rect(0, 0, pixel * 6, Screen.height);

        for (int i = 0; i < buttons.GetLength(0); i++)
        {
            buttons[i] = new Rect(pixel, pixel * i + pixel / 3, pixel * 4, pixel / 1.5F);
        }

        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        Settings settingsStruct = DataManager.LoadVars();
        musicLevel = settingsStruct.musicLevel;
        effectsLevel = settingsStruct.effectsLevel;
        quality = settingsStruct.qualityLevel;
        nickname = settingsStruct.nickname;
        crosshair = settingsStruct.crosshair;
        sensetive = settingsStruct.mouseSensetive;
        language = DataManager.LoadLang();

        l_musicLevel = musicLevel;
        l_effectsLevel = effectsLevel;
        l_quality = quality;
        l_sensetive = sensetive;
        l_crosshair = crosshair;
        if (nickname == null)
            nickname = "Player";
        l_nickname = nickname;
        l_language = language;

        LoadLanguage();
    }

    void LoadLanguage()
    {
        LanguageReader languageReader = networkManager.GetComponent<LanguageReader>();
        wordsList = new List<string>();
        //0
        wordsList.Add(languageReader.langDict[language]["singlePlayer"]);
        //1
        wordsList.Add(languageReader.langDict[language]["multiPlayer"]);
        //2
        wordsList.Add(languageReader.langDict[language]["options"]);
        //3
        wordsList.Add(languageReader.langDict[language]["exit"]);
        //4
        wordsList.Add(languageReader.langDict[language]["difficulty"]);
        //5
        wordsList.Add(languageReader.langDict[language]["play"]);
        //6
        wordsList.Add(languageReader.langDict[language]["darkness"]);
        //7
        wordsList.Add(languageReader.langDict[language]["back"]);
        //8
        wordsList.Add(languageReader.langDict[language]["server"]);
        //9
        wordsList.Add(languageReader.langDict[language]["client"]);
        //10
        wordsList.Add(languageReader.langDict[language]["drawing"]);
        //11
        wordsList.Add(languageReader.langDict[language]["sound"]);
        //12
        wordsList.Add(languageReader.langDict[language]["game"]);
        //13
        wordsList.Add(languageReader.langDict[language]["drawingQuality"]);
        //14
        wordsList.Add(languageReader.langDict[language]["accept"]);
        //15
        wordsList.Add(languageReader.langDict[language]["musicVolume"]);
        //16
        wordsList.Add(languageReader.langDict[language]["effectsVolume"]);
        //17
        wordsList.Add(languageReader.langDict[language]["nickname"]);
        //18
        wordsList.Add(languageReader.langDict[language]["crosshairColor"]);
        //19
        wordsList.Add(languageReader.langDict[language]["mouseSensitivity"]);
        //20
        wordsList.Add(languageReader.langDict[language]["create"]);
        //21
        wordsList.Add(languageReader.langDict[language]["connect"]);
        //22
        wordsList.Add(languageReader.langDict[language]["low"]);
        //23
        wordsList.Add(languageReader.langDict[language]["middle"]);
        //24
        wordsList.Add(languageReader.langDict[language]["high"]);
        //25
        wordsList.Add(languageReader.langDict[language]["easy"]);
        //26
        wordsList.Add(languageReader.langDict[language]["normal"]);
        //27
        wordsList.Add(languageReader.langDict[language]["hard"]);
        //28
        wordsList.Add(languageReader.langDict[language]["red"]);
        //29
        wordsList.Add(languageReader.langDict[language]["green"]);
        //30
        wordsList.Add(languageReader.langDict[language]["blue"]);
        //31
        wordsList.Add(languageReader.langDict[language]["lang"]);


        qualitySelection = new string[] { wordsList[22], wordsList[23], wordsList[24] };
        difficultSelection = new string[] { wordsList[25], wordsList[26], wordsList[27] };
        colorSelection = new string[] { wordsList[28], wordsList[29], wordsList[30] };

        langSelection = new string[languageReader.langDict.Count];
        for (int i = 0; i < langSelection.GetLength(0); i++)
        {
            langSelection[i] = languageReader.langDict[i]["name"];
        }
    }
	
	void OnGUI () {
        GUI.DrawTexture(backgroundRect, background);
        GUI.DrawTexture(menuBackgroundRect, menuBackground);
        switch (menuStatus)
        {
            case MenuStatus.General:
                if (GUI.Button(buttons[0], wordsList[0], buttonStyle))
                {
                    menuStatus = MenuStatus.SinglePlayer;
                }
                if (GUI.Button(buttons[1], wordsList[1], buttonStyle))
                {
                    menuStatus = MenuStatus.MultiPlayer;
                }
                if (GUI.Button(buttons[2], wordsList[2], buttonStyle))
                {
                    menuStatus = MenuStatus. Options;
                }
                if (GUI.Button(buttons[3], wordsList[3], buttonStyle))
                {
                    Application.Quit();
                }
                break;
            case MenuStatus.SinglePlayer:
                GUI.Label(buttons[0], wordsList[4]);
                difficult = GUI.SelectionGrid(buttons[1], difficult, difficultSelection, 3, buttonStyle);
                isDarkness = GUI.Toggle(buttons[2], isDarkness, wordsList[6]);
                if (GUI.Button(buttons[3], wordsList[5], buttonStyle))
                {
                    StartGame();
                    networkManager.StartHost();
                }
                if (GUI.Button(buttons[4], wordsList[7], buttonStyle))
                {
                    menuStatus = MenuStatus.General;
                }
                break;
            case MenuStatus.MultiPlayer:
                if (GUI.Button(buttons[0], wordsList[8], buttonStyle))
                {
                    menuStatus = MenuStatus.Server;
                }
                if (GUI.Button(buttons[1], wordsList[9], buttonStyle))
                {
                    menuStatus = MenuStatus.Client;
                }
                if (GUI.Button(buttons[2], wordsList[7], buttonStyle))
                {
                    menuStatus = MenuStatus.General;
                }
                break;
            case MenuStatus.Options:
                if (GUI.Button(buttons[0], wordsList[10], buttonStyle))
                {
                    menuStatus = MenuStatus.Graphics;
                }
                if (GUI.Button(buttons[1], wordsList[11], buttonStyle))
                {
                    menuStatus = MenuStatus.Sound;
                }
                if (GUI.Button(buttons[2], wordsList[12], buttonStyle))
                {
                    menuStatus = MenuStatus.Gameplay;
                }
                if (GUI.Button(buttons[3], wordsList[7], buttonStyle))
                {
                    menuStatus = MenuStatus.General;
                }               
                break;
            case MenuStatus.Graphics:
                GUI.Label(buttons[0], wordsList[13]);
                l_quality = GUI.SelectionGrid(buttons[1], l_quality, qualitySelection, 3, buttonStyle);
                if (GUI.Button(buttons[2], wordsList[14], buttonStyle))
                {
                    Submit();
                }
                if (GUI.Button(buttons[3], wordsList[7], buttonStyle))
                {
                    Back();
                }
                break;
            case MenuStatus.Sound:
                GUI.Label(buttons[0], wordsList[15]);
                l_musicLevel = GUI.HorizontalSlider(buttons[1], l_musicLevel, 0, 1);
                GUI.Label(buttons[2], wordsList[16]);
                l_effectsLevel = GUI.HorizontalSlider(buttons[3], l_effectsLevel, 0, 1);
                if (GUI.Button(buttons[4], wordsList[14], buttonStyle))
                {
                    Submit();
                }
                if (GUI.Button(buttons[5], wordsList[7], buttonStyle))
                {
                    Back();
                }
                break;
            case MenuStatus.Gameplay:
                GUI.Label(buttons[0], wordsList[17]);
                l_nickname = GUI.TextField(buttons[1], l_nickname);
                if (l_nickname.Length > 15)
                    l_nickname = l_nickname.Substring(0, 15);
                GUI.Label(buttons[2], wordsList[18]);
                l_crosshair = GUI.SelectionGrid(buttons[3], l_crosshair, colorSelection, 3, buttonStyle);
                GUI.Label(buttons[4], wordsList[19]);
                l_sensetive = GUI.HorizontalSlider(buttons[5], l_sensetive, 0, 2);
                GUI.Label(buttons[6], wordsList[31]);
                l_language = GUI.SelectionGrid(buttons[7], l_language, langSelection, 3, buttonStyle);
                if (GUI.Button(buttons[8], wordsList[14], buttonStyle))
                {
                    Submit();
                }
                if (GUI.Button(buttons[9], wordsList[7], buttonStyle))
                {
                    Back();
                }
                break;
            case MenuStatus.Server:
                GUI.Label(buttons[0], wordsList[4]);
                difficult = GUI.SelectionGrid(buttons[1], difficult, difficultSelection, 3, buttonStyle);
                isDarkness = GUI.Toggle(buttons[2], isDarkness, wordsList[6]);
                if (GUI.Button(buttons[3], wordsList[20], buttonStyle))
                {
                    StartGame();
                    networkManager.StartHost();
                }
                if (GUI.Button(buttons[4], wordsList[7], buttonStyle))
                {
                    menuStatus = MenuStatus.General;
                }
                break;
            case MenuStatus.Client:
                GUI.Label(buttons[0], "IP:");
                host = GUI.TextField(buttons[1], host);
                if (GUI.Button(buttons[2], wordsList[21], buttonStyle))
                {
                    Connect();
                }
                if (GUI.Button(buttons[3], wordsList[7], buttonStyle))
                {
                    menuStatus = MenuStatus.General;
                }
                break;
        }
	}

    void Back()
    {
        l_quality = quality;
        l_musicLevel = musicLevel;
        l_effectsLevel = effectsLevel;
        l_sensetive = sensetive;
        l_crosshair = crosshair;
        l_nickname = nickname;
        l_language = language;
        menuStatus = MenuStatus.Options;
    }

    void Submit()
    {
        quality = l_quality;
        musicLevel = l_musicLevel;
        effectsLevel = l_effectsLevel;
        sensetive = l_sensetive;
        crosshair = l_crosshair;
        nickname = l_nickname;
        if (language != l_language)
        {
            language = l_language;
            DataManager.SaveLang(language);
            LoadLanguage();
        }
        DataManager.SaveVars(musicLevel, effectsLevel, sensetive, quality, crosshair, nickname);
        menuStatus = MenuStatus.Options;
    }

    void StartGame()
    {
        DataManager.SaveBool(isDarkness, difficult);
    }

    void OnServerError()
    {
        menuStatus = MenuStatus.Server;
    }

    void OnClientError()
    {
        menuStatus = MenuStatus.Client;
    }

    void Connect()
    {
        networkManager.networkAddress = host;
        networkManager.StartClient();
    }

    void OnFailedToConnect(NetworkConnectionError error)
    {
        Debug.Log("Failed to connect: " + error.ToString());
        menuStatus = MenuStatus.General;
    }

    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        if (Network.isClient)
        {
            Debug.Log("Disconnected from server: " + info.ToString());
        }
        else
        {
            Debug.Log("Connections closed");
        }
        menuStatus = MenuStatus.General;
    }
}
