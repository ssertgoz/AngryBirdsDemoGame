using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksDamage : MonoBehaviour
{
   public delegate void BlockDelegate(int durability);
   public static event BlockDelegate BrokeBlock;
   
   public int durability;
   public Sprite sprite_1;
   public Sprite sprite_2;
   public GameObject CurrentObject;

   private int instance_of_durability;
   private Collider Collider;
   private SpriteRenderer SpriteRenderer;
   private Rigidbody2D rigidBody;
   private Sprite CurrentObjectSprite;

   private void Start()
   {
      instance_of_durability = durability;
      rigidBody = GetComponent<Rigidbody2D>();
      SpriteRenderer = GetComponent<SpriteRenderer>();
      rigidBody.isKinematic = true;
   }
   
   
   
   
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
   

   private void OnCollisionExit2D(Collision2D other)
   {
      switch (other.collider.tag)
      {
         case "wood":
            durability -= 10;
            break;
         case "stone":
            durability -= 20;
            break;
         case "glass":
            durability -= 5;
            break;
         case "bird":
            durability -= 30;
            break;
      }

      if (durability <= 0)
      {
         //StartCoroutine(nameof(Count));
         BrokeBlock(instance_of_durability);
         CurrentObject.SetActive(false);
      }
      else if (durability <= instance_of_durability*40/100)
      {
         SpriteRenderer.sprite = sprite_1;
      }
      else if (durability <= instance_of_durability*70/100)
      {
         SpriteRenderer.sprite= sprite_2;
      }
   }
   
   IEnumerator Count()
   {
      for (int i = 0; i < 2; i++)
      {
         yield return new WaitForSeconds(1);
      }
      GetComponent<ParticleSystem>().Play();
      rigidBody.isKinematic = false;
      Collider.enabled = false;
      SpriteRenderer.enabled = false;
      rigidBody.simulated = false;
      
   }
}
