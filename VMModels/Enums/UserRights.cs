using System;

namespace VMModels.Enums
{
    /// <summary>
    /// Пользовательские права
    /// </summary>
    [Flags]
    public enum UserRights
    {
        NONE = 0,
        DVD = 1,
        EXPORT = 2,
        IMPORT = 4,
        REPORTS = 8,
        SHARE = 16,
        VIEWCAT = 32,
        SETS = 64,
        SYSTEM = 128,
        DELETE = 256,
        UPLOAD = 512,
        SCAN = 1024,
        PRINT = 2048,
        PRINTPRE = 4096,
        VIDEO = 8192,
        IMAGES = 16384,
        VIDEO_EDIT = 32768,
        SEND_EMAIL = 65536,
        MEMO_CREATE = 131072,
        MEMO_VIEW = 262144,
        UPLOAD_OTHER_CAMS = 524288,
        WINDOW_APPS = 1048576,
        GLOBAL_CAT = 2097152,
        REDACT = 4194304,
        DIST_ADMIN = 8388608,
    }
}
