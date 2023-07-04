using MainApi.DataLayer;
using MainApi.DataLayer.Entities;
using System.Collections.Generic;

namespace MainApi.Models
{
    public class OperationViewModel
    {
        public OperationViewModel()
        {

        }

        public int OperationId { get; set; }

        public string OperationName { get; set; }      

        public string AccessId { get; set; }

        public string ActionName { get; set; }

        public List<IdentityAccess> AllAccess { get; set; }

        public List<IdentityOperation> AllOperations { get; set; }
    }    
}