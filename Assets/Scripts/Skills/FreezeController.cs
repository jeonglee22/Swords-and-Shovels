using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class FreezeController : MonoBehaviour
{
    private float frozenUntil = -1f;
    private bool isFrozen = false;

    NavMeshAgent agent;
    Rigidbody rb;

    bool agentPrevStopped;
    bool agentPrevUpdatePos;
    bool agentPrevUpdateRot;

    bool rbPrevKinematic;
    RigidbodyConstraints rbPrevConstraints;

    Transform prevParent;

    readonly List<Behaviour> pausedBehaviours = new List<Behaviour>();

    Animator[] animators; bool[] animatorsPrevEnabled;
    Animation[] legacyAnims; bool[] legacyAnimsPrevEnabled;
    PlayableDirector[] directors; PlayState[] directorsPrevState;

    static readonly HashSet<System.Type> BehaviourWhitelist = new HashSet<System.Type>
    {
        typeof(FreezeController),
        typeof(NavMeshAgent),
    };

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        animators = GetComponentsInChildren<Animator>(true);
        legacyAnims = GetComponentsInChildren<Animation>(true);
        directors = GetComponentsInChildren<PlayableDirector>(true);
    }

    public void ApplyFreeze(float seconds)
    {
        float until = Time.time + Mathf.Max(0.01f, seconds);
        if (until > frozenUntil) frozenUntil = until;

        if (!isFrozen)
        {
            EnterFreeze();
            StartCoroutine(FreezeTick());
        }
    }

    IEnumerator FreezeTick()
    {
        isFrozen = true;
        while (Time.time < frozenUntil) yield return null;
        ExitFreeze();
        isFrozen = false;
        frozenUntil = -1f;
        Destroy(this);
    }

    void EnterFreeze()
    {
        prevParent = transform.parent;
        if (prevParent != null)
        {
            transform.SetParent(null, true);
        }

        if (agent)
        {
            agentPrevStopped = agent.isStopped;
            agentPrevUpdatePos = agent.updatePosition;
            agentPrevUpdateRot = agent.updateRotation;

            agent.isStopped = true;
            agent.updatePosition = false;  
            agent.updateRotation = false;    
            agent.ResetPath();
            agent.velocity = Vector3.zero;
        }

        if (rb)
        {
            rbPrevKinematic = rb.isKinematic;
            rbPrevConstraints = rb.constraints;

            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        if (animators != null && animators.Length > 0)
        {
            animatorsPrevEnabled = new bool[animators.Length];
            for (int i = 0; i < animators.Length; i++)
            {
                var a = animators[i];
                if (!a) continue;
                animatorsPrevEnabled[i] = a.enabled;
                a.enabled = false;
            }
        }

        if (legacyAnims != null && legacyAnims.Length > 0)
        {
            legacyAnimsPrevEnabled = new bool[legacyAnims.Length];
            for (int i = 0; i < legacyAnims.Length; i++)
            {
                var la = legacyAnims[i];
                if (!la) continue;
                legacyAnimsPrevEnabled[i] = la.enabled;
                la.enabled = false;
            }
        }

        if (directors != null && directors.Length > 0)
        {
            directorsPrevState = new PlayState[directors.Length];
            for (int i = 0; i < directors.Length; i++)
            {
                var d = directors[i];
                if (!d) continue;
                directorsPrevState[i] = d.state;
                d.Pause();
            }
        }

        var behaviours = GetComponentsInChildren<Behaviour>(true);
        foreach (var b in behaviours)
        {
            if (b == null || b == this) continue;
            if (b is Animator || b is Animation || b is PlayableDirector) continue;
            if (BehaviourWhitelist.Contains(b.GetType())) continue;
            if (b is IDamagable) continue;

            if (b.enabled)
            {
                b.enabled = false;
                pausedBehaviours.Add(b);
            }
        }
    }

    void ExitFreeze()
    {
        for (int i = 0; i < pausedBehaviours.Count; i++)
        {
            var b = pausedBehaviours[i];
            if (b) b.enabled = true;
        }
        pausedBehaviours.Clear();

        if (animators != null && animatorsPrevEnabled != null)
        {
            for (int i = 0; i < animators.Length; i++)
            {
                var a = animators[i];
                if (a) a.enabled = animatorsPrevEnabled[i];
            }
        }
        if (legacyAnims != null && legacyAnimsPrevEnabled != null)
        {
            for (int i = 0; i < legacyAnims.Length; i++)
            {
                var la = legacyAnims[i];
                if (la) la.enabled = legacyAnimsPrevEnabled[i];
            }
        }

        if (directors != null && directorsPrevState != null)
        {
            for (int i = 0; i < directors.Length; i++)
            {
                var d = directors[i];
                if (!d) continue;
                if (directorsPrevState[i] == PlayState.Playing) d.Play();
                else d.Pause();
            }
        }

        if (rb)
        {
            rb.isKinematic = rbPrevKinematic;
            rb.constraints = rbPrevConstraints;
            rb.WakeUp();
        }
        if (agent)
        {
            agent.updatePosition = agentPrevUpdatePos;
            agent.updateRotation = agentPrevUpdateRot; 
            agent.isStopped = agentPrevStopped;
        }

        if (prevParent != null)
        {
            transform.SetParent(prevParent, true);
        }
    }
}
