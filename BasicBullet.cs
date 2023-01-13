using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : MonoBehaviour
{
    private Vector2 v2;
    private float speed = 10f;
    Rigidbody2D rb2D;
   

    public void setVector2(Vector2 vector)
    {
        this.v2 = vector.normalized;
        
        v2.y = 0;
    }


    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
       
        rb2D.velocity = v2 * speed;
        Destroy(gameObject, 2);
    }


    // Update is called once per frame
    void Update()
    {
       //TODO 还需要建立定时销毁逻辑
    }
}
