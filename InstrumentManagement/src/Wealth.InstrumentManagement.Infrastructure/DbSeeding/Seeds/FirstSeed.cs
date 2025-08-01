﻿using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Repositories;
using Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

namespace Wealth.InstrumentManagement.Infrastructure.DbSeeding.Seeds;

public class FirstSeed : IDbSeeder
{
    private readonly UnitOfWork unitOfWork;
    private readonly IBondsRepository bondsRepository;
    private readonly IStocksRepository stocksRepository;

    public FirstSeed(UnitOfWork unitOfWork, IBondsRepository bondsRepository, IStocksRepository stocksRepository)
    {
        this.unitOfWork = unitOfWork;
        this.bondsRepository = bondsRepository;
        this.stocksRepository = stocksRepository;
    }

    public async Task SeedAsync()
    {
        await unitOfWork.BeginTransaction();
        var bonds = await bondsRepository.GetInstruments();
        if (!bonds.Any())
        {
            await CreateBonds();
            await CreateStocks();
        }
        await unitOfWork.Commit(CancellationToken.None);
    }

    private async Task CreateBonds()
    {
        var bond1 = await bondsRepository.CreateBond(new Guid("00000000-0000-0000-0000-000000000001"), "test-bond-1", new ISIN("000000000001"));
        await bondsRepository.ChangePrice(bond1, new Money("RUB", 12.34m));
        await bondsRepository.ChangeCoupon(bond1, new Coupon("RUB", 33m));

        var bond2 = await bondsRepository.CreateBond(new Guid("00000000-0000-0000-0000-000000000002"), "test-bond-2", new ISIN("000000000002"));
        await bondsRepository.ChangePrice(bond2, new Money("USD", 2.12m));
        await bondsRepository.ChangeCoupon(bond2, new Coupon("RUB", 44m));
    }

    private async Task CreateStocks()
    {
        var stock1 = await stocksRepository.CreateStock(new Guid("00000000-0000-0000-0000-000000000003"), "test-stock-1", new ISIN("000000000003"));
        await stocksRepository.ChangePrice(stock1, new Money("RUB", 111m));
        await stocksRepository.ChangeDividend(stock1, new Dividend("USD", 222m));
        await stocksRepository.ChangeLotSize(stock1, 10);
        
        var stock2 = await stocksRepository.CreateStock(new Guid("00000000-0000-0000-0000-000000000004"), "test-stock-2", new ISIN("000000000004"));
        await stocksRepository.ChangePrice(stock2, new Money("USD", 222m));
        await stocksRepository.ChangeDividend(stock2, new Dividend("USD", 333m));
    }
}