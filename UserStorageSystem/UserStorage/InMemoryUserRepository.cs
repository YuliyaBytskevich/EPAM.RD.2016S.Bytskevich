using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage
{
    public class InMemoryUserStorage: IUserStorage
    {
        internal struct UserEntity
        {
            internal int Id { get; }
            internal User User { get; }
            public UserEntity(int id, User user)
            {
                Id = id;
                User = user;
            }
        }
        private List<UserEntity> userEntities;

        public InMemoryUserStorage()
        {
            userEntities = new List<UserEntity>();
        }

        public int Add(User user)
        {
            int newId = 1; // TODO: generate new id!
            userEntities.Add(new UserEntity(newId, user));
            return newId;
        }

        public int SearchForUser(params Func<User, bool>[] predicates)
        {
            if (!userEntities.Any())
                return 0;
            if (predicates.Count() == 0)
                return userEntities.First().Id;
            Func<User, bool> commonPredicate = predicates[0];
            if (predicates.Count() > 1)
                foreach (var predicate in predicates)
                    commonPredicate += predicate;
            var foundEntities = userEntities.Where(x => commonPredicate(x.User));
            return foundEntities.Any() ? foundEntities.First().Id : 0;           
       }

        public void Delete(User user)
        {
            userEntities.RemoveAll(x => x.User.Equals(user));
        }

        public void Delete(int id)
        {
            userEntities.RemoveAll(x => x.Id == id);
        }

    }
}
