using UnityEngine;
using System.Collections;

public class PointerDownReceiver : MonoBehaviour {
	public GameObject GameObjectToSend;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PointerDownReceived()
	{
		Debug.Log("click");
		if(GameObjectToSend == null) GameObjectToSend = gameObject;
		if(Level1Manager.ActiveObjectPointed != null) Level1Manager.ActiveObjectPointed(GameObjectToSend);
	}
}
