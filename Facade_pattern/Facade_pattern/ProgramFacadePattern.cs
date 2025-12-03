using System;
using System.Collections.Generic;
using System.Linq;

namespace Facade_pattern
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Total { get; set; }
    }


    public class UserDatabase
    {
        private List<User> _users = new List<User>
    {
        new User { Id = 1, Name = "User1", Email = "user1@mail.com" },
        new User { Id = 2, Name = "User2", Email = "user2@mail.com" }
    };

        public User GetUser(int id)
        {
            Console.WriteLine($"[UserDB] Получение пользователя {id}");
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public void SaveUser(User user)
        {
            Console.WriteLine($"[UserDB] Сохранение пользователя {user.Name}");
            var existing = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existing != null)
                _users.Remove(existing);
            _users.Add(user);
        }
    }

    public class OrderDatabase
    {
        private List<Order> _orders = new List<Order>
    {
        new Order { Id = 1, UserId = 1, Total = 100.50m },
        new Order { Id = 2, UserId = 1, Total = 75.25m }
    };
        private int _nextId = 3;

        public Order GetOrder(int id)
        {
            Console.WriteLine($"[OrderDB] Получение заказа {id}");
            return _orders.FirstOrDefault(o => o.Id == id);
        }

        public void SaveOrder(Order order)
        {
            Console.WriteLine($"[OrderDB] Сохранение заказа {order.Id}");
            if (order.Id == 0)
            {
                order.Id = _nextId++;
                _orders.Add(order);
            }
            else
            {
                var existing = _orders.FirstOrDefault(o => o.Id == order.Id);
                if (existing != null)
                {
                    existing.Total = order.Total;
                    existing.UserId = order.UserId;
                }
            }
        }

        public List<Order> GetUserOrders(int userId)
        {
            Console.WriteLine($"[OrderDB] Получение заказов пользователя {userId}");
            return _orders.Where(o => o.UserId == userId).ToList();
        }
    }

    public class DatabaseFacade
    {
        private readonly UserDatabase _userDb;
        private readonly OrderDatabase _orderDb;

        public DatabaseFacade()
        {
            _userDb = new UserDatabase();
            _orderDb = new OrderDatabase();
        }

        public User GetUserWithOrders(int userId)
        {
            Console.WriteLine($"\n=== Получение пользователя {userId} с заказами ===");
            var user = _userDb.GetUser(userId);
            var orders = _orderDb.GetUserOrders(userId);

            Console.WriteLine($"Пользователь: {user.Name}, Заказов: {orders.Count}");
            return user;
        }

        public void CreateOrder(int userId, decimal total)
        {
            Console.WriteLine($"\n=== Создание заказа для пользователя {userId} ===");
            var user = _userDb.GetUser(userId);
            var order = new Order { UserId = userId, Total = total };

            _orderDb.SaveOrder(order);
            Console.WriteLine($"Создан заказ на сумму {total} для {user.Name}");
        }

        public decimal GetUserTotalSpent(int userId)
        {
            Console.WriteLine($"\n=== Расчет общей суммы покупок пользователя {userId} ===");
            var orders = _orderDb.GetUserOrders(userId);
            decimal total = 0;

            foreach (var order in orders)
            {
                total += order.Total;
            }

            Console.WriteLine($"Общая сумма: {total}");
            return total;
        }
    }

    internal class ProgramFacadePattern
    {
        static void Main(string[] args)
        {
            var database = new DatabaseFacade();

            database.GetUserTotalSpent(1);

            database.GetUserWithOrders(1);
            database.CreateOrder(1, 250.75m);

            database.GetUserTotalSpent(1);
        }
    }
}
