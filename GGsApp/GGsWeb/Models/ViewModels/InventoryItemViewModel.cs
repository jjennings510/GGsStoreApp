using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GGsWeb.Models
{
    public class InventoryItemViewModel
    {
        [DisplayName("Name")]
        public string name { get; set; }
        [DataType(DataType.Text)]
        [DisplayName("Platform")]
        public string platform { get; set; }
        [DataType(DataType.Text)]
        [DisplayName("Rating")]
        public string esrb { get; set; }
        [DisplayName("Quantity")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter an integer greater than 1")]
        public int quantity { get; set; }
        [DataType(DataType.Currency)]
        [DisplayName("Cost")]
        public decimal cost { get; set; }
        [DataType(DataType.Text)]
        [DisplayName("Description")]
        public string description { get; set; }
        [DisplayName("Location")]
        public int locationId { get; set; }
        public int apiId { get; set; }
        public string imageURL { get; set; }
    }
}
