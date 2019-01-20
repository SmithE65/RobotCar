using System;
using System.Device.I2c;
using System.Device.I2c.Drivers;
using System.Threading;

namespace CarController
{
    public class PCA9685
    {
        private const ushort counterMax = 4095;
        private bool isInitialized = false;
        private readonly I2cDevice device;

        public PCA9685(I2cDevice _device)
        {
            device = _device;
        }

        public void Initialize()
        {
            if (isInitialized)
                return;

            isInitialized = true;

            // Make sure everything's stopped before we wake up
            SetAllPins(0d);

            // Get the contents of the MODE1 register
            const byte mode1 = (byte)Registers.MODE1;
            device.WriteByte(mode1);
            byte b = device.ReadByte();

            WriteRegister(mode1, (byte)((b & 0x7F) | 0x10));  // Setting bit 4 low puts the clock to sleep

            WriteRegister((byte)Registers.PRE_SCALE, 0x03);  // Set max frequency for chip

            // Reset the devices
            WriteRegister(mode1, b); // Set MODE1 back to how it was before
            Thread.Sleep(1); // Only have to wait 500 microseconds but 1ms is easier to write
            WriteRegister(mode1, (byte)(b | 0x81)); // Set bit 7 high to restart output after waking up
        }

        //public void Sleep()
        //{

        //}

        //public void Wake()
        //{

        //}

        //public void Restart()
        //{

        //}

        /// <summary>
        /// Reads the contents of the MODE1 register
        /// </summary>
        /// <returns>byte representing MODE1 flags</returns>
        public byte GetMode1()
        {
            device.WriteByte((byte)Registers.MODE1);
            return device.ReadByte();
        }

        /// <summary>
        /// Reads the contents of the MODE2 register
        /// </summary>
        /// <returns>byte representing MODE2 flags</returns>
        public byte GetMode2()
        {
            device.WriteByte((byte)Registers.MODE2);
            return device.ReadByte();
        }

        public void SetAllPins(double dutyCycle)
        {
            PWMRegister register = new PWMRegister() { FirstRegister = 250 };
            CalculateDutyCycle(dutyCycle, ref register);
            SetPinInternal(register);
        }

        public void SetPin(int pin, double dutyCycle)
        {
            if (pin < 0 || pin > 15)
                return;

            PWMRegister register = new PWMRegister(pin);
            CalculateDutyCycle(dutyCycle, ref register);
            SetPinInternal(register);
        }

        private void SetPinInternal(PWMRegister register)
        {
            if (register.FirstRegister < 0 || register.FirstRegister > 69)
                return;

            // Write all four registers for pin
            WriteRegister(register.FirstRegister, register.OnLow);
            WriteRegister((byte)(register.FirstRegister + 1), register.OnHigh);
            WriteRegister((byte)(register.FirstRegister + 2), register.OffLow);
            WriteRegister((byte)(register.FirstRegister + 3), register.OffHigh);
        }

        private void CalculateDutyCycle(double cycle, ref PWMRegister register)
        {
            if (cycle >= 1d)
            {
                register.OnLow = 0x00;
                register.OnHigh = 0x10;  // bit #4 high for all on
                register.OffLow = 0x00;
                register.OffHigh = 0x00;
                return;
            }

            register.OnLow = 0x00;
            register.OnHigh = 0x00;

            if (cycle <= 0d)
            {
                register.OffLow = 0x00;
                register.OffHigh = 0x10;  // bit #4 high for all off
                return;
            }

            ushort onTime = (ushort)(cycle * counterMax);
            register.OffLow = (byte)(onTime & 0xFF);
            register.OffHigh = (byte)(onTime >> 8);
        }

        private void WriteRegister(byte register, byte data)
        {
            if (device == null)
                return;

            device.Write(new byte[] { register, data });
        }

        private class PWMRegister
        {
            public PWMRegister() { }

            public PWMRegister(int pin)
            {
                FirstRegister = GetFirstRegisterFromPin(pin);
            }

            private byte GetFirstRegisterFromPin(int pin)
            {
                return (byte)(pin * 4 + 6);
            }

            public byte FirstRegister { get; set; }
            public byte OnLow { get; set; }
            public byte OnHigh { get; set; }
            public byte OffLow { get; set; }
            public byte OffHigh { get; set; }
        }

        private enum Registers : byte
        {
            MODE1 = 0x00,
            MODE2 = 0x01,
            // ...
            // sub addresses and all address
            // ...
            LED0_ON_L = 0x06,
            LED0_ON_H = 0x07,
            // etc...
            PRE_SCALE = 0xFE
        };

        [Flags]
        private enum Mode1Flags : byte
        {
            AllCall = 0x01,
            SUB3 = 0x02,
            SUB2 = 0x04,
            SUB1 = 0x08,
            Sleep = 0x10,
            AutoIncrement = 0x20,
            ExternalClock = 0x40,
            Restart = 0x80
        };

        [Flags]
        private enum Mode2Flags : byte
        {
            OutputsTotemPole = 0x04,
            OutputsChangeOnAck = 0x08,
            InvertOutputs = 0x10
        };
    }
}
