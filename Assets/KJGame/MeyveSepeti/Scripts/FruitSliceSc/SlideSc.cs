using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideSc : MonoBehaviour
{
    Rigidbody2D rg2d;
    CircleCollider2D circleCollider;
    TrailRenderer trail;
    SpriteRenderer sRenderer;
    
    private void Start()
    {
        MeyveSepeti_Sounds.aManager.FruitSceneSound(MeyveSepeti_Sounds.aManager.sounds[4]);

        rg2d = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        trail = GetComponent<TrailRenderer>();
        sRenderer = GetComponent<SpriteRenderer>();
    }

    Touch touch;
    Vector2 vec;
    
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            vec = touch.position;

            if(touch.phase == TouchPhase.Moved)
            {
                trail.enabled = true;
                sRenderer.enabled = true;
                circleCollider.enabled = true;
            }
            if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                trail.enabled = false;
                circleCollider.enabled = false;
                sRenderer.enabled = false;
            }

            rg2d.position = Camera.main.ScreenToWorldPoint(vec);

        }


      //  FingerFollow();
    }
    //public void FingerFollow()
    //{
    //    Vector3 touchPos = Input.GetTouch(0).position;
    //    touchPos.z = 0f;

    //    rg2d.position = Camera.main.ScreenToWorldPoint(touchPos);

        
    //}

}
