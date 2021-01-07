using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace dotNETproject1
{
    internal class OnlineStore
    {
        private const string PATH = @"C:\Dev\Movies.txt";

        private const string ORDERPATH = @"C:\Dev\Orders.txt";

        private const string ACCOUNTPATH = @"C:\Dev\Customers.txt";

        public void MainMenu()
        {
            Console.WriteLine("-------DOTNET-FLIX--------");
            Console.WriteLine("1. Add a movie to the database" +
                "\n2. Order a movie" +
                "\n3. Movies Overview and Modify" +
                "\n4. Make An Account" +
                "\n5. Exit.");

            int userInput = Convert.ToInt32(Console.ReadLine());

            switch (userInput)
            {
                case 1:
                    AddingMoviesMenu();
                    break;

                case 2:
                    OrderMenu();
                    //CheckOut method
                    break;

                case 3:

                    MoviesOverview();
                    ModifyFile();
                    break;

                case 4:
                    MakeAnAccount();
                    break;

                case 5:
                    Environment.Exit(1);
                    break;

                default:
                    Console.Clear();
                    ErrorMessage("Please give a valid answer", 1000);
                    MainMenu();
                    break;
            }
        }

        private void OrderMenu()
        {
            Console.Clear();
            Console.WriteLine("Our movies: ");
            ShowMovieCatalog();
            WriteOrderToFile();
        }

        private void ShowMovieCatalog()
        {
            CreateMovieCatalog();
            PrintMovieCatalog();
        }

        private List<Movie> CreateMovieCatalog()
        {
            List<string> myMovies = retriveMovieLog(PATH);

            List<Movie> movieCatalog = new List<Movie>();

            foreach (string movie in myMovies)
            {
                string[] movieArray = movie.Split(',');
                int id = Convert.ToInt32(movieArray[0]);
                string name = movieArray[1];
                string genre = movieArray[2];
                decimal price = decimal.Parse(movieArray[4]);
                int duration = int.Parse(movieArray[3]);
                int yearofProduction = int.Parse(movieArray[5]);
                Movie movie1 = new Movie(id, name, genre, price, duration, yearofProduction);
                movieCatalog.Add(movie1);
            }

            return movieCatalog;
        }

        private List<string> retriveMovieLog(string path)
        {
            using StreamReader reader = new StreamReader(path);
            string line = string.Empty;

            List<string> movies = new List<string>();

            while ((line = reader.ReadLine()) != null)
            {
                movies.Add(line);
            }
            return movies;
        }

        private void PrintMovieCatalog()
        {
            List<Movie> movies1 = CreateMovieCatalog();
            foreach (Movie movie in movies1)
            {
                Console.WriteLine($"{movie.Id}: {movie.Name} {movie.Genre} {movie.Duration} {movie.Price} {movie.YearOfProduction}");
            }
        }

        private void AddingMoviesMenu()
        {
            Console.Clear();
            Movie movie = CreateMovie();
            AddMovieToFile(movie);
            AddMoreMovies();
        }

        private void MoviesOverview()
        {
            Console.Clear();
            Console.WriteLine("------Our Movies------");
            ShowMovieCatalog();
        }

        private void AddMoreMovies()
        {
            Console.WriteLine("Press E for a new movie, or press B to go Back to main menu");
            string inputUser = Console.ReadLine();
            switch (inputUser.ToUpper())
            {
                case "E":
                    Console.Clear();
                    AddingMoviesMenu();
                    break;

                case "B":
                    Console.Clear();
                    MainMenu();
                    break;

                default:
                    ErrorMessage("Give a valid answer", 1000);
                    Console.Clear();
                    AddMoreMovies();
                    break;
            }
        }

        private void AddMovieToFile(Movie movie)
        {
            string line = ($" { movie.Id },{ movie.Name },{ movie.Genre },{ movie.Duration },{ movie.Price },{ movie.YearOfProduction }");
            FileManager fileManager = new FileManager();
            fileManager.WriteDataToFile(line, PATH);
        }

        private Movie CreateMovie()
        {
            Console.WriteLine("Please give the details of the movie:");
            Console.Write("Name: ");
            string inputName = Console.ReadLine();
            Console.Write("Genre: ");
            string inputGenre = Console.ReadLine();
            Console.Write("Price: ");
            string inputPricetemp = Console.ReadLine();
            decimal inputPrice = IsItADecimal(inputPricetemp);
            Console.Write("Duration: ");
            string inputDurationtemp = Console.ReadLine();
            int inputDuration = IsItAnInteger(inputDurationtemp);
            Console.Write("Year of production: ");
            int inputYear = int.Parse(Console.ReadLine());
            Movie myMovie = new Movie(inputName, inputGenre, inputPrice, inputDuration, inputYear);
            return myMovie;
        }

        private decimal IsItADecimal(string inputPricetemp)
        {
            decimal inputPrice;
            if (!decimal.TryParse(inputPricetemp, out inputPrice))
            {
                Console.WriteLine("Give the correct input.");
                inputPricetemp = Console.ReadLine();
                inputPrice = IsItADecimal(inputPricetemp);
            }
            else
            {
                inputPrice = decimal.Parse(inputPricetemp);
            }

            return inputPrice;
        }

        private int IsItAnInteger(string inputDurationtemp)
        {
            int inputDuration;
            if (!int.TryParse(inputDurationtemp, out inputDuration))
            {
                Console.WriteLine("Give the correct input.");
                inputDurationtemp = Console.ReadLine();
                inputDuration = IsItAnInteger(inputDurationtemp);
            }
            else
            {
                inputDuration = int.Parse(inputDurationtemp);
            }

            return inputDuration;
        }

        //Modify content -- MORE TO DO HERE
        private void ModifyFile()
        {
            Console.WriteLine($"Which movie do you want to modify?");
            string userinput = Console.ReadLine();
            List<Movie> movies = CreateMovieCatalog();
            foreach (Movie movie in movies)
            {
                if (int.Parse(userinput) == movie.Id)
                {
                    Console.WriteLine($"You have selected the movieID: {movie.Id} to modify ");
                    Console.WriteLine($"A. Delete the movie.");
                    Console.WriteLine($"B. Change the price.");
                    Console.WriteLine($"C. Go back to the Main Menu.");
                    string userinput1 = Console.ReadLine();

                    switch (userinput1)
                    {
                        case "A":
                            List<Movie> updatedMovies = RemoveMovie(movie.Id);
                            PrintUpdatedMovieCatalog(updatedMovies);
                            break;

                        case "B":
                            //ReplaceItemInAList(movies, movie);
                            break;

                        case "C":
                            MainMenu();
                            break;

                        default:
                            ErrorMessage("Give a correct input", 1000);
                            ModifyFile();

                            break;
                    }
                }
            }
        }

        private List<Movie> RemoveMovie(int id)
        {
            //remove the selected movie from the list
            List<Movie> movies = CreateMovieCatalog();
            movies.RemoveAll(x => x.Id == id);
            return movies;
        }

        private void PrintUpdatedMovieCatalog(List<Movie> movies)
        {
            foreach (Movie movie in movies)
            {
                Console.WriteLine($"{movie.Name}");
            }
        }

        private void CreateOrder(Order order)
        {
            FileManager fileManager = new FileManager();
            string line = ($"{order.ID}, {order.OrderTime}, {order.OrderedItemID}, {order.TotalPrice} ");
            fileManager.WriteDataToFile(line, ORDERPATH);
        }

        private void WriteOrderToFile()
        {
            Order order = PickAMovie();
            CreateOrder(order);
        }

        private void MakeAnAccount()
        {
            Customer customer = AccountMenu();
            WriteAccountToFile(customer);
            Console.Clear();
            Console.WriteLine("You have made an account.");
            Console.WriteLine(
                $"Name: {customer.CustomerName}, " +
                $"Year Of Birth: {customer.YearOfBirth}, " +
                $"Gender: {customer.Gender}, " +
                $"Billing Address: {customer.BillingAddress}, " +
                $"E-Mail: {customer.EMail}");
            Console.WriteLine("Press M to Main Menu");
            string inputUser = Console.ReadLine();
            if (inputUser == "M" || inputUser == "m")
            {
                Console.Clear();
                MainMenu();
            }
        }

        private void WriteAccountToFile(Customer customer)
        {
            FileManager fileManager = new FileManager();
            string line = ($"{customer.CustomerId}, {customer.CustomerName}, {customer.YearOfBirth}, {customer.Gender}, {customer.BillingAddress}, {customer.EMail} ");
            fileManager.WriteDataToFile(line, ACCOUNTPATH);
        }

        private Customer AccountMenu()
        {
            Console.Write("Fill in your name: ");
            string customerName = Console.ReadLine();
            Console.Write("Fill in your Year of Birth: ");
            int yearOfBirth = int.Parse(Console.ReadLine());
            Console.Write("Fill in your gender: (male/female)");
            Gender gender = (Gender)Enum.Parse(typeof(Gender), Console.ReadLine());
            Console.Write("Fill in your billing address: ");
            string billingAddress = Console.ReadLine();
            Console.Write("Fill in your e-mail: ");
            string e_mail = Console.ReadLine();
            Customer customer = new Customer(customerName, yearOfBirth, gender, billingAddress, e_mail);
            return customer;
        }

        private Order PickAMovie()
        {
            Console.WriteLine("Pick up a movie? (Press E to exit.)");
            string userinput = Console.ReadLine();
            Order order = new Order();
            List<Movie> movies = CreateMovieCatalog();
            foreach (Movie movie in movies)
            {
                if (userinput == "E")
                {
                    Console.Clear();
                    MainMenu();
                }
                else if (int.Parse(userinput) == movie.Id)
                {
                    Console.WriteLine($"This is Your Order: ");
                    Console.WriteLine("----------------------");
                    Console.WriteLine($"Name: {movie.Name}");
                    Console.WriteLine($"Genre : {movie.Genre}");
                    Console.WriteLine($"Duration: {movie.Duration}");
                    Console.WriteLine($"Year Of Production: {movie.YearOfProduction}");
                    Console.WriteLine($"Total price to pay: {movie.Price}");
                    order = new Order(movie.Id, movie.Price);
                }
                else
                {
                    ErrorMessage("Give a correct input", 1000);
                    Console.Clear();
                    OrderMenu();
                }
            }

            return order;
        }

        private void ErrorMessage(string errorInfo, int sleepTimer)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{errorInfo}");
            Thread.Sleep(sleepTimer);
            Console.ResetColor();
        }
    }
}
