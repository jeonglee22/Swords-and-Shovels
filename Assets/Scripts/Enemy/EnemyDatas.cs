using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatas", menuName = "Scriptable Objects/EnemyDatas")]
public class EnemyDatas : ScriptableObject
{
    public float maxHp = 100f;
    public float damage = 20f;
    public float speed = 3.5f;
    public float traceDistance;
    public float attackDistance;

    public EnemyType type = EnemyType.Melee;
}
