////////////////////////////////////////
/// FileName: ShrinkingPlatform.cs
/// Author: Blair White 
/// Student #: 100328532
/// Date Last Modified 12/15/2020
/// Description: Behaviour for a platform that floats up and down
///              When in contact with player it shrinks, and expands
///              back to normal otherwise.
///              Plays appropriate sound effects.
///////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingPlatform : MonoBehaviour
{
    private Vector3 StartPos, StartScale;
    private bool upDown, isShrink;
    public AudioClip audShrink, audGrow;
    // Start is called before the first frame update
    void Start()
    {
        //Get our starting Transforms
        StartPos = this.transform.position;
        StartScale = this.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //If the platform is at its full size don't play the growing sfx
        if (this.transform.localScale.x >= StartScale.x) this.GetComponent<AudioSource>().volume = 0; 

        if (!upDown)
        {
            //if we are moving up: check y position with starting y position.
            //if were above the starting position + the float height switch to moving down
            if (this.transform.position.y > StartPos.y + 0.1f) upDown = true;
            this.transform.position = new Vector3(transform.position.x, transform.position.y + .1f * Time.deltaTime, transform.position.z);
        }
        if (upDown)
        {
            //The oppossite of the above
            if (this.transform.position.y < StartPos.y - 0.1f) upDown = false;
            this.transform.position = new Vector3(transform.position.x, transform.position.y - .1f * Time.deltaTime, transform.position.z);
        }

        if(isShrink)
        {
            //if we are shrinking
            if(this.transform.localScale.x > 0)
            {
                //shrink...
                this.transform.localScale = new Vector3(this.transform.localScale.x - .3f * Time.deltaTime, this.transform.localScale.y -.3f * Time.deltaTime, this.transform.localScale.z);
            }
            if(this.transform.localScale.x <= 0)
            {
                //if we are shrinking and we are smaller than 0 stop shrinking.
                isShrink = false;
            }
            
            this.GetComponent<AudioSource>().volume = 1;
            
        }

        if(!isShrink)
        {
            //if we are not shrinking we are growing.
            if (this.transform.localScale.x < StartScale.x)
            {
                //grow
                this.transform.localScale = new Vector3(this.transform.localScale.x + .3f * Time.deltaTime, this.transform.localScale.y + .3f * Time.deltaTime, this.transform.localScale.z);
            }
        }    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //if we collide with the player start shrinking, set and play the sound effect.
            isShrink = true;
            this.GetComponent<AudioSource>().clip = audShrink;
            this.GetComponent<AudioSource>().Play();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //if we stop colliding with t he player start growing and set and play the sound effect.
            isShrink = false;
            this.GetComponent<AudioSource>().clip = audGrow;
            this.GetComponent<AudioSource>().Play();
        }
    }
}
