// File: WalletRepository.cs

using Barstation.Models;

namespace Barstation.Services
{
    public interface IWalletRepository
    {
        List<Wallet> GetAll();
        Wallet? TryGetByOwner(string owner);
        void Add(Wallet wallet);

        void BlockByOwner(string username);
        Wallet Unlock(Wallet wallet);
        Wallet Update(Wallet wallet);
    }

    public class WalletRepository : IWalletRepository
    {
        //Todo : add persistence
        private readonly List<Wallet> _wallets = new List<Wallet>
    {
        new Wallet
        {
            Id = 0,
            Owner = "User1",
            CurrentValue = 100.00M,
            LastUpdate = DateTime.UtcNow
        },
        new Wallet
        {
            Id = 1,
            Owner = "User2",
            CurrentValue = 50.00M,
            LastUpdate = DateTime.UtcNow
        }
    };

        public List<Wallet> GetAll() => _wallets;

        public Wallet? TryGetByOwner(string owner)
        {
            var target = _wallets.FirstOrDefault(w => w.Owner == owner);
            if (target == null)
            {
                //log "not found"
                return null;
            }
            return target;
        }

        public void Add(Wallet wallet) => _wallets.Add(wallet);

        public void BlockByOwner(string username)
        {
            var target = TryGetByOwner(username);
            if(target == null)
            {
                //log "nothing to do"
                Console.WriteLine("Wallet doesn't exist in system.");
                return;
            }
            target.IsBlocked = true;
            Update(target);
            return;
        }

        public Wallet Unlock(Wallet wallet)
        {
            var target = _wallets.Find(w => w.Id == wallet.Id);
            if (target == null)
            {
                throw new Exception("Wallet does not exist in system");
            }
            if (target.IsBlocked)
            {
                target.IsBlocked = false;

                wallet = Update(target);
            }
            return wallet;
        }

        public Wallet Update(Wallet wallet)
        {
            var target = _wallets.Find(w => w.Id == wallet.Id);
            if (target == null)
            {
                Add(wallet);
            }
            else
            {
                _wallets[_wallets.IndexOf(target)] = wallet;
            }
            return wallet;
        }
    }

}