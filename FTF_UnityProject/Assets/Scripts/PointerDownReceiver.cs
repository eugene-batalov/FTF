using UnityEngine;
using System.Collections;

public class PointerDownReceiver : MonoBehaviour {

    public GameObject GameObjectToSend;

	public void PointerDownReceived()
	{
        if (!Level1Manager.Instance.Idle) return; // пока лягушка прыгает не принимаем нажатия
        Debug.Log("Jump4Frog PointerDownReceiver PointerDownReceived");
		if(GameObjectToSend == null) GameObjectToSend = gameObject;
		if(Level1Manager.ActiveObjectPointed != null) Level1Manager.ActiveObjectPointed(GameObjectToSend);
	}
}
