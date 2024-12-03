using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    RaycastHit2D rayHit;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        /*Jump : Space바 누르면 점프라고 인식됨*/
        if (Input.GetButtonDown("Jump") && !anim.GetBool("IsJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("IsJumping", true);
        }
        //버튼에서 손을 땠을 때 정지
        if(Input.GetButtonUp("Horizontal"))
        {
            /*normalized : 벡터 크기를 1로 만듦(단위벡터 상태)*/
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //방향전환
        if (Input.GetButtonDown("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        /*
        속도(rigid.velocity.x)가 0.3보다 작아질때
        즉, 캐릭터가 멈출때
        */ 
        if(Mathf.Abs(rigid.velocity.x) < 0.3)
        {
            /*
            SetBool : bool을 true / false 로 변환
            캐릭터가 멈추었을때 PlayerIdle 에니메이션 재생 
            */
            anim.SetBool("IsWalking", false);
        }
        else //캐릭터가 움직이고 있을때 PlayerWalk 에니메이션 재생
        {
            anim.SetBool("IsWalking", true);
        }
    }

    void FixedUpdate()
    {
        /*Horizontal : 키를 인식하여 캐릭터가 움직이도록 (2D상에서 x축)*/
        /*Vertical : 키를 인식하여 캐릭터가 움직이도록 (2D상에서 y축)*/
        float height = Input.GetAxisRaw("Horizontal");
        //Debug.Log("height (높이) : " + height);

        /*AddForce : ���� ������Ʈ�� �̵��ϰų�, �̵��ӵ� �Ǵ� ������ ������ �� ����ϴ� �Լ�*/
        rigid.AddForce(Vector2.right*height, ForceMode2D.Impulse);
        /*Mathf.Abs : ������ ���ϴ� �Լ�*/

        if(rigid.velocity.x > maxSpeed) //������ �ִ� �ӵ� ���޽�
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if(rigid.velocity.x < maxSpeed*(-1)) //���� �ִ� �ӵ� ���޽�
        {
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);
        }

        /*Mathf.Abs : ������ ���ϴ� �Լ�.*/
        Debug.Log("���� �ӵ� : " + Mathf.Abs(rigid.velocity.x));

        /*����*/
        if(rigid.velocity.y < 0)
        {
            /*RayCast : ������Ʈ �˻��� ���ؼ� Ray�� ��� ���.*/
            /*DrawRay :  ������ �󿡼� �� Ray�� �׸��� �Լ�.*/
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 0, 0));

            /*RaycastHit : ray�� ���� ������Ʈ*/
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if(rayHit.collider != null)
            {
                if(rayHit.distance < 0.5f)
                {
                    anim.SetBool("IsJumping", false);
                }
                Debug.Log(rayHit.collider.name);
            }
            
        }
    }
}
