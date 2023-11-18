using ArcanumLogic.Dtos;
using ArcanumLogic.EntityFramework;
using ArcanumLogic.EntityFramework.Model;
using ArcanumLogic.Sheets;
using DataAccess.Abstractions;
using Google.Apis.Sheets.v4.Data;
using Integrators.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Requests.Abstractions;

namespace ArcanumLogic
{
    public class ArcanumManager
    {
        private readonly ArcanumOptions _configuration;
        private readonly GoogleSheetDataManager _dm;
        private readonly MyDataManager _dbManager;
        private readonly Cycle _cycle;
        private readonly IDispatcher _dispatcher;

        public ArcanumManager(IDispatcher dispatcher, ArcanumOptions configuration, GoogleSheetDataManager sheetDataManager, MyDataManager dbManager)
        {
            _cycle = new Cycle(configuration.StartCycle);
            _configuration = configuration;
            _dm = sheetDataManager;
            _dbManager = dbManager;
            _dispatcher = dispatcher;
        }

        #region  root commands

        public async Task<string> TransferDestiny(TransferDestinyDto dto)
        {
            var accountFrom = await _dbManager.GetAccountWithTransfers(dto.TgFrom);
            if(accountFrom == null)
                return "пользователь не найден";
            var accountTo = await _dbManager.GetAsync<Account>(a => a.WalletCode == dto.TransferTo);
            if (accountTo == null)
                return $"Пользователь с кошельком {dto.TransferTo} не найден";
            if (dto.Count <= 0)
                return $"Неверная сумма, должно быть больше 0";
            if(accountFrom.DestinySum() < dto.Count)
            {
                return $"У вас недостаточно судьбы для перевода";
            }
            var countResult = dto.Count * 0.9m;
            await AddTransfer(accountFrom.Id, "перевод игрока игроку", -dto.Count, CurrencyEnum.destiny.ToString());
            await AddTransfer(accountTo.Id, "перевод игрока игроку", countResult, CurrencyEnum.destiny.ToString());
            return $"Вы успешно перевели игроку {accountTo.Name} {countResult} судьбы, комиссия составила 10%";
        }

        public async Task<string> AdminAllSchemes()
        {
            var allSchemes = await _dbManager.GetResearchesWithTree();
            var sb = new StringBuilder();
            var magicString = CurrencyEnum.magic.ToString();
            var magicSchemes = allSchemes.Where(s => s.Tree!.Currency == magicString).ToList();
            sb.AppendLine($"Магия: {magicSchemes.Count}");
            foreach (var searchKeyGroup in magicSchemes.GroupBy(s => s.SearchKey))
            {
                var name = searchKeyGroup.FirstOrDefault()?.Tree?.Name ?? "Ошибка с гребанными связями";
                sb.AppendLine($"{name} - {searchKeyGroup.Count()}");
            }

            var techString = CurrencyEnum.tech.ToString();
            var techSchemes = allSchemes.Where(s => s.Tree!.Currency == techString).ToList();
            sb.AppendLine($"Технология: {techSchemes.Count}");
            foreach (var searchKeyGroup in techSchemes.GroupBy(s => s.SearchKey))
            {
                var name = searchKeyGroup.FirstOrDefault()?.Tree?.Name ?? "Ошибка с гребанными связями";
                sb.AppendLine($"{name} - {searchKeyGroup.Count()}");
            }

            var neutralString = CurrencyEnum.neutral.ToString();
            var neutralSchemes = allSchemes.Where(s => s.Tree!.Currency == neutralString).ToList();
            sb.AppendLine($"Нейтральная: {neutralSchemes.Count}");
            foreach (var searchKeyGroup in neutralSchemes.GroupBy(s => s.SearchKey))
            {
                var name = searchKeyGroup.FirstOrDefault()?.Tree?.Name ?? "Ошибка с гребанными связями";
                sb.AppendLine($"{name} - {searchKeyGroup.Count()}");
            }

            return sb.ToString();
        }

