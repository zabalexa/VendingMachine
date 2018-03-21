using System;

namespace VendingMachinePresenter.ViewInterfaces
{
    public interface IVendingMachineView
    {
        /// <summary>
        /// Событие готовности главной формы
        /// </summary>
        event EventHandler Shown;
        /// <summary>
        /// Доступ к свойствам и методам элемента управления кошелька клиента
        /// </summary>
        ICashButtonControlView CustomerCashButtonControlView { get; }
        /// <summary>
        /// Доступ к свойствам и методам элемента управления баланса аппарата
        /// </summary>
        ICashButtonControlView VendingMachineChangeCashButtonControlView { get; }
        /// <summary>
        /// Доступ к свойствам и методам элемента управления ассортимента напитков
        /// </summary>
        IGoodsButtonControlView GoodsButtonControlView { get; }
        /// <summary>
        /// Доступ к свойствам и методам элемента управления очереди выдачи напитков
        /// </summary>
        IFillingCupProgressBarControlView FillingCupProgressBarControlView { get; }
        /// <summary>
        /// Отобразить сообщение "Спасибо!"
        /// </summary>
        void ThanksMessage();
        /// <summary>
        /// Отобразить сообщение "Недостаточно средств!"
        /// </summary>
        void InsufficientMessage();
    }
}
