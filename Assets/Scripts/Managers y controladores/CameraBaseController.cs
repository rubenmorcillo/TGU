using UnityEngine;

public class CameraBaseController : MonoBehaviour
{
    // Start is called before the first frame update

    public Camera cam;

    public float horizontalSpeed;
    public float verticalSpeed;

    float h;
    float v;
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        h = horizontalSpeed * Input.GetAxis("Mouse X");
        v = verticalSpeed * Input.GetAxis("Mouse Y");

        transform.Rotate(0, h, 0);
        cam.transform.Rotate(-v, 0, 0);
    }
}
