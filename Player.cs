using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rig;
    public PlayerStateMachine stateMachine;
    public bool facingRight;
    public int facingDir;

    public float xInput;
    public float yInput;



    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>(); 
        stateMachine = new PlayerStateMachine();                                     //获取组件

        

    }

    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        
    }

    public void SetVelocity(float _xvelocity, float _yvelocity)
    {
        rig.velocity = new Vector2(_xvelocity, _yvelocity);
        FlipController(_xvelocity);

    }                                                    //定义速度控制函数

    protected virtual void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

    }

    protected virtual void FlipController(float _x)
    {
        if (!facingRight && _x > 0)
        {
            Flip();
        }
        else if (_x < 0 && facingRight)
        {
            Flip();
        }
    }
}
