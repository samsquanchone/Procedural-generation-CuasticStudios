using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyNPC : MonoBehaviour
{
    public string npcName = "Npc name";
    public float moveSpeed;
    public float attackRange;
    public float attackSpeed;
    public Stat damage;
    public Transform interactionPoint;

    NpcController controller;
    NpcStats stats;
    NpcInteractable interactable;

    private void Awake()
    {
        controller = gameObject.AddComponent<NpcController>(); // Init the controller
        if (moveSpeed != 0) { controller.moveSpeed = moveSpeed; }

        stats = gameObject.AddComponent<NpcStats>(); // Init the stats
        stats.attackRange = attackRange;
        stats.attackSpeed = attackSpeed;
        stats.damage = damage;

        interactable = gameObject.AddComponent<NpcInteractable>(); // Init interactable
        interactable.interactionTransform = interactionPoint;
        interactable.nameOfObject = npcName;
    }
}
