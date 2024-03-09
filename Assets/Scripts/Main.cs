using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Main : MonoBehaviour
{
    [System.Obsolete]
    public static Vector3 getRandomPositionFromCameraView()
    {
        Vector2 screenSize = Camera.main.ScreenToWorldPoint(Vector2.zero);
        float xPos = UnityEngine.Random.RandomRange(-screenSize.x, screenSize.x);
        float yPos = UnityEngine.Random.RandomRange(-screenSize.y, screenSize.y);
        return new Vector3(xPos, yPos, 0);
    }

    [Range(5, 15)]
    public int numberOfBots = 5;
    public int heroMaxHealth = 5;
    public float heroInitialSpeedMultiplier = 3;

    public bool isEnemyAnimated = false;
    public string userName = "Vladimir";
    public GameObject prefabBot;
    public GameObject prefabHero;
    public GameObject prefabHealth;
    public GameObject tfBots;
    public GameObject imageHeroHealth;
    private GameObject _hero;
    string templateStringBotsScore;
    int _currentScore = 0;
    GameObject _uiBtnStart;
    GameObject _containerBots;
    GameObject _containerHealth;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        templateStringBotsScore = tfBots.GetComponent<Text>().text;
        
        SetUIVisibility(false);
        
        Debug.Log("> Main:Start -> prefabBot: " + prefabBot.ToString());

        _uiBtnStart = GameObject.Find("btnStart");
        _uiBtnStart.GetComponent<UnityEngine.UI.Button>()
            .onClick.AddListener(onButtonStartClick);
        
        _containerBots = GameObject.Find("Bots");
        _containerHealth = GameObject.Find("Health");
    }

    void StartGame() {
        _currentScore = 0;
        SetUIVisibility(true);
        _uiBtnStart.active = false;
        InitiateHero();
        InstantiateBots(numberOfBots, containerBots: _containerBots);
        InstantiateHealth(3, _containerHealth);
        SetBotsScore(0, maxScore: numberOfBots);
    }

    void EndGame() {
        Destroy(_hero);
        foreach (Transform child in _containerBots.transform) {
            Destroy(child.gameObject);
        }
        SetUIVisibility(false);
        _uiBtnStart.active = true;
    }

    void onButtonStartClick() {
        Debug.Log("> Main -> onButtonStartClick");
        StartGame();
    }

    void CreateBot(GameObject parent, Vector3 position)
    {
        GameObject bot = Instantiate(prefabBot);
        bot.transform.parent = parent.transform;
        bot.transform.position = position;
        bot.GetComponent<Bot>().eventDestroyed.AddListener(OnBotDestroy);
    }

    void OnBotDestroy()
    {
        Debug.Log("> Main -> OnBotDestroy");
        _currentScore = _currentScore + 1;
        SetBotsScore(_currentScore, maxScore: numberOfBots);
    }

    void OnHeroHitted()
    {
        float heroHealth = (float)_hero.GetComponent<Hero>().health;
        Debug.Log("> Main -> OnHeroHitted: heroHealth = " + heroHealth);
        float healthBarScale = heroHealth / (float)heroMaxHealth;
        Debug.Log("> Main -> OnHeroHitted: healthBarScale = " + healthBarScale);
        SetHeroHealthUI(new Vector3(healthBarScale, 1.0f, 1.0f));
        bool isHeroDead = heroHealth <= 0;
        Debug.Log("> Main -> OnHeroHitted: isHeroDead = " + isHeroDead);
        if (isHeroDead) {
            EndGame();
        }
    }

    void SetHeroHealthUI(Vector3 value) {
        imageHeroHealth.GetComponent<RectTransform>().localScale = value;
    }

    void SetUIVisibility(bool visible) {
        tfBots.active = visible;
        imageHeroHealth.active = visible;
    }

    void InitiateHero() {
        _hero = Instantiate(prefabHero);
        _hero.GetComponent<Hero>().speedMultiplier = heroInitialSpeedMultiplier;
        _hero.GetComponent<Hero>().health = heroMaxHealth;
        _hero.GetComponent<Hero>().eventHitted.AddListener(OnHeroHitted);
    }

    void InstantiateHealth(int count, GameObject parent)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 position = Main.getRandomPositionFromCameraView();
            GameObject go = Instantiate(prefabHealth);
            go.transform.parent = parent.transform;
            go.transform.position = position;
        }
    }

    void InstantiateBots(int numberOfBots, GameObject containerBots)
    {
        for (int i = 0; i < numberOfBots; i++)
        {
            Vector3 position = Main.getRandomPositionFromCameraView();
            CreateBot(containerBots, position);
        }
    }

    void SetBotsScore(int currentScore, int maxScore)
    {
        tfBots.GetComponent<Text>().text = templateStringBotsScore
            .Replace("%", maxScore.ToString())
            .Replace("$", currentScore.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("> Update -> Time.deltaTime: " + Time.deltaTime);
    }
}
