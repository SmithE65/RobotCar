using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Device.I2c;
using System.Device.I2c.Drivers;
using System.Threading;

using RPiPeripherals;

namespace CarController
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Setting up I2C and PWM Controller...");
            I2cConnectionSettings settings = new I2cConnectionSettings(1, 0x60);
            using (UnixI2cDevice device = new UnixI2cDevice(settings))
            {
                PCA9685 pwmController = new PCA9685(device);
                pwmController.Initialize();

                IMotor m1 = new Motor(new MotorSettings { PwmController = pwmController, PwmPin = 8, InputPin1 = 10, InputPin2 = 9 });
                IMotor m2 = new Motor(new MotorSettings { PwmController = pwmController, PwmPin = 13, InputPin1 = 11, InputPin2 = 12 });
                IMotor m3 = new Motor(new MotorSettings { PwmController = pwmController, PwmPin = 2, InputPin1 = 4, InputPin2 = 3 });
                IMotor m4 = new Motor(new MotorSettings { PwmController = pwmController, PwmPin = 7, InputPin1 = 5, InputPin2 = 6 });

                IMotor[] motors = { m1, m2, m3, m4 };


                Console.WriteLine("Running all motors...");
                foreach (IMotor m in motors)
                {
                    m.SetDirection(MotorMode.Forward);
                    for (int i = 0; i < 50; i++)
                    {
                        m.SetSpeed(i * 0.02);
                        Thread.Sleep(20);
                    }
                    for (int i = 50; i > 0; i--)
                    {
                        m.SetSpeed(i * 0.02);
                        Thread.Sleep(20);
                    }
                    m.SetDirection(MotorMode.Free);
                }
                Console.WriteLine("All stop.");

                pwmController.SetAllPins(0d);

                #region Deleted Code
                //byte[] bytes = { 0xFA, 0x00 };
                //device.Write(bytes);

                //bytes[0] = 0xFB;
                //bytes[1] = 0x00;
                //device.Write(bytes);

                //bytes[0] = 0xFC;
                //bytes[1] = 0x00;
                //device.Write(bytes);

                //bytes[0] = 0xFD;
                //bytes[1] = 0x00;
                //device.Write(bytes);

                //Console.WriteLine("Setting up device.");
                //bytes[0] = 0x01;
                //bytes[1] = 0x04;
                //device.Write(bytes);

                //bytes[0] = 0x00;
                //bytes[1] = 0x01;
                //device.Write(bytes);
                //Thread.Sleep(1);

                //bytes[0] = 0x00;
                //bytes[1] = 0x01;
                //device.Read(bytes);
                //Console.WriteLine($"Read {bytes[1]} from {bytes[0]}.");

                //bytes[0] = 0x00;
                //bytes[1] = (byte)(bytes[1] & 0xEF);
                //device.Write(bytes);
                //Thread.Sleep(1);

                //Console.WriteLine($"Something about PWM frequency...");
                //int prescale = (int)(25000000.0 / 4096.0 / 1600.0 - 1.0);
                //byte[] psbytes = new byte[2];
                //psbytes[0] = (byte)(prescale >> 4);
                //psbytes[1] = (byte)(prescale & 0xFF);
                //device.WriteByte(0x00);
                //byte b = device.ReadByte();
                //Console.WriteLine($"Read {b}");

                //byte b2 = (byte)(b & 0x7F | 0x10);
                //Write8(device, 0x00, b2);
                //device.Write(new byte[] { 0xFE, psbytes[0], psbytes[1] });
                //Write8(device, 0x00, b);
                //Thread.Sleep(1);
                //Write8(device, 0x00, (byte)(b | 0x80));

                //Thread.Sleep(10);
                //Console.WriteLine("Trying to run a motor....");
                //Write8(device, 0x12, 0x00);
                //Write8(device, 0x13, 0x00);
                //Write8(device, 0x14, 0xFF);
                //Write8(device, 0x15, 0x0F);
                //Write8(device, 0x16, 0xFF);
                //Write8(device, 0x17, 0x0F);
                //Write8(device, 0x18, 0x00);
                //Write8(device, 0x19, 0x00);
                //Write8(device, 0x0E, 0x00);
                //Write8(device, 0x0F, 0x00);
                //Write8(device, 0x10, 0xF0);
                //Write8(device, 0x11, 0x0F);


                //Thread.Sleep(3000);

                //Console.WriteLine("Trying to stop the motor...");
                //bytes[0] = 0xFA;
                //bytes[1] = 0x00;
                //device.Write(bytes);

                //bytes[0] = 0xFB;
                //bytes[1] = 0x00;
                //device.Write(bytes);

                //bytes[0] = 0xFC;
                //bytes[1] = 0x00;
                //device.Write(bytes);

                //bytes[0] = 0xFD;
                //bytes[1] = 0x00;
                //device.Write(bytes);
                #endregion
            }

            Console.WriteLine("End of application.");
        }
    }
}
