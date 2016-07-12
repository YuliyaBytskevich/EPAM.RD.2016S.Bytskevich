using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage
{
    public interface IUserStorage
    {
        //IEnumerable<int> GetAllEntities();
        //IEnumerable<User> GetUsersByPredicates(params Func<User, bool>[] predicates);
        //void Create(User newUser);
        //void Update(int id);
        //void Delete(int id);
        //void SaveChanges();

        int Add(User user);

        int SearchForUser(params Func<User, bool>[] predicates);

        void Delete(User user);

        void Delete(int id);

    }
}
