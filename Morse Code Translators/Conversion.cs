public class Conversion
{
    // Converts text to Morse code based on user-selected standard.
    public void TextToMorse(int userID)
    {
        Console.WriteLine("Please enter the text that you want to convert:");
        try
        {
            string text = Console.ReadLine() ?? "";

            MorseCodeStandard standard = GetMorseCodeStandard();
            if (standard != null)
            {
                standard.GetCharacters(); // Load character mappings.
                string morseCode = standard.ConvertText(text, userID);
                Console.WriteLine("Converted Morse Code: " + morseCode);
                standard.ProcessAndConvertText(text, userID); // Further processing.
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions related to conversion or processing errors.
            Console.WriteLine("An error occurred during conversion: " + ex.Message);
        }
    }

    // Converts Morse code to text based on user-selected standard.
    public void MorseToText()
    {
        Console.WriteLine("Please enter the Morse code that you want to convert:");
        try
        {
            string morseCode = Console.ReadLine() ?? "";

            MorseCodeStandard standard = GetMorseCodeStandard();
            if (standard != null)
            {
                standard.GetCharacters(); // Load character mappings.
                string text = standard.ConvertMorse(morseCode);
                Console.WriteLine("Converted Text: " + text);
                Console.WriteLine("Please enter the data you want to decrypt");
                standard.DecryptAndProcessMorseData(morseCode); // Decrypt and possibly decompress.
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions related to conversion or decryption errors.
            Console.WriteLine("An error occurred during conversion: " + ex.Message);
        }
    }

    // Helper method to determine which Morse code standard to use.
    private MorseCodeStandard GetMorseCodeStandard()
    {
        while (true)
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Please select the Morse code standard you are using:");
            Console.WriteLine("1) International");
            Console.WriteLine("2) American");
            Console.WriteLine("--------------------------------------------------");

            try
            {
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        return new International("international");
                    case "2":
                        return new American("american");
                    default:
                        Console.WriteLine("Invalid selection. Please select a valid option by entering a number (1 or 2).\n");
                        break;
                }
            }
            catch (Exception ex)
            {
                // Handle potential errors such as null input.
                Console.WriteLine("An error occurred while reading your input: " + ex.Message);
                continue;
            }
        }
    }
}
