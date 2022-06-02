using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE
public class MooseAnimal : Animal
{

    protected override void GetCaught(AnimalType caughtBy)
    {
        throw new System.NotImplementedException();
    }

    public override void Search()
    {
        // Moose type has now been cut from game

        //visualField.FindPursuingTarget();
        base.Search();
    }
}
