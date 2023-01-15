using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public GameObject prefab;
    public bool head;//true×ó,falseÓÒ
    private float gapTime = 0.5f;
    private float nextTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        head = rb2D.velocity.x < 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(rb2D.velocity.x != 0)
        {
            head = rb2D.velocity.x < 0;
        }
        

        if (Input.GetKeyDown(KeyCode.K) && Time.time > nextTime)
        {
            nextTime = Time.time + gapTime;

            GameObject clone = GameObject.Instantiate(prefab, transform.position, transform.rotation);

            //clone.transform.position = rb2D.transform.position + new Vector3(rb2D.transform.position.x * 2,0,0);
            //clone.transform.position.Set(rb2D.transform.position.x + rb2D.transform.position.normalized.x, 0, 0);
            if(rb2D.velocity.x != 0)
            {
                clone.SendMessage("setVector2", rb2D.velocity);
                GameObject.Find(clone.name).SendMessage("setVector2", rb2D.velocity);
            }
            else
            {
                if (head)
                {
                    clone.SendMessage("setVector2", new Vector2(-1, 0));
                    //GameObject.Find(clone.name).SendMessage("setVector2", new Vector2(-1, 0));
                }
                else
                {
                    clone.SendMessage("setVector2", new Vector2(1, 0));
                    //GameObject.Find(clone.name).SendMessage("setVector2", new Vector2(1, 0));
                }
                
            }


        }
        
    }
}
