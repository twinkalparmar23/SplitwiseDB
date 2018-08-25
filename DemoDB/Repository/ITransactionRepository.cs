using DemoDB.Model;
using DemoDB.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDB.Repository
{
    public interface ITransactionRepository
    {
        Task<TransactionResponse> GetTransactionAsync(int id);

        Task<Transactions> RecordPaymentAsync(Transactions payment);

        Task<List<TransactionResponse>> GetGroupTransactionsAsync(int Groupid);

        Task<List<TransactionResponse>> GetIndividualTransactionsAsync(int Userid,int Friendid);

        Task<List<TransactionResponse>> GetAllTransactionsAsync(int Userid);

        
    }
}
