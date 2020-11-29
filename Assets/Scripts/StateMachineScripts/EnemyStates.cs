
using UnityEngine;

public class EnemyStates : MonoBehaviour
{
    protected EnemyStatesController enemyStatesController;

    protected virtual void Awake()
    {
        enemyStatesController = GetComponent<EnemyStatesController>();
    }

    public virtual void OnEnter()
    {
        this.enabled = true;
    }
    public virtual void OnExit()
    {
        this.enabled = false;
    }
}
