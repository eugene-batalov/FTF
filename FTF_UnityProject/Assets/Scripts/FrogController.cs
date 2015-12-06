using UnityEngine;
using System.Collections;

public class FrogController : MonoBehaviour {
	public Transform RotationPoint;
	public float RotationSpeed; 
	public Animator FrogAnimator;
	public AudioSource FrogAudioSource;

	void Start () {
		Level1Manager.ActiveObjectPointed += NextJump;
	}

	void Update () {
	}

	public void NextJump(GameObject go)
	{
		if(FrogAudioSource != null) FrogAudioSource.Play();
		StartCoroutine(RotateAndJump(go));
	}
	
	IEnumerator RotateAndJump(GameObject go)
	{
		var target = go.transform.position - RotationPoint.transform.position;
		var angle = Vector3.Angle(transform.up, target);
		if(FrogAnimator != null) FrogAnimator.Play("Rotate");

		yield return new WaitForSeconds(0.1f);
		if(RotationSpeed < 0.1) RotationSpeed = 1f;

//		Debug.Log(string.Format("go:{0}, target: {1}, t/15: {2}", go.transform.parent.name, target, target/15f));

		for(var i=0; i< (int) (angle / RotationSpeed); i++)
		{
			transform.RotateAround(RotationPoint.position, Vector3.Cross(transform.up, target), RotationSpeed);// вескторное произведение чтобы определить направление поворота
			yield return new WaitForSeconds(1f/30f); // частота кадров анимации поворота
		}
		if(FrogAnimator != null) FrogAnimator.Play("Jump");
		for(var i=0; i<15; i++) // количество кадров анимации прыжка
		{
			transform.Translate(target/Mathf.Pow(2f, i+1f), Space.World);
			yield return new WaitForSeconds(1f/30f); // частота кадров анимации прыжка
		}
		transform.SetParent(go.transform);
		transform.localPosition = Vector3.zero;
	}
}
