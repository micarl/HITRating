using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects.DataClasses;

namespace HitRating.RestfulModels
{
    public interface IRestful
    {
        int Create(EntityObject entity);
        bool Edit(int id, EntityObject entity);
        EntityObject Read(int id);
        bool Delete(int id);

        IEnumerable<EntityObject> Search(EntityObject conditions);
    }
}