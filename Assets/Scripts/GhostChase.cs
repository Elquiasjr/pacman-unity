using Mono.Cecil;
using UnityEngine;

public class GhostChase : GhostBehavior
{
    public TypeChase specificChase { get; private set; }
    private void OnDisable()
    {
        ghost.scatter.Enable();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        specificChase.Chase(other, ghost);
    }

    private void OnEnable()
    {
        specificChase = GetComponent<TypeChase>();

    }
}
