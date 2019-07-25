using System;
using FluentMigrator;
using LvivDotNet.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LvivDotNet.Persistence.Migrations
{
    /// <summary>
    /// Initial migration.
    /// </summary>
    [Migration(1, TransactionBehavior.Default)]
    public class InitialMigration : Migration
    {
        private readonly ILogger<InitialMigration> logger;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitialMigration"/> class.
        /// </summary>
        /// <param name="logger"> <see cref="ILogger{InitialMigration}"/>. </param>
        /// <param name="configuration"> <see cref="IConfiguration"/>. </param>
        public InitialMigration(ILogger<InitialMigration> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

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
                                    .WithColumn("Male").AsInt32()
                                    .WithColumn("Age").AsInt32();

            this.Create.Table("role").WithColumn("Id").AsInt32().Identity().PrimaryKey()
                                 .WithColumn("Name").AsString();

            this.Create.Table("user").WithColumn("Id").AsInt32().Identity().PrimaryKey()
                                .WithColumn("FirstName").AsString()
                                .WithColumn("LastName").AsString()
                                .WithColumn("Email").AsString().Unique()
                                .WithColumn("Phone").AsString().Nullable()
                                .WithColumn("Sex").AsInt32().Nullable()
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
                                  .WithColumn("UserId").AsInt32().ForeignKey("user", "Id").Nullable()
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
                                 .WithColumn("UserId").AsInt32().ForeignKey("user", "Id");

            this.Insert.IntoTable("role").Row(new { Name = "User" });
            this.Insert.IntoTable("role").Row(new { Name = "Admin" });

            var administartorEmail = this.configuration["AdministratorEmail"];
            var administartorPassword = this.configuration["AdministratorPassword"];

            if (!string.IsNullOrEmpty(administartorEmail) && !string.IsNullOrEmpty(administartorPassword))
            {
                var salt = SecurityHelpers.GetRandomBytes(32);

                this.Insert.IntoTable("user").Row(
                    new
                    {
                        FirstName = "Administartor",
                        LastName = "Administartor",
                        Age = 21,
                        Email = administartorEmail,
                        Sex = 1,
                        Password = SecurityHelpers.GetPasswordHash(administartorPassword, salt),
                        Salt = Convert.ToBase64String(salt),
                        RoleId = 2,
                    });
                this.logger.LogInformation("Default administrator user added to database");
            }
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
