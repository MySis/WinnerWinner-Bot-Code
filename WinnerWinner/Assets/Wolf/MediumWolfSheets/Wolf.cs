using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wolf : MonoBehaviour
{

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] List<RuntimeAnimatorController> wolves;
    private float inputX = 0;
    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private WolfGroundSensor m_groundSensor;
    private bool m_grounded = false;
    private bool winner;
    private bool win;

    void Start()
    {
        win = false;
        winner = false;
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = GetComponentInChildren<WolfGroundSensor>();
        m_animator = GetComponent<Animator>();
        int now = Random.Range(0, wolves.Count);
        m_animator.runtimeAnimatorController = wolves[now];

        int layerint = Random.Range(4, 13);
        SpriteRenderer SPR = GetComponent<SpriteRenderer>();
        SPR.sortingOrder = layerint;
    }

    // Update is called once per frame
    void Update()
    {
        if (win)
            return;
        else
        {
            inputX = 0;
            #region Moving
            float Pos = Mathf.Round(transform.position.x);
            if (Pos < 0 && m_grounded)
                inputX = 1;
            else if (Pos > 0 && m_grounded)
                inputX = -1;
            else if (Mathf.Abs(Pos) <= 0.5f)
            {
                inputX = 0;
                StartCoroutine("Fighting");
            }
            // Swap direction of sprite depending on walk direction
            if (inputX > 0)
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            else if (inputX < 0)
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);

            //Check if character just landed on the ground
            if (!m_grounded && m_groundSensor.State())
            {
                m_grounded = true;
                m_animator.SetBool("Grounded", m_grounded);
            }

            //Check if character just started falling
            if (m_grounded && !m_groundSensor.State())
            {
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
            }           

            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);
            #endregion

            if (Mathf.Abs(inputX) > 0.2f)
                m_animator.SetInteger("AnimState", 2);
            else
                m_animator.SetInteger("AnimState", 0);
        }
    }
    public void setwinner()
    {
        winner = true;
    }
    IEnumerator Fighting()
    {
        GetComponent<AudioSource>().enabled = true;
        m_animator.SetBool("Attack", true);
        yield return new WaitForSeconds(6);
        if (!winner)
        {
            inputX = 0;
            m_animator.SetBool("Attack", false);
            m_animator.SetBool("Death", true);
            gameObject.tag = tag.Replace("Player", "Finish");
            gameObject.layer = LayerMask.NameToLayer("Dead");
            GameObject.Find("SpawnPoints").GetComponent<WolfController>().death();
        }
        else
        {
            m_animator.SetBool("Attack", false);
            m_animator.SetBool("Death", false);
        }
        win = true;
    }
    public IEnumerator WinBan()
    {
        int originalTime = 4;
        float time = 0;
        m_animator.SetBool("Attack", false);
        m_animator.SetInteger("AnimState", 0);
        while (time < originalTime)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(3, 3, 3), time / originalTime);
            yield return new WaitForEndOfFrame();
        }
    }
}