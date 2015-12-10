using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MusicOnOff : MonoBehaviour {
	public AudioSource Music;
	public Text MusicOnOffButtonText;

	int musicOn;

	void Start()
	{
		if(PlayerPrefs.HasKey("MusicOn")) musicOn = PlayerPrefs.GetInt("MusicOn");
		else
		{
			musicOn = 1;
			PlayerPrefs.SetInt("MusicOn", musicOn);
		}
		musicOn = 1 - musicOn;
		SwitchMusic();
	}

	public void SwitchMusic()
	{
		musicOn = 1 - musicOn;
		if(musicOn == 0 && Music.isPlaying)
		{
			Music.Stop();
			MusicOnOffButtonText.text = "Music Off";
		}
		if(musicOn == 1 && !Music.isPlaying)
		{
				Music.Play();
			MusicOnOffButtonText.text = "Music On";
		}
		PlayerPrefs.SetInt("MusicOn", musicOn);
	}
}
