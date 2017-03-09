using UnityEngine;
using System.Collections;
using Leap.Unity;
using UnityEngine.VR;
/* Author: Mathew Tomberlin
 * CST306
 */
public class PlayerMovement : MonoBehaviour {
    private GameObject player;
    private Camera cam;
    private bool moving = false;
    private bool immobile = false;
    private Vector3 throttlePivot;
    private Quaternion throttleRotation;

    public bool VR = false;
    public GameObject LMH;
    public GameObject desktopDisplay;

    public float forwardSpeed;
    public float backwardSpeed;
    public float strafeSpeed;
    public float rotationSpeed;
    public float sensitivity = 0;
    public GameObject leftPalm = null;
    public GameObject leftArm = null;
    public GameObject rightPalm = null;
    public GameObject handsRoot = null;

    // Use this for initialization
    void Start () {
        cam = Camera.main;
        player = this.gameObject;// GameObject.Find("Player");
		//If V is pressed, toggle VRSettings.enabled
		VRSettings.enabled = VR;
        if (VR) {
            cam.enabled = false;
            desktopDisplay.SetActive(false);
            LMH.SetActive(true);
            
            LMH.GetComponentInChildren<Camera>().gameObject.layer = 4;
        } else {
            cam.enabled = true;
            desktopDisplay.SetActive(true);
            LMH.SetActive(false);

            handsRoot.transform.parent = desktopDisplay.transform;
        }
    }

    public void GrabThrottle()
    {
        if (!moving) {
            moving = true;
            throttlePivot = leftPalm.transform.localPosition;
            throttleRotation = leftPalm.transform.localRotation;
        }
    }

    public void ReleaseThrottle()
    {
        if (moving) {
            moving = false;
            throttlePivot = Vector3.zero;
            throttleRotation = Quaternion.identity;
        }
    }

    public void ConfirmSelection() {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, rightPalm.transform.position - cam.transform.position, out hit)) {
            if(hit.transform.GetComponentInParent<Useable>() != null) {
                hit.transform.GetComponentInParent<Useable>().Use();
            }
        }
    }

    // Update is called once per frame
    void Update () {
        //If the player is grabbing the virtual joystick and we're not immobile, get the horizontal and vertical axis clamped between -1 and 1, convert it to an absolute value to determine
        //the rate of change since last frame and then compare each axis to see if its greater than or less than +-0.01. If it is, move in the indicated direction (Can be forward/backward 
        //and strafe simultaneously)
        if (moving && !immobile) {
            float verticalAxis = Mathf.Clamp(throttlePivot.z - leftPalm.transform.localPosition.z, -0.1f, 0.1f) * 10;
            float horizontalAxis = Mathf.Clamp(throttlePivot.x - leftPalm.transform.localPosition.x, -0.1f, 0.1f) * 10;
            float deltaZ = Mathf.Abs(verticalAxis);
            float deltaX = Mathf.Abs(horizontalAxis);

            Vector3 speed = transform.TransformDirection(new Vector3((horizontalAxis > 0.01f) ? -strafeSpeed * deltaX : ((horizontalAxis < -0.01f) ? strafeSpeed * deltaX : 0), 0, (verticalAxis > 0.01f) ? -backwardSpeed * deltaZ : ((verticalAxis < -0.01f) ? forwardSpeed * deltaZ : 0)));

            player.GetComponent<CharacterController>().Move(speed * Time.deltaTime);
            float rawDeltaXRot = (throttleRotation.x - leftPalm.transform.localRotation.x);
            float rawDeltaYRot = (throttleRotation.y - leftPalm.transform.localRotation.y);
            float wristStiffnessOffsetX = 0;//Subtract if user's wrist is difficult bending inward, add if difficult bending outward;
            float signX = Mathf.Sign(rawDeltaXRot + wristStiffnessOffsetX);
            float signY = Mathf.Sign(rawDeltaYRot);
            float deltaXRot = Mathf.Abs(rawDeltaXRot + wristStiffnessOffsetX) * 4;
            float deltaYRot = Mathf.Abs(rawDeltaYRot + wristStiffnessOffsetX) * 4;
            float horizontalRotationAxis = Mathf.Pow(deltaXRot,0.9f)*signX;
            float verticalRotationAxis = Mathf.Pow(deltaYRot, 0.9f) * signY;
            float rotSpeed = (deltaXRot > deltaYRot) ? horizontalRotationAxis : verticalRotationAxis;
            if (!System.Single.IsNaN(rotSpeed) && Mathf.Abs(rotSpeed) > 0.6f) {
                player.transform.Rotate(0, rotSpeed * -rotationSpeed,0,Space.World);
            }
        }

        RaycastHit hit;
        if(Physics.Raycast(transform.position,rightPalm.transform.position-transform.position,out hit)) {
            if (hit.collider.transform.GetComponent<Useable>() != null) {
                hit.collider.transform.GetComponent<Useable>().StartCoroutine(hit.collider.transform.GetComponent<Useable>().Hover());
            }
        }

        //Always move the player down. So far we have not developed a jumping function as we have not perfected the use of gravity with this setup
        player.GetComponent<CharacterController>().Move(Vector3.down);
    }
}
