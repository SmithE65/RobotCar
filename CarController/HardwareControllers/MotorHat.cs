using System;
using System.Collections.Generic;
using System.Device.I2c;

namespace CarController
{
    public class MotorHat
    {
        private const ushort defaultAddress = 0x60;

        public MotorHat(ushort i2cAddress)
        {

        }

        public MotorHat() : this(defaultAddress)
        {

        }
    }
}
