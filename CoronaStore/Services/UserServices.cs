using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using CoronaStore.Models;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace CoronaStore.Services
{
    public class UserServices
    {

        public LoginResult AttemptLogin(LoginDetails input)
        {
            if (input == null)
                return LoginResult.Failed;

            using (var context = new CoronaPageContext())
            {
                var userDetails = context.Users.Where(p => p.Username == input.Username).FirstOrDefault();

                if (userDetails == null || input.Password==null || input.Username==null)
                    return LoginResult.Failed;

                var encryptedPassword = convertToMd5(input.Password);
                var token = convertToMd5(DateTime.Now.ToString());

                if (!encryptedPassword.Equals(userDetails.Password))
                    return LoginResult.Failed;

                return new LoginResult()
                {
                    LoginSucceeded = true,
                    Token = token
                };
            }
        }



        public bool Register(RegisterDetails UserInput)
        {
            if (UserInput is null || CheckInputValid(UserInput))
                return false;
            using (var Context = new CoronaPageContext())
            {
                User newUser = new User()
                {
                    Address = UserInput.Address,
                    FirstName = UserInput.FirstName,
                    IsAdmin = 0,
                    LastName = UserInput.LastName,
                    Password = convertToMd5(UserInput.Password),
                    Phone = UserInput.Phone,
                    Username = UserInput.Username
                };

                Context.Users.Add(newUser);
                Context.SaveChanges();
            }
            return true;
        }

        private bool CheckInputValid(RegisterDetails userInput)
        {
            return (userInput.Address == null ||
                    userInput.FirstName == null ||
                    userInput.LastName == null ||
                    userInput.Password == null ||
                    userInput.Phone == null || userInput.Username == null);
        }

        public IList<ProductsUser> ProductsByUser()
        {
            using (var context = new CoronaPageContext())
            {

                var prodByUser = context.Sales.Join(context.Products, s => s.ProductID, p => p.ProductID, (sale, product) => new
                    {
                        Product = product,
                        Sale = sale
                    })
                    .GroupBy(x => x.Sale.UserID)
                    .Select(x => new ProductsUser()
                    {
                        UserName = GetUserName(x.Key),
                        UserExpense = (int)x.Sum(y => y.Product.Price)
                    })
                    .OrderByDescending(p => p.UserExpense).ToList();

                prodByUser.RemoveRange(6, prodByUser.Count - 6);
                return prodByUser;
            }
        }

        private string GetUserName(int key)
        {
            using (var context = new CoronaPageContext())
            {
                var query = context.Users.Where(u => u.UserID == key).FirstOrDefault();
                return query.FirstName;
            }
        }

        private string convertToMd5(string target)
        {
            var cipher = new MD5CryptoServiceProvider();
            var passwordBytes = Encoding.UTF8.GetBytes(target);

            passwordBytes = cipher.ComputeHash(passwordBytes);

            var sb = new StringBuilder();

            for (int i = 0; i < passwordBytes.Length; i++)
            {
                sb.Append(passwordBytes[i].ToString("x2").ToLower());
            }

            return sb.ToString();
        }

        public User GetUser(string userName)
        {
            using (var context = new CoronaPageContext())
            {
                return context.Users.FirstOrDefault(u => u.Username == userName);
            }
        }

        public bool DoesUserExists(string username)
        {
            using (var context = new CoronaPageContext())
            {
                return context.Users.Count(u => u.Username == username) > 0;
            }
        }
    }
}