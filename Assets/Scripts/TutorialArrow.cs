using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheLastFry
{
    public class TutorialArrow : MonoBehaviour
    {

        [SerializeField] float moveX, moveY, moveZ, moveTime = 1f;

        int moveID;


        // Start is called before the first frame update
        void Start()
        {
            moveID = LeanTween.move(gameObject, 
                                    new Vector3(transform.position.x + moveX, transform.position.y + moveY, transform.position.z + moveZ), moveTime)
                                                        .setLoopType(LeanTweenType.pingPong).id;
            
        }
    }
}
