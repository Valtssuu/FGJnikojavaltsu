using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingBall : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isBeingHeld = false;

    

    // Update is called once per frame
    void Update()
    {

        //when piece is held by mouse/touch
        if (isBeingHeld == true)
        {
            
            //gravity is disabled and piece follows mouses/touches position
            this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.rotation = Quaternion.identity;

            this.gameObject.transform.localPosition = new Vector3(mousePos.x, mousePos.y, 0);
        }

        else
        {
            this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void OnMouseDown()
    {
        //when player holds mouse or touch isBeingHeld changes to true
        if (Input.GetMouseButtonDown(0))
        {
            isBeingHeld = true;
        }     
    }

    private void OnMouseUp()
    {
        //on realease isBeingHeld changes to false
        isBeingHeld = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Piece")
        {
            Rigidbody2D rb = this.gameObject.GetComponent<Rigidbody2D>();
            Vector2 force = new Vector2(this.transform.position.x, this.transform.position.y);

            rb.AddForce(force * 20, ForceMode2D.Impulse);
        }
    }
}
