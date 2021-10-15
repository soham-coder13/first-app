using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkflowManager.Models
{
    public class Reason
    {
        private int id;
        private string comment;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }
    }
}
