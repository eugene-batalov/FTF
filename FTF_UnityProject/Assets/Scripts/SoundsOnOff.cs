using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundsOnOff : MonoBehaviour {
    public Sprite SpriteSoundsOn;
    public Sprite SpriteSoundsOff;

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
        setSprite();
    }

	public void SwitchSounds()
	{
		SoundsOn = 1 - SoundsOn;
		PlayerPrefs.SetInt("SoundsOn", SoundsOn);
        setSprite();

    }
    void setSprite()
    {
        gameObject.GetComponent<Image>().sprite = (SoundsOn == 1) ? SpriteSoundsOn : SpriteSoundsOff;
    }
}
