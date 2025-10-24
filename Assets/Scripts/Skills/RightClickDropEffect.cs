using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class RightClickDropEffect : MonoBehaviour
{
    public Camera cam;
    public Transform player;
    public WeaponChanger weaponChanger;

    public GameObject fireEffectPrefab;
    public GameObject iceEffectPrefab; 
    public GameObject forestEffectPrefab;

    public float maxRayDistance = 200f;
    public float navMeshSampleMaxDist = 2f;   // NavMesh ���� �ݰ�
    public float playerHeightOffset = 0.0f;

    //���� �߻�
    public float fireSpeed = 20f;
    public float destroyDelay = 0.1f;
    public float fireRange = 30f;
    public float forwardOffset = 10.0f;

    //�ٴ� ��ȯ
    public float groundYOffset = 0.04f; //skill 3
    public float forestExtraYOffset = 0.5f; //skill 4
    public float groundRayStartHeight = 2.0f;
    public float groundRayDistance = 50f;
    public Vector3 groundRotationOffset = Vector3.zero;

    public LayerMask wallMask;    
    public LayerMask groundMask;   
    public LayerMask ceilingMask;

    private void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    private void Update()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return; //UI ����
        if (!Input.GetMouseButtonDown(1)) return;
        if (cam == null || weaponChanger == null || player == null) return;

        switch (weaponChanger.CurrentWeaponIndex)
        {
            case 1:
                return; 

            case 2:
                FireForward();
                break;

            case 3:
                PlaceOnGround(iceEffectPrefab);
                break;

            case 4:
                PlaceOnGround(forestEffectPrefab, 0f);
                break;
        }
    }

    private void FireForward() //���� ��ų
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 basePos;

        if (Physics.Raycast(camRay, out var hit, maxRayDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            basePos = hit.point;
            if (NavMesh.SamplePosition(hit.point, out var nHit, navMeshSampleMaxDist, NavMesh.AllAreas))
                basePos = nHit.position;
        }
        else
        {
            basePos = camRay.origin + camRay.direction * fireRange;
        }

        // �߻� ������/����
        Vector3 spawnPos = player.position + Vector3.up * playerHeightOffset;
        Vector3 dir = (basePos - spawnPos).normalized;

        // ���� �浹 Ŭ���� �� forwardOffset ��� safeOffset ���� ����
        float safeOffset = forwardOffset;
        if (Physics.Raycast(spawnPos, dir, out var block, forwardOffset + 0.05f, wallMask, QueryTriggerInteraction.Ignore))
            safeOffset = Mathf.Max(0f, block.distance - 0.02f);

        spawnPos += dir * safeOffset;

        var fx = Instantiate(fireEffectPrefab, spawnPos, Quaternion.LookRotation(dir));
        var mover = fx.GetComponent<FireEffectMover>() ?? fx.AddComponent<FireEffectMover>();
        mover.Init(dir, fireSpeed, fireRange, destroyDelay);
    }

    private void PlaceOnGround(GameObject prefab, float extraOffset = 0f)
    {
        if (prefab == null) return;

        Vector3 rayStart = player.position + Vector3.up * (playerHeightOffset + groundRayStartHeight);
        Vector3 spawnPos = player.position + Vector3.up * playerHeightOffset; // �⺻��

        // �ٴ� ���� ã��
        if (Physics.Raycast(rayStart, Vector3.down, out var downHit, groundRayDistance, groundMask, QueryTriggerInteraction.Ignore))
        {
            spawnPos = downHit.point + Vector3.up * (groundYOffset + extraOffset);
        }
        // ���� �� NavMesh ����
        else if (NavMesh.SamplePosition(player.position, out var nHit, navMeshSampleMaxDist, NavMesh.AllAreas))
        {
            spawnPos = nHit.position + Vector3.up * (groundYOffset + extraOffset);
        }
        else
        {
            // ���и� ���� ��ġ���� ��¦ ����
            spawnPos = player.position + Vector3.up * (playerHeightOffset + groundYOffset + extraOffset);
        }

        // ������ ȸ��(�ʿ� �� �����¸� ����)
        Quaternion rot = Quaternion.Euler(groundRotationOffset);

        Instantiate(prefab, spawnPos, rot);
    }
}
