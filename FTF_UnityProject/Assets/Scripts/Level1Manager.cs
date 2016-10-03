using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.Advertisements;

public class Level1Manager : MonoBehaviour
{
    #region static
    public static Level1Manager Instance;
    public static Action<GameObject> ActiveObjectPointed;
    public static Action<GameObject> CorrectKuvshinkaPointed;
    public static Action NextTurn;
    #endregion
    #region inspector - значения по умолчанию заданые в сцене обновляют те, что заданы в скрипте
    public bool Idle;
    public int EducationModeWrongAttempts = 3;
    public GameObject[] digits;
    public AudioSource Haha1;
    public Text LevelText;
    public Animator LevelTextAnimator;
    public Text LevelDownText;
    public float AdsTimeoutSeconds = 30f;
    public int CorrectJumpsBeforeNextLevel = 10;
    public int CurrentJumpsBeforeNextLevel;
    public int MistakesBeforeLevelDown = 2;
    public int CurrentMistakes = 0;
    public int Level = 1;
    #endregion
    #region locals
    ArrayList _rightWay;
    Dictionary<string, SpriteRenderer> _strelki;
    GameObject[] _kuvshinki;
    bool _showRightWay = false;
    #endregion
    void Awake()
    {
        Instance = this;
        _rightWay = new ArrayList();
        ActiveObjectPointed += KuvshinkaPointed;
        ResetAttempts();
        Init();
        foreach (var digit in digits) digit.SetActive(false);
    }

    void Init()
    {
        _strelki = new Dictionary<string, SpriteRenderer>();
        var strelkiArray = GameObject.FindGameObjectsWithTag("strelka");
        _kuvshinki = GameObject.FindGameObjectsWithTag("kuvshinka");
        foreach (var strelka in strelkiArray) _strelki.Add(strelka.name, strelka.GetComponent<SpriteRenderer>()); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        LevelText.text = "Level " + Level;

        for (var i = _rightWay.Count - 1; i > 0; i--)
        {
            var from = ((GameObject)_rightWay[i]).transform.parent.parent.name.Replace("kyvshinka", "");
            var to = ((GameObject)_rightWay[i - 1]).transform.parent.parent.name.Replace("kyvshinka", "");
            _strelki[@from + "-" + to].enabled = _showRightWay; // показать/не показать стрелки которые входят в "правильные ходы"
        }
    }

    public void LevelUp()
    {
        LevelTextAnimator.Play("level_text");
        Level++;
        ResetAttempts();
        NextTurn();
    }

    public void LevelDown()
    {
        CurrentMistakes = 0;
        if (Level > 1)
        {
            LevelDownText.gameObject.SetActive(true);
            Level--;
        }
        ResetAttempts();
    }

    void ResetAttempts()
    {
        CurrentJumpsBeforeNextLevel = CorrectJumpsBeforeNextLevel + Level;
    }

    void Correct(GameObject go)
    {
        CorrectKuvshinkaPointed(go);
        _rightWay.RemoveAt(_rightWay.Count - 1);
        CurrentJumpsBeforeNextLevel--;
    }

    private bool _timeOut;
    IEnumerator ShowAds()
    {
        if (_timeOut) yield break;
        _timeOut = true;
        if (Advertisement.IsReady()) Advertisement.Show();
        yield return new WaitForSeconds(AdsTimeoutSeconds);
        _timeOut = false;
    }

    void Wrong()
    {
        _showRightWay = true;
        if (LevelDownText.gameObject.activeSelf) return; // ошибка после понижения уровня не в счет
        if (EducationModeWrongAttempts > 0)
        {
            EducationModeWrongAttempts--;
            return;
        }
        CurrentMistakes++;
        if (Haha1 != null && SoundsOnOff.Instance.SoundsOn == 1) Haha1.Play();
        if (CurrentMistakes > MistakesBeforeLevelDown) LevelDown();
        StartCoroutine(ShowAds());
        ResetAttempts();
    }

    void KuvshinkaPointed(GameObject go)
    {
        if (!Idle) return;
        if (go == (GameObject)_rightWay[0] || go == (GameObject)_rightWay[_rightWay.Count - 1]) //	One of Frogs pointed
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
