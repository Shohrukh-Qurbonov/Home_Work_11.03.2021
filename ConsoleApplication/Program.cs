using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApplication
{
    class DBContext : DbContext
    {
        public DBContext(DbContextOptions options) : base(options)
        {
        }

        public DBContext()
        {
        }
        public DbSet<Customer> Customer { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data source=DESKTOP-4HBI2PP\\SQLEXPRESS; Initial catalog=Alif; Integrated security=true;");
        }
    }


    class Customer
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }
    }


    class Operation
    {
        public static void Create()
        {
            try
            {
                using (var Context = new DBContext())
                {
                    Console.Write("Имя:");
                    string FirstName = Console.ReadLine();
                    Console.Write("Фамилия:");
                    string LastName = Console.ReadLine();
                    Console.Write("Отчество:");
                    string MiddleName = Console.ReadLine();
                    Console.Write("День рождения(дд-мм-гггг):");
                    DateTime BirthDate = Convert.ToDateTime(Console.ReadLine());
                    var person = new Customer()
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        MiddleName = MiddleName,
                        BirthDate = BirthDate
                    };
                    Context.Customer.Add(person);
                    if (Context.SaveChanges() > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Успешно добавлен!");
                        Console.ResetColor();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            finally
            {
                Console.ReadKey();
            }
        }
        public static void Read(string Type = null)
        {
            try
            {
                using (var context = new DBContext())
                {
                    var PersonList = context.Customer.ToList();

                    PersonList.ForEach(p =>
                    {
                        Console.WriteLine($"ID:{p.id}\tFirstName:{p.FirstName}\tLastName:{p.LastName}\tMiddleName:{p.MiddleName}\tBirthDate:{p.BirthDate}");
                    });
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            finally
            {
                if (Type == null) Console.ReadKey();
            }

        }
        public static void Update()
        {
            try
            {
                using (var Context = new DBContext())
                {
                    Read("Update");
                    Console.WriteLine("Введите Id человека: ");
                    Console.Write("Id: ");
                    int ID = Convert.ToInt32(Console.ReadLine());
                    var person = Context.Customer.Find(ID);
                    if (person != null)
                    {
                        Console.Write("FirstName:");
                        string FirstName = Console.ReadLine();
                        Console.Write("LastName:");
                        string LastName = Console.ReadLine();
                        Console.Write("MiddleName:");
                        string MiddleName = Console.ReadLine();
                        Console.Write("BirthDate:");
                        DateTime BirthDate = Convert.ToDateTime(Console.ReadLine());
                        person.FirstName = FirstName;
                        person.LastName = LastName;
                        person.MiddleName = MiddleName;
                        person.BirthDate = BirthDate;

                        if (Context.SaveChanges() > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Успешно изменено!");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Не изменень!");
                            Console.ResetColor();
                        }
                    }
                    else Console.WriteLine("В нашу таблицу человек по такой Id не существует!");

                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            finally
            {
                Console.ReadKey();
            }
        }
        public static void Delete()
        {
            try
            {
                using (var Context = new DBContext())
                {
                    Read("Delete");
                    Console.WriteLine("Введите Id человека каторый вы хотели удалить его данныe!");
                    Console.Write("Id: ");
                    var ID = Convert.ToInt32(Console.ReadLine());
                    var person = Context.Customer.Find(ID);

                    if (person != null)
                    {
                        Console.Write("Вы действительно хотели удалить? Д(да)/Н(нет):");
                        var confirm = Console.ReadLine();
                        if (confirm.ToUpper() == "Д") Context.Customer.Remove(person);

                        if (Context.SaveChanges() > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Успешно удален!");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Не удален!");
                            Console.ResetColor();
                        }
                    }
                    else Console.WriteLine("В нашу таблицу человек по такой ID не существует!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.Write($"1.Create\n" +
                              $"2.Read\n" +
                              $"3.Update\n" +
                              $"4.Delete\n" +
                              $"Ваш выбор: ");
                switch (Console.ReadLine())
                {
                    case "1": Operation.Create(); break;
                    case "2": Operation.Read(); break;
                    case "3": Operation.Update(); break;
                    case "4": Operation.Delete(); break;
                    default: break;
                }
            }
        }
    }
}
