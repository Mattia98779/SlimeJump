using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class Player : MonoBehaviour {
    public TextMeshProUGUI highScoreText;
    private float highScore;
    public Rigidbody2D rb;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI startGameText;
    public TextMeshProUGUI scoreText;
    public Image logo;
    public GameObject attackPrefab;
    public bool canShoot;
    public Sprite[] sprites;
    public bool canChange;

    public float jumpForceUp;
    public float jumpForceSide;
    public int moveDirection;
    public float speed;
    public bool jumping;

    public float wallSlidingSpeed;
    public float maxDistance;
    public float currentPosition;

    public float score;

    
    public bool gameOver;
    public bool restart;
    public bool toStart = true;

    public LayerMask whatIsWall;

    public Transform frontCheck;
    public float checkRadius;

    public int health = 3;
    public bool canGetDmg = true;

    public float jumpForceUpMultiplayer=10;
    private SpriteRenderer sprite;
    private Material material;
    public ParticleSystem burstParticle;
    public ParticleSystem deathParticle;
    public float attackRange;
    public GameObject[] healthImage;
    public ParticleSystem trail;
    public ParticleSystem bloodParticle;
    
    private AudioSource audioSource;
    
    public List<AudioClip> jumpAudioClip;
    public List<AudioClip> attackAudioClip;
    public List<AudioClip> getDmgAudioClip;

    public Shake shake;


    // Start is called before the first frame update
    void Start()
    {
        //audioSource = this.GetComponent<AudioSource>();
        moveDirection = 1;
        gameOver = true;
        canShoot = true;
        canChange = true;
        sprite = GetComponent<SpriteRenderer>();
        material = sprite.material;
        if (PlayerPrefs.HasKey("highScore"))
        {
            highScoreText.SetText(PlayerPrefs.GetInt("highScore").ToString());
            highScore = PlayerPrefs.GetInt("highScore");
        }
        else
        {
            highScoreText.SetText("0" );
        }
    }

    // Update is called once per frame
    void Update() {
        if (toStart) {
            if (pressed()) {
                gameOver = false;
                toStart = false;
                startGameText.gameObject.SetActive(false);
                scoreText.gameObject.SetActive(true);
                logo.gameObject.SetActive(false);
            }
        }
        else
        {
            if (!gameOver) {
                
                if (hold() && !jumping){
                    jumpForceUp += Time.deltaTime * jumpForceUpMultiplayer;
                    if(jumpForceUp > 23) 
                        material.SetFloat("_N", 1);
                }else if (released() && !jumping) {
                    jump();
                    material.SetFloat("_N", 0);
                }else if (!jumping) {
                    currentPosition -= wallSlidingSpeed * Time.deltaTime;
                    if(currentPosition < maxDistance - 3)
                    {
                        GameOver();
                    }
                }
                else {
                    if (pressed() && canShoot) {
                        ParticleSystem p = Instantiate(burstParticle);
                        p.transform.position = transform.position;
                        Destroy(p.gameObject, 4);
                        Debug.Log("attacco!!");
                        AoeAttack();
                        //ProjectileAttack();
                        canShoot = false;
                        canChange = false;
                        sprite.sprite = sprites[4];
                        Invoke("SetCanChangeTrue", 0.2f);
                    }
                    jumpForceUp -= 40f * Time.deltaTime;
                    maxDistance += jumpForceUp * Time.deltaTime;
                    scoreText.text = ((int)maxDistance).ToString();
                    currentPosition = maxDistance;
                }

                if (math.abs(jumpForceUp) < 6 && canChange) {
                    sprite.sprite = sprites[2];
                }
            }
            else if(restart){
                if (pressed()) {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }
    }

    void SetCanChangeTrue() {
        canChange = true;
    }

    private void ProjectileAttack()
    {
        Instantiate(attackPrefab, this.transform.position + Vector3.up, Quaternion.Euler(0, 0, 0)).GetComponent<Projectile>().SetSpeed(Mathf.Clamp(jumpForceUp, 10, 30));
    }

    private void AoeAttack()
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("enemy"))
        {
            Vector3 awayFromPlayer = (enemy.transform.position - transform.position);
            if (awayFromPlayer.magnitude < attackRange)
            {
                var particle = Instantiate(bloodParticle);
                particle.transform.position = enemy.transform.position;
                Destroy(particle.gameObject, 2);
                Destroy(enemy);
            }
        }
        //audioSource.PlayOneShot(attackAudioClip[UnityEngine.Random.Range(0, attackAudioClip.Count)]);
    }

    private void jump() {
        jumping = true;
        
        rb.AddForce(Vector2.left *(moveDirection * jumpForceSide), ForceMode2D.Impulse);
        if (moveDirection > 0) {
            moveDirection = -1;
        }
        else {
            moveDirection = 1;
        }

        sprite.flipX = moveDirection == 1;
        sprite.sprite = sprites[1];
        
        //audioSource.PlayOneShot(jumpAudioClip[UnityEngine.Random.Range(0, jumpAudioClip.Count)]);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("wall") )
        {
            Debug.Log("colpito il muro");
            jumping = false;
            jumpForceUp = 20;
            canShoot = true;
            sprite.flipX = !sprite.flipX;
            sprite.sprite = sprites[0];

        }
        
    }

    private void GameOver() {
        StartCoroutine(shake.Shaking());
        var particle = Instantiate(deathParticle);
        particle.transform.position = transform.position;
        trail.gameObject.SetActive(false);
        health = 0;
        gameOver = true;
        Invoke(nameof(setRestartTrue), 0.5f);
        sprite.enabled = false;
        gameOverText.gameObject.SetActive(true);
        gameOverText.SetText(gameOverText.text+ "\nScore: " + (int)maxDistance);
        scoreText.gameObject.SetActive(false);

        if (highScore<(int)maxDistance)
        {
            PlayerPrefs.SetInt("highScore", (int)maxDistance);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("spike") || other.gameObject.CompareTag("enemy"))
        {
            Debug.Log("colpito");
            getDamage();
        } else if (other.gameObject.CompareTag("boss")) {
            GameOver();
        }
    }

    private void getDamage()
    {
        //audioSource.PlayOneShot(getDmgAudioClip[UnityEngine.Random.Range(0, getDmgAudioClip.Count)]);
        if (canGetDmg)
        {
            health = health - 1;
            if (health==0)
            {
                GameOver();
            }
            if(health <= 2 && health >= 0 ){
                StartCoroutine(shake.Shaking());
                healthImage[health].SetActive(false);
                material.SetFloat("_D", 1);
                Invoke(nameof(setDmgEffectOff), 1f);
                var particle = Instantiate(deathParticle);
                particle.transform.position = Camera.main.ScreenToWorldPoint(healthImage[health].transform.position);
                Destroy(particle.gameObject, 2);
            }
            canGetDmg = false;
            Invoke("setCanGetDmgTrue", 1);
        }
       
    }

    private void setCanGetDmgTrue() {
        canGetDmg = true;
    }

    private void setRestartTrue() {
        restart = true;
    }
    private void setDmgEffectOff() {
        material.SetFloat("_D", 0);
    }

    private bool pressed() {
        return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0);
    }
    private bool hold() {
        return Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0);
    }
    private bool released() {
        return Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0);
    }

}
