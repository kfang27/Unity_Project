using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerCheckable
{
    bool IsAggroed { get; set; }
    bool IsWithinStrikingDistance { get; set; }
    bool IsAttacking { get; set; }
    void SetAggroStatus(bool isAggroed);
    void SetWithinStrikingDistance(bool isWithinStrikingDistance);
    void SetDoingComboStatus(bool isDoingCombo);
    void SetAttackingStatus(int isAttacking);
}
