using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMechanics : MonoBehaviour {

    public int time;
    int comboCounter = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        AddCombo();
	}

    void AddCombo()
    {
        //Everytime someone gets hit, cobmCounter++;
        //Reset comboCounter to 0 after a few miliseconds
    }
}
