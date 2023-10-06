using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSensor : MonoBehaviour
{
    public bool sensed = true;

    void OnCollisionStay2D(Collision2D other)
    {
        if ((other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("Block")))
        {
            sensed = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("Block"))
        {
            sensed = false;
        }
    }
}
