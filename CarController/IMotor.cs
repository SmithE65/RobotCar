using System;
using System.Collections.Generic;
using System.Text;

namespace CarController
{
    public interface IMotor
    {
        void Set(double speed, MotorMode mode);
        void SetDirection(MotorMode mode);
        void SetSpeed(double speed);
    }
}
