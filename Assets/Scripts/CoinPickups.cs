using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickups : MonoBehaviour
{
    [SerializeField] int scoreToAdd = 100;
    [SerializeField] AudioClip coinSFX;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource.PlayClipAtPoint(coinSFX, Camera.main.transform.position);
        FindObjectOfType<GameSession>().AddToScore(scoreToAdd);
        Destroy(gameObject);
    }
}
