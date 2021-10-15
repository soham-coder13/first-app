using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkflowManager.Models
{
    public class Approval
    {
        private int id;
        private Author author;
        private string comment;
        private string location;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public Author Author
        {
            get { return author; }
            set { author = value; }
        }

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        public string Location
        {
            get { return location; }
            set { location = value; }
        }
    }
}
