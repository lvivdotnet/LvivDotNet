using FluentMigrator;

namespace LvivDotNet.Persistence.Migrations
{
    /// <summary>
    /// Initial migration.
    /// </summary>
    [Migration(1, TransactionBehavior.Default)]
    public class InitialMigration : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Create.Table("event").WithColumn("Id").AsInt32().Identity().PrimaryKey()
                                 .WithColumn("Name").AsString()
                                 .WithColumn("StartDate").AsDateTime2()
                                 .WithColumn("EndDate").AsDateTime()
                                 .WithColumn("PostDate").AsDateTime2()
                                 .WithColumn("Address").AsString()
                                 .WithColumn("Title").AsString()
                                 .WithColumn("Description").AsString(int.MaxValue)
                                 .WithColumn("MaxAttendees").AsInt32();

            this.Create.Table("attendee").WithColumn("Id").AsInt32().Identity().PrimaryKey()
                                    .WithColumn("FirstName").AsString()
                                    .WithColumn("LastName").AsString()
                                    .WithColumn("Email").AsString()
                                    .WithColumn("Phone").AsString()
                                    .WithColumn("Male").AsCustom("bit")
                                    .WithColumn("Age").AsInt32();

            this.Create.Table("role").WithColumn("Id").AsInt32().Identity().PrimaryKey()
                                 .WithColumn("Name").AsString();

            this.Create.Table("user").WithColumn("Id").AsInt32().Identity().PrimaryKey()
                                .WithColumn("FirstName").AsString()
                                .WithColumn("LastName").AsString()
                                .WithColumn("Email").AsString().Unique()
                                .WithColumn("Phone").AsString().Nullable()
                                .WithColumn("Sex").AsCustom("bit").Nullable()
                                .WithColumn("Age").AsInt32().Nullable()
                                .WithColumn("Avatar").AsString().Nullable()
                                .WithColumn("Password").AsString()
                                .WithColumn("Salt").AsString()
                                .WithColumn("RoleId").AsInt32().ForeignKey("role", "Id");

            this.Create.Table("refresh_token").WithColumn("Id").AsInt32().Identity().PrimaryKey()
                                          .WithColumn("UserId").AsInt32().ForeignKey("user", "Id")
                                          .WithColumn("RefreshToken").AsString()
                                          .WithColumn("Expires").AsDateTime2();

            this.Create.Table("post").WithColumn("Id").AsInt32().Identity().PrimaryKey()
                                .WithColumn("Title").AsString()
                                .WithColumn("Body").AsString(int.MaxValue)
                                .WithColumn("AuthorId").AsInt32().ForeignKey("user", "Id")
                                .WithColumn("PostDate").AsDateTime2();

            this.Create.Table("ticket_template").WithColumn("Id").AsInt32().Identity().PrimaryKey()
                                           .WithColumn("Name").AsString()
                                           .WithColumn("EventId").AsInt32().ForeignKey("event", "Id")
                                           .WithColumn("Price").AsCurrency()
                                           .WithColumn("From").AsDateTime2()
                                           .WithColumn("To").AsDateTime2();

            this.Create.Table("ticket").WithColumn("Id").AsInt32().Identity().PrimaryKey()
                                  .WithColumn("TicketTemplateId").AsInt32().ForeignKey("ticket_template", "Id")
                                  .WithColumn("AttendeeId").AsInt32().ForeignKey("attendee", "Id").Nullable()
                                  .WithColumn("UserId").AsInt32().ForeignKey("User", "Id").Nullable()
                                  .WithColumn("CreatedDate").AsDateTime2();

            this.Create.Table("product").WithColumn("Id").AsInt32().Identity().PrimaryKey()
                                   .WithColumn("Name").AsString()
                                   .WithColumn("Description").AsString(int.MaxValue)
                                   .WithColumn("Count").AsInt32()
                                   .WithColumn("Price").AsCurrency();

            this.Create.Table("addition").WithColumn("Id").AsInt32().Identity().PrimaryKey()
                                    .WithColumn("Blob").AsBinary(int.MaxValue)
                                    .WithColumn("Title").AsString()
                                    .WithColumn("EventId").AsInt32().ForeignKey("event", "Id")
                                    .WithColumn("PostId").AsInt32().ForeignKey("post", "Id")
                                    .WithColumn("ProductId").AsInt32().ForeignKey("product", "Id");

            this.Create.Table("order").WithColumn("Id").AsInt32().Identity().PrimaryKey()
                                 .WithColumn("ProductId").AsInt32().ForeignKey("product", "Id")
                                 .WithColumn("UserId").AsInt32().ForeignKey("User", "Id");

            this.Insert.IntoTable("role").Row(new { Name = "User" });
            this.Insert.IntoTable("role").Row(new { Name = "Admin" });

            this.Insert.IntoTable("user").Row(
                    new
                    {
                        FirstName = "Andrii",
                        LastName = "Maslianko",
                        Age = 21,
                        Email = "caballiero777@gmail.com",
                        Sex = 1,
                        Password = "w+bPSy3KJ7Ru+urivvs52sa81+LZJTP8/Xo1+YxlEPg=",
                        Salt = "lnsIpp53Zy7XF1E22M5EXaEVu5Wv6wSLqxSfv2gkADE=",
                        RoleId = 2,
                    });
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Delete.Table("order");
            this.Delete.Table("addition");
            this.Delete.Table("product");
            this.Delete.Table("ticket");
            this.Delete.Table("ticket_template");
            this.Delete.Table("post");
            this.Delete.Table("refresh_token");
            this.Delete.Table("user");
            this.Delete.Table("attendee");
            this.Delete.Table("event");
        }
    }
}
