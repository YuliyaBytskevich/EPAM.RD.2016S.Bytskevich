using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage
{
    public interface IUserStorage
    {
        int Add(User user, IUserValidation validationRules = null);

        int SearchForUser(params Func<User, bool>[] predicates);

        void Delete(User user);

        void Delete(int id);
    }
}
