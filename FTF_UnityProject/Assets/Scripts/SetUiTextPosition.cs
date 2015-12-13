using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetUiTextPosition : MonoBehaviour {
	public GameObject go;
	public Vector2 posit;
	public Vector2 viewpPoint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
		Vector2 pos = go.transform.position;  // get the game object position
		Vector2 viewportPoint = Camera.main.WorldToViewportPoint(pos);  //convert game object position to VievportPoint

		posit = pos;
		viewpPoint = viewportPoint;

		// set MIN and MAX Anchor values(positions) to the same position (ViewportPoint)
		((RectTransform)transform).anchorMin = viewportPoint;  
		((RectTransform)transform).anchorMax = viewportPoint; 

	}
}
