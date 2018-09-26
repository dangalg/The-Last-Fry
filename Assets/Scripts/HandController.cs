using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{


    public GameObject targetFry;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] Sprite openHand;
    [SerializeField] Sprite closedHand;

    [SerializeField] List<Sprite> hurtHands;
    [SerializeField] GameObject fry;

    SpriteRenderer sr;

    Vector3 beginPosition;

    bool gotFry = false;
    bool handHit = false;

    int currentHit = 0;


    // Use this for initialization
    void Start()
    {

        sr = GetComponentInChildren<SpriteRenderer>();

        beginPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (gotFry)
        {
            runAway();
        }

        if (handHit)
        {
            runAway();
        }

        if (!gotFry && !handHit)
        {
            if (transform.position != targetFry.transform.position)
            {
                atemptTheft();
            }
            else
            {
                gotFry = true;
                sr.sprite = closedHand;
                Destroy(targetFry);
            }
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            ClickHand(Input.mousePosition);
        }
#else
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            ClickHand(Input.GetTouch(0).position);
        }
#endif


    }

    private void ClickHand(Vector3 hitPosition)
    {
        //RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition ,Vector2.zero,Mathf.Infinity); //Hit object that contains gameobject Information
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(hitPosition), Vector2.zero);

        if (hit.collider != null)
        {
            Debug.Log("Something Hit");

            if (hit.collider.CompareTag("ThiefHand"))
            {
                Debug.Log("Hand Hit");
                handHit = true;
                if(currentHit < hurtHands.Count)
                {
                    // Display next hurt hand... It gets worse and worse!
                    sr.sprite = hurtHands[currentHit];
                    currentHit++;

                    if(gotFry)
                    {
                        gotFry = false;
                        Instantiate(fry, transform.position, transform.rotation);
                    }
                }

            }
        }
    }

    private void atemptTheft()
    {
        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y),
                                                         targetFry.transform.position, moveSpeed * Time.deltaTime);
    }

    private void runAway()
    {
        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y),
                                                         beginPosition, moveSpeed * Time.deltaTime);
    }
}