        public async Task<string> Calculate()
        {
            if (_cycle.Locked)
                return $"{_cycle.CurrentCycle} цикл залочен";
            if (!_cycle.Started)
                return $"{_cycle.CurrentCycle} цикл остановлен";
            _cycle.Locked = true;
            try
            {
                await ArcanumHelper.LogMessage($"Запущен пересчет цикла {_cycle.CurrentCycle}", _dispatcher);

                //Получить всех пользователей которые участвовали в ставках
                List<Account> partis = await _dbManager.GetPartis();
                Dictionary<long, CurrencyEnum> dictionary = new();
                foreach (var parti in partis)
                {
                    var destiny = await PayDestiny(parti);
                    dictionary.Add(parti.Id, destiny);
                }

                var imagines = await _dbManager.GetImaginesWithBids();
                foreach (var imagine in imagines)
                {
                    await CalculateImagine(imagine, dictionary);
                }
                await ArcanumHelper.LogMessage($"Пересчет цикла {_cycle.CurrentCycle} завершился", _dispatcher);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _cycle.Locked = false;
            }
            _cycle.CurrentCycle++;
            _cycle.Started = false;
            return $"Пересчет цикла {_cycle.CurrentCycle - 1} успешен, новый цикл {_cycle.CurrentCycle} отановлен";
        }

        public async Task<CurrencyEnum> PayDestiny(Account account)
        {
            decimal sumMagic = 0;
            decimal sumTech = 0;
            decimal sumNeutral = 0;
            decimal mid = 0;
            for (int i = 0; i <= _cycle.CurrentCycle; i++)
            {
                sumMagic = account.SumBidsMagic(i);
                sumTech = account.SumBidsTech(i);
                sumNeutral = account.SumBidsNeutral(i);
                mid += Math.Max(sumTech, sumMagic) + 0.75m * sumNeutral;
            }
            //МАХ(сумма ставок на магию, сумма ставок на технологию) + 0.75*сумма ставок на нейтральное
            decimal currentScoring = await UpdateScoring(account, mid);
            await AddTransfer(account.Id, $"Начисление кармы за {_cycle.CurrentCycle} цикл", currentScoring * _configuration.Cmagic, CurrencyEnum.destiny.ToString());
            if (sumMagic > sumTech)
                return CurrencyEnum.magic;
            if (sumTech > sumMagic)
                return CurrencyEnum.tech;
            return CurrencyEnum.neutral;
        }

        private async Task<decimal> UpdateScoring(Account account, decimal mid)
        {
            List<Tuple<decimal, decimal>> magicTable = new();
            magicTable.Add(new Tuple<decimal, decimal>(1.1m, 1.2m));
            magicTable.Add(new Tuple<decimal, decimal>(2m, 1.4m));
            magicTable.Add(new Tuple<decimal, decimal>(2.5m, 1.8m));
            magicTable.Add(new Tuple<decimal, decimal>(3.5m, 1.9m));
            magicTable.Add(new Tuple<decimal, decimal>(5m, 2m));
            foreach (var magicRow in magicTable)
            {
                if ((magicRow.Item1 * _configuration.Cmagic) <= mid)
                    account.ScoringValue = magicRow.Item2;
            }
            account.Experience = mid;
            await _dbManager.UpdateItem(account);
            return account.ScoringValue;
        }

        public async Task<string> Startycle(string param)
        {
            //param is not implemented
            if (_cycle.Locked)
                return $"{_cycle.CurrentCycle} цикл залочен";
            if (_cycle.Started)
                return $"{_cycle.CurrentCycle} цикл уже запущен";

            await ArcanumHelper.LogMessage($"Запущен цикл {_cycle.CurrentCycle}", _dispatcher);

            _cycle.Started = true;
            return $"{_cycle.CurrentCycle} запущен";
        }

        public async Task<string> StopCycle()
        {
            if (_cycle.Locked)
                return $"{_cycle.CurrentCycle} цикл залочен";
            if (!_cycle.Started)
                return $"{_cycle.CurrentCycle} цикл не запущен";

            await ArcanumHelper.LogMessage($"Остановлен цикл {_cycle.CurrentCycle}", _dispatcher);

            _cycle.Started = false;
            return $"{_cycle.CurrentCycle} остановлен";
        }

