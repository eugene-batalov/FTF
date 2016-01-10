using UnityEngine;
using System.Collections;

public class PointerDownReceiver : MonoBehaviour {

    public GameObject GameObjectToSend;

	public void PointerDownReceived()
	{
        Debug.Log("Jump4Frog PointerDownReceiver PointerDownReceived");
		if(GameObjectToSend == null) GameObjectToSend = gameObject;
		if(Level1Manager.ActiveObjectPointed != null) Level1Manager.ActiveObjectPointed(GameObjectToSend);
	}
}
