using System;

namespace SnackBar.BLL.Models
{
    public class LightOrder
    {
        public string ProductName { get; set; }

        public string UserEmail { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }

        public DateTime Date { get; set; }
    }
}