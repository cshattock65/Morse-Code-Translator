using JollyWrapper;

public class Users
{
    // Checks if the login credentials are valid and returns the user ID if they are.
    public int CheckLogin()
    {
        Console.WriteLine("--------LOGIN--------");
        Console.WriteLine("Enter your username: ");
        string username = Console.ReadLine();

        Console.WriteLine("Enter your password: ");
        string password = Console.ReadLine();

        try
        {
            // Query the database to check if the provided username and password exist
            QueryData users = Database.ExecuteQuery($"SELECT `userID` FROM `Users` WHERE `username` = '{username}' AND `password` = '{password}'").Result;

            foreach (var user in users)
            {
                // Return the user's ID if found
                return Convert.ToInt32(user["userID"]);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking login: {ex.Message}");
        }
        // Return -1 if user not found or an error occurred
        return -1;
    }

    // Creates a new user account with the provided username and password.
    public bool CreateAccount()
    {
        Console.WriteLine("Enter a new username: ");
        string newUsername = Console.ReadLine();

        Console.WriteLine("Enter a new password: ");
        string newPassword = Console.ReadLine();

        try
        {
            // Check if the username already exists in the database
            QueryData existingUsers = Database.ExecuteQuery($"SELECT * FROM `Users` WHERE `username` = '{newUsername}'").Result;
            if (existingUsers.Any())
            {
                return false;
            }

            // Insert the new user into the database
            int rowsAffected = Database.ExecuteNonQuery($"INSERT INTO `Users` (`username`, `password`) VALUES ('{newUsername}', '{newPassword}')").Result;

            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to create account: {ex.Message}");
            return false;
        }
    }

    // Displays all users in the database.
    public void ShowUsers()
    {
        try
        {
            // Query the database to retrieve information about all users
            QueryData usersData = Database.ExecuteQuery("SELECT `username`, `userID` FROM `Users`").Result;

            if (usersData.Any())
            {
                Console.WriteLine("List of Users:");
                foreach (var userData in usersData)
                {
                    string username = userData["username"].ToString();
                    int userID = Convert.ToInt32(userData["userID"]);
                    Console.WriteLine($"User ID: {userID}, Username: {username}");
                }
            }
            else
            {
                Console.WriteLine("No users found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error displaying users: {ex.Message}");
        }
    }

    // Deletes a user account based on the username.
    public bool DeleteAccount(string username)
    {
        Console.WriteLine($"Are you sure you want to delete the account for user '{username}'? (yes/no)");
        string confirmation = Console.ReadLine();

        if (confirmation.ToLower() == "yes")
        {
            try
            {
                // Delete the user from the database
                int rowsAffected = Database.ExecuteNonQuery($"DELETE FROM `Users` WHERE `username` = '{username}'").Result;

                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to delete account. The specified user may not exist.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting account: {ex.Message}");
                return false;
            }
        }
        else
        {
            Console.WriteLine("Account deletion cancelled.");
            return false;
        }
    }

    // Updates user account information.
    public bool EditAccount(string username)
    {
        Console.WriteLine($"Enter a new username for '{username}': ");
        string newUsername = Console.ReadLine();

        Console.WriteLine($"Enter a new password for '{username}': ");
        string newPassword = Console.ReadLine();

        try
        {
            // Update the user's information in the database
            int rowsAffected = Database.ExecuteNonQuery($"UPDATE `Users` SET `username` = '{newUsername}', `password` = '{newPassword}' WHERE `username` = '{username}'").Result;

            if (rowsAffected > 0)
            {
                Console.WriteLine("Account updated successfully.");
                return true;
            }
            else
            {
                Console.WriteLine("Failed to update account. The specified user may not exist.");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating account: {ex.Message}");
            return false;
        }
    }

    // Provides a menu for user management operations.
    public void UserManagementMenu()
    {
        bool exitMenu = false;

        while (!exitMenu)
        {
            Console.WriteLine("--------User Management Menu--------");
            Console.WriteLine("1) Create New Account");
            Console.WriteLine("2) Delete Account");
            Console.WriteLine("3) Edit Account");
            Console.WriteLine("4) View All Users");
            Console.WriteLine("5) Back to Main Menu");
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Enter your choice:");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    bool created = CreateAccount();
                    if (created)
                    {
                        Console.WriteLine("Account created successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to create account.");
                    }
                    break;
                case "2":
                    ShowUsers();
                    Console.WriteLine("Enter the username of the account to delete:");
                    string usernameToDelete = Console.ReadLine();
                    bool deleted = DeleteAccount(usernameToDelete);
                    if (deleted)
                    {
                        Console.WriteLine("Account deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to delete account.");
                    }
                    break;
                case "3":
                    Console.WriteLine("Enter the username of the account to edit:");
                    string usernameToEdit = Console.ReadLine();
                    bool edited = EditAccount(usernameToEdit);
                    if (edited)
                    {
                        Console.WriteLine("Account edited successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to edit account.");
                    }
                    break;
                case "4":
                    ShowUsers();
                    break;
                case "5":
                    exitMenu = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                    break;
            }
        }
    }
}
