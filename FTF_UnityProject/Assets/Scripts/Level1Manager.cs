using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Level1Manager : MonoBehaviour
{
	public static Level1Manager Instance;
	public static Action<GameObject> ActiveObjectPointed;
	public static Action<GameObject> CorrectKuvshinkaPointed;
	public static Action NextTurn;
	public static bool Idle;
	public static bool LevelDown;

	public GameObject[] digits;
	public AudioSource Haha1;
	
	public Text LevelText;
	public Text LevelDownText;

	public Text RightWayText;
	public Text IdleText;
	public Text CurrentJumpsBeforeNextLevelText;
	public Text CurrentCorrectJumpsSerieText;

	public int CorrectJumpsBeforeNextLevel = 10;
	public int CurrentJumpsBeforeNextLevel;
	public int CurrentCorrectJumpsSerie;
	public int Level;

	ArrayList _rightWay;
	Dictionary<string, SpriteRenderer> _strelki;
	GameObject[] _kuvshinki;
	bool _showRightWay;
	bool _wasMistake;

	public void ShowRightWayOn ()
	{
		_showRightWay = true;
	}

	void Awake ()
	{
		Instance = this;
		_rightWay = new ArrayList ();
		ActiveObjectPointed += RightOrWrongKuvshinkaPointed;
		CurrentJumpsBeforeNextLevel = CorrectJumpsBeforeNextLevel;
		CurrentCorrectJumpsSerie = 0;
		Init ();
		Level = 1;
		_showRightWay = false;
		_wasMistake = false;
		foreach (var digit in digits) {
			digit.SetActive (false);
		}
	}

	void Init ()
	{
		_strelki = new Dictionary<string, SpriteRenderer> ();
		var strelkiArray = GameObject.FindGameObjectsWithTag ("strelka");
		_kuvshinki = GameObject.FindGameObjectsWithTag ("kuvshinka");
//		Debug.Log(_kuvshinki.Length + " kuvshinok added");

		foreach (var strelka in strelkiArray) {
//            Debug.Log("key: " + strelka.name + " added");
			_strelki.Add (strelka.name, strelka.GetComponent<SpriteRenderer> ());
		}
	}

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.Quit(); 
		LevelText.text = "Level " + Level;
		//	RightWayText.text = "Right Way: ";
		for (var i = _rightWay.Count - 1; i > -1; i--) {
			//        RightWayText.text += ((GameObject) _rightWay[i]).transform.parent.parent.name.Replace("kyvshinka", "") +
			//                             ((i == 0) ? "" : ", ");
			if (i <= 0)
				continue;
			var from = ((GameObject)_rightWay [i]).transform.parent.parent.name.Replace ("kyvshinka", "");
			var to = ((GameObject)_rightWay [i - 1]).transform.parent.parent.name.Replace ("kyvshinka", "");
			_strelki [@from + "-" + to].enabled = _showRightWay;
			digits [_rightWay.Count - 1 - i].GetComponent<SetUiTextPosition> ().go = ((GameObject)_rightWay [i - 1]).transform.parent.parent.gameObject;
			digits [_rightWay.Count - 1 - i].SetActive (_showRightWay);
		}
//	    IdleText.text = "Idle: " + Idle;
//		LevelDownText.text = "LevelDown: " + LevelDown;
//		CurrentJumpsBeforeNextLevelText.text = "CurrentJumpsBeforeNextLevel: " + CurrentJumpsBeforeNextLevel;
//		CurrentCorrectJumpsSerieText.text = "CurrentCorrectJumpsSerie: " + CurrentCorrectJumpsSerie;
	}

	public void LevelUp ()
	{
		Level++;
		CurrentJumpsBeforeNextLevel = CorrectJumpsBeforeNextLevel;
		CurrentCorrectJumpsSerie = 0;
		NextTurn ();
	}

	void Correct (GameObject go)
	{
		CorrectKuvshinkaPointed (go);
		_rightWay.RemoveAt (_rightWay.Count - 1);
		CurrentJumpsBeforeNextLevel--;
		CurrentCorrectJumpsSerie++;
		_wasMistake = false;
	}

	void Wrong ()
	{
		if (_wasMistake)
			_showRightWay = true;
		if (LevelDownText.gameObject.activeSelf)
			return;
		// ошибка после понижения уровня не в счет
		_wasMistake = true;
		//				Debug.Log("MISTAKE!");
		if (Haha1 != null && SoundsOnOff.Instance.SoundsOn == 1)
			Haha1.Play ();
		if (CurrentCorrectJumpsSerie < 5 && _rightWay.Count > 3) {
			LevelDownText.gameObject.SetActive (true);
			_showRightWay = true;
			Level--;
		}
		CurrentCorrectJumpsSerie /= 2;
	}

	void RightOrWrongKuvshinkaPointed (GameObject go)
	{
		if (!Idle) return;

		_showRightWay = false;

		Update ();

		if (go == (GameObject)_rightWay [0] || go == (GameObject)_rightWay [_rightWay.Count - 1]) //	Frog pointed
			return;
		
		if (go == (GameObject)_rightWay [_rightWay.Count - 2]) Correct (go);
		else Wrong ();
	}

	public void SetMyFrogPosition (GameObject go)
	{
		_rightWay.Add (go);
	}

	public void SetLeadingFrogPosition (GameObject go)
	{
		if (_rightWay.Count < 1)
			_rightWay.Add (go);
		else
			_rightWay.Insert (0, go);
	}

	public GameObject GetFreeKuvshinka ()
	{
		var freePlaces = _kuvshinki.Except(_rightWay.ToArray()).ToArray();
//		GameObject k;
//		do {
//			k = _kuvshinki [UnityEngine.Random.Range (0, _kuvshinki.Length)]; // выбрать случайную из списка кувшинок
//		} while(_rightWay.Contains (k)); // если 
		return (GameObject) freePlaces[UnityEngine.Random.Range (0, freePlaces.Length)];
	}
}
