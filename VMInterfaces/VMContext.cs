using System.Data.Entity;
using VMModels.Model;
using VMInterfaces.Entity_Configs;

namespace VMInterfaces
{
    public class VMContext :DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountGroup> AccountGroups { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Substation> Substations { get; set; }
        public DbSet<Manufacturer> Manufacturers {  get;  set; }
        public DbSet<Product> Products {  get;  set; }
        public DbSet<Inventory> Inventories {  get;  set; }
        public DbSet<InventoryLog> InventoryLogs {  get;  set; }
        public DbSet<UserRank> UserRanks {  get;  set; }
        public DbSet<RMA> RMAs {  get;  set; }
        public DbSet<StateProvince> States {  get;  set; }
        public DbSet<Race> Races {  get;  set; }
        public DbSet<Country> Countries {  get;  set; }
        public DbSet<Classification> Classifications {  get;  set; }
        public DbSet<GlobalConfig> GlobalCfg {  get;  set; }
        public DbSet<DomainConfig> DomainCfg {  get;  set; }
        public DbSet<MetaData> MetaDatas {  get;  set; }
        public DbSet<PersonRole> PersonRoles {  get;  set; }
        public DbSet<CameraFolder> CameraFolders {  get;  set; }
        public DbSet<FileType> FileTypes {  get;  set; }
        public DbSet<FileExt> FileExts {  get;  set; }
        public DbSet<AppExt> AppExts {  get;  set; }
        public DbSet<AlertEmail> AlertEmails {  get;  set; }
        public DbSet<RightsProfile> RightsProfiles {  get;  set; }
        public DbSet<License> Licenses {  get;  set; }
        public DbSet<Case> Cases {  get;  set; }
        public DbSet<CaseAddress> CaseAddresses {  get;  set; }
        public DbSet<Alias> Aliases {  get;  set; }
        public DbSet<CaseMemo> CaseMemos {  get;  set; }
        public DbSet<CaseMetaData> CaseData {  get;  set; }
        public DbSet<CaseVehicle> CaseVehicles {  get;  set; }
        public DbSet<Incident> Incidents {  get;  set; }
        public DbSet<Person> Persons {  get;  set; }
        public DbSet<PersonAddress> PersonAddresses {  get;  set; }
        public DbSet<PersonMemo> PersonMemos {  get;  set; }
        public DbSet<PersonVehicle> PersonVehicles {  get;  set; }
        public DbSet<Evidence> CaseEvidence {  get;  set; }
        public DbSet<SystemLog> SystemLogs {  get;  set; }
        public DbSet<AccountLog> AccountLogs {  get;  set; }
        public DbSet<CameraLog> CameraLogs {  get;  set; }
        public DbSet<DataFile> DataFiles {  get;  set; }
        public DbSet<FileMemo> FileMemos {  get;  set; }
        public DbSet<VideoTag> VideoTags {  get;  set; }
        public DbSet<Snapshot> Snapshots {  get;  set; }
        public DbSet<DFClass> DFClasses {  get;  set; }
        public DbSet<RedactedVideo> RedactedVideos {  get;  set; }
        public DbSet<Report> Reports {  get;  set; }

        public VMContext()
        {
            Database.CommandTimeout = new int?(120);
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CFG_Account());
            modelBuilder.Configurations.Add(new CFG_DomainConfig());
            modelBuilder.Configurations.Add(new CFG_Department());
            modelBuilder.Configurations.Add(new CFG_Substation());
            modelBuilder.Configurations.Add(new CFG_Manufacturer());
            modelBuilder.Configurations.Add(new CFG_Product());
            modelBuilder.Configurations.Add(new CFG_Inventory());
            modelBuilder.Configurations.Add(new CFG_InventoryLog());
            modelBuilder.Configurations.Add(new CFG_AccountGroup());
            modelBuilder.Configurations.Add(new CFG_UserRank());
            modelBuilder.Configurations.Add(new CFG_RMA());
            modelBuilder.Configurations.Add(new CFG_StateProvince());
            modelBuilder.Configurations.Add(new CFG_Race());
            modelBuilder.Configurations.Add(new CFG_Country());
            modelBuilder.Configurations.Add(new CFG_Classification());
            modelBuilder.Configurations.Add(new CFG_GlobalConfig());
            modelBuilder.Configurations.Add(new CFG_MetaData());
            modelBuilder.Configurations.Add(new CFG_PersonRole());
            modelBuilder.Configurations.Add(new CFG_CameraFolder());
            modelBuilder.Configurations.Add(new CFG_FileType());
            modelBuilder.Configurations.Add(new CFG_FileExt());
            modelBuilder.Configurations.Add(new CFG_AppExt());
            modelBuilder.Configurations.Add(new CFG_AlertEmail());
            modelBuilder.Configurations.Add(new CFG_RightsProfile());
            modelBuilder.Configurations.Add(new CFG_License());
            modelBuilder.Configurations.Add(new CFG_Case());
            modelBuilder.Configurations.Add(new CFG_CaseAddress());
            modelBuilder.Configurations.Add(new CFG_Alias());
            modelBuilder.Configurations.Add(new CFG_CaseMemo());
            modelBuilder.Configurations.Add(new CFG_CaseMetaData());
            modelBuilder.Configurations.Add(new CFG_CaseVehicle());
            modelBuilder.Configurations.Add(new CFG_Incident());
            modelBuilder.Configurations.Add(new CFG_Person());
            modelBuilder.Configurations.Add(new CFG_PersonAddress());
            modelBuilder.Configurations.Add(new CFG_PersonMemo());
            modelBuilder.Configurations.Add(new CFG_PersonVehicle());
            modelBuilder.Configurations.Add(new CFG_Evidence());
            modelBuilder.Configurations.Add(new CFG_SystemLog());
            modelBuilder.Configurations.Add(new CFG_AccountLog());
            modelBuilder.Configurations.Add(new CFG_CameraLog());
            modelBuilder.Configurations.Add(new CFG_DataFile());
            modelBuilder.Configurations.Add(new CFG_FileMemo());
            modelBuilder.Configurations.Add(new CFG_VideoTag());
            modelBuilder.Configurations.Add(new CFG_Snapshot());
            modelBuilder.Configurations.Add(new CFG_DFClass());
            modelBuilder.Configurations.Add(new CFG_RedactedVideo());
            modelBuilder.Configurations.Add(new CFG_Report());
        }
    }
}
