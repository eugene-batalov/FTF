using UnityEngine;
using System.Collections;

public class DebugOnOff : MonoBehaviour {
	public GameObject[] DebugObjects;

	bool debugOn;

	void Start()
	{
		if(DebugObjects.Length > 0) debugOn = DebugObjects[0].activeSelf;
	}

	public void SwitchDebug()
	{
		debugOn = !debugOn;
		foreach(var go in DebugObjects) go.SetActive(debugOn);
	}
}
