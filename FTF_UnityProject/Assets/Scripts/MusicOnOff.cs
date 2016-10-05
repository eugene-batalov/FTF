using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MusicOnOff : MonoBehaviour {
	public AudioSource Music;
    public Sprite SpriteMusicOn;
    public Sprite SpriteMusicOff;

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
            gameObject.GetComponent<Image>().sprite = SpriteMusicOff;
		}
		if(musicOn == 1 && !Music.isPlaying)
		{
			Music.Play();
            gameObject.GetComponent<Image>().sprite = SpriteMusicOn;
        }
		PlayerPrefs.SetInt("MusicOn", musicOn);
	}
}
