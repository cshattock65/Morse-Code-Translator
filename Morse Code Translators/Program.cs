using System.Text;
using JollyWrapper;

public abstract class MorseCodeStandard
{
    // Properties to store the morse code standard and the character mappings
    public string Standard { get; set; } = "";
    public Dictionary<string, string> Characters { get; set; } = new Dictionary<string, string>();

    // Method to load Morse code mappings from a text file
    public void GetCharacters()
    {
        try
        {
            Characters.Clear(); // Clear existing mappings
            string[] data = File.ReadAllLines($"{Standard}.txt"); // Read all lines from the file

            foreach (var line in data)
            {
                // Only process lines that are not blank or comments
                if (!string.IsNullOrWhiteSpace(line) && line[0] != '#')
                {
                    Characters.Add(line[0].ToString(), line[2..]); // Add character and its Morse code to dictionary
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"The file {Standard}.txt' was not found.");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
        }
    }

    // Converts plain text to Morse code using the loaded character mappings
    public string ConvertText(string text, int userID)
    {
        string convertedText = "";
        string[] splitLine = text.Split(' '); // Split the input text into words

        for (int i = 0; i < splitLine.Length; i++)
        {
            foreach (char c in splitLine[i]) // Iterate over each character in the word
            {
                if (Characters.ContainsKey(c.ToString().ToUpper())) // Check if the character has a mapping
                {
                    convertedText += Characters[c.ToString().ToUpper()] + "  "; // Append Morse code and space
                }
            }
            convertedText += "  |  "; // Add word separator in Morse code
        }

        return convertedText;
    }

    // Converts Morse code to plain text using the reverse of the loaded mappings
    public string ConvertMorse(string morse)
    {
        string convertedText = "";
        string[] splitLine = morse.Split("  "); // Split the Morse code into Morse letters

        foreach (string s in splitLine)
        {
            if (s == "|")
            {
                convertedText += " "; // Convert word separator to space
            }
            else
            {
                foreach (KeyValuePair<string, string> pair in Characters)
                {
                    if (pair.Value == s) // Find the character corresponding to the Morse code
                    {
                        convertedText += pair.Key; // Append the character to the output string
                    }
                }
            }
        }
        return convertedText;
    }

    // Handles user input for text processing and conversion to Morse code
    public void ProcessAndConvertText(string text, int userID)
    {
        Base16Encoder base16Encoder = new Base16Encoder();
        Console.WriteLine("Would you like this compressed and encrypted?");
        string? compressed = Console.ReadLine();
        byte[] dataToProcess;

        if (compressed?.ToLower() == "yes")
        {
            byte[] rawInputBytes = Encoding.UTF8.GetBytes(text); // Convert text to bytes
            dataToProcess = CompressDecompress.Compress(rawInputBytes); // Compress the byte array

            Console.WriteLine("What would you like the password to be?");
            string? key = Console.ReadLine();

            byte[] encryptedData = EncryptDecrypt.Encrypt(key, dataToProcess); // Encrypt the data
            string encryptedBase16 = base16Encoder.ByteArrayToHexStrings(encryptedData); // Convert encrypted data to Base16

            string morseCode = ConvertBase16ToMorse(encryptedBase16); // Convert Base16 to Morse code
            Console.WriteLine("Encrypted Data in Morse Code: " + morseCode);

            // Optionally save the Morse code to the database
            Console.WriteLine("Would you like to save this Morse code to the database?");
            if (Console.ReadLine()?.ToLower() == "yes")
            {
                DatabaseConnection.InsertDataIntoDb(userID, morseCode);
            }
        }
        else
        {
            string convertedText = ConvertText(text, userID); // Convert text directly to Morse code
            Console.WriteLine(convertedText);
            dataToProcess = Encoding.UTF8.GetBytes(text); // Convert text to byte array

            // Optionally save the Morse code to the database
            Console.WriteLine("Would you like to save this Morse code to the database?");
            if (Console.ReadLine()?.ToLower() == "yes")
            {
                DatabaseConnection.InsertDataIntoDb(userID, convertedText);
            }
        }
    }

    // Helper function to convert Base16 encoded string to Morse code
    private string ConvertBase16ToMorse(string base16Encoded)
    {
        string morseBuilder = "";
        foreach (char character in base16Encoded.ToUpper()) // Iterate over each character in the Base16 string
        {
            if (Characters.ContainsKey(character.ToString())) // Check if there is a Morse code for the character
            {
                morseBuilder += (Characters[character.ToString()] + "|"); // Append Morse code and delimiter
            }
            else
            {
                morseBuilder += ("?|");  // Use '?' for unknown characters
            }
        }

        return morseBuilder.ToString().Trim();
    }

    // Helper function to convert Morse code back to Base16 encoded string
    private string MorseToBase16(string morseEncoded)
    {
        string base16Builder = "";

        foreach (string character in morseEncoded.Split("|")) // Split the Morse code at each delimiter
        {
            string realCharacter = Characters.FirstOrDefault(p => p.Value == character, new KeyValuePair<string, string>("", "?")).Key; // Find the character corresponding to each Morse code
            base16Builder += realCharacter; // Append the character to the Base16 string
        }
        return base16Builder;
    }

    // Handles decryption and decompression of Morse-encoded data
    public void DecryptAndProcessMorseData(string base16EncodedData)
    {
        Base16Encoder base16Encoder = new Base16Encoder();
        GetCharacters(); // Load Morse code mappings
        Console.WriteLine("Is this data encrypted? (yes/no)");
        string? decrypted = Console.ReadLine();
        if (decrypted?.ToLower() == "yes")
        {
            Console.WriteLine("What is the password?");
            string? key = Console.ReadLine();

            try
            {
                // Decode the Base16 encoded encrypted data to binary
                byte[] encryptedData = base16Encoder.HexStringsToByteArray(MorseToBase16(base16EncodedData));

                // Decrypt the data
                byte[] decryptedData = EncryptDecrypt.Decrypt(key, encryptedData);

                // Check if data was possibly compressed and needs decompression
                byte[] outputData;
                try
                {
                    outputData = CompressDecompress.Decompress(decryptedData); // Attempt to decompress the data
                }
                catch
                {
                    // If decompression fails, assume it was not compressed
                    outputData = decryptedData;
                }

                // Convert the binary data back to string (assuming UTF-8 encoding)
                string outputString = Encoding.UTF8.GetString(outputData);
                Console.WriteLine("Decrypted output: " + outputString);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred during decryption or decompression: " + ex.Message);
            }
        }
        else
        {
            string convertedMorse = ConvertMorse(base16EncodedData); // Convert Morse code to text
            Console.WriteLine($"Data:{convertedMorse}");
        }
    }
}

public class International : MorseCodeStandard
{
    // Constructor that sets the Morse code standard to international
    public International(string standard)
    {
        Standard = standard;
    }
}

public class American : MorseCodeStandard
{
    // Constructor that sets the Morse code standard to American
    public American(string standard)
    {
        Standard = standard;
    }
}

class Program
{
    public static void Main()
    {
        // Start the program by displaying the main menu
        Menu.MainMenu();
    }

