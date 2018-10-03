﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Http;

namespace Northwind.API.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private NorthwindContext db = null;

        public CustomerRepository()
        {
            this.db = new NorthwindContext();
        }

        //dependecy injection
        public CustomerRepository(NorthwindContext db)
        {
            this.db = db;
        }

        public IEnumerable<Customer> GetAll()
        {
            return db.Customers.Include(c=>c.Orders);
        }

        public Customer GetById(string id)
        {
            return db.Customers.FirstOrDefault(c => c.CustomerID.Equals(id, StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<Order> GetCustomerOrders(string id)
        {
            return db.Orders.Include(o=>o.Order_Details.Select(x=>x.Product)).Where(m => m.CustomerID.Equals(id, StringComparison.InvariantCultureIgnoreCase));
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if(!this.disposed)
            {
                if(disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}