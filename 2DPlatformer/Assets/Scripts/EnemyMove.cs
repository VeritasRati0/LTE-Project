using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    //이것은 변수입니다.
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRanderer;
    public int nextMove; //값이 궁금하다면 public
    // Start is called before the first frame update
    void Start()
    {
        //이것은 변수 초기화입니다.
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRanderer = GetComponent<SpriteRenderer>();
        Invoke("Think", 5); // Invoke는 가장 아래에 써주는게 메너
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        /* 몹 이동*/
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        /*플렛폼 판정*/
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.2f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0)); //Vector2도 가능하나 Vector3를 권장
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, LayerMask.GetMask("Flatform"));
        if(rayHit.collider == null) //rayHit.collider가 null(없음)일때 => Flatform의 rayer가 없는 곳일 때
        {
            Debug.Log("대학원생이 되는 쉽고 빠른길");
            Turn();
        }
    }

/*재귀 함수 : 자신을 스스로 호출하는 함수*/
    void Think()
    {
        /*Random.Range : 난수 발생 함수 Random.Range(범위최소, 범위최대+1) 형식으로 작성*/
        nextMove = Random.Range(-1, 2); 

        /*
        딜레이 없이 쓰는 재귀함수는 디도스와 같다.
        Invoke : 함수를 딜레이 시킬때 사용. Invoke("함수형", 시간) 형식으로 사용함
        */
        float nextThinkTime = Random.Range(2f, 5f); //2초 부터 4초까지 랜덤한 난수 설정
        //Invoke는 코드 가장 아래 써주는게 메너
        Invoke("Think",nextThinkTime); // Think함수를 5초 딜레이

        /*스프라이트 애니메이션 */
        anim.SetInteger("WalkSpeed", nextMove);
        /*스프라이트 방향전환*/
        if(nextMove != 0)
        {
            spriteRanderer.flipX = nextMove == 1;
        }
    }

    void Turn() //이동중 flip하는 함수
    {
        nextMove *= -1;
        spriteRanderer.flipX = nextMove == 1;
        /* CancleInvoke() : 현재 작동중인 Invoke 함수를 취소 시키는 함수*/
        CancelInvoke();
        Invoke("Think",3);
    }
}

