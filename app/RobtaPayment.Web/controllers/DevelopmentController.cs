namespace RobtaPayment.Web.controllers
{
    using System;
    using System.Security.Cryptography;
    using Castle.ActiveRecord;
    using Castle.MonoRail.ActiveRecordSupport;
    using RobtaPayment.Model.Enums;
    using RobtaPayment.Model.Helpers;
    using Model.Entities;

    public class DevelopmentController : ARSmartDispatcherController
    {
        public void CreateSchema()
        {
            try
            {
                ActiveRecordStarter.CreateSchema();
                RenderText("Nieuw database schema is aangemaakt.");
            }
            catch (Exception ex)
            {
                RenderText("Er is een fout opgetreden bij het creëren van het database schema.");
                RenderText(ex.Message);
            }

        }

        public void UpdateSchema()
        {
            try
            {
                ActiveRecordStarter.UpdateSchema();
                //InsertData();
                RenderText("Database schema is bijgewerkt.");
            }
            catch (Exception ex)
            {
                RenderText("Er is een fout opgetreden bij het bijwerken van het database schema.");
                RenderText(ex.Message);
            }
        }

        public void InsertData()
        {
            User user = new User();
            user.Name = "aux";
            user.Password = "8GHSggwgMf1utzwNEwAHMQ==";
            user.Salt = "fvSxP5CI";
            user.SaveAndFlush();
        }

        public void AddUser(string name, string password)
        {
            var user = new User {Name = name, AccountType = AccountType.Unknown};
            var saltBytes = new byte[8];

            new RNGCryptoServiceProvider().GetBytes(saltBytes);
            user.Salt = Convert.ToBase64String(saltBytes);
            user.Password = PasswordHelper.Encrypt(password, user.Salt);
            user.SaveAndFlush();
       }
    }
}