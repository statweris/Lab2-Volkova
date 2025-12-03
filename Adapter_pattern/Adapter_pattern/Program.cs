using System;

namespace Adapter_pattern
{
    internal class Program
    {
        public interface ITransport
        {
            void Drive();
        }
        public class Car : ITransport
        {
            public void Drive()
            {
                Console.WriteLine("Машина едет по дороге");
            }
        }
        class Driver 
        {
            public void Travel(ITransport transport)
            {
                transport.Drive();
            }
        }
        interface IAnimal
        {
            void Move();
            void Eat();
        }
        public class Donkey: IAnimal //класс осла (не является транспортом)
        {
            public void Eat()
            {
                Console.WriteLine("Осёл ест сено");
            }

            public void Move()
            {
                Console.WriteLine("Осёл идёт медленно по полю");
            }
        }

        public class Saddle : ITransport //адаптер седло
        {
            private readonly Donkey _donkey;

            public Saddle(Donkey donkey)
            {
                _donkey = donkey;
            }

            public void Drive()
            {
                _donkey.Move();
            }
        }

        static void Main(string[] args)
        {
            Driver driver = new Driver();
            ITransport car = new Car();
            driver.Travel(car);

            Console.WriteLine();

            Donkey donkey = new Donkey(); //осёл с седлом
            ITransport donkeyWithSaddle = new Saddle(donkey);
            driver.Travel(donkeyWithSaddle);

            Console.WriteLine();
            donkey.Eat();
        }
    }
}
