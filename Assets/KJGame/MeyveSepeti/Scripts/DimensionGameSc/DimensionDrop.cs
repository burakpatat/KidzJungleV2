using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionDrop : MonoBehaviour
{
    public GameObject bucket;
    public GameObject bucketMask;
    public bool inRightPosition= false;
    public Vector2 startPos;
    public Vector3 startLocalScale;
    public Animator anim;
    public GameObject otherBucket1;
    public GameObject otherBucket2;
    public Vector3 fruitSize;
    private void Start()
    {
        startLocalScale = gameObject.transform.localScale;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals(otherBucket1.name) || collision.gameObject.name.Equals(otherBucket2.name))
        {
            DimensionDrag.isWrongBucket = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals(otherBucket1.name) || collision.gameObject.name.Equals(otherBucket2.name))
        {
            DimensionDrag.isWrongBucket = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals(otherBucket1.name) || collision.gameObject.name.Equals(otherBucket2.name))
        {
            DimensionDrag.isWrongBucket = false;
        }
    }
    private void Update()
    {
        if (Vector2.Distance(transform.position, bucket.transform.position) < 1.2f || (Vector2.Distance(transform.position, bucketMask.transform.position) < 1.2f))
        {
            inRightPosition = true;
        }
        else
        {
            inRightPosition = false;
        }
    }
}
