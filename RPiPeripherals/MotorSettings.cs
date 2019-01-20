using System;
using System.Collections.Generic;
using System.Text;

namespace RPiPeripherals
{
    public class MotorSettings
    {
        public PCA9685 PwmController { get; set; }
        public int PwmPin { get; set; }
        public int InputPin1 { get; set; }
        public int InputPin2 { get; set; }
    }
}
