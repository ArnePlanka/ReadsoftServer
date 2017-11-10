using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using databaseLayer;
using System.ComponentModel.DataAnnotations;
using ReadSoft.Services.Client.Entities;

namespace dataLogicsLayer
{
    class DataLogics
    {
        private static DataBase dataBase = new DataBase();

        public string CreateKey()
        {
            string s = Guid.NewGuid().ToString("N");
            s = s.Substring(0, 14);
            long  x = Math.Abs(Convert.ToInt64(s, 16));
            return x.ToString();
        }

        public DataLogics()
        {

        }

        public bool Authenticate(string username, string password)
        {
            if (username.Count() == 0)
                return false; // Throw exception/ServiceError??
            return dataBase.Authenticate(username, password);
        }

        public IEnumerable<User2> GetUser(string username)
        {
            if (username.Count() == 0)
                return null;
            return dataBase.GetUser(username);
        }

        public bool ActivateCustomer(string customerId)
        {
            if (!customerId.Equals(""))
                return false;
            return dataBase.ActivateCustomer(customerId);
        }

        public Customer CreateCustomer(Customer cust)
        {
            if(cust.Id == null || cust.Id.Equals(""))
            {
                cust.Id = CreateKey();
            }
            if (cust.Name.Length < 1)
                return null;
            return dataBase.CreateCustomer(cust);
        }

        public IEnumerable<Customer> SelectAllCustomers()
        {
            return dataBase.SelectAllCustomers();
        }


        public bool SaveCookie(string cookie, string user)
        {
            if (cookie.Equals("") || user.Equals(""))
            {
                return false;
            }
            return dataBase.saveCookie(cookie, user);
        }
    }
}
