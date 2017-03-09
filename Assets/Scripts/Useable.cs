using UnityEngine;
using System.Collections;

public class Useable : MonoBehaviour {
    public Material originalMaterial;
    public Material glowingMaterial;
    bool rotating = false;
    bool hover = false;

    public virtual void Use() {
        Debug.Log("Using " + gameObject.name);

        if (!rotating)
            StartCoroutine(Rotate());
    }

    public IEnumerator Rotate() {
        Vector3 rotateDir = transform.right;
        rotating = true;
        while (Vector3.Angle(transform.forward,rotateDir) > 0.5f) {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotateDir), Time.deltaTime*2);
            yield return new WaitForEndOfFrame();
        }
        rotating = false;
    }

    public IEnumerator Hover() {
        if (!hover) {
            hover = true;
            float duration = 0.2f;
            while (duration > 0f) {
                duration -= Time.deltaTime;
                Renderer[] renderers = GetComponentsInChildren<Renderer>();
                foreach(Renderer r in renderers) {
                    r.material = glowingMaterial;
                }
                yield return new WaitForEndOfFrame();
            }
            hover = false;
            Renderer[] rndrs = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rndrs) {
                r.material = originalMaterial;
            }
        }
    }
}
