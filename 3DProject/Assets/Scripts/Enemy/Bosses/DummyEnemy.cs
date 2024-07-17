using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : Enemy
{
    const int NUMBER_OF_COMBOS = 2;
    public override int ChooseRandomCombo()
    {
        return base.ChooseRandomCombo() % NUMBER_OF_COMBOS;
    }
}
