using System;

namespace VMModels
{
    /// <summary>
    /// Класс для логирования действия камеры
    /// </summary>
    class CameraLog
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Временная метка журнала
        /// </summary>
        public DateTime? LogTimestamp { get; set; }

        /// <summary>
        /// Доменное имя
        /// </summary>
        public string DomainName { get; set; }

        /// <summary>
        /// Имя машины
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
        public string SerialNumber { get; set; }

        /// <summary>
        /// Тег активов
        /// </summary>
        public string AssetTag { get; set; }
        public Guid AccountID { get; set; }
        public string AccountName { get; set; }

        /// <summary>
        /// Номер жетона
        /// </summary>
        public string BudgeNumber { get; set; }

        /// <summary>
        /// Количество заряда
        /// </summary>
        public int Battery { get; set; }

        /// <summary>
        /// Дисковое пространство
        /// </summary>
        public double DiskSpace { get; set; }

        /// <summary>
        /// Заметка
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// Действие камеры
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Количество файлов
        /// </summary>
        public int FileCount { get; set; }
    }
}
