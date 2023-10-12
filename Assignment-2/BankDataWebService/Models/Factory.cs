using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BankDataWebService.Models
{
    public class Factory
    {
        private static uint numProfiles = 0;
        private static uint numAccounts = 0;
        private static uint numTransactions = 0;
        private static Random rand = new Random();

        public static Profile MakeProfile()
        {
            numProfiles++;

            Profile profile = new Profile();
            profile.UserID = numProfiles;
            profile.Name = MakeName();
            profile.Email = profile.Name + "@realdomain.com";
            profile.Address = MakeAddress();
            profile.Phone = MakePhone();
            profile.Pwd = MakePwd();

            return profile;
        }

        public static Account MakeAccount(uint id)
        {
            numAccounts++;

            Account account = new Account();
            account.Balance = rand.Next(-99, 10000);
            account.AcctNo = numAccounts;
            account.UserID = id;

            return account;
        }

        public static Transaction MakeTransaction(uint id)
        {
            numTransactions++;

            Transaction transaction = new Transaction();
            transaction.TransactionNum = numTransactions;
            transaction.Amount = rand.Next(-100, 101);
            transaction.AcctNo = id;

            return transaction;
        }


        private static string MakeName()
        {
            string name = "";

            name += (char)rand.Next(65, 91);                // intial is capital

            for (int ii = 0; ii < rand.Next(2, 10); ii++)   // subsequent letters 
            {
                name += (char)rand.Next(97, 123);
            }

            return name;
        }
        private static string MakeAddress()
        {
            string addy = "";
            // street number
            addy += rand.Next(1, 201);
            // street name
            addy += " " + (char)rand.Next(65, 91);
            for(int ii = 0; ii < rand.Next(3, 15); ii++)
            {
                addy += (char)rand.Next(97, 123);
            }
            addy += " Street, ";
            // state
            addy += (char)rand.Next(65, 91);
            addy += (char)rand.Next(65, 91) + ", ";
            //post code
            for(int ii = 0; ii < 4; ii++)
            {
                addy += rand.Next(0, 10);
            }

            return addy;
        }
        private static uint MakePhone()
        {
            string number = "";

            // 9 random digits
            for(int ii = 0; ii < 9; ii++)
            {
                number += rand.Next(0, 10);
            }
            // return as uint
            return uint.Parse(number);
        }
        private static string MakePwd()
        {
            string pwd = "";
            // generate lower case letters, min 8, max 24
            for(int ii = 0; ii < rand.Next(8, 25); ii ++)
            {
                pwd += (char)rand.Next(97, 123);
            }

            return pwd;
        }
    }
}