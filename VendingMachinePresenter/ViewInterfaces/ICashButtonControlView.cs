using System;
using System.Collections.Generic;
using VendingMachineDataInjectionInterfaces;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;

namespace VendingMachinePresenter.ViewInterfaces
{
    public interface ICashButtonControlView
    {
        /// <summary>
        /// Событие готовности элемента управления
        /// </summary>
        event EventHandler Shown;
        /// <summary>
        /// Событие опускания монеты в монетоприемник
        /// </summary>
        event VoidEventHandler<CoinType> PutCoin;
        /// <summary>
        /// Событие необходимости обновить информацию по количеству монет в аппарате
        /// </summary>
        event VoidEventHandler RefreshVendingMachine;
        /// <summary>
        /// Событие необходимости произвести размен неизрасходованного баланса и возврат внесенной суммы клиенту
        /// </summary>
        event VoidEventHandler Change;
        /// <summary>
        /// Событие запроса информации по возможности размена
        /// </summary>
        event RequestEventHandler<Dictionary<CoinType, int>> ChangeSimulate;
        /// <summary>
        /// Готов к работе
        /// </summary>
        /// <param name="e">Параметр, передаваемый от родительского элементапо стеку вызова</param>
        void Activate(EventArgs e);
        /// <summary>
        /// Указать тип поведения элемента
        /// </summary>
        /// <param name="type">Тип поведения</param>
        void SetControlType(AccountType type);
        /// <summary>
        /// Обновить информацию по количеству монет различного номинала
        /// </summary>
        /// <param name="coins">Монеты разного номинала</param>
        /// <param name="paid">Баланс внесенной суммы</param>
        void RefreshBalance(Dictionary<CoinType, Coin> coins, int paid = 0);
        /// <summary>
        /// Обновление количества монет в аппарате
        /// </summary>
        void RefreshVendingMachineBalance();
    }
}
