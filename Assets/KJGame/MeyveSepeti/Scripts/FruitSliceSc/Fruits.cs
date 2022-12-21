using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Fruits : MonoBehaviour
{
    public GameObject slicedFruit;
    public GameObject fruitJuice;
    float rotationForce = 200f;
    Rigidbody2D rgFruit;
    public GameObject fruitExplosion;

    private void Start()
    {
        Destroy(gameObject, 5f);
        rgFruit = gameObject.GetComponent<Rigidbody2D>();
    }
    public void SliceFruit()
    {
        rotationForce = Random.Range(150f, 200f);

        Instantiate(fruitExplosion,transform.position,transform.rotation);
        GameObject sliceFruit =Instantiate(slicedFruit, transform.position, transform.rotation);

        GameObject sliceFruitJuice =  Instantiate(fruitJuice, new Vector3(transform.position.x, transform.position.y, 0f), fruitJuice.transform.rotation);

        Rigidbody2D[] slicedFruitRg2d = sliceFruit.transform.GetComponentsInChildren<Rigidbody2D>();

        //foreach(Rigidbody2D rg2d in slicedFruitRg2d)
        //{
        //    rg2d.velocity = rgFruit.velocity * 1.2f;
        //}

        for (int i = 0; i < slicedFruitRg2d.Length; i++)
        {
            if(transform.rotation.z>0 && transform.rotation.z < 180)
            {
                if (i == 0)
                {
                    slicedFruitRg2d[i].AddForce(Vector2.left * rotationForce);

                }
                else
                {
                    slicedFruitRg2d[i].AddForce(Vector2.right * rotationForce);
                }
                slicedFruitRg2d[i].velocity = rgFruit.velocity * 0.6f;
            }
            else
            {
                if (i == 0)
                {
                    slicedFruitRg2d[i].AddForce(Vector2.right * rotationForce);

                }
                else
                {
                    slicedFruitRg2d[i].AddForce(Vector2.left * rotationForce);
                }
                slicedFruitRg2d[i].velocity = rgFruit.velocity * 0.6f;
            }
        }
        Destroy(sliceFruit, 5f);
        Destroy(sliceFruitJuice, 2.5f);
    }
    //private void Update()
    //{
    //    transform.Rotate(Vector2.up * Time.deltaTime * rotationForce);
    //}
    float turnSpeed = 5f;

    private void Update()
    {
        transform.Rotate(0f, 0f, 0.1f * turnSpeed, Space.World);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Finger"))
        {
            //transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            MeyveSepeti_Sounds.aManager.FruitSliceSound();
            Destroy(gameObject);
            SliceFruit();
        }
    }
    //void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    //{
    //    //Quaternion target = Quaternion.Euler(0f, 0f, turnSpeed);

    //    //gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 2f);
    //    SliceFruit();
    //    Destroy(gameObject);
    //}
}
