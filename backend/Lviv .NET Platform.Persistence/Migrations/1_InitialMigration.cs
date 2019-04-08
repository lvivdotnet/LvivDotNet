using FluentMigrator;

namespace Lviv_.NET_Platform.Persistence.Migrations
{
    [Migration(1)]
    public class InitialMigration : Migration
    {
        public override void Up()
        {
            Create.Table("event").WithColumn("Id").AsInt32().Identity().PrimaryKey()
                                 .WithColumn("Name").AsString()
                                 .WithColumn("StartDate").AsDateTime()
                                 .WithColumn("EndDate").AsDateTime()
                                 .WithColumn("PostDate").AsDateTime()
                                 .WithColumn("Address").AsString()
                                 .WithColumn("Title").AsString()
                                 .WithColumn("Description").AsString()
                                 .WithColumn("MaxAttendees").AsInt32();

            Create.Table("attendee").WithColumn("Id").AsInt32().Identity().PrimaryKey()
                                    .WithColumn("FirstName").AsString()
                                    .WithColumn("LastName").AsString()
                                    .WithColumn("Email").AsString()
                                    .WithColumn("Phone").AsString()
                                    .WithColumn("Male").AsCustom("bit")
                                    .WithColumn("Age").AsInt32();

            Create.Table("user").WithColumn("Id").AsInt32().Identity().PrimaryKey()
                                .WithColumn("FirstName").AsString()
                                .WithColumn("LastName").AsString()
                                .WithColumn("Email").AsString().Unique()
                                .WithColumn("Phone").AsString()
                                .WithColumn("Male").AsCustom("bit")
                                .WithColumn("Age").AsInt32()
                                .WithColumn("Avatar").AsString()
                                .WithColumn("Password").AsString()
                                .WithColumn("Salt").AsString();

            Create.Table("refresh_token").WithColumn("Id").AsInt32().Identity().PrimaryKey()
                                          .WithColumn("UserId").AsInt32().ForeignKey("user", "Id")
                                          .WithColumn("RefreshToken").AsString()
                                          .WithColumn("Expires").AsDateTime();

            Create.Table("post").WithColumn("Id").AsInt32().Identity().PrimaryKey()
                                .WithColumn("Title").AsString()
                                .WithColumn("Body").AsString()
                                .WithColumn("AuthorId").AsInt32().ForeignKey("user", "Id")
                                .WithColumn("PostDate").AsDateTime();

            Create.Table("ticket_template").WithColumn("Id").AsInt32().Identity().PrimaryKey()
                                           .WithColumn("Name").AsString()
                                           .WithColumn("EventId").AsInt32().ForeignKey("event", "Id")
                                           .WithColumn("Price").AsCurrency()
                                           .WithColumn("From").AsDateTime()
                                           .WithColumn("To").AsDateTime();

            Create.Table("ticket").WithColumn("Id").AsInt32().Identity().PrimaryKey()
                                  .WithColumn("TicketTemplateId").AsInt32().ForeignKey("ticket_template", "Id")
                                  .WithColumn("AttendeeId").AsInt32().ForeignKey("attendee", "Id")
                                  .WithColumn("UserId").AsInt32().ForeignKey("User", "Id")
                                  .WithColumn("CreatedDate").AsDateTime();

            Create.Table("product").WithColumn("Id").AsInt32().Identity().PrimaryKey()
                                   .WithColumn("Name").AsString()
                                   .WithColumn("Description").AsString()
                                   .WithColumn("Count").AsInt32()
                                   .WithColumn("Price").AsCurrency();

            Create.Table("addition").WithColumn("Id").AsInt32().Identity().PrimaryKey()
                                    .WithColumn("Blob").AsBinary()
                                    .WithColumn("Title").AsString()
                                    .WithColumn("EventId").AsInt32().ForeignKey("event", "Id")
                                    .WithColumn("PostId").AsInt32().ForeignKey("post", "Id")
                                    .WithColumn("ProductId").AsInt32().ForeignKey("product", "Id");

            Create.Table("order").WithColumn("Id").AsInt32().Identity().PrimaryKey()
                                 .WithColumn("ProductId").AsInt32().ForeignKey("product", "Id")
                                 .WithColumn("UserId").AsInt32().ForeignKey("User", "Id");
        }

        public override void Down()
        {
            Delete.Table("order");
            Delete.Table("addition");
            Delete.Table("product");
            Delete.Table("ticket");
            Delete.Table("ticket_template");
            Delete.Table("post");
            Delete.Table("user");
            Delete.Table("attendee");
            Delete.Table("event");
        }
    }
}
