namespace NavisionServiceApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DbAccount2",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        ExternalId = c.String(),
                        IsLoading = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbAccountEmailInputConfiguration2",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MailDomainName = c.String(),
                        IsLoading = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbAssignPrivilege2",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TargetName = c.String(),
                        IsLoading = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbAuthenticationCredentials2",
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
                "dbo.DbAuthenticationResult2",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        Challange_ChallangeType = c.Int(nullable: false),
                        Challange_IsLoading = c.Boolean(nullable: false),
                        ClientDeviceSecret = c.String(),
                        IsLoading = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbUsers2",
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
            DropTable("dbo.DbUsers2");
            DropTable("dbo.DbAuthenticationResult2");
            DropTable("dbo.DbAuthenticationCredentials2");
            DropTable("dbo.DbAssignPrivilege2");
            DropTable("dbo.DbAccountEmailInputConfiguration2");
            DropTable("dbo.DbAccount2");
        }
    }
}
