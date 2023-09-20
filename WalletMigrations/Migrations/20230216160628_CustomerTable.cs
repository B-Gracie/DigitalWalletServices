using FluentMigrator;
using Microsoft.EntityFrameworkCore;

namespace Wallet.Migrations.Migrations;

[Migration(20230216160628)]
public class CustomersTable : Migration
{
    public override void Up()
    {
        Create.Schema("Users");

            Create.Table("Customers").InSchema("Users")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("FirstName").AsString(50).NotNullable()
                .WithColumn("LastName").AsString(50).NotNullable()
                .WithColumn("Email").AsString(100).NotNullable()
                .WithColumn("Username").AsString(50).NotNullable()
                .WithColumn("DateCreated").AsDateTime().NotNullable()
                .WithColumn("Password").AsString(50).NotNullable()
                .WithColumn("AccountNum").AsString(50).NotNullable()
                .WithColumn("Balance").AsDecimal().NotNullable();

            Create.Table("Transactions").InSchema("Users")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("CustomerId").AsInt32()
                .WithColumn("AccountNum").AsString(50).NotNullable()
                .WithColumn("DepositAmount").AsDecimal().NotNullable()
                .WithColumn("Balance").AsDecimal().NotNullable()
                .WithColumn("WithdrawalAmount").AsDecimal().NotNullable()
                .WithColumn("TxnTime").AsDateTime().NotNullable();

            Create.ForeignKey("FK_Transactions_CustomerId_Customers_Id")
                .FromTable("Transactions").InSchema("Users").ForeignColumn("CustomerId")
                .ToTable("Customers").InSchema("Users").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Transactions_CustomerId_Customers_Id").OnTable("Transactions").InSchema("Users");
            Delete.Table("Transactions").InSchema("Users");
            Delete.Table("Customers").InSchema("Users");
            Delete.Schema("Users");
        }
    }



