using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    public void Update()
    {
        transform.Rotate(new Vector3(20, 40, 60) * Time.deltaTime);
    }
}