        public async Task<string> InitCommand(string enumModel)
        {
            switch (enumModel)
            {
                case nameof(DispatcherCommandsEnum.init_users):
                    return await InitUsers() ? "Пользователи созданы" : "Что-то пошло не так.";
                case nameof(DispatcherCommandsEnum.init_imagine):
                    return await InitImagines() ? "Пространства созданы" : "Что-то пошло не так.";
                case nameof(DispatcherCommandsEnum.init_tree):
                    return await InitTree() ? "Дерево исследований заполнено" : "Что-то пошло не так.";
                case nameof(DispatcherCommandsEnum.init_fabricas):
                    return await InitFabricas() ? "Фабрики созданы, фронт обеспечен" : "Что-то пошло не так.";
                default:
                    return $"команда {enumModel} не найдена";
            }
        }

        public async Task<string> UpdateImagines()
        {
            var allImagines = await _dm.ImagineSheet.GetAllItemsAsync();
            foreach (var imagine in allImagines)
            {
                var dbImagine = await _dbManager.GetAsync<Imagine>(i => i.SearchKey == imagine.SearchKey);
                if (dbImagine == null)
                {
                    await _dbManager.AddItemAsync(imagine);
                }
                else
                {
                    dbImagine.Currency = imagine.Currency;
                    dbImagine.MagicValue = imagine.MagicValue;
                    dbImagine.ValueStart = imagine.ValueStart;
                    dbImagine.Description = imagine.Description;
                    await _dbManager.UpdateItem(dbImagine);
                }
            }
            return $"Протранства обновлены";
        }

        public async Task<string> UpdateUsers()
        {
            var allUsers = await _dm.AccountSheet.GetAllItemsAsync();
            foreach (var user in allUsers)
            {
                var account = await _dbManager.GetAsync<Account>(a => a.TgName.ToLower() == user.TgName.ToLower());
                if (account == null)
                {
                    user.TgName = user.TgName.ToLower();
                    await _dbManager.AddItemAsync(user);
                }
                else
                {
                    account.TechPointsStart = user.TechPointsStart;
                    account.Name = user.Name;
                    account.WalletCode = user.WalletCode;
                    account.DestinyPointsStart = user.DestinyPointsStart;
                    account.MagicPointsStart = user.MagicPointsStart;
                    account.TechPointsStart = user.TechPointsStart;
                    account.IsVIP = user.IsVIP;
                    account.ZonesText = user.ZonesText;
                    account.Fabrica = user.Fabrica;
                    await _dbManager.UpdateItem(account);
                }
            }
            return $"Пользователи обновлены";
        }

        public async Task<bool> InitFabricas()
        {
            var fabricas = await _dm.FabricaSheet.GetAllItemsAsync();
            await _dbManager.RemoveAllFabricas();
            foreach (var fabrica in fabricas)
            {
                await _dbManager.AddItemAsync(fabrica);
            }
            return true;
        }

        public async Task<bool> InitTree()
        {
            var trees = await _dm.TreeSheet.GetAllItemsAsync();
            await _dbManager.RemoveAllTrees();
            foreach (var tree in trees)
            {
                await _dbManager.AddItemAsync(tree);
            }
            return true;
        }

        public async Task<bool> InitImagines()
        {
            var allImagines = await _dm.ImagineSheet.GetAllItemsAsync();
            await _dbManager.RemoveAllImagines();
            foreach (var imagine in allImagines)
            {
                await _dbManager.AddItemAsync(imagine);
            }
            return true;
        }

        public async Task<bool> InitUsers()
        {
            var allUsers = await _dm.AccountSheet.GetAllItemsAsync();
            await _dbManager.RemoveAllAccounts();
            foreach (var user in allUsers)
            {
                user.TgName = user.TgName.ToLower();
                user.TgId = -1;
                await _dbManager.AddItemAsync(user);
            }
            return true;
        }

        #endregion

        #region public and vip commands



