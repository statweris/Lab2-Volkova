using System;

namespace Decorator_pattern
{
    public abstract class DeliverySystem
    {
        public abstract decimal CalculateCost();
        public abstract string GetDescription();
        public abstract int GetDeliveryTime();
    }
    public class ExpressDeliverySystem : DeliverySystem
    {
        private readonly DeliverySystem _baseSystem;

        public ExpressDeliverySystem(DeliverySystem baseSystem)
        {
            _baseSystem = baseSystem;
        }

        public override decimal CalculateCost()
        {
            decimal baseCost = _baseSystem.CalculateCost();
            decimal expressSurcharge = CalculateExpressSurcharge();
            return baseCost + expressSurcharge;
        }

        public override string GetDescription()
        {
            return _baseSystem.GetDescription() + " (Экспресс)";
        }

        public override int GetDeliveryTime()
        {
            int baseTime = _baseSystem.GetDeliveryTime();
            return Math.Max(1, baseTime / 2); 
        }

        public string TrackDelivery(string trackingNumber)
        {
            return $"Статус экспресс-доставки {trackingNumber}: В пути, прибытие через {GetDeliveryTime()} дней";
        }

        public decimal CalculateExpressCost()
        {
            return CalculateCost();
        }

        private decimal CalculateExpressSurcharge()
        {
            return 10.0m;
        }
    }
    public class ExpressDeliveryDecorator : DeliverySystem //декоратор для системы доставки
    {
        private readonly ExpressDeliverySystem _expressSystem;

        public ExpressDeliveryDecorator(DeliverySystem baseSystem)
        {
            _expressSystem = new ExpressDeliverySystem(baseSystem);
        }

        public override decimal CalculateCost()
        {
            return _expressSystem.CalculateCost();
        }

        public override string GetDescription()
        {
            return _expressSystem.GetDescription();
        }

        public override int GetDeliveryTime()
        {
            return _expressSystem.GetDeliveryTime();
        }
        public string TrackDelivery(string trackingNumber)  //новые методы, которые добавляются декоратором
        {
            return _expressSystem.TrackDelivery(trackingNumber);
        }

        public decimal CalculateExpressCost()
        {
            return _expressSystem.CalculateExpressCost();
        }
    }

    public class CourierDelivery : DeliverySystem
    {
        public override decimal CalculateCost() => 5.0m;
        public override string GetDescription() => "Курьерская доставка";
        public override int GetDeliveryTime() => 3;
    }

    public class PostalDelivery : DeliverySystem
    {
        public override decimal CalculateCost() => 2.5m;
        public override string GetDescription() => "Почтовая доставка";
        public override int GetDeliveryTime() => 7;
    }

    public class PickupDelivery : DeliverySystem
    {
        public override decimal CalculateCost() => 0m;
        public override string GetDescription() => "Самовывоз";
        public override int GetDeliveryTime() => 1;
    }

    internal class ProgramDecoratorPattern
    {
        static void Main(string[] args)
        {
            DeliverySystem courier = new CourierDelivery(); //базовые системы доставки
            DeliverySystem postal = new PostalDelivery();
            DeliverySystem pickup = new PickupDelivery();

            Console.WriteLine("=== Базовые способы доставки ==="); //демонстрация базовых способов доставки
            TestDeliverySystem(courier);
            TestDeliverySystem(postal);
            TestDeliverySystem(pickup);

            Console.WriteLine("\n=== Экспресс-доставка через декоратор ===");

            DeliverySystem expressCourier = new ExpressDeliveryDecorator(new CourierDelivery());  //экспресс-доставка через декоратор
            DeliverySystem expressPostal = new ExpressDeliveryDecorator(new PostalDelivery());
            DeliverySystem expressPickup = new ExpressDeliveryDecorator(new PickupDelivery());

            TestExpressDelivery(expressCourier);
            TestExpressDelivery(expressPostal);
            TestExpressDelivery(expressPickup);

            Console.ReadLine();
        }

        static void TestDeliverySystem(DeliverySystem system)
        {
            Console.WriteLine($"Способ: {system.GetDescription()}");
            Console.WriteLine($"Стоимость: {system.CalculateCost()} руб.");
            Console.WriteLine($"Срок доставки: {system.GetDeliveryTime()} дней");
            Console.WriteLine();
        }

        static void TestExpressDelivery(DeliverySystem system)
        {
            if (system is ExpressDeliveryDecorator express)
            {
                Console.WriteLine($"Способ: {system.GetDescription()}");
                Console.WriteLine($"Стоимость: {system.CalculateCost()} руб.");
                Console.WriteLine($"Срок доставки: {system.GetDeliveryTime()} дней");

                string trackingNumber = "ORD" + DateTime.Now.Ticks;
                Console.WriteLine($"Отслеживание: {express.TrackDelivery(trackingNumber)}");
                Console.WriteLine($"Стоимость экспресс-доставки: {express.CalculateExpressCost()} руб.");
                Console.WriteLine();
            }
        }
    }
    
}
