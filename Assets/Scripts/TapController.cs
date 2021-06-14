using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]

public class TapController : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnyPlayerDied;
    public static event PlayerDelegate OnPlayerScored;

    public float tapForce = 10;
    public Vector3 startPos;

    public GameObject fartAnimationObject;
    public AudioSource fartAudio1;
    public AudioSource fartAudio2;
    public AudioSource fartAudio3;
    public AudioSource scoreAudio;
    public AudioSource gameOverAudio;

    Rigidbody2D rigidbody;

    void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.simulated = false;

        fartAnimationObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!GameManager.Instance.GameOver)
            {
                StartCoroutine(PlayFartAnimation());
                PlayFlapAudio();
            }
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ScoreZone")
        {
            // Register score event
            OnPlayerScored(); // Event sent to GameManager
            // Sound
            scoreAudio.Play();
        }

        if (collision.gameObject.tag == "DeadZone")
        {
            rigidbody.simulated = false;
            // Register a dead event
            OnyPlayerDied(); // Event sent to GameManager
            // Play sound
            gameOverAudio.Play();
        }
    }

    void OnGameStarted()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.simulated = true;
    }

    void OnGameOverConfirmed()
    {
        transform.localPosition = startPos;
    }

    IEnumerator PlayFartAnimation()
    {
        fartAnimationObject.SetActive(true);
        fartAnimationObject.GetComponent<Animator>().Play("FartAnimation");
        yield return new WaitForSeconds(0.2f);
        fartAnimationObject.SetActive(false);
    }

    void PlayFlapAudio()
    {
        int rndNum = Random.Range(1, 4);
        switch (rndNum)
        {
            case 1:
                fartAudio1.Play();
                break;
            case 2:
                fartAudio2.Play();
                break;
            case 3:
                fartAudio3.Play();
                break;
        }
    }
}
