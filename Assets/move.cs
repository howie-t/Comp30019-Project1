using UnityEngine;
using System.Collections;

public class move : MonoBehaviour {

    public float speed;
    public float spinSpeed;
    
    public int mid_point = 17;
    public Landscape landscape;
    float mouseUpDown;
    float nextW;
    float nextS;
    float nextA;
    float nextD;
    
    Vector3 next;
    float mouseLeftRight;
    


    void start() { 
        speed = 13;
        spinSpeed = 10;
       
        
        this.landscape = landscape.gameObject.GetComponent<Landscape>();
        
       

       
    }

    // Update is called once per frame
    void Update()
	{
        
        //moving mouse
        Mouse_rolling();
        mouseLeftRight = Input.mousePosition.x;
        mouseUpDown = Input.mousePosition.y;
        
    

        //rolling the camera 
        if (Input.GetKey(KeyCode.Q)){
			
            this.transform.Rotate(0, 0, Time.deltaTime * spinSpeed);
           
        }
		if (Input.GetKey (KeyCode.E)) {
            
            this.transform.Rotate(0, 0, -Time.deltaTime * spinSpeed);
        }
        

        //moving the camera 

        //left and right
        if (Input.GetKey(KeyCode.D) )
        {
            nextD = speed * Time.deltaTime;
            this.transform.Translate(nextD, 0,0);
            next = this.transform.localPosition;
            if (this.next.x > mid_point * 2 - 2 || this.next.x < 0)
            {
                this.transform.Translate(-nextD, 0, 0);

            }
            if (this.next.z > mid_point * 2 - 2 || this.next.z < 0)
            {
                this.transform.Translate(-nextD, 0, 0);
            }
            if (this.next.y < landscape.get_height(this.next.x, this.next.z)+3)
            {
                this.transform.Translate(-nextD, 0, 0);
            }


        }
		if (Input.GetKey (KeyCode.A) ) {
            nextA = -speed * Time.deltaTime;
            this.transform.Translate(nextA, 0, 0);
            next = this.transform.localPosition;
            if (this.next.x > mid_point * 2 - 2 || this.next.x < 0)
            {
                this.transform.Translate(-nextA, 0, 0);

            }
            if (this.next.z > mid_point * 2 - 2 || this.next.z < 0)
            {
                this.transform.Translate(-nextA, 0, 0);
            }
            if (this.next.y < landscape.get_height(this.next.x, this.next.z) + 3)
            {
                this.transform.Translate(-nextA, 0, 0);
            }
        }

        //forward and back
        if (Input.GetKey(KeyCode.W) )
        {
           
            nextW = speed * Time.deltaTime;
            this.transform.Translate(0, 0, nextW);
            next = this.transform.localPosition;
            if (this.next.x > mid_point * 2 - 2 || this.next.x < 0)
            {
                this.transform.Translate(0, 0, -nextW);

            }
            if (this.next.z > mid_point * 2 - 2 || this.next.z < 0)
            {
                this.transform.Translate(0, 0, -nextW);
            }
            if (this.next.y < landscape.get_height(this.next.x, this.next.z) + 3)
            {
                this.transform.Translate(0, 0, -nextW);
            }

        }
        if (Input.GetKey (KeyCode.S)) {
            nextS = -speed * Time.deltaTime;
            this.transform.Translate(0, 0, nextS);
            next = this.transform.localPosition;
            if (this.next.x > mid_point * 2 - 2 || this.next.x < 0)
            {
                this.transform.Translate(0, 0, -nextS);

            }
            if (this.next.z > mid_point * 2 - 2 || this.next.z < 0)
            {
                this.transform.Translate(0, 0, -nextS);
            }
            if (this.next.y < landscape.get_height(this.next.x, this.next.z) + 3)
            {
                this.transform.Translate(0, 0, -nextS);
            }
        }
	}

    
   
        //rolling the mouse
    public void Mouse_rolling()
    {
        float speed = 0.5f;
        if (Input.GetMouseButton(0))
        {
            //up down
            float cameraRotationUpDown = (Input.mousePosition.y - mouseUpDown) * speed;
            if (Input.mousePosition.y != mouseUpDown)
            {
               this.transform.Rotate(cameraRotationUpDown, 0, 0);
                
                
            }
            //left and right
            float cameraRotationLeftRight = (Input.mousePosition.x - mouseLeftRight) * speed;
            if (Input.mousePosition.x != mouseLeftRight)
            {
                this.transform.Rotate(0, cameraRotationLeftRight,0);

              
            }
        }
    }
}
