using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToMedicineButton : MonoBehaviour
{
    public GamePlay1 gameplay1;

    private void OnMouseDown()
    {
        gameplay1.GoToMedicine();
    }
}
