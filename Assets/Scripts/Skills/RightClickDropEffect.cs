using System;
using UnityEngine;
using UnityEngine.AI;

public class RightClickDropEffect : MonoBehaviour
{
    public Camera cam;
    public GameObject fireEffectPrefab;
    public Transform player;

    public float maxRayDistance = 200f;
    public float navMeshSampleMaxDist = 2f;   // NavMesh 스냅 반경
    public float playerHeightOffset = 0.0f;

    public float fireSpeed = 20f;
    public float destroyDelay = 0.1f;
    public float fireRange = 30f;

    public float forwardOffset = 10.0f;

    public LayerMask wallMask;

    private void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        if (cam == null || fireEffectPrefab == null || player == null) return;
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 basePos;

        //1) nav mesh hit + nav mesh snap
        if (Physics.Raycast(camRay, out var hit, maxRayDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            basePos = hit.point;
            if (NavMesh.SamplePosition(hit.point, out var nHit, navMeshSampleMaxDist, NavMesh.AllAreas))
                basePos = nHit.position;
        }else //2) nav mesh miss
        {
            basePos = camRay.origin + camRay.direction * fireRange;
        }

        //발사 시작
        Vector3 spawnPos = player.position + Vector3.up * playerHeightOffset;
        Vector3 dir = (basePos - spawnPos).normalized;

        //벽 clamp
        float safeOffset = forwardOffset;
        if (Physics.Raycast(spawnPos, dir, out var block, forwardOffset + 0.05f, wallMask, QueryTriggerInteraction.Ignore))
            safeOffset = Mathf.Max(0f, block.distance - 0.02f);

        spawnPos += dir * forwardOffset;

        //회전
        var fx = Instantiate(fireEffectPrefab, spawnPos, Quaternion.LookRotation(dir));
        //이동, 충돌
        var mover = fx.GetComponent<FireEffectMover>();
        if (mover == null) mover = fx.AddComponent<FireEffectMover>();
       // mover.wallMask = wallMask;          
        mover.Init(dir, fireSpeed, fireRange, destroyDelay);
    }
}
