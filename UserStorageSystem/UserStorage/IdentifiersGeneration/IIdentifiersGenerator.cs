namespace UserStorage.IdentifiersGeneration
{
    public interface IIdentifiersGenerator
    {
        int GenerateNewNumber();

        void ResetGenerator();
    }
}
