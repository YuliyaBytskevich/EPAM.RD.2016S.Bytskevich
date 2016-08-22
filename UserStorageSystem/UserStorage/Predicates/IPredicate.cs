namespace UserStorage.Predicates
{
    using UserEntity;

    public interface IPredicate
    {
        bool IsMatching(User user);
    }
}
