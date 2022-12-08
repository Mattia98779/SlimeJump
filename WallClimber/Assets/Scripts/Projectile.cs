using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 15;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up*Time.deltaTime*speed);
        if (this.transform.position.y>30)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetSpeed(float speed) {
        this.speed = speed;
    }
    
    
}
