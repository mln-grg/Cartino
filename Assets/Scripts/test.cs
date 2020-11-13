using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            Debug.Log("yes yes yes");
            PlayerHealth pc = collision.gameObject.GetComponent<PlayerHealth>();
            pc.TakeDamage(1000f);
        }
    }
}
