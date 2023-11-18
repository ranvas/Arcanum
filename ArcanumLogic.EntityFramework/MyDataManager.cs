using ArcanumLogic.EntityFramework.Model;
using DataAccess.Abstractions;
using DataAccess.EFCore;
using DataAccess.SqlLite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic.EntityFramework;

public class MyDataManager : SqlLiteDataManager<MyContext>
{
    public MyDataManager(IDataAccessConfiguration configuration)
        : base(configuration)
    {
    }

    protected override Func<DbDataContext> DataContextCreator
        => () => new MyContext();

    public async Task UpdateItem<T>(T item) where T : class
    {
        using var context = DataContextCreator();
        context.Entry(item).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task<Fabrica> GetFabrica(string searchKey)
    {
        var fabrica = await GetAsync<Fabrica>(f => f.SearchKey == searchKey);
        if (fabrica == null)
            throw new Exception($"Fabrica {searchKey} not found");
        return fabrica;
    }

    public async Task<List<Account>> GetPartis()
    {
        var partis = await GetListAsync<Account>(a => a.IsVIP.ToLower() == "true");
        //var partis = await GetListAsync<Account>(a => a.IsVIPValue);
        foreach (var parti in partis)
        {
            parti.Bids = (await GetListAsync<Bid>(b => b.AccountId == parti.Id)).ToList();
        }
        return partis.Where(p => p.Bids.Any()).ToList();
    }

    public async Task<List<Imagine>> GetImaginesWithBids()
    {
        var imagines = await GetAllAsync<Imagine>();
        foreach (var imagine in imagines)
        {
            imagine.Bids = (await GetListAsync<Bid>(b => b.EmagineId == imagine.Id)).ToList();
        }
        return imagines.ToList();
    }

    public async Task AddTransfer(long account, string comment, decimal value, string dateTime, string currency)
    {
        var transfer = new Transfer()
        {
            AccountFromId = account,
            Comment = comment,
            CurrencyValue = value,
            TransferTime = dateTime,
            Currency = currency,
        };
        await AddItemAsync(transfer);
    }

    public async Task<Account?> GetAccountWithTransfers(long tgId)
    {
        var account = await GetAsync<Account>(a => a.TgId == tgId);
        if (account == null)
            return null;
        var transfers = await GetListAsync<Transfer>(t => t.AccountFromId == account.Id);
        account.Transfers = transfers.ToList();
        return account;
    }

    public async Task<List<Research>> GetResearchesWithTree(long accountId)
    {
        var researches = await GetListAsync<Research>(r => r.AccountId == accountId, r => r.Tree!);
        return researches.ToList();
    }

    public async Task<List<Research>> GetResearchesWithTree()
    {
        var researches = await GetAllAsync<Research>(r => r.Tree!);
        return researches.ToList();
    }

    public async Task AddItemAsync<T>(T item) where T : class
    {
        using var context = DataContextCreator();
        await context.AddAsync(item);
        await context.SaveChangesAsync();
    }

    public async Task RemoveAllImagines()
    {
        using var context = DataContextCreator();
        await RemoveAllBids();
        var imagines = await GetAllAsync<Imagine>();

        context.RemoveRange(imagines);
        await context.SaveChangesAsync();
    }

    public async Task RemoveAllTrees()
    {
        using var context = DataContextCreator();
        await RemoveAllResearch();
        var trees = await GetAllAsync<Tree>();

        context.RemoveRange(trees);
        await context.SaveChangesAsync();
    }

    public async Task RemoveAllAccounts()
    {
        using var context = DataContextCreator();
        await RemoveAllResearch();
        await RemoveAllBids();
        await RemoveAllTransfers();
        var accounts = await GetAllAsync<Account>();
        context.RemoveRange(accounts);
        await context.SaveChangesAsync();
    }

    public async Task RemoveAllFabricas()
    {
        using var context = DataContextCreator();
        var fabricas = await GetAllAsync<Fabrica>();
        context.RemoveRange(fabricas);
        await context.SaveChangesAsync();
    }

    private async Task RemoveAllResearch()
    {
        var research = await GetAllAsync<Research>();
        using var context = DataContextCreator();
        context.RemoveRange(research);
        await context.SaveChangesAsync();
    }

    private async Task RemoveAllBids()
    {
        using var context = DataContextCreator();
        var bids = await GetAllAsync<Bid>();
        context.RemoveRange(bids);
        await context.SaveChangesAsync();
    }

    private async Task RemoveAllTransfers()
    {
        using var context = DataContextCreator();
        var transfers = await GetAllAsync<Transfer>();
        context.RemoveRange(transfers);
        await context.SaveChangesAsync();
    }


}
