using System;

namespace VideoManager.Model
{
    class FIleCamera
    {
        /// <summary>
        /// Код какой-то, пока не знаю
        /// </summary>
        public int SecurityCode { get; set; }

        /// <summary>
        /// Системное имя файла
        /// </summary>
        public string SystemFileName { get; set; }

        /// <summary>
        /// Оригинальное имя файла
        /// </summary>
        public string OriginalFileName { get; set; }
        
        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string SourcePath { get; set; }
        
        /// <summary>
        /// Дата загрузки файла
        /// </summary>
        public DateTime UploadDate { get; set; }

        /// <summary>
        /// Дата создания файла
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Классификация
        /// </summary>
        public Classification Classification { get; set; }

        /// <summary>
        /// Уровень секретности
        /// </summary>
        public SecurityLevel SecurityLevel { get; set; }

        /// <summary>
        /// Расширение файла
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// Идентификатор системы управления записью
        /// </summary>
        public long RMS_ID { get; set; }

        /// <summary>
        /// Компьютерный идентификатор отправки
        /// </summary>
        public long CAD_ID { get; set; }

        /// <summary>
        /// Рейтинг файла
        /// </summary>
        public Rating Rating { get; set; }

        /// <summary>
        /// Описание файла
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Размер файла
        /// </summary>
        public long Size { get; set; }
    }
}
