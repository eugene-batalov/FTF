using UnityEngine;
using System.Collections;
using System;

public class Level1Manager : MonoBehaviour {
	public static Level1Manager Instance;
	public static Action<GameObject> ActiveObjectPointed;

	// Use this for initialization
	void Start () {
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
