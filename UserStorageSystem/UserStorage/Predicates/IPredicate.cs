using UserStorage.UserEntity;

namespace UserStorage.Predicates
{
    public interface IPredicate
    {
        bool IsMatching(User user);
    }
}
