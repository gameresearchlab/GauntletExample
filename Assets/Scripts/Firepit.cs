using UnityEngine;
using System.Collections;

public class Firepit : MonoBehaviour {
    public GameObject artifact1;
    public GameObject artifact2;
    public GameObject artifact3;
    public GameObject artifact4;
    public GameObject fire;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
	    if(Physics.Raycast(new Vector3(artifact1.transform.position.x,transform.position.y, artifact1.transform.position.z), artifact1.transform.forward,out hit)) {
            if(hit.collider.gameObject == this.gameObject) {
                if (Physics.Raycast(new Vector3(artifact2.transform.position.x, transform.position.y, artifact2.transform.position.z), artifact2.transform.forward, out hit)) {
                    if (hit.collider.gameObject == this.gameObject) {
                        if (Physics.Raycast(new Vector3(artifact3.transform.position.x, transform.position.y, artifact3.transform.position.z), artifact3.transform.forward, out hit)) {
                            if (hit.collider.gameObject == this.gameObject) {
                                if (Physics.Raycast(new Vector3(artifact4.transform.position.x, transform.position.y, artifact4.transform.position.z), artifact4.transform.forward, out hit)) {
                                    if (hit.collider.gameObject == this.gameObject) {
                                        fire.SetActive(true);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
	}
}
