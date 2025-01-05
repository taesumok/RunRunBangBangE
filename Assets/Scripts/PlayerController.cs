using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 플레이어 이동 속도를 설정하는 변수
    public float moveSpeed;
    

    // Rigidbody2D의 마찰력을 설정하는 변수
    //public float drag = 0f;

    // Rigidbody2D 컴포넌트를 저장할 변수
    private Rigidbody2D rb;

    // Animator 컴포넌트를 저장할 변수
    private Animator animator;

    // 터치 위치와 플레이어 위치의 차이를 저장하는 변수
    private Vector2 touchOffset;

    // 플레이어가 드래그 중인지 여부를 저장하는 변수
    private bool isDragging = false;

    // 플레이어 방향 전환 시 반전을 위해 
    private SpriteRenderer spriteRenderer;


    private float rate_x;
    private float rate_y;
    public float ScreenX;
    public float ScreenY;

    // Start is called before the first frame update
    void Start()
    {

        rate_x = (float)Screen.width / ScreenX;
        rate_y = (float)Screen.height / ScreenY;

        // Rigidbody2D 컴포넌트를 가져와서 변수에 저장
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Rigidbody2D의 마찰력을 설정
        //rb.drag = drag;
       
        // Animator 컴포넌트를 가져와서 변수에 저장
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        // 터치 입력을 처리하는 함수 호출
        if(Time.timeScale != 0f)
            HandleMouseOrTouch();

        // 플레이어의 속도에 따라 애니메이션 상태를 변경
        //animator.SetBool("isMoving", rb.velocity.magnitude > 0.1f);


    }

    void HandleMouseOrTouch()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        // 마우스 입력 처리
        if (Input.GetMouseButtonDown(0))
        {

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            //if (hit.collider != null && hit.collider.gameObject == gameObject)
            //{
            isDragging = true;
            touchOffset = (Vector2)transform.position - (Vector2)mousePos;
            //}
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            
             Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
             Vector2 newPos = (Vector2)mousePos + touchOffset;
            // float screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
            // newPos.x = Mathf.Clamp(newPos.x, -screenHalfWidth, screenHalfWidth);

            rb.velocity = (newPos - rb.position) * (moveSpeed* rate_x);
            //rb.AddForce((newPos - rb.position) * (moveSpeed * rate_x),ForceMode2D.Force);


            spriteRenderer.flipX = rb.velocity.x < 0f;
             animator.SetBool("Run", true);

            
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            //rb.velocity = Vector2.zero;
            rb.velocity *= 0.2f;
            animator.SetBool("Run", false);

        }
#elif UNITY_ANDROID || UNITY_IOS
     
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                    //RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
                    //if (hit.collider != null && hit.collider.gameObject == gameObject)
                    //{
                    isDragging = true;
                    touchOffset = (Vector2)transform.position - (Vector2)touchPos;
                    //}
                    break;

                case TouchPhase.Moved:
                      if (isDragging)
                      {
                        touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                        Vector2 newPos = (Vector2)touchPos + touchOffset;
                        // float screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
                        //newPos.x = Mathf.Clamp(newPos.x, -screenHalfWidth, screenHalfWidth);
                        rb.velocity = (newPos - rb.position) * (moveSpeed*rate_x);
                        spriteRenderer.flipX = rb.velocity.x < 0f;
                        animator.SetBool("Run", true);
                      }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isDragging = false;
                    //rb.velocity = Vector2.zero;
                    rb.velocity *= 0.2f;
                    animator.SetBool("Run", false);
                    break;
            }
        }
#endif
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Drop"))
        {
            if (GameManager.instance.isFullGage == true)
            {
                GameManager.instance.NowGage.transform.localScale = new Vector3(0, GameManager.instance.NowGage.transform.localScale.y, 0);
                GameManager.instance.LifeHeart.SetActive(false);
                GameManager.instance.FullGage.SetActive(false);
                GameManager.instance.NowGage.SetActive(true);
                GameManager.instance.destroyAllDrops();

                GameManager.instance.isFullGage = false;

                StartCoroutine(Blink());
                
            }
            else
            {
                Destroy(gameObject);
                GameManager.instance.GameOver();
            }
        }
        
    }
    IEnumerator Blink()
    {
        Color color = spriteRenderer.color;
        Color org_color = spriteRenderer.color;

        for (int i = 0; i < 5; ++i)
        {
            color.a = 1;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(0.1f);

            color.a = 0;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(0.1f);
        }
        spriteRenderer.color = org_color;
    }

}
