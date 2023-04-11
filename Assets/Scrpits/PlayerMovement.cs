using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed; 
    private Rigidbody2D body;

    private void Awake()
    {
        body GetComponent<Rigidbody2D>();
    }
     private void Update()
    {
        body.velocity = new Vector2(Inpit.GetAxis("Horizontal"), * speed, body.velocity.y);
    }
  }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    
 
    
    }

 

}


