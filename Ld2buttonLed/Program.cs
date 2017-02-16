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
            // Izveido ieejas portu uz norâdîtâs CPU kâjas 
            button = new InputPort(
                (Cpu.Pin)FEZ_Pin.Digital.LDR,   // LDR - Loader
                false, // Trokðòu filtrs: false -> izslçgts, true -> ieslçgts 
                Port.ResistorMode.PullUp); // Uzstâdît kâju uz high, kad nekas nav pievienots 

            led = new OutputPort(
                (Cpu.Pin)FEZ_Pin.Digital.LED,
                false);

        }

        public static void Loop()
        {
            // Nolasît kâjas stâvokli : -true, high, false -> low Kad LDR poga ir nospiesta (jo ir pull-up mode),
            // tâ savieno LDR kâju ar zemi, iestatot to uz low. 
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

