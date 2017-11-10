namespace NavisionServiceApp.readsoftModels2
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using MySql.Data.Entity;
    using MySql.Data;
    using ReadSoft.Services.Client.Entities;
    using System.ComponentModel.DataAnnotations;
    using NavisionServiceApp.models;

    // Code-Based Configuration and Dependency resolution
    // [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ReadSoftModel2 : DbContext
    {
        // Your context has been configured to use a 'Model1' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'NavisionServiceApp.models2.Model1' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Model1' 
        // connection string in the application configuration file.
        public ReadSoftModel2()
            : base("name=ReadsoftModel2")
        {
            // DbConfiguration.SetConfiguration(new MySql.Data.Entity.MySqlEFConfiguration());
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        // Just for test.
        private void test()
        {
            Metadata t;
            DocumentReference t2;
            AuthenticationResult t3;
            Account a;
            CustomerUploadConfiguration p;
            CreateUserConfiguration cu;
            UserConfiguration uc;
            UserConfigurationDocumentType ucdt;
            AuthenticationCredentials ac;

            
        }
        /* 
        public class DbAccount : Account
        {
            [Key]
            public new int Id { get; set; }
        }
        */

        public virtual DbSet<models.Document> Db_Document { get; set; }

        public virtual DbSet<Cookie> Db_Cookie { get; set; }

        public virtual DbSet<Account> Db_Account { get; set; }

        public class DbAccountEmailInputConfiguration : AccountEmailInputConfiguration
        {
            [Key]
            public int Id { get; set; }

        }
        public virtual DbSet<DbAccountEmailInputConfiguration> Db_AccountEmailInputConfiguration { get; set; }

        public class DbAssignPrivilege : AssignPrivilege
        {
            [Key]
            public int Id { get; set; }

        }
        public virtual DbSet<DbAssignPrivilege> Db_AssignPrivilege { get; set; }

        public class DbAuthenticationCredentials : AuthenticationCredentials
        {
            [Key]
            public int Id { get; set; }
        }
        public virtual DbSet<DbAuthenticationCredentials> Db_AuthenticationCredentials { get; set; }


        /* public class DbAuthenticationResult : AuthenticationResult
        {
            public int Id { get; set; }
            public string guid { get; set; }

        }
        

        public virtual DbSet<DbAuthenticationResult> Db_AuthenticationResult { get; set; }
        */
        /* 
        public class DbUser2 : User2
        {
            [Key]
            public new int Id { get; set; }
        }
        */ 

        public virtual DbSet<User2> Db_Users { get; set; }

        /* 
        public class DbCustomer : Customer
        {
            [Key]
            public new int Id { get; set; }
        }

            */ 
        public virtual DbSet<Customer> Db_Customer { get; set; }
        
        /* 
        public class DbBuyer : Buyer
        {
            [Key]
            public new int Id { get; set; }
        }
        */ 

        public virtual DbSet<Buyer> Db_Buyer { get; set; }
        

        /* 
        public class DbDocument : Document
        {
            public int Id { get; set; }

        }
        

        public virtual DbSet<Document> Db_Document { get; set; }

        

        public class DbDocumentReference : DocumentReference
        {
            public int Id { get; set; }

        }
        

        public virtual DbSet<DbDocumentReference> Db_DocumentReference { get; set; }

        
        public class DbMetadata : Metadata
        {
            public int Id { get; set; }
        }

        public virtual DbSet<DbMetadata> Db_Metadata { get; set; }

        */

    }
}