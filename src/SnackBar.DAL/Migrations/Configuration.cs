namespace SnackBar.DAL.Migrations
{
    using SnackBar.DAL.Models;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SnackBar.DAL.Context.BarContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SnackBar.DAL.Context.BarContext context)
        {
            if (!context.Set<Buyer>().Any())
            {
                //context.Set<Buyer>().AddOrUpdate(new Buyer { Email = "user1@itexus.com", Pin = "US1R", Balance = -200.0M, IsDeleted = false });
                //context.Set<Buyer>().AddOrUpdate(new Buyer { Email = "user2@itexus.com", Pin = "US2R", Balance = 200.0M, IsDeleted = false });
                //context.Set<Buyer>().AddOrUpdate(new Buyer { Email = "user3@itexus.com", Pin = "US3R", Balance = -2.0M, IsDeleted = false });
            }

            if (!context.Set<Product>().Any())
            {
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Bounty", Count = 1, Barcode = "97762", Price = 1M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Burn", Count = 1, Barcode = "15902", Price = 2M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Kit Kat", Count = 1, Barcode = "08179", Price = 1M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "New mix", Count = 1, Barcode = "00319", Price = 3M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Nuts", Count = 1, Barcode = "355", Price = 1M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Roshen Dark", Count = 1, Barcode = "61328", Price = 2M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Roshen Milk", Count = 1, Barcode = "61325", Price = 2M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Snickers small", Count = 1, Barcode = "00144", Price = 1M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Twix", Count = 1, Barcode = "37702", Price = 1M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Гавайи микс", Count = 1, Barcode = "00039", Price = 2.5M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Жевательный мармелад Бонпари", Count = 1, Barcode = "50626", Price = 1.5M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Зефир Выставочный", Count = 1, Barcode = "01365", Price = 1M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Зефир Чаровей", Count = 1, Barcode = "00848", Price = 1.5M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Кешью", Count = 1, Barcode = "00174", Price = 3M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Молодежная смесь", Count = 1, Barcode = "00032", Price = 3M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Мюсли FreshBox", Count = 1, Barcode = "03208", Price = 0.5M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Мюсли Злаки", Count = 1, Barcode = "08089", Price = 0.5M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Печенье BelVita", Count = 1, Barcode = "27715", Price = 0.5M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Сухарики Bruschette", Count = 1, Barcode = "87500", Price = 2M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Сухарики Хрустим Большие", Count = 1, Barcode = "01365", Price = 1M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Сухарики Хрустим Маленькие", Count = 1, Barcode = "33637", Price = 0.5M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Хлебцы Dr.Korner", Count = 1, Barcode = "71176", Price = 2M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Хлебцы Finn Crisp", Count = 1, Barcode = "09827", Price = 4M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Хлебцы Молодцы", Count = 1, Barcode = "59094", Price = 1.5M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Чипсы Lay's 150г", Count = 1, Barcode = "01605", Price = 3M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Чипсы Lay's 80г", Count = 1, Barcode = "00366", Price = 1.5M, IsDeleted = false });
                //context.Set<Product>().AddOrUpdate(new Product { Name = "Чипсы Pringles", Count = 1, Barcode = "10738", Price = 2M, IsDeleted = false });
            }
        }
    }
}