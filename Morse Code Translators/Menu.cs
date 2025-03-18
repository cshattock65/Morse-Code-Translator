public static class Menu
{
    public static void MainMenu()
    {
        Users user1 = new Users();
        DatabaseConnection.InitialiseDatabase();

        bool accountOption = true;

        while (accountOption)
        {
            Console.Write("--------------------------------------\n");      
            Console.Write("Welcome to the Morse Code Translator!|\n");
            Console.Write("Please login or create an account:   |\n");
            Console.Write("1) Login                             |\n");
            Console.Write("2) Create an account                 |\n");
            Console.Write("3) Exit                              |\n");
            Console.Write("--------------------------------------\n");
            string option = Console.ReadLine();


            switch (option)
            {
                case "1":
                    int userId = user1.CheckLogin();
                    if (userId != -1)
                    {
                        Program.ShowMenu(userId);
                    }
                    else
                    {
                        Console.WriteLine("Invalid username or password. Please try again.");
                    }
                    break;

                case "2":
                    if (user1.CreateAccount())
                    {
                        Console.WriteLine("Account created successfully! Please log in.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to create account. Please try again.");
                    }
                    break;

                case "3":
                    Console.WriteLine("Exiting...");
                    accountOption = false;
                    break;

                default:
                    Console.WriteLine("Invalid option. Please choose 1, 2, or 3!");
                    break;
            }
        }
    }

}

