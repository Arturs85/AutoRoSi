using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using GHIElectronics.NETMF.FEZ;
using System.Threading;

namespace ButtonClicker
{
    // TODO create setup() and loop() method
    // TODO implement same functionality with InterruptPort


    public class Program
    {
        public static void Main()
        {
            Setup();
            while (true) Loop();
        }
        private static InputPort button;
        private static OutputPort led;


        public static void Setup()
        {
            // Izveido ieejas portu uz nor�d�t�s CPU k�jas 
            button = new InputPort(
                (Cpu.Pin)FEZ_Pin.Digital.LDR,   // LDR - Loader
                false, // Trok��u filtrs: false -> izsl�gts, true -> iesl�gts 
                Port.ResistorMode.PullUp); // Uzst�d�t k�ju uz high, kad nekas nav pievienots 

            led = new OutputPort(
                (Cpu.Pin)FEZ_Pin.Digital.LED,
                false);

        }

        public static void Loop()
        {
            // Nolas�t k�jas st�vokli : -true, high, false -> low Kad LDR poga ir nospiesta (jo ir pull-up mode),
            // t� savieno LDR k�ju ar zemi, iestatot to uz low. 
            bool buttonPressed = !button.Read();
            if (buttonPressed)
            {

                led.Write(!led.Read());
                Debug.Print("Button pressed");

            }
            Thread.Sleep(100);
        }

        
    }
}

