using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Level1Manager : MonoBehaviour {
	public static Level1Manager Instance;
	public static Action<GameObject> ActiveObjectPointed;
	public static Action<GameObject> CorrectKuvshinkaPointed;
	public static Action NextTurn;
	public static bool Idle;
	public static bool LevelDown;

	public GameObject[] Kuvshinki;
	public AudioSource HAHA1;
	public Text LevelText;
	public Text LevelDownText;

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

	// Use this for initialization
	void Start () {
		Instance = this;
		rightWay = new ArrayList();
		ActiveObjectPointed += RightOrWrongKuvshinkaPointed;
		CurrentJumpsBeforeNextLevel = correctJumpsBeforeNextLevel;
		CurrentCorrectJumpsSerie = 0;
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
			Debug.Log("MISTAKE!");
			HAHA1.Play();
			if(CurrentCorrectJumpsSerie < 5 && rightWay.Count > 3) 
			{
				LevelDownText.gameObject.SetActive(true);
				LevelText.text = "Level " + (rightWay.Count - 2);
			}
			CurrentCorrectJumpsSerie = 0;
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
