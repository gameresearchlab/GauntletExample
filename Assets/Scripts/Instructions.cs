using UnityEngine;
using System.Collections;

public class Instructions : MonoBehaviour {
    float startTime;
    public float duration;
    public GameObject text;
	void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
	    if(Time.time - startTime > duration) {
            text.SetActive(false);
        }
	}
}
