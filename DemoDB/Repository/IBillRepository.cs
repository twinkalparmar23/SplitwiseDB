using DemoDB.DataModel;
using DemoDB.Model;
using DemoDB.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDB.Repository
{
    public interface IBillRepository
    {
        Task<BillResponse> GetBillAsync(int id);

        Task<Bill> InsertBillAsync(BillModel bill);

        Task<bool> UpdateBillAsync(Bill bill);

        Task<bool> DeleteBillAsync(int id);

        Task<List<BillResponse>> GetAllExpenses(int id);

        Task<List<BillResponse>> GetIndividualExpenses(int Userid, int Friendid);

        Task<List<BillResponse>> GetGroupExpenses(int Groupid);
    }
}
