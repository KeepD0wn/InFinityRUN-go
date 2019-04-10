using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    CharacterController charter;
    Animator animator;
    [SerializeField,Range(0,100)]
    float moveSpeed=15f; // скорость бега
    Vector3 moveVector;
    [SerializeField, Range(0, 10)]
    float jumpPower = 10f;  // сила прыжка
    float gravityForce;  // гравитация
    bool positionChanged=false;  // менялась ли позиция
    int laneCount=2, currentLane=1;
    float firstLanePos = -3f, distanceBtwLane=3f, laneMove=10f;
    bool rolling=false;


    Vector3 charterStayCenter=new Vector3(0,1.8f,0),
        charterSlideCenter= new Vector3(0,0.57f,0f);

    float charterHeightStay=3.77f,
        charterHeightSlide=1f;

    float charterRadiusStay=0.5f,
        charterRadiusSlide=0.7f;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        charter = GetComponent<CharacterController>();
        moveVector = new Vector3(1,0,0);
    }

    // Update is called once per frame
    void Update()
    {      
        Move();
        Gravity();
        Jump();
        StartSlide();
        
    }


    /// <summary>
    /// уменьшаем гравитацию если не стоит на земле иначе -1
    /// </summary>
    void Gravity()
    {
        if (!charter.isGrounded)
        {
            gravityForce -= 20f*Time.deltaTime;
        }
        else
        {
            gravityForce = -1f;
        }
    }

    /// <summary>
    /// тут прописано передвижение, смена линии
    /// </summary>
    void Move()
    {
        float input = Input.GetAxis("Horizontal");
        moveVector.z = moveSpeed;
        moveVector.y = gravityForce; // придаём у значение гравитации (прыжок/падение)
        moveVector *= Time.deltaTime;
        
        if (Mathf.Abs(input) > 0.1f)
        {
            if (positionChanged == false)
            {
                currentLane += (int)Mathf.Sign(input);
                currentLane = Mathf.Clamp(currentLane, 0, laneCount);                
                positionChanged = true;
            }
        }
        else
        {
            positionChanged = false;
        }

        Vector3 newPos = transform.position;
        newPos.x = Mathf.Lerp(newPos.x, firstLanePos + (currentLane * distanceBtwLane), Time.deltaTime * laneMove);
        transform.position = newPos;

        charter.Move(moveVector);
    }


    /// <summary>
    /// анимация прыжка и уменьшение гравитации
    /// </summary>
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && charter.isGrounded && !rolling)
        {
            animator.SetTrigger("Jumping");
            gravityForce = jumpPower;
        }
    }

    /// <summary>
    /// если можно катиться, то запускаем корутину отвечающую за подкат
    /// </summary>
    void StartSlide()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && charter.isGrounded)
        {
            StartCoroutine(Slide());
        }
    }

    IEnumerator Slide()
    {
        rolling = true;
        animator.SetBool("Rolling",true);

        charter.center = charterSlideCenter;
        charter.height = charterHeightSlide;
        charter.radius = charterRadiusSlide;

        yield return new WaitForSeconds(1f);
        rolling = false;
        animator.SetBool("Rolling", false);

        charter.center =charterStayCenter;
        charter.height = charterHeightStay;
        charter.radius = charterRadiusStay;
    }
}
