using System;

namespace VMModels.Model
{
    [Serializable]
    public class DBProfileData
    {
        public string DefaultDBName { get; set; }
        public bool IsLocalDB { get; set; }
        public string DataSource { get; set; }
        public string Catalog { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public bool PersistSecurityInfo { get; set; }
    }
}
