using UnityEngine;
using System.Collections;

public class PointerDownReceiver : MonoBehaviour {

    public GameObject GameObjectToSend;

	public void PointerDownReceived()
	{
        if (!Level1Manager.Instance.Idle) return; // пока лягушка прыгает не принимаем нажатия
		if(GameObjectToSend == null) GameObjectToSend = gameObject;
		Level1Manager.ActiveObjectPointed(GameObjectToSend);
	}
}
