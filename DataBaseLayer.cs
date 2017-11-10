using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using NavisionServiceApp.readsoftModels2;
using ReadSoft.Services.Client.Entities;
using NavisionServiceApp.models;

namespace databaseLayer
{

    class DataBase
    {
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Newtonsoft.Json.Converters.XmlNodeConverter
        private static ReadSoftModel2 mdb = new ReadSoftModel2();
        private static object _lock = new object(); // lock object for threading.

        public DataBase()
        {

        }

        public bool Authenticate(string username, string password)
        {
            lock (_lock)
            {
                var  result = (from user in mdb.Db_AuthenticationCredentials
                              where user.UserName.Equals(username) && user.Password.Equals(password)
                              select user);


                return result.Count() > 0;
            }
        }

        public  IEnumerable<User2> GetUser(string username) {
            lock (_lock)
            {
                var result = (from user in mdb.Db_Users
                              where user.UserName.Equals(username)
                              select user);   
                return result;
            }
        }

        public bool ActivateCustomer(string customerId)
        {
            lock (_lock)
            {
                var result = (from cust in mdb.Db_Customer
                              where cust.Id.Equals(customerId)
                              select cust).First();
                result.ActivationStatus = OrganizationActivationStatus.Active;
                mdb.SaveChanges();
                return true;
            }
        }

        public Customer CreateCustomer(Customer customer)
        {
            lock (_lock)
            {
                mdb.Db_Customer.Add(customer);
                mdb.SaveChanges();
                var result = (from cust in mdb.Db_Customer
                              where cust.Name.Equals(customer.Name)
                              select cust).FirstOrDefault();


                return result;
            }
        }


        public   IQueryable<Customer> SelectAllCustomers()
        {
            lock (_lock)
            {
                var result = (from cust in mdb.Db_Customer
                              select cust);
                return result;
            }
        }

        public bool saveCookie(string cookievalue, string username)
        {
            try
            {
                Cookie cookie = new Cookie(cookievalue, username);
                mdb.Db_Cookie.Add(cookie);
                mdb.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                logger.Logger.Error(ex.Message);
                return false;
            }
        }
    }
}
