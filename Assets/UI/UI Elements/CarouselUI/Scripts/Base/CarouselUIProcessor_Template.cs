using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarouselUI
{
    public class CarouselUIProcessor_Template : CarouselUI_Processor_Base
    {
        //ADDITIONAL VARIABLES HERE TO ALLOW FOR CONTROL (REFER TO CAROUSELUIPROCESSOR_PLAYERPREFS)

        protected override void UpdateCarouselUI()
        {
            // UPDATES THE ASSOCIATED CAROUSEL UI, FIRES ON ENABLE AS WELL AS FROM CAROUSEL ELEMENT INTERACTION
            //_storedSettingsIndex = STORE UPDATED VALUE IN THIS VARIABLE

            base.UpdateCarouselUI(); //NEEDED BECAUSE THIS ALLOWS UPDATING OF THE ASSOCIATED CAROUSEL ELEMENT INDEX
        }

        protected override void DetermineOutput(int input)
        {
            //THIS IS WHERE OUTPUT CODE GOES. I.E WHAT HAPPENS AFTER THE CAROUSEL HAS BEEN UPDATED
        }
    }
}