public static class Training
{
    public static void UserTraining()
    {
        International international = new International("international");
        international.GetCharacters();

        Dictionary<string, string> Characters = international.Characters;

        // Get a list of Morse code characters from the dictionary
        List<string> morseCodes = Characters.Values.ToList();

        // Randomly select a Morse code character
        Random random = new Random();
        int randomIndex = random.Next(morseCodes.Count);
        string randomMorse = morseCodes[randomIndex];

        Console.WriteLine("Guess the letter corresponding to this Morse code:");
        Console.WriteLine(randomMorse);

        int attempts = 3;

        while (attempts > 0)
        {
            Console.WriteLine($"Attempts left: {attempts}");
            Console.WriteLine("Enter your guess:");
            string guess = Console.ReadLine()?.ToUpper();

            if (guess == null)
            {
                Console.WriteLine("Invalid input. Please try again.");
                continue;
            }

            if (Characters.ContainsValue(randomMorse))
            {
                // Find the key (letter) corresponding to the Morse code
                string correctLetter = Characters.FirstOrDefault(x => x.Value == randomMorse).Key;

                if (guess == correctLetter)
                {
                    Console.WriteLine("Congratulations! You guessed correctly.");
                    return; // Exit the function
                }
            }

            Console.WriteLine("Incorrect guess. Try again.");
            attempts--;
        }

        Console.WriteLine("You've exhausted all your attempts. The correct answer was:");
        Console.WriteLine(Characters.FirstOrDefault(x => x.Value == randomMorse).Key);
    }
}
