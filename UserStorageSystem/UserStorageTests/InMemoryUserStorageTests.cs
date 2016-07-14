using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorage;
using UserStorage.UserStorageValidationExceptions;

namespace UserStorageTests
{
    [TestClass]
    public class InMemoryUserStorageTests
    {
        [TestMethod]
        public void Add_UserToEmptyStorageWithPrimeIdsWithNoValidation_ReturnUserIdEqualsTwo()
        {
            InMemoryUserStorage storage = new InMemoryUserStorage(new PrimeNumbersGenerator());
            User newUser = new User("Firstname", "Lastname", new DateTime(1990, 1, 1), "1111111A111PB1", Gender.Male, null);
            int newUserId = storage.Add(newUser);
            Assert.IsNotNull(newUserId);
            Assert.AreEqual(2, newUserId);
        }

        [TestMethod]
        public void Add_ValidUserToEmptyStorageWithPrimeIds_ReturnUserIdEqualsTwo()
        {
            InMemoryUserStorage storage = new InMemoryUserStorage(new PrimeNumbersGenerator());
            User newUser = new User("Firstname", "Lastname", new DateTime(1990, 1, 1), "1111111A111PB1", Gender.Male, new VisaRecord("Neverland", new DateTime(2014, 1, 1), new DateTime(2015, 1, 1)));
            int newUserId = storage.Add(newUser, new BelarusianUsersValidation());
            Assert.IsNotNull(newUserId);
            Assert.AreEqual(2, newUserId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserInfoException))]
        public void Add_UserWithInvalidFirstname_ThrowException()
        {
            InMemoryUserStorage storage = new InMemoryUserStorage(new PrimeNumbersGenerator());
            User newUser = new User("111111111", "Lastname", new DateTime(1990, 1, 1), "1111111A111PB1", Gender.Male, null);
            int newUserId = storage.Add(newUser, new BelarusianUsersValidation());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserInfoException))]
        public void Add_UserWithInvalidVisaRecord_ThrowException()
        {
            InMemoryUserStorage storage = new InMemoryUserStorage(new PrimeNumbersGenerator());
            User newUser = new User("Firstname", "Lastname", new DateTime(1990, 1, 1), "1111111A111PB1", Gender.Male, new VisaRecord("Neverland", new DateTime(2016, 1, 1), new DateTime(2015, 1, 1)));
            int newUserId = storage.Add(newUser, new BelarusianUsersValidation());
        }

        [TestMethod]
        public void Add_TrirdUserToEmptyStorageWithPrimeIdsWithNoValidation_ReturnUserIdEqualsFive()
        {
            InMemoryUserStorage storage = new InMemoryUserStorage(new PrimeNumbersGenerator());
            User newUser = new User("Firstname", "Lastname", new DateTime(1990, 1, 1), "1111111A111PB1", Gender.Male, null);
            int newUserId = storage.Add(newUser);
            newUserId = storage.Add(newUser);
            newUserId = storage.Add(newUser);
            Assert.IsNotNull(newUserId);
            Assert.AreEqual(5, newUserId);
        }

        [TestMethod]
        public void SearchForUser_ByLastnameAndUserExists_ReturnsCorrectUserId()
        {
            InMemoryUserStorage storage = new InMemoryUserStorage(new PrimeNumbersGenerator());
            User firstUser = new User("Somename", "First", new DateTime(1990, 1, 1), "1111111A111PB1", Gender.Male, null);
            storage.Add(firstUser);
            User secondUser = new User("Somename", "Second", new DateTime(1990, 1, 1), "1111111A111PB1", Gender.Male, null);
            int correctId = storage.Add(secondUser);
            int foundId = storage.SearchForUser(x => x.LastName == "Second");
            Assert.IsNotNull(foundId);
            Assert.AreEqual(correctId, foundId);
        }

        [TestMethod]
        public void SearchForUser_ByLastnameAndUserDoesntExist_ReturnsZero()
        {
            InMemoryUserStorage storage = new InMemoryUserStorage(new PrimeNumbersGenerator());
            User firstUser = new User("Somename", "First", new DateTime(1990, 1, 1), "1111111A111PB1", Gender.Male, null);
            storage.Add(firstUser);
            User secondUser = new User("Somename", "Second", new DateTime(1990, 1, 1), "1111111A111PB1", Gender.Male, null);
            storage.Add(secondUser);
            int foundId = storage.SearchForUser(x => x.LastName == "Trird");
            Assert.AreEqual(0, foundId);
        }

        [TestMethod]
        public void SearchForUser_ByFirstnameAndLastnameAndUserExists_ReturnsCorrectUserId()
        {
            InMemoryUserStorage storage = new InMemoryUserStorage(new PrimeNumbersGenerator());
            User firstUser = new User("Somename", "First", new DateTime(1990, 1, 1), "1111111A111PB1", Gender.Male, null);
            storage.Add(firstUser);
            User secondUser = new User("Somename", "Second", new DateTime(1990, 1, 1), "1111111A111PB1", Gender.Male, null);
            int correctId = storage.Add(secondUser);
            int foundId = storage.SearchForUser(x => x.FirstName == "Somename", x => x.LastName == "Second");
            Assert.IsNotNull(foundId);
            Assert.AreEqual(correctId, foundId);
        }

        [TestMethod]
        public void SearchForUser_ByFirstnameAndLastnameAndDateOfBirthAndUserExists_ReturnsCorrectUserId()
        {
            InMemoryUserStorage storage = new InMemoryUserStorage(new PrimeNumbersGenerator());
            User firstUser = new User("Somename", "First", new DateTime(1990, 1, 1), "1111111A111PB1", Gender.Male, null);
            storage.Add(firstUser);
            User secondUser = new User("Somename", "Second", new DateTime(1990, 1, 2), "1111111A111PB1", Gender.Male, null);
            int correctId = storage.Add(secondUser);
            int foundId = storage.SearchForUser(x => x.FirstName == "Somename", x => x.LastName == "Second", x => x.DateOfBirth == new DateTime(1990, 1, 2));
            Assert.IsNotNull(foundId);
            Assert.AreEqual(correctId, foundId);
        }

        [TestMethod]
        public void Delete_ExistingUserFromStorageWithTwoElements_ReturnOneElementLeftInStorage()
        {
            InMemoryUserStorage storage = new InMemoryUserStorage(new PrimeNumbersGenerator());
            User firstUser = new User("Somename", "First", new DateTime(1990, 1, 1), "1111111A111PB1", Gender.Male, null);
            storage.Add(firstUser);
            User secondUser = new User("Somename", "Second", new DateTime(1990, 1, 2), "1111111A111PB1", Gender.Male, null);
            storage.Add(secondUser);
            Assert.AreEqual(2, storage.GetUsersCount());
            storage.Delete(firstUser);
            Assert.AreEqual(1, storage.GetUsersCount());
        }

    }
}