        public async Task<string> GetImagines()
        {
            var imagines = await _dbManager.GetImaginesWithBids();
            var sb = new StringBuilder();
            sb.AppendLine("Пространства судьбы:");
            foreach (var imagine in imagines)
            {
                sb.AppendLine($"<b>{imagine.Description}</b> ID {imagine.SearchKey} {imagine.Currency} {imagine.SumBidsByCycle(_cycle.CurrentCycle)}/{imagine.Capacity}");
            }
            return sb.ToString();
        }

        public async Task<string> GetSchemas(long tgId)
        {
            var account = await _dbManager.GetAsync<Account>(a => a.TgId == tgId);
            if (account == null)
                return $"Пользователь не найден";

            var researched = await _dbManager.GetResearchesWithTree(account.Id);
            if (researched.Count == 0)
                return "Нет изученных схем";
            var sb = new StringBuilder();
            sb.AppendLine("Изученные схемы:");
            foreach (var research in researched)
            {
                sb.AppendLine($"{research.Tree.Name}");
            }
            return sb.ToString();
        }

        public async Task<string> LearnNeutral(LearnNeutralDto dto)
        {
            var tree = await _dbManager.GetAsync<Tree>(t => t.SearchKey == dto.SearchKey);
            if (tree == null)
                return $"Неверный ключ схемы";

            if (tree.Currency != CurrencyEnum.neutral.ToString())
                return $"Для изучения этой схемы выполните команду /learn";

            var account = await _dbManager.GetAccountWithTransfers(dto.TgId);
            if (account == null)
                return $"Пользователь не найден";

            var learned = await _dbManager.GetListAsync<Research>(r => r.AccountId == account.Id);
            if (learned.Any(l => l.SearchKey == tree.SearchKey))
                return $"Схема уже изучена";

            foreach (var req in tree.Requirements)
            {
                if (!learned.Select(l => l.SearchKey).Contains(req))
                    return $"Не выполнены условия для изучения схемы";
            }

            if (tree.CostValue != (dto.MagicPoints + dto.TechPoints))
                return $"Неверное количество очков магии и технологии для изучения схемы";

            if (account.MagicSum() < dto.MagicPoints)
                return $"Недостаточно очков магии";

            if (account.TechSum() < dto.TechPoints)
                return $"Недостаточно очков технологии";

            await AddNeutralResearch(account, tree, dto.MagicPoints, dto.TechPoints);

            return $"Схема изучена";
        }

        public async Task<string> Learn(LearnDto dto)
        {
            var tree = await _dbManager.GetAsync<Tree>(t => t.SearchKey == dto.SearchKey);
            if (tree == null)
                return $"Неверный ключ схемы";

            if (tree.Currency == CurrencyEnum.neutral.ToString())
                return $"Для изучения этой схемы выполните команду /learn_neutral";

            var account = await _dbManager.GetAccountWithTransfers(dto.TgId);
            if (account == null)
                return $"Пользователь не найден";

            var learned = await _dbManager.GetListAsync<Research>(r => r.AccountId == account.Id);
            if (learned.Any(l => l.SearchKey == tree.SearchKey))
                return $"Схема уже изучена";

            foreach (var req in tree.Requirements)
            {
                if (!learned.Select(l => l.SearchKey).Contains(req))
                    return $"Не выполнены условия для изучения схемы";
            }

            if (tree.Currency == CurrencyEnum.magic.ToString())
            {
                if (account.MagicSum() < tree.CostValue)
                    return $"Недостаточно очков магии";
            }
            else
            {
                if (account.TechSum() < tree.CostValue)
                    return $"Недостаточно очков технологии";
            }
            await AddResearch(account, tree);

            return $"Схема изучена";
        }

