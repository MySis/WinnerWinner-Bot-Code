using UnityEngine;
using System.Collections;

public class Bandit : MonoBehaviour {

    [SerializeField] float m_speed = 4.0f;

    private float inputX = 0;
    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Bandit m_groundSensor;
    private bool m_grounded = false;
    private bool winner;
    private bool win;

    // Use this for initialization
    void Start()
    {
        win = false;
        winner = false;
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = GetComponentInChildren<Sensor_Bandit>();
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
            else if (Mathf.Abs(Pos) <= 0.2f)
            {
                inputX = 0;
                m_animator.SetBool("Attack", true);
                StartCoroutine("Fighting");
            }

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

            // Swap direction of sprite depending on walk direction
            if (inputX > 0)
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            else if (inputX < 0)
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);
            #endregion

            #region Animations
            //Set AirSpeed in animator
            m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

            //Run
            if (Mathf.Abs(inputX) > Mathf.Epsilon)
                m_animator.SetInteger("AnimState", 2);
            //Idle
            else
                m_animator.SetInteger("AnimState", 0);
            #endregion
        }
    }
    public void setwinner()
    {
        winner = true;
    }
    IEnumerator Fighting()
    {
        yield return new WaitForSeconds(3.5f);
        if (!winner)
        {
            m_animator.SetBool("Attack", false);
            m_animator.SetBool("Death", true);
            gameObject.tag = tag.Replace("Player", "Finish");
            gameObject.layer = LayerMask.NameToLayer("Dead");
            GameObject.Find("SpawnPoints").GetComponent<FightController>().death();
        }
        else
        {
            m_animator.SetBool("Attack", false);
            m_animator.SetBool("Recover", true); ;
        }
        win = true;
    }
    public IEnumerator WinBan()
    {
        int originalTime = 4;
        float time = 0;
        m_animator.SetBool("Attack", false);
        m_animator.SetBool("Recover", false);
        m_animator.SetInteger("AnimState", 0);
        while (time < originalTime)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(3, 3, 3), time / originalTime);
            yield return new WaitForEndOfFrame();
        }
    }
}