using UnityEngine;
using System.Collections;

public class CameraAutoSize : MonoBehaviour {
	public float widthToBeSeen = 12f;
	// Use this for initialization
	void Start () {
		Camera.main.orthographicSize = widthToBeSeen * Screen.height / Screen.width * 0.5f;
	}
	void Update()
	{
		Camera.main.orthographicSize = widthToBeSeen * Screen.height / Screen.width * 0.5f;
	}
}
