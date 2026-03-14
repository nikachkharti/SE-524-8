namespace Lecture28.OpenClosedPrinciple
{
    public interface IDiscount
    {
        double Calculate(double amount);
    }

    public class LowDiscountCalculator : IDiscount
    {
        public double Calculate(double amount)
        {
            return amount * 0.1;
        }
    }
    public class RegularDiscountCalculator : IDiscount
    {
        public double Calculate(double amount)
        {
            return amount * 0.2;

        }
    }
    public class VipDiscountCalculator : IDiscount
    {
        public double Calculate(double amount)
        {
            return amount * 0.3;

        }
    }
    public class SuperVipDiscountCalculator : IDiscount
    {
        public double Calculate(double amount)
        {
            return amount * 0.4;

        }
    }



    //public class DiscountCalculator
    //{
    //    public double Calculate(UserType customerType, double amount)
    //    {
    //        if (customerType == UserType.Low)
    //            return amount * 0.1;

    //        if (customerType == UserType.Regular)
    //            return amount * 0.2;

    //        if (customerType == UserType.Vip)
    //            return amount * 0.3;

    //        return amount;
    //    }
    //}
}
