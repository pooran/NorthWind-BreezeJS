using Breeze.ContextProvider;
using Breeze.ContextProvider.EF6;
using Newtonsoft.Json.Linq;
using NorthWind.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NorthWind.Controllers
{
    public class NorthWindController : ApiController
    {
        readonly EFContextProvider<NorthWindEntities> _contextProvider
            = new EFContextProvider<NorthWindEntities>();

        [HttpGet]
        public string Metadata()
        {
            return _contextProvider.Metadata();
        }

        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return _contextProvider.SaveChanges(saveBundle);
        }
        #region IQueryables
        [HttpGet]
        public IQueryable<Customer> Customers()
        {
            return  _contextProvider.Context.Customers;
        }
        [HttpGet]
        public IQueryable<Order> Orders()
        {
            return _contextProvider.Context.Orders;
        }
        [HttpGet]
        public IQueryable<Order_Detail> OrderDetails()
        {
            return _contextProvider.Context.Order_Details;
        }

        [HttpGet]
        public IQueryable<Product> Products()
        {
            return _contextProvider.Context.Products;
        }
        [HttpGet]
        public IQueryable<Category> Categories()
        {
            return _contextProvider.Context.Categories;
        }
        [HttpGet]
        public IQueryable<Employee> Employees()
        {
            return _contextProvider.Context.Employees;
        }
        [HttpGet]
        public IQueryable<Shipper> Shippers()
        {
            return _contextProvider.Context.Shippers;
        }
        [HttpGet]
        public IQueryable<CustomerDemographic> CustomerDemographics()
        {
            return _contextProvider.Context.CustomerDemographics;
        }
        [HttpGet]
        public IQueryable<Region> Regions()
        {
            return _contextProvider.Context.Regions;
        }
        [HttpGet]
        public IQueryable<Territory> Territories()
        {
            return _contextProvider.Context.Territories;
        }
        [HttpGet]
        public IQueryable<Supplier> Suppliers()
        {
            return _contextProvider.Context.Suppliers;
        }
        #endregion

        
    }
}