        public async Task PayImagine(Imagine imagine, decimal payperOne, Dictionary<long, CurrencyEnum> dic, decimal maxDestiny)
        {
            foreach (var bid in imagine.Bids.Where(b => b.Cycle == _cycle.CurrentCycle))
            {
                decimal fool = bid.Value;
                if (bid.Value > maxDestiny)
                {
                    fool = maxDestiny;
                }

                if (!dic.ContainsKey(bid.AccountId))
                    return;
                var message = $"выплата по ставке на {imagine.Description} за {_cycle.CurrentCycle} цикл";
                var value = dic.TryGetValue(bid.AccountId, out var result);
                if (!value)
                    throw new NotImplementedException("Ошибка приведения currency");

                if (result == CurrencyEnum.neutral)
                {
                    decimal payed = Math.Round((payperOne * fool) / 2.2m, 0);
                    await AddTransfer(bid.AccountId, message, payed, CurrencyEnum.magic.ToString());
                    await AddTransfer(bid.AccountId, message, payed, CurrencyEnum.tech.ToString());
                }
                else
                {
                    var payed = Math.Round(payperOne * fool, 0);
                    await AddTransfer(bid.AccountId, message, payed, result.ToString());
                }
            }
        }

        public async Task<string> MakeADeal(BidDto bid)
        {
            if (!_cycle.Started)
                return $"Сейчас не время делать ставки";

            if (bid.SumValue <= 0)
                return $"Неверная ставка.";
            var imagine = await _dbManager.GetAsync<Imagine>(i => i.SearchKey == bid.ID);
            if (imagine == null)
                return $"<s>Хватит перебирать, логи пишутся, юникс уже выехал за твоим очком.</s> Неверное пространство.";

            var account = await _dbManager.GetAccountWithTransfers(bid.TgId);
            if (account == null)
                throw new ArgumentNullException(nameof(account));
            var destinySum = account.DestinySum();
            if (destinySum < bid.SumValue)
                return $"Не хватает очков судьбы, требуется: {bid.SumValue}, есть: {destinySum}";
            await AddTransfer(account.Id, $"make a deal {bid.SumValue} судьбы на {imagine.Currency} ", -bid.SumValue, CurrencyEnum.destiny.ToString());

            var realBid = new Bid
            {
                AccountId = account.Id,
                Currency = imagine.Currency,
                EmagineId = imagine.Id,
                Cycle = _cycle.CurrentCycle,
                Value = bid.SumValue
            };
            await _dbManager.AddItemAsync<Bid>(realBid);
            return $"Ставка успешно сделана, вы поставили {bid.SumValue} судьбы на {imagine.Description}({imagine.Currency})";
        }

        public async Task<bool> Login(RequestBase request)
        {
            if (string.IsNullOrEmpty(request.TgName))
            {
                return false;
            }
            var account = await FindAccountAsync(request);
            return account != null;
        }

        public async Task<bool> LoginVip(RequestBase request)
        {
            var account = await FindAccountAsync(request);
            return account?.IsVIPValue ?? false;
        }

        public async Task<ResponseBase> GetFinalStatus(RequestBase request)
        {
            var account = await _dbManager.GetAccountWithTransfers(request.TgId);
            if (account == null)
                return await ErrorMessageAsync(request, $"Вы не зарегистрированы на игру или вас не добавили {request.TgId} - {request.TgName}");
            var sb = new StringBuilder();
            sb.AppendLine($"Статус для <b>@{account.TgName}</b> {request.TgId}:");
            sb.AppendLine($"Имя персонажа: <b>{account.Name}</b>");
            sb.AppendLine($"Ваши зоны: <b>{account.ZonesText}</b>");
            sb.AppendLine();
            if (!string.IsNullOrEmpty(account.Fabrica))
            {
                var fabrica = await _dbManager.GetFabrica(account.Fabrica);
                sb.AppendLine($"Связан с фабрикой <b>{fabrica.Name}</b>");
                sb.AppendLine($"Выработка за одного спутника <b>{fabrica.ProgressPerOne}</b>");
                sb.AppendLine($"Максимальная выработка <b>{fabrica.CurrentMax}</b>");
            }
            if (account.IsVIPValue)
            {
                sb.AppendLine($"Видит исследования");
                sb.AppendLine($"{account.DestinySum()} <b>судьбы</b>");
                sb.AppendLine($"{account.MagicSum()} <b>магии</b>");
                sb.AppendLine($"{account.TechSum()} <b>технологии</b>");
                sb.AppendLine($"{account.Scoring} <b>скоринг</b>");
                sb.AppendLine($"{account.Experience} <b>опыт скоринга</b>");
                sb.AppendLine($"Код кошелька: <b>{account.WalletCode}</b>");
            }

            return new ResponseBase() { ResultMessage = sb.ToString(), Id = request.TgId };
        }

