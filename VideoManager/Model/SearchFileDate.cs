using System.Collections.Generic;

namespace VideoManager.Model
{
    class Classification
    {
        /// <summary>
        /// Имя классификации
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Код классификации
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// Сохранение включено?
        /// </summary>
        public bool RetentionEnabled { get; set; }

        /// <summary>
        /// Количество дней для хранения
        /// </summary>
        public int RetentionDay { get; set; }
    }

    /// <summary>
    /// Уровень Секретности
    /// </summary>
    enum SecurityLevel
    {
        Unclassified,
        Official,
        Secret,
        TopSecret
    }

    /// <summary>
    /// Системные роли
    /// </summary>
    enum SystemRole
    {
        Administrator,
        Supervisor,
        StandartAccount,
        ViewOnly,
        Guest
    }

    /// <summary>
    /// Рейтинг для файла
    /// </summary>
    enum Rating
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5
    }

}
