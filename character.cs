using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class character : MonoBehaviour
{/// <summary>
/// 用于记录数值，玩家敌人都可以用，带部分事件广播与监听用于ui的更新
/// </summary>
    // [Header("监听事件")]
    // public VoidEventSO NewGameEvent;
    // public VoidEventSO ShurikenAttackEvent;
    // // public VoidEventSO sceneandcharacteEventSO;
    [Header("基本属性")]
    public float Maxhealth;
    public float Currenthealth;
    public float MaxBluehealth;
    public float CurrentBluehealth;
    public float MaxPower;
    public float CurrentPower;
    public float InjuryReductionRate; // 减伤率
    [SerializeField] float DamageBoostRate = 1;//增伤率
    public float spellDuration = 15f; // 施法持续时间
    public float spellCooldown = 20f; // 施法冷却时间

    [Header("受伤无敌")]
    public float invulnerableDuration;
    [HideInInspector] public float invulnerableCounter;
    public bool invulnerable;
    public PlayerController playerController;

    public UnityEvent<Transform> OntakeDamge;
    public UnityEvent OnDie;

    public UnityEvent<character> OnhealthChage;
    [Header("能量恢复")]
    public float powerRecoveryRate; // 每秒恢复量

    void Awake() {
        playerController = GetComponent<PlayerController>();
    }
    private void Start()
    {
        Currenthealth = Maxhealth;
    }
    // void OnEnable() {
    //     // NewGameEvent.OnEventRaised += NewGame;
    //     // sceneandcharacteEventSO.OnEventRaised += onsceneandcharacteEventSO;
    //     // ShurikenAttackEvent.OnEventRaised += ONShurikenAttack;
    //     // iSaveable saveable = this;
    //     // saveable.RegisterSaveData();
    // }
    // void OnDisable() {

    //     NewGameEvent.OnEventRaised -= NewGame;
    //     // sceneandcharacteEventSO.OnEventRaised -= onsceneandcharacteEventSO;
    //     // ShurikenAttackEvent.OnEventRaised -= ONShurikenAttack;
    //     iSaveable  saveable = this;
    //     saveable.UnRegisterSaveData();
    // }


    void NewGame()
    {
        Currenthealth = Maxhealth;
        CurrentBluehealth = MaxBluehealth;
        CurrentPower = MaxPower;

        OnhealthChage?.Invoke(this);
    }

    void Update()
    {
        // if (playerController.isDead == true)
        // {
        //     invulnerable = true;
        // }
        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }
        RecoverPower();

    }

    private void OnTriggerEnter2D(Collider2D other)
     {
        
        if (other.CompareTag("water"))
        {
            if (Currenthealth > 0)
            {
                // Debug.Log("1");
                //死亡
                Currenthealth = 0;
                OnhealthChage?.Invoke(this);
                OnDie?.Invoke();
            }

        }
        // if (other.CompareTag("thorns"))
        // {
        //     Currenthealth -= 15;
        //     OnhealthChage?.Invoke(this);
        // }        
    }


    public void Takedamge(attack attacker)
    {

        if (invulnerable)
            return;

        float finalDamage = attacker.damage * InjuryReductionRate * DamageBoostRate;//最终伤害


        if (Currenthealth - finalDamage > 0)
        {
            Currenthealth -= finalDamage;
            TriggerInvulnerable();
            // 受伤
            OntakeDamge?.Invoke(attacker.transform);
        }
        else
        {
            Currenthealth = 0;
            TriggerInvulnerable();
            // 触发死亡
            OnDie?.Invoke();
            // Debug.Log("1");
        }

        OnhealthChage?.Invoke(this);
    }
    public void RestorrHealth(float value)
    {
        if (Currenthealth == Maxhealth) return; // 如果生命值已满，直接返回

        Currenthealth += value; // 增加生命值

        // 防止超过最大生命值
        if (Currenthealth > Maxhealth)
        {
            Currenthealth = Maxhealth;
        }

        // 触发生命值变更事件
        OnhealthChage?.Invoke(this);
    }

    public void SetDamageBoostRate(float rate)
    {
        DamageBoostRate = rate;
    }

    public bool ReduceBluehealth(float amount)
    {
        if (CurrentBluehealth >= amount)
        {
            CurrentBluehealth -= amount;
            OnhealthChage?.Invoke(this);
            return true;
        }
        return false;
    }
    public bool ReducePower(float vae)
    {
        if (CurrentPower >= vae)
        {
            CurrentPower -= vae;
            OnhealthChage?.Invoke(this);
            return true;
        }
        return false;
    }

    private void RecoverPower()
    {
        if (CurrentPower < MaxPower)
        {
            CurrentPower += powerRecoveryRate * Time.deltaTime;
            if (CurrentPower > MaxPower)
            {
                CurrentPower = MaxPower;
            }
            OnhealthChage?.Invoke(this);
        }
    }


    // public void SetBlueHealthy(float rate)
    // {

    //     DamageBoostRate = rate;
    //     if(DamageBoostRate == 1.2f)
    //     {
    //         CurrentBluehealth -= 15;
    //     }
    // }
    /// <summary>
    /// 触发无敌
    /// </summary>
    private void TriggerInvulnerable()
    {
        invulnerable = true;
        invulnerableCounter = invulnerableDuration;
    }

    // public DataDefinition GetDataID()
    // {
    //     // throw new System.NotImplementedException();
    //     return GetComponent<DataDefinition>();
    // }

    // public void GetSaveData(Data data)
    // {
    //     // throw new System.NotImplementedException();
    //     if (data.characterPosDict.ContainsKey(GetDataID().ID))
    //     {
    //         data.characterPosDict[GetDataID().ID] = new Data.SerializeVector3(transform.position);
    //         data.floatSaveData[GetDataID().ID + "health"] = this.Currenthealth;
    //         data.floatSaveData[GetDataID().ID + "Bluehealth"] = this.CurrentBluehealth;
    //         data.floatSaveData[GetDataID().ID + "power"] = this.CurrentPower;
    //     }else
    //     {
    //         data.characterPosDict.Add(GetDataID().ID , new Data.SerializeVector3(transform.position));
    //         data.floatSaveData.Add(GetDataID().ID + "health", this.Currenthealth);
    //         data.floatSaveData.Add(GetDataID().ID + "Bluehealth", this.CurrentBluehealth);
    //         data.floatSaveData.Add(GetDataID().ID + "power", this.CurrentPower);
    //     }
    // }

    private void onsceneandcharacteEventSO()
    {
        throw new NotImplementedException();
    }
    // public void LoadDate(Data data)
    // {
    //     // 加载角色的状态数据，而不涉及场景传送
    //     var playID = GetDataID().ID;
    //     if (data.floatSaveData.ContainsKey(playID + "Bluehealth"))
    //     {
    //         this.CurrentBluehealth = data.floatSaveData[playID + "Bluehealth"];
    //     }

    //     if (data.floatSaveData.ContainsKey(playID + "health"))
    //     {
    //         this.Currenthealth = data.floatSaveData[playID + "health"];
    //     }

    //     if (data.floatSaveData.ContainsKey(playID + "power"))
    //     {
    //         this.CurrentPower = data.floatSaveData[playID + "power"];
    //     }

    //     // UI 更新
    //     OnhealthChage?.Invoke(this);
    // }

}
