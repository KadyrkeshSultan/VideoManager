using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using VMModels.Model;

namespace VMInterfaces
{
    public class RPM_VideoTag : IVideoTag, IDisposable
    {
        private Guid AccountId;
        private VMContext context;

        public string Msg {  get;  set; }

        
        public RPM_VideoTag()
        {
            AccountId = Guid.Empty;
            context = new VMContext();
        }

        
        public VideoTag GetTag(Guid Id)
        {
            return context.VideoTags.Find(Id);
        }

        
        public void DeleteVideoTag(VideoTag rec)
        {
            context.VideoTags.Remove(rec);
        }

        
        public List<VideoTag> GetTagList(Guid Id)
        {
            return context.VideoTags.Where(videoTag => videoTag.DataFile.Id == Id).OrderBy(videoTag => videoTag.StartFrame).ToList();
        }

        
        public bool Save()
        {
            bool flag = false;
            using (DbContextTransaction contextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.SaveChanges();
                    contextTransaction.Commit();
                    flag = true;
                }
                catch (DbUpdateException ex)
                {
                    contextTransaction.Rollback();
                    string message = ex.Message;
                    ex.InnerException.ToString();
                }
                catch (DbEntityValidationException ex)
                {
                    contextTransaction.Rollback();
                    foreach (DbEntityValidationResult entityValidationError in ex.EntityValidationErrors)
                    {
                        foreach (DbValidationError validationError in entityValidationError.ValidationErrors)
                            Msg += string.Format("Property: {0} Error: {1}\n", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            return flag;
        }

        
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
