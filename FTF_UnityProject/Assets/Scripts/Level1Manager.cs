using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class Level1Manager : MonoBehaviour {
	public static Level1Manager Instance;
	public static Action<GameObject> ActiveObjectPointed;
	public static Action<GameObject> CorrectKuvshinkaPointed;
	public static Action NextTurn;
	public static bool Idle;
	public static bool LevelDown;


	public AudioSource Haha1;
	
	public Text LevelText;
	public Text LevelDownText;

	public Text RightWayText;
	public Text IdleText;
	public Text CurrentJumpsBeforeNextLevelText;
	public Text CurrentCorrectJumpsSerieText;

	public int CorrectJumpsBeforeNextLevel = 10;
	
	public int CurrentJumpsBeforeNextLevel {
		get;
		private set;
	}

	public int CurrentCorrectJumpsSerie {
		get;
		private set;
	}
    public int Level
    {
        get;
        private set;
    }

	ArrayList _rightWay;
    Dictionary<string, SpriteRenderer> _strelki;
	GameObject[] _kuvshinki;
    public bool _showRightWay;

    public void ShowRightWayOn()
    {
        _showRightWay = true;
    }
	void Start () {
		Instance = this;
		_rightWay = new ArrayList();
		ActiveObjectPointed += RightOrWrongKuvshinkaPointed;
		CurrentJumpsBeforeNextLevel = CorrectJumpsBeforeNextLevel;
		CurrentCorrectJumpsSerie = 0;
		Init();
	    Level = 1;
	    _showRightWay = false;
	}

	void Init()
	{
        _strelki = new Dictionary<string, SpriteRenderer>();
		var strelkiArray = GameObject.FindGameObjectsWithTag("strelka");
		_kuvshinki = GameObject.FindGameObjectsWithTag("kuvshinka");
//		Debug.Log(_kuvshinki.Length + " kuvshinok added");

		foreach(var strelka in strelkiArray)
		{
//            Debug.Log("key: " + strelka.name + " added");
			_strelki.Add(strelka.name, strelka.GetComponent<SpriteRenderer>());
		}
	}

	void Update()
	{
        LevelText.text = "Level " + Level;
		RightWayText.text = "Right Way: ";
	    for (var i = _rightWay.Count - 1; i > -1; i--)
	    {
	        RightWayText.text += ((GameObject) _rightWay[i]).transform.parent.parent.name.Replace("kyvshinka", "") +
	                             ((i == 0) ? "" : ", ");
	        if (i <= 0) continue;
            var from = ((GameObject)_rightWay[i]).transform.parent.parent.name.Replace("kyvshinka", "");
            var to = ((GameObject)_rightWay[i - 1]).transform.parent.parent.name.Replace("kyvshinka", "");
	        _strelki[@from+"-"+to].enabled = _showRightWay;
	    }
	    IdleText.text = "Idle: " + Idle;
//		LevelDownText.text = "LevelDown: " + LevelDown;
		CurrentJumpsBeforeNextLevelText.text = "CurrentJumpsBeforeNextLevel: " + CurrentJumpsBeforeNextLevel;
		CurrentCorrectJumpsSerieText.text = "CurrentCorrectJumpsSerie: " + CurrentCorrectJumpsSerie;
	}

	public void LevelUp()
	{
		Level++;
		CurrentJumpsBeforeNextLevel = CorrectJumpsBeforeNextLevel;
		NextTurn();
	}

	void RightOrWrongKuvshinkaPointed(GameObject go)
	{
		if(!Idle) return;
	    _showRightWay = false;
        Update();
		if(go == (GameObject) _rightWay[0] || go == (GameObject) _rightWay[_rightWay.Count-1]) 
		{
			Debug.Log("Frog pointed!");
			return;
		}
		if(go == (GameObject) _rightWay[_rightWay.Count-2])
		{
			CorrectKuvshinkaPointed(go);
			_rightWay.RemoveAt(_rightWay.Count-1);
			CurrentJumpsBeforeNextLevel--;
			CurrentCorrectJumpsSerie++;
		}
		else
		{
			if(LevelDownText.gameObject.activeSelf) return; // две ошибки подряд не считаем
			Debug.Log("MISTAKE!");
			if(Haha1 != null && SoundsOnOff.Instance.SoundsOn == 1)Haha1.Play();
			if(CurrentCorrectJumpsSerie < 5 && _rightWay.Count > 3) 
			{
				LevelDownText.gameObject.SetActive(true);
			    Level--;
			}
			CurrentCorrectJumpsSerie /= 2;
		}
	}

	public void SetMyFrogPosition(GameObject go)
	{
		_rightWay.Add(go);
	}

	public void SetLeadingFrogPosition(GameObject go)
	{
		if(_rightWay.Count < 1) _rightWay.Add(go);
		else _rightWay.Insert(0, go);
	}

	public GameObject GetFreeKuvshinka () {
		GameObject k;
		do
		{
			k = _kuvshinki[UnityEngine.Random.Range(0,_kuvshinki.Length)];
		}
		while(_rightWay.Contains(k));
		return k;
	}
}
