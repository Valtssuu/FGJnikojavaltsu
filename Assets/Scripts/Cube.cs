using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{

    public enum CubeColor
    {
        RedCube,
        YellowCube,
        BlueCube
    }

    public GameObject starParticle;
    public Bomb bomb;
    private bool blown = false;
    public CubeColor cubeColor;
    private string gridColor;
    private bool isBeingHeld = false;
    private bool allowSnapping = false;
    public bool isSnapped = false;
    private bool isOnTopOfSomething = false;
    // Boolean flag to check if object is colliding with ground object
    private bool borderCollision = false;
    private HashSet<string> borderCollisions = new HashSet<string>();

    public RaycastHit2D hit;

    void Start()
    {
        Time.timeScale = 0;
        bomb = GameObject.FindGameObjectWithTag("Bomb").GetComponent<Bomb>();

        switch (cubeColor)
        {
            case CubeColor.RedCube:
                gridColor = "GridRed";
                break;
            case CubeColor.YellowCube:
                gridColor = "GridYellow";
                break;
            case CubeColor.BlueCube:
                gridColor = "GridBlue";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        RigidbodyType2D bodyType = this.gameObject.GetComponent<Rigidbody2D>().bodyType;

        if (bomb.blow)
        {

            Blow();
        }
        // Reset object position if it falls offscreen
        if (this.gameObject.transform.position.y <= -6 || this.gameObject.transform.position.y >= 7.3f)
        {
            this.gameObject.transform.position = new Vector3(0, 0, 0);
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            // Disallow snapping to prevent cheesing the game by throwing the boxes out and respawning them
            allowSnapping = false;
        }

        // Casting ray down 
        hit = Physics2D.Raycast(this.transform.position + new Vector3(0, -0.63f, 0), -transform.up, 0.1f, 1 << 8);

        // If ray hits something we change boolean "isOnTopOfSomething" to true and false if it doesn't hit anything
        if (hit.collider != null)
        {
            GameObject colliderObject = hit.collider.gameObject;
            // If raycast hits ground or snapped object, allow snapping
            if (colliderObject.name == "Ground" || (colliderObject.tag == "Piece" && colliderObject.GetComponent<Cube>().isSnapped))
            {
                isOnTopOfSomething = true;
            }
        }
        else
        {
            isOnTopOfSomething = false;
        }

        // When piece is held by mouse/touch
        
        if (isBeingHeld == true )
        {
            
            // New y-axis position when dragging the object
            float newPosX, newPosY;
            // Object's xy-axis components before checking mouse dragging
            float objectPosX = this.gameObject.transform.position.x;
            float objectPosY = this.gameObject.transform.position.y;
            // Booleans to check if dragging along either of the axes should be restricted
            bool restrictX = false;
            bool restrictY = false;

            // Gravity is disabled and piece follows mouses/touches position
            bodyType = RigidbodyType2D.Static;
            Vector3 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.rotation = Quaternion.identity;

            if (borderCollision == true)
            {
                // Iterate through all colliding objects and set axes to restrict
                foreach (string collObject in borderCollisions)
                {
                    switch (collObject)
                    {
                    case "LeftWall":
                    case "RightWall":
                        restrictX = true;
                        break;
                    case "Ground":
                        restrictY = true;
                        break;
                    }
                }
            }

            // Stop restricting axes if object is dragged towards the center
            if (restrictX && Mathf.Abs(mousePos.x) < Mathf.Abs(objectPosX))
            {
                restrictX = false;
            }

            if (restrictY && (objectPosY < mousePos.y))
            {
                restrictY = false;
            }
            
            // If no axes are restricted the object is not colliding with screen borders
            if (!restrictX && !restrictX && !restrictY && borderCollision)
            {
                borderCollision = false;
                borderCollisions.Clear();
            }

            // Set new x and y coordinates for the object depending on if axes are restricted or not
            if (restrictX)
            {
                newPosX = objectPosX;
            }
            else
            {
                newPosX = mousePos.x;
            }

            if (restrictY)
            {
                newPosY = objectPosY;
            }

            else
            {
                newPosY = mousePos.y;
            }

            this.gameObject.transform.position = new Vector3(newPosX, newPosY, 0);
        }

        // If piece has snapped to grid freeze all movement of the piece
        if (isSnapped == true)
        {
            // Freeze Cube position without changing its physics
            transform.rotation = Quaternion.identity;
            this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            starParticle.SetActive(true);
        }
        else
        {
            // If Cube is not snapped or being held and its body type is still static, change it to dynamic
            if (bodyType == RigidbodyType2D.Static)
            {
                bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }




    private void OnMouseDown()
    {
        // When player touches a Cube, it's not snapped and bomb is blown, allow dragging and snapping
        if (isSnapped == false && Input.GetMouseButtonDown(0) && blown == true)
        {
            isBeingHeld = true;
            // Allow snapping only after player touches first object
            if (allowSnapping == false)
            {
                allowSnapping = true;
            }
        }
    }

    private void OnMouseUp()
    {
        if (isBeingHeld == true)
        {
            // On release isBeingHeld changes to false and object velocity is set to zero
            isBeingHeld = false;
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Check if trigger is an object with screen border tag
        if (collision.gameObject.tag == "ScreenBorder")
        {
            borderCollision = true;
            borderCollisions.Add(collision.gameObject.name);
        }

        // When inside gridslots trigger piece snaps to it
        if (isBeingHeld == false && collision.gameObject.tag == gridColor && isOnTopOfSomething == true 
            && allowSnapping == true && collision.gameObject.GetComponent<Grid>().isFree == true)
        {
            this.transform.position = collision.gameObject.transform.position + new Vector3(0,0,0.25f);
        
            isSnapped = true;
            collision.gameObject.GetComponent<Grid>().isFree = false;
        }
    }

    public void Blow()
    {
        Time.timeScale = 1;
        float distance = Vector2.Distance(bomb.transform.position, this.transform.position);
        Vector3 bombForce = bomb.kiloTons * (transform.position - bomb.transform.position) / (distance * distance);
        if (!blown)
        {
            this.GetComponent<Rigidbody2D>().AddForce(bombForce, ForceMode2D.Impulse);
        }
        blown = true;
        StartCoroutine("TurnBombOff");
    }

    private IEnumerator TurnBombOff()
    {
        yield return new WaitForSeconds(0.1f);
        bomb.blow = false;

    }
}
