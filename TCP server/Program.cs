using Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets; 

namespace TCP_server
{
    class Program
    {
        private static List<Book> bookList = new List<Book>
        {
            new Book {ISBN13 = "1111111111111", Author = "Person1", Title = "Title1", PageNumber = 11},
            new Book {ISBN13 = "2222222222222", Author = "Person2", Title = "Title2", PageNumber = 22},
            new Book {ISBN13 = "3333333333333", Author = "Person3", Title = "Title3", PageNumber = 33},
        };
        static void Main(string[] args)
        {
                
                TcpListener listener = new TcpListener(IPAddress.Loopback, 4646);
                listener.Start();
                Console.WriteLine("Server ready");

                while (true) // to continue receiving clients
                {
                    TcpClient socket = listener.AcceptTcpClient();//Until client comes in, the code doesn't continue
                    Console.WriteLine("Incoming client");

                    NetworkStream ns = socket.GetStream();
                    //For more presise use, our reader and writer
                    StreamReader reader = new StreamReader(ns);
                    StreamWriter writer = new StreamWriter(ns);

                    string message = "";

                    while (message != "End")
                    {
                        message = reader.ReadLine();
                        var jsonAll = JsonConvert.SerializeObject(bookList);
                        switch (message)
                        {
                            case "GetAll":
                                writer.WriteLine(jsonAll);
                                break;
                            case "Get":
                                string isbn13 = reader.ReadLine();
                                Book book = bookList.Find(book => book.ISBN13.Contains(isbn13));
                                var jsonOne = JsonConvert.SerializeObject(book);
                                writer.WriteLine(jsonOne);
                                break;
                            case "Save":
                                var jsonBookToSave = reader.ReadLine();
                                var addbook = JsonConvert.DeserializeObject<Book>(jsonBookToSave);
                                 bookList.Add(addbook);
                                break;
                        }
                        writer.Flush();
                    }
                    socket.Close();

                }

            }
        }
    }


