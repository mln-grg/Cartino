using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public class EnemyChase : EnemyStates
{
    EnemyController enemyController;
    private void OnEnable()
    {
        enemyController = GetComponent<EnemyController>();
    }
    public override void OnEnter()
    {
        base.OnEnter();
        enemyController.Chase();
    }
    public override void OnExit()
    {
        base.OnExit();
    }
    private void Update()
    {
        
    }
}
