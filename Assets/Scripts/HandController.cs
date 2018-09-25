using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {


    public GameObject fry;
    [SerializeField] float  moveSpeed = 3f;
    [SerializeField] Sprite openHand;
    [SerializeField] Sprite closedHand;

    SpriteRenderer sr;

    Vector3 beginPosition;

    bool gotFry = false;


    // Use this for initialization
    void Start () {

        sr = GetComponentInChildren<SpriteRenderer>();

        beginPosition = transform.position;    
    }
	
	// Update is called once per frame
	void Update () {

        if(!gotFry && transform.position != fry.transform.position)
        {
            atemptTheft();
        }
        else
        {
            gotFry = true;
            sr.sprite = closedHand;
            Destroy(fry);
        }


        if(gotFry)
        {
            runAway();
        }


    }

    private void atemptTheft()
    {
        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y),
                                                         fry.transform.position, moveSpeed * Time.deltaTime);
    }

    private void runAway()
    {
        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y),
                                                         beginPosition, moveSpeed * Time.deltaTime);
    }
}
