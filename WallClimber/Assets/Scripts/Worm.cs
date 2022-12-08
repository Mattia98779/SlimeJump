using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour
{
    public float speed;

    public Sprite[] sprites;
    public float animationSpeed;
    private SpriteRenderer sprite;
    private float timeCounter;
    // Start is called before the first frame update
    void Start() {
        timeCounter = 0;
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        timeCounter += Time.deltaTime * animationSpeed;
        sprite.sprite = sprites[((int)timeCounter) % sprites.Length]; 
        transform.Translate(Vector3.down * (speed * Time.deltaTime));
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("projectile"))
        {
            Destroy(this.gameObject);
        }
    }
}
