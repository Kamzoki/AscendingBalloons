using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    //Member vars:-
    public float m_AscendingSpeed;

    //Private vars :- 
    bool isTouchingPlayer;

    Vector2 bodyOffset;
    //private PowerUp storedPowerUp
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        MoveAround();
	}

    void MoveAround()
    {
        //This function controls the player by dragging it around.

        if (Input.touchCount != 1)
        {
            isTouchingPlayer = false;
            return;
        }

        else
        {
            Touch currentTouch = Input.touches[0];
            Vector2 lastTouchPosition = new Vector2 (0,0);
            if (currentTouch.phase == TouchPhase.Began)
            {
                //Checking if a touch began occuring
                RaycastHit rayInfo;
                Ray ray = Camera.main.ScreenPointToRay(currentTouch.position);
                
                
                if (Physics.Raycast(ray, out rayInfo) && rayInfo.collider.gameObject == gameObject)
                {
                    //Checking if the touch occured on the player or not.
                    isTouchingPlayer = true;

                    lastTouchPosition = Camera.main.ScreenToWorldPoint(currentTouch.position);
                    bodyOffset = gameObject.transform.position - rayInfo.point;
                }
                else
                {
                    isTouchingPlayer = false;
                }
            }

            else if (currentTouch.phase == TouchPhase.Moved)
            {
                //Checking if touch is moving.

                if (isTouchingPlayer == true)
                {
                    //gets the new position of the touch by world unit, and setting the position of the gameobject to the new position on the condition that the y is smaller than zero (center height)
                    Vector3 newPos = Camera.main.ScreenToWorldPoint(currentTouch.position);
                    
                    #region Check Dragging ability

                    if (newPos.x < - Screen.width || newPos.x > Screen.width)
                    {
                        newPos.x = gameObject.transform.position.x;
                    }

                    if (newPos.y > 0)
                    {
                        newPos.y = 0;
                    }
#endregion

                    gameObject.transform.position = new Vector3(newPos.x + bodyOffset.x, newPos.y + bodyOffset.y, gameObject.transform.position.z);
                }
            }

            else if (currentTouch.phase == TouchPhase.Canceled || currentTouch.phase == TouchPhase.Ended)
            {
                //When touch is cancled or ended, setting isTouching the player to false.
                isTouchingPlayer = false;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //if hit an enemy, destroy the player and reload the scene
            //TODO : change when design come to play death animtion and bring up losing screen.
            GameObject.Destroy(gameObject);
            SceneManager.LoadScene(0);
        }

        else if (collision.gameObject.tag == "Power Up")
        {
            // TODO: call function here collision.gameObject.GetComponent<PowerUp>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //FOR TESTING ONLY
        Debug.Log("entered");
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("here");
            GameObject.Destroy(gameObject);
            SceneManager.LoadScene(0);
        }

        else if (other.gameObject.name == "Power Up")
        {

        }
    }
}
