using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTeclado : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
		{
            Debug.Log("pulsando a");
            RotateLeft();
		}

        if (Input.GetKeyDown(KeyCode.D))
        {
            RotateRight();
            Debug.Log("pulsando d");
        }

    }

    public void RotateLeft()
	{
        transform.Rotate(Vector3.up, 90, Space.Self);
    }

    public void RotateRight()
	{
        transform.Rotate(Vector3.up, -90, Space.Self);
    }
}
