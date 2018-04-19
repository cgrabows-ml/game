using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Victory : MonoBehaviour {

    public Text damage;
    public Text slain;
    public Text spaceContinue;

	// Use this for initialization
	void Start () {
        damage.text = "Damage done: " + VictoryStats.damageDone.ToString();
        slain.text = "Enemies slain: " + VictoryStats.enemiesSlain.ToString();

        VictoryStats.damageDone = 0;

        IEnumerator coroutine = Continue();
        StartCoroutine(coroutine);
    }
	
    IEnumerator Continue()
    {
        yield return new WaitForSeconds(3);
        spaceContinue.gameObject.SetActive(true);
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
