using System;
using System.Collections.Generic;
using System.Text;

namespace CarController
{
    public class Motor : IMotor
    {
        private PCA9685 pwm;
        private int pwmPin;
        private int input1;
        private int input2;

        public Motor(MotorSettings settings)
        {
            // Check input
            if (settings == null || settings.PwmController == null)
                throw new ArgumentException("Settings object or PWM controller is null!");
            if (settings.PwmPin == settings.InputPin1 || settings.InputPin1 == settings.InputPin2 || settings.InputPin2 == settings.PwmPin)
                throw new ArgumentException("Control pins must be unique!");

            pwm = settings.PwmController;
            pwmPin = settings.PwmPin;
            input1 = settings.InputPin1;
            input2 = settings.InputPin2;
        }

        public void Set(double speed, MotorMode mode)
        {
            SetDirection(mode);
            SetSpeed(speed);
        }

        public void SetDirection(MotorMode mode)
        {
            if (mode == MotorMode.Foreward)
            {
                Console.WriteLine($"Setting pin {input1} high, pin {input2} low (foreward)");
                pwm.SetPin(input1, 1.0);
                pwm.SetPin(input2, 0.0);
            }
            else if (mode == MotorMode.Backward)
            {
                Console.WriteLine($"Setting pin {input1} low, pin {input2} high (backward)");
                pwm.SetPin(input1, 0.0);
                pwm.SetPin(input2, 1.0);
            }
            else if (mode == MotorMode.Brake)
            {
                Console.WriteLine($"Setting pin {input1} high, pin {input2} high (brake)");
                pwm.SetPin(input1, 1.0);
                pwm.SetPin(input2, 1.0);
            }
            else if (mode == MotorMode.Free)
            {
                Console.WriteLine($"Setting pin {input1} low, pin {input2} low (free)");
                pwm.SetPin(input1, 0.0);
                pwm.SetPin(input2, 0.0);
            }
            else
            {
                throw new ArgumentException("Invalid motor mode.");
            }
        }

        public void SetSpeed(double speed)
        {
            Console.WriteLine($"Setting pin {pwmPin} to {speed*100}% duty cycle");
            pwm.SetPin(pwmPin, speed);
        }
    }
}
