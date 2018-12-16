using System;
using System.Collections.Generic;
using System.Text;

namespace CarController
{
    public interface IMotorController
    {
        void SetSpeed(double percent);
    }
}
