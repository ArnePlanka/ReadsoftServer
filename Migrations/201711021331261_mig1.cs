namespace NavisionServiceApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        ExternalId = c.String(),
                        IsLoading = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbAccountEmailInputConfigurations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MailDomainName = c.String(),
                        IsLoading = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbAssignPrivileges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TargetName = c.String(),
                        IsLoading = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbAuthenticationCredentials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                        AuthenticationType = c.Int(nullable: false),
                        AuthenticationOptions = c.Int(nullable: false),
                        AuthenticationChallangeResponse_ResponseType = c.Int(nullable: false),
                        AuthenticationChallangeResponse_Value = c.String(),
                        AuthenticationChallangeResponse_IsLoading = c.Boolean(nullable: false),
                        ClientDeviceDescription = c.String(),
                        IsLoading = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Buyers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        ExternalId = c.String(),
                        VatNumber = c.String(),
                        AddressCountry = c.String(),
                        AddressStreetAddress = c.String(),
                        AddressPostcode = c.String(),
                        AddressCity = c.String(),
                        PhoneNumber = c.String(),
                        Fax = c.String(),
                        AlternativeName1 = c.String(),
                        AlternativeName2 = c.String(),
                        AlternativeName3 = c.String(),
                        OrganizationNumber = c.String(),
                        AddressState = c.String(),
                        IsLoading = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        ExternalId = c.String(),
                        ActivationStatus = c.Int(nullable: false),
                        ClassificationValue = c.String(),
                        IsLoading = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User2",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        OrganizationId = c.String(),
                        UserName = c.String(),
                        FullName = c.String(),
                        EmailAddress = c.String(),
                        PhoneNumber = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        SubstituteUserId = c.String(),
                        Notes = c.String(),
                        UseIdentityProvider = c.Boolean(nullable: false),
                        IsLoading = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.User2");
            DropTable("dbo.Customers");
            DropTable("dbo.Buyers");
            DropTable("dbo.DbAuthenticationCredentials");
            DropTable("dbo.DbAssignPrivileges");
            DropTable("dbo.DbAccountEmailInputConfigurations");
            DropTable("dbo.Accounts");
        }
    }
}
