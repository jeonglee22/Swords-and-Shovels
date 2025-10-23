using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatas", menuName = "Scriptable Objects/EnemyDatas")]
public class EnemyDatas : ScriptableObject
{
    public float maxHp = 100f;
    public int damage = 20;
    public float speed = 3.5f;
    public float traceDistance;
    public float attackDistance;

    public EnemyType type = EnemyType.Melee;
}
