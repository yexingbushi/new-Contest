using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.XR;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;        //创建状态机
    protected Player player;                          //获取玩家对象
    protected Rigidbody2D rig;                        
    protected float stateTimer;                       //计时器

    private string animBoolName;                //这里的string用于储存状态名称
    protected float xInput;
    protected float yInput;
    protected bool triggerCalled;               //动画的trigger，比如去判断这一刀是否砍完了

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)  //属性分配，构造函数，之后的所有state都要拥有这三个属性
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;                
    }
    public virtual void Enter()
    {
        rig = player.rig;
        stateMachine = player.stateMachine;
        player.anim.SetBool(animBoolName, true);                     //设置animator controler中的该状态激活
        triggerCalled = false;
    }

    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("Yvelocity", rig.velocity.y);           //设置animator controller中的y速度 = 玩家刚体的y速度
        stateTimer -= Time.deltaTime;                                //计时器刷新
    }



    public virtual void Exit() 
    {
        
        player.anim.SetBool(animBoolName, false);                    //退出状态

    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;                                        //和animaotor controller配合使用
    }
}
