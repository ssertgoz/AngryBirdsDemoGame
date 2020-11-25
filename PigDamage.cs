using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigDamage : MonoBehaviour
{
    public  delegate void PigDelegate(int s);
    public static event PigDelegate PigDied;
        
    private SpriteRenderer CurrentPigSprite;
    public Sprite InjuredPigSprite;
    public Sprite DiedPigSprite;
    public GameObject CurrentPigObject;
    public int PowerOfPig = 10;

    private bool died = false;
    private Rigidbody2D rigidBody;
    private Collider2D Collider;
    private int birdCount = 0;
    private int WoodCount = 0;
    private int StoneCount = 0;
    private int GlassCount = 0;
    private int TotalCount = 0;


    
    private void OnEnable()
    {
        BirdsControl.WhenBirdFly += WhenBirdFly;
        GameManager.GamePaused += GamePaused;
        GameManager.GameContinou += GameContinou;
    }

    private void OnDisable()
    {
        BirdsControl.WhenBirdFly -= WhenBirdFly;
        GameManager.GamePaused -= GamePaused;
        GameManager.GameContinou -= GameContinou;
    }

    
    void GamePaused()
    {
        rigidBody.simulated = false;
    }
    void GameContinou()
    {
        rigidBody.simulated = true;
    }

    void WhenBirdFly()
    {
        rigidBody.isKinematic = false;
    }

    private void Start()
    {
        CurrentPigSprite = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider2D>();
        rigidBody.isKinematic = true;

    }
    
    void HitControl(string staff)
    {
        switch (staff)
        {
            case "wood":
                WoodCount += 1;
                break;
            case "stone":
                StoneCount += 1;
                break;
            case "glass":
                GlassCount += 1;
                break;
            case "bird":
                Debug.Log("girdi");
                birdCount += 10;
                break;
        }

        TotalCount = WoodCount * 1 + GlassCount * 1 + StoneCount * 2 +birdCount;
        if ((TotalCount > PowerOfPig) && !died )
        {
            died = !died;
            PigDied(PowerOfPig);
            CurrentPigSprite.sprite = DiedPigSprite;
            StartCoroutine(nameof(Count));
            
        }
        else if (TotalCount > 8 && !died)
        {
            CurrentPigSprite.sprite = InjuredPigSprite;
        }
    }

    IEnumerator Count()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1);
        }
        GetComponent<ParticleSystem>().Play();
        rigidBody.isKinematic = false;
        Collider.enabled = false;
        CurrentPigSprite.enabled = false;
        rigidBody.simulated = false;
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(1);
        }
        Kill();
    }
    void Kill()
    {
        CurrentPigObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {

        HitControl(collision2D.collider.tag);
        Debug.Log(TotalCount);
    }


}
