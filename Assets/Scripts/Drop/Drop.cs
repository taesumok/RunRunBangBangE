using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Drop : MonoBehaviour
{
 
    // Animator ������Ʈ�� ������ ����
    private Animator animator;
    private Rigidbody2D  dropRigidBody;
    private BoxCollider2D boxCollider;

    private SpriteRenderer spriteRenderer;

    private float rate_x;
    private float rate_y;

    public float ScreenX;
    public float ScreenY;
    public  Text plusScoreText;
    private bool isAddScore = false;

    void Start()
    {
        // Animator ������Ʈ�� �����ͼ� ������ ����
        animator = GetComponent<Animator>();
        dropRigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rate_x = (float)Screen.width / ScreenX;
        rate_y = (float)Screen.height / ScreenY;
    }
  
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            GameManager.instance.AddScore();

            //transform.position = new Vector3(transform.position.x, other.transform.position.y + (1.2f*rate_y), transform.position.z);
            transform.position = new Vector3(transform.position.x, other.transform.position.y + (1.2f*rate_y), transform.position.z);
            isAddScore = true;
            destroyDrops();
            
            //Destroy(gameObject);
        }
        
    }
    public void destroyDrops()
    {
        dropRigidBody.velocity = Vector2.zero;
        boxCollider.size = Vector2.zero;
        dropRigidBody.isKinematic = true;
        transform.localScale = new Vector3(0.5f * rate_x * 1.2f , 0.5f * rate_y * 1.2f, 1);
        animator.SetTrigger("dropCrash");
        if(isAddScore == true){
            plusScoreText.gameObject.SetActive(true);
            plusScoreText.text = "+" + GameManager.instance.addScore;
            isAddScore = false;
        }
        StartCoroutine(DestroyAfterAnimation());
    }

    IEnumerator DestroyAfterAnimation()
    {
        // �ִϸ��̼� ���̸�ŭ ���
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // ������Ʈ ����
        Destroy(gameObject);
    }
}
