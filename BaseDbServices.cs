using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trenia2.Models;

namespace Trenia2
{
    public class BaseDbServices
    {

        private BaseDbServices()
        {
            context = new Trenia2Context();
        }

        public static BaseDbServices instance;

        public static BaseDbServices Instance
        {
            get
            {
                if (instance == null)
                    instance = new BaseDbServices();
                return instance;
            }
        }

        public Trenia2Context context;
        public Trenia2Context Context => context;
    }
}
