using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Renderer>().isVisible == false) {
            Debug.Log("> Bullet -> Destroy");
            Destroy(this.gameObject);
        }
    }

    public void SetVelocity(Vector2 direction) {
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("> Bullet -> OnTriggerEnter2D: " + collider.gameObject.name);
        Destroy(this.gameObject);
    }
}
