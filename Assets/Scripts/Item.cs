using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    private float rate_x;
    private float rate_y;

    public float ScreenX;
    public float ScreenY;

    private Rigidbody2D  dropRigidBody;
    private BoxCollider2D boxCollider;

    private SpriteRenderer spriteRenderer;

    public Text questeionText;

    private Animator animator;



    // Start is called before the first frame update
    void Start()
    {
        dropRigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    

        rate_x = (float)Screen.width / ScreenX;
        rate_y = (float)Screen.height / ScreenY;


    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
           // transform.position = new Vector3(transform.position.x, other.transform.position.y , transform.position.z);
            transform.position = new Vector3(transform.position.x , transform.position.y - (0.1f * rate_y), 0);
            destroyItem();

        }
        if( other.CompareTag("Player"))
        {

            Destroy(this.gameObject);
        }
    }

    public void destroyItem()
    {
        Debug.Log("destroyItem");
        dropRigidBody.velocity = Vector2.zero;
        boxCollider.size = Vector2.zero;
        dropRigidBody.isKinematic = true;
        transform.localScale = new Vector3(rate_x * 4f , rate_y * 4f, 1);
        questeionText.gameObject.SetActive(false);
        Destroy(gameObject);
        //animator.SetTrigger("itemBreak");
        //StartCoroutine(DestroyAfterAnimation());
    }

    IEnumerator DestroyAfterAnimation()
    {
        Debug.Log("DestroyAfterAnimation");
        // �ִϸ��̼� ���̸�ŭ ���
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // ������Ʈ ����
        Destroy(gameObject);
    }
}
