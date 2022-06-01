using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MooseAnimal : Animal
{

    protected override void GetCaught(AnimalType caughtBy)
    {
        throw new System.NotImplementedException();
    }

    public override void Search()
    {
        visualField.FindPursuingTarget();
        base.Search();
    }
}
