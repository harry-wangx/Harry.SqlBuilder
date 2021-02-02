using System;
using System.Collections.Generic;
using System.Text;

namespace Sample
{
    public sealed class UserModel
    {
        public int ID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public DateTime? JoinTime { get; set; }

        public bool IsAdmin { get; set; }
    }
}
