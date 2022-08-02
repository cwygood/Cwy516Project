using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class IdentityServerTokenResult
    {
        public string Access_Token { get; set; }
        public int Expires_In { get; set; }
        public string Token_Type { get; set; }
        public string Scope { get; set; }
    }
}
