using UnityEngine;

public class DeathState : EnemyStates
{
    EnemyController enemyController;
    private void OnEnable()
    {
        enemyController = GetComponent<EnemyController>();
    }
    public override void OnEnter()
    {
        base.OnEnter();
        enemyController.Death();
    }
    public override void OnExit()
    {
        base.OnExit();
    }
}
