using DbModelsLib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using DbContextLib;
namespace DbCRUDReposLib
{
    public class CustomerRepository : ICustomerRepository
    {
        DbContextLib.MainDbContext _db = null;

        public async Task<Customer> CreateAsync(Customer cust)
        {
            var added = await _db.Customers.AddAsync(cust);

            int affected = await _db.SaveChangesAsync();
            if (affected != 0)
                return cust;
            else
                return null;
        }

        public async Task<Customer> DeleteAsync(Guid custId)
        {
            var cusDel = await _db.Customers.FindAsync(custId);
            _db.Customers.Remove(cusDel);

            int affected = await _db.SaveChangesAsync();
            if (affected != 0)
                return cusDel;
            else
                return null;
        }

        public async Task<Customer> ReadAsync(Guid custId)
        {
            var cus = await _db.Customers.FindAsync(custId);
            var orders = _db.Orders.ToList();                   //Needed if I want EFC to load the embedded pearls

            return cus;
        }

        public async Task<IEnumerable<Customer>> ReadAllAsync()
        {
            return await Task.Run(() => _db.Customers);
        }

        public async Task<DbInfo> ReadDbInfoAsync()
        {
            var dbInfo = new DbInfo();

            dbInfo.dBConnection = AppConfig.DbExecution.DbConnection;
            dbInfo.NrCustomers = await _db.Customers.CountAsync();
            dbInfo.NrOrders = await _db.Orders.CountAsync();

            return dbInfo;
        }

        public async Task<Customer> UpdateAsync(Customer cust)
        {
            _db.Customers.Update(cust); //No db interaction until SaveChangesAsync
            int affected = await _db.SaveChangesAsync();
            if (affected != 0)
                return cust;
            else
                return null;
        }
 
        public CustomerRepository(DbContextLib.MainDbContext db)
        {
            _db = db; 
        }
    }
}
