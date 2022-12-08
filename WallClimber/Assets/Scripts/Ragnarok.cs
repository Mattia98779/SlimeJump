using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragnarok : MonoBehaviour
{
    public float speed;
    public GameObject back;
    public GameObject front;
    public float speed1;
    public float speed2;
    public float position1;
    public float position2;

    private Player player;
    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update() {
        if(!player.gameOver) {
            transform.Translate(Vector3.up * (Time.deltaTime * speed));
            if (transform.position.y < -17) {
                transform.position = new Vector3(0, -16, 0);
            }
        }

        position1 = Mathf.Sin(Time.realtimeSinceStartup) * speed1;
        position2 = Mathf.Cos(Time.realtimeSinceStartup) * speed2;
        
        back.transform.localPosition = Vector3.right * position1;
        front.transform.localPosition = Vector3.right * position2;
    }
}
