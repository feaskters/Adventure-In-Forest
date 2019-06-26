using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotScripts : MonoBehaviour
{
    public ParticleSystem collectEffective;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().healthChange(1);
            Instantiate(collectEffective,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
