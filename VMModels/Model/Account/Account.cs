using System;
using System.Collections;
using System.Collections.Generic;
using VMModels.Enums;

namespace VMModels.Model
{
    public class Account
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Включен
        /// </summary>
        public bool? IsEnabled { get; set; }

        /// <summary>
        /// Номер жетона
        /// </summary>
        public string BadgeNumber { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Ранг
        /// </summary>
        public string Rank { get; set; }

        /// <summary>
        /// Идентификатор входа
        /// </summary>
        public string LogonID { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Срок истекает
        /// </summary>
        public bool? IsExpires { get; set; }

        /// <summary>
        /// Время истечения
        /// </summary>
        public DateTime? Expiration { get; set; }

        /// <summary>
        /// Блокирован
        /// </summary>
        public bool? IsLocked { get; set; }

        /// <summary>
        /// Закрытая временная метка
        /// </summary>
        public DateTime? LockedTimestamp { get; set; }

        /// <summary>
        /// Пароль сброшен
        /// </summary>
        public bool? IsPwdReset { get; set; }
        public byte[] Photo { get; set; }
        public SECURITY Security { get; set; }
        public SYSTEM_ROLE SystemRole { get; set; }

        /// <summary>
        /// Время создания записи
        /// </summary>
        public DateTime RecordCreated { get; set; }

        /// <summary>
        /// Заметка
        /// </summary>
        public string Memo { get; set; }
        public string PIN { get; set; }

        /// <summary>
        /// Права приложения
        /// </summary>
        public int ApplicationRights { get; set; }

        /// <summary>
        /// Группа переопределена
        /// </summary>
        public bool IsGroupOverride { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// Предупреждающее письмо
        /// </summary>
        public bool IsAlertEmail { get; set; }

        /// <summary>
        /// Идентификатор регистратора подразделения
        /// </summary>
        public Guid Dept_RecId { get; set; }

        /// <summary>
        /// Идентификатор регистратора подстанции
        /// </summary>
        public Guid SubStation_RecId { get; set; }

        /// <summary>
        /// Коллекция групп аккаунтов
        /// </summary>
        public virtual ICollection<AccountGroup> AccountGroups { get; set; }

        /// <summary>
        /// Идентификатор ранга пользователя
        /// </summary>
        public virtual UserRank AccountRank_Id { get; set; }

        public Account()
        {
            AccountGroups = new HashSet<AccountGroup>();
        }

        public override string ToString()
        {
            string str = string.Format("{0}, {1}", LastName, FirstName);
            if (!string.IsNullOrEmpty(MiddleName) && !string.IsNullOrEmpty(MiddleName))
                str = string.Format("{0}, {1} {2}", LastName, FirstName, MiddleName.Substring(0, 1).ToUpper());
            return str;
        }
    }
}
