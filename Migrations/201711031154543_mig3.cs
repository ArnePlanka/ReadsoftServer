namespace NavisionServiceApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cookies",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        idField = c.String(),
                        versionField = c.String(),
                        typeField = c.String(),
                        originalFilenameField = c.String(),
                        filenameField = c.String(),
                        partiesField = c.String(),
                        headerFieldsField = c.String(),
                        tablesField = c.String(),
                        processMessagesField = c.String(),
                        systemFieldsField = c.String(),
                        erpCorrelationDataField = c.String(),
                        baseTypeField = c.String(),
                        permalinkField = c.String(),
                        historyField = c.String(),
                        trackIdField = c.String(),
                        documentTypeField = c.String(),
                        validationInfoCollectionField = c.String(),
                        originField = c.String(),
                        codingLinesField = c.String(),
                        parkDocumentField = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Documents");
            DropTable("dbo.Cookies");
        }
    }
}
