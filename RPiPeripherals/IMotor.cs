namespace RPiPeripherals
{
    public interface IMotor
    {
        void Set(double speed, MotorMode mode);
        void SetDirection(MotorMode mode);
        void SetSpeed(double speed);
    }
}