    // View and manage data related to a specific user
    public static void ViewData(int userID)
    {
        // Initialize database connection to handle user data
        DatabaseConnection.InitialiseDatabase();

        // Create objects for international and American Morse standards
        International international = new International("international");
        American american = new American("american");

        try
        {
            // Prepare query parameters to fetch user data from the database
            QueryParms userParams = new QueryParms()
            {
                { "@userID", userID.ToString() }
            };
            // Execute the query to retrieve data
            QueryData userData = Database.ExecuteQuery("SELECT * FROM Data WHERE userID = @userID", userParams).Result;

            // Display retrieved data to the user
            Console.WriteLine("Your Current saved data:");
            foreach (var data in userData)
            {
                Console.WriteLine($"{data["dataID"]}: {data["content"]}");
            }

            // Prompt user to choose a piece of data to decrypt or process
            Console.WriteLine("You may select a piece of your data that is encrypted to be decrypted by typing in its data ID:");
            string input = Console.ReadLine();
            int choice;
            if (int.TryParse(input, out choice))
            {
                QueryParms choiceParams = new QueryParms()
                {
                    { "@dataID", choice.ToString() }
                };
                Console.WriteLine("What standard are you using?");
                string standard = Console.ReadLine();
                QueryData userData2 = Database.ExecuteQuery("SELECT * FROM `Data` WHERE `dataID` = @dataID", choiceParams).Result;

                foreach (var data in userData2)
                {
                    if (standard == "international")
                    {
                        var content = data["content"];
                        // Process the content using international Morse code standards
                        international.DecryptAndProcessMorseData(content);
                    }
                    else if (standard == "american")
                    {
                        var content = data["content"];
                        // Process the content using American Morse code standards
                        american.DecryptAndProcessMorseData(content);
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid data ID.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    // Display the main menu and handle user interactions
    public static void ShowMenu(int userID)
    {
        bool validMenuChoice = false;

        while (!validMenuChoice)
        {
            Console.WriteLine("--------Main Menu--------");
            Console.WriteLine("1) Text into morse code");
            Console.WriteLine("2) Morse code into text");
            Console.WriteLine("3) Training class");
            Console.WriteLine("4) View user data");
            Console.WriteLine("5) Manage users");
            Console.WriteLine("-------------------------");

            string choice = Console.ReadLine();

            // Instantiate objects to handle different functionalities
            Conversion conversion = new Conversion();
            Users user1 = new Users();

            switch (choice)
            {
                case "1":
                    // Convert text to Morse code
                    conversion.TextToMorse(userID);
                    validMenuChoice = true;
                    break;
                case "2":
                    // Convert Morse code to text
                    conversion.MorseToText();
                    validMenuChoice = true;
                    break;
                case "3":
                    // Access training functionalities
                    Training.UserTraining();
                    validMenuChoice = true;
                    break;
                case "4":
                    // View user data
                    ViewData(userID);
                    validMenuChoice = true;
                    break;
                case "5":
                    // Access user management functionalities
                    user1.UserManagementMenu();
                    validMenuChoice = true;
                    break;
                default:
                    Console.WriteLine("Please select a valid option by entering a number");
                    break;
            }
        }
    }
}

