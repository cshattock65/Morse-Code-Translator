namespace ConversionUnitTests
{
    [TestClass]
    public class MorseCodeConverterTests
    {
        // International
        [TestMethod]
        public void ConvertTextInternational_ConvertsTextToMorseCode()
        {

            International international = new International("international");
            string inputText = "HELLO WORLD";
            string expectedOutput = "····  ·  ·-··  ·-··  ---    |  ·--  ---  ·-·  ·-··  -··    |  ";

            international.GetCharacters();
            string testing = international.ConvertText(inputText, 1);

            // Assert
            Assert.AreEqual(expectedOutput, testing);
            
        }
        [TestMethod]
        public void ConvertMorseInternational_ConvertsTextToMorseCode()
        {
            International international = new International("international");
            string inputMorse = "····  ·  ·-··  ·-··  ---    |  ·--  ---  ·-·  ·-··  -··    |";
            string expectedOutput = "HELLO WORLD ";

            international.GetCharacters();
            string testing = international.ConvertMorse(inputMorse);

            // Assert

            Assert.AreEqual(expectedOutput, testing);

        }

        // American
        [TestMethod]
        public void ConvertTextAmerican_ConvertsTextToMorseCode()
        {
            American american = new American("american");
            string inputText = "HELLO WORLD";
            string expectedOutput = "····  ·  --  --  · ·    |  ·--  · ·  · ··  --  -··    |  ";

            american.GetCharacters();
            string testing = american.ConvertText(inputText, 1);
             
            // Assert

            Assert.AreEqual(expectedOutput, testing);
        }

        [TestMethod]
        public void ConvertMorseAmerican_ConvertsTextToMorseCode()
        {

            American american = new American("american");
            string inputText = "····  ·  --  --  · ·    |  ·--  · ·  · ··  --  -··    |  ";
            string expectedOutput = "HELLO WORLD ";

            american.GetCharacters();
            string testing = american.ConvertMorse(inputText);

            // Assert
            Assert.AreEqual(expectedOutput, testing);

        }
    }
}