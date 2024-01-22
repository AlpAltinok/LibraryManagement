using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public int CopyCount { get; set; }
    public int BorrowedCopyCount { get; set; }
}

class Library
{
    private List<Book> books;

    public Library()
    {
         books = new List<Book>();

        // Boş kalmaması için  üç kitap ekledim başlangıç olarak
        Book book1 = new Book
        {
            Title = "Olasılıksız",
            Author = "Adam Fawer",
            ISBN = "123456789",
            CopyCount = 5,
            BorrowedCopyCount = 0
        };
        books.Add(book1);

        Book book2 = new Book
        {
            Title = "Sefiller",
            Author = "Victor Hugo",
            ISBN = "987654321",
            CopyCount = 8,
            BorrowedCopyCount = 0
        };
        books.Add(book2);

        Book book3 = new Book
        {
            Title = "Kan, Ter ve Pikseller",
            Author = "George Dyson",
            ISBN = "456789123",
            CopyCount = 3,
            BorrowedCopyCount = 0
        };
        books.Add(book3);

        // Veri kalıcılığı için dosyadan yükleme
        LoadDataFromFile();
    }

    public void AddBook(Book book)
    {
        books.Add(book);
        Console.WriteLine("Book added: " + book.Title);
        // verileri dosyaya kaydediyoruz
        SaveDataToFile();
    }

    public void DisplayAllBooks()
    {
        Console.WriteLine("All Books:");
        foreach (var book in books)
        {
            Console.WriteLine($"Title: {book.Title}, Author: {book.Author}, ISBN: {book.ISBN}, Copy Count: {book.CopyCount}, Borrowed Copy Count: {book.BorrowedCopyCount}");
        }
    }

    public void SearchBook(string keyword)
    {
        var results = books.Where(b => b.Title.Contains(keyword) || b.Author.Contains(keyword)).ToList();
        if (results.Count > 0)
        {
            Console.WriteLine("Search Results:");
            foreach (var book in results)
            {
                Console.WriteLine($"Title: {book.Title}, Author: {book.Author}, ISBN: {book.ISBN}, Copy Count: {book.CopyCount}, Borrowed Copy Count: {book.BorrowedCopyCount}");
            }
        }
        else
        {
            Console.WriteLine("No matching books found.");
        }
    }

    public void BorrowBook(string title)
    {
        var book = books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (book != null && book.CopyCount > book.BorrowedCopyCount)
        {
            book.BorrowedCopyCount++;
            Console.WriteLine($"{book.Title} has been borrowed.");  
            SaveDataToFile();
        }
        else
        {
            Console.WriteLine("The book could not be borrowed. All copies are borrowed, or the book was not found.");
        }
    }

    public void ReturnBook(string title)
    {
        var book = books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (book != null && book.BorrowedCopyCount > 0)
        {
            book.BorrowedCopyCount--;
            Console.WriteLine($"{book.Title} has been returned.");
            SaveDataToFile();
        }
        else
        {
            Console.WriteLine("The book could not be returned. The book was not found, or no copies are currently borrowed.");
        }
    }

    public void DisplayOverdueBooks()
    {
        var overdueBooks = books.Where(b => b.BorrowedCopyCount > 0).ToList();
        if (overdueBooks.Count > 0)
        {
            Console.WriteLine("Borrowed Books:");
            foreach (var book in overdueBooks)
            {
                Console.WriteLine($"Title: {book.Title}, Author: {book.Author}, ISBN: {book.ISBN}, Copy Count: {book.CopyCount}, Borrowed Copy Count: {book.BorrowedCopyCount}");
            }
        }
        else
        {
            Console.WriteLine("No borrowed books currently.");
        }
    }

    private void SaveDataToFile()
    {
        using (StreamWriter writer = new StreamWriter("library_data.txt"))
        {
            foreach (var book in books)
            {
                writer.WriteLine($"{book.Title},{book.Author},{book.ISBN},{book.CopyCount},{book.BorrowedCopyCount}");
            }
        }
    }

    private void LoadDataFromFile()
    {
        if (File.Exists("library_data.txt"))
        {
            using (StreamReader reader = new StreamReader("library_data.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var data = line.Split(',');
                    if (data.Length == 5)
                    {
                        Book book = new Book
                        {
                            Title = data[0],
                            Author = data[1],
                            ISBN = data[2],
                            CopyCount = int.Parse(data[3]),
                            BorrowedCopyCount = int.Parse(data[4])
                        };
                        books.Add(book);
                    }
                }
            }
        }
    }
}

class Program
{
    static void Main()
    {
        Library library = new Library();

        while (true)
        {
            Console.WriteLine("\nLibrary Management System");
            Console.WriteLine("1. Add New Book");
            Console.WriteLine("2. Display All Books");
            Console.WriteLine("3. Search for a Book");
            Console.WriteLine("4. Borrow a Book");
            Console.WriteLine("5. Return a Book");
            Console.WriteLine("6. Display Overdue Books");
            Console.WriteLine("0. Exit");

            Console.Write("Please enter an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Book newBook = new Book();
                    Console.Write("Title: ");
                    newBook.Title = Console.ReadLine();
                    Console.Write("Author: ");
                    newBook.Author = Console.ReadLine();
                    Console.Write("ISBN: ");
                    newBook.ISBN = Console.ReadLine();
                    Console.Write("Copy Count: ");
                    int copyCount;
                    if (int.TryParse(Console.ReadLine(), out copyCount))
                    {
                        newBook.CopyCount = copyCount;
                        newBook.BorrowedCopyCount = 0; 
                        library.AddBook(newBook);
                    }
                    else
                    {
                        Console.WriteLine("Invalid copy count input.");
                    }
                    break;

                case "2":
                    library.DisplayAllBooks();
                    break;

                case "3":
                    Console.Write("Search keyword: ");
                    string keyword = Console.ReadLine();
                    library.SearchBook(keyword);
                    break;

                case "4":
                    Console.Write("Title of the book to borrow: ");
                    string bookToBorrow = Console.ReadLine();
                    library.BorrowBook(bookToBorrow);
                    break;

                case "5":
                    Console.Write("Title of the book to return: ");
                    string bookToReturn = Console.ReadLine();
                    library.ReturnBook(bookToReturn);
                    break;

                case "6":
                    library.DisplayOverdueBooks();
                    break;

                case "0":
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("You entered an invalid option. Please try again.");
                    break;
            }
        }
    }
}
