using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Sprite[] sprites;
    public float animationSpeed;
    private SpriteRenderer sprite;
    private float timeCounter;
    private GameObject player;
    // Start is called before the first frame update
    void Start() {
        timeCounter = 0;
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        timeCounter += Time.deltaTime * animationSpeed;
        sprite.sprite = sprites[((int)timeCounter) % sprites.Length]; 
        if (Vector3.Distance(this.transform.position, player.transform.position)< 5)
        {
            if (player.transform.position.x - this.transform.position.x < 0) {
                sprite.flipX = true;
            }
            else {
                sprite.flipX = false;
            }

            transform.Translate(  (player.transform.position - this.transform.position)  * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("projectile"))
        {
            Destroy(this.gameObject);
        }
    }
}
