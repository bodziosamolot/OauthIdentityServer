using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.MVC.Models
{
    public class ValueViewModel
    {
        public List<string> PublicValues { get; set; }
        public List<string> SecretValues { get; set; }
        public List<string> ManagementValues { get; set; }

        public ValueViewModel(List<string> publicValues, List<string> secretValues, List<string> managementValues)
        {
            PublicValues = publicValues;
            SecretValues = secretValues;
            ManagementValues = managementValues;
        }
    }
}