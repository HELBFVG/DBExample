using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBExample.Model;

public class UserProfile
{
    public Int16 Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public Int16 AccessType { get; set; }
}
