using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
    private Player player;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void LateUpdate() {
        if (!player.gameOver) {
            if (player.jumping) {
                transform.Translate(Vector3.down * (player.jumpForceUp * Time.deltaTime));
            }
            else if (player.currentPosition > player.maxDistance - 3) {
                transform.Translate(Vector3.up * (player.wallSlidingSpeed * Time.deltaTime));
            }

            if (transform.position.y < -20) {
                Destroy(gameObject);
            }
        }
    }
}
