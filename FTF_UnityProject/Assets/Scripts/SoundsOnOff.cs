using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundsOnOff : MonoBehaviour {
	public Text SoundsOnOffButtonText;

	public static SoundsOnOff Instance;

	public int SoundsOn {
		get;
		private set;
	}

	void Start()
	{
		Instance = this;
		if(PlayerPrefs.HasKey("SoundsOn")) SoundsOn = PlayerPrefs.GetInt("SoundsOn");
		else
		{
			SoundsOn = 1;
			PlayerPrefs.SetInt("SoundsOn", SoundsOn);
		}
		SoundsOnOffButtonText.text = (SoundsOn == 1)? "Sounds On": "Sounds Off";
	}

	public void SwitchSounds()
	{
		SoundsOn = 1 - SoundsOn;
		PlayerPrefs.SetInt("SoundsOn", SoundsOn);
		SoundsOnOffButtonText.text = (SoundsOn == 1)? "Sounds On": "Sounds Off";
	}
}
