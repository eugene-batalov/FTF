using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

    void Start()
    {
        Time.timeScale = 0.0f;
        Level1Manager.Instance.Idle = false;
    }

     public void Go () {
        Time.timeScale = 1.0f;
        Level1Manager.NextTurn();
        gameObject.SetActive(false);
    }
}
