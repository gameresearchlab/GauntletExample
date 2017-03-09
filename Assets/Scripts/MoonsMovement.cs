using UnityEngine;
using System.Collections;

public class MoonsMovement : MonoBehaviour {
    GameObject player;
    public Vector3 distance;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(player.transform.position.x + distance.x, player.transform.position.y + 300, player.transform.position.z + distance.z);
	}
}
