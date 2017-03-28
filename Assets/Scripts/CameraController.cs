using UnityEngine;

public class CameraController : MonoBehaviour {

    void Update()
    {
        var x = Input.GetAxis("Horizontal") * 0.3f;
        var z = Input.GetAxis("Vertical") * 0.3f;

        transform.Translate(x, 0, z);
    }
}
