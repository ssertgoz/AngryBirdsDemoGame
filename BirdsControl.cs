using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdsControl : MonoBehaviour
{
    public delegate void BirdFly();
    public static event BirdFly WhenBirdFly;
    public static event BirdFly GamePause;
    public static event BirdFly BirdDied;
    
    
    
    public bool ClickedOn;
    public LineRenderer CatapultBandFront;
    public LineRenderer CatapultBandBack;
    public Sprite BirdOnAir;
    public Sprite BirdWhenHit;
    public Sprite BirdWhenHitButton;
    public GameObject CurrentBird;
    
    private bool BirdDiedBool = false;
    private CircleCollider2D circleCollider;
    private SpriteRenderer BirdSprite;
    private SpringJoint2D spring;
    private Rigidbody2D rigidBody;
    private Vector2 preVelocity;
    private Ray CatapultRay;
    private float circleRadius;
    private Transform catapult;
    private Ray rayToMouse;


    private void OnEnable()
    {
        GameManager.GamePaused += GamePaused;
        GameManager.GameContinou += GameContinou;
    }

    private void OnDisable()
    {
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
    


    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        CatapultRay = new Ray(CatapultBandFront.transform.position,Vector3.zero);
        circleCollider = GetComponent<CircleCollider2D>();
        circleRadius = circleCollider.radius;
        rayToMouse = new Ray(catapult.position,Vector3.zero);
        BirdSprite = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        spring = GetComponent<SpringJoint2D>();
        catapult = spring.connectedBody.transform;
    }

    void LineRendererSetup()
    {
        CatapultBandBack.SetPosition(0,CatapultBandBack.transform.position);
        CatapultBandFront.SetPosition(0,CatapultBandFront.transform.position);
    }

    void LineRendereUpdate()
    {
        Vector2 catapultToProjectile = transform.position - CatapultBandFront.transform.position;
        CatapultRay.direction = catapultToProjectile;
        Vector3 birdposition = CatapultRay.GetPoint(catapultToProjectile.magnitude+circleRadius);
        CatapultBandBack.SetPosition(1,birdposition);
        CatapultBandFront.SetPosition(1,birdposition);
    }

    private void OnMouseDown()
    {
        ClickedOn = true;
        GetComponent<Rigidbody2D>().isKinematic = true;
        
    }

    private void OnMouseUp()
    {
        GetComponent<Rigidbody2D>().isKinematic = false;
        
        ClickedOn = false;
    }

    void Dragging()
    {
        Vector3 MouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 catapulToMouse = MouseWorldPoint - catapult.position;
        if (catapulToMouse.sqrMagnitude > 9)
        {
            rayToMouse.direction = catapulToMouse;
            MouseWorldPoint = rayToMouse.GetPoint(3f); 
        }
        MouseWorldPoint.z = 0f;
        transform.position = MouseWorldPoint;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        BirdSprite.sprite = BirdWhenHit;
        BirdDiedBool = true;
    }

    void Pause()
    {
        rigidBody.isKinematic = false;
        circleCollider.enabled = false;
        BirdSprite.enabled = false;
        rigidBody.simulated = false;
        BirdDiedBool = false;
    }

    void BirdDiedControl()
    {
        if (BirdDiedBool && (rigidBody.velocity.sqrMagnitude < 0.003f))
        {
            StartCoroutine(nameof(Count));
        }
    }
    

    void Kill()
    {
        BirdDied();
        CurrentBird.SetActive(false);
    }
    
    IEnumerator Count()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1);
        }
        GetComponent<ParticleSystem>().Play();
        rigidBody.isKinematic = false;
        circleCollider.enabled = false;
        BirdSprite.enabled = false;
        rigidBody.simulated = false;
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(1);
        }
        Kill();
    }
    

    void DraggingControl()
    {
        if (ClickedOn == true)
        {
            transform.rotation = Quaternion.Euler(0,0,0);
            Dragging();
        }

        if (!rigidBody.isKinematic && (preVelocity.sqrMagnitude > rigidBody.velocity.sqrMagnitude) && (spring != null))
        {
            Destroy(spring);
            BirdSprite.sprite = BirdOnAir;
            rigidBody.velocity = preVelocity;
            if (WhenBirdFly != null)
            {
                WhenBirdFly();
            }
        }

        if (spring == null)
        {
            CatapultBandBack.enabled = false;
            CatapultBandFront.enabled = false;
        }

        
        LineRendereUpdate();
        preVelocity = rigidBody.velocity;
    }

    void Update()
    {
        DraggingControl();
        BirdDiedControl();
        
        
    }
}
