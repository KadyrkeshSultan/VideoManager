using System;

namespace VideoManager.Model
{
    class VideoFile
    {
        public VideoFile(string SystemName, string OriginalName, string SourcePath, DateTime UploadDate, DateTime CreateDate, long Size)
        {
            this.SystemFileName = SystemName;
            this.OriginalFileName = OriginalName;
            this.SourcePath = SourcePath;
            this.UploadDate = UploadDate;
            this.CreateDate = CreateDate;
            this.Size = Size;
        }

        /// <summary>
        /// Код какой-то, пока не знаю
        /// </summary>
        public int? SecurityCode { get { return null; }}

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

        /// <summary>            ♦
        /// Идентификатор системы управления записью
        /// </summary>
        public long RMS_ID { get; set; }

        /// <summary>
        /// Компьютерный идентификатор отправки
        /// </summary>
        public long CAD_ID { get; set; }

        /// <summary>
        /// Отмечено как доказательство
        /// </summary>
        public bool MarkedEvidence { get; set; }

        /// <summary>
        /// Рейтинг файла
        /// </summary>
        public Rating Rating { get; set; }

        /// <summary>
        /// Описание файла
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Короткое описание файла
        /// </summary>
        public string ShortDescription { get; set; }
        
        /// <summary>
        /// Размер файла
        /// </summary>
        public long Size { get; set; }
    }

    class VideoTag
    {
        /// <summary>
        /// Промежуток времени
        /// </summary>
        public TimeSpan TimeOffset { get; set; }

        /// <summary>
        /// Кадр видео
        /// </summary>
        public int VideoFrame { get; set; }

        /// <summary>
        /// Конструктор Видеотэга
        /// </summary>
        /// <param name="TimeOffset">Промежуток времени</param>
        /// <param name="VideoFrame">Кадр видео</param>
        public VideoTag(TimeSpan TimeOffset, int VideoFrame)
        {
            this.TimeOffset = TimeOffset;
            this.VideoFrame = VideoFrame;
        }
    }
}
