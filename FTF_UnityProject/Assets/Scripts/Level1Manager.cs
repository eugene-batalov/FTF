using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;

public class Level1Manager : MonoBehaviour
{
    public static Level1Manager Instance;
    public static Action<GameObject> ActiveObjectPointed;
    public static Action<GameObject> CorrectKuvshinkaPointed;
    public static Action NextTurn;
    public static bool LevelDown;

    public bool Idle;

    public int EducationModeWrongAttempts = 3;

    public GameObject[] digits;
    public AudioSource Haha1;

    public Text LevelText;
    public Text LevelDownText;

    public Text RightWayText;
    public Text IdleText;
    public Text CurrentJumpsBeforeNextLevelText;
    public Text CurrentCorrectJumpsSerieText;

    public float AdsTimeout = 30f;
    public int CorrectJumpsBeforeNextLevel = 10;
    public int CurrentJumpsBeforeNextLevel;
    public int CurrentCorrectJumpsSerie;
    public int Level;

    ArrayList _rightWay;
    Dictionary<string, SpriteRenderer> _strelki;
    GameObject[] _kuvshinki;
    bool _showRightWay;
    bool _wasMistake;
    private InterstitialAd _interstitial;
    private AdRequest _request;

    public void ShowRightWayOn()
    {
        _showRightWay = true;
    }

    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        var adUnitId = "ca-app-pub-1989038349842297/1295232363";
#elif UNITY_IPHONE
        string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
#else
        string adUnitId = "unexpected_platform";
#endif
        // Initialize an InterstitialAd.
        _interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        _request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        _interstitial.LoadAd(_request);
        _timeOut = false;
    }

    void Awake()
    {
        Instance = this;
        _rightWay = new ArrayList();
        ActiveObjectPointed += RightOrWrongKuvshinkaPointed;
        CurrentJumpsBeforeNextLevel = CorrectJumpsBeforeNextLevel;
        CurrentCorrectJumpsSerie = 0;
        Init();
        Level = 1;
        _showRightWay = false;
        _wasMistake = false;
        foreach (var digit in digits)
        {
            digit.SetActive(false);
        }
        //RequestInterstitial();
    }

    void Init()
    {
        _strelki = new Dictionary<string, SpriteRenderer>();
        var strelkiArray = GameObject.FindGameObjectsWithTag("strelka");
        _kuvshinki = GameObject.FindGameObjectsWithTag("kuvshinka");
        Debug.LogWarning(string.Format("Level1Manager Init: {0} kuvshinok added", _kuvshinki.Length));

        foreach (var strelka in strelkiArray)
        {
            //            Debug.Log("key: " + strelka.name + " added");
            _strelki.Add(strelka.name, strelka.GetComponent<SpriteRenderer>());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        LevelText.text = "Level " + Level;
        //	RightWayText.text = "Right Way: ";
        for (var i = _rightWay.Count - 1; i >= 0; i--)
        {
            //        RightWayText.text += ((GameObject) _rightWay[i]).transform.parent.parent.name.Replace("kyvshinka", "") +
            //                             ((i == 0) ? "" : ", ");
            if (i <= 0)
                continue;
            var from = ((GameObject)_rightWay[i]).transform.parent.parent.name.Replace("kyvshinka", "");
            var to = ((GameObject)_rightWay[i - 1]).transform.parent.parent.name.Replace("kyvshinka", "");
            _strelki[@from + "-" + to].enabled = _showRightWay;
            //            digits[_rightWay.Count - 1 - i].GetComponent<SetUiTextPosition>().go = ((GameObject)_rightWay[i - 1]).transform.parent.parent.gameObject;
            //            digits[_rightWay.Count - 1 - i].SetActive(_showRightWay);
        }
    }

    public void LevelUp()
    {
        Level++;
        CurrentJumpsBeforeNextLevel = CorrectJumpsBeforeNextLevel;
        CurrentCorrectJumpsSerie = 0;
        NextTurn();
    }

    void Correct(GameObject go)
    {
        //       Debug.Log("Jump4Frog Level1Manager Correct");
        CorrectKuvshinkaPointed(go);
        _rightWay.RemoveAt(_rightWay.Count - 1);
        CurrentJumpsBeforeNextLevel--;
        CurrentCorrectJumpsSerie++;
        _wasMistake = false;
    }

    private bool _timeOut;
    IEnumerator ShowAds()
    {
        if (_timeOut) yield break;
        _timeOut = true;
        //        if (_interstitial.IsLoaded()) _interstitial.Show();
        //        _interstitial.LoadAd(_request);
        if (Advertisement.IsReady())
        {
            Debug.LogWarning("Start ADS!");
            Advertisement.Show();
        }
        yield return new WaitForSeconds(AdsTimeout);
        _timeOut = false;
    }

    void Wrong()
    {
        //        Debug.Log("Jump4Frog Level1Manager Wrong");
        //if (_wasMistake)
            _showRightWay = true;
        // ошибка после понижения уровня не в счет
        if (LevelDownText.gameObject.activeSelf)
            return;
        if (EducationModeWrongAttempts > 0)
        {
            EducationModeWrongAttempts--;
           // _showRightWay = true;
            return;
        }
        _wasMistake = true;
        if (Haha1 != null && SoundsOnOff.Instance.SoundsOn == 1)
            Haha1.Play();
        if (CurrentCorrectJumpsSerie < 5 && _rightWay.Count > 3)
        {
            LevelDownText.gameObject.SetActive(true);
            _showRightWay = true;
            Level--;
        }
        CurrentCorrectJumpsSerie /= 2;
        StartCoroutine(ShowAds());
    }

    void RightOrWrongKuvshinkaPointed(GameObject go)
    {
        if (!Idle) return;
        if (go == (GameObject)_rightWay[0] || go == (GameObject)_rightWay[_rightWay.Count - 1]) //	Frog pointed
            return;
        _showRightWay = false;
        Update();
        if (go == (GameObject)_rightWay[_rightWay.Count - 2]) Correct(go);
        else Wrong();
    }

    public void SetMyFrogPosition(GameObject go)
    {
        _rightWay.Add(go);
    }

    public void SetLeadingFrogPosition(GameObject go)
    {
        if (_rightWay.Count < 1)
            _rightWay.Add(go);
        else
            _rightWay.Insert(0, go);
    }

    public GameObject GetFreeKuvshinka()
    {
        var freePlaces = _kuvshinki.Except(_rightWay.ToArray()).ToArray();
        return (GameObject)freePlaces[UnityEngine.Random.Range(0, freePlaces.Length)];
    }
}
