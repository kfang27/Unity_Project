using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMoveable
{
    void RotateTowardsPlayer();

    void MoveTowardsPlayer();

    bool IsFacingPlayer();
}
