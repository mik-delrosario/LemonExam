using LemonExam.Infrastructure;
using LemonExam.Model.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LemonExam.Features.Category
{
    public class CategoryParam : ICommand<CategoryResponse>
    {
        public string ID { get; set; }
        public string JsonLog { get; set; }

        public string action { get; set; }

        public static bool PropertyExists(dynamic obj, string name)
        {
            if (obj == null) return false;
            if (obj is IDictionary<string, object> dict)
            {
                return dict.ContainsKey(name);
            }
            if (obj is Newtonsoft.Json.Linq.JObject)
            {
                return ((Newtonsoft.Json.Linq.JObject)obj).ContainsKey(name);
            }
            return obj.GetType().GetProperty(name) != null;
        }

        public CategoryEntry convertToModel(bool includeIdentity)
        {
            var cat = new CategoryEntry();
            dynamic _log = Newtonsoft.Json.JsonConvert.DeserializeObject(this.JsonLog);

            if (PropertyExists(_log, "ID"))
            {
                if (includeIdentity)
                {
                    cat.ID = _log.ID;
                }
            }
            if (PropertyExists(_log, "Name"))
            {
                cat.Name = _log.Name;
            }

            return cat;
        }
    }
}
