using System;

namespace VideoManager.Model
{
    class AccountLog
    {
        public Guid Id { get; set; }
        public Guid? AccountId { get; set; }
        public string AccountName { get; set; }

        /// <summary>
        /// Номер жетона
        /// </summary>
        public string BudgeNumber { get; set; }

        /// <summary>
        /// Временная метка журнала
        /// </summary>
        public DateTime? LogTimestamp { get; set; }

        /// <summary>
        /// Доменное имя
        /// </summary>
        public string DomainName { get; set; }

        /// <summary>
        /// Название машины
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// Аккаунт машины
        /// </summary>
        public string MachineAccount { get; set; }
        public string IPAddress { get; set; }

        /// <summary>
        /// Идентификатор машины
        /// </summary>
        public string MachineID { get; set; }

        /// <summary>
        /// Заметка
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// Действие
        /// </summary>
        public string Action { get; set; }
    }
}
