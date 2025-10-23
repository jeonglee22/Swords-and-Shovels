using UnityEngine;
using UnityEngine.AI;

public class RightClickDropEffect : MonoBehaviour
{
    public Camera cam;
    public GameObject fireEffectPrefab;
    public Transform player;

    public float maxRayDistance = 200f;
    public float navMeshSampleMaxDist = 2f;   // NavMesh ���� �ݰ�
    public float playerHeightOffset = 0.0f;

    public float fallSpeed = 60f;
    public float destroyDelay = 0.1f;

    private void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        if (cam == null || fireEffectPrefab == null) return;
        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out var hit, maxRayDistance, ~0, QueryTriggerInteraction.Ignore)) return;

        //NavMesh Snap -> nav mesh ���� ��� ���� ����� �������� ����
        Vector3 basePos = hit.point;
        if (NavMesh.SamplePosition(hit.point, out var nHit, navMeshSampleMaxDist, NavMesh.AllAreas)) basePos = nHit.position;

        Vector3 spawnPos = new Vector3(basePos.x, player.position.y + playerHeightOffset, basePos.z);
        var fx = Instantiate(fireEffectPrefab, spawnPos, Quaternion.identity);

        //test: �ٴ� ���� �ϰ� �� ����
        fx.AddComponent<TestFallAndDie>().Init(basePos, fallSpeed, destroyDelay);

        Debug.DrawLine(spawnPos, basePos, Color.yellow, 1.5f);
    }
}
