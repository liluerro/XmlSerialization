using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Profession { get; set; }
    public Address UniAddress { get; set; }

    public Person() { }
}

public class Address
{
    public string City { get; set; }
    public string Street { get; set; }
    public Address() { }
}

class Program
{

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Person person = new Person
        {
            Name = "Єлизавета",
            Age = 18,
            Profession = "Програміст",
            UniAddress = new Address { City = "Дніпро", Street = "Володимира Вернадського 2/4" }
        };

        string filePath = "person.xml";

        try
        {
            // сереалізація
            SerializeToXml(person, filePath);
            Console.WriteLine($"Об'єкт серіалізовано у файл: {Path.GetFullPath(filePath)}\n");

            // вміст хмл
            Console.WriteLine("--- Вміст XML ---");
            Console.WriteLine(File.ReadAllText(filePath));
            Console.WriteLine("---------------------------\n");

            // десералізація 
            Person deserialized = Deserialize<Person>(filePath);
            Console.WriteLine("--- Об'єкт після десеріалізації ---");
            Console.WriteLine($"Ім'я: {deserialized.Name}");
            Console.WriteLine($"Вік: {deserialized.Age}");
            Console.WriteLine($"Професія: {deserialized.Profession}");
            Console.WriteLine($"Адреса Університету: {deserialized.UniAddress?.City}, {deserialized.UniAddress?.Street}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Сталася помилка: " + ex.Message);
        }

        Console.WriteLine("\nНатисніть будь-яку клавішу для виходу...");
        Console.ReadKey();
    }

    static void SerializeToXml<T>(T obj, string filePath)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));

        XmlWriterSettings settings = new XmlWriterSettings
        {
            Indent = true, 
            Encoding = new UTF8Encoding(false) 
        };

        using (var writer = XmlWriter.Create(filePath, settings))
        {
            serializer.Serialize(writer, obj);
        }
    }

    static T Deserialize<T>(string filePath)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        using (var reader = XmlReader.Create(filePath))
        {
            return (T)serializer.Deserialize(reader);
        }
    }

    static void SerializeObjectAtRuntime(object obj, string filePath)
    {
        XmlSerializer serializer = new XmlSerializer(obj.GetType());
        using (var writer = XmlWriter.Create(filePath, new XmlWriterSettings { Indent = true }))
        {
            serializer.Serialize(writer, obj);
        }
    }
}
