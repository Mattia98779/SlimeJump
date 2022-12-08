using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private Player player;
    public Material backgroundMaterial;
    private static readonly int Pos = Shader.PropertyToID("_Pos");

    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void LateUpdate() {
        if (!player.gameOver) {
            float pos = backgroundMaterial.GetFloat(Pos);
            if (player.jumping) {
                backgroundMaterial.SetFloat(Pos, pos + player.jumpForceUp * Time.deltaTime);
            }
            else if (player.currentPosition > player.maxDistance - 3) {
                backgroundMaterial.SetFloat(Pos, pos - player.wallSlidingSpeed * Time.deltaTime);
            }

            if (transform.position.y < -20) {
                Destroy(gameObject);
            }
        }
    } 
}
