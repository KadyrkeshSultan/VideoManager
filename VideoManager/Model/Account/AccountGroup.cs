using System;
using System.Collections;
using System.Collections.Generic;

namespace VideoManager.Model
{
    class AccountGroup
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Включено
        /// </summary>
        public bool IsEnabled { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Описание группы аккаунтов
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// Права приложения
        /// </summary>
        public int ApplicationRights { get; set; }

        /// <summary>
        /// Коллекция аккаунтов
        /// </summary>
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
