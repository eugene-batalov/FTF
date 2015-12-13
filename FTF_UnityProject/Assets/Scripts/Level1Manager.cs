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


	public AudioSource HAHA1;
	
	public Text LevelText;
	public Text LevelDownText;

	public Text RightWayText;
	public Text IdleText;
	public Text CurrentJumpsBeforeNextLevelText;
	public Text CurrentCorrectJumpsSerieText;

	public int correctJumpsBeforeNextLevel = 10;
	
	public int CurrentJumpsBeforeNextLevel {
		get;
		private set;
	}

	public int CurrentCorrectJumpsSerie {
		get;
		private set;
	}

	ArrayList rightWay;
	Dictionary<string,GameObject> Strelki;
	GameObject[] Kuvshinki;

	// Use this for initialization
	void Start () {
		Instance = this;
		rightWay = new ArrayList();
		ActiveObjectPointed += RightOrWrongKuvshinkaPointed;
		CurrentJumpsBeforeNextLevel = correctJumpsBeforeNextLevel;
		CurrentCorrectJumpsSerie = 0;
		Init();
	}

	void Init()
	{
		Strelki = new Dictionary<string, GameObject>();
		GameObject[] strelki_array = GameObject.FindGameObjectsWithTag("strelka");
		Kuvshinki = GameObject.FindGameObjectsWithTag("kuvshinka");
		Debug.Log(Kuvshinki.Length + " kuvshinok added");

		foreach(var strelka in strelki_array)
		{
			Strelki.Add(strelka.name, strelka);
		}
	}

	void Update()
	{
		RightWayText.text = "Right Way: ";
		for(var i = rightWay.Count-1; i > -1; i--)
			RightWayText.text += ((GameObject)rightWay[i]).transform.parent.parent.name.Replace("kyvshinka","") + ((i == 0)? "" : ", ");
		IdleText.text = "Idle: " + Idle;
//		LevelDownText.text = "LevelDown: " + LevelDown;
		CurrentJumpsBeforeNextLevelText.text = "CurrentJumpsBeforeNextLevel: " + CurrentJumpsBeforeNextLevel;
		CurrentCorrectJumpsSerieText.text = "CurrentCorrectJumpsSerie: " + CurrentCorrectJumpsSerie;
	}

	public void LevelUp()
	{
		LevelText.text = "Level " + (rightWay.Count - 1);
		CurrentJumpsBeforeNextLevel = correctJumpsBeforeNextLevel;
		NextTurn();
	}

	void RightOrWrongKuvshinkaPointed(GameObject go)
	{
		if(!Idle) return;
		if(go == rightWay[0] || go == rightWay[rightWay.Count-1]) 
		{
			Debug.Log("Frog pointed!");
			return;
		}
		if(go == rightWay[rightWay.Count-2])
		{
			CorrectKuvshinkaPointed(go);
			rightWay.RemoveAt(rightWay.Count-1);
			CurrentJumpsBeforeNextLevel--;
			CurrentCorrectJumpsSerie++;
		}
		else
		{
			if(LevelDownText.gameObject.activeSelf) return; // две ошибки подряд не считаем
			Debug.Log("MISTAKE!");
			if(HAHA1 != null && SoundsOnOff.Instance.SoundsOn == 1)HAHA1.Play();
			if(CurrentCorrectJumpsSerie < 5 && rightWay.Count > 3) 
			{
				LevelDownText.gameObject.SetActive(true);
				LevelText.text = "Level " + (rightWay.Count - 3);
			}
			CurrentCorrectJumpsSerie /= 2;
		}
	}

	public void SetMyFrogPosition(GameObject go)
	{
		rightWay.Add(go);
	}

	public void SetLeadingFrogPosition(GameObject go)
	{
		if(rightWay.Count < 1) rightWay.Add(go);
		else rightWay.Insert(0, go);
	}

	public GameObject GetFreeKuvshinka () {
		GameObject k;
		do
		{
			k = Kuvshinki[UnityEngine.Random.Range(0,Kuvshinki.Length)];
		}
		while(rightWay.Contains(k));
		return k;
	}
}