        #endregion

        #region private

        private async Task AddTransfer(long account, string comment, decimal value, string currency)
        {
            await _dbManager.AddTransfer(account,
                comment, value, ArcanumHelper.GetDateTimeString(), currency);
        }

        private async Task AddNeutralResearch(Account account, Tree tree, decimal magicPoints, decimal techPoints)
        {
            await AddTransfer(account.Id, $"списание средств на изучение схемы {tree.SearchKey}", -magicPoints, CurrencyEnum.magic.ToString());
            await AddTransfer(account.Id, $"списание средств на изучение схемы {tree.SearchKey}", -techPoints, CurrencyEnum.tech.ToString());
            var research = new Research
            {
                AccountId = account.Id,
                SearchKey = tree.SearchKey,
                TimeOfResearch = ArcanumHelper.GetDateTimeString()
            };
            await _dbManager.AddItemAsync(research);
        }

        private async Task AddResearch(Account account, Tree tree)
        {
            await AddTransfer(account.Id, $"списание средств на изучение схемы {tree.SearchKey}", -tree.CostValue, tree.Currency);
            var research = new Research
            {
                AccountId = account.Id,
                SearchKey = tree.SearchKey,
                TimeOfResearch = ArcanumHelper.GetDateTimeString()

            };
            await _dbManager.AddItemAsync(research);
            var added = await _dbManager.GetAsync<Research>(r => r.SearchKey == tree.SearchKey);
            var list = await _dbManager.GetAllAsync<Research>();
        }

        private async Task<bool> CalculateImagine(Imagine imagine, Dictionary<long, CurrencyEnum> dic)
        {
            double a = _configuration.Amagic;
            double b = _configuration.Bmagic;
            decimal currentBidsSum = imagine.Bids.Where(b => b.Cycle == _cycle.CurrentCycle).Sum(b => b.Value);
            if (currentBidsSum == 0)
                return false;
            decimal sumDestiny = 0;
            decimal maxDestiny = (decimal.TryParse(imagine.ValueStart, out decimal result) ? result : 0) *
                (decimal.TryParse(imagine.MagicValue, out decimal result2) ? result2 : 0);
            decimal payPerOne = 0;
            if (currentBidsSum > maxDestiny)
            {
                currentBidsSum = maxDestiny;
            }
            if (currentBidsSum == 0)
                return true;
            for (var i = 1; i <= currentBidsSum; i++)
            {
                decimal increment = 0;
                try
                {
                    increment = (decimal)Math.Round((20 / (Math.Max(1, Math.Round(Math.Log(a * i, b), 3)))), 0);
                }
                catch (Exception e)
                {
                    increment = 5;
                    Console.WriteLine("Ошибка в математике, штош!!!");
                }
                sumDestiny += increment;
            }
            payPerOne = sumDestiny / currentBidsSum;
            await PayImagine(imagine, payPerOne, dic, maxDestiny);

            return true;
        }

        private async Task<Account?> FindAccountAsync(RequestBase request)
        {
            var lookById = await _dbManager.GetAsync<Account>(x => x.TgId == request.TgId);
            if (lookById != null)
                return lookById;
            if (string.IsNullOrEmpty(request.TgName))
                return null;
            var lookByName = await _dbManager.GetAsync<Account>(x => x.TgName == request.TgName.ToLower());
            if (lookByName != null)
            {
                lookByName.TgId = request.TgId;
                await _dbManager.UpdateItem(lookByName);
            }
            return lookByName;
        }

        private Task<ResponseBase> ErrorMessageAsync(RequestBase request, string msg)
        {
            return Task.FromResult(new ResponseBase { ResultMessage = msg, Id = request.TgId, IsError = true });
        }

        #endregion
    }
}
