using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UserStorage
{
    public interface IUserStorage : IXmlSerializable
    {
        int Add(User user, IUserValidation validationRules = null);

        int SearchForUser(params Func<User, bool>[] predicates);

        IEnumerable<int> SearchForUsers(params Func<User, bool>[] predicates);

        void Delete(User user);

        void Delete(int id);
    }
}
